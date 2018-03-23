using System;
using System.Collections.Generic;
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
    /// Wii 字库分析结果
    /// </summary>
    public partial class WiiFontEditer : BaseForm
    {
        #region " 全局变量 "

        /// <summary>
        /// 旧字库信息
        /// </summary>
        private WiiFontInfo oldFontInfo = new WiiFontInfo();

        /// <summary>
        /// 要拖拽的图片
        /// </summary>
        private Image dragDropImg;

        /// <summary>
        /// 新字库图片是否显示
        /// </summary>
        private bool isNewFontLoaded = false;

        #endregion

        #region " 公有方法 "

        /// <summary>
        /// 初始化
        /// </summary>
        public WiiFontEditer()
        {
            InitializeComponent();

            this.ResetHeight();
        }

        /// <summary>
        /// 取得Wii字库信息
        /// </summary>
        /// <param name="byteData"></param>
        /// <returns></returns>
        public Image[] GetWiiFontInfo(byte[] byteData, List<ImageHeader> imageInfo)
        {
            // 取得并显示各种Header信息
            byte[] imageData = this.SetHeadersInfo(byteData);
            ImageHeader imageHeader = new ImageHeader();
            imageInfo.Add(imageHeader);
            imageHeader.Width = this.oldFontInfo.tglpHeader.ImageWidth;
            imageHeader.Height = this.oldFontInfo.tglpHeader.ImageHeight;
            imageHeader.Format = this.oldFontInfo.tglpHeader.ImageFormat;

            // 显示滚动条
            this.GetForm().ResetProcessBar(this.oldFontInfo.tglpHeader.TextureNum);
            
            // 返回字库图片信息
            Image[] fontInfo = this.CreateFontInfo(imageData);

            // 关闭滚动条
            this.GetForm().CloseProcessBar();

            return fontInfo;
        }

        /// <summary>
        /// 查看字库信息
        /// </summary>
        /// <param name="byteData"></param>
        public void ViewFontInfo(params object[] param)
        {
            this.isNewFontLoaded = false;
            byte[] byteData = (byte[])param[0];

            // 取得并显示各种Header信息
            this.oldFontInfo.fileData = byteData;
            this.oldFontInfo.imgData = this.SetHeadersInfo(byteData);

            // 显示进度条
            this.ResetProcessBar(this.oldFontInfo.tglpHeader.TextureNum * 2);

            // 开始生成字库图像信息
            Image[] fontImgList = this.CreateFontInfo(this.oldFontInfo.imgData);

            // 将大图片变成小图片
            List<Image[]> rowColImg = new List<Image[]>();
            foreach (Image img in fontImgList)
            {
                List<Bitmap[]> pageImage = Util.GetRowColImage((Bitmap)img, this.oldFontInfo.tglpHeader.CellWidth + 1, this.oldFontInfo.tglpHeader.CellHeight + 1);
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

            this.Invoke((MethodInvoker)delegate()
            {
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
                this.fontGrid.Columns[blankCol].Width = this.oldFontInfo.tglpHeader.CellWidth / 3;

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
            });

            this.CloseProcessBar();
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
            this.baseFile = Util.SetOpenDailog("Wii 字库文件（*.brfnt）|*.brfnt|所有文件|*.*", string.Empty);
            if (string.IsNullOrEmpty(this.baseFile))
            {
                return;
            }

            // 查看字库信息
            this.Do(this.ViewFontInfo, File.ReadAllBytes(this.baseFile));
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
        /// 保存新做成的字库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            this.baseFile = Util.SetSaveDailog("Wii 字库文件（*.brfnt）|*.brfnt|所有文件|*.*", string.Empty);
            if (string.IsNullOrEmpty(this.baseFile))
            {
                return;
            }

            try
            {
                byte[] oldNewFileData = this.GenerateNewFont(
                    this.oldFontInfo.cwdhEntriesList, 
                    this.oldFontInfo.byCmapList, 
                    this.oldFontInfo.byCmapEntriesList, 
                    this.oldFontInfo.imgData);

                // 开始保存文件
                File.WriteAllBytes(this.baseFile, oldNewFileData);

                // 验证
                //int diff = 0;
                //if (oldNewFileData.Length != this.oldFontInfo.fileData.Length)
                //{
                //    diff++;
                //}

                //for (int i = 0; i < oldNewFileData.Length; i++)
                //{
                //    if (oldNewFileData[i] != this.oldFontInfo.fileData[i])
                //    {
                //        diff++;
                //    }
                //}
                //diff++;
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
            this.isNewFontLoaded = true;

            this.ResetProcessBar(this.oldFontInfo.codeCharMap.Count);

            // 根据最新的字符映射表，生成新的字符串
            this.LoadJpCnCharList();

            // 使用新字体、新的字符串生成新字库
            this.CreateNewFontByNewStr();

            this.CloseProcessBar();
        }

        /// <summary>
        /// 生成字符映射
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateCharMap_Click(object sender, EventArgs e)
        {
            this.CreateCharJpCnTable();
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
                if (e.Button == MouseButtons.Left)
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
                // 复制图片
                this.fontGrid.Rows[hit.RowIndex].Cells[hit.ColumnIndex].Value = this.dragDropImg;
                int pageNum = hit.RowIndex / (this.oldFontInfo.tglpHeader.CharactersPerColumn + 1);
                this.ResetImagePageByte(pageNum);
            }
        }

        /// <summary>
        /// 双击旧字库图片，复制到新字库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fontGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // 判断新字库是否显示
            if (!this.isNewFontLoaded)
            {
                return;
            }

            // 只处理双击旧库的图片
            int colCount = (this.fontGrid.Columns.Count - 1) / 2;
            if (e.RowIndex >= this.fontGrid.Rows.Count || e.ColumnIndex >= colCount)
            {
                return;
            }

            // 取得双击的图片
            Image selectedImg = (Image)this.fontGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

            // 复制旧库图片到新库
            this.fontGrid.Rows[e.RowIndex].Cells[colCount + 1 + e.ColumnIndex].Value = selectedImg;

            // 保存当前页的图像数据
            int pageNum = e.RowIndex / (this.oldFontInfo.tglpHeader.CharactersPerColumn + 1);
            this.ResetImagePageByte(pageNum);
        }

        #endregion
        
        #region " 私有方法 "

        /// <summary>
        /// 复制选中的旧库的Cell到新库
        /// </summary>
        /// <param name="cells"></param>
        private void CopySelectedCells(DataGridViewSelectedCellCollection cells)
        {
            // 判断新字库是否显示
            if (!this.isNewFontLoaded)
            {
                return;
            }

            // 复制选中的Cell到新库
            List<int> changedPage = new List<int>();
            int colCount = (this.fontGrid.Columns.Count - 1) / 2 + 1;
            for (int i = 0; i < cells.Count; i++)
            {
                int row = cells[i].RowIndex;
                int col = cells[i].ColumnIndex;
                this.fontGrid.Rows[row].Cells[col + colCount].Value = cells[i].Value;

                int pageNum = row / (this.oldFontInfo.tglpHeader.CharactersPerColumn + 1);
                if (!changedPage.Contains(pageNum))
                {
                    changedPage.Add(pageNum);
                }
            }

            // 保存修改过的图片
            for (int i = 0; i < changedPage.Count; i++)
            {
                this.ResetImagePageByte(changedPage[i]);
            }
        }

        /// <summary>
        /// 保存当前页的图像数据
        /// </summary>
        /// <param name="rowIndex"></param>
        private void ResetImagePageByte(int pageNum)
        {
            try
            {
                // 保存数据到图片字节数组
                int pageStartRow = pageNum * (this.oldFontInfo.tglpHeader.CharactersPerColumn + 1);
                int colCount = (this.fontGrid.Columns.Count - 1) / 2;

                // 设置当前修改的图片
                Bitmap bmp = new Bitmap(this.oldFontInfo.tglpHeader.ImageWidth, this.oldFontInfo.tglpHeader.ImageHeight, PixelFormat.Format32bppArgb);
                int bmpY = 0;
                int bmpX = 0;

                for (int row = pageStartRow; row < pageStartRow + this.oldFontInfo.tglpHeader.CharactersPerColumn + 1; row++)
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
                byte[] editedPageImg = Util.ImageEncode(bmp, this.oldFontInfo.tglpHeader.ImageFormat);
                Array.Copy(editedPageImg, 0, this.oldFontInfo.imgData, pageNum * this.oldFontInfo.tglpHeader.TextureSize, editedPageImg.Length);
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
            }
        }

        /// <summary>
        /// 读取日中字符对照表
        /// </summary>
        private void LoadJpCnCharList()
        {
            // 取得对照表中的日中汉字
            if (File.Exists(Util.jpCnCharMapFileName))
            {
                string[] jpChCharTable = File.ReadAllLines(Util.jpCnCharMapFileName);
                int oldCharListLen = this.oldFontInfo.charList.Count;
                StringBuilder addedChar = new StringBuilder();
                this.oldFontInfo.charList.Clear();

                for (int i = 0; i < jpChCharTable.Length; i++)
                {
                    string charMapInfo = jpChCharTable[i];
                    if (string.IsNullOrEmpty(charMapInfo))
                    {
                        break;
                    }

                    string currentChar = charMapInfo.Substring(charMapInfo.Length - 1);
                    this.oldFontInfo.charList.Add(currentChar);

                    // 追加扩展文字
                    if (i >= oldCharListLen)
                    {
                        addedChar.Append(currentChar);
                    }
                }

                // 扩展字库
                if (addedChar.Length > 0)
                {
                    this.AddSpecialHanzi(oldCharListLen, addedChar.ToString());
                }
            }
            else
            {
                throw new Exception("中日文字符对照表文件没找到！");
            }
        }

        /// <summary>
        /// 生成日中对照表
        /// </summary>
        private void CreateCharJpCnTable()
        {
            try
            {
                int index = 0;
                List<string> charMapList = new List<string>();
                this.oldFontInfo.codeCharMap.ForEach(p => charMapList.Add((index++).ToString().PadLeft(4, '0') + "=" + p.Value));

                // 保存文件
                string charMapFileName = this.GetCharMapFileName();
                File.WriteAllLines(charMapFileName, charMapList.ToArray(), Encoding.UTF8);

                MessageBox.Show("生成了字符映射表：\n" + charMapFileName);
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
            }
        }

        /// <summary>
        /// 取得字符映射表名称
        /// </summary>
        /// <returns></returns>
        private string GetCharMapFileName()
        {
            FileInfo fi = new FileInfo(Util.jpCnCharMapFileName);
            return fi.Directory.FullName + @"\Wii" + Util.jpCnCharMapFileName.Replace(@"./", "");
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

            Rectangle rectangle;
            FontFamily family = new FontFamily(txtFontTest.Font.Name);
            Pen blackPen = new Pen(Color.DimGray, 1f);

            // 图片
            Bitmap bmp = new Bitmap(this.oldFontInfo.tglpHeader.ImageWidth, this.oldFontInfo.tglpHeader.ImageHeight);
            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(this.GetFontImgBkColor(this.oldFontInfo.tglpHeader.ImageFormat));

            // 字符相关属性
            // 字符的宽度
            int charWidth;
            // 字符左边距离边框的距离
            int charLeftWidth;
            bool needCharBorder = this.NeedCharBorder(this.oldFontInfo.tglpHeader.ImageFormat);

            for (int y = 0; y < this.oldFontInfo.tglpHeader.CharactersPerColumn; y++)
            {
                for (int x = 0; x < this.oldFontInfo.tglpHeader.CharactersPerRow; x++)
                {
                    this.ProcessBarStep();
                    if (unicodeCharIndex >= unicodeCharList.Count)
                    {
                        break;
                    }
                    string strCurrentChar = unicodeCharList[unicodeCharIndex];

                    if (!string.Empty.Equals(strCurrentChar))
                    {
                        // 半角文字靠左对齐
                        if (Util.GetUnicodeFromStr(strCurrentChar) < 255)
                        {
                            sf.Alignment = StringAlignment.Near;
                        }
                        else
                        {
                            sf.Alignment = StringAlignment.Center;
                        }

                        // 在指定的区域内写入特定汉字
                        rectangle = new Rectangle(x * (this.oldFontInfo.tglpHeader.CellWidth + 1), y * (this.oldFontInfo.tglpHeader.CellHeight + 1),
                            this.oldFontInfo.tglpHeader.CellWidth + 1, this.oldFontInfo.tglpHeader.CellHeight + 1);
                        GraphicsPath graphPath = new GraphicsPath();
                        graphPath.AddString(strCurrentChar, family, (int)FontStyle.Bold, this.oldFontInfo.tglpHeader.FontCharacterHeight, rectangle, sf);

                        // 为了实现图像里面是白色，边框是黑色，真是费了一番脑筋
                        // 下面是最后的方案，过程真是艰辛，结果还真简单
                        g.FillPath(Brushes.White, graphPath);
                        if (needCharBorder)
                        {
                            g.DrawPath(blackPen, graphPath);
                        }

                        if (string.Empty.Equals(strCurrentChar.Trim()))
                        {
                            newCwdhEntriesList.Add(new CwdhEntries(13, 0, 13));
                        }
                        else
                        {
                            //Region[] charRegions = g.MeasureCharacterRanges(strCurrentChar, txtFontTest.Font, rectangle, sf);
                            //charWidth = (int)charRegions[0].GetBounds(g).Width;
                            charWidth = this.GetImgCharWidth(bmp, rectangle);

                            // 为了理解和计算CwdhEntries的三个参数，也是花费了一番周折
                            // 第一个参数：应该是Left marging，离前一个字符的距离
                            // 第二个参数：显示字符时，从当前字符图片中，取得的宽度
                            // 第一个参数：显示字符时，这个字符占的宽度
                            int blankWidth = (this.oldFontInfo.tglpHeader.FontCharacterWidth + 1 - charWidth);
                            if (blankWidth <= 0)
                            {
                                charLeftWidth = 0;
                            }
                            else if (blankWidth % 2 > 0)
                            {
                                charLeftWidth = blankWidth / 2 + 1;
                            }
                            else
                            {
                                charLeftWidth = blankWidth / 2;
                            }
                            charWidth += charLeftWidth;
                            
                            newCwdhEntriesList.Add(
                                new CwdhEntries(-charLeftWidth, charWidth, charWidth));
                        }
                    }

                    unicodeCharIndex++;
                }
            }

            imageData.Add(Util.ImageEncode(bmp, this.oldFontInfo.tglpHeader.ImageFormat));

            return bmp;
        }

        /// <summary>
        /// 取得字符的宽度
        /// </summary>
        /// <param name="charBmp"></param>
        /// <returns></returns>
        private int GetImgCharWidth(Bitmap pageBmp, Rectangle rectangle)
        {
            int beginX = -1;
            int endX = -1;
            for (int x = rectangle.X; x < rectangle.Width; x++)
            {
                for (int y = rectangle.Y; y < rectangle.Height; y++)
                {
                    if (pageBmp.GetPixel(x, y).ToArgb() != 0)
                    {
                        beginX = x;
                        break;
                    }
                }
                if (beginX >= 0)
                {
                    break;
                }
            }

            for (int x = rectangle.Width - 1; x >= rectangle.X; x--)
            {
                for (int y = rectangle.Y; y < rectangle.Height; y++)
                {
                    if (pageBmp.GetPixel(x, y).ToArgb() != 0)
                    {
                        endX = x;
                        break;
                    }
                }
                if (endX >= 0)
                {
                    break;
                }
            }

            return (endX + 1) - (beginX - 1);
        }

        /// <summary>
        /// 追加新汉字到字库
        /// </summary>
        /// <param name="hanzi"></param>
        private void AddSpecialHanzi(int charPos, string hanzi)
        {
            try
            {
                // 去掉和元字库重复的文字
                int newCharNum = 0;
                List<byte> newCmapEntries = new List<byte>();
                for (int i = 0; i < hanzi.Length; i++)
                {
                    string strChar = hanzi.Substring(i, 1);
                    // 设置CmapEntires信息
                    int charCode = Util.GetUnicodeFromStr(strChar);

                    newCmapEntries.Add((byte)((charCode >> 8) & 0xFF));
                    newCmapEntries.Add((byte)(charCode & 0xFF));
                    newCmapEntries.Add((byte)((charPos >> 8) & 0xFF));
                    newCmapEntries.Add((byte)(charPos & 0xFF));

                    newCharNum++;
                    charPos++;
                }

                // 追加最后一个结束CmapEntries信息
                newCmapEntries.Add(0);
                newCmapEntries.Add(0);

                //// 追加新的Cmap和CmapEntires信息
                //Cmap newCmap = new Cmap();
                //newCmap.CmapType = 2;
                //newCmap.FirstChar = 0;
                //newCmap.LastChar = 0xFFFF;
                //newCmap.LengthOfSection = 22 + newCharNum * 4;
                //newCmap.OffsetToNextCMAPdata = 0;
                //newCmap.TextureEntry = newCharNum;

                //this.oldFontInfo.byCmapList.Add(this.GetCmapByte(newCmap));
                //this.oldFontInfo.byCmapEntriesList.Add(newCmapEntries.ToArray());

                // 修改最后一个CmapEntires信息
                byte[] byLastCmapEntries = this.oldFontInfo.byCmapEntriesList[this.oldFontInfo.byCmapEntriesList.Count - 1];
                byte[] byAddCmapEntries = newCmapEntries.ToArray();
                byte[] byNewLast = new byte[byLastCmapEntries.Length + byAddCmapEntries.Length - 2];
                Array.Copy(byLastCmapEntries, 0, byNewLast, 0, byLastCmapEntries.Length - 2);
                Array.Copy(byAddCmapEntries, 0, byNewLast, byLastCmapEntries.Length - 2, byAddCmapEntries.Length);

                this.oldFontInfo.byCmapEntriesList.RemoveAt(this.oldFontInfo.byCmapEntriesList.Count - 1);
                this.oldFontInfo.byCmapEntriesList.Add(byNewLast);

                // 修改最后一个Cmap的Entries个数
                byte[] byLastCmap = this.oldFontInfo.byCmapList[this.oldFontInfo.byCmapList.Count - 1];
                int lastCmapSectionLen = Util.GetOffset(byLastCmap, 4, 7) + newCmapEntries.Count - 2;
                int lastCmapTextureEntry = Util.GetOffset(byLastCmap, 20, 21) + newCmapEntries.Count / 4;
                byLastCmap[4] = (byte)((lastCmapSectionLen >> 24) & 0xFF);
                byLastCmap[5] = (byte)((lastCmapSectionLen >> 16) & 0xFF);
                byLastCmap[6] = (byte)((lastCmapSectionLen >> 8) & 0xFF);
                byLastCmap[7] = (byte)(lastCmapSectionLen & 0xFF);
                byLastCmap[20] = (byte)((lastCmapTextureEntry >> 8) & 0xFF);
                byLastCmap[21] = (byte)(lastCmapTextureEntry & 0xFF);
                byte[] byNewLastCmap = new byte[22];
                Array.Copy(byLastCmap, 0, byNewLastCmap, 0, byNewLastCmap.Length);
                this.oldFontInfo.byCmapList.RemoveAt(this.oldFontInfo.byCmapList.Count - 1);
                this.oldFontInfo.byCmapList.Add(byNewLastCmap);
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
            }
        }

        /// <summary>
        /// 根据Cmap信息返回字节数据
        /// </summary>
        /// <param name="cmap"></param>
        /// <returns></returns>
        private byte[] GetCmapByte(Cmap cmap)
        {
            List<byte> listCmap = new List<byte>();

            byte[] cmapBytes = Encoding.UTF8.GetBytes("CMAP");
            listCmap.Add(cmapBytes[0]);
            listCmap.Add(cmapBytes[1]);
            listCmap.Add(cmapBytes[2]);
            listCmap.Add(cmapBytes[3]);
            listCmap.Add((byte)((cmap.LengthOfSection >> 24) & 0xFF));
            listCmap.Add((byte)((cmap.LengthOfSection >> 16) & 0xFF));
            listCmap.Add((byte)((cmap.LengthOfSection >> 8) & 0xFF));
            listCmap.Add((byte)((cmap.LengthOfSection) & 0xFF));
            listCmap.Add((byte)((cmap.FirstChar >> 8) & 0xFF));
            listCmap.Add((byte)(cmap.FirstChar & 0xFF));
            listCmap.Add((byte)((cmap.LastChar >> 8) & 0xFF));
            listCmap.Add((byte)(cmap.LastChar & 0xFF));
            listCmap.Add(0);
            listCmap.Add((byte)(cmap.CmapType));
            listCmap.Add(0);
            listCmap.Add(0);
            listCmap.Add((byte)((cmap.OffsetToNextCMAPdata >> 24) & 0xFF));
            listCmap.Add((byte)((cmap.OffsetToNextCMAPdata >> 16) & 0xFF));
            listCmap.Add((byte)((cmap.OffsetToNextCMAPdata >> 8) & 0xFF));
            listCmap.Add((byte)((cmap.OffsetToNextCMAPdata) & 0xFF));

            if (cmap.CmapType == 0
                || cmap.CmapType == 2)
            {
                listCmap.Add((byte)((cmap.TextureEntry >> 8) & 0xFF));
                listCmap.Add((byte)(cmap.TextureEntry & 0xFF));

                if (cmap.CmapType == 0)
                {
                    listCmap.Add(0);
                    listCmap.Add(0);
                }
            }

            return listCmap.ToArray();
        }

        /// <summary>
        /// 设置下一个Cmap的偏移
        /// </summary>
        /// <param name="cmap"></param>
        private void SetNextCmapOffset(byte[] cmap, int offsetToNextCMAPdata)
        {
            cmap[16] = (byte)((offsetToNextCMAPdata >> 24) & 0xFF);
            cmap[17] = (byte)((offsetToNextCMAPdata >> 16) & 0xFF);
            cmap[18] = (byte)((offsetToNextCMAPdata >> 8) & 0xFF);
            cmap[19] = (byte)(offsetToNextCMAPdata & 0xFF);
        }

        /// <summary>
        /// 生产新的字库文件
        /// </summary>
        /// <param name="newCwdhEntriesList"></param>
        /// <param name="newCmapList"></param>
        /// <param name="imageData"></param>
        private byte[] GenerateNewFont(List<CwdhEntries> newCwdhEntriesList, List<byte[]> newCmapList, List<byte[]> cmapEntries, byte[] imageData)
        {
            #region " 设置RFNT Header、Pinf Header信息 "

            // 设置RFNT信息
            // (RFNT + FINF + TGLP) + textures + CWDH
            // 计算总字节数
            int intBeforeCwdhSize = 96;
            int intAfterCwdhSize = 0;
            intBeforeCwdhSize += imageData.Length + 16;

            intAfterCwdhSize = (newCwdhEntriesList.Count) * 3;
            intAfterCwdhSize += newCmapList.Count * 24;
            foreach (byte[] cmapItem in newCmapList)
            {
                int cmapType = Util.GetOffset(cmapItem, 12, 13);
                if (cmapType == 1)
                {
                    intAfterCwdhSize -= 4;
                }
                else if (cmapType == 2)
                {
                    intAfterCwdhSize -= 2;
                }
            }
            foreach (byte[] entriesItem in cmapEntries)
            {
                intAfterCwdhSize += entriesItem.Length;
            }
            byte[] newFontByte = new byte[intBeforeCwdhSize + intAfterCwdhSize];
            this.oldFontInfo.rfntHeader.LengthOfFileInBytes = newFontByte.Length;
            this.oldFontInfo.byRfntHeader[0x8] = (byte)((this.oldFontInfo.rfntHeader.LengthOfFileInBytes >> 24) & 0xFF);
            this.oldFontInfo.byRfntHeader[0x9] = (byte)((this.oldFontInfo.rfntHeader.LengthOfFileInBytes >> 16) & 0xFF);
            this.oldFontInfo.byRfntHeader[0xA] = (byte)((this.oldFontInfo.rfntHeader.LengthOfFileInBytes >> 8) & 0xFF);
            this.oldFontInfo.byRfntHeader[0xB] = (byte)(this.oldFontInfo.rfntHeader.LengthOfFileInBytes & 0xFF);
            Array.Copy(this.oldFontInfo.byRfntHeader, 0, newFontByte, 0, this.oldFontInfo.byRfntHeader.Length);

            // 设置FinfHeader信息
            this.oldFontInfo.finfHeader.TglpDataOffset = 0x38;
            this.oldFontInfo.byPinfHeader[0x10] = (byte)((this.oldFontInfo.finfHeader.TglpDataOffset >> 24) & 0xFF);
            this.oldFontInfo.byPinfHeader[0x11] = (byte)((this.oldFontInfo.finfHeader.TglpDataOffset >> 16) & 0xFF);
            this.oldFontInfo.byPinfHeader[0x12] = (byte)((this.oldFontInfo.finfHeader.TglpDataOffset >> 8) & 0xFF);
            this.oldFontInfo.byPinfHeader[0x13] = (byte)(this.oldFontInfo.finfHeader.TglpDataOffset & 0xFF);

            this.oldFontInfo.finfHeader.CwdhDataOffset = intBeforeCwdhSize - 8;
            this.oldFontInfo.byPinfHeader[0x14] = (byte)((this.oldFontInfo.finfHeader.CwdhDataOffset >> 24) & 0xFF);
            this.oldFontInfo.byPinfHeader[0x15] = (byte)((this.oldFontInfo.finfHeader.CwdhDataOffset >> 16) & 0xFF);
            this.oldFontInfo.byPinfHeader[0x16] = (byte)((this.oldFontInfo.finfHeader.CwdhDataOffset >> 8) & 0xFF);
            this.oldFontInfo.byPinfHeader[0x17] = (byte)(this.oldFontInfo.finfHeader.CwdhDataOffset & 0xFF);

            this.oldFontInfo.finfHeader.CMapDataOffset = intBeforeCwdhSize + (newCwdhEntriesList.Count) * 3 + 8;
            this.oldFontInfo.byPinfHeader[0x18] = (byte)((this.oldFontInfo.finfHeader.CMapDataOffset >> 24) & 0xFF);
            this.oldFontInfo.byPinfHeader[0x19] = (byte)((this.oldFontInfo.finfHeader.CMapDataOffset >> 16) & 0xFF);
            this.oldFontInfo.byPinfHeader[0x1A] = (byte)((this.oldFontInfo.finfHeader.CMapDataOffset >> 8) & 0xFF);
            this.oldFontInfo.byPinfHeader[0x1B] = (byte)(this.oldFontInfo.finfHeader.CMapDataOffset & 0xFF);

            #endregion

            #region " 设置TGLP "

            // 保存TGLP、textures
            this.oldFontInfo.tglpHeader.LengthOfSection = imageData.Length + 48;
            this.oldFontInfo.byTgplHeader[4] = (byte)((this.oldFontInfo.tglpHeader.LengthOfSection >> 24) & 0xFF);
            this.oldFontInfo.byTgplHeader[5] = (byte)((this.oldFontInfo.tglpHeader.LengthOfSection >> 16) & 0xFF);
            this.oldFontInfo.byTgplHeader[6] = (byte)((this.oldFontInfo.tglpHeader.LengthOfSection >> 8) & 0xFF);
            this.oldFontInfo.byTgplHeader[7] = (byte)(this.oldFontInfo.tglpHeader.LengthOfSection & 0xFF);

            this.oldFontInfo.tglpHeader.TextureNum = imageData.Length / this.oldFontInfo.tglpHeader.TextureSize;
            this.oldFontInfo.byTgplHeader[0x10] = (byte)((this.oldFontInfo.tglpHeader.TextureNum >> 8) & 0xFF);
            this.oldFontInfo.byTgplHeader[0x11] = (byte)(this.oldFontInfo.tglpHeader.TextureNum & 0xFF);

            // 写入PinfHeader、TgplHeader
            Array.Copy(this.oldFontInfo.byPinfHeader, 0, newFontByte, this.oldFontInfo.byRfntHeader.Length, this.oldFontInfo.byPinfHeader.Length);
            Array.Copy(this.oldFontInfo.byTgplHeader, 0, newFontByte, this.oldFontInfo.byPinfHeader.Length + this.oldFontInfo.byRfntHeader.Length, this.oldFontInfo.byTgplHeader.Length);

            // 当前字节写入位置
            int intStartPos = 96;

            Array.Copy(imageData, 0, newFontByte, intStartPos, imageData.Length);

            #endregion

            #region " 设置CWDH "

            // 设置CWDH Section信息
            this.oldFontInfo.cwdhSection.LengthOfSection = (newCwdhEntriesList.Count) * 3 + 16;
            this.oldFontInfo.cwdhSection.NumEntries = newCwdhEntriesList.Count - 2;
            this.oldFontInfo.byCwdhSection[0x4] = (byte)((this.oldFontInfo.cwdhSection.LengthOfSection >> 24) & 0xFF);
            this.oldFontInfo.byCwdhSection[0x5] = (byte)((this.oldFontInfo.cwdhSection.LengthOfSection >> 16) & 0xFF);
            this.oldFontInfo.byCwdhSection[0x6] = (byte)((this.oldFontInfo.cwdhSection.LengthOfSection >> 8) & 0xFF);
            this.oldFontInfo.byCwdhSection[0x7] = (byte)(this.oldFontInfo.cwdhSection.LengthOfSection & 0xFF);
            this.oldFontInfo.byCwdhSection[0x8] = (byte)((this.oldFontInfo.cwdhSection.NumEntries >> 24) & 0xFF);
            this.oldFontInfo.byCwdhSection[0x9] = (byte)((this.oldFontInfo.cwdhSection.NumEntries >> 16) & 0xFF);
            this.oldFontInfo.byCwdhSection[0xA] = (byte)((this.oldFontInfo.cwdhSection.NumEntries >> 8) & 0xFF);
            this.oldFontInfo.byCwdhSection[0xB] = (byte)(this.oldFontInfo.cwdhSection.NumEntries & 0xFF);

            // 保存CWDH信息
            intStartPos += imageData.Length;
            Array.Copy(this.oldFontInfo.byCwdhSection, 0, newFontByte, intStartPos, this.oldFontInfo.byCwdhSection.Length);
            intStartPos += this.oldFontInfo.byCwdhSection.Length;

            // 写入CWDH Entries信息
            for (int i = 0; i < newCwdhEntriesList.Count; i++)
            {
                newFontByte[intStartPos++] = (byte)(newCwdhEntriesList[i].Unknown1);
                newFontByte[intStartPos++] = (byte)(newCwdhEntriesList[i].Unknown2);
                newFontByte[intStartPos++] = (byte)(newCwdhEntriesList[i].Unknown3);
            }

            #endregion

            #region " 设置CMAP "

            // 设置CMAP信息
            int intCmapEntriesIndex = 0;
            byte[] cmapEntriesItem = new byte[0];
            int offsetToNextCMAPdata = this.oldFontInfo.finfHeader.CMapDataOffset;
            for (int i = 0; i < newCmapList.Count; i++)
            {
                int cmapType = Util.GetOffset(newCmapList[i], 12, 13);
                byte[] cmap = newCmapList[i];

                // 设置CMAP Entries信息
                if (cmapType == 0)
                {
                    if (i < (newCmapList.Count - 1))
                    {
                        offsetToNextCMAPdata += 24;
                        this.SetNextCmapOffset(cmap, offsetToNextCMAPdata);
                    }
                    else
                    {
                        this.SetNextCmapOffset(cmap, 0);
                    }

                    Array.Copy(cmap, 0, newFontByte, intStartPos, cmap.Length);
                    intStartPos += cmap.Length;
                }
                else if (cmapType == 1)
                {
                    cmapEntriesItem = cmapEntries[intCmapEntriesIndex++];
                    if (i < (newCmapList.Count - 1))
                    {
                        offsetToNextCMAPdata += 20 + cmapEntriesItem.Length;
                        this.SetNextCmapOffset(cmap, offsetToNextCMAPdata);
                    }
                    else
                    {
                        this.SetNextCmapOffset(cmap, 0);
                    }

                    Array.Copy(cmap, 0, newFontByte, intStartPos, cmap.Length);
                    intStartPos += cmap.Length;

                    Array.Copy(cmapEntriesItem, 0, newFontByte, intStartPos, cmapEntriesItem.Length);
                    intStartPos += cmapEntriesItem.Length;
                }
                else if (cmapType == 2)
                {
                    cmapEntriesItem = cmapEntries[intCmapEntriesIndex++];
                    if (i < (newCmapList.Count - 1))
                    {
                        offsetToNextCMAPdata += 22 + cmapEntriesItem.Length;
                        this.SetNextCmapOffset(cmap, offsetToNextCMAPdata);
                    }
                    else
                    {
                        this.SetNextCmapOffset(cmap, 0);
                    }

                    Array.Copy(cmap, 0, newFontByte, intStartPos, cmap.Length);
                    intStartPos += cmap.Length;

                    Array.Copy(cmapEntriesItem, 0, newFontByte, intStartPos, cmapEntriesItem.Length);
                    intStartPos += cmapEntriesItem.Length;
                }
            }

            #endregion

            return newFontByte;
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
            string strImageFormat = this.oldFontInfo.tglpHeader.ImageFormat;
            int textureSize = this.oldFontInfo.tglpHeader.TextureSize;
            int imageWidth = this.oldFontInfo.tglpHeader.ImageWidth;
            int imageHeight = this.oldFontInfo.tglpHeader.ImageHeight;
            int[] blockHeightWidth = Util.GetBlockWidthHeight(strImageFormat);

            // 将原始图像数据分成小块
            List<byte[]> textureList = new List<byte[]>();
            int copyStartIndex = 0;
            int imageLength = imageData.Length;
            while (copyStartIndex < imageLength)
            {
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

                this.GetForm().ProcessBarStep();
            }

            return imageList.ToArray();
        }

        /// <summary>
        /// 取得当前BaseForm
        /// </summary>
        /// <returns></returns>
        private BaseForm GetForm()
        {
            if (this.Owner == null)
            {
                return this;
            }
            else
            {
                return (BaseForm)this.Owner;
            }
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

                // 先读取Rfnt Header
                Array.Copy(byData, 0, this.oldFontInfo.byRfntHeader, 0, this.oldFontInfo.byRfntHeader.Length);
                this.oldFontInfo.rfntHeader.FileMagic = Util.GetHeaderString(byData, 0, 3);
                this.oldFontInfo.rfntHeader.Endianess = Util.GetBytesString(byData, 0x4, 0x5);
                this.oldFontInfo.rfntHeader.VersionMinor = Util.GetBytesString(byData, 0x6, 0x7);
                this.oldFontInfo.rfntHeader.LengthOfFileInBytes = Util.GetOffset(byData, 0x8, 0xb);
                this.oldFontInfo.rfntHeader.OffsetToFinfHeader = Util.GetOffset(byData, 0xc, 0xd);
                this.oldFontInfo.rfntHeader.NumberOfSections = Util.GetOffset(byData, 0xe, 0xf);

                #endregion

                #region " 获取FINF Section信息 "

                // 获取FINF Section信息
                Array.Copy(byData, this.oldFontInfo.rfntHeader.OffsetToFinfHeader, this.oldFontInfo.byPinfHeader, 0, this.oldFontInfo.byPinfHeader.Length);

                // 获取FINF Section中相关信息
                this.oldFontInfo.finfHeader.Magic = Util.GetHeaderString(this.oldFontInfo.byPinfHeader, 0, 3);
                this.oldFontInfo.finfHeader.LengthOfSectionInBytes = Util.GetOffset(this.oldFontInfo.byPinfHeader, 0x4, 0x7);
                this.oldFontInfo.finfHeader.Fonttype = Util.GetBytesString(this.oldFontInfo.byPinfHeader, 0x8, 0x8);
                this.oldFontInfo.finfHeader.Leading = Util.GetBytesString(this.oldFontInfo.byPinfHeader, 0x9, 0x9);

                this.oldFontInfo.finfHeader.Leftmargin = Util.GetOffset(this.oldFontInfo.byPinfHeader, 0xc, 0xc);
                this.oldFontInfo.finfHeader.CharWidth = Util.GetOffset(this.oldFontInfo.byPinfHeader, 0xd, 0xd);
                this.oldFontInfo.finfHeader.FullWidth = Util.GetOffset(this.oldFontInfo.byPinfHeader, 0xe, 0xe);
                this.oldFontInfo.finfHeader.Encoding = Util.GetOffset(this.oldFontInfo.byPinfHeader, 0xf, 0xf);
                this.oldFontInfo.finfHeader.DefaultChar = Util.GetStrFromNumber(Util.GetOffset(this.oldFontInfo.byPinfHeader, 0xa, 0xb),
                    this.oldFontInfo.finfHeader.Encoding, this.oldFontInfo.rfntHeader.Endianess);
                this.oldFontInfo.finfHeader.TglpDataOffset = Util.GetOffset(this.oldFontInfo.byPinfHeader, 0x10, 0x13);
                this.oldFontInfo.finfHeader.CwdhDataOffset = Util.GetOffset(this.oldFontInfo.byPinfHeader, 0x14, 0x17);
                this.oldFontInfo.finfHeader.CMapDataOffset = Util.GetOffset(this.oldFontInfo.byPinfHeader, 0x18, 0x1b);
                this.oldFontInfo.finfHeader.Height = Util.GetOffset(this.oldFontInfo.byPinfHeader, 0x1c, 0x1c);
                this.oldFontInfo.finfHeader.Width = Util.GetOffset(this.oldFontInfo.byPinfHeader, 0x1d, 0x1d);

                // 设置字库编码器
                this.oldFontInfo.encoding = Util.GetFontEncoding(this.oldFontInfo.finfHeader.Encoding, this.oldFontInfo.rfntHeader.Endianess);

                #endregion

                #region " 获取TGLP Header的字节信息 "

                // 获取TGLP Header的字节信息
                Array.Copy(byData, this.oldFontInfo.finfHeader.TglpDataOffset - 8, this.oldFontInfo.byTgplHeader, 0, this.oldFontInfo.byTgplHeader.Length);

                // 获取TGLP Header的具体信息
                this.oldFontInfo.tglpHeader.Magic = Util.GetHeaderString(this.oldFontInfo.byTgplHeader, 0, 3);
                this.oldFontInfo.tglpHeader.LengthOfSection = Util.GetOffset(this.oldFontInfo.byTgplHeader, 0x4, 0x7);
                this.oldFontInfo.tglpHeader.CellWidth = Util.GetOffset(this.oldFontInfo.byTgplHeader, 0x8, 0x8);
                this.oldFontInfo.tglpHeader.CellHeight = Util.GetOffset(this.oldFontInfo.byTgplHeader, 0x9, 0x9);
                this.oldFontInfo.tglpHeader.FontCharacterWidth = Util.GetOffset(this.oldFontInfo.byTgplHeader, 0xa, 0xa);
                this.oldFontInfo.tglpHeader.FontCharacterHeight = Util.GetOffset(this.oldFontInfo.byTgplHeader, 0xb, 0xb);
                this.oldFontInfo.tglpHeader.TextureSize = Util.GetOffset(this.oldFontInfo.byTgplHeader, 0xc, 0xf);
                this.oldFontInfo.tglpHeader.TextureNum = Util.GetOffset(this.oldFontInfo.byTgplHeader, 0x10, 0x11);
                this.oldFontInfo.tglpHeader.ImageFormat = Util.GetImageFormat(Util.GetOffset(this.oldFontInfo.byTgplHeader, 0x12, 0x13));
                this.oldFontInfo.tglpHeader.CharactersPerRow = Util.GetOffset(this.oldFontInfo.byTgplHeader, 0x14, 0x15);
                this.oldFontInfo.tglpHeader.CharactersPerColumn = Util.GetOffset(this.oldFontInfo.byTgplHeader, 0x16, 0x17);
                this.oldFontInfo.tglpHeader.ImageWidth = Util.GetOffset(this.oldFontInfo.byTgplHeader, 0x18, 0x19);
                this.oldFontInfo.tglpHeader.ImageHeight = Util.GetOffset(this.oldFontInfo.byTgplHeader, 0x1a, 0x1b);
                this.oldFontInfo.tglpHeader.PositionOfData = Util.GetOffset(this.oldFontInfo.byTgplHeader, 0x1c, 0x1f);

                #endregion

                #region " 取得TGLP Data信息 "

                // 取得TGLP Data信息
                byte[] byteTglpData = new byte[this.oldFontInfo.tglpHeader.LengthOfSection - 48];
                Array.Copy(byData, this.oldFontInfo.tglpHeader.PositionOfData, byteTglpData, 0, byteTglpData.Length);

                #endregion

                #region " 取得CWDH Section信息 "

                // 取得CWDH Section信息
                this.oldFontInfo.cwdhSection = new CwdhSection();
                Array.Copy(byData, this.oldFontInfo.finfHeader.CwdhDataOffset - 8, this.oldFontInfo.byCwdhSection, 0, this.oldFontInfo.byCwdhSection.Length);

                this.oldFontInfo.cwdhSection.Magic = Util.GetHeaderString(this.oldFontInfo.byCwdhSection, 0, 3);
                this.oldFontInfo.cwdhSection.LengthOfSection = Util.GetOffset(this.oldFontInfo.byCwdhSection, 4, 7);
                this.oldFontInfo.cwdhSection.NumEntries = Util.GetOffset(this.oldFontInfo.byCwdhSection, 0x8, 0xb);
                this.oldFontInfo.cwdhSection.FirstCharacter = Util.GetOffset(this.oldFontInfo.byCwdhSection, 0xc, 0xf);
                if (!"CWDH".Equals(this.oldFontInfo.cwdhSection.Magic))
                {
                    MessageBox.Show("错误的CWDH！");
                    return null;
                }

                #endregion

                #region " 获取CWDH entries信息 "

                this.oldFontInfo.cwdhEntriesList = new List<CwdhEntries>();
                int intCwdhEntriesPos = this.oldFontInfo.finfHeader.CwdhDataOffset + 8;
                for (int i = 0; i <= this.oldFontInfo.cwdhSection.NumEntries; i++)
                {
                    byte[] cwdhEntriesItem = new byte[3];
                    Array.Copy(byData, intCwdhEntriesPos, cwdhEntriesItem, 0, cwdhEntriesItem.Length);

                    CwdhEntries item = new CwdhEntries((sbyte)cwdhEntriesItem[0], cwdhEntriesItem[1], (sbyte)cwdhEntriesItem[2]);
                    this.oldFontInfo.cwdhEntriesList.Add(item);

                    intCwdhEntriesPos += 3;
                }
                this.oldFontInfo.cwdhEntriesList.Add(new CwdhEntries(0, 0, 0));

                #endregion

                #region " 取得CMap(字符映射表)信息 "

                int cmapPos = this.oldFontInfo.finfHeader.CMapDataOffset - 8;
                while (cmapPos != -8)
                {
                    // 取得Cmap基本信息
                    byte[] cmapData = new byte[24];
                    Array.Copy(byData, cmapPos, cmapData, 0, cmapData.Length);

                    Cmap cmapItem = new Cmap();
                    cmapItem.Magic = Util.GetHeaderString(cmapData, 0, 3);
                    if (!"CMAP".Equals(cmapItem.Magic))
                    {
                        MessageBox.Show("错误的Cmap格式！");
                        return null;
                    }
                    cmapItem.LengthOfSection = Util.GetOffset(cmapData, 4, 7);
                    cmapItem.FirstChar = Util.GetOffset(cmapData, 8, 9);
                    cmapItem.LastChar = Util.GetOffset(cmapData, 10, 11);
                    cmapItem.CmapType = Util.GetOffset(cmapData, 12, 13);
                    cmapItem.OffsetToNextCMAPdata = Util.GetOffset(cmapData, 16, 19);
                    cmapItem.TextureEntry = Util.GetOffset(cmapData, 20, 21);

                    // 开始循环判断Cmap信息
                    int charIndex;
                    switch (cmapItem.CmapType)
                    {
                        case 0:
                            // 保存Cmap信息
                            this.oldFontInfo.byCmapList.Add(cmapData);

                            // 保存CmapEntries信息
                            charIndex = cmapItem.TextureEntry;
                            for (int i = cmapItem.FirstChar; i <= cmapItem.LastChar; i++)
                            {
                                this.oldFontInfo.cmapList.Add(new KeyValuePair<int, int>(charIndex++, i));
                            }
                            break;

                        case 1:
                            // 保存Cmap信息
                            byte[] newCmapData = new byte[20];
                            Array.Copy(cmapData, 0, newCmapData, 0, newCmapData.Length);
                            this.oldFontInfo.byCmapList.Add(newCmapData);

                            // 保存CmapEntries信息
                            byte[] indexByte = new byte[(cmapItem.LastChar - cmapItem.FirstChar + 2) * 2];
                            Array.Copy(byData, cmapPos + 20, indexByte, 0, indexByte.Length);
                            this.oldFontInfo.byCmapEntriesList.Add(indexByte);

                            int[] indexEntries = new int[indexByte.Length / 2];
                            int byteIndex = 0;
                            for (int i = 0; i < indexEntries.Length; i++)
                            {
                                indexEntries[i] = Util.GetOffset(indexByte, byteIndex, byteIndex + 1);
                                byteIndex += 2;
                            }

                            int entriesIndex = 0;
                            for (int i = cmapItem.FirstChar; i <= cmapItem.LastChar; i++)
                            {
                                charIndex = indexEntries[entriesIndex++];

                                this.oldFontInfo.cmapList.Add(new KeyValuePair<int, int>(charIndex, i));
                            }
                            this.oldFontInfo.cmapList.Add(new KeyValuePair<int, int>(0, 0));
                            break;

                        case 2:
                            // 保存Cmap信息
                            newCmapData = new byte[22];
                            Array.Copy(cmapData, 0, newCmapData, 0, newCmapData.Length);
                            this.oldFontInfo.byCmapList.Add(newCmapData);

                            // 保存CmapEntries信息
                            byte[] entriesByte = new byte[cmapItem.TextureEntry * 4 + 2];
                            Array.Copy(byData, cmapPos + 22, entriesByte, 0, entriesByte.Length);
                            this.oldFontInfo.byCmapEntriesList.Add(entriesByte);
                            entriesIndex = 0;
                            for (int i = 0; i < cmapItem.TextureEntry; i++)
                            {
                                this.oldFontInfo.cmapList.Add(new KeyValuePair<int, int>(
                                    Util.GetOffset(entriesByte, entriesIndex + 2, entriesIndex + 3),
                                    Util.GetOffset(entriesByte, entriesIndex, entriesIndex + 1)));
                                entriesIndex += 4;
                            }
                            break;

                        default:
                            MessageBox.Show("错误的Cmap格式！");
                            return null;
                    }

                    cmapPos = cmapItem.OffsetToNextCMAPdata - 8;
                }

                #endregion

                // CMap(字符映射表)信息排序
                this.oldFontInfo.cmapList.Sort(Util.Comparison);

                // 保存原来的Cmap信息
                this.oldFontInfo.charList.Clear();
                this.oldFontInfo.codeCharMap.Clear();
                foreach (KeyValuePair<int, int> item in this.oldFontInfo.cmapList)
                {
                    if (item.Key != 0xFFFF && item.Value != 0)
                    {
                        // 取得日文字符
                        string jpChar = Util.GetStrFromNumber(item.Value,
                            this.oldFontInfo.finfHeader.Encoding, this.oldFontInfo.rfntHeader.Endianess);
                        
                        // 保存日文字符
                        this.oldFontInfo.charList.Add(jpChar);

                        // 保存Code、日文字符映射
                        this.oldFontInfo.codeCharMap.Add(new KeyValuePair<string, string>(item.Value.ToString().PadLeft(4, '0'), jpChar));
                    }
                }

                return byteTglpData;
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
                this.oldFontInfo.cwdhEntriesList = new List<CwdhEntries>();
                int unicodeCharIndex = 0;

                while (unicodeCharIndex < this.oldFontInfo.charList.Count)
                {
                    // 生成一页的图片
                    Bitmap newImg = this.CreateFontImagePage(
                        imageData, this.oldFontInfo.cwdhEntriesList, this.oldFontInfo.charList, ref unicodeCharIndex);

                    // 将一页图片分割成小个子
                    List<Bitmap[]> pageImage = Util.GetRowColImage((Bitmap)newImg, this.oldFontInfo.tglpHeader.CellWidth + 1, this.oldFontInfo.tglpHeader.CellHeight + 1);
                    foreach (Bitmap[] rowImage in pageImage)
                    {
                        rowColImg.Add(rowImage);
                    }
                }
                this.oldFontInfo.cwdhEntriesList.Add(new CwdhEntries(0, 0, 0));

                // 图像数据转换
                this.oldFontInfo.imgData = new byte[imageData.Count * this.oldFontInfo.tglpHeader.TextureSize];
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

        /// <summary>
        /// 根据图片类型取得图片背景色
        /// </summary>
        /// <param name="imgFormat"></param>
        /// <returns></returns>
        private Color GetFontImgBkColor(string imgFormat)
        {
            if ("I4".Equals(imgFormat))
            {
                return Color.Black;
            }
            else
            {
                return Color.FromArgb(0, 0, 0, 0);
            }
        }

        /// <summary>
        /// 判断字符是否需要边框
        /// </summary>
        /// <param name="imgFormat"></param>
        /// <returns></returns>
        private bool NeedCharBorder(string imgFormat)
        {
            if ("I4".Equals(imgFormat))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion
    }
}
