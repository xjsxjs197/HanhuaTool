using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Hanhua.Common;

namespace Hanhua.TextEditTools.BioCvEdit
{
    /// <summary>
    /// BioCvAfsEditor
    /// </summary>
    public partial class BioCvAfsEditor : BaseForm
    {
        #region " 私有变量 "

        /// <summary>
        /// 记录所有文本文件
        /// </summary>
        private List<FilePosInfo> textFiles = new List<FilePosInfo>();

        /// <summary>
        /// 右键菜单
        /// </summary>
        private ContextMenuStrip contextMenu = new ContextMenuStrip();

        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        public BioCvAfsEditor()
        {
            InitializeComponent();

            this.SetContextMenu();
            this.lstAsfFiles.MouseUp += new MouseEventHandler(this.lstAsfFiles_MouseUp);
        }

        #region " 页面事件 "

        /// <summary>
        /// 选择目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelFolder_Click(object sender, EventArgs e)
        {
            this.baseFolder = Util.OpenFolder(string.Empty);
            if (string.IsNullOrEmpty(this.baseFolder))
            {
                return;
            }
        }

        /// <summary>
        /// 选择文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelFile_Click(object sender, EventArgs e)
        {
            // 打开要分析的文件
            this.baseFile = Util.SetOpenDailog("Afs 格式文件（*.afs）|*.afs", string.Empty);
            if (string.IsNullOrEmpty(this.baseFile))
            {
                return;
            }

            this.Do(this.SelFile);
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
                    this.Do(this.ExportFile);
                    break;

                // 导出所有
                case "exportAll":
                    this.Do(this.ExportAllFiles);
                    break;

                // 导入
                case "import":
                    this.Do(this.ImportFile);
                    break;

                // 导入所有
                case "importAll":
                    this.Do(this.ImportAllFiles);
                    break;
            }
        }

        /// <summary>
        /// 弹出右键菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstAsfFiles_MouseUp(object sender, MouseEventArgs e)
        {
            // 只有右键弹出菜单
            if (e.Button == MouseButtons.Right)
            {
                Point p = Control.MousePosition;
                this.contextMenu.Show(p);
            }
        }

