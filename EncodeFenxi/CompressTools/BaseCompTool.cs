using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Hanhua.CompressTools;

namespace Hanhua.Common
{
    /// <summary>
    /// 压缩文件共通处理
    /// </summary>
    public partial class BaseCompTool : BaseForm
    {
        /// <summary>
        /// 压缩文件处理类
        /// </summary>
        private BaseComp compObj;

        /// <summary>
        /// 构造方法
        /// </summary>
        public BaseCompTool(BaseComp compObj)
        {
            this.InitializeComponent();

            this.compObj = compObj;
            this.ResetHeight();

            // 设置Title
            this.Text = this.compObj.GetTitle() + "  " + this.Text;
        }

        #region " 页面事件 "

        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDec_Click(object sender, EventArgs e)
        {
            // 打开要分析的文件
            string compFileSuffix = this.compObj.GetCompFileSuffix();
            this.baseFile = Util.SetOpenDailog("选择压缩文件（*" + compFileSuffix + "）|*" + compFileSuffix + "|所有文件|*", string.Empty);
            if (string.IsNullOrEmpty(this.baseFile))
            {
                return;
            }

            byte[] byData = this.compObj.Decompress(this.baseFile);
            if (byData == null)
            {
                return;
            }

            // 保存解压缩文件
            string decompFileSuffix = this.compObj.GetDecomFileSuffix();
            this.baseFile = Util.SetSaveDailog("保存解压缩文件（*" + decompFileSuffix + "）|*" + decompFileSuffix + "|所有文件|*", string.Empty);
            if (string.IsNullOrEmpty(this.baseFile))
            {
                return;
            }

            File.WriteAllBytes(this.baseFile, byData);
        }

        /// <summary>
        /// 开始压缩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCom_Click(object sender, EventArgs e)
        {
            // 打开要分析的文件
            string decompFileSuffix = this.compObj.GetDecomFileSuffix();
            this.baseFile = Util.SetOpenDailog("选择需要压缩的文件（*" + decompFileSuffix + "）|*" + decompFileSuffix + "|所有文件|*", string.Empty);
            if (string.IsNullOrEmpty(this.baseFile))
            {
                return;
            }

            byte[] byData = this.compObj.Compress(this.baseFile);
            if (byData == null)
            {
                return;
            }

            // 保存压缩文件
            string compFileSuffix = this.compObj.GetCompFileSuffix();
            this.baseFile = Util.SetSaveDailog("保存压缩后文件（*" + compFileSuffix + "）|*" + compFileSuffix + "|所有文件|*", string.Empty);
            if (string.IsNullOrEmpty(this.baseFile))
            {
                return;
            }

            File.WriteAllBytes(this.baseFile, byData);
        }

        /// <summary>
        /// 解压缩目录所有文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAllDec_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.SelectedPath = this.compObj.GetDefaultPath();
            folderDlg.Description = "打开需要解压缩的路径";
            DialogResult dr = folderDlg.ShowDialog();
            this.baseFolder = folderDlg.SelectedPath;

            if (dr == DialogResult.Cancel || string.IsNullOrEmpty(this.baseFolder))
            {
                return;
            }

            base.Do(this.DecompressAll);
        }

        /// <summary>
        /// 压缩指定目录下所有文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAllCom_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.SelectedPath = this.compObj.GetDefaultPath();
            folderDlg.Description = "打开需要压缩的路径";
            DialogResult dr = folderDlg.ShowDialog();
            this.baseFolder = folderDlg.SelectedPath;

            if (dr == DialogResult.Cancel || string.IsNullOrEmpty(this.baseFolder))
            {
                return;
            }

            base.Do(this.CompressAll);
        }

        #endregion

        #region " 私有方法 "

        /// <summary>
        /// 解压缩目录所有文件
        /// </summary>
        private void DecompressAll()
        {
            List<FilePosInfo> fileNameInfo = Util.GetAllFiles(this.baseFolder).Where(
                    p => !p.IsFolder && p.File.EndsWith(this.compObj.GetCompFileSuffix(), StringComparison.OrdinalIgnoreCase)).ToList();

            // 显示进度条
            this.ResetProcessBar(fileNameInfo.Count);

            foreach (FilePosInfo file in fileNameInfo)
            {
                // 读入文件内容
                byte[] byDecData = this.compObj.Decompress(file.File);

                // 生成解压缩的文件
                File.WriteAllBytes(file.File + this.compObj.GetDecomFileSuffix(), byDecData);

                // 更新进度条
                this.ProcessBarStep();
            }

            // 隐藏进度条
            this.CloseProcessBar();

            MessageBox.Show("批量解压缩完成");
        }

        /// <summary>
        /// 压缩目录所有文件
        /// </summary>
        private void CompressAll()
        {
            string decompFileSuffix = this.compObj.GetDecomFileSuffix();
            List<FilePosInfo> fileNameInfo = Util.GetAllFiles(this.baseFolder).Where(p => !p.IsFolder && p.File.EndsWith(decompFileSuffix)).ToList();

            // 显示进度条
            this.ResetProcessBar(fileNameInfo.Count);

            foreach (FilePosInfo file in fileNameInfo)
            {
                // 读入文件内容
                byte[] byComressData = this.compObj.Compress(file.File);

                File.WriteAllBytes(file.File.Replace(decompFileSuffix, string.Empty), byComressData);

                // 更新进度条
                this.ProcessBarStep();
            }

            // 隐藏进度条
            this.CloseProcessBar();

            MessageBox.Show("批量压缩完成");
        }

        #endregion
    }
}
