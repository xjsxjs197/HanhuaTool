using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Linq;
using System.Reflection;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace Hanhua.Common.TextEditTools.Dino
{
    public partial class DinoEdit : Form
    {
        public List<DC2_ENTRY_GENERIC> Entries = new List<DC2_ENTRY_GENERIC>();
        public List<byte[]> Segments = new List<byte[]>();
        public int PackType;

        public const int Type_DC1 = 0;
        public const int Type_DC2 = 1;

        private string[] coreFontChar1 = { 
            "　", "１", "３", "５", "７", "９", "Ｂ", "Ｄ", "Ｆ", "Ｈ", "Ｊ", "Ｌ", "Ｎ", "Ｐ", "Ｒ", "Ｔ",
            "Ｖ", "Ｘ", "Ｚ", "ｍ", "ｓ", "い", "え", "か", "く", "こ", "し", "せ", "た", "つ", "と", "に",
            "ね", "は", "ふ", "ほ", "み", "め", "や", "よ", "り", "れ", "わ", "ん", "ぎ", "げ", "ざ", "ず",
            "ぞ", "ぢ", "で", "ば", "ぶ", "ぼ", "ぴ", "ぺ", "ゃ", "ょ", "ア", "ウ", "オ", "キ", "ケ", "サ",
            "ス", "ソ", "チ", "テ", "ナ", "ヌ", "ノ", "ヒ", "ヘ", "マ", "ム", "モ", "ユ", "ラ", "ル", "ロ",
            "ヲ", "ガ", "グ", "ゴ", "ジ", "ゼ", "ダ", "ヅ", "ド", "ビ", "ベ", "パ", "プ", "ポ", "ュ", "ッ",
            "ー", "…", "。", "？", "．", "＆", "／", "》", "製", "型", "攻", "力", "用", "弾", "破", "付",
            "着", "射", "定", "ィ", "上", "中", "出", "ェ", "敵", "与", "部", "散", "持", "酔", "一", "的",
            "物", "効", "発", "要", "猛", "死", "頭", "準", "専", "爆", "炎", "限", "特", "参", "続", "血",
            "体", "復", "抑", "果", "完", "蘇", "状", "素", "調", "品", "面", "作", "成", "増", "緊", "赤",
            "黄", "集", "古", "予", "電", "地", "箱", "身", "明", "設", "異", "際", "仕", "通", "員", "資",
            "研", "Ⅰ", "柄", "刻", "解", "念", "起", "供", "容", "今", "紋", "取", "現", "保", "個", "理",
            "自", "学", "搬", "料", "手", "‟", "固", "応", "任", "博", "般", "機", "行", "重", "他", "実",
            "護", "構", "原", "示", "納", "核", "字", "何", "受", "場", "色", "白", "計", "対", "暗", "認",
            "表", "屋", "前", "戦", "議", "庫", "庭", "路", "点", "医", "休", "過", "可", "運", "階", "島",
            "選", "差", "空", "決", "振", "＋", "補", "押", "操"
        };

        private string[] coreFontChar2 = { 
            "０", "２", "４", "６", "８", "Ａ", "Ｃ", "Ｅ", "Ｇ", "Ｉ", "Ｋ", "Ｍ", "Ｏ", "Ｑ", "Ｓ", "Ｕ",
            "Ｗ", "Ｙ", "ｅ", "o", "あ", "う", "お", "き", "け", "さ", "す", "そ", "ち", "て", "な", "ぬ",
            "の", "ひ", "へ", "ま", "む", "も", "ゆ", "ら", "る", "ろ", "を", "が", "ぐ", "ご", "じ", "ぜ",
            "だ", "づ", "ど", "び", "べ", "ぱ", "ぷ", "ぽ", "ゅ", "っ", "イ", "エ", "カ", "ク", "コ", "シ",
            "セ", "タ", "ツ", "ト", "ニ", "ネ", "ハ", "フ", "ホ", "ミ", "メ", "ヤ", "ヨ", "リ", "レ", "ワ",
            "ン", "ギ", "ゲ", "ザ", "ズ", "ゾ", "ヂ", "デ", "バ", "ブ", "ボ", "ピ", "ペ", "ャ", "ョ", "ヴ",
            "▼", "、", "！", "：", "・", "×", "《", "社", "小", "改", "撃", "軍", "長", "丸", "壊", "装",
            "連", "安", "使", "率", "命", "高", "強", "ォ", "大", "造", "分", "広", "麻", "弱", "時", "生",
            "眠", "数", "必", "間", "毒", "至", "標", "（", "）", "火", "無", "制", "殊", "最", "止", "剤",
            "回", "失", "痛", "少", "全", "薬", "態", "材", "合", "画", "新", "系", "化", "殖", "急", "緑",
            "開", "室", "記", "備", "源", "救", "人", "証", "施", "事", "動", "書", "組", "信", "置", "格",
            "究", "図", "号", "子", "除", "Ⅱ", "充", "給", "量", "指", "採", "録", "在", "存", "管", "入",
            "御", "質", "形", "換", "”", "常", "V", "責", "者", "士", "港", "密", "厳", "多", "Ⅲ", "験",
            "—", "守", "内", "収", "性", "違", "印", "器", "居", "所", "下", "文", "兵", "同", "確", "武",
            "見", "外", "廊", "会", "修", "裏", "絡", "配", "検", "務", "憩", "許", "私", "央", "段", "水",
            "抜", "直", "残", "照", "整", "向", "横", "枚", "以"
        };

        private string[] st8210AChar = {
             "", "風", "口", "先", "項", "張", "『", "未", "来", "』"
        };
        private string[] st8210BChar = {
             "　", "惨",
            "年", "夜",
            "試", "暴",
            "走", "故",
            "名", "瞬",
            "悲", "劇",
            "繰", "返",
            "警", "戒",
            "勢", "幅",
            "近", "端",
            "末", "悪",
            "偽", "件",
            "本", "日",
            "処", "策",
            "講", "気",
            "後", "退",
            "忘", "登",
            "番", "属",
            "線", "話",
            "呼", "誰",
            "留", "類",
            "未", "提",
            "＜", "旧",
            "＞", "別",
            "航", "週",
            "害", "虫",
            "駆", "机",
            "関", "係",
            "並", "職",
            "項", "ァ",
            "読", "索",
            "方", "式"
        };

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

            string fileName = Util.GetShortNameWithoutType(baseFile);
            //this.ExtractRaw(@"G:\Study\MySelfProject\Hanhua\Dino1\Ps_IsoCn\PSX\DATA_Test\" + fileName);
            this.ExtractRaw(@"G:\Study\MySelfProject\Hanhua\Dino1\Pc\" + fileName);
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
            string folder = Util.OpenFolder(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn");

            this.CompressDatFile(folder.ToUpper());

            MessageBox.Show("打包完成");
        }

        private void CompressDatFile(string folder)
        {
            // 获取 XML 索引
            string xmlPath = Path.Combine(folder, "manifest.xml");
            if (!File.Exists(xmlPath))
            {
                MessageBox.Show("未找到 manifest.xml 文件！");
                return;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNodeList nodes = doc.SelectNodes("/DinoCrisisPackage/Entry");

            string outputDat = folder + ".dat";
            using (FileStream fs = new FileStream(outputDat, FileMode.Create, FileAccess.Write))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                // 写 2048 字节头
                byte[] header = new byte[2048];
                byte[] byDummyHeader = Encoding.ASCII.GetBytes("dummy header    ");
                for (int i = 0; i < 2048 / 16; i++)
                {
                    Array.Copy(byDummyHeader, 0, header, i * 16, byDummyHeader.Length);
                }
                bw.Write(header, 0, header.Length);

                int dataOffset = 2048;
                List<DC2_ENTRY_GENERIC> newEntries = new List<DC2_ENTRY_GENERIC>();

                foreach (XmlNode node in nodes)
                {
                    int type = int.Parse(node.Attributes["type"].Value);
                    string file = node.InnerText.Trim();

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
                            entry.size = (uint)outputData.Length;
                            outputData = AlignData(outputData, 2048);
                            entry.reserve[0] = uint.Parse(node.Attributes["address"].Value.Replace("0x", ""), NumberStyles.HexNumber);
                            break;

                        case GEntryType.GET_LZSS1:
                            entry.reserve[0] = (uint.Parse(node.Attributes["y"].Value) << 16) | uint.Parse(node.Attributes["x"].Value);
                            entry.reserve[1] = (uint.Parse(node.Attributes["h"].Value) << 16) | uint.Parse(node.Attributes["w"].Value);
                            SwizzleGfx(rawData, entry);
                            outputData = Dc2LzssEnc(rawData);
                            entry.size = (uint)outputData.Length;
                            outputData = AlignData(outputData, 2048);
                            break;

                        case GEntryType.GET_TEXTURE:
                            entry.reserve[0] = (uint.Parse(node.Attributes["y"].Value) << 16) | uint.Parse(node.Attributes["x"].Value);
                            entry.reserve[1] = (uint.Parse(node.Attributes["h"].Value) << 16) | uint.Parse(node.Attributes["w"].Value);
                            SwizzleGfx(rawData, entry);
                            outputData = AlignData(rawData, 2048);
                            break;

                        case GEntryType.GET_PALETTE:
                            outputData = AlignData(rawData, 2048);
                            entry.reserve[0] = (uint.Parse(node.Attributes["y"].Value) << 16) | uint.Parse(node.Attributes["x"].Value);
                            entry.reserve[1] = (uint.Parse(node.Attributes["h"].Value) << 16) | uint.Parse(node.Attributes["w"].Value);
                            break;

                        default:
                            outputData = AlignData(rawData, 2048);
                            entry.reserve[0] = uint.Parse(node.Attributes["address"].Value.Replace("0x", ""), NumberStyles.HexNumber);
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
                    bw.Write(entry.reserve[0]);
                    bw.Write(entry.reserve[1]);
                }
            }

            byte[] byJpDat = File.ReadAllBytes(outputDat.Replace("PS_CN", @"mkpsxiso-2.20-win64\DinoJpBak\PSX\DATA"));
            byte[] byCnDat = File.ReadAllBytes(outputDat);

            for (int i = 0; i < 2048; i += 16)
            {
                if (byCnDat[i] == 0x64 && byCnDat[i + 1] == 0x75 && byCnDat[i + 2] == 0x6D && byCnDat[i + 3] == 0x6D)
                {
                    break;
                }

                GEntryType type = (GEntryType)byJpDat[i];
                if (!(type == GEntryType.GET_LZSS1 || type == GEntryType.GET_TEXTURE || type == GEntryType.GET_PALETTE))
                {
                    byCnDat[i + 15] = byJpDat[i + 15];
                    byCnDat[i + 14] = byJpDat[i + 14];
                    byCnDat[i + 13] = byJpDat[i + 13];
                    byCnDat[i + 12] = byJpDat[i + 12];
                }
            }

            File.WriteAllBytes(outputDat, byCnDat);
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

        private void btnSearchTxt_Click(object sender, EventArgs e)
        {
            string chkTxt = this.txtChk.Text.Trim();
            if (string.IsNullOrEmpty(chkTxt))
            {
                MessageBox.Show("请输入查找的文本");
                return;
            }

            //List<string> char80 = new List<string>();
            //List<string> char81 = new List<string>();
            //int charIdx = 0;
            //int charIdx1 = 0;
            //while (charIdx1 < (coreFontChar1.Length + coreFontChar2.Length))
            //{
            //    if (charIdx1 < 255)
            //    {
            //        char80.Add(coreFontChar1[charIdx]);
            //        char80.Add(coreFontChar2[charIdx]);
            //        charIdx1 += 2;
            //        charIdx++;
            //    }
            //    else
            //    {
            //        char81.Add(coreFontChar1[charIdx]);
            //        char81.Add(coreFontChar2[charIdx]);
            //        charIdx1 += 2;
            //        charIdx++;
            //    }
            //}
            Dictionary<string, List<string>> cnFontChar = new Dictionary<string, List<string>>();
            string[] allCnFontChar = File.ReadAllLines(@"G:\Study\MySelfProject\Hanhua\Dino1\dino1FontChar_cn.txt", Encoding.UTF8);
            for (int i = 0; i < allCnFontChar.Length; i += 2)
            {
                List<string> curCnChars = new List<string>();
                curCnChars.AddRange(allCnFontChar[i + 1].Split(','));
                cnFontChar.Add(allCnFontChar[i].ToUpper(), curCnChars);
            }

            List<string> comnCnFontChars = cnFontChar["SLPS_021.80"];
            List<string> char80 = new List<string>();
            List<string> char81 = new List<string>();
            for (int j = 0; j < comnCnFontChars.Count; j++)
            {
                if (char80.Count < 256)
                {
                    char80.Add(comnCnFontChars[j]);
                }
                else
                {
                    char81.Add(comnCnFontChars[j]);
                }
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < chkTxt.Length; i++)
            {
                string curChar = chkTxt.Substring(i, 1);
                if (char80.Contains(curChar))
                {
                    sb.Append(char80.IndexOf(curChar).ToString("X2")).Append(" 80 ");
                }
                else if (char81.Contains(curChar))
                {
                    sb.Append(char81.IndexOf(curChar).ToString("X2")).Append(" 81 ");
                }
                else
                {
                    sb.Append(999).Append(" ");
                }
            }

            this.txtChk.Text = sb.ToString();
        }

        private string DecodeDinoText(byte[] binByte, int startPos, int endPos, List<string> char80, List<string> char81, List<string> char82, StringBuilder sb2)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = startPos; i < endPos; i += 2)
            {
                if (binByte[i] == 0x00 && binByte[i + 1] == 0xA0)
                {
                    sb.Append("^00A0^\r\n");
                }
                else if (binByte[i] == 0x00 && binByte[i + 1] == 0xC0)
                {
                    sb.Append("^00C0^\r\n");
                }
                else if (binByte[i] == 0x00 && binByte[i + 1] == 0x90)
                {
                    sb.Append("^0090^\r\n");
                }
                else if (binByte[i + 1] == 0x80)
                {
                    sb.Append(char80[binByte[i]]);
                }
                else if (binByte[i + 1] == 0x81)
                {
                    if (binByte[i] < char81.Count)
                    {
                        sb.Append(char81[binByte[i]]);
                    }
                    else
                    {
                        sb2.Append("^").Append(binByte[i].ToString("X2")).Append(" 81").Append("^");
                        sb.Append("^").Append(binByte[i].ToString("X2")).Append(" 81").Append("^");
                    }
                }
                else if (binByte[i + 1] == 0x82)
                {
                    if (binByte[i] < char82.Count)
                    {
                        sb.Append(char82[binByte[i]]);
                    }
                    else
                    {
                        sb2.Append("^").Append(binByte[i].ToString("X2")).Append(" 82").Append("^");
                        //sb.Append("^").Append(binByte[i].ToString("X2")).Append(" 82").Append("^");
                        sb.Append("　");
                    }
                }
                else
                {
                    sb.Append("^").Append(binByte[i].ToString("X2")).Append(binByte[i + 1].ToString("X2")).Append("^");
                }
            }

            return sb.ToString();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            //StringBuilder sb = new StringBuilder();
            //XmlDocument doc = new XmlDocument();
            //List<FilePosInfo> allPic = Util.GetAllFiles(@"G:\Study\MySelfProject\Hanhua\Dino1\Dino1FontPic\FontPic\jp0");
            //foreach (FilePosInfo file in allPic)
            //{
            //    string folderName = Util.GetShortName(file.File).Replace("_00.png", "").Replace("_01.png", "").ToUpper();
            //    string folder = @"G:\Study\MySelfProject\Hanhua\Dino1\PS_jp\" + folderName;
            //    if (Directory.Exists(folder))
            //    {
            //        string xmlPath = Path.Combine(folder, "manifest.xml");
            //        if (!File.Exists(xmlPath))
            //        {
            //            continue;
            //        }

            //        Image img = Bitmap.FromFile(file.File);

            //        doc.Load(xmlPath);
            //        XmlNodeList nodes = doc.SelectNodes("/DinoCrisisPackage/Entry");
            //        foreach (XmlNode node in nodes)
            //        {
            //            int type = int.Parse(node.Attributes["type"].Value);
            //            if ((GEntryType)type == GEntryType.GET_LZSS1 || (GEntryType)type == GEntryType.GET_TEXTURE)
            //            {
            //                string path = Path.Combine(folder, node.InnerText.Trim());
            //                if (!File.Exists(path)) continue;

            //                if (uint.Parse(node.Attributes["h"].Value) == img.Height)
            //                {
            //                    sb.Append(folderName).Append("\r\n");
            //                    sb.Append(path).Append("\r\n");
            //                    break;
            //                }
            //            }
            //        }
            //    }
            //}

            //File.WriteAllText(@"G:\Study\MySelfProject\Hanhua\Dino1\fontImgInfo.txt", sb.ToString(), Encoding.UTF8);

            Dictionary<string, List<string>> cnFontChar = new Dictionary<string, List<string>>();

            List<string> char10a = new List<string>();
            List<string> char10b = new List<string>();
            char10a.AddRange(st8210AChar);
            char10b.AddRange(st8210BChar);
            cnFontChar.Add("st10a_00", char10a);
            cnFontChar.Add("st10b_00", char10b);
            cnFontChar.Add("ST1", char10b);

            //string[] allCnFontChar = File.ReadAllLines(@"G:\Study\MySelfProject\Hanhua\Dino1\dino1FontChar_cn.txt", Encoding.UTF8);
            string[] allCnFontChar = File.ReadAllLines(@"G:\Study\MySelfProject\Hanhua\Dino1\dino1FontChar.txt", Encoding.UTF8);
            for (int i = 0; i < allCnFontChar.Length; i += 2)
            {
                List<string> curCnChars = new List<string>();
                curCnChars.AddRange(allCnFontChar[i + 1].Split(','));
                string key = allCnFontChar[i].ToUpper();
                cnFontChar.Add(key, curCnChars);
                if (key.StartsWith("ST10B"))
                {
                    cnFontChar.Add("ST1", curCnChars);
                }
                else if (key.StartsWith("ST506"))
                {
                    cnFontChar.Add("ST5", curCnChars);
                }
                else if (key.StartsWith("ST806"))
                {
                    cnFontChar.Add("ST8", curCnChars);
                }
            }

            //List<string> comnCnFontChars = cnFontChar["SLPS_021.80"];
            //List<string> cnFont80 = new List<string>();
            //List<string> cnFont81 = new List<string>();
            //for (int j = 0; j < comnCnFontChars.Count; j += 2)
            //{
            //    cnFont80.Add(comnCnFontChars[j]);
            //    if (j < comnCnFontChars.Count - 1)
            //    {
            //        cnFont81.Add(comnCnFontChars[j + 1]);
            //    }
            //}

            List<string> cnFont80 = new List<string>();
            List<string> cnFont81 = new List<string>();
            int charIdx = 0;
            int charIdx1 = 0;
            while (charIdx1 < (coreFontChar1.Length + coreFontChar2.Length))
            {
                if (charIdx1 < 255)
                {
                    cnFont80.Add(coreFontChar1[charIdx]);
                    cnFont80.Add(coreFontChar2[charIdx]);
                    charIdx1 += 2;
                    charIdx++;
                }
                else
                {
                    cnFont81.Add(coreFontChar1[charIdx]);
                    cnFont81.Add(coreFontChar2[charIdx]);
                    charIdx1 += 2;
                    charIdx++;
                }
            }

            string[] allJpTxtFiles = File.ReadAllLines(@"G:\Study\MySelfProject\Hanhua\Dino1\textAddr.txt");
            StringBuilder sb = new StringBuilder();

            Microsoft.Office.Interop.Excel.Application xApp = null;
            Microsoft.Office.Interop.Excel.Workbook xBook = null;
            Microsoft.Office.Interop.Excel.Worksheet xSheet = null;

            try
            {
                // 创建Application对象 
                xApp = new Microsoft.Office.Interop.Excel.ApplicationClass();
                //xApp.Visible = true;

                // 追加一个WorkBook
                xBook = xApp.Workbooks.Add(Missing.Value);

                for (int i = 0; i < allJpTxtFiles.Length; i += 2)
                {
                    //byte[] byTxt = File.ReadAllBytes(allJpTxtFiles[i].Replace("PS_jp", "PS_cn"));
                    byte[] byTxt = File.ReadAllBytes(allJpTxtFiles[i]);
                    string[] posInfos = allJpTxtFiles[i + 1].Split(' ');
                    int endPos = byTxt.Length;
                    int startPos = Convert.ToInt32(posInfos[0], 16);
                    if (posInfos.Length > 1)
                    {
                        endPos = Convert.ToInt32(posInfos[1], 16);
                    }

                    //if (allJpTxtFiles[i].EndsWith("SLPS_021.80", StringComparison.OrdinalIgnoreCase))
                    if (posInfos.Length > 2)
                    {
                        string[] fileNames = allJpTxtFiles[i].Split('\\');
                        string shortName = fileNames[fileNames.Length - 2];

                        xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                        xSheet.Name = shortName;
                        int lineIdx = 1;
                        List<string> allComText = new List<string>();

                        for (int j = startPos; j < endPos - 4; j += 4)
                        {
                            int txtStartPos = endPos + ((byTxt[j] << 0) | (byTxt[j + 1] << 8) | (byTxt[j + 2] << 16) | (byTxt[j + 3] << 24)) * 2;
                            int txtEndPos = endPos + ((byTxt[j + 4] << 0) | (byTxt[j + 5] << 8) | (byTxt[j + 6] << 16) | (byTxt[j + 7] << 24)) * 2;
                            allComText.AddRange(this.DecodeDinoText(byTxt, txtStartPos, txtEndPos, cnFont80, cnFont81, cnFontChar[shortName.ToUpper()], sb).Split('\n'));
                        }

                        int txtStartPos1 = endPos + ((byTxt[endPos - 4] << 0) | (byTxt[endPos - 3] << 8) | (byTxt[endPos - 2] << 16) | (byTxt[endPos - 1] << 24)) * 2;
                        int txtEndPos1 = Convert.ToInt32(posInfos[2], 16);
                        allComText.AddRange(this.DecodeDinoText(byTxt, txtStartPos1, txtEndPos1, cnFont80, cnFont81, cnFontChar[shortName.ToUpper()], sb).Split('\n'));

                        for (int j = 0; j < allComText.Count; j++)
                        {
                            string curLine = allComText[j].Replace("\r", "");
                            if (!string.IsNullOrEmpty(curLine))
                            {
                                Microsoft.Office.Interop.Excel.Range rngCn = xSheet.get_Range("A" + (lineIdx++), Missing.Value);
                                rngCn.Value2 = curLine;
                            }
                        }
                    }
                    else
                    {
                        string[] fileNames = allJpTxtFiles[i].Split('\\');
                        string shortName = fileNames[fileNames.Length - 2];
                        if (cnFontChar.ContainsKey(shortName.ToUpper()))
                        {
                            sb.Append(shortName).Append(" ");
                            string[] allTxtLines = this.DecodeDinoText(byTxt, startPos, endPos, cnFont80, cnFont81, cnFontChar[shortName.ToUpper()], sb).Split('\n');

                            xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                            xSheet.Name = shortName;

                            for (int j = 0; j < allTxtLines.Length; j++)
                            {

                                Microsoft.Office.Interop.Excel.Range rngCn = xSheet.get_Range("A" + (j + 1), Missing.Value);
                                rngCn.Value2 = allTxtLines[j].Replace("\r", "");
                            }
                            sb.Append("\r\n");
                        }
                        else
                        {
                            sb.Append(shortName).Append("\r\n");
                        }
                    }
                }

                // 保存
                xSheet.SaveAs(
                    @"G:\Study\MySelfProject\Hanhua\Dino1\allCnText4.xlsx",
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                // 显示保存完成信息
                MessageBox.Show("导出完成！");

            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
            }
            finally
            {
                // 清空各种对象
                xSheet = null;
                xBook = null;
                if (xApp != null)
                {
                    xApp.Quit();
                    xApp = null;
                }
            }
            
        }

        private void btnDecAllDat_Click(object sender, EventArgs e)
        {
            List<FilePosInfo> allDat = Util.GetAllFiles(@"G:\Study\MySelfProject\Hanhua\Dino1\Ps_IsoCn\PSX\DATA").Where(p => !p.IsFolder && p.File.EndsWith(".dat", StringComparison.OrdinalIgnoreCase)).ToList();
            foreach (FilePosInfo datFile in allDat)
            {
                try
                {
                    this.Open(datFile.File);

                    string folder = @"G:\Study\MySelfProject\Hanhua\Dino1\Ps_IsoCn\PSX\DATA_Test\" + Util.GetShortNameWithoutType(datFile.File);
                    Directory.CreateDirectory(folder);

                    this.ExtractRaw(folder);
                }
                catch (Exception exp)
                { 
                }
            }

            MessageBox.Show("OK");
        }

        private void btnSearTextAddr_Click(object sender, EventArgs e)
        {
            List<FilePosInfo> allTextBin = Util.GetAllFiles(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_jp").Where(p => !p.IsFolder && p.File.EndsWith(".bin", StringComparison.OrdinalIgnoreCase)).ToList();
            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            foreach (FilePosInfo binFile in allTextBin)
            {
                try
                {
                    byte[] byBin = File.ReadAllBytes(binFile.File);
                    int textPos = byBin.Length - 1;
                    bool hasEndFlg = false;
                    bool hasBlankFlg = false;
                    bool hasText = false;
                    while (textPos > 0 )
                    {
                        if (byBin[textPos - 3] == 0x04 && byBin[textPos - 2] == 0x00 && byBin[textPos - 1] == 0x00 && byBin[textPos] == 0x00)
                        {
                            if ((textPos - 3) != 0 && textPos != (byBin.Length - 1) && hasBlankFlg && hasEndFlg)
                            {
                                sb.Append((binFile.File)).Append("\r\n");
                                sb.Append((textPos - 3).ToString("X")).Append("\r\n");
                                hasText = true;
                            }
                            break;

                        }
                        else
                        {
                            if ((byBin[textPos - 3] == 0x00 && byBin[textPos - 2] == 0x80)
                                || (byBin[textPos - 1] == 0x00 && byBin[textPos] == 0x80))
                            {
                                hasBlankFlg = true;
                            }
                            else if ((byBin[textPos - 3] == 0x00 && byBin[textPos - 2] == 0xA0)
                                || (byBin[textPos - 1] == 0x00 && byBin[textPos] == 0xA0))
                            {
                                hasEndFlg = true;
                            }
                            else if (byBin[textPos - 2] > 0xC0 || byBin[textPos] > 0xC0)
                            {
                                break;
                            }

                        }
                        textPos -= 4;
                    }

                    if (!hasText)
                    {
                        sb2.Append((binFile.File)).Append("\r\n");
                    }
                            
                }
                catch (Exception exp)
                {
                }
            }

            File.WriteAllText(@"G:\Study\MySelfProject\Hanhua\Dino1\textAddr.txt", sb.ToString(), Encoding.UTF8);
            File.WriteAllText(@"G:\Study\MySelfProject\Hanhua\Dino1\noTextInfo.txt", sb2.ToString(), Encoding.UTF8);

            MessageBox.Show("OK");
        }


        private Dictionary<string, List<string>> GetFontChar(string excelFileName)
        {
            Dictionary<string, List<string>> fontChars = new Dictionary<string, List<string>>();
            if (string.IsNullOrEmpty(excelFileName))
            {
                return fontChars;
            }

            StringBuilder sb = new StringBuilder();
            List<string> char10a = new List<string>();
            List<string> char10b = new List<string>();
            char10a.AddRange(st8210AChar);
            char10b.AddRange(st8210BChar);
            fontChars.Add("st10a_00", char10a);
            fontChars.Add("st10b_00", char10b);

            string[] txtFiles = File.ReadAllLines(@"G:\Study\MySelfProject\Hanhua\Dino1\Dino1FontPic\fontImgInfo.txt");

            Microsoft.Office.Interop.Excel.Application xApp = null;
            Microsoft.Office.Interop.Excel.Workbook xBook = null;
            Microsoft.Office.Interop.Excel.Worksheet xSheet = null;

            try
            {
                // 创建Application对象 
                xApp = new Microsoft.Office.Interop.Excel.ApplicationClass();

                // 得到WorkBook对象, 打开已有的文件 
                xBook = xApp.Workbooks._Open(
                    excelFileName,
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                // 取得相应的Sheet
                xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets[1];

                int curFileIdx = 0;
                int lineNum = 21;
                int blankNum = 0;
                string[] char0Pos = { "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q" };
                string[] char1Pos = { "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH" };
                while (blankNum < 10)
                {
                    string cellValue = xSheet.get_Range("S" + lineNum, Missing.Value).Value2 as string;

                    int row = 0;
                    if (string.IsNullOrEmpty(cellValue))
                    {
                        blankNum++;
                    }
                    else
                    {
                        //xSheet.get_Range("A" + lineNum, Missing.Value).Value2 = txtFiles[curFileIdx];

                        blankNum = 0;
                        List<string> charList = new List<string>();

                        bool isCharOk = false;
                        for (row = 0; row < 3; row++)
                        {
                            for (int col = 0; col < 16; col++)
                            {
                                string cell0Value = xSheet.get_Range(char0Pos[col] + (lineNum + row), Missing.Value).Value2 as string;
                                string cell1Value = xSheet.get_Range(char1Pos[col] + (lineNum + row), Missing.Value).Value2 as string;
                                if (string.IsNullOrEmpty(cell0Value))
                                {
                                    if (charList.Count == 0)
                                    {
                                        cell0Value = "　";
                                    }
                                    else
                                    {
                                        isCharOk = true;
                                        break;
                                    }
                                }
                                charList.Add(cell0Value);
                                if (string.IsNullOrEmpty(cell1Value))
                                {
                                    isCharOk = true;
                                    break;
                                }
                                charList.Add(cell1Value);
                            }
                            if (isCharOk)
                            {
                                break;
                            }
                        }


                        sb.Append(txtFiles[curFileIdx]).Append("\r\n");
                        sb.Append(string.Join(",", charList.ToArray())).Append("\r\n");
                        fontChars.Add(txtFiles[curFileIdx++], charList);
                    }

                    lineNum += row + 1;
                }

                
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\r\n" + me.StackTrace);
            }
            finally
            {
                //xSheet.SaveAs(
                //    @"G:\Study\MySelfProject\Hanhua\Dino1\Dino1FontPic\DinoFontChk.xlsx",
                //    Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                //    Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                // 清空各种对象
                xSheet = null;
                xBook = null;
                if (xApp != null)
                {
                    xApp.Quit();
                    xApp = null;
                }
            }
            File.WriteAllText(@"G:\Study\MySelfProject\Hanhua\Dino1\dino1FontChar.txt", sb.ToString());

            return fontChars;
        }

        private void btnFormatText_Click(object sender, EventArgs e)
        {
            string curTxt = this.txtFormat.Text;
            if (string.IsNullOrEmpty(curTxt))
            {
                return;
            }

            string halfStr = "0123456789ABCDEFHIKLNOPRSVWm&/.";
            string fullStr = "０１２３４５６７８９ＡＢＣＤＥＦＨＩＫＬＮＯＰＲＳＶＷｍ＆／．";
            Dictionary<string, string> halfFullMap = new Dictionary<string, string>();
            for (int i = 0; i < halfStr.Length; i++)
            {
                halfFullMap.Add(halfStr.Substring(i, 1), fullStr.Substring(i, 1));
            }

            StringBuilder sb = new StringBuilder();
            string[] allLine = curTxt.Split('\n');
            foreach (string curLine in allLine)
            {
                if (string.IsNullOrEmpty(curLine))
                {
                    break;
                }
                string newLine = curLine.Replace("\r", "");
                string trueLine = newLine.Substring(0, newLine.Length - 6);
                for (int i = 0; i < trueLine.Length; i++)
                {
                    string curChar = trueLine.Substring(i, 1);
                    if (halfFullMap.ContainsKey(curChar))
                    {
                        sb.Append(halfFullMap[curChar]);
                    }
                    else
                    {
                        sb.Append(curChar);
                    }
                }
                sb.Append(newLine.Substring(newLine.Length - 6)).Append("\r\n");
            }
            this.txtFormat.Text = sb.ToString();

            //string[] allLine = curTxt.Split('\n');
            //List<string> newText = new List<string>();
            //int maxLen = 0;
            //string newLine;
            //foreach (string curLine in allLine)
            //{
            //    newLine = curLine.Replace("\r", "");
            //    if (newLine.EndsWith("^00C0^") || newLine.EndsWith("^0090^"))
            //    {
            //        newText.Add(newLine);

            //        newLine = Regex.Replace(newLine, @"\^.*?\^", "").Trim();
            //        if (newLine.Length > maxLen)
            //        {
            //            maxLen = newLine.Length;
            //        }
            //    }
            //    else if (newLine.EndsWith("^00A0^"))
            //    {
            //        string tmpLine = Regex.Replace(newLine, @"\^.*?\^", "").Trim();
            //        if (tmpLine.Length > maxLen)
            //        {
            //            maxLen = tmpLine.Length;
            //        }

            //        if (tmpLine.Length > 0)
            //        {
            //            newText.Add(this.getFormatedString(newLine, tmpLine, maxLen));
            //        }
            //        else
            //        {
            //            newText.Add(newLine);
            //        }

            //        int nextIdx = newText.Count - 2;
            //        while (nextIdx >= 0)
            //        {
            //            newLine = newText[nextIdx];
            //            if (newLine.EndsWith("^00C0^") || newLine.EndsWith("^0090^"))
            //            {
            //                tmpLine = Regex.Replace(newLine, @"\^.*?\^", "").Trim();
            //                if (tmpLine.Length > 0)
            //                {
            //                    newText[nextIdx] = this.getFormatedString(newLine, tmpLine, maxLen);
            //                }
            //            }
            //            else
            //            {
            //                break;
            //            }
            //            nextIdx--;
            //        }
            //        maxLen = 0;
            //    }
            //}

            //this.txtFormat.Text = string.Join("\r\n", newText.ToArray());
            Clipboard.SetText(this.txtFormat.Text);
        }

        private string getFormatedString(string input, string trimInput, int maxLen)
        {
            int leftBlank = (18 - maxLen) / 2;
            int rightBlank = 18 - leftBlank - trimInput.Length;

            // 匹配开头的 ^...^ 部分（可能连续多个）
            string startPattern = @"^(?<!\s)\^[^^]*\^(?:\^[^^]*\^)*"; //@"^\^[^^]*\^(?:\^[^^]*\^)*";
            Match startMatch = Regex.Match(input, startPattern);
            string startPart = startMatch.Success ? startMatch.Value : "";

            // 匹配结尾的 ^...^ 部分（可能连续多个）
            string endPattern = @"\^[^^]*\^(?:\^[^^]*\^)*$";
            Match endMatch = Regex.Match(input, endPattern);
            string endPart = endMatch.Success ? endMatch.Value : "";

            // 中间部分 = 原始字符串 - 开头部分 - 结尾部分
            string middlePart = input.Substring(
                startPart.Length,
                input.Length - startPart.Length - endPart.Length
            );

            return startPart + "".PadLeft(leftBlank, '　') + middlePart.Trim() + "".PadRight(rightBlank, '　') + endPart;
        }

        private void btnChkCnChar_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application xApp = null;
            Microsoft.Office.Interop.Excel.Workbook xBook = null;
            Microsoft.Office.Interop.Excel.Worksheet xSheet = null;

            try
            {
                // 创建Application对象 
                xApp = new Microsoft.Office.Interop.Excel.ApplicationClass();

                // 得到WorkBook对象, 打开已有的文件 
                xBook = xApp.Workbooks._Open(
                    @"G:\Study\MySelfProject\Hanhua\Dino1\allJpText.xlsx",
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                int lineNum = 1;
                int blankNum = 0;
                Dictionary<string, string> halfFullMap = new Dictionary<string, string>();
                Dictionary<string, List<string>> cnFontChar = new Dictionary<string, List<string>>();
                List<string> comnCnFontChars = new List<string>();
                cnFontChar["SLPS_021.80"] = comnCnFontChars;

                string halfStr = " 0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZemos&/.+";
                string fullStr = "　０１２３４５６７８９ＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺｅｍｏｓ＆／．＋";
                for (int i = 0; i < fullStr.Length; i++)
                {
                    comnCnFontChars.Add(fullStr.Substring(i, 1));
                    halfFullMap.Add(halfStr.Substring(i, 1), fullStr.Substring(i, 1));
                }

                List<KeyValuePair<string, int>> charCount = new List<KeyValuePair<string, int>>();
                // 取得相应的Sheet
                for (int i = 1; i <= xBook.Sheets.Count; i++)
                {
                    xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets[i];
                    List<string> curCnFontChars = new List<string>();
                    comnCnFontChars = cnFontChar["SLPS_021.80"];

                    if (!xSheet.Name.Equals("SLPS_021.80"))
                    {
                        cnFontChar.Add(xSheet.Name, curCnFontChars);
                    }
                    else
                    {
                        curCnFontChars = comnCnFontChars;
                    }

                    lineNum = 1;
                    blankNum = 0;
                    while (blankNum < 5)
                    {
                        string cnTxtValue = xSheet.get_Range("J" + lineNum, Missing.Value).Value2 as string;

                        if (string.IsNullOrEmpty(cnTxtValue))
                        {
                            blankNum++;
                        }
                        else
                        {
                            blankNum = 0;
                            cnTxtValue = Regex.Replace(cnTxtValue, @"\^.*?\^", "");
                            for (int j = 0; j < cnTxtValue.Length; j++)
                            {
                                string curChar = cnTxtValue.Substring(j, 1);
                                if (halfFullMap.ContainsKey(curChar))
                                {
                                    curChar = halfFullMap[curChar];
                                }

                                if (!comnCnFontChars.Contains(curChar))
                                {
                                    if (!curCnFontChars.Contains(curChar))
                                    {
                                        curCnFontChars.Add(curChar);
                                        charCount.Add(new KeyValuePair<string, int>(curChar, 1));
                                    }
                                    //else
                                    //{
                                    //    KeyValuePair<string, int> charInfo = charCount.FirstOrDefault(p => p.Key.Equals(curChar));
                                    //    int count = charInfo.Value + 1;
                                    //    charCount.Remove(charInfo);
                                    //    charCount.Add(new KeyValuePair<string, int>(curChar, count));
                                    //}
                                }
                                //else
                                //{
                                //    KeyValuePair<string, int> charInfo = charCount.FirstOrDefault(p => p.Key.Equals(curChar));
                                //    int count = charInfo.Value + 1;
                                //    charCount.Remove(charInfo);
                                //    charCount.Add(new KeyValuePair<string, int>(curChar, count));
                                //}
                            }
                        }

                        lineNum++;
                    }
                    //break;
                }

                StringBuilder sb = new StringBuilder();
                //charCount.Sort(this.CharCountCompare);
                //foreach (KeyValuePair<string, int> curST in charCount)
                //{
                //    sb.Append(curST.Key).Append(" ").Append(curST.Value).Append("\r\n");
                //}
                //File.WriteAllText(@"G:\Study\MySelfProject\Hanhua\Dino1\dino1FontCharCmnChk.txt", sb.ToString(), Encoding.UTF8);


                foreach (KeyValuePair<string, List<string>> curST in cnFontChar)
                {
                    sb.Append(curST.Key).Append("\r\n");
                    List<string> curCnFontChars = curST.Value;
                    if (!curST.Key.Equals("SLPS_021.80"))
                    {
                        curCnFontChars.Sort();
                    }

                    sb.Append(string.Join(",", curCnFontChars.ToArray())).Append("\r\n");
                }

                //comnCnFontChars = cnFontChar["SLPS_021.80"];
                //comnCnFontChars.Sort();

                File.WriteAllText(@"G:\Study\MySelfProject\Hanhua\Dino1\dino1FontChar_cn.txt", sb.ToString(), Encoding.UTF8);

            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\r\n" + me.StackTrace);
            }
            finally
            {
                //xSheet.SaveAs(
                //    @"G:\Study\MySelfProject\Hanhua\Dino1\Dino1FontPic\DinoFontChk.xlsx",
                //    Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                //    Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                // 清空各种对象
                xSheet = null;
                xBook = null;
                if (xApp != null)
                {
                    xApp.Quit();
                    xApp = null;
                }
            }
        }

        /// <summary>
        /// 对象比较
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private int CharCountCompare(KeyValuePair<string, int> a, KeyValuePair<string, int> b)
        {
            return b.Value - a.Value;
        }

        private void btnCreateCnPic_Click(object sender, EventArgs e)
        {
            ImgUtil.DrawImgType = 2;
            SolidBrush customBrush = new SolidBrush(Color.FromArgb(255, (0x5AD6 & 0x1F) << 3, (0x5AD6 & 0x1F) << 3, (0x5AD6 & 0x1F) << 3));
            SolidBrush outBrush = new SolidBrush(Color.FromArgb(255, (0x318C & 0x1F) << 3, (0x318C & 0x1F) << 3, (0x318C & 0x1F) << 3));
            Pen newPen = new Pen(Color.FromArgb(255, (0x318C & 0x1F) << 3, (0x318C & 0x1F) << 3, (0x318C & 0x1F) << 3), 0.5F);
            //Pen newPen = new Pen(Color.FromArgb(255, (0x0842 & 0x1F) << 3, (0x0842 & 0x1F) << 3, (0x0842 & 0x1F) << 3), 1F);

            Font boldFont = new Font("SimSun", 14f, FontStyle.Bold, GraphicsUnit.Pixel);
            Font font = new Font("SimSun", 13.5f, FontStyle.Regular, GraphicsUnit.Pixel);
            Color fillColor = Color.FromArgb(255, (0x5AD6 & 0x1F) << 3, (0x5AD6 & 0x1F) << 3, (0x5AD6 & 0x1F) << 3);
            Color outlineColor = Color.FromArgb(255, (0x318C & 0x1F) << 3, (0x318C & 0x1F) << 3, (0x318C & 0x1F) << 3);

            string[] allCnFontChar = File.ReadAllLines(@"G:\Study\MySelfProject\Hanhua\Dino1\dino1FontChar_cn.txt", Encoding.UTF8);
            for (int i = 0; i < allCnFontChar.Length; i += 2)
            {
                string path = @"G:\Study\MySelfProject\Hanhua\Dino1\Dino1FontPic\FontPicCn\old";
                string[] curPicChars = allCnFontChar[i + 1].Split(',');
                int tmpCount = ((curPicChars.Length + 1) & 0xFFFE) / 2;
                int imgRows = tmpCount / 16;
                if ((tmpCount & 0xF) > 0)
                {
                    imgRows++;
                }

                ImgInfo imgInfo = new ImgInfo(256, imgRows * 16);
                imgInfo.BlockImgH = 16;
                imgInfo.BlockImgW = 16;
                imgInfo.PosX = -1;
                imgInfo.PosY = 0;
                imgInfo.XPadding = -1;
                imgInfo.YPadding = 0;
                imgInfo.YMarging = 1f;
                //imgInfo.NeedBorder = false;
                imgInfo.NeedBorder = true;
                //imgInfo.FontSize = 16;
                imgInfo.FontSize = 13.5f;
                //imgInfo.FontStyle = FontStyle.Regular;
                //imgInfo.FontName = "WenQuanYi Bitmap Song 16px";
                imgInfo.FontStyle = FontStyle.Bold;
                imgInfo.FontName = "SimSun";
                //imgInfo.FontName = "迷你简美黑";
                //imgInfo.FontName = "JetLink BoldMauKai";
                imgInfo.Brush = customBrush; // Brushes.White;
                imgInfo.OutPathBrush = customBrush; // Brushes.White;
                imgInfo.Pen = newPen;
                imgInfo.Pen.LineJoin = LineJoin.Round;
                imgInfo.Pen.Alignment = PenAlignment.Outset;
                imgInfo.Pen.MiterLimit = 0.5f;
                imgInfo.Sf.LineAlignment = StringAlignment.Center;
                imgInfo.Sf.FormatFlags = StringFormatFlags.NoClip;
                imgInfo.Grp.Clear(Color.Black);
                //imgInfo.Grp.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                //imgInfo.Grp.SmoothingMode = SmoothingMode.None;
                imgInfo.Grp.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //imgInfo.Grp.PixelOffsetMode = PixelOffsetMode.None;
                imgInfo.Grp.PixelOffsetMode = PixelOffsetMode.HighQuality;
                //imgInfo.Grp.SmoothingMode = SmoothingMode.AntiAlias;
                //imgInfo.Grp.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

                List<string> txtList1 = new List<string>();
                List<string> txtList2 = new List<string>();
                for (int j = 0; j < curPicChars.Length; j += 2)
                {
                    txtList1.Add(curPicChars[j]);
                    if (j < curPicChars.Length - 1)
                    {
                        txtList2.Add(curPicChars[j + 1]);
                    }
                }

                //Bitmap curImg = ImgUtil.WriteFontImg(imgInfo, txtList1);
                Bitmap curImg = DrawOutlinedTextAtlas(string.Join("", txtList1.ToArray()), 16, font, outlineColor, fillColor);
                //Bitmap curImg = RenderStringToFontAtlas(string.Join("", txtList1.ToArray()), "SimSun", FontStyle.Regular);
                curImg.Save(path + @"1\" + allCnFontChar[i].ToUpper() + @".png");

                //imgInfo.Grp.Clear(Color.Black);
                //curImg = ImgUtil.WriteFontImg(imgInfo, txtList2);
                curImg = DrawOutlinedTextAtlas(string.Join("", txtList2.ToArray()), 16, font, outlineColor, fillColor);
                //curImg = RenderStringToFontAtlas(string.Join("", txtList1.ToArray()), "SimSun", FontStyle.Regular);
                curImg.Save(path + @"2\" + allCnFontChar[i].ToUpper() + @".png");
            }
        }

        private Bitmap DrawOutlinedTextAtlas(
            string text,
            int charSize,
            Font font,
            Color outlineColor,
            Color fillColor)
        {
            const int ATLAS_WIDTH = 256;

            int charsPerRow = ATLAS_WIDTH / charSize;
            int rows = (int)Math.Ceiling(text.Length / (float)charsPerRow);

            int width = ATLAS_WIDTH;
            int height = rows * charSize;

            Font boldFont = new Font("SimSun", font.Size, FontStyle.Bold, GraphicsUnit.Pixel);
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                // 背景
                g.Clear(Color.Black);

                // 关键设置
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                //g.SmoothingMode = SmoothingMode.None;
                //g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                //g.InterpolationMode = InterpolationMode.NearestNeighbor;
                //g.PixelOffsetMode = PixelOffsetMode.Half;

                using (Brush outlineBrush = new SolidBrush(outlineColor))
                using (Brush fillBrush = new SolidBrush(fillColor))
                {
                    StringFormat sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center,
                        FormatFlags = StringFormatFlags.NoClip
                    };

                    for (int i = 0; i < text.Length; i++)
                    {
                        int col = i % charsPerRow;
                        int row = i / charsPerRow;

                        int x = col * charSize;
                        int y = row * charSize;

                        Rectangle rect = new Rectangle(x, y, charSize, charSize);

                        // ---- 描边：上下左右 ----
                        //DrawChar(g, text[i].ToString(), font, outlineBrush, rect, sf, -1f, 0);
                        DrawChar(g, text[i].ToString(), font, outlineBrush, rect, sf, 0.5f, 0);
                        //DrawChar(g, text[i].ToString(), font, outlineBrush, rect, sf, 0, -1f);
                        DrawChar(g, text[i].ToString(), font, outlineBrush, rect, sf, 0, 0.5f);

                        //DrawChar(g, text[i].ToString(), boldFont, outlineBrush, rect, sf, 0, 0);
                        

                        // ---- 正文 ----
                        DrawChar(g, text[i].ToString(), font, fillBrush, rect, sf, 0, 0);
                    }
                }
            }

            return bmp;
        }

        private void DrawChar(
            Graphics g,
            string ch,
            Font font,
            Brush brush,
            Rectangle rect,
            StringFormat sf,
            float offsetX,
            float offsetY)
        {
            RectangleF r = new RectangleF(
                rect.X + offsetX,
                rect.Y + offsetY + 1,
                rect.Width,
                rect.Height);

            g.DrawString(ch, font, brush, r, sf);
        }

        private Bitmap RenderStringToFontAtlas(
            string text,
            string fontName,
            FontStyle style)
        {
            const int CELL_SIZE = 16;
            const int ATLAS_WIDTH = 256;
            const int COLS = ATLAS_WIDTH / CELL_SIZE;

            int charCount = text.Length;
            int rows = (charCount + COLS - 1) / COLS;
            int atlasHeight = rows * CELL_SIZE;

            Bitmap atlas = new Bitmap(
                ATLAS_WIDTH,
                atlasHeight,
                PixelFormat.Format32bppArgb
            );

            using (Graphics g = Graphics.FromImage(atlas))
            {
                g.Clear(Color.Black);
            }

            for (int i = 0; i < charCount; i++)
            {
                int col = i % COLS;
                int row = i / COLS;

                int x = col * CELL_SIZE;
                int y = row * CELL_SIZE;

                Bitmap ch = RenderOutlinedCharTo4Color(
                    text[i].ToString(),
                    fontName,
                    style
                );

                using (Graphics g = Graphics.FromImage(atlas))
                {
                    g.DrawImage(ch, x, y);
                }
            }

            return atlas;
        }

        private Bitmap RenderOutlinedCharTo4Color(
            string text,
            string fontName,
            FontStyle style)
        {
            const int SRC_SIZE = 64;
            const int DST_SIZE = 16;
            SolidBrush customBrush = new SolidBrush(Color.FromArgb(255, (0x5AD6 & 0x1F) << 3, (0x5AD6 & 0x1F) << 3, (0x5AD6 & 0x1F) << 3));

            // ===== Step 1：64×64 黑底 + 描边 =====
            Bitmap src = new Bitmap(SRC_SIZE, SRC_SIZE, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(src))
            {
                g.Clear(Color.Black);
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (GraphicsPath path = new GraphicsPath())
                using (Font font = new Font(fontName, 56, style, GraphicsUnit.Pixel))
                {
                    StringFormat sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };

                    path.AddString(
                        text,
                        font.FontFamily,
                        (int)style,
                        font.Size,
                        new Rectangle(0, 4, SRC_SIZE, SRC_SIZE),
                        sf
                    );

                    //using (Pen pen = new Pen(Color.FromArgb(255, (0x0842 & 0x1F) << 3, (0x0842 & 0x1F) << 3, (0x0842 & 0x1F) << 3), 4F))
                    using (Pen pen = new Pen(Color.FromArgb(160, 160, 160), 8f))
                    {
                        pen.LineJoin = LineJoin.Round;
                        pen.Alignment = PenAlignment.Outset;
                        pen.MiterLimit = 0.5f;
                        g.DrawPath(pen, path);
                    }

                    using (Brush brush = new SolidBrush(Color.White))
                    {
                        g.FillPath(brush, path);
                    }
                    //g.FillPath(customBrush, path);
                }
            }

            // ===== Step 2：缩放到 16×16 =====
            Bitmap scaled = new Bitmap(DST_SIZE, DST_SIZE, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(scaled))
            {
                g.Clear(Color.Black);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.DrawImage(src, 0, 0, DST_SIZE, DST_SIZE);
            }

            // ===== Step 3：4 色量化 =====
            //Bitmap result = new Bitmap(DST_SIZE, DST_SIZE, PixelFormat.Format32bppArgb);

            //Color[] palette =
            //{
            //    Color.Black,
            //    Color.FromArgb((0x0842 & 0x1F) << 3,(0x0842 & 0x1F) << 3,(0x0842 & 0x1F) << 3),
            //    Color.FromArgb((0x318C & 0x1F) << 3,(0x318C & 0x1F) << 3,(0x318C & 0x1F) << 3),
            //    Color.FromArgb((0x5AD6 & 0x1F) << 3,(0x5AD6 & 0x1F) << 3,(0x5AD6 & 0x1F) << 3)
            //};

            //for (int y = 0; y < DST_SIZE; y++)
            //{
            //    for (int x = 0; x < DST_SIZE; x++)
            //    {
            //        Color c = scaled.GetPixel(x, y);

            //        // 灰度（0~1）
            //        float gray =
            //            (0.299f * c.R +
            //             0.587f * c.G +
            //             0.114f * c.B) / 255f;

            //        // Alpha（0~1）
            //        float alpha = c.A / 255f;

            //        // ★ 实际笔画强度（关键）
            //        float intensity = gray * alpha;

            //        Color dst;
            //        if (intensity < 0.20f)
            //            dst = palette[0];
            //        else if (intensity < 0.45f)
            //            dst = palette[1];
            //        else if (intensity < 0.75f)
            //            dst = palette[2];
            //        else
            //            dst = palette[3];

            //        result.SetPixel(x, y, dst);
            //    }
            //}


            return scaled;
        }


        private void btnImportImg_Click(object sender, EventArgs e)
        {
            List<string> palGreyscale4bp = new List<string>();
            palGreyscale4bp.Add("00000000");
            palGreyscale4bp.Add((0x0842 & 0x1F).ToString("X4") + "0000");
            palGreyscale4bp.Add((0x318C & 0x1F).ToString("X4") + "0000");
            palGreyscale4bp.Add((0x5AD6 & 0x1F).ToString("X4") + "0000");

            palGreyscale4bp.Add("0000" + (0x0842 & 0x1F).ToString("X4"));
            palGreyscale4bp.Add((0x0842 & 0x1F).ToString("X4") + (0x0842 & 0x1F).ToString("X4"));
            palGreyscale4bp.Add((0x318C & 0x1F).ToString("X4") + (0x0842 & 0x1F).ToString("X4"));
            palGreyscale4bp.Add((0x5AD6 & 0x1F).ToString("X4") + (0x0842 & 0x1F).ToString("X4"));

            palGreyscale4bp.Add("0000" + (0x318C & 0x1F).ToString("X4"));
            palGreyscale4bp.Add((0x0842 & 0x1F).ToString("X4") + (0x318C & 0x1F).ToString("X4"));
            palGreyscale4bp.Add((0x318C & 0x1F).ToString("X4") + (0x318C & 0x1F).ToString("X4"));
            palGreyscale4bp.Add((0x5AD6 & 0x1F).ToString("X4") + (0x318C & 0x1F).ToString("X4"));

            palGreyscale4bp.Add("0000" + (0x5AD6 & 0x1F).ToString("X4"));
            palGreyscale4bp.Add((0x0842 & 0x1F).ToString("X4") + (0x5AD6 & 0x1F).ToString("X4"));
            palGreyscale4bp.Add((0x318C & 0x1F).ToString("X4") + (0x5AD6 & 0x1F).ToString("X4"));
            palGreyscale4bp.Add((0x5AD6 & 0x1F).ToString("X4") + (0x5AD6 & 0x1F).ToString("X4"));

            XmlDocument doc = new XmlDocument();
            string[] impBinMap = File.ReadAllLines(@"G:\Study\MySelfProject\Hanhua\Dino1\fontImgInfo.txt", Encoding.UTF8);
            for (int i = 0; i < impBinMap.Length; i += 2)
            {
                string imgName = impBinMap[i] + ".png";
                if (imgName.StartsWith("CLEAR", StringComparison.OrdinalIgnoreCase))
                {
                    imgName = "ENDING.png";
                }
                else if (imgName.StartsWith("CORE", StringComparison.OrdinalIgnoreCase))
                {
                    imgName = "SLPS_021.80.png";
                }

                if (!File.Exists(@"G:\Study\MySelfProject\Hanhua\Dino1\Dino1FontPic\FontPicCn\reduceColor1\" + imgName))
                {
                    continue;
                }
                Bitmap image1 = (Bitmap)Bitmap.FromFile(@"G:\Study\MySelfProject\Hanhua\Dino1\Dino1FontPic\FontPicCn\reduceColor1\" + imgName);
                Bitmap image2 = (Bitmap)Bitmap.FromFile(@"G:\Study\MySelfProject\Hanhua\Dino1\Dino1FontPic\FontPicCn\reduceColor2\" + imgName);
                //int newHeight = Math.Max(3, 1 + image1.Height / 16) * 16; 
                int newHeight = ((image1.Height + 0x1F) & (~0x1F));
                byte[] by = new byte[Align(image1.Width * newHeight / 2, 2048)];
                int byImgIdx = 0;
                for (int y = 0; y < image1.Height; y++)
                {
                    for (int x = 0; x < image1.Width; x += 2)
                    {
                        int tmpL = palGreyscale4bp.IndexOf((image1.GetPixel(x, y).R >> 3).ToString("X4") + (image2.GetPixel(x, y).R >> 3).ToString("X4"));
                        int tmpH = palGreyscale4bp.IndexOf((image1.GetPixel(x + 1, y).R >> 3).ToString("X4") + (image2.GetPixel(x + 1, y).R >> 3).ToString("X4"));

                        by[byImgIdx++] = (byte)(tmpL | (tmpH << 4));
                    }
                }

                
                if (imgName.StartsWith("SLPS_021.80", StringComparison.OrdinalIgnoreCase))
                {
                    byte[] byAllImg = File.ReadAllBytes(impBinMap[i + 1]);
                    for (int j = 0; j < 256; j++)
                    {
                        Array.Copy(by, j * 128, byAllImg, j * 512, 128);
                    }
                    File.WriteAllBytes(impBinMap[i + 1].Replace("PS_jp", "PS_cn"), byAllImg);
                }
                else
                {
                    File.WriteAllBytes(impBinMap[i + 1].Replace("PS_jp", "PS_cn"), by);

                    string folder = impBinMap[i + 1].Substring(0, impBinMap[i + 1].IndexOf(impBinMap[i])).Replace("PS_jp", "PS_cn");
                    string texName = Util.GetShortName(impBinMap[i + 1]);
                    string xmlPath = Path.Combine(folder + impBinMap[i], "manifest.xml");
                    doc.Load(xmlPath);
                    XmlNodeList nodes = doc.SelectNodes("/DinoCrisisPackage/Entry");
                    foreach (XmlNode node in nodes)
                    {
                        if (texName.Equals(node.InnerText.Trim()))
                        {
                            node.Attributes["h"].Value = image1.Height.ToString();
                            doc.Save(xmlPath);
                            break;
                        }
                    }
                }
            }
        }

        private void btnTestFontPic_Click(object sender, EventArgs e)
        {
            List<int> palGreyscale4bp1 = new List<int>();
            List<int> palGreyscale4bp2 = new List<int>();
            palGreyscale4bp1.Add(0);
            palGreyscale4bp2.Add(0);

            palGreyscale4bp1.Add((0x0842 & 0x1F) << 3);
            palGreyscale4bp2.Add(0);

            palGreyscale4bp1.Add((0x318C & 0x1F) << 3);
            palGreyscale4bp2.Add(0);

            palGreyscale4bp1.Add((0x5AD6 & 0x1F) << 3);
            palGreyscale4bp2.Add(0);

            palGreyscale4bp1.Add(0);
            palGreyscale4bp2.Add((0x0842 & 0x1F) << 3);

            palGreyscale4bp1.Add((0x0842 & 0x1F) << 3);
            palGreyscale4bp2.Add((0x0842 & 0x1F) << 3);

            palGreyscale4bp1.Add((0x318C & 0x1F) << 3);
            palGreyscale4bp2.Add((0x0842 & 0x1F) << 3);

            palGreyscale4bp1.Add((0x5AD6 & 0x1F) << 3);
            palGreyscale4bp2.Add((0x0842 & 0x1F) << 3);

            palGreyscale4bp1.Add(0);
            palGreyscale4bp2.Add((0x318C & 0x1F) << 3);

            palGreyscale4bp1.Add((0x0842 & 0x1F) << 3);
            palGreyscale4bp2.Add((0x318C & 0x1F) << 3);

            palGreyscale4bp1.Add((0x318C & 0x1F) << 3);
            palGreyscale4bp2.Add((0x318C & 0x1F) << 3);

            palGreyscale4bp1.Add((0x5AD6 & 0x1F) << 3);
            palGreyscale4bp2.Add((0x318C & 0x1F) << 3);

            palGreyscale4bp1.Add(0);
            palGreyscale4bp2.Add((0x5AD6 & 0x1F) << 3);

            palGreyscale4bp1.Add((0x0842 & 0x1F) << 3);
            palGreyscale4bp2.Add((0x5AD6 & 0x1F) << 3);

            palGreyscale4bp1.Add((0x318C & 0x1F) << 3);
            palGreyscale4bp2.Add((0x5AD6 & 0x1F) << 3);

            palGreyscale4bp1.Add((0x5AD6 & 0x1F) << 3);
            palGreyscale4bp2.Add((0x5AD6 & 0x1F) << 3);

            XmlDocument doc = new XmlDocument();
            string[] impBinMap = File.ReadAllLines(@"G:\Study\MySelfProject\Hanhua\Dino1\fontImgInfo.txt", Encoding.UTF8);
            for (int i = 0; i < impBinMap.Length; i += 2)
            {
                if (!File.Exists(impBinMap[i + 1].Replace("PS_jp", "PS_cn")))
                {
                    continue;
                }

                Bitmap image1 = null;
                Bitmap image2 = null;
                string folder = impBinMap[i + 1].Substring(0, impBinMap[i + 1].IndexOf(impBinMap[i])).Replace("PS_jp", "PS_cn");
                string texName = Util.GetShortName(impBinMap[i + 1]);
                string xmlPath = Path.Combine(folder + impBinMap[i], "manifest.xml");
                doc.Load(xmlPath);
                XmlNodeList nodes = doc.SelectNodes("/DinoCrisisPackage/Entry");
                foreach (XmlNode node in nodes)
                {
                    if (texName.Equals(node.InnerText.Trim()))
                    {
                        image1 = new Bitmap(Convert.ToInt32(node.Attributes["w"].Value) * 4, Convert.ToInt32(node.Attributes["h"].Value));
                        image2 = new Bitmap(Convert.ToInt32(node.Attributes["w"].Value) * 4, Convert.ToInt32(node.Attributes["h"].Value));
                        break;
                    }
                }

                byte[] byImg = File.ReadAllBytes(impBinMap[i + 1].Replace("PS_jp", "PS_cn"));
                int imgIdx = 0;
                for (int y = 0; y < image1.Height; y++)
                {
                    for (int x = 0; x < image1.Width; x += 2)
                    {
                        int curData = byImg[imgIdx++];
                        image1.SetPixel(x, y, Color.FromArgb(palGreyscale4bp1[curData & 0xF], palGreyscale4bp1[curData & 0xF], palGreyscale4bp1[curData & 0xF]));
                        image1.SetPixel(x + 1, y, Color.FromArgb(palGreyscale4bp1[(curData >> 4) & 0xF], palGreyscale4bp1[(curData >> 4) & 0xF], palGreyscale4bp1[(curData >> 4) & 0xF]));

                        image2.SetPixel(x, y, Color.FromArgb(palGreyscale4bp2[curData & 0xF], palGreyscale4bp2[curData & 0xF], palGreyscale4bp2[curData & 0xF]));
                        image2.SetPixel(x + 1, y, Color.FromArgb(palGreyscale4bp2[(curData >> 4) & 0xF], palGreyscale4bp2[(curData >> 4) & 0xF], palGreyscale4bp2[(curData >> 4) & 0xF]));
                    }
                }
                image1.Save(@"G:\Study\MySelfProject\Hanhua\Dino1\Dino1FontPic\FontPicCn\test1\" + impBinMap[i] + ".png");
                image2.Save(@"G:\Study\MySelfProject\Hanhua\Dino1\Dino1FontPic\FontPicCn\test2\" + impBinMap[i] + ".png");
            }
        }

        private void btnImpCnText_Click(object sender, EventArgs e)
        {
            Dictionary<string, List<string>> cnFontChar = new Dictionary<string, List<string>>();
            string[] allCnFontChar = File.ReadAllLines(@"G:\Study\MySelfProject\Hanhua\Dino1\dino1FontChar_cn.txt", Encoding.UTF8);
            for (int i = 0; i < allCnFontChar.Length; i += 2)
            {
                List<string> curCnChars = new List<string>();
                curCnChars.AddRange(allCnFontChar[i + 1].Split(','));
                string key = allCnFontChar[i].ToUpper();
                cnFontChar.Add(key, curCnChars);

                if (key.StartsWith("ST10B"))
                {
                    cnFontChar.Add("ST1", curCnChars);
                }
                else if (key.StartsWith("ST506"))
                {
                    cnFontChar.Add("ST5", curCnChars);
                }
                else if (key.StartsWith("ST806"))
                {
                    cnFontChar.Add("ST8", curCnChars);
                }
            }

            List<string> comnCnFontChars = cnFontChar["SLPS_021.80"];
            List<string> cnFont80 = new List<string>();
            List<string> cnFont81 = new List<string>();
            for (int j = 0; j < comnCnFontChars.Count; j++)
            {
                if (cnFont80.Count < 256)
                {
                    cnFont80.Add(comnCnFontChars[j]);
                }
                else
                {
                    cnFont81.Add(comnCnFontChars[j]);
                }
            }

            string[] allCnFileInfo = File.ReadAllLines(@"G:\Study\MySelfProject\Hanhua\Dino1\textAddr.txt", Encoding.UTF8);
            Dictionary<string, string> cnFileInfo = new Dictionary<string, string>();
            for (int i = 0; i < allCnFileInfo.Length; i += 2)
            {
                if (allCnFileInfo[i].EndsWith(".bin", StringComparison.OrdinalIgnoreCase))
                {
                    string keyName = Util.GetShortName(allCnFileInfo[i].Substring(0, allCnFileInfo[i].Length - 7));
                    cnFileInfo.Add(keyName.ToUpper(), allCnFileInfo[i] + " " + allCnFileInfo[i + 1]);
                }
                else
                {
                    //string keyName = "SLPS_021.80";
                    if (!cnFileInfo.ContainsKey("SLPS_021.80"))
                    {
                        cnFileInfo.Add("SLPS_021.80", allCnFileInfo[i] + " " + allCnFileInfo[i + 1]);
                    }
                    else if (!cnFileInfo.ContainsKey("SLPS_021.81"))
                    {
                        cnFileInfo.Add("SLPS_021.81", allCnFileInfo[i] + " " + allCnFileInfo[i + 1]);
                    }
                    else
                    {
                        cnFileInfo.Add("SLPS_021.82", allCnFileInfo[i] + " " + allCnFileInfo[i + 1]);
                    }
                }
            }

            Microsoft.Office.Interop.Excel.Application xApp = null;
            Microsoft.Office.Interop.Excel.Workbook xBook = null;
            Microsoft.Office.Interop.Excel.Worksheet xSheet = null;
            StringBuilder textErr = new StringBuilder();
            string updateCnFile = string.Empty;

            try
            {
                // 创建Application对象 
                xApp = new Microsoft.Office.Interop.Excel.ApplicationClass();

                // 得到WorkBook对象, 打开已有的文件 
                xBook = xApp.Workbooks._Open(
                    @"G:\Study\MySelfProject\Hanhua\Dino1\allJpText.xlsx",
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                int lineNum = 1;
                int blankNum = 0;
                Dictionary<string, string> halfFullMap = new Dictionary<string, string>();
                string halfStr = " 0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZemos&/.+";
                string fullStr = "　０１２３４５６７８９ＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺｅｍｏｓ＆／．＋";
                for (int i = 0; i < fullStr.Length; i++)
                {
                    comnCnFontChars.Add(fullStr.Substring(i, 1));
                    halfFullMap.Add(halfStr.Substring(i, 1), fullStr.Substring(i, 1));
                }

                string nextChar;
                string currentChar;
                StringBuilder keyWordsSb = new StringBuilder();
                List<byte> byComnEntryInfo = new List<byte>();

                // 取得相应的Sheet
                for (int i = 1; i <= xBook.Sheets.Count; i++)
                {
                    xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets[i];
                    string sheetName = xSheet.Name.ToUpper();

                    if (chkOnlyComn.Checked)
                    {
                        if (!sheetName.StartsWith("SLPS_021"))
                        {
                            continue;
                        }
                    }

                    //if (sheetName.IndexOf("ST60A") < 0)
                    //{
                    //    continue;
                    //}


                    List<string> curCnFontChars;
                    if (sheetName.IndexOf("SLPS_021") >= 0)
                    {
                        curCnFontChars = cnFont80;
                    }
                    else
                    {
                        curCnFontChars = cnFontChar[sheetName];
                    }

                    string[] cnToFiles = cnFileInfo[sheetName].Split(' ');
                    updateCnFile = cnToFiles[0];
                    if (sheetName.IndexOf("SLPS_021") >= 0)
                    {
                        updateCnFile = @"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\SLPS_021.80";
                    }

                    byte[] byOldCnFile = File.ReadAllBytes(updateCnFile);
                    int startPos = Convert.ToInt32(cnToFiles[1], 16);
                    int endPos = byOldCnFile.Length;
                    if (cnToFiles.Length > 2)
                    {
                        endPos = Convert.ToInt32(cnToFiles[2], 16);
                    }
                    int startTextPos = startPos;
                    int endTextPos = endPos;
                    if (cnToFiles.Length > 3)
                    {
                        startTextPos = endPos;
                        endTextPos = Convert.ToInt32(cnToFiles[3], 16);

                        byComnEntryInfo.Clear();
                        byComnEntryInfo.Add(0);
                        byComnEntryInfo.Add(0);
                        byComnEntryInfo.Add(0);
                        byComnEntryInfo.Add(0);
                    }

                    List<byte> byNewCnFile = new List<byte>();
                    lineNum = 1;
                    blankNum = 0;
                    while (blankNum < 5)
                    {
                        string cnTxtValue = xSheet.get_Range("J" + lineNum, Missing.Value).Value2 as string;

                        if (string.IsNullOrEmpty(cnTxtValue))
                        {
                            blankNum++;
                        }
                        else
                        {
                            blankNum = 0;
                            for (int j = 0; j < cnTxtValue.Length; j++)
                            {
                                currentChar = cnTxtValue.Substring(j, 1);
                                if ("^" == currentChar)
                                {
                                    // 关键字的解码
                                    keyWordsSb.Length = 0;
                                    while ((nextChar = cnTxtValue.Substring(++j, 1)) != "^")
                                    {
                                        keyWordsSb.Append(nextChar);
                                    }

                                    string keyWords = keyWordsSb.ToString();
                                    for (int k = 0; k < keyWords.Length; k += 2)
                                    {
                                        string keyWord = keyWords.Substring(k, 2);
                                        byNewCnFile.Add(Convert.ToByte(keyWord, 16));
                                    }

                                    continue;
                                }
                                else
                                {
                                    if (halfFullMap.ContainsKey(currentChar))
                                    {
                                        currentChar = halfFullMap[currentChar];
                                    }

                                    if (cnFont80.Contains(currentChar))
                                    {
                                        byNewCnFile.Add((byte)(cnFont80.IndexOf(currentChar)));
                                        byNewCnFile.Add(0x80);
                                    }
                                    else if (cnFont81.Contains(currentChar))
                                    {
                                        byNewCnFile.Add((byte)(cnFont81.IndexOf(currentChar)));
                                        byNewCnFile.Add(0x81);
                                    }
                                    else if (curCnFontChars.Contains(currentChar))
                                    {
                                        byNewCnFile.Add((byte)(curCnFontChars.IndexOf(currentChar)));
                                        byNewCnFile.Add(0x82);
                                    }
                                    else
                                    {
                                        textErr.Append(sheetName).Append(" ").Append(currentChar).Append("\r\n");
                                    }
                                }
                            }

                            if (cnToFiles.Length > 3 && cnTxtValue.EndsWith("^00A0^"))
                            {
                                int curCnTextLen = byNewCnFile.Count / 2;
                                byComnEntryInfo.Add((byte)((curCnTextLen >> 0) & 0xFF));
                                byComnEntryInfo.Add((byte)((curCnTextLen >> 8) & 0xFF));
                                byComnEntryInfo.Add((byte)((curCnTextLen >> 16) & 0xFF));
                                byComnEntryInfo.Add((byte)((curCnTextLen >> 24) & 0xFF));
                            }
                        }

                        lineNum++;
                    }

                    if (cnToFiles.Length > 3)
                    {
                        byte[] byEnrtyInfo = byComnEntryInfo.ToArray();
                        Array.Copy(byEnrtyInfo, 0, byOldCnFile, startPos, byEnrtyInfo.Length);
                    }

                    // 判断是否超长
                    //int minLen = Math.Min(byNewCnFile.Count, endTextPos - startTextPos);
                    if (byNewCnFile.Count != (endTextPos - startTextPos))
                    {
                        if (byNewCnFile.Count > (endTextPos - startTextPos))
                        {
                            Array.Resize(ref byOldCnFile, byOldCnFile.Length + (byNewCnFile.Count - (endTextPos - startTextPos)));
                        }
                        textErr.Append(sheetName).Append(" 文字个数不一致：").Append(byNewCnFile.Count - (endTextPos - startTextPos)).Append("\r\n");
                    }

                    Array.Clear(byOldCnFile, startTextPos, endTextPos - startTextPos);
                    byte[] byImpCnFile = byNewCnFile.ToArray();
                    Array.Copy(byImpCnFile, 0, byOldCnFile, startTextPos, byNewCnFile.Count);

                    File.WriteAllBytes(updateCnFile.Replace("PS_jp", "PS_cn"), byOldCnFile);
                    if (textErr.Length > 0)
                    {
                        File.WriteAllText(@"G:\Study\MySelfProject\Hanhua\Dino1\impCnTextErr.txt", textErr.ToString(), Encoding.UTF8);
                    }

                    //if (sheetName.Equals("SLPS_021.80"))
                    //{
                    //    break;
                    //}
                }
            }
            catch (Exception me)
            {
                MessageBox.Show(updateCnFile + "\r\n" + me.Message + "\r\n" + me.StackTrace);
            }
            finally
            {
                // 清空各种对象
                xSheet = null;
                xBook = null;
                if (xApp != null)
                {
                    xApp.Quit();
                    xApp = null;
                }
            }

            File.WriteAllText(@"G:\Study\MySelfProject\Hanhua\Dino1\cnTextImpResult.txt", textErr.ToString(), Encoding.UTF8);
        }

        private void btnChkCnCount_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application xApp = null;
            Microsoft.Office.Interop.Excel.Workbook xBook = null;
            Microsoft.Office.Interop.Excel.Worksheet xSheet = null;

            try
            {
                // 创建Application对象 
                xApp = new Microsoft.Office.Interop.Excel.ApplicationClass();

                // 得到WorkBook对象, 打开已有的文件 
                xBook = xApp.Workbooks._Open(
                    @"G:\Study\MySelfProject\Hanhua\Dino1\allJpText.xlsx",
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                int lineNum = 1;
                int blankNum = 0;
                Regex regex = new Regex(@"\^(.*?)\^");
                // 取得相应的Sheet
                for (int i = 1; i <= xBook.Sheets.Count; i++)
                {
                    xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets[i];
                    if (xSheet.Name.Equals("SLPS_021.80"))
                    {
                        continue;
                    }

                    xSheet.Tab.Color = ColorTranslator.ToOle(Color.Transparent);
                    lineNum = 1;
                    blankNum = 0;
                    while (blankNum < 5)
                    {
                        string oldJpTxtValue = xSheet.get_Range("A" + lineNum, Missing.Value).Value2 as string;
                        string oldCnTxtValue = xSheet.get_Range("J" + lineNum, Missing.Value).Value2 as string;

                        if (string.IsNullOrEmpty(oldCnTxtValue))
                        {
                            blankNum++;
                        }
                        else
                        {
                            blankNum = 0;
                            string cnTxtValue = Regex.Replace(oldCnTxtValue, @"\^.*?\^", "");
                            string jpTxtValue = Regex.Replace(oldJpTxtValue, @"\^.*?\^", "");

                            xSheet.get_Range("J" + lineNum, Missing.Value).Interior.Color = ColorTranslator.ToOle(Color.Transparent);
                            xSheet.get_Range("Q" + lineNum, Missing.Value).Value2 = string.Empty;
                            if (oldCnTxtValue.Length != oldJpTxtValue.Length)
                            {
                                // 检查长度
                                xSheet.Tab.Color = ColorTranslator.ToOle(Color.LightBlue);
                                xSheet.get_Range("J" + lineNum, Missing.Value).Interior.Color = ColorTranslator.ToOle(Color.LightBlue);
                                xSheet.get_Range("Q" + lineNum, Missing.Value).Value2 = oldJpTxtValue.Length + " " + oldCnTxtValue.Length;
                            }
                            else
                            {
                                // 检查关键字
                                StringBuilder result = new StringBuilder();
                                MatchCollection matches = regex.Matches(oldCnTxtValue);
                                foreach (Match match in matches)
                                {
                                    if (match.Groups[1].Success)
                                    {
                                        result.Append(match.Groups[1].Value);
                                    }
                                }

                                string cnKey = result.ToString();

                                result.Length = 0;
                                matches = regex.Matches(oldJpTxtValue);
                                foreach (Match match in matches)
                                {
                                    if (match.Groups[1].Success)
                                    {
                                        result.Append(match.Groups[1].Value);
                                    }
                                }
                                if (!cnKey.Equals(result.ToString()))
                                {
                                    xSheet.Tab.Color = ColorTranslator.ToOle(Color.LightYellow);
                                    xSheet.get_Range("J" + lineNum, Missing.Value).Interior.Color = ColorTranslator.ToOle(Color.LightYellow);
                                }
                            }
                        }

                        lineNum++;
                    }
                }

            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\r\n" + me.StackTrace);
            }
            finally
            {
                xSheet.SaveAs(
                    @"G:\Study\MySelfProject\Hanhua\Dino1\allJpText.xlsx",
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                // 清空各种对象
                xSheet = null;
                xBook = null;
                if (xApp != null)
                {
                    xApp.Quit();
                    xApp = null;
                }
            }
        }

        private void btnCompressFiles_Click(object sender, EventArgs e)
        {
            string[] allCnFileInfo = File.ReadAllLines(@"G:\Study\MySelfProject\Hanhua\Dino1\textAddr.txt", Encoding.UTF8);

            // 使用Queue和lock替代ConcurrentQueue
            Queue<string> fileQueue = new Queue<string>();
            object queueLock = new object();

            // 线程计数器
            int activeThreads = 0;

            // 使用ManualResetEvent等待所有线程完成
            ManualResetEvent allThreadsDone = new ManualResetEvent(false);

            // 1. 准备需要多线程处理的任务
            for (int i = 0; i < allCnFileInfo.Length; i += 2)
            {
                if (allCnFileInfo[i].EndsWith(".bin", StringComparison.OrdinalIgnoreCase))
                {
                    string folder = allCnFileInfo[i].Substring(0, allCnFileInfo[i].Length - 7).Replace("PS_jp", "PS_cn").ToUpper();
                    fileQueue.Enqueue(folder);
                }
            }

            // 其他需要处理的目录
            fileQueue.Enqueue(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\CLEAR".ToUpper());
            fileQueue.Enqueue(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\CORE".ToUpper());
            fileQueue.Enqueue(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\C_CHANGE".ToUpper());
            fileQueue.Enqueue(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\TITLE".ToUpper());
            fileQueue.Enqueue(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\OMAKE1".ToUpper());
            fileQueue.Enqueue(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\OMAKE2".ToUpper());
            fileQueue.Enqueue(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\OMAKE3".ToUpper());
            fileQueue.Enqueue(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\OMAKE5".ToUpper());
            fileQueue.Enqueue(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\WARNING".ToUpper());
            fileQueue.Enqueue(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\WIPE".ToUpper());
            fileQueue.Enqueue(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\WIPE1".ToUpper());
            fileQueue.Enqueue(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\WIPE2".ToUpper());
            fileQueue.Enqueue(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\WIPE3".ToUpper());

            fileQueue.Enqueue(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\ST40D".ToUpper());

            // 2. 启动工作线程（最多maxThreads个）
            int threadCount = 5;
            for (int i = 0; i < threadCount; i++)
            {
                Thread thread = new Thread(() =>
                {
                    Interlocked.Increment(ref activeThreads);

                    while (true)
                    {
                        string folder = null;
                        lock (queueLock)
                        {
                            if (fileQueue.Count > 0)
                            {
                                folder = fileQueue.Dequeue();
                            }
                        }

                        if (folder == null)
                        {
                            break;
                        }

                        try
                        {
                            string outputDat = folder + ".dat";
                            string destPath = outputDat.Replace("PS_CN", @"mkpsxiso-2.20-win64\NewDino\PSX\DATA");

                            this.CompressDatFile(folder);
                            File.Copy(outputDat, destPath, true);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }

                    if (Interlocked.Decrement(ref activeThreads) == 0)
                    {
                        allThreadsDone.Set();
                    }
                });

                thread.Start();
            }

            // 3. 等待所有多线程任务完成
            allThreadsDone.WaitOne();
        
            //string lastFolder = @"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\CORE".ToUpper();
            //this.CompressDatFile(lastFolder);

            //string lastOutputDat = lastFolder.ToUpper() + ".dat";
            //File.Copy(lastOutputDat, lastOutputDat.Replace("PS_CN", @"mkpsxiso-2.20-win64\NewDino\PSX\DATA"), true);

            //lastFolder = @"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\C_CHANGE".ToUpper();
            //this.CompressDatFile(lastFolder);

            //lastOutputDat = lastFolder.ToUpper() + ".dat";
            //File.Copy(lastOutputDat, lastOutputDat.Replace("PS_CN", @"mkpsxiso-2.20-win64\NewDino\PSX\DATA"), true);

            //lastFolder = @"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\TITLE".ToUpper();
            //this.CompressDatFile(lastFolder);

            //lastOutputDat = lastFolder.ToUpper() + ".dat";
            //File.Copy(lastOutputDat, lastOutputDat.Replace("PS_CN", @"mkpsxiso-2.20-win64\NewDino\PSX\DATA"), true);

            // 修正LBA地址
            this.btnUpdLba_Click(sender, e);
            
            MessageBox.Show("打包完成");
        }

        private void btnAddrTest_Click(object sender, EventArgs e)
        {
            List<FilePosInfo> allCnDat = Util.GetAllFiles(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn").Where(p => !p.IsFolder && p.File.EndsWith(".dat", StringComparison.OrdinalIgnoreCase)).ToList();

            foreach (FilePosInfo fileInfo in allCnDat)
            {
                byte[] byJpDat = File.ReadAllBytes(fileInfo.File.Replace("PS_cn", @"mkpsxiso-2.20-win64\DinoJpBak\PSX\DATA"));
                byte[] byCnDat = File.ReadAllBytes(fileInfo.File);
                byte[] byTmpCnDat = File.ReadAllBytes(fileInfo.File.Replace("PS_cn", @"Patch0111"));

                for (int i = 0; i < 2048; i += 16)
                {
                    if (byCnDat[i] == 0x64 && byCnDat[i + 1] == 0x75 && byCnDat[i + 2] == 0x6D && byCnDat[i + 3] == 0x6D)
                    {
                        break;
                    }

                    byCnDat[i + 15] = byTmpCnDat[i + 15];
                    byCnDat[i + 14] = byTmpCnDat[i + 14];
                    byCnDat[i + 13] = byTmpCnDat[i + 13];
                    byCnDat[i + 12] = byTmpCnDat[i + 12];

                    GEntryType type = (GEntryType)byJpDat[i];
                    if (!(type == GEntryType.GET_LZSS1 || type == GEntryType.GET_TEXTURE || type == GEntryType.GET_PALETTE))
                    {
                        byCnDat[i + 15] = byJpDat[i + 15];
                        byCnDat[i + 14] = byJpDat[i + 14];
                        byCnDat[i + 13] = byJpDat[i + 13];
                        byCnDat[i + 12] = byJpDat[i + 12];
                    }
                }

                File.WriteAllBytes(fileInfo.File, byCnDat);
            }


            //byte[] byTemp = File.ReadAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino1\Patch\CORE.dat");
            //Array.Resize(ref byTemp, byTemp.Length + 2048 * 6);
            //File.WriteAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\CORE.dat", byTemp);

            //byTemp = File.ReadAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\OPENING.dat");
            //Array.Resize(ref byTemp, byTemp.Length + 2048);
            //File.WriteAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\OPENING.dat", byTemp);

            //List<FilePosInfo> allCnDat = Util.GetAllFiles(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn").Where(p => !p.IsFolder && p.File.EndsWith(".dat", StringComparison.OrdinalIgnoreCase)).ToList();
            //StringBuilder sb = new StringBuilder();
            //StringBuilder sbAdd = new StringBuilder();
            //int totalDiff = 0;
            //foreach (FilePosInfo datFileInfo in allCnDat)
            //{
            //    sb.Append(Util.GetShortNameWithoutType(datFileInfo.File)).Append(" ");
                
            //    FileInfo fi = new FileInfo(datFileInfo.File);
            //    int newSector = ((int)fi.Length + 2047) / 2048;
            //    sb.Append(newSector).Append(" ");

            //    fi = new FileInfo(datFileInfo.File.Replace("PS_cn", @"mkpsxiso-2.20-win64\DinoJpBak\PSX\DATA"));
            //    int oldSector = ((int)fi.Length + 2047) / 2048;
            //    totalDiff += (newSector - oldSector);
            //    sb.Append(oldSector).Append(" ").Append(newSector - oldSector).Append(" ").Append(totalDiff).Append("\r\n");

            //    if (newSector - oldSector > 0)
            //    {
            //        sbAdd.Append(Util.GetShortNameWithoutType(datFileInfo.File)).Append(" ").Append(newSector - oldSector).Append("\r\n");
            //    }
            //}



            Util.IsGetAllFile = true;
            ////List<FilePosInfo> allDatFiles = Util.GetAllFiles(@"G:\Study\MySelfProject\Hanhua\Dino1\Ps_IsoJpBak\PSX").Where(p => !p.IsFolder).ToList();
            //List<FilePosInfo> allDatFiles = Util.GetAllFiles(@"G:\Study\MySelfProject\Hanhua\Dino1\mkpsxiso-2.20-win64\NewDino\PSX").Where(p => !p.IsFolder).ToList();
            //byte[] keyBytes = new byte[4];
            //StringBuilder sb = new StringBuilder();
            ////byte[] byLbaFile = File.ReadAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino1\Ps_IsoJpBak\SLPS_021.80");
            //byte[] byLbaFile = File.ReadAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino1\mkpsxiso-2.20-win64\NewDino\SLPS_021.80");
            //int startPos = 0x8172C;
            //bool xasStarted = false;
            //foreach (FilePosInfo fileInfo in allDatFiles)
            //{
            //    string jpFile = fileInfo.File;
            //    FileInfo fi = new FileInfo(jpFile);
            //    int fileSize = (int)fi.Length;

            //    keyBytes[0] = (byte)((fileSize >> 0) & 0xFF);
            //    keyBytes[1] = (byte)((fileSize >> 8) & 0xFF);
            //    keyBytes[2] = (byte)((fileSize >> 16) & 0xFF);
            //    keyBytes[3] = (byte)((fileSize >> 24) & 0xFF);

            //    sb.Append(jpFile).Append("\r\n");
            //    sb.Append(byLbaFile[startPos + 0].ToString("X2")).Append(" ");
            //    sb.Append(byLbaFile[startPos + 1].ToString("X2")).Append(" ");
            //    sb.Append(byLbaFile[startPos + 2].ToString("X2")).Append(" ");
            //    sb.Append(byLbaFile[startPos + 3].ToString("X2")).Append(" ");

            //    sb.Append(keyBytes[0].ToString("X2")).Append(" ");
            //    sb.Append(keyBytes[1].ToString("X2")).Append(" ");
            //    sb.Append(keyBytes[2].ToString("X2")).Append(" ");
            //    sb.Append(keyBytes[3].ToString("X2")).Append(" ");

            //    if (jpFile.EndsWith(".XAS", StringComparison.OrdinalIgnoreCase) || jpFile.EndsWith(".STR", StringComparison.OrdinalIgnoreCase))
            //    {
            //        Search(startPos, byLbaFile, keyBytes, sb, 2336, 0);
            //    }
            //    else
            //    {
            //        Search(startPos, byLbaFile, keyBytes, sb, 2048, 0);
            //    }

            //    startPos += 12;
            //}

            //File.WriteAllText(@"G:\Study\MySelfProject\Hanhua\Dino1\LBAChk3.txt", sb.ToString(), Encoding.UTF8);

            //byte[] byOldXa = File.ReadAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino1\Ps_IsoJpBak\PSX\DATA_XA\STAGE00.XAS");
            ////byte[] byOldXa = File.ReadAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino1\mkpsxiso-2.20-win64\DinoJpBak\PSX\DATA\WIRE502.DAT");
            //////byte[] byOldXa = File.ReadAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino1\mkpsxiso-2.20-win64\NewDino\license_data.dat");
            //int chkLen = 8;
            //byte[] keyBytes = new byte[chkLen];
            //Array.Copy(byOldXa, 0, keyBytes, 0, keyBytes.Length);
            //bool findedKey = true;

            //StringBuilder sb = new StringBuilder();
            //using (FileStream fs = new FileStream(@"G:\Study\MySelfProject\Hanhua\Dino1\mkpsxiso-2.20-win64\Dino.bin", FileMode.Open, FileAccess.Read))
            //{
            //    byte[] byData = new byte[chkLen];
            //    //fs.Position = 0x5A00000;
            //    //byte[] byTempData = new byte[0x8000 + 2048 + 4560 + 2048 - 0x5A0 + 2352];
            //    int bytesRead;
            //    //fs.Position = 0x6A4DF48 - 0x8000 - 2048 - 4560 - 2048 + 0x5A0; //0x5A00000;
            //    //fs.Position = 0x6A4DF40;
            //    fs.Position = 0x0;
            //    byte[] byTempData = new byte[1024 * 1024 * 10];

            //    //while ((bytesRead = fs.Read(byData, 0, byData.Length)) > 0)
            //    //{
            //    //    findedKey = true;
            //    //    for (int i = 0; i < byData.Length; i++)
            //    //    {
            //    //        if (byData[i] != keyBytes[i])
            //    //        {
            //    //            findedKey = false;
            //    //            break;
            //    //        }
            //    //    }

            //    //    if (findedKey)
            //    //    {
            //    //        sb.Append("old xa pos: ").Append((fs.Position - 8).ToString("X")).Append("\r\n");
            //    //        //sb.Append("old WIRE502 pos: ").Append(fs.Position.ToString("X")).Append("\r\n");
            //    //        break;
            //    //    }
            //    //}

            //    bytesRead = fs.Read(byTempData, 0, byTempData.Length);
            //    File.WriteAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino1\tempBlockO.dat", byTempData);
            //}

            ////using (FileStream fs = new FileStream(@"G:\Study\MySelfProject\Hanhua\Dino1\mkpsxiso-2.20-win64\DinoCnTst.bin", FileMode.Open, FileAccess.Read))
            //using (FileStream fs = new FileStream(@"G:\Study\MySelfProject\Hanhua\Dino1\mkpsxiso-2.20-win64\DinoCn2.bin", FileMode.Open, FileAccess.Read))
            //{
            //    byte[] byData = new byte[chkLen];
            //    //fs.Position = 0x5A00000;
            //    //byte[] byTempData = new byte[57344 + 2352 * 5];
            //    //byte[] byTempData = new byte[0x8000 + 2048 + 4560 + 2048 - 0x5A0 + 2352 + 2352];
            //    int bytesRead;
            //    //fs.Position = 0x6a4d699 - 57344 - 2352 * 4 + 0x2FF; //0x5A00000;
            //    //fs.Position = 0x6a4d699 - 0x8000 - 2048 - 4560 - 2048 + 0x5A0 + 0x8AF; //0x5A00000;
            //    //fs.Position = 0x6A47100;
            //    fs.Position = 0;
            //    byte[] byTempData = new byte[1024 * 1024 * 10];

            //    //while ((bytesRead = fs.Read(byData, 0, byData.Length)) > 0)
            //    //{
            //    //    findedKey = true;
            //    //    for (int i = 0; i < byData.Length; i++)
            //    //    {
            //    //        if (byData[i] != keyBytes[i])
            //    //        {
            //    //            findedKey = false;
            //    //            break;
            //    //        }
            //    //    }

            //    //    if (findedKey)
            //    //    {
            //    //        sb.Append("new xa pos: ").Append((fs.Position - 8).ToString("X")).Append("\r\n");
            //    //        //sb.Append("new WIRE502 pos: ").Append(fs.Position.ToString("X")).Append("\r\n");
            //    //        //sb.Append("license pos: ").Append(fs.Position.ToString("X")).Append("\r\n");
            //    //        break;
            //    //    }
            //    //}
            //    bytesRead = fs.Read(byTempData, 0, byTempData.Length);
            //    File.WriteAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino1\tempBlockN.dat", byTempData);
            //}
            ////File.WriteAllText(@"G:\Study\MySelfProject\Hanhua\Dino1\xaPosChk.txt", sb.ToString(), Encoding.UTF8);

            ////File.WriteAllText(@"G:\Study\MySelfProject\Hanhua\Dino1\WIRE502PosChk.txt", sb.ToString(), Encoding.UTF8);
            //////File.WriteAllText(@"G:\Study\MySelfProject\Hanhua\Dino1\licensePosChk.txt", sb.ToString(), Encoding.UTF8);
        }

        private bool Search(int startPos, byte[] byData, byte[] keyBytes, StringBuilder sb, int sectorSize, int addSectorCnt)
        {
            // 二进制检索
            bool findedKey = true;
            int maxLen = byData.Length - 12;

            for (int j = startPos + 4; j < maxLen; j += 12)
            {
                if (byData[j] == keyBytes[0])
                {
                    findedKey = true;
                    for (int i = 1; i < keyBytes.Length; i++)
                    {
                        if (byData[j + i] != keyBytes[i])
                        {
                            findedKey = false;
                            break;
                        }
                    }

                    if (findedKey)
                    {
                        if (j == (startPos + 4))
                        {
                            sb.Append(j.ToString("X")).Append(" OK ");

                            int startSector = byData[startPos + 0] + (byData[startPos + 1] << 8) + (byData[startPos + 2] << 16) + (byData[startPos + 3] << 24);
                            int size = keyBytes[0] + (keyBytes[1] << 8) + (keyBytes[2] << 16) + (keyBytes[3] << 24);
                            int nextSector = 0;
                            nextSector = startSector + (size + (sectorSize - 1)) / sectorSize + addSectorCnt;
                            sb.Append(((nextSector >> 0) & 0xFF).ToString("X2")).Append(" ");
                            sb.Append(((nextSector >> 8) & 0xFF).ToString("X2")).Append(" ");
                            sb.Append(((nextSector >> 16) & 0xFF).ToString("X2")).Append(" ");
                            sb.Append(((nextSector >> 24) & 0xFF).ToString("X2")).Append(" ");
                        }
                        else
                        {
                            sb.Append(j.ToString("X")).Append(" NG");
                        }
                        
                        j += keyBytes.Length - 1;
                        break;
                    }
                }
            }
            sb.Append("\r\n");

            return true;
        }

        private void btnUpdLba_Click(object sender, EventArgs e)
        {
            Util.IsGetAllFile = true;
            List<FilePosInfo> allCnDat = Util.GetAllFiles(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn").Where(p => !p.IsFolder && 
                (p.File.EndsWith(".dat", StringComparison.OrdinalIgnoreCase) || p.File.EndsWith(".str", StringComparison.OrdinalIgnoreCase))).ToList();

            string cnExe = @"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\SLPS_021.80";
            byte[] bySlps = File.ReadAllBytes(cnExe);
            byte[] byOldSlps = File.ReadAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino1\mkpsxiso-2.20-win64\DinoJpBak\SLPS_021.80");
            Array.Copy(byOldSlps, 0x81730, bySlps, 0x81730, 0x82DEC - 0x81730);

            string[] allAddrInfo = File.ReadAllLines(@"G:\Study\MySelfProject\Hanhua\Dino1\LBAChk.txt", Encoding.UTF8);
            bool startChgPos = false;
            int sectorDiff = 0;
            int totalDiff = 0;

            bool firstXs = true;
            bool firstMov = true;

            for (int i = 0; i < allAddrInfo.Length; i += 2)
            {
                if (allAddrInfo[i].EndsWith(".dat", StringComparison.OrdinalIgnoreCase) || allAddrInfo[i].EndsWith(".str", StringComparison.OrdinalIgnoreCase) || startChgPos)
                {
                    if (allAddrInfo[i].EndsWith(".XAS", StringComparison.OrdinalIgnoreCase) && firstXs)
                    {
                        //totalDiff += 2;
                        firstXs = false;
                    }
                    else if (allAddrInfo[i].EndsWith(".STR", StringComparison.OrdinalIgnoreCase))
                    {
                        //totalDiff += 2;
                        firstMov = false;
                    }

                    string datName = Util.GetShortName(allAddrInfo[i]);
                    string[] allSizeInfo = allAddrInfo[i + 1].Split(' ');
                    int oldSizePos = Convert.ToInt32(allSizeInfo[4], 16);
                    int sizeInfo = (Convert.ToInt32(allSizeInfo[3], 16) << 24) +
                        (Convert.ToInt32(allSizeInfo[2], 16) << 16) +
                        (Convert.ToInt32(allSizeInfo[1], 16) << 8) +
                        (Convert.ToInt32(allSizeInfo[0], 16) << 0);

                    int startSector = (bySlps[oldSizePos - 4] << 0) + (bySlps[oldSizePos - 3] << 8) + (bySlps[oldSizePos - 2] << 16) + (bySlps[oldSizePos - 1] << 24);
                    sectorDiff = 0;

                    FilePosInfo curCnDat = allCnDat.FirstOrDefault(p => p.File.EndsWith(datName, StringComparison.OrdinalIgnoreCase));
                    if (curCnDat != null)
                    {
                        FileInfo fi = new FileInfo(curCnDat.File);
                        if (fi.Length != sizeInfo)
                        {
                            sectorDiff = (((int)fi.Length + 2047) / 2048) - ((sizeInfo + 2047) / 2048);
                            startChgPos = true;
                            sizeInfo = (int)fi.Length;
                        }
                    }

                    if (startChgPos)
                    {
                        startSector += totalDiff;
                        bySlps[oldSizePos - 4] = (byte)((startSector >> 0) & 0xFF);
                        bySlps[oldSizePos - 3] = (byte)((startSector >> 8) & 0xFF);
                        bySlps[oldSizePos - 2] = (byte)((startSector >> 16) & 0xFF);
                        bySlps[oldSizePos - 1] = (byte)((startSector >> 24) & 0xFF);

                        bySlps[oldSizePos + 0] = (byte)((sizeInfo >> 0) & 0xFF);
                        bySlps[oldSizePos + 1] = (byte)((sizeInfo >> 8) & 0xFF);
                        bySlps[oldSizePos + 2] = (byte)((sizeInfo >> 16) & 0xFF);
                        bySlps[oldSizePos + 3] = (byte)((sizeInfo >> 24) & 0xFF);

                        totalDiff += sectorDiff;
                    }
                }
            }

            //// 最后XA部分的对齐
            //if (totalDiff < 0)
            //{
            //    string lastFile = @"G:\Study\MySelfProject\Hanhua\Dino1\mkpsxiso-2.20-win64\NewDino\PSX\DATA\WIRE502.DAT";
            //    byte[] byLastFile = File.ReadAllBytes(lastFile);
            //    int startSector = bySlps[0x82d40 + 0] + (bySlps[0x82d40 + 1] << 8) + (bySlps[0x82d40 + 2] << 16) + (bySlps[0x82d40 + 3] << 24);
            //    int endSector = 0xB920;

            //    int lastFileSize = (endSector - startSector) * 2048;

            //    Array.Resize(ref byLastFile, lastFileSize);
            //    File.WriteAllBytes(lastFile, byLastFile);

            //    bySlps[0x82d44 + 0] = (byte)((lastFileSize >> 0) & 0xFF);
            //    bySlps[0x82d44 + 1] = (byte)((lastFileSize >> 8) & 0xFF);
            //    bySlps[0x82d44 + 2] = (byte)((lastFileSize >> 16) & 0xFF);
            //    bySlps[0x82d44 + 3] = (byte)((lastFileSize >> 24) & 0xFF);
            //}

            // 使用、调查、整理的居中
            //bySlps[0xd65] = 0x10;
            //bySlps[0xd67] = 0x10;

            File.WriteAllBytes(cnExe, bySlps);

            File.Copy(cnExe, @"G:\Study\MySelfProject\Hanhua\Dino1\mkpsxiso-2.20-win64\NewDino\SLPS_021.80", true);
        }

        private void btnTestOtherPic_Click(object sender, EventArgs e)
        {
            ////string pointStr = "前往的目的地";
            ////string pointStr = "现在位置";
            //string pointStr = "记录点";
            //List<string> txtList = pointStr.ToCharArray().Select(c => c.ToString()).ToList();
            //ImgInfo imgInfo = new ImgInfo(64, 8);
            //imgInfo.BlockImgH = 8;
            //imgInfo.BlockImgW = 8;
            //imgInfo.PosX = 0;
            //imgInfo.PosY = 0;
            //imgInfo.XPadding = 0;
            //imgInfo.YPadding = 0;
            //imgInfo.NeedBorder = false;
            //imgInfo.FontSize = 6f;
            //imgInfo.FontStyle = FontStyle.Regular;
            //imgInfo.FontName = "GuanZhi 8px";
            //imgInfo.Brush = Brushes.White;
            //imgInfo.Sf.LineAlignment = StringAlignment.Center;
            //imgInfo.Grp.Clear(Color.Black);
            //imgInfo.Grp.SmoothingMode = SmoothingMode.None;
            //imgInfo.Grp.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            //imgInfo.Grp.DrawString(pointStr, new Font(new FontFamily(imgInfo.FontName), imgInfo.FontSize, imgInfo.FontStyle), imgInfo.Brush,
            //    new Rectangle(0,
            //        0,
            //        64,
            //        8));

            //imgInfo.Bmp.Save(@"G:\Study\MySelfProject\Hanhua\Dino1\CmnPicTest\point3.png");

            List<Color> pciPal = new List<Color>();
            byte[] byPal = File.ReadAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\CORE\05.pal");
            for (int i = 0x220; i < 0x240; i += 2)
            {
                int pixelColor = byPal[i] + (byPal[i + 1] << 8);
                int colorR = ((byte)(pixelColor & 0x1F)) << 3;
                int colorG = ((byte)((pixelColor >> 5) & 0x1F)) << 3;
                int colorB = ((byte)((pixelColor >> 10) & 0x1F)) << 3;
                pciPal.Add(Color.FromArgb(colorR, colorG, colorB));
            }

            byte[] byCore = File.ReadAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\CORE\06.tex");
            Bitmap menuImg = (Bitmap)Image.FromFile(@"G:\Study\MySelfProject\Hanhua\Dino1\CmnPicTest2\menuNew\point1.png");
            int coreIdx = 176 / 2;
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 56; x += 2)
                {
                    byCore[coreIdx++] = (byte)(pciPal.IndexOf(menuImg.GetPixel(x, y)) | (pciPal.IndexOf(menuImg.GetPixel(x + 1, y)) << 4));
                }
                coreIdx += 100;
            }

            menuImg = (Bitmap)Image.FromFile(@"G:\Study\MySelfProject\Hanhua\Dino1\CmnPicTest2\menuNew\point2.png");
            coreIdx = 176 / 2 + (8 * 128);
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 56; x += 2)
                {
                    byCore[coreIdx++] = (byte)(pciPal.IndexOf(menuImg.GetPixel(x, y)) | (pciPal.IndexOf(menuImg.GetPixel(x + 1, y)) << 4));
                }
                coreIdx += 100;
            }

            menuImg = (Bitmap)Image.FromFile(@"G:\Study\MySelfProject\Hanhua\Dino1\CmnPicTest2\menuNew\point3.png");
            coreIdx = 160 / 2 + (112 * 128);
            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 40; x += 2)
                {
                    if (y >= 8)
                    {
                        byCore[coreIdx++] = 0;
                    }
                    else
                    {
                        byCore[coreIdx++] = (byte)(pciPal.IndexOf(menuImg.GetPixel(x, y)) | (pciPal.IndexOf(menuImg.GetPixel(x + 1, y)) << 4));
                    }
                }
                coreIdx += 108;
            }

            File.WriteAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\CORE\06.tex", byCore);
        }

        private void MakeMovieSubTitle()
        {
            int idx = 0;
            List<string> cnImg = new List<string>();
            while (idx <= 11)
            {
                cnImg.Add(@"G:\Study\MySelfProject\Hanhua\Dino1\TempChk\Ps_subTitle\CN\ENDING" + idx.ToString().PadLeft(2, '0') + ".png");
                idx++;
            }
            this.CompressSubTitle(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\ENDING\02.bin", 0x0, 0, cnImg, "ENDING", "0 8d8 1300 1c2c 2494 2650 2868 30f0 36b0 3ec0 45d0 4b94");

            cnImg.Clear();
            idx = 0;
            while (idx <= 9)
            {
                cnImg.Add(@"G:\Study\MySelfProject\Hanhua\Dino1\TempChk\Ps_subTitle\CN\OPENING" + idx.ToString().PadLeft(2, '0') + ".png");
                idx++;
            }
            this.CompressSubTitle(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\OPENING\16.bin", 0x3fd8, 0, cnImg, "OPENING", "3fd8 45ac 4d74 5744 592c 5dc8 62e8 662c 698c 6f98");

            cnImg.Clear();
            idx = 0;
            while (idx <= 8)
            {
                cnImg.Add(@"G:\Study\MySelfProject\Hanhua\Dino1\TempChk\Ps_subTitle\CN\ST40D" + idx.ToString().PadLeft(2, '0') + ".png");
                idx++;
            }
            this.CompressSubTitle(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\ST40D\00.bin", 0x1c, 0x3008, cnImg, "ST40D", "1c 45c 758 1154 1af0 20a4 224c 2820 2cf0");

            this.DecompressSubTitle(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\ENDING\02.bin", 0x0, "ENDING", "0 8d8 1300 1c2c 2494 2650 2868 30f0 36b0 3ec0 45d0 4b94");
            //this.DecompressSubTitle(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_jp\ENDING\02.bin", 0x0, "ENDING_JP");
            this.DecompressSubTitle(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\OPENING\16.bin", 0x3fd8, "OPENING", "3fd8 45ac 4d74 5744 592c 5dc8 62e8 662c 698c 6f98");
            //this.DecompressSubTitle(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_jp\OPENING\16.bin", 0x3fd8, "OPENING_JP");
            this.DecompressSubTitle(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\ST40D\00.bin", 0x1c, "ST40D", "1c 45c 758 1154 1af0 20a4 224c 2820 2cf0");
            //this.DecompressSubTitle(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_jp\ST40D\00.bin", 0x1c, "ST40D_JP");

            //Font boldFont = new Font("SimSun", 13, FontStyle.Bold, GraphicsUnit.Pixel);
            //StringFormat sf = new StringFormat
            //{
            //    Alignment = StringAlignment.Near,
            //    LineAlignment = StringAlignment.Center,
            //    FormatFlags = StringFormatFlags.NoClip
            //};

            //string[] titles = File.ReadAllLines(@"G:\Study\MySelfProject\Hanhua\Dino1\TempChk\Ps_subTitle\subTitle.txt");
            //for (int i = 0; i < titles.Length; i += 3)
            //{
            //    string imgPath = @"G:\Study\MySelfProject\Hanhua\Dino1\TempChk\Ps_subTitle\" + titles[i];
            //    Bitmap oldImg = (Bitmap)Image.FromFile(imgPath + ".png");

            //    Bitmap newImg = new Bitmap(oldImg.Width, oldImg.Height, PixelFormat.Format32bppArgb);
                

            //    using (Graphics g = Graphics.FromImage(newImg))
            //    {
            //        // 背景
            //        g.Clear(Color.Black);

            //        // 关键设置
            //        g.SmoothingMode = SmoothingMode.HighQuality;
            //        g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            //        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            //        //g.SmoothingMode = SmoothingMode.None;
            //        //g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
            //        //g.InterpolationMode = InterpolationMode.NearestNeighbor;
            //        //g.PixelOffsetMode = PixelOffsetMode.Half;

            //        Graphics graphics = this.CreateGraphics();
            //        SizeF sizeF = graphics.MeasureString(titles[i + 1], boldFont);
            //        float width1 = sizeF.Width;
            //        float width2 = 0f;
            //        if (!string.IsNullOrEmpty(titles[i + 2]))
            //        {
            //            sizeF = graphics.MeasureString(titles[i + 2], boldFont);
            //            width2 = sizeF.Width;
            //        }
            //        float startPos = 0f;
            //        float totalWidth = 0f;
            //        if (width1 >= width2)
            //        {
            //            startPos = (oldImg.Width - width1) / 2;
            //            totalWidth = width1;
            //        }
            //        else
            //        {
            //            startPos = (oldImg.Width - width2) / 2;
            //            totalWidth = width2;
            //        }

            //        if (startPos < 0)
            //        {
            //            startPos = 0;
            //        }
            //        RectangleF rectangle = new RectangleF(startPos, 2, totalWidth, 14);
            //        g.DrawString(titles[i + 1], boldFont, Brushes.White, rectangle, sf);
            //        if (!string.IsNullOrEmpty(titles[i + 2]))
            //        {
            //            rectangle = new RectangleF(startPos, 16, totalWidth, 14);
            //            g.DrawString(titles[i + 2], boldFont, Brushes.White, rectangle, sf);
            //        }

            //        newImg.Save(imgPath.Replace("Ps_subTitle", @"Ps_subTitle\CN") + ".png");
            //    }
            //}
        }

        private void CompressSubTitle(string path, int startPos, int endPos, List<string> cnImg, string outName, string addInfo)
        {
            byte[] byTitle = File.ReadAllBytes(path);
            if (endPos == 0)
            {
                endPos = byTitle.Length;
            }
            Array.Clear(byTitle, startPos, endPos - startPos);

            string[] adds = addInfo.Split(' ');
            bool chkFirstColor = true;
            byte lastData = 0;
            byte dataCnt = 0;
            List<byte> byImg = new List<byte>();
            List<byte> byOldImg = new List<byte>();
            int imgIdx = 0;
            for (int i = 0; i < cnImg.Count; i++)
            {
                byImg.Clear();
                byOldImg.Clear();
                Bitmap img = (Bitmap)Image.FromFile(cnImg[i]);
                Color color;
                bool lastColorAdded = false;
                chkFirstColor = true;
                dataCnt = 0;
                lastData = 0;
                for (int y = 0; y < img.Height; y++)
                {
                    for (int x = 0; x < img.Width; x++)
                    {
                        lastColorAdded = false;
                        color = img.GetPixel(x, y);
                        byOldImg.Add(color.R);
                        byOldImg.Add(color.G);
                        byOldImg.Add(color.B);

                        if (color.R != lastData && chkFirstColor == false)
                        {
                            byImg.Add(lastData);
                            byImg.Add(dataCnt);

                            lastData = color.R;
                            dataCnt = 3;
                            lastColorAdded = false;
                        }
                        else
                        {
                            chkFirstColor = false;
                            lastData = color.R;
                            dataCnt += 3;

                            if (dataCnt == 255)
                            {
                                byImg.Add(lastData);
                                byImg.Add(dataCnt);

                                lastData = color.R;
                                dataCnt = 0;
                                chkFirstColor = true;
                                lastColorAdded = true;
                            }
                        }
                    }
                }

                if (!lastColorAdded)
                {
                    byImg.Add(lastData);
                    byImg.Add(dataCnt);
                }

                startPos = Convert.ToInt16(adds[i], 16);
                int curImgLen = byImg.Count();
                byTitle[startPos] = (byte)((curImgLen >> 0) & 0xFF);
                byTitle[startPos + 1] = (byte)((curImgLen >> 8) & 0xFF);
                byTitle[startPos + 2] = 0;
                byTitle[startPos + 3] = 0;
                Array.Copy(byImg.ToArray(), 0, byTitle, startPos + 4, curImgLen);

                //File.WriteAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino1\TempChk\Ps_subTitle\" + outName + "_" + (imgIdx++) + "_CN" + (startPos.ToString("x")) + ".bin", byOldImg.ToArray());

                //startPos = startPos + 4 + curImgLen;
                //if ((startPos & 2) > 0)
                //{
                //    byTitle[startPos] = 0;
                //    byTitle[startPos + 1] = 0;
                //    startPos += 2;
                //}
            }

            //if (startPos < endPos)
            //{
            //    Array.Clear(byTitle, startPos, endPos - startPos);
            //}
            //else
            //{
            //}

            File.WriteAllBytes(path, byTitle);
        }

        private void DecompressSubTitle(string path, int startPos, string outName, string addInfo)
        {
            string subTitle = path;
            byte[] byTitle = File.ReadAllBytes(subTitle);
            List<byte> listImg = new List<byte>();
            int imgIdx = 0;
            string[] adds = addInfo.Split(' ');
            StringBuilder sb = new StringBuilder();
            //for (int i = startPos; i < byTitle.Length; )
            for (int i = 0; i < adds.Length; i++)
            {
                startPos = Convert.ToInt16(adds[i], 16);
                listImg.Clear();
                if (byTitle[startPos] == 0 && byTitle[startPos + 1] == 0)
                {
                    break;
                }

                int sectorImgSize = byTitle[startPos] + (byTitle[startPos + 1] << 8) + (byTitle[startPos + 2] << 16) + (byTitle[startPos + 3] << 24);
                int endSector = startPos + 4 + sectorImgSize;
                for (int j = startPos + 4; j < endSector; j += 2)
                {
                    int loopCount = byTitle[j + 1];
                    while (loopCount-- > 0)
                    {
                        listImg.Add(byTitle[j]);
                    }
                }

                //File.WriteAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino1\TempChk\Ps_subTitle\" + outName + "_" + (imgIdx++) + "_" + (i.ToString("x")) + ".bin", listImg.ToArray());
                //sb.Append(i.ToString("x")).Append(" ");

                //i = ((endSector + 3) / 4) * 4;

                int imgHeight = 28;
                int imgWidth = listImg.Count / 3 / imgHeight;

                Bitmap img = new Bitmap(imgWidth, imgHeight);
                int pixIdx = 0;
                for (int y = 0; y < imgHeight; y++)
                {
                    for (int x = 0; x < imgWidth; x++)
                    {
                        if ((pixIdx + 2) < listImg.Count)
                        {
                            img.SetPixel(x, y, Color.FromArgb(listImg[pixIdx], listImg[pixIdx + 1], listImg[pixIdx + 2]));
                            pixIdx += 3;
                        }
                    }
                }
                
                img.Save(@"G:\Study\MySelfProject\Hanhua\Dino1\TempChk\Ps_subTitle\" + outName + (imgIdx++) + ".png");
            }
            //File.WriteAllText(@"G:\Study\MySelfProject\Hanhua\Dino1\TempChk\Ps_subTitle\" + outName + "Addr.txt", sb.ToString());
        }


        private void btnMyTest_Click(object sender, EventArgs e)
        {
            Util.IsGetAllFile = true;
            //Directory.CreateDirectory(@"D:\OneDrive-SYSHD\OneDrive - 株式会社SYSホールディングス\龙雪蓉D盘\2.西安裕日\公司財務\ひみつ\工资\2026\1月\");
            //List<FilePosInfo> allDatFiles = Util.GetAllFiles(@"G:\Study\MySelfProject\Hanhua\Dino1\TempChk").Where(p => !p.IsFolder).ToList();
            //StringBuilder sb = new StringBuilder();
            //foreach (FilePosInfo fileInfo in allDatFiles)
            //{
            //    FileInfo fi = new FileInfo(fileInfo.File);
            //    if (fi.Length >= 50 * 1024 && (fileInfo.File.EndsWith(".bin", StringComparison.OrdinalIgnoreCase) || fileInfo.File.EndsWith(".tex", StringComparison.OrdinalIgnoreCase)))
            //    {
            //        sb.Append(fileInfo.File).Append("\r\n");
            //    }
            //}

            this.MakeMovieSubTitle();

            //// 写WARNING图片文字 Start
            //Bitmap wipe = (Bitmap)Image.FromFile(@"G:\Study\MySelfProject\Hanhua\Dino1\CmnPicTest2\blank\WARNING.png");
            //Color fillColor = Color.FromArgb(255, 248, 248, 248);
            ////Color fillColor = Color.FromArgb(255, (0x5AD6 & 0x1F) << 3, (0x5AD6 & 0x1F) << 3, (0x5AD6 & 0x1F) << 3);
            //Color outlineColor = Color.FromArgb(255, 128, 128, 128);

            //Bitmap dst = new Bitmap(
            //    wipe.Width,
            //    wipe.Height,
            //    PixelFormat.Format32bppArgb);

            //using (Graphics g = Graphics.FromImage(dst))
            //using (Brush outlineBrush = new SolidBrush(outlineColor))
            //using (Brush fillBrush = new SolidBrush(fillColor))
            //{
            //    g.DrawImage(wipe, 0, 0, wipe.Width, wipe.Height);

            //    g.SmoothingMode = SmoothingMode.HighQuality;
            //    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            //    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //    g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            //    Rectangle r = new Rectangle(98, 100, 134, 16);
            //    StringFormat sf = new StringFormat
            //    {
            //        Alignment = StringAlignment.Center,
            //        LineAlignment = StringAlignment.Center,
            //        FormatFlags = StringFormatFlags.NoClip
            //    };
            //    Font font = new Font("SimSun", 13.5f, FontStyle.Regular, GraphicsUnit.Pixel);


            //    //g.DrawString("夺回柯克博士！", font, fillBrush, r, sf);

            //    string title1 = "本游戏包含暴力场景";

            //    DrawChar(g, title1, font, outlineBrush, r, sf, -1f, 0);
            //    DrawChar(g, title1, font, outlineBrush, r, sf, 1f, 0);
            //    DrawChar(g, title1, font, outlineBrush, r, sf, 0, 1f);
            //    DrawChar(g, title1, font, fillBrush, r, sf, 0, 0);

            //    Rectangle r2 = new Rectangle(98, 118, 134, 16);
            //    string title2 = "以及血腥、猎奇内容";

            //    DrawChar(g, title2, font, outlineBrush, r2, sf, -1f, 0);
            //    DrawChar(g, title2, font, outlineBrush, r2, sf, 1f, 0);
            //    DrawChar(g, title2, font, outlineBrush, r2, sf, 0, 1f);
            //    DrawChar(g, title2, font, fillBrush, r2, sf, 0, 0);
            //}
            //dst.Save(@"G:\Study\MySelfProject\Hanhua\Dino1\CmnPicTest2\Old\WARNING.png");
            //// 写WARNING图片文字 End

            //byte[] compressed = File.ReadAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino1\TempChk\TITLE\05.bin");
            //List<byte> allData = new List<byte>();
            //int curVal = 0;
            //for (int i = 0x2d70; i < compressed.Length; i += 8)
            //{
            //    if (compressed[i + 4] > 0)
            //    {
            //        curVal = compressed[i + 4];
            //        while (curVal-- > 0)
            //        {
            //            allData.Add(0);
            //            allData.Add(compressed[i + 5]);
            //        }
            //    }
            //    else
            //    {
            //        allData.Add(0);
            //        allData.Add(compressed[i + 5]);
            //    }

            //    if (compressed[i + 6] > 0)
            //    {
            //        curVal = compressed[i + 6];
            //        while (curVal-- > 0)
            //        {
            //            allData.Add(0);
            //            allData.Add(compressed[i + 7]);
            //        }
            //    }
            //    else
            //    {
            //        allData.Add(0);
            //        allData.Add(compressed[i + 7]);
            //    }
            //}
            //File.WriteAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino1\TempChk\testMovie2.bin", allData.ToArray());

            //sb.Length = 0;
            //byte[] compressed = File.ReadAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino1\TempChk\ST701\10.bin");
            //var output = new List<byte>();

            //int pos = 0;
            //while (pos < compressed.Length)
            //{
            //    byte control = compressed[pos++];
            //    if (pos >= compressed.Length)
            //    {
            //        output.Add(control);
            //    }
                    

            //    int count = control & 0x7F;
            //    bool isRle = (control & 0x80) != 0;

            //    byte value = compressed[pos++];

            //    if (isRle)
            //    {
            //        for (int i = 0; i < count; i++)
            //        {
            //            output.Add(value);
            //            output.Add(0x80);
            //        }
            //    }
            //    else
            //    {
            //        // 理论上的 literal（你这个文件里可能没用）
            //        output.Add(value);
            //        output.Add(0x80);
            //    }
            //}
            //File.WriteAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino1\TempChk\ST701\10d.bin", output.ToArray());


            //byte[] keyBytes = new byte[16];
            //StringBuilder sb = new StringBuilder();
            //byte[] byMsgFile = File.ReadAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino Crisis (Japan)_DC\Jp\OP_MSG.DAT");
            //Array.Copy(byMsgFile, 0x0, keyBytes, 0, keyBytes.Length);

            //foreach (FilePosInfo fileInfo in allDatFiles)
            //{
            //    // 二进制检索
            //    bool findedKey = true;
            //    byte[] byData = File.ReadAllBytes(fileInfo.File);
            //    //if (byData.Length < 1024 * 100)
            //    //{
            //    //    continue;
            //    //}
            //    int maxLen = byData.Length - keyBytes.Length;

            //    for (int j = 0; j < maxLen; j += 4)
            //    {
            //        if (byData[j] == keyBytes[0])
            //        {
            //            findedKey = true;
            //            for (int i = 1; i < keyBytes.Length; i++)
            //            {
            //                if (byData[j + i] != keyBytes[i])
            //                {
            //                    findedKey = false;
            //                    break;
            //                }
            //            }

            //            if (findedKey)
            //            {
            //                sb.Append(fileInfo.File).Append("  ");
            //                sb.Append(j.ToString("X")).Append("\r\n");
            //                break;
            //            }
            //        }
            //    }
            //}

            //return;
            //List<FilePosInfo> allCnPic1 = Util.GetAllFiles(@"G:\Study\MySelfProject\Hanhua\Dino1\Dino1FontPic\FontPicCn\old1Out\");
            //foreach (FilePosInfo file in allCnPic1)
            //{
            //    if (file.IsFolder)
            //    {
            //        continue;
            //    }
            //    Bitmap impOut = (Bitmap)Image.FromFile(file.File);
            //    Bitmap impIn = (Bitmap)Image.FromFile(file.File.Replace("old1Out", "old1"));
            //    for (int y = 0; y < impIn.Height; y++)
            //    {
            //        for (int x = 0; x < impIn.Width; x++)
            //        {
            //            if (impIn.GetPixel(x, y).R > 0)
            //            {
            //                impOut.SetPixel(x, y, impIn.GetPixel(x, y));
            //            }
            //        }
            //    }

            //    impOut.Save(file.File.Replace("old1Out", "reduceColor1"));
            //}

            //allCnPic1 = Util.GetAllFiles(@"G:\Study\MySelfProject\Hanhua\Dino1\Dino1FontPic\FontPicCn\old2Out\");
            //foreach (FilePosInfo file in allCnPic1)
            //{
            //    if (file.IsFolder)
            //    {
            //        continue;
            //    }
            //    Bitmap impOut = (Bitmap)Image.FromFile(file.File);
            //    Bitmap impIn = (Bitmap)Image.FromFile(file.File.Replace("old2Out", "old2"));
            //    for (int y = 0; y < impIn.Height; y++)
            //    {
            //        for (int x = 0; x < impIn.Width; x++)
            //        {
            //            if (impIn.GetPixel(x, y).R > 0)
            //            {
            //                impOut.SetPixel(x, y, impIn.GetPixel(x, y));
            //            }
            //        }
            //    }

            //    impOut.Save(file.File.Replace("old2Out", "reduceColor2"));
            //}

            //byte[] byBap = File.ReadAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino1\Dino1FontPic\FontPicCn\fixPal80.bap");
            //int bapIdx = 0;
            //List<Color> pciPal = new List<Color>();
            //byte[] byPal = File.ReadAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\CORE\05.pal");
            //for (int i = 0x200; i < 0x460; i += 2)
            ////for (int i = 0x220; i < 0x240; i += 2)
            //{
            //    int pixelColor = byPal[i] + (byPal[i + 1] << 8);
            //    int colorR = ((byte)(pixelColor & 0x1F)) << 3;
            //    int colorG = ((byte)((pixelColor >> 5) & 0x1F)) << 3;
            //    int colorB = ((byte)((pixelColor >> 10) & 0x1F)) << 3;
            //    pciPal.Add(Color.FromArgb(colorR, colorG, colorB));

            //    //if (bapIdx < 16)
            //    //{
            //    //byBap[0x10 + bapIdx + 0] = (byte)colorR;
            //    //byBap[0x10 + bapIdx + 1] = (byte)colorG;
            //    //byBap[0x10 + bapIdx + 2] = (byte)colorB;
            //    //byBap[0x10 + bapIdx + 3] = 0xFF;
            //    //bapIdx += 4;
            //    //}
            //}
            //File.WriteAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino1\Dino1FontPic\FontPicCn\fixPalMenu.bap", byBap);
            //pciPal.Add(Color.FromArgb(0, 0, 0));
            //pciPal.Add(Color.FromArgb(0, 0, 0));
            //pciPal.Add(Color.FromArgb(0, 0, 0));
            //pciPal.Add(Color.FromArgb(0, 0, 0));

            //int pixelColor;
            //int colorR;
            //int colorG;
            //int colorB;
            //pixelColor = 0x0842;
            //colorR = ((byte)(pixelColor & 0x1F)) << 3;
            //colorG = ((byte)((pixelColor >> 5) & 0x1F)) << 3;
            //colorB = ((byte)((pixelColor >> 10) & 0x1F)) << 3;
            //pciPal.Add(Color.FromArgb(colorR, colorG, colorB));
            //pciPal.Add(Color.FromArgb(colorR, colorG, colorB));
            //pciPal.Add(Color.FromArgb(colorR, colorG, colorB));
            //pciPal.Add(Color.FromArgb(colorR, colorG, colorB));

            //pixelColor = 0x318C;
            //colorR = ((byte)(pixelColor & 0x1F)) << 3;
            //colorG = ((byte)((pixelColor >> 5) & 0x1F)) << 3;
            //colorB = ((byte)((pixelColor >> 10) & 0x1F)) << 3;
            //pciPal.Add(Color.FromArgb(colorR, colorG, colorB));
            //pciPal.Add(Color.FromArgb(colorR, colorG, colorB));
            //pciPal.Add(Color.FromArgb(colorR, colorG, colorB));
            //pciPal.Add(Color.FromArgb(colorR, colorG, colorB));

            //pixelColor = 0x5AD6;
            //colorR = ((byte)(pixelColor & 0x1F)) << 3;
            //colorG = ((byte)((pixelColor >> 5) & 0x1F)) << 3;
            //colorB = ((byte)((pixelColor >> 10) & 0x1F)) << 3;
            //pciPal.Add(Color.FromArgb(colorR, colorG, colorB));
            //pciPal.Add(Color.FromArgb(colorR, colorG, colorB));
            //pciPal.Add(Color.FromArgb(colorR, colorG, colorB));
            //pciPal.Add(Color.FromArgb(colorR, colorG, colorB));

            byte[] byImg = File.ReadAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\CORE\06.tex");
            ////////byte[] byImg = File.ReadAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino1\CmnPicTest\TITLE_Cn2.png");
            ////////Bitmap img = (Bitmap)Image.FromFile(@"G:\Study\MySelfProject\Hanhua\Dino1\CmnPicTest\TITLE_Cn2.png");
            //////byte[] byImg = File.ReadAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino1\TempChk\OP_MSG.DAT");

            //int imgWidth = 256;
            //int imgHeight = 432;
            ////Bitmap img = new Bitmap(imgWidth, imgHeight);
            ////int imgIdx = 0;
            ////for (int y = 0; y < imgHeight; y++)
            ////{
            ////    //for (int x = 0; x < imgWidth; x += 2)
            ////    for (int x = 0; x < imgWidth; )
            ////    {
            ////        img.SetPixel(x++, y, pciPal[byImg[imgIdx++]]);
            ////        //img.SetPixel(x++, y, pciPal[byImg[imgIdx] & 0xF]);
            ////        //img.SetPixel(x++, y, pciPal[(byImg[imgIdx++] >> 4) & 0xF]);
            ////        //byImg[imgIdx++] = (byte)(pciPal.IndexOf(img.GetPixel(x, y)) | (pciPal.IndexOf(img.GetPixel(x + 1, y)) << 4));
            ////    }
            ////}
            //////File.WriteAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\TITLE\00.tex", byImg);
            ////img.Save(@"G:\Study\MySelfProject\Hanhua\Dino1\CmnPicTest\CnTest\WIPE_Test.png");

            //int imgIdx = 0;
            //for (int i = 0; i < 10; i++)
            //{
            //    Bitmap img = new Bitmap(imgWidth, imgHeight);
            //    imgIdx = 0;
            //    for (int y = 0; y < imgHeight; y++)
            //    {
            //        for (int x = 0; x < imgWidth; x += 2)
            //        {
            //            if (imgIdx < byImg.Length)
            //            {
            //                int palIdx = byImg[imgIdx++];
            //                img.SetPixel(x, y, pciPal[(palIdx & 0xF) + i * 16]);
            //                img.SetPixel(x + 1, y, pciPal[((palIdx >> 4) & 0xF) + i * 16]);
            //                //int pixelColor = byImg[imgIdx] + (byImg[imgIdx + 1] << 8);
            //                //int colorR = ((byte)(pixelColor & 0x1F)) << 3;
            //                //int colorG = ((byte)((pixelColor >> 5) & 0x1F)) << 3;
            //                //int colorB = ((byte)((pixelColor >> 10) & 0x1F)) << 3;
            //                //img.SetPixel(x, y, Color.FromArgb(colorR, colorG, colorB));
            //                //imgIdx += 1;

            //                //img.SetPixel(x, y, Color.FromArgb(byImg[imgIdx + 2], byImg[imgIdx + 1], byImg[imgIdx]));
            //                //imgIdx += 3;
            //            }
            //        }
            //    }
            //    //img.Save(@"G:\Study\MySelfProject\Hanhua\Dino1\CmnPicTest\STUFF" + i + ".png");
            //    img.Save(@"G:\Study\MySelfProject\Hanhua\Dino1\CmnPicTest2\menuChk\Core" + i + ".png");
            //}

            //Bitmap msgImg = (Bitmap)Image.FromFile(@"G:\Study\MySelfProject\Hanhua\Dino1\TempChk\OP_MSG0.png");
            //List<int> colorList = new List<int>();
            //for (int y = 0; y < msgImg.Height; y++)
            //{
            //    for (int x = 0; x < msgImg.Width; x++)
            //    {
            //        int color = msgImg.GetPixel(x, y).ToArgb();
            //        if (!colorList.Contains(color))
            //        {
            //            colorList.Add(color);
            //        }
            //    }
            //}
            //colorList.Sort();
            //StringBuilder sb = new StringBuilder();
            //StringBuilder sb2 = new StringBuilder();
            //for (int i = 0; i < colorList.Count; i++)
            //{
            //    int colorR = ((colorList[i] >> 16) & 0xFF) >> 3;
            //    int colorG = (((colorList[i] >> 8) & 0xFF) >> 3) << 5;
            //    int colorB = (((colorList[i] >> 0) & 0xFF) >> 3) << 10;
            //    int pixel = colorR | colorG | colorB;
            //    sb.Append(pixel.ToString("X")).Append("\r\n");

            //    colorR = (((colorList[i] >> 16) & 0xFF) >> 3) << 10;
            //    colorG = (((colorList[i] >> 8) & 0xFF) >> 3) << 5;
            //    colorB = (((colorList[i] >> 0) & 0xFF) >> 3) << 0;
            //    pixel = colorR | colorG | colorB;
            //    sb2.Append(pixel.ToString("X")).Append("\r\n");
            //}
            //sb.Length = 0;
            //sb2.Length = 0;

            //List<FilePosInfo> allCnPic1 = Util.GetAllFiles(@"G:\Study\MySelfProject\Hanhua\Dino1\CmnPicTest2\new\").Where(p => !p.IsFolder).ToList();
            //foreach (FilePosInfo file in allCnPic1)
            //{
            //    if (file.File.EndsWith("WIPE.png", StringComparison.OrdinalIgnoreCase))
            //    {
            //        //List<int> pciPal = new List<int>();
            //        //byte[] byPal = File.ReadAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\WIPE\01.pal");
            //        //for (int i = 0x0; i < byPal.Length; i += 2)
            //        //{
            //        //    int pixelColor = byPal[i] + (byPal[i + 1] << 8);
            //        //    //int colorR = ((byte)(pixelColor & 0x1F)) << 3;
            //        //    //int colorG = ((byte)((pixelColor >> 5) & 0x1F)) << 3;
            //        //    //int colorB = ((byte)((pixelColor >> 10) & 0x1F)) << 3;
            //        //    pciPal.Add(pixelColor);
            //        //}

            //        //Bitmap img = (Bitmap)Image.FromFile(file.File);
            //        //string name = Util.GetShortNameWithoutType(file.File);
            //        //string binFile = file.File.Replace(@"CmnPicTest\CnNew", "PS_cn").Replace(".png", "") + @"\00.tex";
            //        //byte[] byImp = File.ReadAllBytes(binFile);
            //        //int imgIdx = 0;
            //        //for (int y = 0; y < img.Height; y++)
            //        //{
            //        //    for (int x = 0; x < img.Width; x++)
            //        //    {
            //        //        Color pixelColor = img.GetPixel(x, y);

            //        //        int colorR = (pixelColor.R >> 3);
            //        //        int colorG = (pixelColor.G >> 3) << 5;
            //        //        int colorB = (pixelColor.B >> 3) << 10;
            //        //        int pixel = colorR | colorG | colorB;
            //        //        int pixIdx = pciPal.IndexOf(pixel);

            //        //        byImp[imgIdx++] = (byte)(pixIdx & 0xFF);
            //        //        //byImp[imgIdx + 1] = (byte)((pixIdx >> 8) & 0xFF);

            //        //        //imgIdx += 2;
            //        //    }
            //        //}
            //        //File.WriteAllBytes(binFile, byImp);
            //    }
            //    //else if (file.File.EndsWith("WARNING.png"))
            //    else
            //    {
            //        Bitmap img = (Bitmap)Image.FromFile(file.File);
            //        string name = Util.GetShortNameWithoutType(file.File);
            //        string binFile = file.File.Replace(@"CmnPicTest2\new", "PS_cn").Replace(".png", "") + @"\00.bin";
            //        byte[] byImp = File.ReadAllBytes(binFile);
            //        int imgIdx = 0;
            //        for (int y = 0; y < img.Height; y++)
            //        {
            //            for (int x = 0; x < img.Width; x++)
            //            {
            //                Color pixelColor = img.GetPixel(x, y);

            //                int colorR = (pixelColor.R >> 3);
            //                int colorG = (pixelColor.G >> 3) << 5;
            //                int colorB = (pixelColor.B >> 3) << 10;
            //                int pixel = colorR | colorG | colorB;

            //                byImp[imgIdx] = (byte)(pixel & 0xFF);
            //                byImp[imgIdx + 1] = (byte)((pixel >> 8) & 0xFF);

            //                imgIdx += 2;
            //            }
            //        }
            //        File.WriteAllBytes(binFile, byImp);
            //    }
            //}

            //int imgWidth = 160;
            //int imgHeight = 320;
            //Bitmap img = new Bitmap(imgWidth, imgHeight);
            //int imgIdx = 0;
            //byte[] byImp = File.ReadAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino1\TempChk\testMovie2.bin");
            //for (int y = 0; y < imgHeight; y++)
            //{
            //    for (int x = 0; x < imgWidth; x++)
            //    {
            //        int pixelColor = byImp[imgIdx] + (byImp[imgIdx + 1] << 8);
            //        int colorR = ((byte)(pixelColor & 0x1F)) << 3;
            //        int colorG = ((byte)((pixelColor >> 5) & 0x1F)) << 3;
            //        int colorB = ((byte)((pixelColor >> 10) & 0x1F)) << 3;
            //        img.SetPixel(x, y, Color.FromArgb(colorR, colorG, colorB));
            //        imgIdx += 2;
            //    }
            //}
            //img.Save(@"G:\Study\MySelfProject\Hanhua\Dino1\TempChk\testMovie2.png");

            //int imgWidth = 320;
            //int imgHeight = 240;
            //Bitmap img = new Bitmap(imgWidth, imgHeight);
            //int imgIdx = 0;
            //byte[] byImp = File.ReadAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\omake1\00.bin");
            //for (int y = 0; y < imgHeight; y++)
            //{
            //    for (int x = 0; x < imgWidth; x++)
            //    {
            //        int pixelColor = byImp[imgIdx] + (byImp[imgIdx + 1] << 8);
            //        int colorR = ((byte)(pixelColor & 0x1F)) << 3;
            //        int colorG = ((byte)((pixelColor >> 5) & 0x1F)) << 3;
            //        int colorB = ((byte)((pixelColor >> 10) & 0x1F)) << 3;
            //        img.SetPixel(x, y, Color.FromArgb(colorR, colorG, colorB));
            //        imgIdx += 2;
            //    }
            //}
            //img.Save(@"G:\Study\MySelfProject\Hanhua\Dino1\CmnPicTest\CnTest\omake1.png");
            //Bitmap img = (Bitmap)Image.FromFile(@"G:\Study\MySelfProject\Hanhua\Dino1\CmnPicTest\TITLE06_cnImp2.png");
            //for (int y = 0; y < imgHeight; y++)
            //{
            //    for (int x = 0; x < imgWidth; x++)
            //    {
            //        Color pixelColor = img.GetPixel(x, y);

            //        int colorR = (pixelColor.R >> 3);
            //        int colorG = (pixelColor.G >> 3) << 5;
            //        int colorB = (pixelColor.B >> 3) << 10;
            //        int pixel = colorR | colorG | colorB;

            //        byImp[imgIdx] = (byte)(pixel & 0xFF);
            //        byImp[imgIdx + 1] = (byte)((pixel >> 8) & 0xFF);

            //        imgIdx += 2;
            //    }
            //}
            //File.WriteAllBytes(@"G:\Study\MySelfProject\Hanhua\Dino1\PS_cn\TITLE\06.bin", byImp);
        }
    }

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
}
