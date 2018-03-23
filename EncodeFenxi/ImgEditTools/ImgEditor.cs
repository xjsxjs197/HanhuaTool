using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Hanhua.Common;

namespace Hanhua.ImgEditTools
{
    /// <summary>
    /// 查找文件中特殊图片的工具
    /// </summary>
    public partial class ImgEditor : BaseForm
    {
        #region " 私有变量 "

        /// <summary>
        /// 所有图片数据
        /// </summary>
        private List<byte[]> ImgFiles = new List<byte[]>();

        /// <summary>
        /// 记录文件名
        /// </summary>
        private List<string> ImgFileNames = new List<string>();

        /// <summary>
        /// 保存图片是否被导入过
        /// </summary>
        private List<bool> ImgChangeInfo = new List<bool>();

        /// <summary>
        /// 右键菜单
        /// </summary>
        private ContextMenuStrip contextMenu = new ContextMenuStrip();

        /// <summary>
        /// 图片类型
        /// </summary>
        private ImgType imgType;

        /// <summary>
        /// 当前的图片编辑器
        /// </summary>
        private ImgEditorBase currentImgEditor;

        /// <summary>
        /// 选中的图片Index
        /// </summary>
        private int selectedImgIndex = 0;

        #endregion

        #region " 初始化 "

        /// <summary>
        /// 初始化
        /// </summary>
        public ImgEditor()
        {
            InitializeComponent();

            this.ResetHeight();

            // 设置图片类型
            this.SetImgType();

            // 设置右键菜单
            this.SetContextMenu();

            // 事件绑定
            this.imgGrid.CellMouseUp += new DataGridViewCellMouseEventHandler(this.imgGrid_CellMouseUp);
            this.FormClosing += new FormClosingEventHandler(this.ImgEditTools_FormClosing);
        }

        #endregion

        #region " 页面事件 "

        /// <summary>
        /// 关闭窗口时提示保存信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImgEditTools_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult chk = this.CheckChange();
            if (chk == DialogResult.Cancel || chk == DialogResult.Yes)
            {
                e.Cancel = true;
                return;
            }
        }

        /// <summary>
        /// 读取目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadDic_Click(object sender, EventArgs e)
        {
            DialogResult chk = this.CheckChange();
            if (chk == DialogResult.Cancel || chk == DialogResult.Yes)
            {
                return;
            }

            this.baseFolder = Util.OpenFolder(string.Empty);
            if (string.IsNullOrEmpty(this.baseFolder))
            {
                return;
            }

            // 初始化文件列表
            this.Do(this.SearchImgFiles, this.chkCheckOther.Checked);
        }

        /// <summary>
        /// 读取单个文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelFile_Click(object sender, EventArgs e)
        {
            DialogResult chk = this.CheckChange();
            if (chk == DialogResult.Cancel || chk == DialogResult.Yes)
            {
                return;
            }

            // 打开要分析的文件
            this.baseFile = Util.SetOpenDailog(this.GetOpenFileFilter(), string.Empty);
            if (string.IsNullOrEmpty(this.baseFile))
            {
                return;
            }

            this.Do(this.LoadSelectedFile, this.chkCheckOther.Checked);
        }

        /// <summary>
        /// 切换图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstImg_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.baseFile = this.ImgFileNames[this.lstImg.SelectedIndex];
            this.currentImgEditor.editingFile = this.baseFile;
            Image imgSelected = null;

            try
            {
                // 获取图片
                this.currentImgEditor.paletteIndex = 0;
                imgSelected = this.AddImageToGrid(this.currentImgEditor.ImageDecode(this.ImgFiles[this.lstImg.SelectedIndex], this.lstImg.Items[this.lstImg.SelectedIndex].ToString()));

                // 取得调色板个数
                if (this.imgType == ImgType.Tim)
                {
                    if (this.currentImgEditor.paletteCount > 0)
                    {
                        this.cmbTimPalette.Items.Clear();
                        for (int i = 0; i < this.currentImgEditor.paletteCount; i++)
                        {
                            this.cmbTimPalette.Items.Add(i);
                        }

                        this.lblPalette.Visible = true;
                        this.cmbTimPalette.Visible = true;
                    }
                    else
                    {
                        this.lblPalette.Visible = false;
                        this.cmbTimPalette.Visible = false;
                    }
                }
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
            }

