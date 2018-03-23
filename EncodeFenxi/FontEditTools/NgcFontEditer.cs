using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Hanhua.ImgEditTools;
using Hanhua.Common;

namespace Hanhua.FontEditTools
{
    /// <summary>
    /// Ngc 字库分析结果
    /// </summary>
    public partial class NgcFontEditer : BaseForm
    {
        #region " 全局变量 "

        /// <summary>
        /// 旧字库信息
        /// </summary>
        NgcFontInfo oldFontInfo = new NgcFontInfo();

        // 要拖拽的图片
        Image dragDropImg;

        /// <summary>
        /// 右键菜单
        /// </summary>
        private ContextMenuStrip contextMenu = new ContextMenuStrip();

        /// <summary>
        /// 日文、字符对照表位置
        /// </summary>
        private int charJpCnPos;

        /// <summary>
        /// 保存当前选择的字库文件
        /// </summary>
        private string fontFile;

        #endregion

        #region " 公有方法 "

        /// <summary>
        /// 初始化
        /// </summary>
        public NgcFontEditer()
        {
            InitializeComponent();

            this.ResetHeight();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public NgcFontEditer(byte[] byteData)
        {
            InitializeComponent();

            this.ResetHeight();

            // 查看字库信息
            this.ViewFontInfo(byteData);

            // 设置右键菜单
            this.SetContextMenu();
        }

        /// <summary>
        /// 取得Ngc字库信息
        /// </summary>
        /// <param name="byteData"></param>
        /// <returns></returns>
        public Image[] GetNgcFontInfo(byte[] byteData, List<ImageHeader> imageInfo, ProgressBar processLoad)
        {
            // 取得并显示各种Header信息
            byte[] imageData = this.SetHeadersInfo(byteData);
            ImageHeader imageHeader = new ImageHeader();
            imageInfo.Add(imageHeader);
            imageHeader.Width = this.oldFontInfo.ImageWidth;
            imageHeader.Height = this.oldFontInfo.ImageHeight;
            imageHeader.Format = Util.GetImageFormat(this.oldFontInfo.ImageFormat);

            // 返回字库图片信息
            return this.CreateFontInfo(imageData);
        }

        #endregion

        #region " Button事件 "
  
        /// <summary>
        /// 选择字体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFontSelect_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFontTest.Font = fontDialog1.Font;
            }
        }

        /// <summary>
        /// 将旧的字库的图片Copy到新字库中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopyOldFont_Click(object sender, EventArgs e)
        {
            try
            {
                int colCount = (this.fontGrid.Columns.Count - 1) / 2;
                int colIndex = 0;
                for (int i = 0; i < this.fontGrid.Rows.Count; i++)
                {
                    DataGridViewRow row = this.fontGrid.Rows[i];
                    colIndex = 0;
                    for (int j = colCount + 1; j < this.fontGrid.Columns.Count; j++)
                    {
                        row.Cells[j].Value = row.Cells[colIndex++].Value;
                    }
                }
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
            }
        }

        /// <summary>
        /// 查看旧字库图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewOldFont_Click(object sender, EventArgs e)
        {
            // 打开要分析的字库文件
            this.baseFile = Util.SetOpenDailog("Ngc 字库文件（*.fnt）|*.fnt|所有文件|*.*", string.Empty);
            if (string.IsNullOrEmpty(this.baseFile))
            {
                return;
            }

            // 保存选择的文件
            this.fontFile = this.baseFile;

            // 查看字库信息
            this.ViewFontInfo(File.ReadAllBytes(this.baseFile));
        }

        /// <summary>
        /// 旧字库具体信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewFontInfo_Click(object sender, EventArgs e)
        {
            FontInfo fontInfoFrm = new FontInfo(this.oldFontInfo.fileData);
            fontInfoFrm.Show(this);
        }

        /// <summary>
        /// 取得要分析的文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openFile_FileOk(object sender, CancelEventArgs e)
        {
            baseFile = ((FileDialog)sender).FileName;
        }

        /// <summary>
        /// 保存新做成的字库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            this.baseFile = Util.SetSaveDailog("Ngc 字库文件（*.fnt）|*.fnt|所有文件|*.*", string.Empty);
            if (string.IsNullOrEmpty(this.baseFile))
            {
                return;
            }

