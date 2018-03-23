using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Hanhua.Common.Bio0Edit
{
    /// <summary>
    /// 生化0Lz工具
    /// </summary>
    public partial class Bio0LzTool : BaseForm
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public Bio0LzTool()
        {
            this.InitializeComponent();

            this.ResetHeight();
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
            this.baseFile = Util.SetOpenDailog("生化危机0 Lz压缩文件（*.*）|*.*", string.Empty);
            if (string.IsNullOrEmpty(this.baseFile))
            {
                return;
            }

            byte[] byLzData = this.Bio0LzDeCompress(this.baseFile);
            if (byLzData == null)
            {
                return;
            }

            // 保存解压缩文件
            this.baseFile = Util.SetSaveDailog("生化危机0 Lz解压缩文件（*.*）|*.*", string.Empty);
            if (string.IsNullOrEmpty(this.baseFile))
            {
                return;
            }

            File.WriteAllBytes(this.baseFile, byLzData);
        }

        /// <summary>
        /// 开始压缩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCom_Click(object sender, EventArgs e)
        {
            // 打开要分析的文件
            this.baseFile = Util.SetOpenDailog("生化危机0 Lz压缩文件（*.*）|*.*", string.Empty);
            if (string.IsNullOrEmpty(this.baseFile))
            {
                return;
            }

            int oldLen = 0;
            if (File.Exists(this.baseFile.Replace(".bio0dec_cn", string.Empty)))
            {
                oldLen = File.ReadAllBytes(this.baseFile.Replace(".bio0dec_cn", string.Empty)).Length;
            }

            byte[] byLzData = Util.LzCompress(this.baseFile, oldLen);
            if (byLzData == null)
            {
                return;
            }

            // 保存压缩文件
            this.baseFile = Util.SetSaveDailog("生化危机0 Lz压缩文件（*.*）|*.*", string.Empty);
            if (string.IsNullOrEmpty(this.baseFile))
            {
                return;
            }

            File.WriteAllBytes(this.baseFile, byLzData);
        }

        /// <summary>
        /// 生成Message文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateMes_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 解压缩目录所有文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAllDec_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            //folderDlg.SelectedPath = @"E:\My\Hanhua\testFile\Biohazard_0\Cn\files\bio0\scene\";
            folderDlg.SelectedPath = @"D:\game\iso\wii\生化危机0汉化\汉化\files\bio0\demo";
            folderDlg.Description = "打开需要解压缩的路径";
            DialogResult dr = folderDlg.ShowDialog();
            string strFolder = folderDlg.SelectedPath;

            if (dr == DialogResult.Cancel || string.IsNullOrEmpty(strFolder))
            {
                return;
            }

            try
            {
                List<FilePosInfo> fileNameInfo = Util.GetAllFiles(strFolder).Where(p => !p.IsFolder).ToList();
                
                // 显示进度条
                this.ResetProcessBar(fileNameInfo.Count);

                foreach (FilePosInfo file in fileNameInfo)
                {
                    if (!file.File.EndsWith(".alz"))
                    {
                        continue;
                    }

                    // 读入文件内容
                    //byte[] byDecData = this.Bio0LzDeCompress(file.FileName);

                    //File.WriteAllBytes(file.FileName + ".bio0dec", byDecData);

                    if (File.Exists(file.File + ".bio0dec"))
                    {
                        File.Copy(file.File + ".bio0dec", file.File + ".bio0dec_cn", true);
                    }

                    // 更新进度条
                    this.ProcessBarStep();
                }

                // 隐藏进度条
                this.CloseProcessBar();

                MessageBox.Show("解压缩目录完成");
            }
            catch (Exception me)
            {
                // 隐藏进度条
                this.CloseProcessBar();

                MessageBox.Show(me.Message + "\n" + me.StackTrace);
            }
        }

        /// <summary>
        /// 压缩指定目录下所有问题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAllCom_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            //folderDlg.SelectedPath = @"E:\My\Hanhua\testFile\Biohazard_0\Cn\files\bio0\scene\";
            folderDlg.SelectedPath = @"D:\game\iso\wii\生化危机0汉化\汉化\files\bio0\demo";
            folderDlg.Description = "打开需要压缩的路径";
            DialogResult dr = folderDlg.ShowDialog();
            string strFolder = folderDlg.SelectedPath;

            if (dr == DialogResult.Cancel || string.IsNullOrEmpty(strFolder))
            {
                return;
            }

            try
            {
                List<FilePosInfo> fileNameInfo = Util.GetAllFiles(strFolder).Where(p => !p.IsFolder && p.File.EndsWith("bio0dec_cn")).ToList();

                // 显示进度条
                this.ResetProcessBar(fileNameInfo.Count);

                foreach (FilePosInfo file in fileNameInfo)
                {
                    // 读入文件内容
                    byte[] byComressData = Util.LzCompress(file.File, 0);

                    File.WriteAllBytes(file.File.Replace(".bio0dec_cn", string.Empty), byComressData);

                    // 更新进度条
                    this.ProcessBarStep();
                }

                // 隐藏进度条
                this.CloseProcessBar();

                MessageBox.Show("压缩目录完成");
            }
            catch (Exception me)
            {
                // 隐藏进度条
                this.CloseProcessBar();

                MessageBox.Show(me.Message + "\n" + me.StackTrace);
            }
        }

        #endregion

        #region " 私有方法 "

        /// <summary>
        /// 解压缩文件
        /// </summary>
        /// <param name="bio0LzFile"></param>
        /// <returns></returns>
        private byte[] Bio0LzDeCompress(string bio0LzFile)
        {
            FileStream fs = null;
            try
            {
                // 将文件中的数据，读取到byData中
                fs = new FileStream(bio0LzFile, FileMode.Open);
                byte[] alz = new byte[fs.Length];
                fs.Read(alz, 0, alz.Length);
                fs.Close();
                fs = null;

                return Util.LzDeCompress(alz);
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
                return null;
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
        /// 判断压缩的数据是否正确
        /// </summary>
        /// <param name="oldData"></param>
        /// <param name="compressData"></param>
        /// <returns></returns>
        private bool IsCompressedDataRight(byte[] oldData, byte[] compressData)
        {
            byte[] deCompressData = Util.LzDeCompress(compressData);
            if (deCompressData != null && deCompressData.Length == oldData.Length)
            {
                int i = 0;
                while (i < deCompressData.Length)
                {
                    if (deCompressData[i] != oldData[i])
                    {
                        MessageBox.Show("压缩的数据不正确，总长度：" + compressData.Length + "， 不同的位置：" + i);
                        return false;
                    }
                    i++;
                }

                return true;
            }

            MessageBox.Show("压缩的数据不正确");
            return false;
        }

        #endregion
    }
}
