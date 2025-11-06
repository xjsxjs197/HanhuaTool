using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace Hanhua.Common.TextEditTools.Dino
{
    public enum GEntryType
    {
        GET_DATA,       // generic literal data
        GET_TEXTURE,    // stripped TIM pixel
        GET_PALETTE,    // stripped TIM clut
        GET_SNDH,       // VAG header 'Gian'
        GET_SNDB,       // VAG body
        GET_SNDE,       // configuration for sound samples?
        GET_UNK,
        GET_LZSS0,
        GET_LZSS1       // compressed texture
    }

    // Generic entry (32 bytes)
    public class DC2_ENTRY_GENERIC
    {
        public uint type;
        public uint size;
        public uint[] reserve = new uint[6];
    }

    // GFX entry
    public class DC2_ENTRY_GFX
    {
        public uint type;
        public uint size;
        public ushort x, y;
        public ushort w, h;
    }

    public partial class DinoEdit : Form
    {
        public List<DC2_ENTRY_GENERIC> Entries = new List<DC2_ENTRY_GENERIC>();
        public List<byte[]> Segments = new List<byte[]>();
        public int PackType;

        public const int Type_DC1 = 0;
        public const int Type_DC2 = 1;

        private readonly string[] TypeExtensions = new string[]
        {
            "bin",  // GET_DATA
            "tex",  // GET_TEXTURE
            "pal",  // GET_PALETTE
            "gnh",  // GET_SNDH
            "gns",  // GET_SNDB
            "gnt",  // GET_SNDE
            "unk",  // GET_UNK
            "bin",  // GET_LZSS0
            "tex"   // GET_LZSS1
        };

        public DinoEdit()
        {
            InitializeComponent();
        }

        private void btnViewDat_Click(object sender, EventArgs e)
        {
            // 打开要分析的文件
            string baseFile = Util.SetOpenDailog("Dino DAT文件（*.dat）|*.dat|所有文件|*.*", string.Empty);
            if (string.IsNullOrEmpty(baseFile))
            {
                return;
            }

            this.Open(baseFile);

            this.ExtractRaw(@"E:\Game\Dino1\DatDecode\");
        }

        public void Reset()
        {
            Entries.Clear();
            Segments.Clear();
        }

        public void Open(string filename)
        {
            byte[] data = File.ReadAllBytes(filename);
            Open(data);
        }

        public void Open(byte[] data)
        {
            Reset();

            int entrySize = 16;
            PackType = Type_DC1;

            // 判断是否是DC2结构（32字节entry）
            uint[] check = new uint[4];
            Buffer.BlockCopy(data, 16, check, 0, 16);
            if (check[0] == 0 && check[1] == 0 && check[2] == 0 && check[3] == 0)
            {
                PackType = Type_DC2;
                entrySize = 32;
            }

            int pos = 2048;
            int si = 2048 / entrySize;
            int i = 0;

            while (true)
            {
                DC2_ENTRY_GENERIC entry = ReadEntry(data, i * entrySize, entrySize);
                if (entry == null) break;

                byte[] buffer = null;
                int ssize = Align((int)entry.size, 2048);
                if (pos + ssize > data.Length) break;

                byte[] segmentData = new byte[entry.size];
                Array.Copy(data, pos, segmentData, 0, entry.size);

                switch ((GEntryType)entry.type)
                {
                    case GEntryType.GET_TEXTURE:
                        {
                            buffer = new byte[ssize];
                            Array.Clear(buffer, 0, ssize);
                            Array.Copy(segmentData, buffer, entry.size);
                            entry.size = (uint)ssize;
                            UnswizzleGfx(buffer, entry);
                        }
                        break;

                    case GEntryType.GET_LZSS0:
                        {
                            byte[] dst;
                            entry.size = Dc2LzssDec(segmentData, out dst);
                            buffer = dst;
                        }
                        break;

                    case GEntryType.GET_LZSS1:
                        {
                            byte[] temp;
                            entry.size = Dc2LzssDec(segmentData, out temp);
                            buffer = new byte[Align((int)entry.size, 2048)];
                            Array.Clear(buffer, 0, buffer.Length);
                            Array.Copy(temp, buffer, entry.size);
                            entry.size = (uint)buffer.Length;
                            UnswizzleGfx(buffer, entry);
                        }
                        break;

                    case GEntryType.GET_DATA:
                    case GEntryType.GET_PALETTE:
                    case GEntryType.GET_SNDH:
                    case GEntryType.GET_SNDB:
                    case GEntryType.GET_SNDE:
                    case GEntryType.GET_UNK:
                        buffer = segmentData;
                        break;

                    default:
                        i = si;
                        break;
                }

                if (i == si) break;

                Entries.Add(entry);
                Segments.Add(buffer);
                pos += ssize;
                i++;
            }
        }

        private DC2_ENTRY_GENERIC ReadEntry(byte[] data, int offset, int size)
        {
            if (offset + size > data.Length) return null;
            DC2_ENTRY_GENERIC e = new DC2_ENTRY_GENERIC();
            e.type = BitConverter.ToUInt32(data, offset);
            e.size = BitConverter.ToUInt32(data, offset + 4);

            int reserveCount = (size - 8) / 4;
            e.reserve = new uint[reserveCount];
            for (int i = 0; i < reserveCount; i++)
            {
                e.reserve[i] = BitConverter.ToUInt32(data, offset + 8 + i * 4);
            }
            return e;
        }

        private int Align(int val, int align)
        {
            return (val + align - 1) / align * align;
        }

        /// <summary>
        /// 解交错图像数据 (unswizzle)
        /// </summary>
        private void UnswizzleGfx(byte[] buf, DC2_ENTRY_GENERIC entry)
        {
            // 解释entry为DC2_ENTRY_GFX
            ushort x = (ushort)(entry.reserve[0] & 0xFFFF);
            ushort y = (ushort)((entry.reserve[0] >> 16) & 0xFFFF);
            ushort w = (ushort)(entry.reserve[1] & 0xFFFF);
            ushort h = (ushort)((entry.reserve[1] >> 16) & 0xFFFF);

            int tw = w / 32;
            int bw = tw * 64;
            byte[] buffer = new byte[entry.size];
            Array.Copy(buf, buffer, entry.size);

            int bIndex = 0;
            for (int yi = 0; yi < h; yi += 32)
            {
                for (int xi = 0; xi < tw; xi++)
                {
                    int scanlineIndex = yi * bw + xi * 64;
                    for (int j = 0; j < 32; j++)
                    {
                        Array.Copy(buffer, bIndex, buf, scanlineIndex, 64);
                        bIndex += 64;
                        scanlineIndex += bw;
                    }
                }
            }
        }

        /// <summary>
        /// LZSS解压
        /// </summary>
        private uint Dc2LzssDec(byte[] src, out byte[] dst)
        {
            int flag = 1;
            dst = new byte[src.Length * 8];
            int srcIndex = 0;
            int dstIndex = 0;

            while (srcIndex < src.Length)
            {
                if (flag == 1)
                {
                    if (srcIndex >= src.Length) break;
                    flag = src[srcIndex++] | 0x100;
                }

                if (srcIndex >= src.Length) break;
                byte ch = src[srcIndex++];

                if ((flag & 1) != 0)
                {
                    dst[dstIndex++] = ch;
                }
                else
                {
                    if (srcIndex >= src.Length) break;
                    byte t = src[srcIndex++];

                    int jump = ((t & 0xF) << 8) | ch;
                    int size = (t >> 4) + 2;

                    int srcDecIndex = dstIndex - jump;
                    for (int i = 0; i < size; i++)
                    {
                        if (srcDecIndex < 0 || srcDecIndex >= dst.Length) break;
                        dst[dstIndex++] = dst[srcDecIndex++];
                    }
                }
                flag >>= 1;
            }

            Array.Resize(ref dst, dstIndex);
            return (uint)dstIndex;
        }

        /// <summary>
        /// 导出所有资源为文件（和原C++ ExtractRaw类似，但简化）
        /// </summary>
        private void ExtractRaw(string outputDir)
        {
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Create XML document (using System.Xml for .NET 3.5)
            XmlDocument xml = new XmlDocument();
            XmlElement root = xml.CreateElement("DinoCrisisPackage");
            root.SetAttribute("type", PackType == Type_DC1 ? "0" : "1");
            xml.AppendChild(root);

            for (int i = 0; i < Entries.Count; i++)
            {
                string fileName = string.Format("{0:D2}.{1}", i, TypeExtensions[Entries[i].type]);
                string filePath = Path.Combine(outputDir, fileName);
                File.WriteAllBytes(filePath, Segments[i]);

                XmlElement entryElement = xml.CreateElement("Entry");
                entryElement.InnerText = fileName;
                entryElement.SetAttribute("type", Entries[i].type.ToString());

                if (Entries[i].type == (uint)GEntryType.GET_TEXTURE ||
                    Entries[i].type == (uint)GEntryType.GET_PALETTE ||
                    Entries[i].type == (uint)GEntryType.GET_LZSS1)
                {
                    // 解释entry为DC2_ENTRY_GFX
                    ushort x = (ushort)(Entries[i].reserve[0] & 0xFFFF);
                    ushort y = (ushort)((Entries[i].reserve[0] >> 16) & 0xFFFF);
                    ushort w = (ushort)(Entries[i].reserve[1] & 0xFFFF);
                    ushort h = (ushort)((Entries[i].reserve[1] >> 16) & 0xFFFF);

                    entryElement.SetAttribute("x", x.ToString());
                    entryElement.SetAttribute("y", y.ToString());
                    entryElement.SetAttribute("w", w.ToString());
                    entryElement.SetAttribute("h", h.ToString());
                }
                else
                {
                    entryElement.SetAttribute("address", string.Format("0x{0:X8}", Entries[i].reserve[0]));
                    entryElement.SetAttribute("size", Entries[i].size.ToString());
                }

                root.AppendChild(entryElement);
            }

            xml.Save(Path.Combine(outputDir, "manifest.xml"));
        }

        private void btnComDat_Click(object sender, EventArgs e)
        {
            // 选择已解压文件的目录
            string folder = @"G:\Study\MySelfProject\Hanhua\Dino2\DatDeCom";
            if (string.IsNullOrEmpty(folder)) return;

            // 获取 XML 索引
            string xmlPath = Path.Combine(folder, "package.xml");
            if (!File.Exists(xmlPath))
            {
                MessageBox.Show("未找到 package.xml 文件！");
                return;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNodeList nodes = doc.SelectNodes("/DinoCrisisPackage/Entry");

            string outputDat = Path.Combine(folder, "DinoRepacked.dat");
            using (FileStream fs = new FileStream(outputDat, FileMode.Create, FileAccess.Write))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                // 写 2048 字节头
                byte[] header = new byte[2048];
                bw.Write(header, 0, header.Length);

                int dataOffset = 2048;
                List<DC2_ENTRY_GENERIC> newEntries = new List<DC2_ENTRY_GENERIC>();

                foreach (XmlNode node in nodes)
                {
                    int index = int.Parse(node.Attributes["index"].Value);
                    int type = int.Parse(node.Attributes["type"].Value);
                    string file = node.Attributes["file"].Value;

                    string path = Path.Combine(folder, file);
                    if (!File.Exists(path)) continue;

                    byte[] rawData = File.ReadAllBytes(path);
                    byte[] outputData = null;

                    DC2_ENTRY_GENERIC entry = new DC2_ENTRY_GENERIC();
                    entry.type = (uint)type;
                    entry.size = (uint)rawData.Length;
                    entry.reserve = new uint[6]; // 可根据需要填充 reserve

                    switch ((GEntryType)type)
                    {
                        case GEntryType.GET_LZSS0:
                            outputData = Dc2LzssEnc(rawData);
                            outputData = AlignData(outputData, 2048);
                            break;

                        case GEntryType.GET_LZSS1:
                            SwizzleGfx(rawData, entry);
                            outputData = Dc2LzssEnc(rawData);
                            outputData = AlignData(outputData, 2048);
                            break;

                        case GEntryType.GET_TEXTURE:
                            SwizzleGfx(rawData, entry);
                            outputData = AlignData(rawData, 2048);
                            break;

                        default:
                            outputData = AlignData(rawData, 2048);
                            break;
                    }

                    // 写入数据段
                    bw.Seek(dataOffset, SeekOrigin.Begin);
                    bw.Write(outputData, 0, outputData.Length);

                    newEntries.Add(entry);
                    dataOffset += outputData.Length;
                }

                // 回写条目表
                bw.Seek(0, SeekOrigin.Begin); // 假设条目表从文件头开始
                foreach (DC2_ENTRY_GENERIC entry in newEntries)
                {
                    bw.Write(entry.type);
                    bw.Write(entry.size);
                    if (PackType == Type_DC2)
                    {
                        for (int i = 0; i < 6; i++) bw.Write(entry.reserve[i]);
                    }
                }
            }

            MessageBox.Show("打包完成: " + outputDat);
        }

        /// <summary>
        /// LZSS 压缩 (bit-level 与 Capcom 完全一致)
        /// </summary>
        private byte[] Dc2LzssEnc(byte[] src)
        {
            List<byte> dst = new List<byte>();
            int srcIndex = 0;
            int flag = 0;
            int flagBit = 0;
            int flagPos = 0;

            dst.Add(0); // flag 占位
            flagPos = dst.Count - 1;

            while (srcIndex < src.Length)
            {
                int matchOffset = 0;
                int matchLength = 0;

                int startSearch = Math.Max(0, srcIndex - 0xFFF);
                for (int i = startSearch; i < srcIndex; i++)
                {
                    int length = 0;
                    while (length < 17 && srcIndex + length < src.Length && src[i + length] == src[srcIndex + length])
                        length++;
                    if (length > matchLength && length >= 2)
                    {
                        matchLength = length;
                        matchOffset = srcIndex - i;
                    }
                }

                if (matchLength >= 2)
                {
                    int b1 = matchOffset & 0xFF;
                    int b2 = ((matchLength - 2) << 4) | ((matchOffset >> 8) & 0xF);
                    dst.Add((byte)b1);
                    dst.Add((byte)b2);
                    flag |= 0 << flagBit;
                    srcIndex += matchLength;
                }
                else
                {
                    dst.Add(src[srcIndex++]);
                    flag |= 1 << flagBit;
                }

                flagBit++;
                if (flagBit == 8)
                {
                    dst[flagPos] = (byte)flag;
                    flag = 0;
                    flagBit = 0;
                    dst.Add(0);
                    flagPos = dst.Count - 1;
                }
            }

            if (flagBit > 0)
                dst[flagPos] = (byte)flag;

            return dst.ToArray();
        }

        /// <summary>
        /// 将解交错图像重新交错回原始布局 (swizzle)
        /// </summary>
        private void SwizzleGfx(byte[] buf, DC2_ENTRY_GENERIC entry)
        {
            ushort x = (ushort)(entry.reserve[0] & 0xFFFF);
            ushort y = (ushort)((entry.reserve[0] >> 16) & 0xFFFF);
            ushort w = (ushort)(entry.reserve[1] & 0xFFFF);
            ushort h = (ushort)((entry.reserve[1] >> 16) & 0xFFFF);

            int tw = w / 32;
            int bw = tw * 64;
            byte[] temp = new byte[buf.Length];
            Array.Copy(buf, temp, buf.Length);

            int bIndex = 0;
            for (int yi = 0; yi < h; yi += 32)
            {
                for (int xi = 0; xi < tw; xi++)
                {
                    int scanlineIndex = yi * bw + xi * 64;
                    for (int j = 0; j < 32; j++)
                    {
                        Array.Copy(temp, scanlineIndex, buf, bIndex, 64);
                        bIndex += 64;
                        scanlineIndex += bw;
                    }
                }
            }
        }

        /// <summary>
        /// 数据按指定对齐字节数扩展
        /// </summary>
        private byte[] AlignData(byte[] data, int align)
        {
            int size = Align(data.Length, align);
            if (data.Length < size)
                Array.Resize(ref data, size);
            return data;
        }

    }
}