            // 设置图片信息
            this.Text = this.currentImgEditor.GetEditorTitle(imgSelected) + "  No：" + this.lstImg.SelectedIndex.ToString().PadLeft(4, '0');

            this.lstImg.Focus();
        }

        /// <summary>
        /// 切换调色板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTimPalette_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.currentImgEditor.paletteIndex = this.cmbTimPalette.SelectedIndex;
            
            // 获取图片
            this.AddImageToGrid(this.currentImgEditor.ImageDecode(this.ImgFiles[this.lstImg.SelectedIndex], this.lstImg.Items[this.lstImg.SelectedIndex].ToString()));

            this.lstImg.Focus();
        }

        /// <summary>
        /// 右键菜单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // 导入、导出图片旋转信息判断
            RotateFlipType rotateFlipType = RotateFlipType.RotateNoneFlipNone;
            if (e.ClickedItem.Name.IndexOf("FlipUpDown") >= 0)
            {
                rotateFlipType = RotateFlipType.Rotate180FlipX;
            }

            this.contextMenu.Visible = false;
            switch (e.ClickedItem.Name)
            {
                // 导出
                case "export":
                case "exportFlipUpDown":
                    this.baseFile = Util.SetSaveDailog("图片文件（*.png）|*.png|所有文件|*.*", string.Empty);
                    if (string.IsNullOrEmpty(this.baseFile))
                    {
                        return;
                    }
                    this.Do(this.ExportSelectedImg, this.lstImg.SelectedIndex, rotateFlipType);
                    break;

                // 导出所有
                case "exportAll":
                case "exportFlipUpDownAll":
                    if (this.GetFolder() == false)
                    {
                        return;
                    }
                    this.Do(this.ExportAllImg, rotateFlipType);
                    break;

                // 导入
                case "import":
                case "importFlipUpDown":
                    this.baseFile = Util.SetOpenDailog("图片类型数据(*.png)|*.png|所有类型(*.*)|*.*", string.Empty);
                    if (string.IsNullOrEmpty(this.baseFile))
                    {
                        return;
                    }
                    this.Do(this.ImportImg, this.lstImg.SelectedIndex, this.lstImg.Items[this.lstImg.SelectedIndex].ToString(), rotateFlipType);
                    break;

                // 导入所有
                case "importAll":
                case "importFlipUpDownAll":
                    if (this.GetFolder() == false)
                    {
                        return;
                    }
                    this.Do(this.ImportAllImg, rotateFlipType);
                    break;

                // 保存
                case "save":
                    this.Do(this.Save);
                    break;
            }
        }

        /// <summary>
        /// 弹出右键菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgGrid_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            // 只有右键弹出菜单
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0 && e.RowIndex < this.imgGrid.Rows.Count)
                {
                    // 显示操作菜单
                    this.ShowContextMenu(e.RowIndex);
                }
            }
        }

        /// <summary>
        /// 切换图片类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbImgType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 改变图片类型
            this.imgType = (ImgType)this.cmbImgType.SelectedIndex;

            // 改变表示文字
            string imgTypeName = Enum.GetName(typeof(ImgType), this.imgType);
            this.Text = "{0}图片汉化专用工具".Replace("{0}", imgTypeName);
            this.chkCheckOther.Text = "是否分析非{0}后缀的文件".Replace("{0}", imgTypeName);

            // 特殊处理Adt压缩图片类型
            if (this.imgType == ImgType.Adt)
            {
                this.chkCheckOther.Checked = false;
                this.chkCheckOther.Visible = false;
            }
            else
            {
                this.chkCheckOther.Visible = true;
            }

            // Tim格式需要显示调色板下拉框
            if (this.imgType == ImgType.Tim)
            {
                this.lblPalette.Visible = true;
                this.cmbTimPalette.Visible = true;
            }
            else
            {
                this.lblPalette.Visible = false;
                this.cmbTimPalette.Visible = false;
            }

            // 改变图片编辑器
            this.currentImgEditor = this.GetImgEditor();

            // 清空各种缓存
            this.lstImg.Items.Clear();
            this.ImgFiles.Clear();
            this.ImgFileNames.Clear();
            this.imgGrid.Rows.Clear();
        }

        /// <summary>
        /// 保存修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Do(this.Save);
        }

        /// <summary>
        /// 各种操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDoWork_Click(object sender, EventArgs e)
        {
            // 显示操作菜单
            this.ShowContextMenu(this.selectedImgIndex);
        }

        #endregion

        #region " 私有方法 "

        /// <summary>
        /// 显示操作菜单
        /// </summary>
        /// <param name="imgIndex"></param>
        private void ShowContextMenu(int imgIndex)
        {
            // 菜单项目制御
            if (this.currentImgEditor is ImgEditorTpl)
            {
                this.SetFlipUpDownMenuItemVisible(true);
            }
            else
            {
                this.SetFlipUpDownMenuItemVisible(false);
            }

            // 只有存在数据才弹出菜单
            this.selectedImgIndex = imgIndex;
            Point p = Control.MousePosition;
            this.contextMenu.Show(p);
        }

        /// <summary>
        /// 打开导入、导出的目录
        /// </summary>
        /// <returns></returns>
        private bool GetFolder()
        {
            if (this.ImgFileNames.Count == 0)
            {
                return false;
            }

            string defaultFolder = string.Empty;
            if (!string.IsNullOrEmpty(this.baseFile))
            {
                defaultFolder = this.baseFile.Replace(Util.GetShortName(this.baseFile), string.Empty);
            }

            if (string.IsNullOrEmpty(this.baseFolder = Util.OpenFolder(defaultFolder)))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 设置翻转菜单的可见性
        /// </summary>
        /// <param name="visible"></param>
        private void SetFlipUpDownMenuItemVisible(bool visible)
        {
            this.contextMenu.Items["exportFlipUpDown"].Visible = visible;
            this.contextMenu.Items["exportFlipUpDownAll"].Visible = visible;
            this.contextMenu.Items["exportFlipUpDownAllLine"].Visible = visible;
            this.contextMenu.Items["importFlipUpDown"].Visible = visible;
            this.contextMenu.Items["importFlipUpDownAll"].Visible = visible;
            this.contextMenu.Items["importFlipUpDownAllLine"].Visible = visible;
        }

        /// <summary>
        /// 判断图片是否变更
        /// </summary>
        /// <returns></returns>
        private DialogResult CheckChange()
        {
            if (this.ImgChangeInfo.Count(p => p) > 0)
            {
                // 如果存在变更的图片，提示保存信息
                DialogResult dr = MessageBox.Show("图片被变更过，需要保存吗？", "是否保存", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Yes)
                {
                    this.Do(this.Save);
                }

                return dr;
            }

            return DialogResult.None;
        }

        /// <summary>
        /// 取得图片编辑器
        /// </summary>
        /// <returns></returns>
        private ImgEditorBase GetImgEditor()
        {
            switch (this.imgType)
            {
                case ImgType.Adt:
                    return new ImgEditorAdt(baseFile);

                case ImgType.Tim:
                    return new ImgEditorTim(baseFile);

                case ImgType.Tim2:
                    return new ImgEditorTim2(baseFile);

                case ImgType.Jpg:
                    return new ImgEditorJpg(baseFile);

                case ImgType.Gvr:
                    return new ImgEditorGvr(baseFile);

                case ImgType.Pvr:
                    return new ImgEditorPvr(baseFile);

                case ImgType.Mhp:
                    return new ImgEditorMhp(baseFile);

                case ImgType.Tpl:
                    return new ImgEditorTpl(baseFile);
            }

            return new ImgEditorBase(this.baseFile);
        }

        /// <summary>
        /// 取得打开文件时的过滤字符串
        /// </summary>
        /// <returns></returns>
        private string GetOpenFileFilter()
        { 
            switch (this.imgType)
            {
                case ImgType.Tim:
                    return "Ps Tim 图片文件（*.tim）|*.tim|所有文件|*.*";

                case ImgType.Tim2:
                    return "Ps2 Tim2 图片文件（*.tim2）|*.tim|所有文件|*.*";

                case ImgType.Jpg:
                    return "Jpeg 图片文件（*.jpg）|*.jpg|所有文件|*.*";

                case ImgType.Gvr:
                    return "Dc Gvr 图片文件（*.gvr）|*.gvr|所有文件|*.*";
            }

            return "所有文件|*.*";
        }

        /// <summary>
        /// 设置图片类型
        /// </summary>
        private void SetImgType()
        {
            this.cmbImgType.Items.Clear();
            this.cmbImgType.Items.Add("Ps Tim");
            this.cmbImgType.Items.Add("Ps2 Tim2");
            this.cmbImgType.Items.Add("Jpeg");
            this.cmbImgType.Items.Add("Ngc Grv");
            this.cmbImgType.Items.Add("Ps Adt");
            this.cmbImgType.Items.Add("Bio0 mhp");
            this.cmbImgType.Items.Add("Dc Prv");
            this.cmbImgType.Items.Add("Ngc Tpl");

            this.cmbImgType.SelectedIndex = 7;
        }

        /// <summary>
        /// 初始化文件列表
        /// </summary>
        /// <param name="folder"></param>
        private void SearchImgFiles(params object[] param)
        {
            bool isChkCheckOther = (bool)param[0];

            this.Invoke((MethodInvoker)delegate()
            {
                this.lstImg.Items.Clear();
            });

            this.ImgFiles.Clear();
            this.ImgFileNames.Clear();
            this.ImgChangeInfo.Clear();

            Util.NeedCheckTpl = true;
            List<FilePosInfo> fileNameInfo = Util.GetAllFiles(this.baseFolder).Where(p => !p.IsFolder).ToList();
            Util.NeedCheckTpl = false;

            // 显示进度条
            this.ResetProcessBar(fileNameInfo.Count);
            this.currentImgEditor.Reset();

            foreach (FilePosInfo file in fileNameInfo)
            {
                this.LoadImgFromFile(isChkCheckOther, file.File);

                // 更新进度条
                this.ProcessBarStep();
            }

            this.Invoke((MethodInvoker)delegate()
            {
                if (this.ImgFiles.Count > 0)
                {
                    this.lstImg.SelectedIndex = 0;
                }
            });

            // 隐藏进度条
            this.CloseProcessBar();
        }

        /// <summary>
        /// 取得选择的文件
        /// </summary>
        private void LoadSelectedFile(params object[] param)
        {
            bool isChkCheckOther = (bool)param[0];

            this.Invoke((MethodInvoker)delegate()
            {
                this.lstImg.Items.Clear();
            });

            this.ImgFiles.Clear();
            this.ImgFileNames.Clear();
            this.ImgChangeInfo.Clear();

            this.currentImgEditor.Reset();
            this.LoadImgFromFile(isChkCheckOther, this.baseFile);

            this.Invoke((MethodInvoker)delegate()
            {
                if (this.ImgFiles.Count > 0)
                {
                    this.lstImg.SelectedIndex = 0;
                }
            });
        }

        /// <summary>
        /// 读取图片
        /// </summary>
        /// <param name="file"></param>
        private void LoadImgFromFile(bool isChkCheckOther, string file)
        {
            bool isAddFile = true;

            // 读入文件内容
            byte[] byData = File.ReadAllBytes(file);

            if (file.EndsWith(Enum.GetName(typeof(ImgType), this.imgType), StringComparison.OrdinalIgnoreCase))
            {
                if (this.imgType == ImgType.Gvr || this.imgType == ImgType.Tim || this.imgType == ImgType.Adt
                    || this.imgType == ImgType.Mhp)
                {
                    isAddFile = false;
                }
                else
                {
                    isAddFile = true;
                }
            }
            else if (isChkCheckOther)
            {
                isAddFile = false;
            }
            else
            {
                return;
            }

            if (isAddFile)
            {
                // 如果是单个图片，直接加入List
                this.ImgFileNames.Add(file);
                this.ImgFiles.Add(byData);
                this.ImgChangeInfo.Add(false);
                this.Invoke((MethodInvoker)delegate()
                {
                    this.lstImg.Items.Add(Util.GetShortName(file));
                });
            }
            else
            {
                // 分析文件内部是否包括特定图片文件
                List<string> imgInfos = new List<string>();
                List<byte[]> byImgList = this.currentImgEditor.SearchImg(byData, file, imgInfos);
                if (byImgList != null && byImgList.Count > 0)
                {
                    int imgCount = byImgList.Count;
                    while (imgCount-- > 0)
                    {
                        this.ImgFileNames.Add(file.ToLower().Replace(".adt", ".tim"));
                        this.ImgChangeInfo.Add(false);
                    }

                    this.ImgFiles.AddRange(byImgList.ToArray());
                    this.Invoke((MethodInvoker)delegate()
                    {
                        this.lstImg.Items.AddRange(imgInfos.ToArray());
                    });
                }
            }
        }

        /// <summary>
        /// 设置右键菜单
        /// </summary>
        private void SetContextMenu()
        {
            // 绑定右键菜单事件
            this.contextMenu.ItemClicked -= new ToolStripItemClickedEventHandler(this.ContextMenu_ItemClicked);
            this.contextMenu.ItemClicked += new ToolStripItemClickedEventHandler(this.ContextMenu_ItemClicked);

            // 添加菜单
            this.contextMenu.Items.Clear();
            ToolStripMenuItem item = new ToolStripMenuItem();
            item.Name = "export";
            item.Text = "单个图片导出";
            this.contextMenu.Items.Add(item);

            item = new ToolStripMenuItem();
            item.Name = "exportAll";
            item.Text = "所有图片导出";
            this.contextMenu.Items.Add(item);
            this.contextMenu.Items.Add(new ToolStripSeparator());

            item = new ToolStripMenuItem();
            item.Name = "exportFlipUpDown";
            item.Text = "上下翻转导出";
            this.contextMenu.Items.Add(item);

            item = new ToolStripMenuItem();
            item.Name = "exportFlipUpDownAll";
            item.Text = "上下翻转导出所有";
            this.contextMenu.Items.Add(item);
            ToolStripSeparator exportFlipUpDownAllLine = new ToolStripSeparator();
            exportFlipUpDownAllLine.Name = "exportFlipUpDownAllLine";
            this.contextMenu.Items.Add(exportFlipUpDownAllLine);

            item = new ToolStripMenuItem();
            item.Name = "import";
            item.Text = "单个图片导入";
            item.Enabled = true;
            this.contextMenu.Items.Add(item);

            item = new ToolStripMenuItem();
            item.Name = "importAll";
            item.Text = "所有图片导入";
            item.Enabled = true;
            this.contextMenu.Items.Add(item);
            this.contextMenu.Items.Add(new ToolStripSeparator());

            item = new ToolStripMenuItem();
            item.Name = "importFlipUpDown";
            item.Text = "上下翻转导入";
            this.contextMenu.Items.Add(item);

            item = new ToolStripMenuItem();
            item.Name = "importFlipUpDownAll";
            item.Text = "上下翻转导入所有";
            this.contextMenu.Items.Add(item);
            ToolStripSeparator importFlipUpDownAllLine = new ToolStripSeparator();
            importFlipUpDownAllLine.Name = "importFlipUpDownAllLine";
            this.contextMenu.Items.Add(importFlipUpDownAllLine);

            item = new ToolStripMenuItem();
            item.Name = "save";
            item.Text = "保存修改";
            item.Enabled = true;
            this.contextMenu.Items.Add(item);
        }

        /// <summary>
        /// 导出选择节点的图片
        /// </summary>
        private void ExportSelectedImg(params object[] param)
        {
            int selectedIndex = (int)param[0];
            this.currentImgEditor.rotateFlipType = (RotateFlipType)param[1];

            // 开始保存文件
            Image selImg = (Image) this.imgGrid.Rows[this.selectedImgIndex].Cells[0].Value;
            this.currentImgEditor.ExportSelectedImg(selImg, this.baseFile, this.ImgFiles[selectedIndex]);
        }

        /// <summary>
        /// 导出所有的图片
        /// </summary>
        private void ExportAllImg(params object[] param)
        {
            this.currentImgEditor.rotateFlipType = (RotateFlipType)param[0];

            // 显示进度条
            this.ResetProcessBar(this.ImgFileNames.Count);

            //int imgIndex = -1;
            //int subImgIndex = -1;
            string oldName = string.Empty;
            int subIndex = 0;
            Image[] img = null;

            for (int i = 0; i < this.ImgFileNames.Count; i++)
            {
                // 取得当前文件名
                string shortName = Util.GetShortNameWithoutType(this.ImgFileNames[i]);

                // 取得当前图片
                string itemText = string.Empty;
                this.Invoke((MethodInvoker)delegate()
                {
                    itemText = this.lstImg.Items[i].ToString();
                });
                img = this.currentImgEditor.ImageDecode(this.ImgFiles[i], itemText);

                // 取得图片位置信息
                string[] fileInfo = Regex.Split(itemText, "　");
                string posInfo = fileInfo[fileInfo.Length - 1];

                // 开始保存文件

                if (!oldName.Equals(shortName))
                {
                    subIndex = 0;
                    oldName = shortName;
                }
                else
                {
                    subIndex++;
                }

                if (img.Length == 1)
                {
                    shortName = this.baseFolder + @"\" + oldName + "_" + subIndex.ToString().PadLeft(2, '0') + ".png";
                    this.currentImgEditor.ExportSelectedImg(img[0], shortName, this.ImgFiles[i]);
                }
                else
                {
                    for (int j = 0; j < img.Length; j++)
                    {
                        shortName = this.baseFolder + @"\" + oldName + "_" + subIndex.ToString().PadLeft(2, '0') + "_" + j.ToString().PadLeft(2, '0') + ".png";
                        this.currentImgEditor.ExportSelectedImg(img[j], shortName, this.ImgFiles[i]);
                    }
                }

                // 更新进度条
                this.ProcessBarStep();
            }

            // 隐藏进度条
            this.CloseProcessBar();

            MessageBox.Show("所有图片导出完成");
        }

        /// <summary>
        /// 导入图片
        /// </summary>
        private void ImportImg(params object[] param)
        {
            int selectedIndex = (int)param[0];
            string itemText = (string)param[1];
            this.currentImgEditor.rotateFlipType = (RotateFlipType)param[2];
            Image selImg = (Image)this.imgGrid.Rows[this.selectedImgIndex].Cells[0].Value;

            this.ImgFiles[selectedIndex] =
                    this.currentImgEditor.ImportImg(this.baseFile, selImg, this.ImgFiles[selectedIndex], itemText);
            this.ImgChangeInfo[selectedIndex] = true;

            this.Invoke((MethodInvoker)delegate()
            {
                // 切换最新图片
                this.lstImg_SelectedIndexChanged(this.lstImg, new EventArgs());
            });
        }

        /// <summary>
        /// 导入所有的图片
        /// </summary>
        private void ImportAllImg(params object[] param)
        {
            List<FilePosInfo> fileNameInfo =
                    Util.GetAllFiles(this.baseFolder).Where(p => !p.IsFolder
                        && p.File.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
                        ).ToList();
            if (fileNameInfo.Count == 0)
            {
                MessageBox.Show("没有发现能导入的文件，请重新检查选择的目录，或导入的文件是否是[fileName_XX.png]的格式");
                return;
            }

            this.currentImgEditor.rotateFlipType = (RotateFlipType)param[0];

            // 统计信息
            List<string> impFiles = new List<string>();
            int impImgCount = 0;

            // 显示进度条
            this.ResetProcessBar(fileNameInfo.Count);

            for (int i = 0; i < fileNameInfo.Count; i++)
            {
                FilePosInfo fileInfo = fileNameInfo[i];

                // 取得要导入的文件名
                string[] names = Util.GetShortName(fileInfo.File).Replace(".png", string.Empty).Split('_');
                string impShortName = names[0];
                int impFileIndex = Convert.ToInt16(names[1]);

                // 查找导入文件的位置
                int oldFileIndex = -1;
                int oldFileSubIndex = 0;
                string itemText = string.Empty;
                this.Invoke((MethodInvoker)delegate()
                {
                    for (int j = 0; j < this.lstImg.Items.Count; j++)
                    {
                        itemText = this.lstImg.Items[j].ToString();
                        if (itemText.IndexOf(impShortName) >= 0)
                        {
                            if (oldFileSubIndex == impFileIndex)
                            {
                                oldFileIndex = j;
                                break;
                            }
                            else
                            {
                                oldFileSubIndex++;
                            }
                        }
                    }
                });
                

                if (oldFileIndex != -1)
                {
                    // 保存当前文件名
                    impImgCount++;
                    if (!impFiles.Contains(impShortName))
                    {
                        impFiles.Add(impShortName);
                    }

                    this.Invoke((MethodInvoker)delegate()
                    {
                        this.lstImg.SelectedIndex = oldFileIndex;
                    });

                    Image[] oldImg = this.currentImgEditor.ImageDecode(this.ImgFiles[oldFileIndex], itemText);
                    if (oldImg.Length == 1)
                    {
                        this.ImgFiles[oldFileIndex] =
                            this.currentImgEditor.ImportImg(fileInfo.File, oldImg[0], this.ImgFiles[oldFileIndex], itemText);
                    }
                    else
                    {
                        int gridImgIndex = Convert.ToInt16(names[2]);
                        this.currentImgEditor.imageIndex = gridImgIndex;
                        this.ImgFiles[oldFileIndex] =
                            this.currentImgEditor.ImportImg(fileInfo.File, oldImg[gridImgIndex], this.ImgFiles[oldFileIndex], itemText);
                    }
                    
                    this.ImgChangeInfo[oldFileIndex] = true;
                }

                // 更新进度条
                this.ProcessBarStep();
            }

            // 隐藏进度条
            this.CloseProcessBar();

            StringBuilder sb = new StringBuilder();
            sb.Append("所有图片导入完成！\n");
            sb.Append("总共：" + fileNameInfo.Count + " 张图片\n");
            sb.Append("其中：" + impImgCount + " 张图片，导入到了：" + impFiles.Count + " 个文件中\n");
            MessageBox.Show(sb.ToString());

            // 切换最新图片
            this.Invoke((MethodInvoker)delegate()
            {
                this.lstImg_SelectedIndexChanged(this.lstImg, new EventArgs());
            });
        }

        /// <summary>
        /// 保存修改
        /// </summary>
        private void Save()
        {
            if (this.ImgFileNames.Count == 0)
            {
                return;
            }

            // 显示进度条
            this.ResetProcessBar(this.ImgFileNames.Count);

            for (int i = 0; i < this.ImgChangeInfo.Count; i++)
            {
                if (this.ImgChangeInfo[i])
                {
                    this.ImgChangeInfo[i] = false;

                    // 读入文件内容
                    byte[] byData = File.ReadAllBytes(this.ImgFileNames[i]);

                    // 取得当前图片数据开始，结束位置
                    int startPos = 0;
                    int endPos = byData.Length;
                    string sortName = string.Empty;
                    this.Invoke((MethodInvoker)delegate()
                    {
                        sortName = this.lstImg.Items[i].ToString();
                    });
                    if (sortName.IndexOf("--") >= 0)
                    {
                        string[] names = sortName.Split('　');
                        string[] pos = Regex.Split(names[names.Length - 1], "--");
                        startPos = Convert.ToInt32(pos[0], 16);
                        endPos = Convert.ToInt32(pos[1], 16);
                    }

                    Array.Copy(this.ImgFiles[i], 0, byData, startPos, endPos - startPos);
                    this.currentImgEditor.Save(this.ImgFileNames[i], byData, sortName);
                }

                // 更新进度条
                this.ProcessBarStep();
            }

            // 隐藏进度条
            this.CloseProcessBar();

            MessageBox.Show("保存成功！");
        }

        /// <summary>
        /// 追加图片到Grid
        /// </summary>
        private Image AddImageToGrid(Image[] imgList)
        {
            // 取最大宽度和长度
            int maxWidth = imgList[0].Width;
            int maxHeight = imgList[0].Height;
            for (int i = 1; i < imgList.Length; i++)
            {
                if (imgList[i].Width > maxWidth)
                {
                    maxWidth = imgList[i].Width;
                }

                if (imgList[i].Height > maxHeight)
                {
                    maxHeight = imgList[i].Height;
                }
            }

            this.imgGrid.Columns[0].Width = maxWidth;

            this.imgGrid.Rows.Clear();
            for (int i = 0; i < imgList.Length; i++)
            {
                this.imgGrid.Rows.Add(new object[] { imgList[i] });
                this.imgGrid.Rows[i].Height = imgList[i].Height;
            }

            return imgList[0];
        }

        #endregion
    }
}
