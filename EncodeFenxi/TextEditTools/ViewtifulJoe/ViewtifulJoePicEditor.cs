using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.IO;
using Hanhua.Common;

namespace Hanhua.TextEditTools.ViewtifulJoe
{
    /// <summary>
    /// 红侠乔伊图片文本编辑
    /// </summary>
    public partial class ViewtifulJoePicEditor : BaseForm
    {
        /// <summary>
        /// 图片的大小信息
        /// </summary>
        private List<string> picSizeInfo = new List<string>();

        /// <summary>
        /// 图片的翻译信息
        /// </summary>
        private List<string> picCnTxt = new List<string>();

        /// <summary>
        /// 记录图片文件信息
        /// </summary>
        List<List<string>> picFiles = new List<List<string>>();

        /// <summary>
        /// 图片的类型信息
        /// </summary>
        private List<int> picType = new List<int>();

        #region " 图片颜色数据 "

        private Color[] stageSelRColor = new Color[] {
                Color.FromArgb(247, 247, 247)
               ,Color.FromArgb(254,255,176)
               ,Color.FromArgb(254,239,239)
               ,Color.FromArgb(201,231,254)
               ,Color.FromArgb(254,239,255)
               ,Color.FromArgb(247,255,239)
               ,Color.FromArgb(239,239,247)
               ,Color.FromArgb(254,239,247)
               ,Color.FromArgb(245,231,120)
               ,Color.FromArgb(196,181,102) // todo
               ,Color.FromArgb(254,255,176)
               ,Color.FromArgb(254,239,239)
               ,Color.FromArgb(201,231,254)
               ,Color.FromArgb(254,239,255)
               ,Color.FromArgb(247,255,239)
               ,Color.FromArgb(239,239,247)
               ,Color.FromArgb(124,206,238) // todo
               ,Color.FromArgb(61,214,254)
            };
        private Color[] stageSelLColor = new Color[] {
                Color.FromArgb(140, 140, 140)
               ,Color.FromArgb(252,231,97)
               ,Color.FromArgb(233,64,65)
               ,Color.FromArgb(44,89,236)
               ,Color.FromArgb(178,81,179)
               ,Color.FromArgb(151,206,111)
               ,Color.FromArgb(84,73,179)
               ,Color.FromArgb(234,98,171)
               ,Color.FromArgb(217,98,163)
               ,Color.FromArgb(126,148,205) // todo
               ,Color.FromArgb(252,231,97)
               ,Color.FromArgb(233,64,65)
               ,Color.FromArgb(44,89,236)
               ,Color.FromArgb(178,81,179)
               ,Color.FromArgb(151,206,111)
               ,Color.FromArgb(84,73,179)
               ,Color.FromArgb(137,198,157) // todo
               ,Color.FromArgb(217,80,196)
            };

        private Color[] pazuruRColor = new Color[] {
                Color.FromArgb(255,255,231)
               ,Color.FromArgb(254,239,239)
               ,Color.FromArgb(201,231,254)
               ,Color.FromArgb(254,239,255)
               ,Color.FromArgb(247,255,239)
               ,Color.FromArgb(239,239,247)
               ,Color.FromArgb(254,255,176)
               ,Color.FromArgb(254,239,239)
               ,Color.FromArgb(201,231,254)
               ,Color.FromArgb(254,239,255)
               ,Color.FromArgb(247,255,239)
               ,Color.FromArgb(239,239,247)
            };
        private Color[] pazuruLColor = new Color[] {
                Color.FromArgb(255,247,99)
               ,Color.FromArgb(233,64,65)
               ,Color.FromArgb(44,89,236)
               ,Color.FromArgb(178,81,179)
               ,Color.FromArgb(151,206,111)
               ,Color.FromArgb(84,73,179)
               ,Color.FromArgb(252,231,97)
               ,Color.FromArgb(233,64,65)
               ,Color.FromArgb(44,89,236)
               ,Color.FromArgb(178,81,179)
               ,Color.FromArgb(151,206,111)
               ,Color.FromArgb(84,73,179)
            };

        #endregion

        /// <summary>
        /// 图片文字字体大小
        /// </summary>
        private int picTxtFontSize = 34;

        /// <summary>
        /// 图片字符宽度
        /// </summary>
        private int picCharWidth = 26;