            try
            {
                //byte[] oldNewFileData = this.GenerateNewFont();

                //// 开始保存文件
                //File.WriteAllBytes(this.strFileOpen, oldNewFileData);
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
            }
        }
        
        /// <summary>
        /// 使用新字体生成字库图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewNewFont_Click(object sender, EventArgs e)
        {
            // 根据最新的字符映射表，生成新的字符串
            this.LoadJpCnCharList();

            // 使用新字体、新的字符串生成新字库
            this.CreateNewFontByNewStr();
        }

        #endregion

        #region " Grid事件 "

        /// <summary>
        /// 单元格点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fontGrid_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            int colCount = (this.fontGrid.Columns.Count - 1) / 2;

            // 只可以拖拽旧库的图片
            if (e.RowIndex < this.fontGrid.Rows.Count
                && e.ColumnIndex < colCount)
            {
                // 右键弹出菜单
                if (e.Button == MouseButtons.Right)
                {
                    if (((Image)this.fontGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value).Width >=
                        ((Image)this.fontGrid.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value).Width)
                    {
                        int pageNum = e.RowIndex / (this.oldFontInfo.CharactersPerColumn + 1);
                        int rowNum = e.RowIndex % (this.oldFontInfo.CharactersPerColumn + 1);
                        int onePageCharNum = this.oldFontInfo.CharactersPerColumn * this.oldFontInfo.CharactersPerRow;

                        // 计算日文、字符对照表位置
                        this.charJpCnPos = pageNum * onePageCharNum + rowNum * this.oldFontInfo.CharactersPerRow + e.ColumnIndex;

                        // 弹出菜单
                        Point p = Control.MousePosition;
                        this.contextMenu.Show(p);
                    }
                }
                else
                {
                    // 左键为了拖拽设置点击的图片
                    this.dragDropImg = (Image)this.fontGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                }
            }
        }

        /// <summary>
        /// 单击结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fontGrid_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.dragDropImg = null;
        }

        /// <summary>
        /// 拖拽开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fontGrid_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left
                && this.dragDropImg != null)
            {
                // 开始拖拽
                DragDropEffects dropEffect = this.fontGrid.DoDragDrop(this.dragDropImg, DragDropEffects.Copy);
            }
        }

        /// <summary>
        /// 到达目标单元格时的判断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fontGrid_DragOver(object sender, DragEventArgs e)
        {
            // 如果没有数据，禁止拖拽
            if (!e.Data.GetDataPresent(typeof(Bitmap)))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            // 如果不在新库的位置，禁止拖拽
            Point p = this.fontGrid.PointToClient(new Point(e.X, e.Y));
            DataGridView.HitTestInfo hit = this.fontGrid.HitTest(p.X, p.Y);

            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                int colCount = (this.fontGrid.Columns.Count - 1) / 2;
                if (hit.ColumnIndex >= (colCount + 1))
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
        private void fontGrid_DragDrop(object sender, DragEventArgs e)
        {
            // 取得目标单元格
            Point p = this.fontGrid.PointToClient(new Point(e.X, e.Y));
            DataGridView.HitTestInfo hit = this.fontGrid.HitTest(p.X, p.Y);

            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                try
                {
                    // 复制图片
                    this.fontGrid.Rows[hit.RowIndex].Cells[hit.ColumnIndex].Value = this.dragDropImg;

                    // 保存数据到图片字节数组
                    int pageNum = hit.RowIndex / (this.oldFontInfo.CharactersPerColumn + 1);
                    int pageStartRow = pageNum * (this.oldFontInfo.CharactersPerColumn + 1);
                    int colCount = (this.fontGrid.Columns.Count - 1) / 2;

                    // 设置当前修改的图片
                    Bitmap bmp = new Bitmap(this.oldFontInfo.ImageWidth, this.oldFontInfo.ImageHeight, PixelFormat.Format32bppArgb);
                    int bmpY = 0;
                    int bmpX = 0;

                    for (int row = pageStartRow; row < pageStartRow + this.oldFontInfo.CharactersPerColumn + 1; row++)
                    {
                        for (int col = colCount + 1; col < this.fontGrid.Columns.Count; col++)
                        {
                            Bitmap cellImg = (Bitmap)this.fontGrid.Rows[row].Cells[col].Value;
                            for (int y = 0; y < cellImg.Height; y++)
                            {
                                for (int x = 0; x < cellImg.Width; x++)
                                {
                                    bmp.SetPixel(bmpX++, bmpY, cellImg.GetPixel(x, y));
                                }
                                bmpX -= cellImg.Width;
                                bmpY++;
                            }
                            bmpX += cellImg.Width;
                            if (col < (this.fontGrid.Columns.Count - 1))
                            {
                                bmpY -= cellImg.Height;
                            }
                        }
                        bmpX = 0;
                    }

                    // 将当前图片数据保存到图片字节数据中
                    byte[] editedPageImg = Util.ImageEncode(bmp, Util.GetImageFormat(this.oldFontInfo.ImageFormat));
                    Array.Copy(editedPageImg, 0, this.oldFontInfo.imgData, pageNum * this.oldFontInfo.ImageWidth * this.oldFontInfo.ImageHeight, editedPageImg.Length);
                }
                catch (Exception me)
                {
                    MessageBox.Show(me.Message + "\n" + me.StackTrace);
                }
            }
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
                // 设置日中对照表开始位置
                case "setStartPos":
                    e.ClickedItem.Enabled = false;
                    this.contextMenu.Items["setEndPos"].Enabled = true;
                    this.oldFontInfo.charJpCnStartPos = this.charJpCnPos;
                    break;

                // 设置日中对照表结束位置
                case "setEndPos":
                    if (this.charJpCnPos <= this.oldFontInfo.charJpCnStartPos)
                    {
                        MessageBox.Show("结束位置不能小于开始位置");
                        return;
                    }
                    e.ClickedItem.Enabled = false;
                    this.contextMenu.Items["createJpCnTable"].Enabled = true;
                    this.oldFontInfo.charJpCnEndPos = this.charJpCnPos;
                    break;

                // 生成日中对照表
                case "createJpCnTable":
                    this.contextMenu.Items["setStartPos"].Enabled = true;
                    this.contextMenu.Items["setEndPos"].Enabled = false;
                    e.ClickedItem.Enabled = false;

                    this.CreateCharJpCnTable();
                    break;
            }
        }

        #endregion
        
        #region " 私有方法 "

        /// <summary>
        /// 读取日中字符对照表
        /// </summary>
        private void LoadJpCnCharList()
        {
            // 取得日中字符对照表文件名
            string[] names = this.baseFile.Split('\\');
            StringBuilder nameSb = new StringBuilder();
            for (int i = 0; i < names.Length - 1; i++)
            {
                nameSb.Append(names[i]).Append("\\");
            }
            nameSb.Append(Util.jpCnCharMapFileName);
            string charJpCnFile = nameSb.ToString();

            // 取得对照表中的日中汉字
            if (File.Exists(charJpCnFile))
            {
                string[] jpChCharTable = File.ReadAllLines(charJpCnFile);
                this.oldFontInfo.charList.Clear();

                for (int i = 0; i < jpChCharTable.Length; i++)
                {
                    string currentChar = jpChCharTable[i];
                    if (currentChar.Length > 1)
                    {
                        this.oldFontInfo.charList.Add(currentChar.Substring(1));
                    }
                    else
                    {
                        this.oldFontInfo.charList.Add(currentChar.Substring(0, 1));
                    }
                }
            }
        }

        /// <summary>
        /// 生成日中对照表
        /// </summary>
        private void CreateCharJpCnTable()
        {
            try
            {
                // 将可以替换成中文的日文做标记
                int charIndex = 0;
                while (charIndex < this.oldFontInfo.charJpCnStartPos)
                {
                    KeyValuePair<string, string> charJpCn = this.oldFontInfo.charJpCnList[charIndex];
                    this.oldFontInfo.charJpCnList[charIndex] = new KeyValuePair<string, string>(charJpCn.Key, charJpCn.Key);
                    charIndex++;
                }
                charIndex = this.oldFontInfo.charJpCnEndPos + 1;
                while (charIndex < this.oldFontInfo.charJpCnList.Count)
                {
                    KeyValuePair<string, string> charJpCn = this.oldFontInfo.charJpCnList[charIndex];
                    this.oldFontInfo.charJpCnList[charIndex] = new KeyValuePair<string, string>(charJpCn.Key, charJpCn.Key);
                    charIndex++;
                }


                List<string> charJpCnList = new List<string>();
                for (int i = 0; i < this.oldFontInfo.charJpCnList.Count; i++)
                {
                    KeyValuePair<string, string> charJpCn = this.oldFontInfo.charJpCnList[i];
                    charJpCnList.Add(charJpCn.Key + charJpCn.Value);
                }

                // 定义保存文件名
                string[] names = this.baseFile.Split('\\');
                StringBuilder nameSb = new StringBuilder();
                for (int i = 0; i < names.Length - 1; i++)
                {
                    nameSb.Append(names[i]).Append("\\");
                }
                nameSb.Append(Util.jpCnCharMapFileName);

                // 保存文件
                File.WriteAllLines(nameSb.ToString(), charJpCnList.ToArray(), Encoding.UTF8);
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
            }
        }

        /// <summary>
        /// 设置右键菜单
        /// </summary>
        private void SetContextMenu()
        {
            // 绑定右键菜单事件
            this.contextMenu.ItemClicked += new ToolStripItemClickedEventHandler(this.ContextMenu_ItemClicked);

            // 添加菜单
            this.contextMenu.Items.Clear();
            ToolStripMenuItem item = new ToolStripMenuItem();
            item.Name = "setStartPos";
            item.Text = "设置日中对照表开始位置";
            this.contextMenu.Items.Add(item);

            item = new ToolStripMenuItem();
            item.Name = "setEndPos";
            item.Text = "设置日中对照表结束位置";
            item.Enabled = false;
            this.contextMenu.Items.Add(item);

            item = new ToolStripMenuItem();
            item.Name = "createJpCnTable";
            item.Text = "生成日中对照表";
            item.Enabled = false;
            this.contextMenu.Items.Add(item);
        }
      
        /// <summary>
        /// 生成一页的字库图像数据
        /// </summary>
        private Bitmap CreateFontImagePage(List<byte[]> imageData, List<CwdhEntries> newCwdhEntriesList, List<string> unicodeCharList, ref int unicodeCharIndex)
        {
            // 单个子对齐风格
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Near;
            CharacterRange[] characterRanges = { new CharacterRange(0, 1) };
            sf.SetMeasurableCharacterRanges(characterRanges);

            RectangleF rectangle;
            FontFamily family = new FontFamily(txtFontTest.Font.Name);
            Pen blackPen = new Pen(Color.Black, 1);

            // 图片
            Bitmap bmp = new Bitmap(this.oldFontInfo.ImageWidth, this.oldFontInfo.ImageHeight);
            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.FromArgb(0, 0xFF, 0xFF, 0xFF));

            // 字符相关属性
            // 字符的宽度
            int charWidth;
            // 字符左边距离边框的距离
            int charLeftWidth;

            for (int y = 0; y < this.oldFontInfo.CharactersPerColumn; y++)
            {
                for (int x = 0; x < this.oldFontInfo.CharactersPerRow; x++)
                {
                    this.ProcessBarStep();
                    if (unicodeCharIndex >= unicodeCharList.Count)
                    {
                        break;
                    }
                    string strCurrentChar = unicodeCharList[unicodeCharIndex];

                    if (!string.Empty.Equals(strCurrentChar))
                    {
                        // 在指定的区域内写入特定汉字
                        rectangle = new RectangleF(x * (this.oldFontInfo.CellWidth + 1), y * (this.oldFontInfo.CellHeight + 1),
                            this.oldFontInfo.CellWidth + 1, this.oldFontInfo.CellHeight + 1);
                        GraphicsPath graphPath = new GraphicsPath();
                        graphPath.AddString(strCurrentChar, family, (int)FontStyle.Bold, this.oldFontInfo.CellHeight, rectangle, sf);

                        // 为了实现图像里面是白色，边框是黑色，真是费了一番脑筋
                        // 下面是最后的方案，过程真是艰辛，结果还真简单
                        g.FillPath(Brushes.White, graphPath);
                        g.DrawPath(blackPen, graphPath);

                        Region[] charRegions = g.MeasureCharacterRanges(strCurrentChar, txtFontTest.Font, rectangle, sf);
                        charWidth = (int)charRegions[0].GetBounds(g).Width;

                        if (string.Empty.Equals(strCurrentChar.Trim()))
                        {
                            newCwdhEntriesList.Add(new CwdhEntries(13, 0, 13));
                        }
                        else
                        {
                            // 为了理解和计算CwdhEntries的三个参数，也是花费了一番周折
                            // 第一个参数：应该是Left padding，离前一个字符的距离
                            // 第二个参数：显示字符时，从当前字符图片中，取得的宽度
                            // 第一个参数：显示字符时，这个字符占的宽度
                            charLeftWidth = (this.oldFontInfo.CellWidth + 1 - charWidth) / 2 + 1;
                            if (charLeftWidth < 0)
                            {
                                charLeftWidth = 0;
                            }
                            charWidth += charLeftWidth;

                            newCwdhEntriesList.Add(
                                new CwdhEntries(0, charWidth, charWidth + 1));
                        }
                    }

                    unicodeCharIndex++;
                }
            }

            imageData.Add(Util.ImageEncode(bmp, Util.GetImageFormat(this.oldFontInfo.ImageFormat)));

            return bmp;
        }
            
        /// <summary>
        /// 查看字库信息
        /// </summary>
        /// <param name="byteData"></param>
        private void ViewFontInfo(byte[] byteData)
        {
            // 取得并显示各种Header信息
            this.oldFontInfo.fileData = byteData;
            this.oldFontInfo.imgData = this.SetHeadersInfo(byteData);

            // 开始生成字库图像信息
            this.ResetProcessBar(this.oldFontInfo.TextureNum * 2);
            Image[] fontImgList = this.CreateFontInfo(this.oldFontInfo.imgData);

            // 将大图片变成小图片
            List<Image[]> rowColImg = new List<Image[]>();
            foreach (Image img in fontImgList)
            {
                List<Bitmap[]> pageImage = Util.GetRowColImage((Bitmap)img, this.oldFontInfo.CellWidth + 1, this.oldFontInfo.CellHeight + 1);
                foreach (Bitmap[] rowImage in pageImage)
                {
                    rowColImg.Add(rowImage);
                }

                this.ProcessBarStep();
            }

            // 生成一个空白的小图片
            Image blankImg = (Image)rowColImg[0][0].Clone();
            Graphics g = Graphics.FromImage(blankImg);
            g.Clear(Color.FromArgb(0, 0xFF, 0xFF, 0xFF));

            // 生成一个分割新旧字库的图片
            Image lineImg = (Image)blankImg.Clone();
            g = Graphics.FromImage(lineImg);
            g.Clear(Color.Silver);

            // 设置Grid的参数
            this.fontGrid.Rows.Clear();
            this.fontGrid.Columns.Clear();
            int gridCols = rowColImg[0].Length;
            // 追加旧库的列
            for (int i = 0; i < gridCols; i++)
            {
                this.fontGrid.Columns.Add(new DataGridViewImageColumn());
                this.fontGrid.Columns[i].Width = rowColImg[0][i].Width + 1;
                this.fontGrid.Columns[i].ValueType = typeof(Image);
            }

            // 追加空白列
            int blankCol = this.fontGrid.Columns.Add(new DataGridViewImageColumn());
            this.fontGrid.Columns[blankCol].Width = this.oldFontInfo.CellWidth / 3;

            // 追加新库的列（和旧的是一样的）
            for (int i = 0; i < gridCols; i++)
            {
                int addedCol = this.fontGrid.Columns.Add(new DataGridViewImageColumn());
                this.fontGrid.Columns[addedCol].Width = rowColImg[0][i].Width + 1;
                this.fontGrid.Columns[addedCol].ValueType = typeof(Image);
            }

            // 显示字库图像
            for (int i = 0; i < rowColImg.Count; i++)
            {
                Image[] rowImage = new Image[gridCols * 2 + 1];
                int imgIndex = 0;
                for (int j = 0; j < gridCols; j++)
                {
                    rowImage[imgIndex++] = rowColImg[i][j];
                }
                rowImage[imgIndex++] = lineImg;
                for (int j = 0; j < gridCols; j++)
                {
                    rowImage[imgIndex++] = blankImg;
                }

                int addedRow = this.fontGrid.Rows.Add(rowImage);
                this.fontGrid.Rows[addedRow].Height = rowImage[0].Height + 1;
            }
        }

        /// <summary>
        /// 开始生成字库图像信息
        /// </summary>
        /// <param name="imageData"></param>
        private Image[] CreateFontInfo(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
            {
                return null;
            }

            // 获得字库基本信息
            string strImageFormat = Util.GetImageFormat(this.oldFontInfo.ImageFormat);
            int textureSize = this.oldFontInfo.ImageWidth * this.oldFontInfo.ImageHeight;
            int imageWidth = this.oldFontInfo.ImageWidth;
            int imageHeight = this.oldFontInfo.ImageHeight;
            int[] blockHeightWidth = Util.GetBlockWidthHeight(strImageFormat);

            // 将原始图像数据分成小块
            List<byte[]> textureList = new List<byte[]>();
            int copyStartIndex = 0;
            int imageLength = imageData.Length;
            while (copyStartIndex < imageLength)
            {
                if ((copyStartIndex + textureSize) > imageLength)
                {
                    textureSize = imageLength - copyStartIndex;
                }
                byte[] textureItem = new byte[textureSize];
                Array.Copy(imageData, copyStartIndex, textureItem, 0, textureSize);
                textureList.Add(textureItem);

                copyStartIndex += textureSize;
            }

            // 开始生成字库图片
            List<Image> imageList = new List<Image>();
            for (int i = 0; i < textureList.Count; i++)
            {
                Bitmap image = Util.ImageDecode(
                    new Bitmap(imageWidth, imageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb),
                    textureList[i], strImageFormat);

                imageList.Add(image);

                this.ProcessBarStep();
            }

            return imageList.ToArray();
        }

        /// <summary>
        /// 显示各种Header信息,并返回字符Image的数据
        /// </summary>
        /// <param name="byteData"></param>
        private byte[] SetHeadersInfo(byte[] byData)
        {
            try
            {
                #region " 读取Rfnt Header "

                // 先读取Bfn Header
                Array.Copy(byData, 0, this.oldFontInfo.byBfnHeader, 0, this.oldFontInfo.byBfnHeader.Length);
                string strFileMagic = Util.GetHeaderString(byData, 0, 6);
                int intLengthOfFileInBytes = Util.GetOffset(byData, 0x8, 0xb);

                #endregion

                #region " 获取INF Section信息 "

                // 获取INF Section信息
                Array.Copy(byData, 0x20, this.oldFontInfo.byInfHeader, 0, this.oldFontInfo.byInfHeader.Length);

                // 获取INF Section中相关信息
                string strInfHeaderMagic = Util.GetHeaderString(this.oldFontInfo.byInfHeader, 0, 2);
                int intCharHeight = Util.GetOffset(this.oldFontInfo.byInfHeader, 0xE, 0xF);
                int intCharWidth = Util.GetOffset(this.oldFontInfo.byInfHeader, 0x10, 0x11);

                #endregion

                #region " 获取Wid的信息 "

                // 获取Wid Header的字节信息
                byte[] byWidHeader = new byte[8];
                Array.Copy(byData, 0x40, byWidHeader, 0, byWidHeader.Length);
                if (!"WID".Equals(Util.GetHeaderString(byWidHeader, 0, 2)))
                {
                    MessageBox.Show("不正确的Wid Section。");
                    return null;
                }

                // 获取Wid 其他的字节信息
                int intWidSectionLen = Util.GetOffset(byWidHeader, 0x4, 0x7);
                this.oldFontInfo.byWidData = new byte[intWidSectionLen];
                Array.Copy(byData, 0x40, this.oldFontInfo.byWidData, 0, this.oldFontInfo.byWidData.Length);

                #endregion

                #region " 取得Map Data信息 "

                // 取得Map Header信息
                int currentPos = 0x40 + this.oldFontInfo.byWidData.Length;
                byte[] byMapHeader = new byte[16];
                Array.Copy(byData, currentPos, byMapHeader, 0, byMapHeader.Length);

                // 循环取得每个Map Section信息
                while ("MAP".Equals(Util.GetHeaderString(byMapHeader, 0, 2)))
                {
                    byte[] mapSection = new byte[Util.GetOffset(byMapHeader, 0x4, 0x7)];
                    Array.Copy(byData, currentPos, mapSection, 0, mapSection.Length);

                    this.oldFontInfo.byMapData.Add(mapSection);

                    // 取得下一个Map Header信息
                    currentPos += mapSection.Length;
                    Array.Copy(byData, currentPos, byMapHeader, 0, byMapHeader.Length);
                }

                #endregion

                #region " 取得Map(字符映射表)信息 "

                foreach (byte[] byMapData in this.oldFontInfo.byMapData)
                {
                    int mapType = Util.GetOffset(byMapData, 8, 9);
                    int startChar = Util.GetOffset(byMapData, 10, 11);
                    int charNum = Util.GetOffset(byMapData, 14, 15);

                    int dataPos = 16;
                    for (int i = 0; i < charNum; i++)
                    {
                        if (mapType == 2)
                        {
                            this.oldFontInfo.cmapList.Add(new KeyValuePair<int, int>(Util.GetOffset(byMapData, dataPos, dataPos + 1), startChar++));
                            dataPos += 2;
                        }
                        else if (mapType == 3)
                        {
                            this.oldFontInfo.cmapList.Add(
                                new KeyValuePair<int, int>(
                                    Util.GetOffset(byMapData, dataPos + 2, dataPos + 3),
                                    Util.GetOffset(byMapData, dataPos, dataPos + 1)));
                            dataPos += 4;
                        }
                        else
                        {
                            MessageBox.Show("不能识别的Map类型 ： " + mapType);
                            return null;
                        }
                    }
                }

                // Map(字符映射表)信息排序
                this.oldFontInfo.cmapList.Sort(Util.Comparison);

                // 保存原来的Cmap信息
                string cnOneLevel = Util.CreateOneLevelHanzi();
                foreach (KeyValuePair<int, int> item in this.oldFontInfo.cmapList)
                {
                    if (item.Key != 0xFFFF && item.Value != 0)
                    {
                        // 保存原来的日文字符
                        string jpChar = Util.GetStrFromNumber(item.Value, 2, "");
                        this.oldFontInfo.charList.Add(jpChar);

                        // 生成日文、中文字符对照表
                        if (cnOneLevel.IndexOf(jpChar) != -1)
                        {
                            this.oldFontInfo.charJpCnList.Add(new KeyValuePair<string, string>(jpChar, jpChar));
                        }
                        else
                        {
                            this.oldFontInfo.charJpCnList.Add(new KeyValuePair<string, string>(jpChar, ""));
                        }
                    }
                }

                #endregion

                #region " 取得GLY Section信息 "

                // 取得Gly Header信息
                Array.Copy(byData, currentPos, this.oldFontInfo.byGlyHeader, 0, this.oldFontInfo.byGlyHeader.Length);
                if (!"GLY".Equals(Util.GetHeaderString(this.oldFontInfo.byGlyHeader, 0, 2)))
                {
                    MessageBox.Show("不正确的GLY Section。");
                    return null;
                }
                this.oldFontInfo.CellWidth = Util.GetOffset(this.oldFontInfo.byGlyHeader, 12, 13);
                this.oldFontInfo.CellHeight = Util.GetOffset(this.oldFontInfo.byGlyHeader, 14, 15);
                this.oldFontInfo.TextureNum = Util.GetOffset(this.oldFontInfo.byGlyHeader, 16, 19);
                this.oldFontInfo.ImageFormat = Util.GetOffset(this.oldFontInfo.byGlyHeader, 20, 21);
                this.oldFontInfo.CharactersPerRow = Util.GetOffset(this.oldFontInfo.byGlyHeader, 22, 23);
                this.oldFontInfo.CharactersPerColumn = Util.GetOffset(this.oldFontInfo.byGlyHeader, 24, 25);
                this.oldFontInfo.ImageWidth = Util.GetOffset(this.oldFontInfo.byGlyHeader, 26, 27);
                this.oldFontInfo.ImageHeight = Util.GetOffset(this.oldFontInfo.byGlyHeader, 28, 29);

                // 取得Gly 图像字节信息
                currentPos += 0x20;
                this.oldFontInfo.imgData = new byte[intLengthOfFileInBytes - currentPos];
                Array.Copy(byData, currentPos, this.oldFontInfo.imgData, 0, this.oldFontInfo.imgData.Length);

                #endregion

                return this.oldFontInfo.imgData;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n" + e.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// 使用新字体、新的字符串生成新字库
        /// </summary>
        private void CreateNewFontByNewStr()
        {
            try
            {
                // 将大图片变成小图片
                List<Image[]> rowColImg = new List<Image[]>();

                // 根据旧字库的Cmap数据，生成新字库的数据
                List<byte[]> imageData = new List<byte[]>();
                List<CwdhEntries> cwdhEntriesList = new List<CwdhEntries>();
                int unicodeCharIndex = 0;

                while (unicodeCharIndex < this.oldFontInfo.charList.Count)
                {
                    // 生成一页的图片
                    Bitmap newImg = this.CreateFontImagePage(
                        imageData, cwdhEntriesList, this.oldFontInfo.charList, ref unicodeCharIndex);

                    // 将一页图片分割成小个子
                    List<Bitmap[]> pageImage = Util.GetRowColImage((Bitmap)newImg, this.oldFontInfo.CellWidth + 1, this.oldFontInfo.CellHeight + 1);
                    foreach (Bitmap[] rowImage in pageImage)
                    {
                        rowColImg.Add(rowImage);
                    }
                }

                // 设置Wid 其他的字节信息
                int widSectionLen = Util.GetOffset(this.oldFontInfo.byWidData, 0x4, 0x7);
                int widDataPos = 8;
                for (int i = 0; i < widSectionLen; i++)
                {
                    this.oldFontInfo.byWidData[widDataPos] = (byte)((cwdhEntriesList[i].Unknown3 >> 8) & 0xFF);
                    this.oldFontInfo.byWidData[widDataPos + 1] = (byte)(cwdhEntriesList[i].Unknown3 & 0xFF);
                    widDataPos += 2;
                }

                // 图像数据转换
                this.oldFontInfo.imgData = new byte[imageData.Count * this.oldFontInfo.ImageWidth * this.oldFontInfo.ImageHeight];
                int imagePos = 0;
                foreach (byte[] imagePer in imageData)
                {
                    Array.Copy(imagePer, 0, this.oldFontInfo.imgData, imagePos, imagePer.Length);
                    imagePos += imagePer.Length;
                }

                // 生成一个空白的小图片
                Image blankImg = (Image)rowColImg[0][0].Clone();
                Graphics g = Graphics.FromImage(blankImg);
                g.Clear(Color.FromArgb(0, 0xFF, 0xFF, 0xFF));

                // 生成一个分割新旧字库的图片
                Image lineImg = (Image)blankImg.Clone();
                g = Graphics.FromImage(lineImg);
                g.Clear(Color.Silver);

                // 显示新字体字库
                int colCount = (this.fontGrid.Columns.Count - 1) / 2;
                int colIndex = 0;
                for (int i = 0; i < rowColImg.Count; i++)
                {
                    DataGridViewRow row = this.fontGrid.Rows[i];
                    colIndex = 0;
                    for (int j = colCount + 1; j < this.fontGrid.Columns.Count; j++)
                    {
                        row.Cells[j].Value = rowColImg[i][colIndex++];
                    }

                    if (i == (this.fontGrid.Rows.Count - 1)
                        && rowColImg.Count > this.fontGrid.Rows.Count)
                    {
                        Image[] rowImage = new Image[colCount * 2 + 1];
                        int imgIndex = 0;
                        for (int j = 0; j < colCount; j++)
                        {
                            rowImage[imgIndex++] = blankImg;
                        }
                        rowImage[imgIndex++] = lineImg;
                        for (int j = 0; j < colCount; j++)
                        {
                            rowImage[imgIndex++] = rowColImg[i + 1][j];
                        }
                        int addedRow = this.fontGrid.Rows.Add(rowImage);
                        this.fontGrid.Rows[addedRow].Height = rowColImg[i + 1][0].Height + 1;
                    }
                }
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
            }
        }

        #endregion
    }
}
