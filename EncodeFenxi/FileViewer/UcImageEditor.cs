using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Hanhua.Common;
using Hanhua.ImgEditTools;

namespace Hanhua.FileViewer
{
    /// <summary>
    /// 图片查看页面
    /// </summary>
    public partial class UcImageEditor : UserControl
    {
        #region " 私有变量 "

        /// <summary>
        /// 右键菜单
        /// </summary>
        private ContextMenuStrip contextMenu = new ContextMenuStrip();

        /// <summary>
        /// 选择的节点
        /// </summary>
        private int selectedIndex;

        /// <summary>
        /// 图片格式
        /// </summary>
        private string imgFormat;

        /// <summary>
        /// 要拖拽的图片
        /// </summary>
        private Image dragDropImg;

        /// <summary>
        /// 保存旧的数据
        /// </summary>
        private byte[] byOldData;

        /// <summary>
        /// 保存ImageHeader信息
        /// </summary>
        private List<ImageHeader> tplImageInfo;

        /// <summary>
        /// 对外暴漏的保存事件，用于从Szs直接打开Tpl数据的情况
        /// </summary>
        public delegate void SaveAs(byte[] newData);
        public SaveAs OutSaveFunc = null;

        /// <summary>
        /// 对外暴露的设置图片信息
        /// </summary>
        /// <param name="imgInfo"></param>
        public delegate void SetTplImgInfo(string imgInfo);
        public SetTplImgInfo OutSetText = null;

        /// <summary>
        /// 暴漏给外部的文件名
        /// </summary>
        public string strFileOpen { get; set; }

        #endregion

        #region " 构造方法 "

        /// <summary>
        /// 初始化
        /// </summary>
        public UcImageEditor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public UcImageEditor(Image[] tplImages, List<ImageHeader> tplImageInfo, byte[] byTplData)
        {
            InitializeComponent();

            this.ViewImage(tplImages, tplImageInfo, byTplData);
        }

        /// <summary>
        /// 开始查看图片
        /// </summary>
        /// <param name="tplImages"></param>
        /// <param name="tplImageInfo"></param>
        /// <param name="byTplData"></param>
        public void ViewImage(Image[] tplImages, List<ImageHeader> tplImageInfo, byte[] byTplData)
        {
            this.byOldData = byTplData;
            this.tplImageInfo = tplImageInfo;

            // 设置右键菜单
            this.SetContextMenu(tplImageInfo);

            // 显示图片
            this.DisplayTplImage(tplImages, tplImageInfo);
        }

        #endregion

        #region " 页面事件 "

