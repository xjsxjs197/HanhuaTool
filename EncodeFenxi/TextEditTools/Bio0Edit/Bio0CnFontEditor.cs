using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Hanhua.Common;
using Hanhua.TextEditTools.Bio1Edit;

namespace Hanhua.TextEditTools.Bio0Edit
{
    /// <summary>
    /// Tpl类型的字库的编辑器
    /// </summary>
    public partial class Bio0CnFontEditor : BaseForm
    {
        #region " 私有变量 "

        /// <summary>
        /// 旧字库的图片
        /// </summary>
        private Bitmap fontBmp;

        /// <summary>
        /// 共通的中文字符
        /// </summary>
        private string[] comCnFont = new string[] {
            ""  , "0" , "1" , "2" , "3" , "4" , "5" , "6", "7" , "8" , "9" , "A" , "B" , "C" , "D" , "E" , "部", "果", "拿", "值", "注", "起", "启", "整", "乘", "针", "间", "成", "按", "实", "码", "存", 
            "F" , "G" , "H" , "I" , "J" , "K" , "L" , "M", "N" , "O" , "P" , "Q" , "R" , "S" , "T" , "U" , "请", "定", "和", "所", "制", "让", "天", "降", "容", "完", "把", "板", "输", "室", "理", "化", 
            "V" , "W" , "X" , "Y" , "Z" , "a" , "b" , "c", "d" , "e" , "f" , "g" , "h" , "i" , "j" , "k" , "型", "片", "活", "迹", "列", "始", "落", "路", "小", "们", "消", "房", "瑞", "贝", "多", "拉", 
            "l" , "m" , "n" , "o" , "p" , "q" , "r" , "s", "t" , "u" , "v" , "w" , "x" , "y" , "z" , "　",  "只", "工", "断", "员", "各", "写", "女", "再", "养", "散", "焰", "吧", "些", "左", "右", "供", 
            "的", "了", "有", "没", "像", "上", "要", "是", "么", "好", "什", "被", "面", "在", "吗", "一",  "需", "量", "全", "正", "或", "件", "古", "光", "性", "分", "感", "道", "虽", "然", "初", "务", 
            "着", "地", "个", "用", "不", "锁", "下", "作", "已", "经", "很", "方", "里", "到", "子", "放",  "留", "查", "Ｓ", "Ｔ", "Ａ", "Ｒ", "±", "℃", "＋", "＝", "。", "←", "→", "、", "。", "゜", 
            "动", "大", "品", "装", "进", "法", "卡", "置", "去", "行", "使", "无", "入", "以", "物", "话", "…", "：", "！", "?" , "（", "）", "「", "」","『", "』" , "〇", "ー", "‐", "”", "“", "×",
            "都", "可", "人", "出", "操", "画", "力", "特", "现", "别", "坏", "中", "这", "门", "掉", "从", "／", "’", "，", "▽", "▷" , "△", "※", "觉", "应", "其", "系", "数", "外", "内", "组", "类", 
            "来", "车", "记", "钥", "匙", "梯", "打", "调", "得", "关", "还", "看", "西", "过", "水", "东", "刻", "显", "统", "保", "档", "取", "同", "角", "于", "状", "为", "弹", "我", "会", "枪", "击",
            "插", "药", "机", "手", "槽", "器", "也", "除", "色", "通", "解", "转", "意", "就", "危", "险", "↑", "↓", "钮", "那", "蛭", "究", "开", "研", "日", "后", "态", "合", "设", "草", "换", "长",
            "时", "必", "具", "种", "火", "运", "但", "处", "如", "样", "对", "给", "升", "盘", "密", "能", "变", "游", "戏", "备", "斯", "高", "示", "比"
        };

        /// <summary>
        /// 共通的旧的中文字符
        /// </summary>
        private string[] comOldFont = new string[] {
            ""  , "0" , "1" , "2" , "3" , "4" , "5" , "6", "7" , "8" , "9" , "A" , "B" , "C" , "D" , "E" , "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 
            "F" , "G" , "H" , "I" , "J" , "K" , "L" , "M", "N" , "O" , "P" , "Q" , "R" , "S" , "T" , "U" , "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 
            "V" , "W" , "X" , "Y" , "Z" , "a" , "b" , "c", "d" , "e" , "f" , "g" , "h" , "i" , "j" , "k" , "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 
            "l" , "m" , "n" , "o" , "p" , "q" , "r" , "s", "t" , "u" , "v" , "w" , "x" , "y" , "z" , "",  "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 
            "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",  "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 
            "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",  "", "", "Ｓ", "Ｔ", "Ａ", "Ｒ", "±", "℃", "＋", "＝", "。", "←", "→", "、", "。", "゜", 
            "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "…", "：", "！", "?" , "（", "）", "「", "」","『", "』" , "〇", "ー", "‐", "”", "“", "×",
            "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "／", "’", "，", "▽", "▷" , "△", "※", "", "", "", "", "", "", "", "", "", 
            "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "↑", "↓", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
        };