        /// <summary>
        /// 选择文件变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstAsfFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilePosInfo fileInfo = this.textFiles[this.lstAsfFiles.SelectedIndex];
            this.baseFile = fileInfo.File;
        }

        /// <summary>
        /// 生成新的Afs文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateAfs_Click(object sender, EventArgs e)
        {
            string afsFiles = @"E:\游戏汉化\NgcBioCv\BioCvNgcCn\A\root\rdx_lnkA";
            List<FilePosInfo> fileInfos = Util.GetAllFiles(afsFiles).Where(p => !p.IsFolder && p.File.EndsWith(".rdx@", StringComparison.OrdinalIgnoreCase)).ToList();

            string oldAfsFile = @"E:\游戏汉化\NgcBioCv\BioCvNgcJp\A\root\rdx_lnk1.afs";
            byte[] byOldAfs = File.ReadAllBytes(oldAfsFile);
            int subFileCount = (byOldAfs[7] << 24) | (byOldAfs[6] << 16) | (byOldAfs[5] << 8) | byOldAfs[4];
            int entryPos = 8;
            int nameEntryPos = entryPos + subFileCount * 8;
            nameEntryPos = (byOldAfs[nameEntryPos + 3] << 24) | (byOldAfs[nameEntryPos + 2] << 16) | (byOldAfs[nameEntryPos + 1] << 8) | byOldAfs[nameEntryPos];

            List<FilePosInfo> oldFiles = new List<FilePosInfo>();
            int firstPos = 0;
            for (int i = 0; i < subFileCount + 1; i++)
            {
                FilePosInfo fileInfo = new FilePosInfo(oldAfsFile);
                fileInfo.FilePos = (byOldAfs[entryPos + i * 8 + 3] << 24) | (byOldAfs[entryPos + i * 8 + 2] << 16) | (byOldAfs[entryPos + i * 8 + 1] << 8) | byOldAfs[entryPos + i * 8];
                fileInfo.FileSize = (byOldAfs[entryPos + i * 8 + 7] << 24) | (byOldAfs[entryPos + i * 8 + 6] << 16) | (byOldAfs[entryPos + i * 8 + 5] << 8) | byOldAfs[entryPos + i * 8 + 4];

                oldFiles.Add(fileInfo);

                if (i == 0)
                {
                    firstPos = fileInfo.FilePos;
                }
            }

            if ((oldFiles.Count - fileInfos.Count) != 1)
            {
                return;
            }

            List<byte[]> newFileBy = new List<byte[]>();
            for (int i = 0; i < oldFiles.Count - 1; i++)
            {
                FilePosInfo oldItem = oldFiles[i];
                FilePosInfo newItem = fileInfos[i];

                byte[] cnFile = File.ReadAllBytes(newItem.File);
                newFileBy.Add(cnFile);
                int newSize = cnFile.Length;
                int sizeDiff = newSize - oldItem.FileSize;

                if (sizeDiff != 0)
                {
                    for (int j = i + 1; j < oldFiles.Count; j++)
                    {
                        FilePosInfo resetItem = oldFiles[j];
                        resetItem.FilePos += sizeDiff;

                        // 开始位置特殊处理，必须是2048的倍数
                        int chkDiffLen = resetItem.FilePos % 2048;
                        if (chkDiffLen > 0)
                        {
                            resetItem.FilePos += (2048 - chkDiffLen);
                        }
                    }
                }

                oldItem.FileSize = newSize;
            }

            FilePosInfo lastItem = oldFiles[oldFiles.Count - 1];
            byte[] byNewAfs = new byte[lastItem.FilePos + lastItem.FileSize];
            Array.Copy(byOldAfs, 0, byNewAfs, 0, 8);

            for (int i = 0; i < oldFiles.Count; i++)
            {
                FilePosInfo oldItem = oldFiles[i];
                byNewAfs[entryPos + i * 8] = (byte)(oldItem.FilePos & 0xFF);
                byNewAfs[entryPos + i * 8 + 1] = (byte)((oldItem.FilePos >> 8) & 0xFF);
                byNewAfs[entryPos + i * 8 + 2] = (byte)((oldItem.FilePos >> 16) & 0xFF);
                byNewAfs[entryPos + i * 8 + 3] = (byte)((oldItem.FilePos >> 24) & 0xFF);

                byNewAfs[entryPos + i * 8 + 4] = (byte)(oldItem.FileSize & 0xFF);
                byNewAfs[entryPos + i * 8 + 5] = (byte)((oldItem.FileSize >> 8) & 0xFF);
                byNewAfs[entryPos + i * 8 + 6] = (byte)((oldItem.FileSize >> 16) & 0xFF);
                byNewAfs[entryPos + i * 8 + 7] = (byte)((oldItem.FileSize >> 24) & 0xFF);

                if (i < oldFiles.Count - 1)
                {
                    FilePosInfo newItem = fileInfos[i];
                    Array.Copy(newFileBy[i], 0, byNewAfs, oldItem.FilePos, newFileBy[i].Length);
                }
                else
                {
                    Array.Copy(byOldAfs, nameEntryPos, byNewAfs, lastItem.FilePos, lastItem.FileSize);
                }
            }

            File.WriteAllBytes(oldAfsFile.Replace("BioCvNgcJp", "BioCvNgcCn"), byNewAfs);
        }

        #endregion

        #region " 私有方法 "

        /// <summary>
        /// 选择目录
        /// </summary>
        private void SelFolder()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(this.SelFolder));
            }
            else
            {
                this.lstAsfFiles.Items.Clear();
                this.textFiles.Clear();

                List<FilePosInfo> afsFiles = Util.GetAllFiles(this.baseFolder).Where(p => !p.IsFolder && p.File.EndsWith(".afs", StringComparison.OrdinalIgnoreCase)).ToList();

                // 显示进度条
                this.ResetProcessBar(afsFiles.Count);

                foreach (FilePosInfo file in afsFiles)
                {
                    this.AddAfsFile(file.File);

                    // 更新进度条
                    this.ProcessBarStep();
                }

                if (this.textFiles.Count > 0)
                {
                    this.lstAsfFiles.SelectedIndex = 0;
                    this.lstAsfFiles.Focus();
                }

                // 隐藏进度条
                this.CloseProcessBar();
            }
        }

        /// <summary>
        /// 选择单个文件
        /// </summary>
        private void SelFile()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(this.SelFile));
            }
            else
            {
                this.lstAsfFiles.Items.Clear();
                this.textFiles.Clear();

                this.AddAfsFile(this.baseFile);

                this.lstAsfFiles.SelectedIndex = 0;
                this.lstAsfFiles.Focus();
            }
        }

        /// <summary>
        /// 添加Afs文件
        /// </summary>
        /// <param name="afsFile"></param>
        private void AddAfsFile(string afsFile)
        {
            byte[] byAfs = File.ReadAllBytes(afsFile);
            int subFileCount = (byAfs[7] << 24) | (byAfs[6] << 16) | (byAfs[5] << 8) | byAfs[4];
            int entryPos = 8;
            int nameEntryPos = entryPos + subFileCount * 8;
            nameEntryPos = (byAfs[nameEntryPos + 3] << 24) | (byAfs[nameEntryPos + 2] << 16) | (byAfs[nameEntryPos + 1] << 8) | byAfs[nameEntryPos];

            for (int i = 0; i < subFileCount; i++)
            {
                FilePosInfo fileInfo = new FilePosInfo(afsFile);
                fileInfo.FilePos = (byAfs[entryPos + i * 8 + 3] << 24) | (byAfs[entryPos + i * 8 + 2] << 16) | (byAfs[entryPos + i * 8 + 1] << 8) | byAfs[entryPos + i * 8];
                fileInfo.FileSize = (byAfs[entryPos + i * 8 + 7] << 24) | (byAfs[entryPos + i * 8 + 6] << 16) | (byAfs[entryPos + i * 8 + 5] << 8) | byAfs[entryPos + i * 8 + 4];
                fileInfo.SubName = Util.GetFileNameFromStringTable(byAfs, nameEntryPos + i * 0x30);
                fileInfo.SubIndex = i.ToString();
                if (string.IsNullOrEmpty(fileInfo.SubName.Trim()))
                {
                    fileInfo.SubName = Util.GetShortName(afsFile) + "_" + i.ToString().PadLeft(2, '0');
                }

                this.textFiles.Add(fileInfo);
                this.lstAsfFiles.Items.Add(fileInfo.SubName.PadRight(40, ' ') + " " + fileInfo.FilePos.ToString("x") + "--" + (fileInfo.FilePos + fileInfo.FileSize).ToString("x"));
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
            item.Text = "单个文件导出";
            this.contextMenu.Items.Add(item);

            item = new ToolStripMenuItem();
            item.Name = "exportAll";
            item.Text = "所有文件导出";
            this.contextMenu.Items.Add(item);
            this.contextMenu.Items.Add(new ToolStripSeparator());

            item = new ToolStripMenuItem();
            item.Name = "import";
            item.Text = "单个文件导入";
            item.Enabled = true;
            this.contextMenu.Items.Add(item);

            item = new ToolStripMenuItem();
            item.Name = "importAll";
            item.Text = "所有文件导入";
            item.Enabled = true;
            this.contextMenu.Items.Add(item);
        }

        /// <summary>
        /// 导出选择节点的图片
        /// </summary>
        private void ExportFile()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(this.ExportFile));
            }
            else
            {
                if (this.textFiles.Count == 0 || string.IsNullOrEmpty(this.baseFolder = Util.OpenFolder(this.baseFile.Replace(Util.GetShortName(this.baseFile), string.Empty))))
                {
                    return;
                }

                // 开始保存文件
                this.ExportFile(this.textFiles[this.lstAsfFiles.SelectedIndex]);
            }
        }

        /// <summary>
        /// 导出所有的图片
        /// </summary>
        private void ExportAllFiles()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(this.ExportAllFiles));
            }
            else
            {
                if (this.textFiles.Count == 0 || string.IsNullOrEmpty(this.baseFolder = Util.OpenFolder(this.baseFile.Replace(Util.GetShortName(this.baseFile), string.Empty))))
                {
                    return;
                }

                // 显示进度条
                this.ResetProcessBar(this.textFiles.Count);

                for (int i = 0; i < this.textFiles.Count; i++)
                {
                    // 开始保存文件
                    this.ExportFile(this.textFiles[i]);

                    // 更新进度条
                    this.ProcessBarStep();
                }

                // 隐藏进度条
                this.CloseProcessBar();

                MessageBox.Show("所有文件导出完成");
            }
        }

        /// <summary>
        /// 导入图片
        /// </summary>
        private void ImportFile()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(this.ImportFile));
            }
            else
            {
                if (this.textFiles.Count == 0 || string.IsNullOrEmpty(this.baseFolder = Util.OpenFolder(this.baseFile.Replace(Util.GetShortName(this.baseFile), string.Empty))))
                {
                    return;
                }

                // 开始导入文件
                this.ImportFile(this.textFiles[this.lstAsfFiles.SelectedIndex]);
            }
        }

        /// <summary>
        /// 导入所有的图片
        /// </summary>
        private void ImportAllFiles()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(this.ImportAllFiles));
            }
            else
            {
                if (this.textFiles.Count == 0 || string.IsNullOrEmpty(this.baseFolder = Util.OpenFolder(this.baseFile.Replace(Util.GetShortName(this.baseFile), string.Empty))))
                {
                    return;
                }

                // 显示进度条
                this.ResetProcessBar(this.textFiles.Count);

                for (int i = 0; i < this.textFiles.Count; i++)
                {
                    // 开始导入文件
                    this.ImportFile(this.textFiles[i]);

                    // 更新进度条
                    this.ProcessBarStep();
                }

                // 隐藏进度条
                this.CloseProcessBar();

                MessageBox.Show("所有文件导入完成");
            }
        }

        /// <summary>
        /// 文件导出
        /// </summary>
        /// <param name="fileInfo"></param>
        private void ExportFile(FilePosInfo fileInfo)
        {
            FileStream fs = null;

            try
            {
                fs = File.Open(fileInfo.File, FileMode.Open);
                fs.Seek(fileInfo.FilePos, SeekOrigin.Begin);

                byte[] subFile = new byte[fileInfo.FileSize];
                fs.Read(subFile, 0, subFile.Length);

                File.WriteAllBytes(this.baseFolder + @"\" + fileInfo.SubName, subFile);
                fs.Close();
                fs = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }
            finally
            { 
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// 导入文件
        /// </summary>
        /// <param name="fileInfo"></param>
        private void ImportFile(FilePosInfo fileInfo)
        {
            FileStream fs = null;

            try
            {
                string impFile = this.baseFolder + @"\" + fileInfo.SubName;
                if (!File.Exists(impFile))
                {
                    throw new Exception("没有找到导入的文件！");
                }

                byte[] subFile = File.ReadAllBytes(impFile);
                if (subFile.Length > fileInfo.FileSize)
                {
                    throw new Exception("导入的文件容量大于原始文件！");
                }
                else if (impFile.ToLower().IndexOf(fileInfo.SubName.ToLower()) == -1)
                {
                    throw new Exception("导入的文件名和原始文件名不一致");
                }

                fs = File.Open(fileInfo.File, FileMode.Open);
                fs.Seek(fileInfo.FilePos, SeekOrigin.Begin);

                fs.Write(subFile, 0, subFile.Length);
                fs.Close();
                fs = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        #endregion
    }
}
