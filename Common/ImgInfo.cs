using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Hanhua.Common
{
    /// <summary>
    /// 写图片的时的共通信息
    /// </summary>
    public class ImgInfo
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public ImgInfo(int imgW, int imgH)
        {
            // 初始化参数
            this.Brush = Brushes.White;
            this.Pen = new Pen(Color.Black, 1F);

            this.ImgW = imgW;
            this.ImgH = imgH;
            this.NewImg();

            this.Sf = new StringFormat();
            this.Sf.Alignment = StringAlignment.Center;
            this.Sf.LineAlignment = StringAlignment.Center;

            this.FontName = "Microsoft YaHei";
            this.FontSize = 20;

            this.XPadding = 2;
            this.YPadding = 2;

            this.NeedBorder = true;
            this.FontStyle = FontStyle.Bold;
        }

        /// <summary>
        /// 重新初始化图片
        /// </summary>
        public void NewImg()
        {
            this.Bmp = new Bitmap(this.ImgW, this.ImgH);
            this.Grp = Graphics.FromImage(this.Bmp);
            this.Grp.SmoothingMode = SmoothingMode.HighQuality;
            this.Grp.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.PosX = 0;
            this.PosY = 0;
        }

        /// <summary>
        /// 当前文字
        /// </summary>
        public string CharTxt { get; set; }

        /// <summary>
        /// 字库名称
        /// </summary>
        public string FontName { get; set; }

        /// <summary>
        /// 文字大小
        /// </summary>
        public float FontSize { get; set; }

        /// <summary>
        /// 文字位置
        /// </summary>
        public int PosX { get; set; }

        /// <summary>
        /// 文字位置
        /// </summary>
        public int PosY { get; set; }

        /// <summary>
        /// 文字位置微调
        /// </summary>
        public int XPadding { get; set; }

        /// <summary>
        /// 文字位置微调
        /// </summary>
        public int YPadding { get; set; }

        /// <summary>
        /// 文字对齐信息
        /// </summary>
        public StringFormat Sf { get; set; }

        /// <summary>
        /// 当前图片
        /// </summary>
        public Bitmap Bmp { get; set; }

        /// <summary>
        /// 当前画图
        /// </summary>
        public Graphics Grp { get; set; }

        /// <summary>
        /// 是否需要描边
        /// </summary>
        public bool NeedBorder { get; set; }

        /// <summary>
        /// 当前画笔
        /// </summary>
        public Pen Pen { get; set; }

        /// <summary>
        /// 当前笔刷
        /// </summary>
        public Brush Brush { get; set; }

        /// <summary>
        /// 块图片的宽
        /// </summary>
        public int BlockImgW { get; set; }

        /// <summary>
        /// 块图片的高
        /// </summary>
        public int BlockImgH { get; set; }

        /// <summary>
        /// 图片的宽
        /// </summary>
        public int ImgW { get; set; }

        /// <summary>
        /// 图片的高
        /// </summary>
        public int ImgH { get; set; }

        /// <summary>
        /// 图片的高
        /// </summary>
        public FontStyle FontStyle { get; set; }
    }
}
