using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace Hanhua.Common.Bio1Edit
{
    /// <summary>
    /// 查找文件中Jpg的工具
    /// </summary>
    public partial class JpgImgSearch : BaseForm
    {
        #region " 私有变量 "

        /// <summary>
        /// 所有Tim文件
        /// </summary>
        private List<byte[]> jpgFiles = new List<byte[]>();

        /// <summary>
        /// 记录文件名
        /// </summary>
        private List<string> jpgFileNames = new List<string>();

        /// <summary>
        /// 保存图片是否被导入过
        /// </summary>
        private List<bool> jpgChangeInfo = new List<bool>();

        /// <summary>
        /// 右键菜单
        /// </summary>
        private ContextMenuStrip contextMenu = new ContextMenuStrip();

        #endregion

        #region " 初始化 "

        /// <summary>
        /// 初始化
        /// </summary>
        public JpgImgSearch()
        {
            InitializeComponent();

            // 设置右键菜单
            this.SetContextMenu();

            // 事件绑定
            this.imgPic.MouseUp += new MouseEventHandler(this.imgPic_MouseUp);
        }

        #endregion

        #region " 页面事件 "

        /// <summary>
        /// 读取目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadJpg_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            //folderDlg.SelectedPath = @"E:\My\Hanhua\testFile\bio3Tim";
            folderDlg.SelectedPath = @"D:\game\iso\wii\";
            folderDlg.Description = "打开需要查看的的路径";
            DialogResult dr = folderDlg.ShowDialog();
            string strFolder = folderDlg.SelectedPath;

            if (dr == DialogResult.Cancel || string.IsNullOrEmpty(strFolder))
            {
                return;
            }

            // 初始化文件列表
            this.InitFileList(strFolder);
        }

        /// <summary>
        /// 读取单个文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelFile_Click(object sender, EventArgs e)
        {
            // 打开要分析的文件
            if (!this.SetOpenDailogFilter("Jpg 图片文件（*.jpg）|*.jpg|所有文件|*.*"))
            {
                return;
            }

            this.lstImg.Items.Clear();
            this.jpgFiles.Clear();
            this.jpgFileNames.Clear();
            FileStream fs = null;

            try
            {
                this.LoadJpgFromFile(fs, this.strFileOpen);

                if (this.jpgFiles.Count > 0)
                {
                    this.lstImg.SelectedIndex = 0;
                }
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
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
        /// 切换图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstImg_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.strFileOpen = this.jpgFileNames[this.lstImg.SelectedIndex];

            // 获取图片
            this.imgPic.Image = this.GetJpgPic(this.jpgFiles[this.lstImg.SelectedIndex]);
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
                    this.ExportSelectedImg();
                    break;

                // 导入
                case "import":
                    this.ImportImg();
                    break;

                // 保存
                case "save":
                    this.Save();
                    break;
            }
        }

        /// <summary>
        /// 弹出右键菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgPic_MouseUp(object sender, MouseEventArgs e)
        {
            // 只有右键弹出菜单
            if (e.Button == MouseButtons.Right)
            {
                Point p = Control.MousePosition;
                this.contextMenu.Show(p);
            }
        }

        #endregion

        #region " 私有方法 "

        /// <summary>
        /// 初始化文件列表
        /// </summary>
        /// <param name="folder"></param>
        private void InitFileList(string folder)
        {
            this.lstImg.Items.Clear();
            this.jpgFiles.Clear();
            this.jpgFileNames.Clear();
            FileStream fs = null;

            try
            {
                List<FileNameInfo> fileNameInfo = Util.GetAllFiles(folder);
                foreach (FileNameInfo file in fileNameInfo)
                {
                    if (!file.IsFolder)
                    {
                        this.LoadJpgFromFile(fs, file.FileName);
                    }
                }

                if (this.jpgFiles.Count > 0)
                {
                    this.lstImg.SelectedIndex = 0;
                }
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
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
        /// 读取Jpg图片
        /// </summary>
        /// <param name="file"></param>
        private void LoadJpgFromFile(FileStream fs, string file)
        {
            // 读入文件内容
            fs = new FileStream(file, FileMode.Open);
            byte[] byData = new byte[fs.Length];
            fs.Read(byData, 0, byData.Length);
            fs.Close();

            if (file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
            {
                // 如果是单个图片，直接加入List
                this.jpgFileNames.Add(file);
                this.jpgFiles.Add(byData);
                this.jpgChangeInfo.Add(false);
                this.lstImg.Items.Add(this.GetShortName(file));
            }
            else if (this.chkCheckOther.Checked)
            {
                // 分析文件内部是否包括Tim文件
                byte[] byCheck = new byte[4];
                for (int i = 0; i < byData.Length - 4; i++)
                {
                    if (byData[i] == 0xFF && byData[i + 1] == 0xD8 && byData[i + 2] == 0xFF && byData[i + 3] == 0xE0)
                    {
                        for (int j = i + 4; j < byData.Length - 2; j++)
                        {
                            if (byData[j] == 0xFF && byData[j + 1] == 0xD9)
                            { 
                                byte[] byJpgData = new byte[j + 2 - i];
                                Array.Copy(byData, i, byJpgData, 0, byJpgData.Length);

                                this.jpgFileNames.Add(file);
                                this.jpgFiles.Add(byJpgData);
                                this.jpgChangeInfo.Add(false);
                                this.lstImg.Items.Add(this.GetShortName(file) + "　" + i.ToString("x") + "--" + (j + 2).ToString("x"));

                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 取得文件名
        /// </summary>
        /// <param name="fileFullName"></param>
        /// <returns></returns>
        private string GetShortName(string fileFullName)
        {
            string[] names = fileFullName.Split('\\');
            return names[names.Length - 1];
        }

        /// <summary>
        /// 取得图片
        /// </summary>
        /// <param name="byImgData"></param>
        /// <returns></returns>
        private Image GetJpgPic(byte[] byImgData)
        {
            MemoryStream ms = new MemoryStream(byImgData);
            ms.Position = 0;
            Image img = Image.FromStream(ms);
            ms.Close();

            return img;
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
            item.Text = "图片导出";
            this.contextMenu.Items.Add(item);
            this.contextMenu.Items.Add(new ToolStripSeparator());

            item = new ToolStripMenuItem();
            item.Name = "import";
            item.Text = "图片导入";
            item.Enabled = true;
            this.contextMenu.Items.Add(item);
            this.contextMenu.Items.Add(new ToolStripSeparator());

            item = new ToolStripMenuItem();
            item.Name = "save";
            item.Text = "保存修改";
            item.Enabled = true;
            this.contextMenu.Items.Add(item);

            this.contextMenu.Items.Add(item);
        }

        /// <summary>
        /// 导出选择节点的图片
        /// </summary>
        private void ExportSelectedImg()
        {
            if (!this.SetSaveDailogFilter("图片类型数据(*.jpg)|*.jpg|所有类型(*.*)|*.*"))
            {
                return;
            }

            try
            {
                // 开始保存文件
                File.WriteAllBytes(this.strFileOpen, this.jpgFiles[this.lstImg.SelectedIndex]);
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
            }
        }

        /// <summary>
        /// 导入图片
        /// </summary>
        private void ImportImg()
        {
            if (!this.SetOpenDailogFilter("图片类型数据(*.jpg)|*.jpg|所有类型(*.*)|*.*"))
            {
                return;
            }

            MemoryStream ms = null;

            try
            {
                Image img = Image.FromFile(this.strFileOpen);
                byte[] byImgData = new byte[this.jpgFiles[this.lstImg.SelectedIndex].Length];

                ms = new MemoryStream(byImgData);
                ms.Position = 0;
                img.Save(ms, ImageFormat.Jpeg);
                img.Dispose();

                this.jpgFiles[this.lstImg.SelectedIndex] = byImgData;
                this.jpgChangeInfo[this.lstImg.SelectedIndex] = true;
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
            }
            finally
            { 
                if (ms != null)
                {
                    ms.Close();
                }
            }
        }

        /// <summary>
        /// 保存修改
        /// </summary>
        private void Save()
        {
            FileStream fs = null;

            try
            {
                for (int i = 0; i < this.jpgChangeInfo.Count; i++)
                {
                    if (this.jpgChangeInfo[i])
                    {
                        int startPos = -1;
                        int endPos = -1;

                        string sortName = this.lstImg.Items[i].ToString();
                        if (!sortName.EndsWith(".jpg"))
                        {
                            string[] names = sortName.Split('　');
                            string[] pos = Regex.Split(names[names.Length - 1], "--");
                            startPos = Convert.ToInt32(pos[0], 16);
                            endPos = Convert.ToInt32(pos[1], 16);
                        }

                        // 读入文件内容
                        fs = new FileStream(this.jpgFileNames[i], FileMode.Open);
                        byte[] byData = new byte[fs.Length];
                        fs.Read(byData, 0, byData.Length);
                        fs.Close();

                        if (startPos == -1 || endPos == -1)
                        {
                            startPos = 0;
                            endPos = byData.Length;
                        }

                        Array.Copy(this.jpgFiles[i], 0, byData, startPos, endPos - startPos);
                        File.WriteAllBytes(this.jpgFileNames[i], byData);
                    }
                }

                MessageBox.Show("保存成功！");
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
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
