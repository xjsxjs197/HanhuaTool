using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Linq;

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
            "着", "射", "定", "ィ", "上", "中", "出", "ェ", "敵", "与", "部", "散", "持", "酔", "—", "的",
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
            "Ｗ", "Ｙ", "c", "o", "あ", "う", "お", "き", "け", "さ", "す", "そ", "ち", "て", "な", "ぬ",
            "の", "ひ", "へ", "ま", "む", "も", "ゆ", "ら", "る", "ろ", "を", "が", "ぐ", "ご", "じ", "ぜ",
            "だ", "づ", "ど", "び", "べ", "ぱ", "ぷ", "ぽ", "ゅ", "っ", "イ", "エ", "カ", "ク", "コ", "シ",
            "セ", "タ", "ツ", "ト", "ニ", "ネ", "ㇵ", "フ", "ホ", "ミ", "メ", "ヤ", "ヨ", "リ", "レ", "ワ",
            "ン", "ギ", "ゲ", "ザ", "ズ", "ゾ", "ヂ", "デ", "バ", "ブ", "ボ", "ピ", "ペ", "ャ", "ョ", "ヴ",
            "▼", "、", "！", "：", "・", "×", "《", "社", "小", "改", "撃", "軍", "長", "丸", "壊", "装",
            "連", "安", "使", "率", "命", "高", "強", "ォ", "大", "造", "分", "広", "麻", "弱", "時", "生",
            "眠", "数", "必", "間", "毒", "至", "標", "（", "）", "火", "無", "制", "殊", "最", "止", "剤",
            "回", "失", "痛", "少", "全", "薬", "態", "材", "合", "画", "新", "系", "化", "殖", "急", "緑",
            "開", "室", "記", "備", "源", "救", "人", "証", "施", "事", "動", "書", "組", "信", "置", "格",
            "究", "図", "号", "子", "除", "Ⅱ", "充", "給", "量", "指", "採", "録", "在", "存", "管", "入",
            "御", "質", "形", "換", "”", "常", "V", "責", "者", "士", "港", "密", "厳", "多", "Ⅲ", "験",
            "一", "守", "内", "収", "性", "違", "印", "器", "居", "所", "下", "文", "兵", "同", "確", "武",
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

            this.ExtractRaw(@"E:\Game\Dino1\DatDecode\LOAD2");
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

        private void btnSearchTxt_Click(object sender, EventArgs e)
        {
            string chkTxt = this.txtChk.Text.Trim();
            if (string.IsNullOrEmpty(chkTxt))
            {
                MessageBox.Show("请输入查找的文本");
                return;
            }

            Dictionary<string, string> dinoCoreFontChar = new Dictionary<string, string>();
            int charIdx = 0;
            for (int i = 0; i < coreFontChar1.Length; i++)
            {
                dinoCoreFontChar.Add(coreFontChar1[i], charIdx + " 0");
                dinoCoreFontChar.Add(coreFontChar2[i], charIdx + " 1");
                charIdx++;
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < chkTxt.Length; i++)
            {
                string curChar = chkTxt.Substring(i, 1);
                if (dinoCoreFontChar.ContainsKey(curChar))
                {
                    sb.Append(dinoCoreFontChar[curChar]).Append(" ");
                }
                else
                {
                    sb.Append(999).Append(" ");
                }
            }

            this.txtChk.Text = sb.ToString();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            List<FilePosInfo> allBin = Util.GetAllFiles(@"E:\Game\Dino1\DatDecode").Where(p => !p.IsFolder && p.File.EndsWith(".bin", StringComparison.OrdinalIgnoreCase)).ToList();
            StringBuilder sb = new StringBuilder();

            string[] char1 = new string[256];
            string[] char2 = new string[256];
            int charIdx = 0;
            int charIdx1 = 0;
            int charIdx2 = 0;
            bool isChar1 = true;
            while (charIdx1 < (coreFontChar1.Length + coreFontChar2.Length))
            {
                if (charIdx1 < 255)
                {
                    char1[charIdx1++] = coreFontChar1[charIdx];
                    char1[charIdx1++] = coreFontChar2[charIdx];
                    charIdx++;
                }
                else
                {
                    char2[charIdx1 - 256] = coreFontChar1[charIdx];
                    charIdx1++;
                    char2[charIdx1 - 256] = coreFontChar2[charIdx];
                    charIdx1++;
                    charIdx++;
                }
            }

            byte[] binByte = File.ReadAllBytes(@"H:\游戏汉化\Dino Crisis\Ps_jp\ST10B\08.bin");
            //  st10a 07.bin 0x499b0

            for (int i = 0x39058; i < binByte.Length; i += 2 )
            {
                if (binByte[i] == 0x00 && binByte[i + 1] == 0xA0)
                {
                    sb.Append("^00A0^\r\n");
                }
                else if (binByte[i] == 0x00 && binByte[i + 1] == 0xC0)
                {
                    sb.Append("^00C0^\r\n");
                }
                else if (binByte[i + 1] == 0x80)
                {
                    sb.Append(char1[binByte[i]]); //.Append("80");
                }
                else if (binByte[i + 1] == 0x81)
                {
                    sb.Append(char2[binByte[i]]);
                }
                else if (binByte[i + 1] == 0x82)
                {
                    if (binByte[i] < st8210BChar.Length)
                    {
                        sb.Append(st8210BChar[binByte[i]]);
                    }
                    else
                    {
                        sb.Append(binByte[i].ToString("X2")).Append("82");
                    }
                }
                else
                {
                    sb.Append(binByte[i].ToString("X2")).Append(binByte[i + 1].ToString("X2"));
                }
            }
        }

        private void btnDecAllDat_Click(object sender, EventArgs e)
        {
            List<FilePosInfo> allDat = Util.GetAllFiles(@"G:\Study\MySelfProject\Hanhua\Dino1\Ps_Iso\PSX\DATA").Where(p => !p.IsFolder && p.File.EndsWith(".dat", StringComparison.OrdinalIgnoreCase)).ToList();
            foreach (FilePosInfo datFile in allDat)
            {
                try
                {
                    this.Open(datFile.File);

                    string folder = @"G:\Study\MySelfProject\Hanhua\Dino1\PS_jp\" + Util.GetShortNameWithoutType(datFile.File);
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
