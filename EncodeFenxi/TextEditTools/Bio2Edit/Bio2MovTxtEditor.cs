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

namespace Hanhua.BioTools.Bio2Edit
{
    /// <summary>
    /// Bio2动画字幕编辑
    /// </summary>
    public partial class Bio2MovTxtEditor : BaseForm
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public Bio2MovTxtEditor()
        {
            InitializeComponent();

            this.txtCn.KeyDown += new KeyEventHandler(this.txtCn_KeyDown);
            //this.baseFolder = @"E:\Study\Hanhua\TodoCn\Bio2\movTxt\allJp\";
            this.baseFolder = @"E:\游戏汉化\NgcBio2\MovTxt\allJp\";
            this.PageLoad();
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

        #endregion

        #region " 私有方法 "

        /// <summary>
        /// 初始化
        /// </summary>
        private void PageLoad()
        {
            this.lstMov.Items.Clear();
            List<FilePosInfo> fileNameInfo = Util.GetAllFiles(this.baseFolder).Where(p => p.File.EndsWith(".png", StringComparison.OrdinalIgnoreCase)).ToList();

            foreach (FilePosInfo file in fileNameInfo)
            {
                this.lstMov.Items.Add(Util.GetShortFileName(file.File));
            }

            if (this.lstMov.Items.Count > 0)
            {
                this.lstMov.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 保存中文翻译
        /// </summary>
        private void SaveCnImg()
        {
            string movTxt = this.txtCn.Text.Trim();
            Bitmap bmp = new Bitmap(256, 32);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.Black);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Center;

            // 在指定的区域内写入特定汉字
            Graphics graphics = this.CreateGraphics();
            SizeF sizeF = graphics.MeasureString(movTxt, this.txtCn.Font);

            GraphicsPath graphPath = new GraphicsPath();
            RectangleF rectangle = new RectangleF((256 - sizeF.Width) / 2 - 10, 0, 256, 18);
            graphPath.AddString(movTxt, new FontFamily(this.txtCn.Font.Name), (int)FontStyle.Regular, 18, rectangle, sf);
            g.FillPath(Brushes.White, graphPath);

            this.imgCn.Image.Dispose();
            this.imgCn.Image = bmp;

            //Bitmap bmp2 = new Bitmap(bmp.Width, bmp.Height);
            ////将第一个bmp拷贝到bmp2中
            //Graphics draw = Graphics.FromImage(bmp2);
            //draw.DrawImage(bmp, 0, 0);
            //this.imgCn.Image = bmp2;
            //draw.Dispose();
            //bmp.Dispose();

            bmp.Save(this.baseFolder.Replace("allJp", "allCn") + this.lstMov.Items[this.lstMov.SelectedIndex].ToString());
        }

        /// <summary>
        /// 加载当前选择图片
        /// </summary>
        private void LoadImg()
        {
            string jpImg = this.baseFolder + this.lstMov.Items[this.lstMov.SelectedIndex].ToString();
            if (this.imgJp.Image != null)
            {
                this.imgJp.Image.Dispose();
            }
            if (this.imgCn.Image != null)
            {
                this.imgCn.Image.Dispose();
            }

            this.imgJp.Image = Image.FromFile(jpImg);
            this.imgCn.Image = Image.FromFile(jpImg.Replace("allJp", "allCn"));
        }

        #endregion
    }
}
