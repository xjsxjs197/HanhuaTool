using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Hanhua.Common;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Hanhua.TextEditTools.ViewtifulJoe
{
    /// <summary>
    /// 红侠乔伊文本编辑器
    /// </summary>
    public partial class ViewtifulJoeTextEditor : BaseTextEditor
    {
        #region " 本地变量 "

        #region " 字库 "

        /// <summary>
        /// 原字库
        /// </summary>
        private static string[] oldJpFontChars = { 
" ", "!", "\"", "#", "$", "%", "&", "'", "(", ")", "*", "+", ",", "·", ".", "/", "0", "1", "2", "3", "4",
"5", "6", "7", "8", "9", ":", ";", "<", "=", ">", "?", "@", "A", "B", "C", "D", "E", "F", "G", "H", "I",
"J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "[", "", "]", "^",
"_", "‘", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s",
"t", "u", "v", "w", "x", "u", "z", "{", "|", "}", "~", "", "", "", "", "", "", "", "", "", "",
"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
"　", "！", "”", "＃", "＄", "％", "＆", "’", "（", "）", "＊", "＋", "，", "－", "．", "／", "０", "１", "２", "３", "４",
"５", "６", "７", "８", "９", "：", "；", "＜", "＝", "＞", "？", "＠", "Ａ", "Ｂ", "Ｃ", "Ｄ", "Ｅ", "Ｆ", "Ｇ", "Ｈ", "Ｉ",
"Ｊ", "Ｋ", "Ｌ", "Ｍ", "Ｎ", "Ｏ", "Ｐ", "Ｑ", "Ｒ", "Ｓ", "Ｔ", "Ｕ", "Ｖ", "Ｗ", "Ｘ", "Ｙ", "Ｚ", "［", "＼", "］", "＾",
"＿", "‘", "ａ", "ｂ", "ｃ", "ｄ", "ｅ", "ｆ", "ｇ", "ｈ", "ｉ", "ｊ", "ｋ", "ｌ", "ｍ", "ｎ", "ｏ", "ｐ", "ｑ", "ｒ", "ｓ",
"ｔ", "ｕ", "ｖ", "ｗ", "ｘ", "ｙ", "ｚ", "｛", "｜", "｝", "～", "☆", "", "ー", "、", "・", "「", "」", "…", "", "",
"ぁ", "あ", "ぃ", "い", "ぅ", "う", "ぇ", "え", "ぉ", "お", "か", "が", "き", "ぎ", "く", "ぐ", "け", "げ", "こ", "ご", "さ",
"ざ", "し", "じ", "す", "ず", "せ", "ぜ", "そ", "ぞ", "た", "だ", "ち", "ぢ", "っ", "つ", "づ", "て", "で", "と", "ど", "な",
"に", "ぬ", "ね", "の", "は", "ば", "ぱ", "ひ", "び", "ぴ", "ほ", "ぼ", "ぽ", "へ", "べ", "ぺ", "ほ", "ぼ", "ぽ", "ま", "み",
"む", "め", "も", "ゃ", "や", "ゅ", "ゆ", "ょ", "よ", "ら", "り", "る", "れ", "ろ", "ゎ", "わ", "を", "ん", "", "", "",
"ァ", "ア", "ィ", "イ", "ゥ", "ウ", "ェ", "エ", "ォ", "オ", "カ", "ガ", "キ", "ギ", "ク", "グ", "ケ", "ゲ", "コ", "ゴ", "サ",
"ザ", "シ", "ジ", "ス", "ズ", "セ", "ゼ", "ソ", "ゾ", "タ", "ダ", "チ", "ヂ", "ッ", "ツ", "ヅ", "テ", "デ", "ト", "ド", "ナ",
"ニ", "ヌ", "ネ", "ノ", "ハ", "バ", "パ", "ヒ", "ビ", "ピ", "フ", "ブ", "プ", "ヘ", "ベ", "ペ", "ホ", "ボ", "ポ", "マ", "ミ",
"ム", "メ", "モ", "ャ", "ヤ", "ュ", "ユ", "ョ", "ヨ", "ラ", "リ", "ル", "レ", "ロ", "ヮ", "ワ", "ヲ", "ン", "ヴ", "", "",
"戦", "屋", "上", "映", "画", "撮", "影", "始", "帝", "国", "眠", "秘", "宝", "恋", "冒", "険", "物", "語", "狙", "謎", "組",
"織", "大", "乱", "展", "開", "用", "心", "棒", "訳", "主", "人", "公", "差", "置", "勝", "知", "今", "回", "側", "役", "意",
"絶", "体", "命", "空", "中", "攻", "手", "逃", "度", "谷", "底", "真", "逆", "汗", "握", "舞", "台", "負", "無", "準", "備",
"決", "墜", "王", "熱", "間", "一", "髪", "複", "葉", "機", "飛", "乗", "呼", "破", "格", "苦", "労", "番", "神", "像", "墓",
"所", "着", "達", "石", "動", "出", "襲", "言", "小", "先", "演", "客", "君", "悪", "丈", "夫", "筋", "書", "変", "聖", "域",
"足", "踏", "受", "悲", "観", "良", "思", "次", "作", "遺", "跡", "守", "更", "予", "定", "黙", "騒", "休", "日", "巻", "実",
"世", "立", "化", "話", "完", "全", "風", "入", "生", "懸", "頑", "張", "加", "減", "頼", "誰", "壊", "博", "士", "勘", "違",
"過", "去", "名", "誉", "挽", "夜", "闘", "待", "面", "白", "遅", "登", "場", "扱", "力", "者", "優", "監", "督", "利", "怪",
"様", "抜", "新", "笑", "潰", "肉", "調", "子", "常", "識", "囚", "明", "賭", "元", "原", "点", "返", "好", "同", "柳", "下",
"前", "何", "故", "若", "幕", "鳴", "暴", "車", "間", "危", "怖", "静", "仕", "込", "採", "確", "気", "時", "押", "増", "引",
"締", "宇", "宙", "争", "恥", "感", "聞", "然", "速", "際", "制", "器", "界", "女", "暑", "涼", "寒", "冷", "房", "切", "二",
"暖", "到", "我", "見", "消", "刃", "斬", "希", "望", "断", "兄", "外", "性", "期", "合", "相", "嫁", "行", "緊", "急", "兵",
"工", "追", "陰", "謀", "取", "材", "陣", "徐", "雰", "囲", "直", "接", "向", "持", "目", "録", "音", "可", "能", "冗", "談",
"仏", "炎", "深", "本", "江", "戸", "代", "存", "在", "配", "及", "固", "捨", "遊", "星", "恐", "竜", "根", "源", "来", "南",
"極", "数", "万", "年", "落", "突", "如", "現", "多", "倒", "因", "係", "仇", "死", "森", "痛", "結", "講", "売", "当", "記",
"正", "統", "派", "旅", "解", "釈", "船", "医", "脚", "天", "造", "型", "鍵", "級", "他", "柔", "密", "園", "Ⅲ", "少", "困",
"彼", "三", "華", "麗", "貴", "枢", "戻", "道", "具", "重", "得", "続", "地", "響", "掛", "腹", "食", "終", "憩", "迫", "親",
"難", "顔", "考", "海", "楽", "裏", "連", "適", "必", "要", "洋", "居", "忙", "候", "補", "艦", "技", "才", "流", "使", "不",
"安", "発", "特", "殊", "検", "詳", "細", "判", "反", "応", "殿", "自", "慢", "慌", "誇", "移", "供", "頃", "憶", "倉", "怨",
"念", "済", "集", "都", "市", "議", "隠", "砦", "幹", "部", "別", "編", "成", "暗", "躍", "酒", "館", "長", "超", "員", "選",
"択", "対", "設", "以", "後", "容", "量", "整", "理", "限", "超", "初", "失", "敗", "壊", "定", "活", "陸", "色", "分", "想",
"的", "俺", "颯", "爽", "輩", "美", "歌", "声", "獣", "久", "頭", "強", "渡", "座", "則", "形", "非", "表", "情", "疲", "喋",
"孤", "独", "降", "最", "熾", "烈", "援", "忍", "耐", "努", "信", "証", "近", "敵", "覚", "身", "燃", "譲", "四", "指", "導",
"等", "描", "納", "師", "私", "走", "列", "氷", "事", "装", "父", "高", "早", "止", "庫", "許", "古", "撃", "胆", "魔", "激",
"々", "放", "会", "祭", "求", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
    };

        #endregion

        /// <summary>
        /// rle 解压缩文件的后缀
        /// </summary>
        private const string DESC_FILE_SUFFIX = ".rleDec";

        /// <summary>
        /// 中文字库
        /// </summary>
        private string[] cnFontChars = null;

        /// <summary>
        /// 日文字库
        /// </summary>
        private string[] jpFontChars = null;

        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        public ViewtifulJoeTextEditor()
        {
            InitializeComponent();

            this.ResetHeight();

            this.gameName = "ViewtifulJoe";
            //this.baseFolder = @"E:\Study\MySelfProject\Hanhua\TodoCn\ViewtifulJoe";
            this.baseFolder = @"G:\游戏汉化\红侠乔伊";
            
            // 初始化
            this.EditorInit(false);
        }

        #region " 事件 "

        /// <summary>
        /// 字库做成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFont_Click(object sender, EventArgs e)
        {
            // 做成中文字符
            //this.CopyFontChars();

            Bitmap fontImg = new Bitmap(512, 512);
            Graphics g = Graphics.FromImage(fontImg);
            g.Clear(Color.FromArgb(0, 0, 0, 0));
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            Pen blackPen = new Pen(Color.FromArgb(0x42, 0x42, 0x42), 0.01F);
            int imgIndex = 1;

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            // 复制旧字库图片
            //this.CopyOldFontImg(fontImg);

            // 生成新库图片
            for (int i = 1; i < this.cnFontChars.Length; i++)
            {
                // 判断是否换图片
                if (i % 441 == 0)
                {
                    fontImg.Save(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont" + imgIndex + ".png");

                    imgIndex++;
                    fontImg = new Bitmap(512, 512);
                    g = Graphics.FromImage(fontImg);
                    g.Clear(Color.FromArgb(0, 0, 0, 0));
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                }

                this.WrithFontChar(g, this.cnFontChars[i], sf, blackPen, i);
            }

            // 写最后一页
            fontImg.Save(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont" + imgIndex + ".png");
            if (imgIndex == 2)
            {
                imgIndex++;
                fontImg = new Bitmap(512, 512);
                fontImg.Save(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont" + imgIndex + ".png");
            }

            // 复制字库图片到目标目录
            this.CopyFontImgToTarget();
        }

        #endregion

        #region " 重写父类方法 "

        /// <summary>
        /// 读取需要汉化的文件
        /// </summary>
        protected override void LoadAllFiles()
        {
            base.LoadAllFiles();

            // 根据配置文件，取得所有汉化的文件
            List<FilePosInfo> allFiles = this.LoadFiles();
            if (allFiles.Count == 0)
            {
                MessageBox.Show("路径错误，没有找到需要Copy的文件！");
                return;
            }

            // 添加文件
            string jpFile = string.Empty;
            foreach (FilePosInfo fileInfo in allFiles)
            {
                jpFile = this.baseFolder + @"\jp\" + fileInfo.File;
                // 取得各个文件名
                if (File.Exists(jpFile))
                {
                    this.AddFile(jpFile, fileInfo);
                }
            }
        }

        /// <summary>
        /// 开始解码文本
        /// </summary>
        /// <param name="currentFileInfo">当前选择的文件</param>
        /// <param name="isCnTxt">是否是中文</param>
        /// <returns>解码的文本</returns>
        protected override string DecodeText(FilePosInfo currentFileInfo, bool isCnTxt)
        {
            if (isCnTxt)
            {
                return this.DecodeText(File.ReadAllBytes(this.cnFile), currentFileInfo, this.cnFontChars);
            }
            else
            {
                return this.DecodeText(File.ReadAllBytes(currentFileInfo.File), currentFileInfo, this.jpFontChars);
            }
        }

        /// <summary>
        /// 取得当前文字的编码
        /// </summary>
        /// <param name="currenChar">当前文字</param>
        /// <returns>当前文字的编码</returns>
        protected override byte[] EncodeChar(string currentChar)
        {
            for (int i = 0; i < this.cnFontChars.Length; i++)
            {
                string fontChar = this.cnFontChars[i];
                if (fontChar.Equals(currentChar))
                {
                    return new byte[] { (byte)(i >> 8 & 0xFF), (byte)(i & 0xFF) };
                }
            }

            throw new Exception("未查询到相应的中文字符 : " + currentChar);
        }

        /// <summary>
        /// 重新设置带Entry信息的翻译后的数据
        /// </summary>
        /// <param name="currentFileInfo">当前选择的文件</param>
        /// <param name="byData">当前选择的文件的字节数据</param>
        /// <param name="cnBytes">翻译后的字节数据</param>
        /// <returns>带Entry信息的翻译后的数据</returns>
        protected override byte[] ResetCnDataWithEnrty(FilePosInfo currentFileInfo, byte[] byData, List<byte> cnBytes)
        {
            // 带Entry的文本，先保存修改后的各个Entry
            int entryLen = currentFileInfo.TextEntrys.Count * 2;
            byte[] byCnData = new byte[entryLen + 2 + cnBytes.Count];
            int idx = 0;
            for (int i = 0; i < currentFileInfo.TextEntrys.Count; i += 2)
            {
                int startPos = currentFileInfo.TextEntrys[i] / 2;
                int lenInfo = currentFileInfo.TextEntrys[i + 1] / 2;

                byCnData[idx * 4] = (byte)((startPos >> 8) & 0xFF);
                byCnData[idx * 4 + 1] = (byte)(startPos & 0xFF);

                byCnData[idx * 4 + 2] = (byte)((lenInfo >> 8) & 0xFF);
                byCnData[idx * 4 + 3] = (byte)(lenInfo & 0xFF);

                idx++;
            }

            // 再保存文本数据
            byCnData[entryLen] = 0x80;
            byCnData[entryLen + 1] = 0;
            Array.Copy(cnBytes.ToArray(), 0, byCnData, entryLen + 2, cnBytes.Count);

            return byCnData;
        }

        /// <summary>
        /// 重新设置Entry位置信息
        /// </summary>
        /// <param name="currentFileInfo">当前选择的文件</param>
        /// <param name="cnTxtLen">当前行中文文本字节长度</param>
        /// <param name="prevEntryPos">前一个Entry位置信息</param>
        /// <returns>当前Entry位置信息</returns>
        protected override int ResetEntryPosInfo(FilePosInfo currentFileInfo, int cnTxtLen, int prevEntryPos)
        {
            // 先保存文本的长度
            currentFileInfo.TextEntrys.Add(cnTxtLen);

            // 保存下一个Entry的开始位置
            currentFileInfo.TextEntrys.Add(prevEntryPos + cnTxtLen);

            return prevEntryPos + cnTxtLen;
        }

        /// <summary>
        /// 重新设置Entry位置信息
        /// </summary>
        /// <param name="currentFileInfo">当前选择的文件</param>
        /// <param name="cnTxtLen">当前行中文文本字节长度</param>
        /// <param name="prevEntryPos">前一个Entry位置信息</param>
        /// <returns>当前Entry位置信息</returns>
        protected override int ResetLastEntryPosInfo(FilePosInfo currentFileInfo, int cnTxtLen, int prevEntryPos)
        {
            // 保存文本的长度
            currentFileInfo.TextEntrys.Add(cnTxtLen);

            return prevEntryPos + cnTxtLen;
        }

        /// <summary>
        /// 检查输入的中文长度是否正确
        /// </summary>
        /// <param name="chkKeyWords">是否需要检查关键字</param>
        /// <returns>输入的中文长度是否正确</returns>
        protected override bool CheckCnText(bool chkKeyWords)
        {
            return base.CheckCnText(false);
        }

        /// <summary>
        /// 读取字库信息
        /// </summary>
        protected override void ReadFontChar()
        {
            // 读取字库
            this.cnFontChars = this.GetFontChars(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFontChar.txt");
            this.jpFontChars = this.GetFontChars(@"..\EncodeFenxi\EditTools\ViewtifulJoe\JpFontChar.txt");
        }

        #endregion

        #region " 公有方法 "

        /// <summary>
        /// 根据输入的字符串，得到对应的位置
        /// </summary>
        /// <param name="text">字符串文本</param>
        /// <param name="isComText">是共通的文本还是文件的文本</param>
        /// <returns>文本对应的字符的位置</returns>
        public static string GetDiffData(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            string[] byFontChar = oldJpFontChars;
            for (int i = 0; i < text.Length; i++)
            {
                string strChar = text.Substring(i, 1);
                for (int j = 0; j < byFontChar.Length; j++)
                {
                    if (byFontChar[j] == strChar)
                    {
                        sb.Append(" " + j);
                        break;
                    }
                }
            }

            return sb.ToString().Substring(1);
        }

        #endregion

        #region " 私有方法 "

        /// <summary>
        /// 复制字库图片到目标目录
        /// </summary>
        private void CopyFontImgToTarget()
        {
            File.Copy(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont1.png", this.baseFolder + @"\Pic\cn\st00-08\st013_06.png", true);
            File.Copy(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont2.png", this.baseFolder + @"\Pic\cn\st00-08\st013_07.png", true);
            File.Copy(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont3.png", this.baseFolder + @"\Pic\cn\st00-08\st013_08.png", true);

            File.Copy(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont1.png", this.baseFolder + @"\Pic\cn\st00-08\st023_14.png", true);
            File.Copy(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont2.png", this.baseFolder + @"\Pic\cn\st00-08\st023_15.png", true);
            File.Copy(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont3.png", this.baseFolder + @"\Pic\cn\st00-08\st023_16.png", true);

            File.Copy(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont1.png", this.baseFolder + @"\Pic\cn\st00-08\st033_23.png", true);
            File.Copy(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont2.png", this.baseFolder + @"\Pic\cn\st00-08\st033_24.png", true);
            File.Copy(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont3.png", this.baseFolder + @"\Pic\cn\st00-08\st033_25.png", true);

            File.Copy(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont1.png", this.baseFolder + @"\Pic\cn\st00-08\st043_05.png", true);
            File.Copy(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont2.png", this.baseFolder + @"\Pic\cn\st00-08\st043_06.png", true);
            File.Copy(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont3.png", this.baseFolder + @"\Pic\cn\st00-08\st043_07.png", true);

            File.Copy(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont1.png", this.baseFolder + @"\Pic\cn\st00-08\st070_05.png", true);
            File.Copy(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont2.png", this.baseFolder + @"\Pic\cn\st00-08\st070_06.png", true);
            File.Copy(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont3.png", this.baseFolder + @"\Pic\cn\st00-08\st070_07.png", true);

            File.Copy(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont1.png", this.baseFolder + @"\Pic\cn\st00-08\st084_08.png", true);
            File.Copy(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont2.png", this.baseFolder + @"\Pic\cn\st00-08\st084_09.png", true);
            File.Copy(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont3.png", this.baseFolder + @"\Pic\cn\st00-08\st084_10.png", true);

            File.Copy(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont1.png", this.baseFolder + @"\Pic\cn\st00-08\st085_501.png", true);
            File.Copy(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont2.png", this.baseFolder + @"\Pic\cn\st00-08\st085_502.png", true);
            File.Copy(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont3.png", this.baseFolder + @"\Pic\cn\st00-08\st085_503.png", true);

            File.Copy(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont1.png", this.baseFolder + @"\Pic\cn\st00-08\st08400_194.png", true);
            File.Copy(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont2.png", this.baseFolder + @"\Pic\cn\st00-08\st08400_195.png", true);
            File.Copy(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont3.png", this.baseFolder + @"\Pic\cn\st00-08\st08400_196.png", true);

            File.Copy(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont1.png", this.baseFolder + @"\Pic\cn\st00-08\st08401_04.png", true);
            File.Copy(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont2.png", this.baseFolder + @"\Pic\cn\st00-08\st08401_05.png", true);
            File.Copy(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont3.png", this.baseFolder + @"\Pic\cn\st00-08\st08401_06.png", true);

            File.Copy(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont1.png", this.baseFolder + @"\Pic\cn\etc\title_07.png", true);
            File.Copy(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont2.png", this.baseFolder + @"\Pic\cn\etc\title_08.png", true);
            File.Copy(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFont3.png", this.baseFolder + @"\Pic\cn\etc\title_09.png", true);
        }

        /// <summary>
        /// 写入中文字符图片
        /// </summary>
        /// <param name="g"></param>
        /// <param name="charTxt"></param>
        private void WrithFontChar(Graphics g, string charTxt, StringFormat sf, Pen blackPen, int charIndex)
        {
            // 每行21个，每个24个像素
            int pageIndex = charIndex % 441;
            int yPos = pageIndex / 21;
            int xPos = (pageIndex - yPos * 21) * 24;
            yPos = yPos * 24;

            GraphicsPath graphPath = new GraphicsPath();
            RectangleF rectangle = new RectangleF(xPos, yPos + 3, 24, 24);
            graphPath.AddString(charTxt, new FontFamily("SimHei"), (int)FontStyle.Bold, 22, rectangle, sf);
            g.FillPath(Brushes.White, graphPath);
            g.DrawPath(blackPen, graphPath);
        }

        /// <summary>
        /// 复制旧字库图片
        /// </summary>
        /// <returns></returns>
        private void CopyOldFontImg(Bitmap newFontImg)
        {
            Bitmap oldFontImg = (Bitmap)Bitmap.FromFile(@"..\EncodeFenxi\EditTools\ViewtifulJoe\JpFont1.png");
            for (int y = 0; y < 120; y++)
            {
                for (int x = 0; x < 512; x++)
                {
                    newFontImg.SetPixel(x, y, oldFontImg.GetPixel(x, y + 192));
                }
            }

            // 清空最后一个字符位置
            for (int y = 96; y < 120; y++)
            {
                for (int x = 432; x < 512; x++)
                {
                    newFontImg.SetPixel(x, y, Color.Transparent);
                }
            }
        }

        /// <summary>
        /// 做成中文字符
        /// </summary>
        private void CopyFontChars()
        {
            List<string> fontList = new List<string>();
            bool startCopy = false;
            for (int i = 0; i < oldJpFontChars.Length; i++)
            {
                if (oldJpFontChars[i].Equals("　"))
                {
                    startCopy = true;
                }
                else if (oldJpFontChars[i].Equals("…"))
                {
                    break;
                }

                if (startCopy)
                {
                    fontList.Add(oldJpFontChars[i]);
                }
            }

            string[] cnChars = File.ReadAllLines(this.baseFolder + @"\文本.xlsx.txt", Encoding.UTF8);
            for (int i = 0; i < cnChars.Length; i++)
            {
                string curChar = cnChars[i].Substring(7, 1);
                if (!fontList.Contains(curChar))
                {
                    fontList.Add(curChar);
                }
            }

            cnChars = new string[fontList.Count];
            for (int i = 0; i < fontList.Count; i++)
            {
                cnChars[i] = i.ToString().PadLeft(4, '0') + " " + fontList[i];
            }

            File.WriteAllLines(@"..\EncodeFenxi\EditTools\ViewtifulJoe\CnFontChar.txt", cnChars, Encoding.UTF8);
        }

        /// <summary>
        /// 读取字库文件
        /// </summary>
        /// <param name="fontFile"></param>
        /// <returns></returns>
        private string[] GetFontChars(string fontFile)
        {
            string[] fontChars = File.ReadAllLines(fontFile, Encoding.UTF8);
            string[] temp = new string[fontChars.Length];
            for (int i = 0; i < fontChars.Length; i++)
            {
                temp[i] = fontChars[i].Split(' ')[1];
            }

            return temp;
        }

        /// <summary>
        /// 根据配置文件，读入需要汉化的文件
        /// </summary>
        /// <returns></returns>
        private List<FilePosInfo> LoadFiles()
        {
            List<FilePosInfo> needCopyFiles = new List<FilePosInfo>();
            needCopyFiles.AddRange(base.LoadFiles(this.baseFolder + @"\addrInfo.txt"));
            return needCopyFiles;

            //List<FilePosInfo> allFiles = Util.GetAllFiles(this.baseFolder + @"\jp\root\hard\st08\").Where(
            //    p => !p.IsFolder && p.File.EndsWith(DESC_FILE_SUFFIX)).ToList();
            //foreach (FilePosInfo fiInfo in allFiles)
            //{
            //    byte[] byData = File.ReadAllBytes(fiInfo.File);
            //    int txtPos = Util.GetOffset(byData, 0x828, 0x82b);
            //    if (txtPos == 0)
            //    {
            //        txtPos = Util.GetOffset(byData, 0x820, 0x823);
            //    }
            //    txtPos += 0x800;
            //    fiInfo.EntryPos = txtPos + 0x10;
            //    fiInfo.TextStart = txtPos + Util.GetOffset(byData, txtPos + 0x8, txtPos + 0xb);
            //}

            //return allFiles;
        }

        /// <summary>
        /// 开始解码
        /// </summary>
        /// <param name="byData"></param>
        /// <param name="startPos"></param>
        private string DecodeText(byte[] byData, FilePosInfo filePosInfo, string[] fontChar)
        {
            StringBuilder sb = new StringBuilder();
            int txtStart = filePosInfo.TextStart + 2;
            for (int j = filePosInfo.EntryPos; j < filePosInfo.TextStart; j += 4)
            {
                int startPos = txtStart + Util.GetOffset(byData, j, j + 1) * 2;
                int endPos = startPos + Util.GetOffset(byData, j + 2, j + 3) * 2;
                sb.Append(this.DecodeText(byData, fontChar, startPos, endPos));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 开始解码
        /// </summary>
        /// <param name="byData"></param>
        /// <param name="startPos"></param>
        private string DecodeText(byte[] byData, string[] fontChar, int startPos, int endPos)
        {
            StringBuilder sb = new StringBuilder();
            for (int j = startPos; j < endPos; j += 2)
            {
                if (byData[j] == 0x80 && byData[j + 1] == 0)
                {
                    sb.Append("^80 00^");
                }
                else
                {
                    int pos = Util.GetOffset(byData, j, j + 1);
                    if (pos == 0)
                    {
                        sb.Append("^00 00^");
                    }
                    else if (pos > 0 && pos < fontChar.Length)
                    {
                        string posChar = fontChar[pos];
                        if (string.IsNullOrEmpty(posChar))
                        {
                            sb.Append("^" + byData[j].ToString("x") + " " + byData[j + 1].ToString("x") + "^");
                        }
                        else
                        {
                            sb.Append(posChar);
                        }
                    }
                    else
                    {
                        sb.Append("^" + byData[j].ToString("x") + " " + byData[j + 1].ToString("x") + "^");
                    }
                }
            }

            sb.Append("\n");

            return sb.ToString();
        }

        #endregion
       
    }
}