        /// <summary>
        /// 弹出右键菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tplImageView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            // 只有右键弹出菜单
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0 && e.RowIndex < this.tplImageView.Rows.Count)
                {
                    // 只有存在数据才弹出菜单
                    selectedIndex = e.RowIndex;
                    Point p = Control.MousePosition;
                    this.contextMenu.Show(p);
                }
            }

            this.dragDropImg = null;
        }

        /// <summary>
        /// 右键菜单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            this.contextMenu.Visible = false;
            switch (e.ClickedItem.Name)
            {
                // 导出
                case "export":
                    this.ExportSelectedImg(RotateFlipType.RotateNoneFlipNone);
                    break;

                // 翻转导出
                case "exportFlipUpDown":
                    this.ExportSelectedImg(RotateFlipType.Rotate180FlipX);
                    break;

                // 导出所有
                case "exportAll":
                    this.ExportAllImg(RotateFlipType.RotateNoneFlipNone);
                    break;

                // 翻转导出所有
                case "exportFlipUpDownAll":
                    this.ExportAllImg(RotateFlipType.Rotate180FlipX);
                    break;

                // 导入
                case "import":
                    this.ImportTplImg(RotateFlipType.RotateNoneFlipNone);
                    break;

                // 翻转导入
                case "importFlipUpDown":
                    this.ImportTplImg(RotateFlipType.Rotate180FlipX);
                    break;

                // 导入所有
                case "importAll":
                    this.ImportAllImg(RotateFlipType.RotateNoneFlipNone);
                    break;

                // 翻转导入所有
                case "importFlipUpDownAll":
                    this.ImportAllImg(RotateFlipType.Rotate180FlipX);
                    break;

                // 合成
                case "merge":
                    this.MergeImg();
                    break;
            }
        }

        /// <summary>
        /// 处理拖拽开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tplImageView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < this.tplImageView.Rows.Count - 1 && this.OutSetText != null)
            {
                this.dragDropImg = (Image)this.tplImageView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                ImageHeader headerInfo = this.tplImageInfo[e.RowIndex];
                this.imgFormat = headerInfo.Format;
                this.OutSetText("Format : " + headerInfo.Format
                    + " ,W : " + headerInfo.Width
                    + " ,H : " + headerInfo.Height
                    + " ,Number : " + this.tplImageInfo.Count);
            }
        }

        /// <summary>
        /// 处理拖拽中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tplImageView_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left
                && this.dragDropImg != null)
            {
                // 开始拖拽
                DragDropEffects dropEffect = this.tplImageView.DoDragDrop(this.dragDropImg, DragDropEffects.Copy);
            }
        }

        /// <summary>
        /// 到达目标单元格时的判断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tplImageView_DragOver(object sender, DragEventArgs e)
        {
            // 如果没有数据，禁止拖拽
            if (!e.Data.GetDataPresent(typeof(Bitmap)))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            // 如果不在新库的位置，禁止拖拽
            Point p = this.tplImageView.PointToClient(new Point(e.X, e.Y));
            DataGridView.HitTestInfo hit = this.tplImageView.HitTest(p.X, p.Y);

            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                if (hit.RowIndex == 0)
                {
                    e.Effect = DragDropEffects.Copy;
                    return;
                }
            }

            e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// 拖拽结束的处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tplImageView_DragDrop(object sender, DragEventArgs e)
        {
            // 取得目标单元格
            Point p = this.tplImageView.PointToClient(new Point(e.X, e.Y));
            DataGridView.HitTestInfo hit = this.tplImageView.HitTest(p.X, p.Y);

            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                // 复制图片

            }
        }

        /// <summary>
        /// 合成图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMerge_Click(object sender, EventArgs e)
        {
            // 输入检查
            string strX = this.txtX.Text.Trim();
            if (string.IsNullOrEmpty(strX)
                || !Util.IsNumber(strX))
            {
                MessageBox.Show("X值不正确！");
                this.txtX.Focus();
                return;
            }

            string strY = this.txtY.Text.Trim();
            if (string.IsNullOrEmpty(strY)
                || !Util.IsNumber(strX))
            {
                MessageBox.Show("X值不正确！");
                this.txtY.Focus();
                return;
            }

            int mergeX = Convert.ToInt32(strX);
            int mergeY = Convert.ToInt32(strY);
            int secImgX = 0;
            int secImgY = 0;
            Bitmap oldImg = this.dragDropImg as Bitmap;
            Bitmap secImg = this.tplImageView.Rows[1].Cells[0].Value as Bitmap;
            Bitmap newImg = new Bitmap(oldImg.Width, oldImg.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            for (int y = 0; y < oldImg.Height; y++)
            {
                for (int x = 0; x < oldImg.Width; x++)
                {
                    if (x >= mergeX && y >= mergeY
                        && secImgX < secImg.Width && secImgY < secImg.Height)
                    {
                        newImg.SetPixel(x, y, secImg.GetPixel(secImgX++, secImgY));
                    }
                    else
                    {
                        newImg.SetPixel(x, y, oldImg.GetPixel(x, y));
                    }
                }
                if (secImgX != 0)
                {
                    secImgX = 0;
                    secImgY++;
                }
            }

            this.tplImageView.Rows[0].Cells[0].Value = newImg;
        }

        #endregion

        #region " 私有方法 "

        /// <summary>
        /// 显示图片
        /// </summary>
        /// <param name="tplImages"></param>
        private void DisplayTplImage(Image[] tplImages, List<ImageHeader> tplImageInfo)
        {
            if (tplImages == null)
            {
                return;
            }

            // 取最大宽度和长度
            int maxWidth = tplImageInfo[0].Width;
            int maxHeight = tplImageInfo[0].Height;
            for (int i = 1; i < tplImageInfo.Count; i++)
            {
                if (tplImageInfo[i].Width > maxWidth)
                {
                    maxWidth = tplImageInfo[i].Width;
                }

                if (tplImageInfo[i].Height > maxHeight)
                {
                    maxHeight = tplImageInfo[i].Height;
                }
            }

            // 设置页面长宽
            ////if (maxWidth > this.Width)
            ////{
            ////    this.Width = maxWidth + 40;
            ////}
            ////if (maxHeight > this.Height)
            ////{
            ////    this.Height = maxHeight + 40;
            ////}

            this.tplImageView.Columns[0].Width = maxWidth;
            this.Text = "Format : " + tplImageInfo[0].Format
                + " ,W : " + tplImageInfo[0].Width
                + " ,H : " + tplImageInfo[0].Height
                + " ,Number : " + tplImages.Length;

            this.tplImageView.Rows.Clear();
            for (int i = 0; i < tplImages.Length; i++)
            {
                this.tplImageView.Rows.Add(new object[] { tplImages[i] });
                this.tplImageView.Rows[i].Height = tplImages[i].Height;
            }
        }

        /// <summary>
        /// 设置右键菜单
        /// </summary>
        private void SetContextMenu(List<ImageHeader> tplImageInfo)
        {
            // 绑定右键菜单事件
            this.contextMenu.ItemClicked -= new ToolStripItemClickedEventHandler(this.ContextMenu_ItemClicked);
            this.contextMenu.ItemClicked += new ToolStripItemClickedEventHandler(this.ContextMenu_ItemClicked);

            // 添加菜单
            this.contextMenu.Items.Clear();
            ToolStripMenuItem item = new ToolStripMenuItem();
            item.Name = "export";
            item.Text = "导出";
            this.contextMenu.Items.Add(item);

            item = new ToolStripMenuItem();
            item.Name = "exportAll";
            item.Text = "导出所有";
            if (tplImageInfo.Count <= 1)
            {
                //item.Enabled = false;
            }
            this.contextMenu.Items.Add(item);
            this.contextMenu.Items.Add(new ToolStripSeparator());

            item = new ToolStripMenuItem();
            item.Name = "exportFlipUpDown";
            item.Text = "上下翻转导出";
            this.contextMenu.Items.Add(item);

            item = new ToolStripMenuItem();
            item.Name = "exportFlipUpDownAll";
            item.Text = "上下翻转导出所有";
            if (tplImageInfo.Count <= 1)
            {
                item.Enabled = false;
            }
            this.contextMenu.Items.Add(item);
            this.contextMenu.Items.Add(new ToolStripSeparator());

            item = new ToolStripMenuItem();
            item.Name = "import";
            item.Text = "导入";
            if (this.byOldData == null)
            {
                item.Enabled = false;
            }
            this.contextMenu.Items.Add(item);

            item = new ToolStripMenuItem();
            item.Name = "importAll";
            item.Text = "导入所有";
            if (tplImageInfo.Count <= 1 || this.byOldData == null)
            {
                //item.Enabled = false;
            }
            this.contextMenu.Items.Add(item);
            this.contextMenu.Items.Add(new ToolStripSeparator());

            item = new ToolStripMenuItem();
            item.Name = "importFlipUpDown";
            item.Text = "上下翻转导入";
            if (this.byOldData == null)
            {
                item.Enabled = false;
            }
            this.contextMenu.Items.Add(item);

            item = new ToolStripMenuItem();
            item.Name = "importFlipUpDownAll";
            item.Text = "上下翻转导入所有";
            if (tplImageInfo.Count <= 1 || this.byOldData == null)
            {
                item.Enabled = false;
            }
            this.contextMenu.Items.Add(item);
            this.contextMenu.Items.Add(new ToolStripSeparator());

            item = new ToolStripMenuItem();
            item.Name = "merge";
            item.Text = "合成";
            if ("bmp".Equals(tplImageInfo[0].Format))
            {
                item.Enabled = true;
                this.panelParam.Visible = true;
            }
            else
            {
                item.Enabled = false;
            }

            this.contextMenu.Items.Add(item);
        }

        /// <summary>
        /// 简单合成图片
        /// </summary>
        private void MergeImg()
        {
            // 打开要分析的文件
            string openFile = Util.SetOpenDailog("图片类型数据(*.png)|*.png|所有类型(*.*)|*.*", string.Empty);
            if (string.IsNullOrEmpty(openFile))
            {
                return;
            }

            this.dragDropImg = this.tplImageView.Rows[0].Cells[0].Value as Image;
            Bitmap img = new Bitmap(openFile);
            int rowIndex = this.tplImageView.Rows.Add(new object[] { img });
            this.tplImageView.Rows[rowIndex].Height = img.Height;
        }

        /// <summary>
        /// 导入Tpl文件
        /// </summary>
        private void ImportTplImg(RotateFlipType rotateFlipType)
        {
            // 打开要分析的文件
            string openFile = Util.SetOpenDailog("图片类型数据(*.png)|*.bmp;*.png|所有类型(*.*)|*.*", string.Empty);
            if (string.IsNullOrEmpty(openFile))
            {
                return;
            }

            this.ImportImgByIndex(this.selectedIndex, openFile, rotateFlipType);

            // 保存字节数据
            this.CallSaveFunc();
        }

        /// <summary>
        /// 导入所有Tpl文件
        /// </summary>
        private void ImportAllImg(RotateFlipType rotateFlipType)
        {
            string folder = Util.OpenFolder(string.Empty);
            if (string.IsNullOrEmpty(folder))
            {
                return;
            }

            try
            {
                // 取得保存的文件名
                string[] filePaths = this.strFileOpen.Split('.')[0].Split('\\');
                string openFilePath = Path.Combine(folder, filePaths[filePaths.Length - 1]);

                // 循环保存
                for (int i = 0; i < this.tplImageInfo.Count; i++)
                {
                    string importFile = openFilePath + "_" + i.ToString().PadLeft(3, '0') + ".png";
                    if (!File.Exists(importFile))
                    {
                        //MessageBox.Show("需要导入的图片不存在！\n" + importFile);
                    }
                    else
                    {
                        this.ImportImgByIndex(i, importFile, rotateFlipType);
                    }
                }

                // 保存字节数据
                this.CallSaveFunc();
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
            }
        }

        /// <summary>
        /// 导入图片
        /// </summary>
        /// <param name="index">导入图片在Grid的位置</param>
        /// <param name="filePath">图片的存放位置</param>
        /// <param name="rotateFlipType">是否翻转</param>
        private void ImportImgByIndex(int index, string filePath, RotateFlipType rotateFlipType)
        {
            ImageHeader headerInfo = this.tplImageInfo[index];
            Bitmap importImg = new Bitmap(filePath);
            importImg.RotateFlip(rotateFlipType);

            byte[] byImg;
            List<byte> byPalette = new List<byte>();

            if (headerInfo.Format.Equals("C4_CI4")
                || headerInfo.Format.Equals("C8_CI8")
                || headerInfo.Format.Equals("C14X2_CI14x2"))
            {
                byImg = Util.PaletteImageEncode(importImg, headerInfo.Format, byPalette, headerInfo.PaletteFormat);
            }
            else
            {
                byImg = Util.ImageEncode(importImg, headerInfo.Format);
            }

            // 替换Tpl文件数据
            TplFileManager tplEditor = new TplFileManager();
            this.byOldData = tplEditor.TplImgImport(this.byOldData, index, byImg, byPalette.ToArray());

            // 更新图片
            Image img;
            if ("CMPR".Equals(headerInfo.Format))
            {
                img = Util.CmprImageDecode(
                    new Bitmap(importImg.Width, importImg.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb), byImg);
            }
            else if (headerInfo.Format.Equals("C4_CI4")
                || headerInfo.Format.Equals("C8_CI8")
                || headerInfo.Format.Equals("C14X2_CI14x2"))
            {
                img = Util.PaletteImageDecode(
                    new Bitmap(importImg.Width, importImg.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb),
                    byImg, headerInfo.Format, byPalette.ToArray(), headerInfo.PaletteFormat);
            }
            else
            {
                img = Util.ImageDecode(
                    new Bitmap(importImg.Width, importImg.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb),
                    byImg, headerInfo.Format);
            }

            this.tplImageView.Rows[index].Cells[0].Value = img;
        }

        /// <summary>
        /// 导出选择节点的图片
        /// </summary>
        private void ExportSelectedImg(RotateFlipType rotateFlipType)
        {
            string openFile = Util.SetSaveDailog("图片类型数据(*.png)|*.png|所有类型(*.*)|*.*", string.Empty);
            if (string.IsNullOrEmpty(openFile))
            {
                return;
            }

            try
            {
                Image selectedImg = (Image)this.tplImageView.Rows[this.selectedIndex].Cells[0].Value;
                selectedImg.RotateFlip(rotateFlipType);

                // 开始保存文件
                selectedImg.Save(openFile);
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
            }
        }

        /// <summary>
        /// 导出所有图片
        /// </summary>
        private void ExportAllImg(RotateFlipType rotateFlipType)
        {
            string folder = Util.OpenFolder(string.Empty);
            if (string.IsNullOrEmpty(folder))
            {
                return;
            }

            try
            {
                // 取得保存的文件名
                string[] filePaths = this.strFileOpen.Split('.')[0].Split('\\');
                string savePath = filePaths[filePaths.Length - 1];

                // 循环保存
                for (int i = 0; i < this.tplImageInfo.Count; i++)
                {
                    Image selectedImg = (Image)this.tplImageView.Rows[i].Cells[0].Value;
                    selectedImg.RotateFlip(rotateFlipType);

                    // 开始保存文件
                    selectedImg.Save(Path.Combine(folder, savePath) + "_" + i.ToString().PadLeft(3, '0') + ".png");
                }

                MessageBox.Show("成功导出所有图片。");

                // 打开此文件。
                System.Diagnostics.Process.Start(folder);
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
            }
        }

        /// <summary>
        /// 保存最新的数据
        /// </summary>
        private void CallSaveFunc()
        {
            // 如果设置了外部保存
            if (this.OutSaveFunc != null)
            {
                this.OutSaveFunc(this.byOldData);

                MessageBox.Show("当前打开的文件的数据已经变更，不要忘记保存！");
            }
            else
            {
                // 保存当前的文件
                if (!string.IsNullOrEmpty(this.strFileOpen) && File.Exists(this.strFileOpen))
                {
                    File.WriteAllBytes(this.strFileOpen, this.byOldData);
                }
                MessageBox.Show("成功导入图片。");
            }
        }

        #endregion
    }
}