        /// <summary>
        /// 是否使用旧字库
        /// </summary>
        private bool isUseOld;

        /// <summary>
        /// 旧字库字符信息
        /// </summary>
        private List<FontCharInfo> fontCharInfoLst = new List<FontCharInfo>();

        /// <summary>
        /// 新字库字符信息
        /// </summary>
        private List<FontCharInfo> newFontCharInfoLst = new List<FontCharInfo>();

        /// <summary>
        /// 选择的字库
        /// </summary>
        private System.Drawing.Font selectedFont;

        /// <summary>
        /// 字库设置文件
        /// </summary>
        private string cnFont;

        private int fontWidth = 0x1C;
        private int fontHeight = 0x1C;

        #endregion

        #region " 构造方法 "

        /// <summary>
        /// 初始化
        /// </summary>
        public Bio0CnFontEditor()
        {
            InitializeComponent();

            this.ResetHeight();

            this.selectedFont = this.txtFontTest.Font;
            this.rdoFont4.Checked = true;

            // 绑定事件
            this.rdoFont1.CheckedChanged += new EventHandler(this.rdoFont_CheckedChanged);
            this.rdoFont2.CheckedChanged += new EventHandler(this.rdoFont_CheckedChanged);
            this.rdoFont3.CheckedChanged += new EventHandler(this.rdoFont_CheckedChanged);
            this.rdoFont4.CheckedChanged += new EventHandler(this.rdoFont_CheckedChanged);
            this.rdoFontCom.CheckedChanged += new EventHandler(this.rdoFont_CheckedChanged);

            // 显示字库信息
            this.cnFont = @"..\EncodeFenxi\BioTools\Bio0Edit\Bio0CnFont04.txt";
        }

        #endregion

        #region " 页面事件 "

        /// <summary>
        /// 打开文件的字库文本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadFileFont_Click(object sender, EventArgs e)
        {
            // 显示字库信息
            this.isUseOld = false;
            this.ViewFont(21, File.ReadAllLines(this.cnFont));
        }

        /// <summary>
        /// 选择字体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFontSelect_Click(object sender, EventArgs e)
        {
            try
            {

                if (fontDialog1.ShowDialog() == DialogResult.OK)
                {

                    this.selectedFont = fontDialog1.Font;
                    txtFontTest.Font = fontDialog1.Font;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 保存字库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            FileStream fs = null;

            try
            {
                Bitmap fntBmp;
                int rowCount;
                string fontName = string.Empty;
                if (this.isUseOld)
                {
                    fntBmp = new Bitmap(896, 308);
                    rowCount = 11;
                    fontName = @"..\BioTools\Bio0Edit\bio0CnComFont.png";
                    
                }
                else
                {
                    fntBmp = new Bitmap(896, 588);
                    rowCount = 21;
                    fontName = this.cnFont.Substring(0, this.cnFont.Length - 4) + ".png";
                }

                // 显示进度条
                this.ResetProcessBar(this.grdFont.Rows.Count);

                // 生成字库图片文件
                this.CreateImgFontFile(0, rowCount, fntBmp, fontName);

                // 写入字符宽度信息
                ////int charInfoStart = 0x2ee900;
                //int charInfoStart = 0x17d9d0;
                //int charInfoLen = 32 * 34 + 2;
                //if (!this.isComText)
                //{
                //    charInfoStart += (32 * 34 + 2) * 2;
                //    charInfoLen = 32 * 29 + 6;
                //}
                //byte[] byCharWidInfo = new byte[charInfoLen * 2];
                //for (int i = 0; i < byCharWidInfo.Length; i += 2)
                //{
                //    if (i / 2 < this.newFontCharInfoLst.Count)
                //    {
                //        FontCharInfo charInfo = this.newFontCharInfoLst[i / 2];
                //        byCharWidInfo[i] = (byte)charInfo.LeftPadding;
                //        byCharWidInfo[i + 1] = (byte)charInfo.Width;
                //    }
                //}

                //fs = new FileStream(this.fontWidInfo + "_cn.dol", FileMode.Open);
                //fs.Seek(charInfoStart, SeekOrigin.Begin);
                //fs.Write(byCharWidInfo, 0, byCharWidInfo.Length);
                //fs.Close();
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
            }
            finally
            {
                // 隐藏进度条
                this.CloseProcessBar();

                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// 搜索字符
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.SearchChar();
        }

        /// <summary>
        /// 快捷搜索字符
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SearchChar();
            }
        }

        /// <summary>
        /// 切换字库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoFont_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdo = (RadioButton)sender;
            if (rdo.Checked)
            {
                if (rdo == this.rdoFontCom)
                {
                    // 共通
                    this.isUseOld = true;
                    this.ViewFont(11, this.comCnFont);
                }
                else
                {
                    this.cnFont = @"..\BioTools\Bio0Edit\Bio0" + rdo.Text + ".txt";

                    this.isUseOld = false;
                    this.ViewFont(21, File.ReadAllLines(this.cnFont));
                }
            }
        }

