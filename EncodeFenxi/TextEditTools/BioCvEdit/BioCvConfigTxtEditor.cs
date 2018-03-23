using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Hanhua.Common;

namespace Hanhua.BioTools.BioCvEdit
{
    /// <summary>
    /// Bio2动画字幕编辑
    /// </summary>
    public partial class BioCvConfigTxtEditor : BaseForm
    {
        private List<string[]> picPos = new List<string[]>();

        /// <summary>
        /// 初始化
        /// </summary>
        public BioCvConfigTxtEditor()
        {
            InitializeComponent();

            this.txtCn.KeyDown += new KeyEventHandler(this.txtCn_KeyDown);
            //this.baseFolder = @"E:\Study\Hanhua\TodoCn\BioCv\BioCvNgcCn\Pic\optionNgc\";
            this.baseFolder = @"E:\游戏汉化\NgcBioCv\BioCvNgcCn\Pic\optionNgc\";
            this.baseFile = this.baseFolder + "adv_07.png";

            picPos.Clear();
            picPos.Add(new string[] { "250", "50", "240", "18" });
            picPos.Add(new string[] { "250", "90", "240", "18" });
            picPos.Add(new string[] { "250", "110", "240", "16" });
            picPos.Add(new string[] { "250", "130", "158", "18" });
            picPos.Add(new string[] { "250", "156", "158", "18" });
            picPos.Add(new string[] { "250", "180", "150", "15" });
            picPos.Add(new string[] { "250", "202", "100", "18" });
            picPos.Add(new string[] { "0", "224", "80", "18" });
            picPos.Add(new string[] { "134", "224", "300", "18" });
            picPos.Add(new string[] { "78", "246", "74", "18" });
            picPos.Add(new string[] { "256", "246", "200", "22" });
            picPos.Add(new string[] { "16", "264", "160", "20" });
            picPos.Add(new string[] { "260", "271", "200", "18" });
            picPos.Add(new string[] { "0", "285", "110", "18" });

            this.cmbPosInfo.Items.Clear();
            this.cmbPosInfo.SelectedIndex = -1;
            for (int i = 0; i < picPos.Count; i++)
            {
                this.cmbPosInfo.Items.Add(i.ToString());
            }

            this.cmbPosInfo.SelectedIndex = 0;

            this.PageLoad();
        }

        #region " 页面事件 "

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

            this.SaveCnImg();
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
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRe_Click(object sender, EventArgs e)
        {
            this.PageLoad();
        }

        private void cmbPosInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbPosInfo.SelectedIndex == -1)
            {
                return;
            }

            string[] picPosInfo = this.picPos[this.cmbPosInfo.SelectedIndex];
            this.txtX.Text = picPosInfo[0];
            this.txtY.Text = picPosInfo[1];
            this.txtW.Text = picPosInfo[2];
            this.txtH.Text = picPosInfo[3];
        }

        #endregion

        #region " 私有方法 "

        /// <summary>
        /// 初始化
        /// </summary>
        private void PageLoad()
        {
            Bitmap jpConfigImg = new Bitmap(this.baseFile);
            this.imgConfig.Image = jpConfigImg;

            int x = 0, y = 0, w = 0, h = 0;
            if (!string.IsNullOrEmpty(this.txtX.Text.Trim())) 
            {
                x = Convert.ToInt32(this.txtX.Text.Trim());
            }
            if (!string.IsNullOrEmpty(this.txtY.Text.Trim()))
            {
                y = Convert.ToInt32(this.txtY.Text.Trim());
            }
            if (!string.IsNullOrEmpty(this.txtW.Text.Trim()))
            {
                w = Convert.ToInt32(this.txtW.Text.Trim());
            }
            if (!string.IsNullOrEmpty(this.txtH.Text.Trim()))
            {
                h = Convert.ToInt32(this.txtH.Text.Trim());
            }

            if (y > 0 && w > 0 && h > 0)
            {
                Bitmap chkJpImg = new Bitmap(w, h);
                for (int chkY = 0; chkY < h; chkY++)
                {
                    for (int chkX = 0; chkX < w; chkX++)
                    {
                        chkJpImg.SetPixel(chkX, chkY, jpConfigImg.GetPixel(x + chkX, y + chkY));
                    }
                }

                this.imgJp.Image = chkJpImg;
            }
        }

        /// <summary>
        /// 保存中文翻译
        /// </summary>
        private void SaveCnImg()
        {
            string movTxt = this.txtCn.Text.Trim();
            Bitmap bmp = new Bitmap(Convert.ToInt32(this.txtW.Text), Convert.ToInt32(this.txtH.Text));
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.Transparent);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Center;

            // 在指定的区域内写入特定汉字
            Graphics graphics = this.CreateGraphics();
            SizeF sizeF = graphics.MeasureString(movTxt, this.txtCn.Font);

            Pen blackPen = new Pen(Color.FromArgb(0x42, 0x42, 0x42), 1F);
            blackPen.SetLineCap(LineCap.Round, LineCap.Round, DashCap.Round);
            blackPen.LineJoin = LineJoin.Round;

            GraphicsPath graphPath = new GraphicsPath();
            RectangleF rectangle = new RectangleF((256 - sizeF.Width) / 2 - 10, 0, 256, 18);
            graphPath.AddString(movTxt, new FontFamily(this.txtCn.Font.Name), (int)FontStyle.Regular, 18, rectangle, sf);
            g.FillPath(Brushes.White, graphPath);
            g.DrawPath(blackPen, graphPath);

            //this.imgCn.Image.Dispose();
            this.imgCn.Image = bmp;

            //Bitmap bmp2 = new Bitmap(bmp.Width, bmp.Height);
            ////将第一个bmp拷贝到bmp2中
            //Graphics draw = Graphics.FromImage(bmp2);
            //draw.DrawImage(bmp, 0, 0);
            //this.imgCn.Image = bmp2;
            //draw.Dispose();
            //bmp.Dispose();

            //bmp.Save(this.baseFolder.Replace("allJp", "allCn") + this.lstMov.Items[this.lstMov.SelectedIndex].ToString());
        }

        #endregion
    }
}