        /// <summary>
        /// 初始化
        /// </summary>
        public ViewtifulJoePicEditor()
        {
            InitializeComponent();

            this.txtCn.KeyDown += new KeyEventHandler(this.txtCn_KeyDown);
            this.txtMoveRight.KeyDown += new KeyEventHandler(txtMoveRight_KeyDown);
            //this.baseFolder = @"E:\Study\MySelfProject\Hanhua\TodoCn\ViewtifulJoe\Pic\jp\";
            //this.baseFile = @"E:\Study\MySelfProject\Hanhua\TodoCn\ViewtifulJoe\Pic\picTxtInfo.xlsx";
            this.baseFolder = @"G:\游戏汉化\红侠乔伊\Pic\jp\";
            this.baseFile = @"G:\游戏汉化\红侠乔伊\Pic\picTxtInfo.xlsx";
            

            this.PageLoad(0);
        }

        #region " 页面事件 "

        /// <summary>
        /// 换图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstMov_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadImg();
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            string movTxt = this.txtCn.Text.Trim();
            if (string.IsNullOrEmpty(movTxt))
            {
                MessageBox.Show("请输入文本！");
                this.txtCn.Focus();
                return;
            }

            //this.SaveCnImg();
            //this.CreateBackgroundType4Img(Color.FromArgb(247, 247, 247), Color.FromArgb(140, 140, 140), "st085_298");
            //this.CreateEmptyType4Pic();
            this.CreateBackgroundType4Img307("st085_315.png");
        }