        #endregion

        #region " 私有方法 "

        /// <summary>
        /// 搜索字符
        /// </summary>
        private void SearchChar()
        {
            string key = this.txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            this.grdFont.ClearSelection();

            string[] fontChars = File.ReadAllLines(this.cnFont);
            for (int i = 0; i < fontChars.Length; i++)
            {
                if (fontChars[i] == key)
                {
                    int row = i / this.fontHeight;
                    int col = i % this.fontWidth;
                    this.grdFont.Rows[row].Cells[col].Selected = true;
                    break;
                }
            }
        }

        /// <summary>
        /// 生成字库图片文件
        /// </summary>
        /// <param name="startRow"></param>
        /// <param name="endRow"></param>
        /// <param name="fntBmp"></param>
        private void CreateImgFontFile(int startRow, int endRow, Bitmap fntBmp, string fontName)
        {
            for (int row = startRow; row < endRow; row++)
            {
                int yIndex = row;
                for (int col = 0; col < 32; col++)
                {
                    Bitmap cellImg = this.grdFont.Rows[row].Cells[col].Value as Bitmap;
                    for (int y = 0; y < this.fontHeight; y++)
                    {
                        for (int x = 0; x < this.fontWidth; x++)
                        {
                            fntBmp.SetPixel(col * this.fontWidth + x, yIndex * this.fontHeight + y, cellImg.GetPixel(x, y));
                        }
                    }
                }

                // 更新进度条
                this.ProcessBarStep();
            }

            fntBmp.Save(fontName);
        }

