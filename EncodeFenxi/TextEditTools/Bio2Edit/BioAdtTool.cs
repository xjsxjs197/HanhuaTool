using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hanhua.Common;

namespace Hanhua.TextEditTools.BioAdt
{
    /// <summary>
    /// 生化危机Adt图片文件处理工具
    /// </summary>
    public partial class BioAdtTool : BaseForm
    {
        #region " 私有变量 "
        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        public BioAdtTool()
        {
            InitializeComponent();

            this.ResetHeight();

            this.txtAdtFolder.Text = @"E:\游戏汉化\NgcBio2\temp";
            this.txtTimFolder.Text = @"E:\游戏汉化\NgcBio2\temp";
        }

        #region " 页面事件 "

        /// <summary>
        /// 开始处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdtDec_Click(object sender, EventArgs e)
        {
            if (!this.CheckFath(this.txtAdtFolder)
                || !this.CheckFath(this.txtTimFolder))
            {
                return;
            }

            if (this.rdoDec.Checked)
            {
                this.DecompressAdt(this.txtAdtFolder.Text, this.txtTimFolder.Text);
            }
            else
            {
                this.CompressTim(this.txtAdtFolder.Text, this.txtTimFolder.Text);
            }
        }

        /// <summary>
        /// 选中Adt文件目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnJpSelect_Click(object sender, EventArgs e)
        {
            // 打开目录
            this.baseFolder = Util.OpenFolder(string.Empty);
            if (string.IsNullOrEmpty(this.baseFolder))
            {
                return;
            }

            this.txtAdtFolder.Text = this.baseFolder;
        }

        /// <summary>
        /// 选中Tim文件目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCnSelect_Click(object sender, EventArgs e)
        {
            // 打开目录
            this.baseFolder = Util.OpenFolder(@"E:\My\Hanhua\testFile\adt\tim");
            if (string.IsNullOrEmpty(this.baseFolder))
            {
                return;
            }

            this.txtTimFolder.Text = this.baseFolder;
        }

        #endregion

        #region " 私有方法 "

        /// <summary>
        /// 检查是否输入了目录
        /// </summary>
        /// <param name="txtBox"></param>
        /// <returns></returns>
        private bool CheckFath(TextBox txtBox)
        {
            if (string.IsNullOrEmpty(txtBox.Text))
            {
                MessageBox.Show("要选择目录！");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 解压缩所有Adt文件
        /// </summary>
        /// <param name="adtFilesPath"></param>
        /// <param name="timFilesPath"></param>
        private void DecompressAdt(string adtFilesPath, string timFilesPath)
        {
            try
            {
                List<FilePosInfo> fileNameInfo = Util.GetAllFiles(adtFilesPath).Where(p => p.File.EndsWith(".adt") && !p.IsFolder).ToList();
                
                // 显示进度条
                this.ResetProcessBar(fileNameInfo.Count);

                System.Diagnostics.Process exep = new System.Diagnostics.Process();
                exep.StartInfo.FileName = @".\AdtDec.exe";
                exep.StartInfo.CreateNoWindow = true;
                exep.StartInfo.UseShellExecute = false;

                // 循环解压
                foreach (FilePosInfo fileInfo in fileNameInfo)
                {
                    exep.StartInfo.Arguments = fileInfo.File + " " + (timFilesPath + @"\" + Util.GetShortName(fileInfo.File)).Replace(".adt", ".tim");
                    exep.Start();
                    exep.WaitForExit();

                    // 更新进度条
                    this.ProcessBarStep();
                }

                // 隐藏进度条
                this.CloseProcessBar();
            }
            catch (Exception me)
            {
                MessageBox.Show(this.baseFile + "\n" + me.Message);
            }
            finally
            {
                // 隐藏进度条
                this.CloseProcessBar();
            }
        }

        /// <summary>
        /// 压缩所有Tim文件
        /// </summary>
        /// <param name="adtFilesPath"></param>
        /// <param name="timFilesPath"></param>
        private void CompressTim(string adtFilesPath, string timFilesPath)
        {
            try
            {
                List<FilePosInfo> fileNameInfo = Util.GetAllFiles(timFilesPath).Where(p => p.File.EndsWith(".tim", StringComparison.OrdinalIgnoreCase) && !p.IsFolder).ToList();

                // 显示进度条
                this.ResetProcessBar(fileNameInfo.Count);

                System.Diagnostics.Process exep = new System.Diagnostics.Process();
                exep.StartInfo.FileName = @".\AdtCom.exe";
                exep.StartInfo.CreateNoWindow = true;
                exep.StartInfo.UseShellExecute = false;

                // 循环解压
                foreach (FilePosInfo fileInfo in fileNameInfo)
                {
                    exep.StartInfo.Arguments = "e " + fileInfo.File + " " + (adtFilesPath + @"\" + Util.GetShortName(fileInfo.File)).ToLower().Replace(".tim", ".adt");
                    exep.Start();
                    exep.WaitForExit();

                    // 更新进度条
                    this.ProcessBarStep();
                }

                // 隐藏进度条
                this.CloseProcessBar();
            }
            catch (Exception me)
            {
                MessageBox.Show(this.baseFile + "\n" + me.Message);
            }
            finally
            {
                // 隐藏进度条
                this.CloseProcessBar();
            }
        }

        #endregion
    }
}
