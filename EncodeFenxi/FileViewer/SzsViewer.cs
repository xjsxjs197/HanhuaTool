using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Hanhua.Common;
using Hanhua.FontEditTools;
using Hanhua.ImgEditTools;

namespace Hanhua.FileViewer
{
    /// <summary>
    /// 显示SZS文件中的信息
    /// </summary>
    public partial class SzsViewer : BaseForm
    {
        #region " 私有变量 "

        /// <summary>
        /// 右键菜单
        /// </summary>
        private ContextMenuStrip contextMenu = new ContextMenuStrip();

        /// <summary>
        /// 选择的节点
        /// </summary>
        private RarcNode selectedNode;

        /// <summary>
        /// 保存旧的数据
        /// </summary>
        private byte[] byOldData;

        /// <summary>
        /// 当前看的节点数据的位置
        /// </summary>
        private int viewDataStartPos;

        /// <summary>
        /// 文件名称
        /// </summary>
        private string oldFileName;

        /// <summary>
        /// 文本查看TextBox
        /// </summary>
        private RichTextBox txtFileInfo = new RichTextBox();

        /// <summary>
        /// 图片查看工具
        /// </summary>
        private UcImageEditor ucImageViewer = new UcImageEditor();

        /// <summary>
        /// 显示图片的控件
        /// </summary>
        private PictureBox imgViewer = new PictureBox();

        /// <summary>
        /// 保存的类型 true : 使用节点内的文件名保存， false ：使用传进来的文件名保存
        /// </summary>
        private bool useNodeFileSave;

        /// <summary>
        /// 选择节点的文件名
        /// </summary>
        private string nodeFileName;

        #endregion

        #region " 构造方法 "

        /// <summary>
        /// 初始化
        /// </summary>
        public SzsViewer(TreeNode root, byte[] byData, string fileName)
        {
            InitializeComponent();

            this.ResetHeight();

            this.byOldData = byData;
            this.oldFileName = fileName;
            this.useNodeFileSave = string.IsNullOrEmpty(fileName);

            // 显示文件信息
            this.SetTreeNode(root);

            // 设置右键菜单
            this.SetContextMenu();
        }

        #endregion

        #region " 页面事件 "

        /// <summary>
        /// 弹出右键菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileInfoTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // 只有右键弹出菜单
            if (e.Button == MouseButtons.Right)
            {
                if (e.Node.Tag != null)
                {
                    // 只有存在数据才弹出菜单
                    RarcNode fileInfo = (RarcNode)e.Node.Tag;
                    if (fileInfo == null || fileInfo.FileData == null || fileInfo.FileData.Length == 0)
                    {
                        return;
                    }

                    selectedNode = fileInfo;
                    Point p = Control.MousePosition;
                    this.contextMenu.Show(p);
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
                // 导出
                case "export":
                    this.ExportSelectedNode();
                    break;

                // 另存
                case "saveAs":
                    this.SaveAs();
                    break;
            }
        }

        /// <summary>
        /// 选中变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileInfoTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // 显示节点信息
            this.ShowTreeNodeInfo(e.Node);
        }

        #endregion

        #region " 私有方法 "

        /// <summary>
        /// 显示文件信息
        /// </summary>
        private void SetTreeNode(TreeNode root)
        {
            this.fileInfoTree.Nodes.Clear();
            this.fileInfoTree.Nodes.Add(root);
            this.fileInfoTree.ExpandAll();
        }

        /// <summary>
        /// 替换Tpl数据
        /// </summary>
        /// <param name="newTplData"></param>
        /// <param name="startPos"></param>
        private void SaveTpl(byte[] newTplData)
        {
            if (this.useNodeFileSave)
            {
                // 如果使用节点的文件保存，需要先读取节点的数据
                FileStream fs = new FileStream(this.nodeFileName, FileMode.Open);
                this.byOldData = new byte[fs.Length];
                fs.Read(this.byOldData, 0, this.byOldData.Length);
                fs.Close();
                fs = null;
            }

            Array.Copy(newTplData, 0, this.byOldData, this.viewDataStartPos, newTplData.Length);
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
            item.Name = "export";
            item.Text = "导出";
            this.contextMenu.Items.Add(item);

            item = new ToolStripMenuItem();
            item.Name = "import";
            item.Text = "导入";
            item.Enabled = false;
            this.contextMenu.Items.Add(item);

            item = new ToolStripMenuItem();
            item.Name = "saveAs";
            item.Text = "另存所有";
            this.contextMenu.Items.Add(item);
        }

