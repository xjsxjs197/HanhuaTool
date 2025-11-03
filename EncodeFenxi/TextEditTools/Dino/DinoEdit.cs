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
        GET_DATA,     // Generic literal data
        GET_TEXTURE,  // Stripped TIM pixel data
        GET_PALETTE,  // Stripped TIM CLUT (palette)
        GET_SNDH,     // VAG header ('Gian')
        GET_SNDB,     // VAG body
        GET_SNDE,     // Configuration for sound samples
        GET_UNK,      // Unknown type
        GET_LZSS0,    // LZSS-compressed data
        GET_LZSS1     // LZSS-compressed texture
    }

    public struct DC2_ENTRY_GENERIC
    {
        public uint type;      // Entry type (GEntryType)
        public uint size;      // Actual data size
        public uint[] reserve; // Padding (6 words for DC2, 2 words for DC1)
    }

    public struct DC2_ENTRY_GFX
    {
        public uint type;
        public uint size;
        public ushort x, y;    // Framebuffer coordinates
        public ushort w, h;    // Framebuffer dimensions
    }

    public struct GIAN_ENTRY
    {
        public byte reverb;     // Only bit 1<<3 is checked
        public byte unk1;
        public byte unk2;
        public byte sample_note; // Multiply by 8 for actual value
        public ushort note;
        public ushort note_copy; // Unused, same as note>>8
        public ushort reserved;  // Unused
        public ushort adsr1;     // Always 0x80FF
        public ushort adsr2;
        public ushort addr;      // Sample address (multiply by 8)
    }

    public partial class DinoEdit : Form
    {
        public List<DC2_ENTRY_GENERIC> Entries { get; private set; }
        public List<byte[]> Segments { get; private set; }
        public int PackType { get; private set; }

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
            Entries = new List<DC2_ENTRY_GENERIC>();
            Segments = new List<byte[]>();
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

            this.ExtractRaw(@".\");
        }

        private void btnComDat_Click(object sender, EventArgs e)
        {
            this.RepackDat(@".\", @".\DinoTest.dat");
        }

        private void Reset()
        {
            Entries.Clear();
            Segments.Clear();
        }

        private void Open(string filename)
        {
            byte[] data = File.ReadAllBytes(filename);
            Open(data);
        }

        private void Open(byte[] data)
        {
            Reset();

            int entrySize = 16; // Default for DC1
            PackType = Type_DC1;

            // Check if it's DC2 format (reserve fields are zero)
            bool isDC2 = true;
            for (int i = 16; i < 32; i += 4)
            {
                if (BitConverter.ToUInt32(data, i) != 0)
                {
                    isDC2 = false;
                    break;
                }
            }

            if (isDC2)
            {
                PackType = Type_DC2;
                entrySize = 32;
            }

            int pos = 2048; // Start reading after header
            int entryCount = 2048 / entrySize;

            for (int i = 0; i < entryCount; i++)
            {
                int entryOffset = i * entrySize;
                uint type = BitConverter.ToUInt32(data, entryOffset);
                uint size = BitConverter.ToUInt32(data, entryOffset + 4);

                if (type >= (uint)GEntryType.GET_UNK && type != (uint)GEntryType.GET_LZSS0 && type != (uint)GEntryType.GET_LZSS1)
                    break; // Stop if unknown type (except LZSS)

                byte[] buffer = null;
                int alignedSize = Align(size, 2048);

                switch ((GEntryType)type)
                {
                    case GEntryType.GET_TEXTURE:
                        buffer = new byte[alignedSize];
                        Array.Copy(data, pos, buffer, 0, size);
                        UnswizzleGfx(ref buffer, BitConverter.ToUInt16(data, entryOffset + 8), BitConverter.ToUInt16(data, entryOffset + 10));
                        break;

                    case GEntryType.GET_LZSS0:
                        buffer = LzssDec(data, pos, (int)size);
                        break;

                    case GEntryType.GET_LZSS1:
                        byte[] temp = LzssDec(data, pos, (int)size);
                        buffer = new byte[Align((uint)temp.Length, 2048)];
                        Array.Copy(temp, buffer, temp.Length);
                        UnswizzleGfx(ref buffer, BitConverter.ToUInt16(data, entryOffset + 8), BitConverter.ToUInt16(data, entryOffset + 10));
                        break;

                    default:
                        buffer = new byte[size];
                        Array.Copy(data, pos, buffer, 0, size);
                        break;
                }

                var entry = new DC2_ENTRY_GENERIC
                {
                    type = type,
                    size = (uint)buffer.Length,
                    reserve = new uint[isDC2 ? 6 : 2]
                };

                Entries.Add(entry);
                Segments.Add(buffer);
                pos += alignedSize;
            }
        }

        private void ExtractRaw(string outputDir)
        {
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Create XML document (using System.Xml for .NET 3.5)
            XmlDocument xml = new XmlDocument();
            XmlElement root = xml.CreateElement("DinoCrisisPackage");
            root.SetAttribute("type", PackType == Type_DC1 ? "DC1" : "DC2");
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
                    entryElement.SetAttribute("x", BitConverter.ToUInt16(Segments[i], 8).ToString());
                    entryElement.SetAttribute("y", BitConverter.ToUInt16(Segments[i], 10).ToString());
                    entryElement.SetAttribute("w", BitConverter.ToUInt16(Segments[i], 12).ToString());
                    entryElement.SetAttribute("h", BitConverter.ToUInt16(Segments[i], 14).ToString());
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

        private void RepackDat(string inputDir, string outputDatPath)
        {
            // 1. 读取 manifest.xml
            string manifestPath = Path.Combine(inputDir, "manifest.xml");
            if (!File.Exists(manifestPath))
                throw new FileNotFoundException("manifest.xml not found.");

            XmlDocument xml = new XmlDocument();
            xml.Load(manifestPath);

            // 2. 解析 XML
            bool isDC2 = xml.DocumentElement.Attributes["type"].Value == "DC2";
            int entrySize = isDC2 ? 32 : 16;

            XmlNodeList entries = xml.SelectNodes("/DinoCrisisPackage/Entry");
            if (entries == null || entries.Count == 0)
                throw new InvalidDataException("No entries found in manifest.xml.");

            // 3. 处理所有条目
            List<byte[]> entryData = new List<byte[]>();
            List<DC2_ENTRY_GENERIC> entriesInfo = new List<DC2_ENTRY_GENERIC>();

            foreach (XmlNode entry in entries)
            {
                string fileName = entry.InnerText;
                string filePath = Path.Combine(inputDir, fileName);

                if (!File.Exists(filePath))
                    throw new FileNotFoundException(string.Format("File not found: {0}", fileName));

                // 读取文件内容
                byte[] data = File.ReadAllBytes(filePath);
                uint type = uint.Parse(entry.Attributes["type"].Value);

                // 根据类型处理数据
                switch ((GEntryType)type)
                {
                    case GEntryType.GET_LZSS0:
                    case GEntryType.GET_LZSS1:
                        data = LzssEnc(data); // LZSS 压缩
                        break;

                    case GEntryType.GET_TEXTURE:
                    case GEntryType.GET_PALETTE:
                        // 重新 Swizzle 纹理
                        ushort w = ushort.Parse(entry.Attributes["w"].Value);
                        ushort h = ushort.Parse(entry.Attributes["h"].Value);
                        data = SwizzleGfx(data, w, h);
                        break;
                }

                entryData.Add(data);

                // 构建条目信息
                var entryInfo = new DC2_ENTRY_GENERIC
                {
                    type = type,
                    size = (uint)data.Length,
                    reserve = new uint[isDC2 ? 6 : 2]
                };

                // 如果是图形数据，设置坐标（写入 reserve 字段）
                if (type == (uint)GEntryType.GET_TEXTURE ||
                    type == (uint)GEntryType.GET_PALETTE ||
                    type == (uint)GEntryType.GET_LZSS1)
                {
                    // 将坐标信息写入 reserve（如果需要）
                    entryInfo.reserve[0] = ushort.Parse(entry.Attributes["x"].Value);
                    entryInfo.reserve[1] = ushort.Parse(entry.Attributes["y"].Value);
                    entryInfo.reserve[2] = ushort.Parse(entry.Attributes["w"].Value);
                    entryInfo.reserve[3] = ushort.Parse(entry.Attributes["h"].Value);
                }

                entriesInfo.Add(entryInfo);
            }

            // 4. 计算总大小（2048 对齐）
            int totalSize = 2048; // 文件头
            foreach (var data in entryData)
            {
                totalSize += Align((uint)(data.Length), 2048);
            }

            // 5. 写入 .dat 文件
            using (FileStream fs = new FileStream(outputDatPath, FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                // 6. 写入文件头（DC2: 32字节/条目，DC1: 16字节/条目）
                writer.Write(new byte[2048]); // 预留头部

                // 7. 写入所有条目数据
                for (int i = 0; i < entryData.Count; i++)
                {
                    byte[] data = entryData[i];
                    writer.Write(data);

                    // 填充对齐字节
                    int padding = Align((uint)(data.Length), 2048) - data.Length;
                    if (padding > 0)
                    {
                        writer.Write(new byte[padding]);
                    }
                }
            }
        }

        private void UnswizzleGfx(ref byte[] data, ushort width, ushort height)
        {
            // 1. 检查输入数据是否有效
            if (data == null || data.Length == 0)
                throw new ArgumentException("Input data is empty.");

            // 2. 计算反 Swizzle 后所需的总大小（假设每像素 4 字节，如 16bpp TIM）
            int destSize = width * height * 4;
            if (data.Length < destSize)
            {
                // 如果输入不足，扩展数组（保留原始数据）
                byte[] newData = new byte[destSize];
                Array.Copy(data, newData, Math.Min(data.Length, destSize));
                data = newData;
            }

            // 3. 创建临时缓冲区（与原 C++ 逻辑一致）
            byte[] temp = new byte[data.Length];
            Array.Copy(data, temp, data.Length);

            // 4. 反 Swizzle 逻辑（与 C++ 一致）
            int tileWidth = width / 32;
            int blockWidth = tileWidth * 64; // 每行字节数（64 = 32像素 × 2字节/像素）

            for (int y = 0; y < height; y += 32)
            {
                for (int x = 0; x < tileWidth; x++)
                {
                    int srcOffset = (y * blockWidth) + (x * 64);
                    int destOffset = (y * width * 4) + (x * 32 * 4);

                    for (int line = 0; line < 32; line++)
                    {
                        // 确保不会越界
                        if (srcOffset + 64 <= temp.Length && destOffset + 64 <= data.Length)
                        {
                            Array.Copy(temp, srcOffset, data, destOffset, 64);
                        }
                        srcOffset += blockWidth;
                        destOffset += width * 4;
                    }
                }
            }
        }

        private byte[] LzssDec(byte[] src, int offset, int srcSize)
        {
            List<byte> output = new List<byte>(srcSize * 8);
            int flag = 1;
            int end = offset + srcSize;

            while (offset < end)
            {
                if (flag == 1)
                {
                    flag = src[offset++] | 0x100;
                }

                byte ch = src[offset++];

                if ((flag & 1) != 0)
                {
                    output.Add(ch);
                }
                else
                {
                    byte t = src[offset++];
                    int jump = ((t & 0xF) << 8) | ch;
                    int length = (t >> 4) + 2;
                    int back = output.Count - jump;

                    for (int i = 0; i < length; i++)
                    {
                        output.Add(output[back + i]);
                    }
                }

                flag >>= 1;
            }

            return output.ToArray();
        }

        private int Align(uint value, int alignment)
        {
            return (int)((value + alignment - 1) & ~(alignment - 1));
        }

        /// <summary>
        /// LZSS 压缩算法（与 Dino Crisis 2 的压缩格式匹配）
        /// </summary>
        private byte[] LzssEnc(byte[] input)
        {
            // 最大搜索窗口大小（与游戏一致）
            const int WindowSize = 4096;
            // 最大匹配长度
            const int MaxMatchLength = 18;

            using (MemoryStream output = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(output))
            {
                int position = 0;
                int inputLength = input.Length;

                while (position < inputLength)
                {
                    byte flagByte = 0;
                    long flagPosition = output.Position;
                    writer.Write(flagByte); // 预留标志位

                    for (int bit = 0; bit < 8 && position < inputLength; bit++)
                    {
                        int bestMatchLength = 0;
                        int bestMatchOffset = 0;

                        // 查找最长匹配
                        for (int offset = 1; offset <= Math.Min(WindowSize, position); offset++)
                        {
                            int matchLength = 0;
                            while (matchLength < MaxMatchLength &&
                                   position + matchLength < inputLength &&
                                   input[position - offset + matchLength] == input[position + matchLength])
                            {
                                matchLength++;
                            }

                            if (matchLength > bestMatchLength)
                            {
                                bestMatchLength = matchLength;
                                bestMatchOffset = offset;
                            }
                        }

                        // 如果找到足够长的匹配，写入压缩标记
                        if (bestMatchLength >= 3)
                        {
                            // 写入偏移和长度（12-bit offset + 4-bit length）
                            ushort token = (ushort)(((bestMatchOffset - 1) << 4) | (bestMatchLength - 3));
                            writer.Write((byte)(token & 0xFF));
                            writer.Write((byte)(token >> 8));

                            position += bestMatchLength;
                            flagByte |= (byte)(1 << bit); // 标记为压缩块
                        }
                        else
                        {
                            // 直接写入字面量字节
                            writer.Write(input[position++]);
                        }
                    }

                    // 回写标志位
                    long endPosition = output.Position;
                    output.Position = flagPosition;
                    writer.Write(flagByte);
                    output.Position = endPosition;
                }

                return output.ToArray();
            }
        }

        /// <summary>
        /// 将线性纹理数据重新 Swizzle 为 PS1 格式（与 UnswizzleGfx 相反）
        /// </summary>
        private byte[] SwizzleGfx(byte[] linearData, ushort width, ushort height)
        {
            // 1. 检查输入
            if (linearData == null || linearData.Length == 0)
                throw new ArgumentException("Input data is empty.");

            // 2. 计算 Swizzle 后的数据大小（按 32x32 块排列）
            int tileWidth = width / 32;
            int blockWidth = tileWidth * 64; // 每行字节数（64 = 32像素 × 2字节/像素）
            int swizzledSize = height * blockWidth;
            byte[] swizzledData = new byte[swizzledSize];

            // 3. Swizzle 处理
            for (int y = 0; y < height; y += 32)
            {
                for (int x = 0; x < tileWidth; x++)
                {
                    int srcOffset = (y * width * 4) + (x * 32 * 4);
                    int destOffset = (y * blockWidth) + (x * 64);

                    for (int line = 0; line < 32; line++)
                    {
                        if (srcOffset + 64 <= linearData.Length && destOffset + 64 <= swizzledData.Length)
                        {
                            Array.Copy(linearData, srcOffset, swizzledData, destOffset, 64);
                        }
                        srcOffset += width * 4;
                        destOffset += blockWidth;
                    }
                }
            }

            return swizzledData;
        }
    }
}
