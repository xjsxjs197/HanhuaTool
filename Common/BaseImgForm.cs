using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hanhua.Common;

namespace Hanhua.Common
{
    /// <summary>
    /// 图片处理基类
    /// </summary>
    public partial class BaseImgForm : BaseForm
    {
        #region " 全局变量 "

        /// <summary>
        /// 图片参数
        /// </summary>
        private ImgInfo imgInfo;

        /// <summary>
        /// 字库信息
        /// </summary>
        private List<KeyValuePair<string, string>> fontList;

        #endregion

        #region " 初始化 "

        /// <summary>
        /// 初始化
        /// </summary>
        public BaseImgForm()
        {
            InitializeComponent();

            this.InitPage();
        }

        #endregion

        #region " 各种事件 "

        /// <summary>
        /// 刷新页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReDraw_Click(object sender, EventArgs e)
        {
            // 生成Sample图片
            this.CreateSampleImg();
        }

        /// <summary>
        /// 字库变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 生成Sample图片
            this.CreateSampleImg();
        }

        /// <summary>
        /// 是否描边变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkNeedBorder_CheckedChanged(object sender, EventArgs e)
        {
            // 生成Sample图片
            this.CreateSampleImg();
        }

        #endregion

        #region " 私有方法 "

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitPage()
        {
            // 读取字库信息
            this.lstFont.Items.Clear();
            this.fontList = ImgUtil.GetFontList();
            foreach (KeyValuePair<string, string> fontInfo in this.fontList)
            {
                this.lstFont.Items.Add(fontInfo.Value);
            }
            this.lstFont.SelectedIndex = 0;

            // 绑定事件
            this.lstFont.SelectedIndexChanged += new System.EventHandler(this.lstFont_SelectedIndexChanged);

            // 生成Sample图片
            this.CreateSampleImg();
        }

        /// <summary>
        /// 生成Sample图片
        /// </summary>
        private void CreateSampleImg()
        {
            //this.pnlSample.Height = Convert.ToInt32(this.txtBlockH.Text) + 2;

            // 设置图片参数
            int blockImgW = Convert.ToInt32(this.txtBlockW.Text);
            int blockImgH = Convert.ToInt32(this.txtBlockH.Text);
            this.imgInfo = new ImgInfo(blockImgW, blockImgH);
            this.imgInfo.BlockImgW = blockImgW;
            this.imgInfo.BlockImgH = blockImgH;
            this.imgInfo.XPadding = Convert.ToInt32(this.txtXPadding.Text);
            this.imgInfo.YPadding = Convert.ToInt32(this.txtYPadding.Text);
            this.imgInfo.NeedBorder = this.chkNeedBorder.Checked;
            this.imgInfo.FontName = this.fontList[this.lstFont.SelectedIndex].Key;
            this.imgInfo.FontSize = Convert.ToInt32(this.txtFontSize.Text);

            // 开始写文字
            // 拆分字符
            char[] txtList = this.txtSample.Text.ToCharArray();
            Image[] rowImage = new Image[txtList.Length];
            for (int i = 0; i < txtList.Length; i++)
            {
                this.imgInfo.NewImg();

                // 设置当前字符
                imgInfo.CharTxt = txtList[i].ToString();

                // 生成当前块图片
                ImgUtil.WriteBlockImg(imgInfo);

                rowImage[i] = imgInfo.Bmp;
            }

            this.grdSampleImg.Rows.Clear();
            int addedRow = this.grdSampleImg.Rows.Add(rowImage);
            //this.grdSampleImg.Rows[addedRow].Height = 32;
        }

        #endregion
    }
}