        /// <summary>
        /// 回车保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.btnSave_Click(this.btnSave, new EventArgs());
            }
        }

        /// <summary>
        /// 回车右移图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMoveRight_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.imgCn.Image = this.MoveImgRight((Bitmap)this.imgCn.Image, this.txtMoveRight.Text);
                this.imgCn.Image.Save(this.baseFolder.Replace(@"jp", @"cn") + this.lstPic.Items[this.lstPic.SelectedIndex].ToString());
            }
        }

        /// <summary>
        /// 换字库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChgFont_Click(object sender, EventArgs e)
        {
            try
            {
                FontDialog fontDialog = new System.Windows.Forms.FontDialog();
                fontDialog.Font = this.txtCn.Font;
                if (fontDialog.ShowDialog() == DialogResult.OK)
                {
                    this.txtCn.Font = fontDialog.Font;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 字体大小减1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFontD_Click(object sender, EventArgs e)
        {
            System.Drawing.Font newFont = new System.Drawing.Font(this.txtCn.Font.FontFamily, this.txtCn.Font.Size - 1, FontStyle.Bold);
            this.txtCn.Font = newFont;

            this.picTxtFontSize -= 1;
        }

        /// <summary>
        /// 字体大小加1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFontA_Click(object sender, EventArgs e)
        {
            System.Drawing.Font newFont = new System.Drawing.Font(this.txtCn.Font.FontFamily, this.txtCn.Font.Size + 1, FontStyle.Bold);
            this.txtCn.Font = newFont;

            this.picTxtFontSize += 1;
        }

        /// <summary>
        /// 刷新页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReload_Click(object sender, EventArgs e)
        {
            this.PageLoad(this.lstPic.SelectedIndex);
        }

        #endregion

        #region " 私有方法 "

        /// <summary>
        /// 生成关卡选择的图片
        /// </summary>
        private void SaveStageSelectImg()
        {
            string cnTxt = this.txtCn.Text.Trim();
            string imgName = this.picFiles[this.lstPic.SelectedIndex][0];
            int imgNameNo = Convert.ToInt32(imgName.Substring(14, 3));
            int imgIndex = imgNameNo - 298;
            int fontSize = 27;
            Point point = new Point(280, 20);
            Rectangle rect = new Rectangle(30, 4, 392, 34);
            // 生成背景图片
            string backgroundImg = this.baseFolder.Replace(@"jp", @"cn") + @"\" + imgName;
            Bitmap bmp = null;

            // 特殊处理
            if (imgName.IndexOf("298") >= 0)
            {
                fontSize = 36;
                point.X = 225;
                point.Y = 40;
                rect.Y = 20;
                rect.Height = 40;
                bmp = this.CreateBackgroundType4Img(stageSelRColor[imgIndex], stageSelLColor[imgIndex], imgName);
            }
            else if (imgName.IndexOf("304") >= 0)
            {
                fontSize = 25;
                point.X = 272;
                bmp = this.CreateBackgroundType4Img(stageSelRColor[imgIndex], stageSelLColor[imgIndex], imgName);
            }
            else if (imgNameNo >= 308)
            {
                File.Copy(backgroundImg, backgroundImg + "_bak.png", true);
                bmp = (Bitmap)Bitmap.FromFile(backgroundImg + "_bak.png");
                fontSize = 22;
                point.X = 245;
                point.Y = 19;
                if (imgNameNo == 313)
                {
                    fontSize = 20;
                    point.X = 235;
                    point.Y = 18;
                }
            }

            // 写入文字
            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            Pen blackPen = new Pen(Color.Black, 2F);

            // Create a horizontal linear gradient with four stops.
            LinearGradientBrush linearGradientBrush = new LinearGradientBrush(
                rect, stageSelRColor[imgIndex], stageSelLColor[imgIndex], LinearGradientMode.Vertical);
            GraphicsPath graphPath = new GraphicsPath();
            graphPath.AddString(cnTxt, new FontFamily(this.txtCn.Font.Name), (int)FontStyle.Bold, fontSize, point, sf);
            g.FillPath(linearGradientBrush, graphPath);
            g.DrawPath(blackPen, graphPath);

            this.imgCn.Image.Dispose();
            this.imgCn.Image = null;
            bmp.Save(backgroundImg);
            this.imgCn.Image = Image.FromFile(backgroundImg);
        }

        /// <summary>
        /// 生成拼图选择的图片
        /// </summary>
        private void SavePazuruImg()
        {
            string cnTxt = this.txtCn.Text.Trim();
            string imgName = this.picFiles[this.lstPic.SelectedIndex][0];
            int imgIndex = Convert.ToInt32(imgName.Substring(14, 2)) - 20;

            // 生成背景图片
            string emptyImgPath = this.baseFolder.Replace(@"jp", @"cn") + @"\st00-08\EmptyPazuruImg.png";
            Bitmap bmp = (Bitmap)Image.FromFile(emptyImgPath);
            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            Rectangle rect = new Rectangle(29, 2, 136, 59);

            // Create a horizontal linear gradient with four stops.
            LinearGradientBrush myHorizontalGradient = new LinearGradientBrush(
                rect, pazuruRColor[imgIndex], pazuruLColor[imgIndex], LinearGradientMode.Horizontal);
            g.FillRectangle(myHorizontalGradient, rect);


            // 写文字
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Near;

            Pen blackPen = new Pen(Color.Black, 2F);

            LinearGradientBrush linearGradientBrush = new LinearGradientBrush(
                new Rectangle(29, 15, 136, 35), pazuruRColor[imgIndex], pazuruLColor[imgIndex], LinearGradientMode.Vertical);
            GraphicsPath graphPath = new GraphicsPath();
            graphPath.AddString(cnTxt, new FontFamily(this.txtCn.Font.Name), (int)FontStyle.Bold, 29, new Point(96, 15), sf);
            g.FillPath(linearGradientBrush, graphPath);
            g.DrawPath(blackPen, graphPath);

            this.imgCn.Image.Dispose();
            string backgroundImg = this.baseFolder.Replace(@"jp", @"cn") + @"\" + imgName;
            bmp.Save(backgroundImg);
            this.imgCn.Image = Image.FromFile(backgroundImg);
        }

        /// <summary>
        /// 生成开始按钮说明的图片
        /// </summary>
        private void SaveStartMemoImg()
        {
            string cnTxt = this.txtCn.Text.Trim();
            string imgName = this.picFiles[this.lstPic.SelectedIndex][0];

            // 生成背景图片
            Bitmap bmp = new Bitmap(this.imgCn.Width, this.imgCn.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.Transparent);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            Rectangle rect = new Rectangle(29, 2, 136, 59);

            // 写文字
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Near;

            Pen blackPen = new Pen(Color.Black, 2F);

            LinearGradientBrush linearGradientBrush = new LinearGradientBrush(
                new Rectangle(29, 20, 136, 30), Color.FromArgb(255, 231, 247), Color.FromArgb(239, 66, 165), LinearGradientMode.Vertical);
            GraphicsPath graphPath = new GraphicsPath();
            graphPath.AddString(cnTxt, new FontFamily(this.txtCn.Font.Name), (int)FontStyle.Bold, 27, new Point(120, 15), sf);
            g.FillPath(linearGradientBrush, graphPath);
            g.DrawPath(blackPen, graphPath);

            this.imgCn.Image.Dispose();
            string backgroundImg = this.baseFolder.Replace(@"jp", @"cn") + @"\" + imgName;
            Bitmap newBmp = this.MoveImgRight(bmp, this.txtMoveRight.Text);
            newBmp.Save(backgroundImg);
            this.imgCn.Image = Image.FromFile(backgroundImg);
        }

        /// <summary>
        /// 生成类型4（关卡说明）图片的背景
        /// </summary>
        /// <param name="colorRight"></param>
        /// <param name="colorLeft"></param>
        private Bitmap CreateBackgroundType4Img307(string imgName)
        {
            string emptyImgPath = this.baseFolder.Replace(@"jp", @"cn") + @"\st00-08\EmptyType4Img.png";
            Bitmap bmp = (Bitmap)Image.FromFile(emptyImgPath);
            //Bitmap bmp = new Bitmap(472, 79);
            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            Rectangle rect = new Rectangle(30, 3, 392, 73);

            //Color[] colors = new Color[] { Color.FromArgb(198, 181, 99), Color.FromArgb(115, 189, 107), Color.FromArgb(123, 140, 206) };
            //Color[] colors = new Color[] { Color.FromArgb(115, 206, 239), 
            //    Color.FromArgb(156, 132, 189), 
            //    Color.FromArgb(247, 148, 132), 
            //    Color.FromArgb(255, 239, 156), 
            //    Color.FromArgb(132, 198, 156) };
            Color[] colors = new Color[] { Color.FromArgb(0, 222, 255), Color.FromArgb(0, 82, 255), Color.FromArgb(222, 82, 198) };
            float[] positions = new float[] { 0.0f, 0.5f, 1.0f };
            //float[] positions = new float[] { 0.0f, 0.4f, 0.45f, 0.5f, 1.0f };
            ColorBlend cb = new ColorBlend();
            cb.Colors = colors;
            cb.Positions = positions;

            LinearGradientBrush lineBrush = new LinearGradientBrush(rect, Color.Transparent, Color.Transparent, LinearGradientMode.Horizontal);
            lineBrush.InterpolationColors = cb;
            lineBrush.LinearColors = colors;
            //lineBrush.RotateTransform(-30f);

            g.FillRectangle(lineBrush, rect);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Near;

            Pen blackPen = new Pen(Color.FromArgb(100, 255, 255, 255), 3F);

            GraphicsPath graphPath = new GraphicsPath();
            graphPath.AddString("红侠乔伊 火热乱斗", new FontFamily("SimHei"), (int)FontStyle.Bold, 43, new Point(225, 20), sf);
            g.DrawPath(blackPen, graphPath);

            this.imgCn.Image.Dispose();
            string backgroundImg = this.baseFolder.Replace(@"jp", @"cn") + @"\" + imgName;
            bmp.Save(backgroundImg);
            //bmp.Dispose();
            this.imgCn.Image = Image.FromFile(backgroundImg);

            return bmp;
        }

        /// <summary>
        /// 生成类型4（关卡说明）图片的背景
        /// </summary>
        /// <param name="colorRight"></param>
        /// <param name="colorLeft"></param>
        private Bitmap CreateBackgroundType4Img(Color colorRight, Color colorLeft, string imgName)
        {
            string emptyImgPath = this.baseFolder.Replace(@"jp", @"cn") + @"\st00-08\EmptyType4Img.png";
            Bitmap bmp = (Bitmap)Image.FromFile(emptyImgPath);
            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            Rectangle rect = new Rectangle(30, 3, 392, 73);

            // Create a horizontal linear gradient with four stops.   
            LinearGradientBrush myHorizontalGradient = new LinearGradientBrush(
                rect, colorRight, colorLeft, LinearGradientMode.Horizontal);
            g.FillRectangle(myHorizontalGradient, rect);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Near;

            Pen blackPen = new Pen(Color.FromArgb(100, 255, 255, 255), 3F);

            GraphicsPath graphPath = new GraphicsPath();
            graphPath.AddString("红侠乔伊 火热乱斗", new FontFamily("SimHei"), (int)FontStyle.Bold, 43, new Point(225, 20), sf);
            g.DrawPath(blackPen, graphPath);

            this.imgCn.Image.Dispose();
            string backgroundImg = this.baseFolder.Replace(@"jp", @"cn") + @"\" + imgName;
            bmp.Save(backgroundImg);
            //bmp.Dispose();
            this.imgCn.Image = Image.FromFile(backgroundImg);

            return bmp;
        }

        /// <summary>
        /// 做成空白的类型4图片（关卡说明）
        /// </summary>
        private void CreateEmptyType4Pic()
        {
            string jpImgPath = this.baseFolder + @"\st00-08\st084_20.png";
            Bitmap emptyImg = (Bitmap)Image.FromFile(jpImgPath);
            for (int y = 2; y < emptyImg.Height - 2; y++)
            {
                for (int x = 30; x < emptyImg.Width - 29; x++)
                {
                    emptyImg.SetPixel(x, y, Color.Transparent);
                }
            }

            //emptyImg.Save(this.baseFolder.Replace(@"jp", @"cn") + @"\st00-08\EmptyType4Img.png");
            emptyImg.Save(this.baseFolder.Replace(@"jp", @"cn") + @"\st00-08\EmptyPazuruImg.png");
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void PageLoad(int selectedIndex)
        {
            this.lstPic.Items.Clear();
            this.picSizeInfo.Clear();
            this.picCnTxt.Clear();
            this.picFiles.Clear();

            // 读取配置文件信息
            Microsoft.Office.Interop.Excel.Workbook xBook = null;
            Microsoft.Office.Interop.Excel.Worksheet xSheet = null;

            try
            {
                // 创建Application对象 
                this.xApp = new Microsoft.Office.Interop.Excel.ApplicationClass();

                // 得到WorkBook对象, 打开已有的文件 
                xBook = this.xApp.Workbooks._Open(
                    this.baseFile,
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                // 取得相应的Sheet
                xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets[1];

                // 取得当前行内容
                int lineNum = 2;
                while (true)
                {
                    // 文件名
                    string fileNm = xSheet.get_Range("A" + lineNum, Missing.Value).Value2 as string;
                    if (string.IsNullOrEmpty(fileNm))
                    {
                        break;
                    }

                    // 大小信息
                    string sizeInfo = xSheet.get_Range("B" + lineNum, Missing.Value).Value2.ToString();
                    // 中文信息
                    string cnTxtVal = xSheet.get_Range("D" + lineNum, Missing.Value).Value2.ToString();
                    // 类型信息
                    string typeVal = xSheet.get_Range("F" + lineNum, Missing.Value).Value2.ToString();

                    // 追加图片信息
                    this.AddPicItem(fileNm, sizeInfo, cnTxtVal, typeVal);

                    lineNum++;
                }
            }
            catch (Exception me)
            {
                MessageBox.Show(this.baseFile + "\n" + me.Message);
            }
            finally
            {
                // 清空各种对象
                xSheet = null;
                xBook = null;
                if (this.xApp != null)
                {
                    this.xApp.Quit();
                    this.xApp = null;
                }
            }

            // 显示第一个
            if (this.lstPic.Items.Count > 0)
            {
                this.lstPic.SelectedIndex = selectedIndex;
            }
        }

        /// <summary>
        /// 追加图片信息
        /// </summary>
        /// <param name="fileNm"></param>
        /// <param name="sizeInfo"></param>
        /// <param name="cnTxtVal"></param>
        private void AddPicItem(string fileNm, string sizeInfo, string cnTxtVal, string typeVal)
        {
            List<string> fileList = new List<string>();

            // 判断是否是多个图片
            if (fileNm.IndexOf("\n") > 0)
            {
                fileList.AddRange(fileNm.Split('\n'));
            }
            else
            {
                fileList.Add(fileNm);
            }
            
            this.picFiles.Add(fileList);

            // 添加到列表中
            this.lstPic.Items.Add(fileList[0]);
            this.picSizeInfo.Add(sizeInfo);
            this.picCnTxt.Add(cnTxtVal);
            this.picType.Add(Convert.ToInt32(typeVal));
        }

        /// <summary>
        /// 保存中文翻译
        /// </summary>
        private void SaveCnImg()
        {
            // 设置页面状态
            this.SetPageStatus(false);

            int picType = this.picType[this.lstPic.SelectedIndex];
            if (picType == 4)
            {
                this.SaveStageSelectImg();
            }
            else if (picType == 5)
            {
                this.SavePazuruImg();
            }
            else if (picType == 6)
            {
                this.SaveStartMemoImg();
            }
            else
            {
                this.SaveComnCnImg(picType);
            }

            // 设置页面状态
            this.SetPageStatus(true);
        }

        /// <summary>
        /// 保存中文翻译
        /// </summary>
        private void SaveComnCnImg(int picType)
        {
            string cnTxt = this.txtCn.Text.Trim();
            Bitmap bmp = new Bitmap(this.imgCn.Width, this.imgCn.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.Transparent);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            Brush brushes = Brushes.White;
            bool changeBrushes = false;

            Pen blackPen = new Pen(Color.Black, 0.01F);


            float xPos = 0;
            float yPos = 0;
            string fontName = this.txtCn.Font.Name;
            if (picType == 1)
            {
                fontName = "Microsoft YaHei";
            }

            for (int i = 0; i < cnTxt.Length; i++)
            {
                string curChar = cnTxt.Substring(i, 1);

                if ("R".Equals(curChar))
                {
                    // 红色文字处理
                    if (changeBrushes == false)
                    {
                        changeBrushes = true;
                        brushes = Brushes.Red;
                    }
                    else
                    {
                        changeBrushes = false;
                        brushes = Brushes.White;
                    }
                    continue;
                }
                else if ("!".Equals(curChar))
                {
                    xPos -= 5;
                    yPos -= 5;
                }
                else if ("☆".Equals(curChar))
                {
                    // 设置特殊的图标
                    int iconWidth = this.SetIconChar(bmp, this.picSizeInfo[this.lstPic.SelectedIndex], (int)xPos);
                    if (this.lstPic.SelectedIndex == 57)
                    {
                        // 特殊处理左下角的一块区域
                        this.ClearCorner(bmp, (int)xPos);
                        xPos -= 5;
                    }
                    xPos += iconWidth + 2;
                    continue;
                }
                else if (Encoding.UTF8.GetByteCount(curChar) == 1)
                {
                    // 英文、符号
                    this.picCharWidth = 19;
                    xPos -= 5;
                }
                else
                {
                    yPos = 0;
                    this.picCharWidth = 26;
                    if (this.picTxtFontSize < 34)
                    {
                        this.picCharWidth = 22;
                    }

                    if (picType == 1)
                    {
                        this.picCharWidth = 28;
                        if ("・".Equals(curChar))
                        {
                            xPos -= 5;
                            this.picCharWidth = 23;
                        }
                    }
                }

                if (picType == 1)
                {
                    if ("4".Equals(curChar) || "8".Equals(curChar) || "2".Equals(curChar))
                    {
                        xPos += 5;
                    }
                    else if ("1".Equals(curChar) && cnTxt.IndexOf("10") < 0)
                    {
                        xPos += 5;
                    }
                }

                // 在指定的区域内写入特定汉字
                GraphicsPath graphPath = new GraphicsPath();
                RectangleF rectangle = new RectangleF(xPos, yPos, this.picCharWidth, bmp.Height);
                graphPath.AddString(curChar, new FontFamily(fontName), (int)FontStyle.Bold, this.picTxtFontSize, rectangle, sf);
                g.FillPath(brushes, graphPath);
                if (picType == 1)
                {
                    g.DrawPath(blackPen, graphPath);
                }

                xPos += this.picCharWidth + 2;
            }

            this.imgCn.Image.Dispose();
            this.imgCn.Image = this.MoveImgRight(bmp, this.txtMoveRight.Text);
            this.SaveCnImg(this.lstPic.SelectedIndex);
        }

        /// <summary>
        /// 设置特殊的图标
        /// </summary>
        /// <param name="cnImg"></param>
        /// <param name="sizeInfo"></param>
        private int SetIconChar(Bitmap cnImg, string sizeInfo, int xPos)
        {
            if (string.IsNullOrEmpty(sizeInfo))
            {
                return 0;
            }

            string[] sizeArray = sizeInfo.Split(':');
            string[] iconPosInfo = sizeArray[2].Split(',');
            int startX = Convert.ToInt32(iconPosInfo[0]);
            int endX = Convert.ToInt32(iconPosInfo[1]);
            xPos -= Convert.ToInt32(iconPosInfo[2]);

            // 将日语图片中的图标信息，copy到中文图片中
            Bitmap jpImg = (Bitmap)this.imgJp.Image;
            int startY = 0;
            for (int y = startY; y < cnImg.Height; y++)
            {
                for (int x = startX; x < endX; x++)
                {
                    if (xPos + x < cnImg.Width)
                    {
                        cnImg.SetPixel(xPos + x, y - startY, jpImg.GetPixel(x, y));
                    }
                }
            }

            return endX - startX;
        }

        /// <summary>
        /// 特殊处理左下角的一块区域
        /// </summary>
        /// <param name="cnImg"></param>
        private void ClearCorner(Bitmap cnImg, int xPos)
        {
            Color clearPoint = cnImg.GetPixel(3, 10);
            for (int y = 40; y < cnImg.Height; y++)
            {
                for (int x = -5; x < 26; x++)
                {
                    cnImg.SetPixel(x + xPos, y, clearPoint);
                }
            }
        }

        /// <summary>
        /// 保持图片
        /// </summary>
        /// <param name="index"></param>
        private void SaveCnImg(int index)
        {
            List<string> fileList = this.picFiles[index];
            string targetFile = string.Empty;
            foreach (string fileNm in fileList)
            {
                targetFile = this.baseFolder.Replace(@"jp", @"cn") + fileNm;
                if (!File.Exists(targetFile))
                {
                    string shortName = Util.GetShortName(targetFile);
                    System.IO.Directory.CreateDirectory(targetFile.Replace(shortName, string.Empty));
                }

                this.imgCn.Image.Save(targetFile);
            }
        }

        /// <summary>
        /// 设置页面状态
        /// </summary>
        /// <param name="status"></param>
        private void SetPageStatus(bool status)
        {
            this.lstPic.Enabled = status;
            this.btnSave.Enabled = status;
        }

        /// <summary>
        /// 加载当前选择图片
        /// </summary>
        private void LoadImg()
        {
            // 设置图片
            int index = this.lstPic.SelectedIndex;
            string jpImg = this.baseFolder + this.lstPic.Items[index].ToString();
            string cnImg = jpImg.Replace(@"\jp", @"\cn");
            if (!File.Exists(cnImg))
            {
                cnImg = jpImg;
            }
            if (this.imgJp.Image != null)
            {
                this.imgJp.Image.Dispose();
            }
            if (this.imgCn.Image != null)
            {
                this.imgCn.Image.Dispose();
            }

            this.imgJp.Image = Image.FromFile(jpImg);
            this.imgCn.Image = Image.FromFile(cnImg);

            this.imgJp.Width = this.imgJp.Image.Width;
            this.imgJp.Height = this.imgJp.Image.Height;
            this.imgCn.Width = this.imgCn.Image.Width;
            this.imgCn.Height = this.imgCn.Image.Height;
            this.txtCn.Width = this.imgCn.Image.Width;

            // 设置文本
            this.txtCn.Text = this.picCnTxt[index];

            // 设置字体等信息
            this.SetFontInfo(index);
        }

        /// <summary>
        /// 设置字体等信息
        /// </summary>
        /// <param name="sizeInfo"></param>
        /// <returns></returns>
        private void SetFontInfo(int index)
        {
            string sizeInfo = this.picSizeInfo[index];
            if (string.IsNullOrEmpty(sizeInfo))
            {
                return;
            }

            // 设置字体大小
            string[] info = sizeInfo.Split(':');
            this.picTxtFontSize = Convert.ToInt32(info[0]);

            // 设置向右的偏移
            this.txtMoveRight.Text = info[1];
        }

        /// <summary>
        /// 图片向右移动
        /// </summary>
        /// <returns></returns>
        private Bitmap MoveImgRight(Bitmap bmp, string rightTxt)
        {
            Bitmap newBmp = new Bitmap(bmp.Width, bmp.Height);
            int startX = Convert.ToInt32(rightTxt);
            int xPos = 0;

            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    xPos = x + startX;
                    if (xPos < newBmp.Width)
                    {
                        newBmp.SetPixel(xPos, y, bmp.GetPixel(x, y));
                    }
                }
            }

            bmp.Dispose();

            return newBmp;
        }

        #endregion
    }
}
