using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Hanhua.Common;

namespace Hanhua.ImgEditTools
{
    /// <summary>
    /// 简单的图片生成器
    /// 根据原来的图片，生成新的图片加文字
    /// </summary>
    public partial class SampleImgCreater : BaseForm
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public SampleImgCreater(string imgPath)
        {
            InitializeComponent();

            this.ResetHeight();

            // 初始化新旧图片
            this.InitImage(imgPath);
        }

        /// <summary>
        /// 初始化新旧图片
        /// </summary>
        /// <param name="imgPath"></param>
        private void InitImage(string imgPath)
        {
            // 设置旧的图片
            this.oldImg.Image = Image.FromFile(imgPath);
            this.oldImg.Width = this.oldImg.Image.Width;
            this.oldImg.Height = this.oldImg.Image.Height;

            // 生成新的图片
            Bitmap newImg = new Bitmap(this.oldImg.Image.Width, this.oldImg.Image.Height, this.oldImg.Image.PixelFormat);
            this.newImg.Width = this.oldImg.Image.Width;
            this.newImg.Height = this.oldImg.Image.Height;
            this.newImg.Image = newImg;

            // 预览
            this.Preview();
        }

        /// <summary>
        /// Copy图片的边框
        /// </summary>
        private void CopyBorder()
        {
            Bitmap oldImg = (Bitmap)this.oldImg.Image;
            Bitmap newImg = (Bitmap)this.newImg.Image;
            for (int y = 0; y < oldImg.Height; y++)
            {
                for (int x = 0; x < oldImg.Width; x++)
                {
                    if (x < 3 || x >= oldImg.Width - 3)
                    {
                        newImg.SetPixel(x, y, Color.White);
                    }
                    else if (y < 3 || y >= oldImg.Height - 3)
                    {
                        newImg.SetPixel(x, y, Color.White);
                    }
                }
            }
        }

        /// <summary>
        /// 变更字体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChgFont_Click(object sender, System.EventArgs e)
        {
            // 判断输入是否正确
            if (!this.IsInputRight())
            {
                return;
            }

            if (this.fontDialog.ShowDialog() == DialogResult.OK)
            {
                this.txtWenzi.Font = this.fontDialog.Font;

                // 预览
                this.Preview();
            }
        }

        /// <summary>
        /// 生成新图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreate_Click(object sender, System.EventArgs e)
        {
            // 判断输入是否正确
            if (!this.IsInputRight())
            {
                return;
            }

            // 预览
            this.Preview();
        }

        /// <summary>
        /// 预览
        /// </summary>
        private void Preview()
        {
            // 计算输入文字的矩形
            int x = Convert.ToInt32(this.txtX.Text);
            int y = Convert.ToInt32(this.txtY.Text);
            RectangleF rectangle = new RectangleF(x, y, this.oldImg.Width - x, this.oldImg.Height - y);
            GraphicsPath graphPath = new GraphicsPath();
            FontFamily family = new FontFamily(this.txtWenzi.Font.Name);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Far;
            CharacterRange[] characterRanges = { new CharacterRange(0, this.txtWenzi.Text.Length) };
            sf.SetMeasurableCharacterRanges(characterRanges);

            graphPath.AddString(this.txtWenzi.Text, family, (int)FontStyle.Bold, rectangle.Height, rectangle, sf);

            Graphics g = Graphics.FromImage(this.newImg.Image);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            g.Clear(Color.FromArgb(0, 0xFF, 0xFF, 0xFF));
            g.FillPath(Brushes.White, graphPath);

            if (this.chkColorChg.Checked)
            {
                Bitmap newImg = (Bitmap)this.newImg.Image;
                for (y = 0; y < newImg.Height; y++)
                {
                    for (x = 0; x < newImg.Width; x++)
                    {
                        Color color = newImg.GetPixel(x, y);
                        if (color.A == 0)
                        {
                            newImg.SetPixel(x, y, Color.White);
                        }
                        else
                        {
                            newImg.SetPixel(x, y, Color.FromArgb(0, color.R, color.G, color.B));
                        }
                    }
                }
            }
            //g.DrawPath(new Pen(Color.Black, 1), graphPath);

            // Copy图片的边框
            if (this.chkBorder.Checked)
            {
                this.CopyBorder();
            }

            this.newImg.Refresh();
        }

        /// <summary>
        /// 判断输入是否正确
        /// </summary>
        /// <returns></returns>
        private bool IsInputRight()
        {
            if (string.IsNullOrEmpty(this.txtX.Text)
                || !Util.IsNumber(this.txtX.Text))
            {
                MessageBox.Show("X坐标不正确！");
                this.txtX.Focus();
                return false;
            }
            else if (Convert.ToInt32(this.txtX.Text) >= this.oldImg.Width)
            {
                MessageBox.Show("X最大值超出！");
                this.txtX.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(this.txtY.Text)
                || !Util.IsNumber(this.txtY.Text))
            {
                MessageBox.Show("Y坐标不正确！");
                this.txtY.Focus();
                return false;
            }
            else if (Convert.ToInt32(this.txtY.Text) >= this.oldImg.Height)
            {
                MessageBox.Show("Y最大值超出！");
                this.txtY.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(this.txtWenzi.Text))
            {
                MessageBox.Show("输入的文字不正确！");
                this.txtWenzi.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            this.baseFile = Util.SetSaveDailog("图片文件（*.png）|*.png|所有文件|*.*", string.Empty);
            if (string.IsNullOrEmpty(this.baseFile))
            {
                return;
            }

            try
            {
                // 开始保存文件
                this.newImg.Image.Save(this.baseFile);
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
            }
        }
    }
}
