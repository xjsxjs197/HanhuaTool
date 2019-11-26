using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Hanhua.Common;

namespace Hanhua.TextEditTools.Bio1Edit
{
    /// <summary>
    /// Tpl类型的字库的编辑器
    /// </summary>
    public partial class Bio1FontEditor : BaseForm
    {
        #region " 私有变量 "

        /// <summary>
        /// 共通文本的字库
        /// </summary>
        private const string cnTextFont = "Bio1CnTextFont.txt";

        /// <summary>
        /// 文件的字库
        /// </summary>
        private const string cnFileFont = "Bio1CnFileFont.txt";

        /// <summary>
        /// 旧字库的图片
        /// </summary>
        private Bitmap fontBmp;

        /// <summary>
        /// 是共通的文本还是文件的文本
        /// </summary>
        private bool isComText = true;

        /// <summary>
        /// 要保存的字库图片的名称
        /// </summary>
        private string fontImgName;

        /// <summary>
        /// 表示哪些文字使用旧字库的图片
        /// </summary>
        private List<string> useOldFont = new List<string>() {
            "　", "▷", "▽", ".", "'", "、", "。", "（", "）", "゜", "「", "」", "．", "～"
        };

        /// <summary>
        /// 是否正在查看旧字库
        /// </summary>
        private bool isViewOld;

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
        /// 是否是Wii汉化
        /// </summary>
        private bool isWii;

        #endregion

        #region " 构造方法 "

        /// <summary>
        /// 初始化
        /// </summary>
        public Bio1FontEditor(string folder)
        {
            InitializeComponent();

            this.ResetHeight();

            this.baseFolder = folder;
            selectedFont = this.txtFontTest.Font;

            this.isWii = true;
            if (folder.IndexOf("Ngc") != -1)
            {
                this.isWii = false;
            }
        }

        #endregion

        #region " 页面事件 "

        /// <summary>
        /// 打开共通的字库文本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadComFont_Click(object sender, EventArgs e)
        {
            this.fontImgName = this.baseFolder + @"\Font\textFont";
            this.isComText = true;
            this.isViewOld = false;

            // 显示字库信息
            this.ViewFont(cnTextFont);
        }