        /// <summary>
        /// 另存数据
        /// </summary>
        private void SaveAs()
        {
            if (this.useNodeFileSave)
            {
                // 如果使用节点的文件保存，需要重新设置保存的路径
                this.oldFileName = this.nodeFileName;
            }

            // 解析文件名
            int indexPoint = this.oldFileName.IndexOf(".");
            string fileType = this.oldFileName.Substring(indexPoint + 1);

            // 打开要分析的文件
            this.baseFile = Util.SetSaveDailog(fileType + "类型数据(*." + fileType + ")|*." + fileType + "|所有类型(*.*)|*.*", string.Empty);
            if (string.IsNullOrEmpty(this.baseFile))
            {
                return;
            }

            try
            {
                // 开始保存文件
                File.WriteAllBytes(this.baseFile, this.byOldData);
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
            }
        }

        /// <summary>
        /// 导出选择节点的数据
        /// </summary>
        private void ExportSelectedNode()
        {
            this.baseFile = Util.SetSaveDailog(this.selectedNode.FileType + "类型数据(*." + this.selectedNode.FileType + ")|*." + this.selectedNode.FileType +
                "|所有类型(*.*)|*.*", string.Empty);
            if (string.IsNullOrEmpty(this.baseFile))
            {
                return;
            }

            try
            {
                // 开始保存文件
                File.WriteAllBytes(this.baseFile, this.selectedNode.FileData);
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
            }
        }

        /// <summary>
        /// 显示节点信息
        /// </summary>
        private void ShowTreeNodeInfo(TreeNode treeNode)
        {
            if (treeNode.Tag != null)
            {
                // 解析双击的文件
                RarcNode fileInfo = (RarcNode)treeNode.Tag;
                if (string.IsNullOrEmpty(fileInfo.FileType) || fileInfo == null || fileInfo.FileData == null || fileInfo.FileData.Length == 0)
                {
                    return;
                }

                try
                {
                    switch (fileInfo.FileType.ToLower())
                    {
                        case "bti":
                            this.imgViewer.Image = Util.BtiDecode(fileInfo.FileData);
                            this.ShowPanelRight(this.imgViewer);
                            break;

                        case "bmg":
                            //this.txtFileInfo.Text = Util.DecodeByteArray(fileInfo.FileData, Encoding.GetEncoding(932).GetDecoder());
                            this.txtFileInfo.Text = Util.BmgDecode(fileInfo.FileData);
                            this.ShowPanelRight(this.txtFileInfo);
                            break;

                        case "bfn":
                            NgcFontEditer ngcFontView = new NgcFontEditer(fileInfo.FileData);
                            ngcFontView.Show();
                            break;

                        case "brfnt":
                            WiiFontEditer wiiFontView = new WiiFontEditer();
                            wiiFontView.Show();
                            this.Do(wiiFontView.ViewFontInfo, fileInfo.FileData);
                            break;

                        case "tpl":
                            List<ImageHeader> tplImageInfo = new List<ImageHeader>();
                            Image[] tplImages = new TplFileManager().TplDecode(fileInfo.FileData, tplImageInfo);
                            this.viewDataStartPos = fileInfo.DataStartPos;
                            this.nodeFileName = fileInfo.FileName;

                            this.ucImageViewer.strFileOpen = fileInfo.FileName;
                            this.ucImageViewer.OutSaveFunc += this.SaveTpl;
                            this.ucImageViewer.OutSetText += this.SetTplInfo;
                            this.ucImageViewer.ViewImage(tplImages, tplImageInfo, fileInfo.FileData);

                            this.ShowPanelRight(this.ucImageViewer);
                            break;

                        default:
                            //MessageBox.Show("不支持的格式 ： " + fileInfo.FileType);
                            break;
                    }
                }
                catch (Exception me)
                {
                    MessageBox.Show(me.Message + "\n" + me.StackTrace);
                }
            }
        }

        /// <summary>
        /// 设置Tpl图片信息
        /// </summary>
        /// <param name="imgInfo"></param>
        private void SetTplInfo(string imgInfo)
        {
            this.Text = imgInfo;
        }

        /// <summary>
        /// 显示右侧区域
        /// </summary>
        /// <param name="viewControl"></param>
        private void ShowPanelRight(Control viewControl)
        {
            if (this.pnlRight.Controls.Count > 0 && this.pnlRight.Controls[0] == viewControl)
            {
                // 如果已经加载过，不用再次加载
                return;
            }

            this.pnlRight.Controls.Clear();
            this.pnlRight.Controls.Add(viewControl);
            viewControl.Dock = DockStyle.Fill;
            this.pnlRight.Visible = true;
        }

        #endregion
    }
}
