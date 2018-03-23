using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using Hanhua.Common;

namespace Hanhua.TextEditTools.Bio0Edit
{
    /// <summary>
    /// 生化危机0字库查看
    /// </summary>
    public partial class Bio0FontEditor : BaseForm
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="fontFile"></param>
        public Bio0FontEditor(string fontFile)
        {
            InitializeComponent();

            this.ResetHeight();

            // 绑定事件
            this.rdoFont1.CheckedChanged += new EventHandler(this.rdoFont_CheckedChanged);
            this.rdoFont2.CheckedChanged += new EventHandler(this.rdoFont_CheckedChanged);
            this.rdoFont3.CheckedChanged += new EventHandler(this.rdoFont_CheckedChanged);
            this.rdoFont4.CheckedChanged += new EventHandler(this.rdoFont_CheckedChanged);

            // 显示字库信息
            this.baseFile = @"..\Bio0Edit\jfont01.bin";
            this.ReadFontFile(this.baseFile);
        }

        #region " 页面事件 "

        /// <summary>
        /// 查看原始字库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewOld_Click(object sender, EventArgs e)
        {
            // 加载最原始的共通图片
            this.AddCommonFont();
        }

        /// <summary>
        /// 换其他字库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOtherFont_Click(object sender, EventArgs e)
        {
            // 打开要分析的文件
            this.baseFile = Util.SetOpenDailog("生化危机0 字库设定文件（*.bin）|*.bin", string.Empty);
            if (string.IsNullOrEmpty(this.baseFile))
            {
                return;
            }

            // 显示字库信息
            this.ReadFontFile(this.baseFile);
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
                this.baseFile = @"..\Bio0Edit\" + rdo.Text + ".bin";
                this.ReadFontFile(this.baseFile);
            }
        }

        #endregion

        #region " 私有方法 "

        /// <summary>
        /// 显示字库信息
        /// </summary>
        private void ReadFontFile(string fontFile)
        {
            FileStream fs = null;
            try
            {
                // 将文件中的数据，读取到byData中
                fs = new FileStream(fontFile, FileMode.Open);
                byte[] byData = new byte[fs.Length];
                fs.Read(byData, 0, byData.Length);
                fs.Close();
                fs = null;

                // 判断是否正确
                if (byData[0x03] != 0x2C || byData[0x07] != 0x0C || byData[0x09] != 0x60)
                {
                    MessageBox.Show("不是正确生化危机0字库设置文件！");
                    return;
                }

                // 读取当前字库图片
                string picNum = fontFile.Substring(fontFile.IndexOf(".bin") - 2, 2);
                Bitmap img = new Bitmap(@"..\Bio0Edit\bio0Font_" + picNum + ".bmp");

                // 取得当前字库信息
                int[] picPos = new int[] { 0x2C, 0x03EC, 0x0DEC, 0x17EC, 0x21EC, 0x2BEC, 0x35EC, 0x3FEC, 0x49EC };
                List<KeyValuePair<int, int>> charList = new List<KeyValuePair<int, int>>();
                List<Bio0CharInfo> charInfoList = new List<Bio0CharInfo>();
                int charIndex = 0;

                for (int i = 0; i < picPos.Length; i++)
                {
                    // 取得当前页的字库信息
                    byte[] fontPage;
                    if (i == 0)
                    {
                        fontPage = new byte[0x60 * 10];
                    }
                    else
                    {
                        fontPage = new byte[256 * 10];
                    }
                    Array.Copy(byData, picPos[i], fontPage, 0, fontPage.Length);

                    for (int j = 0; j < fontPage.Length; )
                    {
                        // 每10个字节标识一个字符信息
                        byte[] temp = new byte[10];
                        Array.Copy(fontPage, j, temp, 0, temp.Length);

                        // 设置当前字符信息
                        Bio0CharInfo charInfo = new Bio0CharInfo();
                        charInfo.ByCharInfo = temp;
                        charInfo.CharPage = i == 0 ? 0 : 0x60 + (i - 1);
                        charInfo.X = Util.GetOffset(temp, 2, 3);
                        charInfo.Y = Util.GetOffset(temp, 4, 5);
                        charInfo.Width = Util.GetOffset(temp, 6, 7);
                        charInfo.Height = Util.GetOffset(temp, 8, 9);

                        charInfo.IsUseSecondImg = Util.GetOffset(temp, 0, 1) > 0;

                        charInfoList.Add(charInfo);
                        charList.Add(new KeyValuePair<int, int>(charInfo.Y / 0x1C, charInfo.X / 0x1C));

                        j += 10;
                        charIndex++;
                    }
                }

                // 添加字库图片到Grid
                List<Bio0CharInfo> aaa = charInfoList.Where(p => p.IsUseSecondImg).ToList();
                this.SetFontCharToGrid(charInfoList, img);
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
                return;
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
        /// 添加字库图片到Grid
        /// </summary>
        /// <param name="charInfoList"></param>
        /// <param name="secondImg"></param>
        private void SetFontCharToGrid(List<Bio0CharInfo> charInfoList, Bitmap secondImg)
        {
            try
            {
                // 读取旧字库图片
                Bitmap comImg = new Bitmap(@"..\Bio0Edit\ComFont.png");
                this.grdFont.Rows.Clear();

                // 显示进度条
                this.ResetProcessBar(charInfoList.Count);

                int rowCells = 16;
                for (int i = 0; i < charInfoList.Count; i += rowCells)
                {
                    Bitmap[] rowImg = new Bitmap[rowCells];
                    int rowNum = this.grdFont.Rows.Add();
                    this.grdFont.Rows[rowNum].Height = 0x22;
                    if (i < 0x60)
                    {
                        this.grdFont.Rows[rowNum].HeaderCell.Value = "00";
                    }
                    else
                    {
                        this.grdFont.Rows[rowNum].HeaderCell.Value = ((i - 0x60) / 0xFF + 60).ToString();
                    }

                    for (int j = i; j < charInfoList.Count && j < i + rowCells; j++)
                    {
                        rowImg[j - i] = this.GetCellImage(comImg, secondImg, charInfoList[j]);

                        if (charInfoList[j].IsUseSecondImg)
                        {
                            this.grdFont.Rows[rowNum].Cells[j - i].Style.BackColor = Color.Blue;
                        }

                        // 更新进度条
                        this.ProcessBarStep();
                    }

                    this.grdFont.Rows[rowNum].SetValues(rowImg);
                }

                // 隐藏进度条
                this.CloseProcessBar();
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
        /// 取得当前字符的图片
        /// </summary>
        /// <param name="comImg"></param>
        /// <param name="secondImg"></param>
        /// <param name="charInfo"></param>
        /// <returns></returns>
        private Bitmap GetCellImage(Bitmap comImg, Bitmap secondImg, Bio0CharInfo charInfo)
        {
            Bitmap cellImg = new Bitmap(charInfo.Width, charInfo.Height, PixelFormat.Format32bppArgb);
            Bitmap fontImg = charInfo.IsUseSecondImg ? secondImg : comImg;
            if (charInfo.Y > fontImg.Height || charInfo.X > fontImg.Width
                || charInfo.Y + charInfo.Height > fontImg.Height || charInfo.X + charInfo.Width > fontImg.Width)
            {
                return cellImg;
            }

            for (int y = charInfo.Y; y < charInfo.Y + charInfo.Height; y++)
            {
                for (int x = charInfo.X; x < charInfo.X + charInfo.Width; x++)
                {
                    cellImg.SetPixel(x - charInfo.X, y - charInfo.Y, fontImg.GetPixel(x, y));
                }
            }

            return cellImg;
        }

        /// <summary>
        /// 加载最原始的共通图片
        /// </summary>
        private List<Bitmap> AddCommonFont()
        {
            try
            {
                int fontW = 0x1C;
                int fontH = 0x1C;

                // 读取旧字库图片
                Bitmap bio0Font = new Bitmap(@"..\Bio0Edit\bio0Font.bmp");
                List<Bitmap> commonFontImg = new List<Bitmap>();
                Bitmap blankImg = new Bitmap(fontW, fontH, PixelFormat.Format32bppArgb);

                // 显示进度条
                this.ResetProcessBar(16 * 16);

                for (int y = 0; y < 16; y++)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        Bitmap fontImg = new Bitmap(fontW, fontH, PixelFormat.Format32bppArgb);
                        for (int yPixel = 0; yPixel < fontH; yPixel++)
                        {
                            for (int xPixel = 0; xPixel < fontW; xPixel++)
                            {
                                fontImg.SetPixel(xPixel, yPixel, bio0Font.GetPixel(x * fontW + xPixel, y * fontH + yPixel));
                            }
                        }

                        commonFontImg.Add(fontImg);

                        // 更新进度条
                        this.ProcessBarStep();
                    }
                }

                // 隐藏进度条
                this.CloseProcessBar();

                // 添加空的图片
                for (int i = 0; i < 8; i++)
                {
                    commonFontImg.Insert(0, blankImg);
                }

                for (int i = 0; i < 7 * 16; i++)
                {
                    commonFontImg.Insert(0x98, blankImg);
                }

                for (int i = 0; i < 56; i++)
                {
                    commonFontImg.Add(blankImg);
                }

                // 添加到Grid
                this.grdFont.Rows.Clear();
                int rowCells = 16;
                for (int i = 0; i < commonFontImg.Count; i += rowCells)
                {
                    Bitmap[] rowImg = new Bitmap[rowCells];
                    int rowNum = this.grdFont.Rows.Add();
                    this.grdFont.Rows[rowNum].Height = 0x22;
                    if (i < 0x60)
                    {
                        this.grdFont.Rows[rowNum].HeaderCell.Value = "00";
                    }
                    else
                    {
                        this.grdFont.Rows[rowNum].HeaderCell.Value = ((i - 0x60) / 0xFF + 60).ToString();
                    }

                    for (int j = i; j < commonFontImg.Count && j < i + rowCells; j++)
                    {
                        rowImg[j - i] = commonFontImg[j];
                    }

                    this.grdFont.Rows[rowNum].SetValues(rowImg);
                }

                return commonFontImg;
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
                return null;
            }
            finally
            {
                // 隐藏进度条
                this.CloseProcessBar();
            }
        }

        #endregion
    }
}