        /// <summary>
        /// 打开文件的字库文本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadFileFont_Click(object sender, EventArgs e)
        {
            this.fontImgName = this.baseFolder + @"\Font\fileFont";
            this.isComText = false;
            this.isViewOld = false;

            // 显示字库信息
            this.ViewFont(cnFileFont);
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
                Bitmap fntBmp = new Bitmap(1024, 1024);

                // 显示进度条
                this.ResetProcessBar(this.grdFont.Rows.Count);

                // 生成字库图片文件
                this.CreateImgFontFile(0, 32, fntBmp, this.fontImgName + ".png");

                if (this.grdFont.Rows.Count > 32)
                {
                    fntBmp = new Bitmap(1024, 128);

                    // 生成字库图片文件
                    this.CreateImgFontFile(32, 36, fntBmp, this.fontImgName + "2.png");
                }

                // 写入字符宽度信息
                int charInfoStart = 0;
                string fontWidInfoFile = string.Empty;
                if (this.isWii)
                {
                    charInfoStart = 0x2ee900;
                    fontWidInfoFile = this.baseFolder + @"\WiiCn\sys\main.dol";
                }
                else
                {
                    charInfoStart = 0x17d9d0;
                    fontWidInfoFile = this.baseFolder + @"\&&systemdata\Start.dol";
                }

                int charInfoLen = 32 * 34 + 2;
                if (!this.isComText)
                {
                    charInfoStart += (32 * 34 + 2) * 2;
                    charInfoLen = 32 * 29 + 6;
                }
                byte[] byCharWidInfo = new byte[charInfoLen * 2];
                for (int i = 0; i < byCharWidInfo.Length; i += 2)
                {
                    if (i / 2 < this.newFontCharInfoLst.Count)
                    {
                        FontCharInfo charInfo = this.newFontCharInfoLst[i / 2];
                        byCharWidInfo[i] = (byte)charInfo.LeftPadding;
                        byCharWidInfo[i + 1] = (byte)charInfo.Width;
                    }
                }

                fs = new FileStream(fontWidInfoFile, FileMode.Open);
                fs.Seek(charInfoStart, SeekOrigin.Begin);
                fs.Write(byCharWidInfo, 0, byCharWidInfo.Length);
                fs.Close();
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
        /// 查看旧字库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewOldFont_Click(object sender, EventArgs e)
        {
            try
            {
                this.isViewOld = true;

                // 读取旧字库图片
                this.LoadOldFontBmp();
                this.grdFont.Rows.Clear();

                int fontW = 32;
                int fontH = 32;

                // 显示进度条
                this.ResetProcessBar((this.fontBmp.Height / fontH) * (this.fontBmp.Width / fontW));

                for (int y = 0; y < this.fontBmp.Height; y += fontH)
                {
                    Image[] rowImage = new Image[this.fontBmp.Width / fontW];
                    for (int x = 0; x < this.fontBmp.Width; x += fontW)
                    {
                        Bitmap cellImg = new Bitmap(fontW, fontH);
                        rowImage[x / fontW] = cellImg;

                        for (int yPixel = 0; yPixel < fontH; yPixel++)
                        {
                            for (int xPixel = 0; xPixel < fontW; xPixel++)
                            {
                                cellImg.SetPixel(xPixel, yPixel, this.fontBmp.GetPixel(x + xPixel, y + yPixel));
                            }
                        }

                        // 更新进度条
                        this.ProcessBarStep();
                    }

                    int addedRow = this.grdFont.Rows.Add(rowImage);
                    this.grdFont.Rows[addedRow].Height = 32;
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
            }

        }

        /// <summary>
        /// 显示当前Cell的信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdFont_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                // 计算当前Cell字符宽度
                Bitmap cellImg = this.grdFont.Rows[e.RowIndex].Cells[e.ColumnIndex].Value as Bitmap;
                int minX = cellImg.Width;
                for (int y = 0; y < cellImg.Height; y++)
                {
                    for (int x = 0; x < cellImg.Width; x++)
                    {
                        Color color = cellImg.GetPixel(x, y);
                        if (color.A != 0)
                        {
                            if (x < minX)
                            {
                                minX = x;
                            }
                            break;
                        }
                    }
                }

                int maxX = 0;
                for (int y = 0; y < cellImg.Height; y++)
                {
                    for (int x = cellImg.Width - 1; x >= 0; x--)
                    {
                        Color color = cellImg.GetPixel(x, y);
                        if (color.A != 0)
                        {
                            if (x > maxX)
                            {
                                maxX = x;
                            }
                            break;
                        }
                    }
                }

                // 查看对应的旧（或新）字符
                string[] fontChars;
                if (this.isViewOld)
                {
                    fontChars = File.ReadAllLines(this.isComText ? cnTextFont : cnFileFont);
                }
                else
                {
                    fontChars = Bio1TextEditor.GetFontChars(this.isComText);
                }

                string otherChar = fontChars[e.RowIndex * 32 + e.ColumnIndex];

                this.Text = "Cell图片宽度 ： " + (maxX - minX) + "  对应的字符：" + otherChar;
            }
            catch
            { 
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

            string[] fontChars = File.ReadAllLines(this.isComText ? cnTextFont : cnFileFont);
            for (int i = 0; i < fontChars.Length; i++)
            {
                if (fontChars[i] == key)
                {
                    int row = i / 32;
                    int col = i % 32;
                    this.grdFont.Rows[row].Cells[col].Selected = true;
                    break;
                }
            }
        }

        /// <summary>
        /// 读取旧字库图片
        /// </summary>
        private void LoadOldFontBmp()
        {
            if (this.isComText)
            {
                Bitmap bio1ComFont1 = new Bitmap(@"..\EncodeFenxi\TextEditTools\Bio1Edit\bio1ComFont1.bmp");
                Bitmap bio1ComFont2 = new Bitmap(@"..\EncodeFenxi\TextEditTools\Bio1Edit\bio1ComFont2.bmp");

                this.fontBmp = new Bitmap(bio1ComFont1.Width, bio1ComFont1.Height + bio1ComFont2.Height);
                for (int y = 0; y < bio1ComFont1.Height; y += 1)
                {
                    for (int x = 0; x < bio1ComFont1.Width; x += 1)
                    {
                        this.fontBmp.SetPixel(x, y, bio1ComFont1.GetPixel(x, y));
                    }
                }

                for (int y = 0; y < bio1ComFont2.Height; y += 1)
                {
                    for (int x = 0; x < bio1ComFont2.Width; x += 1)
                    {
                        this.fontBmp.SetPixel(x, y + bio1ComFont1.Height, bio1ComFont2.GetPixel(x, y));
                    }
                }
            }
            else
            {
                this.fontBmp = new Bitmap(@"..\\EncodeFenxi\TextEditTools\Bio1Edit\bio1FileFont.bmp");
            }

            this.fontCharInfoLst.Clear();

            FileStream fs = null;
            byte[] byData;
            try
            {
                if (this.isWii)
                {
                    fs = new FileStream(this.baseFolder + @"\WiiJp\sys\main.dol", FileMode.Open);
                    fs.Seek(0x2ee900, SeekOrigin.Begin);
                    byData = new byte[0x2ef8d0 - 0x2ee900];
                }
                else
                {
                    fs = new FileStream(this.baseFolder + @"\&&systemdata\Start.dol", FileMode.Open);
                    fs.Seek(0x17d9d0, SeekOrigin.Begin);
                    byData = new byte[0x17e9a0 - 0x17d9d0];
                }

                fs.Read(byData, 0, byData.Length);
                fs.Close();

                for (int i = 0; i < byData.Length; i += 2)
                {
                    FontCharInfo charInfo = new FontCharInfo();
                    charInfo.LeftPadding = byData[i];
                    charInfo.Width = byData[i + 1];

                    this.fontCharInfoLst.Add(charInfo);
                }
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
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
                int yIndex = row >= 32 ? row - 32 : row;
                for (int col = 0; col < 32; col++)
                {
                    Bitmap cellImg = this.grdFont.Rows[row].Cells[col].Value as Bitmap;
                    for (int y = 0; y < 32; y++)
                    {
                        for (int x = 0; x < 32; x++)
                        {
                            fntBmp.SetPixel(col * 32 + x, yIndex * 32 + y, cellImg.GetPixel(x, y));
                        }
                    }
                }

                // 更新进度条
                this.ProcessBarStep();
            }

            fntBmp.Save(fontName);
        }

        /// <summary>
        /// 判断字库字符文件是否存在
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private bool IsFontCharFileExist(string file)
        {
            if (!File.Exists(file))
            {
                MessageBox.Show("字库字符文件：" + file + "不存在！");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 显示字库信息
        /// </summary>
        /// <param name="fontFile"></param>
        private void ViewFont(string fontFile)
        {
            // 判断字库字符文件是否存在
            if (!this.IsFontCharFileExist(fontFile))
            {
                return;
            }

            FileStream fs = null;
            this.grdFont.Rows.Clear();

            try
            {
                // 读取字库字符文件
                int maxRow = this.isComText ? 36 : 32;
                string[] fontChars = File.ReadAllLines(fontFile);
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
                int maxCharCount = 32 * 34 + 2;
                if (!this.isComText)
                {
                    maxCharCount = 32 * 29 + 6;
                }
                if (fontChars.Length > maxCharCount)
                {
                    MessageBox.Show("字库数超过了容量（目前：" + fontChars.Length + "，最大：" + maxCharCount + "，超出：" + (fontChars.Length - maxCharCount) + "），游戏可能无法运行！");
                }

                // 读取旧字库图片
                this.LoadOldFontBmp();

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
                    this.grdFont.Rows[addedRow].Height = 32;
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
            Bitmap bmp = new Bitmap(32, 32);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.FromArgb(0, 0, 0, 0));
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            Pen blackPen = new Pen(Color.Black, 0.01F);

            if (this.useOldFont.Contains(fontChar))
            {
                // 取得原有的字库
                int fontW = 32;
                int fontH = 32;
                int charFontIndex = 0;
                string[] diffData = Bio1TextEditor.GetDiffData(fontChar, this.isComText).Split(' ');
                int index = Convert.ToInt32(diffData[0]);

                // 根据旧字符宽度信息，设置新字符宽度信息
                if (this.isComText)
                {
                    this.newFontCharInfoLst.Add(this.fontCharInfoLst[index]);
                }
                else
                {
                    this.newFontCharInfoLst.Add(this.fontCharInfoLst[index + 32 * 34 + 2]);
                }

                // 取得旧字库图片
                for (int y = 0; y < fontBmp.Height; y += fontH)
                {
                    Image[] rowImage = new Image[this.fontBmp.Width / fontW];
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
                FontCharInfo charInfo = null;
                if (string.IsNullOrEmpty(fontChar))
                {
                    // 更新进度条
                    this.ProcessBarStep();

                    return bmp;
                }
                else if ("　" == fontChar)
                {
                    charInfo = new FontCharInfo();
                    charInfo.LeftPadding = 32;
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
                RectangleF rectangle = new RectangleF(0, 0, 32, 30);
                GraphicsPath graphPath = new GraphicsPath();

                graphPath.AddString(fontChar, new FontFamily(txtFontTest.Font.Name), (int)FontStyle.Regular, txtFontTest.Font.Size, rectangle, sf);
                g.FillPath(Brushes.White, graphPath);
                g.DrawPath(blackPen, graphPath);

                Region[] charRegions = g.MeasureCharacterRanges(fontChar, this.selectedFont, rectangle, sf);
                int charWidth = (int)charRegions[0].GetBounds(g).Width;
                int charLeftWidth = 32 + 1 - charWidth;

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

                charInfo = new FontCharInfo();
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