        /// <summary>
        /// 显示字库信息
        /// </summary>
        private void ViewFont(int maxRow, string[] fontChars)
        {
            FileStream fs = null;
            this.grdFont.Rows.Clear();

            try
            {
                // 读取字库字符文件
                List<string> fntCharList = new List<string>();
                fntCharList.AddRange(fontChars);
                int needAddedNum = maxRow * 32 - fntCharList.Count;
                if (needAddedNum >= 0)
                {
                    while (needAddedNum-- > 0)
                    {
                        fntCharList.Add(string.Empty);
                    }
                }

                // 判断超过了多少个字符
                int maxCharCount = 32 * maxRow;
                if (fontChars.Length > maxCharCount)
                {
                    MessageBox.Show("字库数超过了容量（目前：" + fontChars.Length + "，最大：" + maxCharCount + "，超出：" + (fontChars.Length - maxCharCount) + "），游戏可能无法运行！");
                }

                // 显示进度条
                this.ResetProcessBar(fontChars.Length);

                // 开始按行、列画字库图片
                this.newFontCharInfoLst.Clear();
                for (int row = 0; row < maxRow; row++)
                {
                    Image[] rowImage = new Image[32];
                    for (int col = 0; col < 32; col++)
                    {
                        rowImage[col] = this.CreateCharBitMap(fntCharList[row * 32 + col]);
                    }

                    int addedRow = this.grdFont.Rows.Add(rowImage);
                    this.grdFont.Rows[addedRow].Height = this.fontHeight + 2;
                }
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
            }
            finally
            {
                // 隐藏进度条
                this.CloseProcessBar();

                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// 根据单个字符生成字库图片
        /// </summary>
        /// <param name="fontChar"></param>
        /// <returns></returns>
        private Bitmap CreateCharBitMap(string fontChar)
        {
            Bitmap bmp = new Bitmap(this.fontWidth, this.fontHeight);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.FromArgb(0, 0, 0, 0));
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            Pen blackPen = new Pen(Color.Black, 0.01F);

            if (this.isUseOld && this.comOldFont.Contains(fontChar))
            {
                // 取得原有的字库
                int fontW = 0x1C;
                int fontH = 0x1C;
                int index = 0;
                int charFontIndex = 0;
                for (int i = 0; i < this.comOldFont.Length; i++)
                {
                    if (fontChar == this.comOldFont[i])
                    {
                        index = i;
                        break;
                    }
                }

                // 根据旧字符宽度信息，设置新字符宽度信息
                this.newFontCharInfoLst.Add(new FontCharInfo());

                // 取得旧字库图片
                this.fontBmp = new Bitmap(@"..\BioTools\Bio0Edit\bio0ComFont.bmp");
                for (int y = 0; y < this.fontBmp.Height; y += fontH)
                {
                    for (int x = 0; x < this.fontBmp.Width; x += fontW)
                    {
                        if (charFontIndex == index)
                        {
                            for (int yPixel = 0; yPixel < fontH; yPixel++)
                            {
                                for (int xPixel = 0; xPixel < fontW; xPixel++)
                                {
                                    bmp.SetPixel(xPixel, yPixel, this.fontBmp.GetPixel(x + xPixel, y + yPixel));
                                }
                            }
                            return bmp;
                        }

                        charFontIndex++;
                    }
                }
            }
            else
            {
                FontCharInfo charInfo = new FontCharInfo();

                if (string.IsNullOrEmpty(fontChar))
                {
                    // 更新进度条
                    this.ProcessBarStep();

                    return bmp;
                }
                else if ("　" == fontChar)
                {
                    charInfo.LeftPadding = this.fontWidth;
                    charInfo.Width = 0;
                    this.newFontCharInfoLst.Add(charInfo);

                    // 更新进度条
                    this.ProcessBarStep();

                    return bmp;
                }

                // 单个子对齐风格
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Near;
                CharacterRange[] characterRanges = { new CharacterRange(0, 1) };
                sf.SetMeasurableCharacterRanges(characterRanges);

                // 在指定的区域内写入特定汉字
                RectangleF rectangle = new RectangleF(0, 0, this.fontWidth, this.fontHeight);
                GraphicsPath graphPath = new GraphicsPath();

                graphPath.AddString(fontChar, new FontFamily(txtFontTest.Font.Name), (int)FontStyle.Bold, this.fontHeight, rectangle, sf);
                g.FillPath(Brushes.White, graphPath);
                g.DrawPath(blackPen, graphPath);

                //rectangle.Offset(-1, 0);  // 绘制左背景文字  
                //g.DrawString(fontChar, this.selectedFont, Brushes.Black, rectangle, sf);
                //rectangle.Offset(2, 0);  // 绘制右背景文字  
                //g.DrawString(fontChar, this.selectedFont, Brushes.Black, rectangle, sf);
                //rectangle.Offset(-1, -1);  // 绘制下背景文字  
                //g.DrawString(fontChar, this.selectedFont, Brushes.Black, rectangle, sf);
                //rectangle.Offset(0, 2);  // 绘制上背景文字  
                //g.DrawString(fontChar, this.selectedFont, Brushes.Black, rectangle, sf);
                //rectangle.Offset(0, -1);  // 定位点归位

                //// 绘制前景文字  
                //g.DrawString(fontChar, this.selectedFont, Brushes.White, rectangle, sf);

                // 设置字符宽度信息
                Region[] charRegions = g.MeasureCharacterRanges(fontChar, this.selectedFont, rectangle, sf);
                int charWidth = (int)charRegions[0].GetBounds(g).Width;
                int charLeftWidth = this.fontWidth + 1 - charWidth;

                if (charLeftWidth <= 0)
                {
                    charLeftWidth = 0;
                }
                else if (charLeftWidth % 2 != 0)
                {
                    charLeftWidth = charLeftWidth / 2 + 1;
                }
                else
                {
                    charLeftWidth = charLeftWidth / 2;
                }

                charInfo.LeftPadding = charLeftWidth;
                charInfo.Width = charLeftWidth + charWidth;
                this.newFontCharInfoLst.Add(charInfo);
            }

            // 更新进度条
            this.ProcessBarStep();

            return bmp;
        }

        #endregion
    }
}