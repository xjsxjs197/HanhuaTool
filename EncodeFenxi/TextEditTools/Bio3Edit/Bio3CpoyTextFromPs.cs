using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hanhua.Common.Bio1Edit;
using System.IO;

namespace Hanhua.Common.Bio3Edit
{
    /// <summary>
    /// 从ps版Copy文本到Ngc版本中
    /// </summary>
    public partial class Bio3CpoyTextFromPs : BaseForm
    {
        private Dictionary<string, int> fileCheckInfo = new Dictionary<string, int>();
        private int maxFindLen = 0;
        private int ngcTxtPos = 0;

        /// <summary>
        /// 记录有多少个需要Copy的Bin目录中的文件数
        /// </summary>
        private int comBinFiles = 0;

        /// <summary>
        /// 初始化
        /// </summary>
        public Bio3CpoyTextFromPs()
        {
            InitializeComponent();

            this.ResetHeight();

            // 初始化目录
            //this.txtJpFolder.Text = @"E:\Study\Hanhua\待漢化\Bio3\PsBio3Jp\CD_DATA";
            //this.txtCnFolder.Text = @"E:\Study\Hanhua\待漢化\Bio3\PsBio3Cn\CD_DATA";
            //this.txtNgcFolder.Text = @"E:\Study\Hanhua\待漢化\Bio3\NgcBio3";
            this.txtJpFolder.Text = @"D:\game\iso\wii\生化危机3汉化\ps_Bio3\JP\CD_DATA";
            this.txtCnFolder.Text = @"D:\game\iso\wii\生化危机3汉化\ps_Bio3\CN\CD_DATA";
            this.txtNgcFolder.Text = @"D:\game\iso\wii\生化危机3汉化\生化危机补丁工具\NgcBio3";

            this.btnJpSelect.Click += new EventHandler(this.btnSelect_Click);
            this.btnCnSelect.Click += new EventHandler(this.btnSelect_Click);
            this.btnNgcSelect.Click += new EventHandler(this.btnSelect_Click);
        }

        /// <summary>
        /// 选择目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            // 打开目录
            this.baseFolder = Util.OpenFolder(string.Empty);
            if (string.IsNullOrEmpty(this.baseFolder))
            {
                return;
            }

            if (sender == this.btnJpSelect)
            {
                this.txtJpFolder.Text = this.baseFolder;
            }
            else if (sender == this.btnCnSelect)
            {
                this.txtCnFolder.Text = this.baseFolder;
            }
            else if (sender == this.btnNgcSelect)
            {
                this.txtNgcFolder.Text = this.baseFolder;
            }
        }

        /// <summary>
        /// 开始复制文本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (!this.CheckFath(this.txtJpFolder)
                || !this.CheckFath(this.txtCnFolder)
                || !this.CheckFath(this.txtNgcFolder))
            {
                return;
            }

            // 根据配置文件，取得所有需要Copy的文件
            List<FilePosInfo> needCopyFiles = this.GetNeedCopyFiles(this.txtNgcFolder.Text, false);
            List<FilePosInfo> needCopyBinFiles = this.GetNeedCopyFiles(this.txtNgcFolder.Text, true);
            if (needCopyFiles.Count == 0 || needCopyBinFiles.Count == 0)
            {
                MessageBox.Show("路径错误，没有找到需要Copy的文件！");
                return;
            }

            StringBuilder notExistFiles = new StringBuilder();
            StringBuilder saveFaileFiles = new StringBuilder();
            this.fileCheckInfo.Clear();

            // 显示进度条
            this.ResetProcessBar(needCopyFiles.Count + needCopyBinFiles.Count);

            try
            {
                // 开始循环所有的日文bin文件
                for (int i = 0; i < needCopyBinFiles.Count; i++)
                {
                    FilePosInfo fileInfo = needCopyBinFiles[i];

                    // 取得各个文件名
                    string fileName = @"\" + fileInfo.File.Replace("_1", string.Empty).Replace("_2", string.Empty).Replace("_3", string.Empty).Replace("_4", string.Empty).Replace("_5", string.Empty);
                    string jpFile = this.txtJpFolder.Text + @"\BIN" + fileName + ".bin";
                    string cnFile = this.txtCnFolder.Text + @"\BIN" + fileName + ".bin";
                    string ngcFile = this.txtNgcFolder.Text + @"\root\&&systemdata\start_cn.dol";

                    if (File.Exists(jpFile)
                        && File.Exists(cnFile))
                    {
                        // 取得文本数据
                        int startPos = Convert.ToInt32(fileInfo.PosInfo[0], 16);
                        int endPos = Convert.ToInt32(fileInfo.PosInfo[1], 16);
                        byte[] byJpData = new byte[endPos - startPos];
                        byte[] byCnData = new byte[byJpData.Length];
                        this.GetTextData(jpFile, cnFile, fileInfo.PosInfo, byJpData, byCnData);

                        // 保存文本数据
                        this.SaveTextData(ngcFile, byJpData, byCnData, notExistFiles, saveFaileFiles, fileInfo.File);
                    }

                    // 更新进度条
                    this.ProcessBarStep();
                }

                // 开始循环所有的日文rdt文件
                //for (int i = 1; i <= 7; i++)
                //{
                //    List<FilePosInfo> copyFiles = needCopyFiles.Where(p => p.File.IndexOf("r" + i) != -1).ToList();
                //    foreach (FilePosInfo fileInfo in copyFiles)
                //    {
                //        // 取得各个文件名
                //        string fileName = @"\" + fileInfo.File.Replace("_1", string.Empty).Replace("_2", string.Empty);
                //        string jpFile = this.txtJpFolder.Text + @"\STAGE" + i + fileName + ".ard";
                //        string cnFile = this.txtCnFolder.Text + @"\STAGE" + i + fileName + ".ard";
                //        string ngcFile1 = this.txtNgcFolder.Text + @"\root\bio19\data_j\rdt" + fileName + ".rdt";
                //        string ngcFile2 = this.txtNgcFolder.Text + @"\root\bio19\data_aj\rdt" + fileName + ".rdt";

                //        if (File.Exists(jpFile)
                //            && File.Exists(cnFile))
                //        {
                //            // 取得文本数据
                //            int startPos = Convert.ToInt32(fileInfo.PosInfo[0], 16);
                //            int endPos = Convert.ToInt32(fileInfo.PosInfo[1], 16);
                //            byte[] byJpData = new byte[endPos - startPos];
                //            byte[] byCnData = new byte[byJpData.Length];
                //            this.GetTextData(jpFile, cnFile, fileInfo.PosInfo, byJpData, byCnData);

                //            // 保存文本数据
                //            this.SaveTextData(ngcFile1, byJpData, byCnData, notExistFiles, saveFaileFiles, fileInfo.File);
                //            this.SaveTextData(ngcFile2, byJpData, byCnData, notExistFiles, saveFaileFiles, fileInfo.File);
                //        }

                //        // 更新进度条
                //        this.ProcessBarStep();
                //    }
                //}

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

            // 显示结果
            if (notExistFiles.Length == 0 && saveFaileFiles.Length == 0)
            {
                MessageBox.Show("完全Copy成功！");
            }
            else
            {
                MessageBox.Show("不存在的文件：\n" + notExistFiles.ToString() + "\n" + "错误的文件：\n" + saveFaileFiles.ToString());
            }
        }

        #region " 私有方法 "

        /// <summary>
        /// 保存文本数据
        /// </summary>
        /// <param name="ngcFile"></param>
        /// <param name="byJpData"></param>
        /// <param name="byCnData"></param>
        /// <param name="notExistFiles"></param>
        /// <param name="saveFaileFiles"></param>
        private void SaveTextData(string ngcFile, byte[] byJpData, byte[] byCnData, StringBuilder notExistFiles, StringBuilder saveFaileFiles, string sortName)
        {
            if (File.Exists(ngcFile))
            {
                this.baseFile = ngcFile;
                if (!this.SaveTextData(ngcFile, byJpData, byCnData))
                {
                    saveFaileFiles.Append(ngcFile + " : " + sortName).Append("\n");

                    //string namePre = string.Empty;
                    //if (sortName.EndsWith("_1")
                    //    || sortName.EndsWith("_2"))
                    //{
                    //    namePre = " " + sortName.Substring(sortName.Length - 2);
                    //}
                    this.fileCheckInfo.Add(ngcFile + " : " + sortName, this.maxFindLen);
                }
            }
            else
            {
                notExistFiles.Append(ngcFile + " : " + sortName).Append("\n");
            }
        }

        /// <summary>
        /// 保存文本数据
        /// </summary>
        /// <param name="ngcFile"></param>
        /// <param name="byJpData"></param>
        /// <param name="byCnData"></param>
        /// <returns></returns>
        private bool SaveTextData(string ngcFile, byte[] byJpData, byte[] byCnData)
        {
            FileStream fs = null;

            try
            {
                // 取得Ngc数据
                fs = new FileStream(ngcFile, FileMode.Open);
                byte[] byNgcData = new byte[fs.Length];
                fs.Read(byNgcData, 0, byNgcData.Length);
                fs.Close();

                // 根据Ps日文文本数据，查找Ngc中的文本数据
                int txtStartPos = this.GetTextStartPos(byNgcData, byJpData);
                if (txtStartPos > -1)
                {
                    // 将中文数据写入Ngc数据
                    Array.Copy(byCnData, 0, byNgcData, txtStartPos, byCnData.Length);

                    // 保存中文数据
                    File.WriteAllBytes(ngcFile, byNgcData);
                    //string cnNgcFile = ngcFile.Replace("NgcBio3", "NgcBio3Cn");
                    //if (!File.Exists(cnNgcFile))
                    //{
                    //    string directory = Path.GetDirectoryName(cnNgcFile);
                    //    if (!Directory.Exists(directory))
                    //    {
                    //        Directory.CreateDirectory(directory);
                    //    }
                    //}

                    //File.Copy(ngcFile, cnNgcFile, true);
                    //File.WriteAllBytes(cnNgcFile, byNgcData);
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }

            return true;
        }

        /// <summary>
        /// 根据Ps文本数据，查找Ngc中的文本数据
        /// </summary>
        /// <param name="byData"></param>
        /// <param name="byJpData"></param>
        /// <returns></returns>
        private int GetTextStartPos(byte[] byData, byte[] byJpData)
        {
            // 二进制检索
            bool findedKey = true;
            int maxLen = byData.Length - byJpData.Length;
            this.maxFindLen = 0;
            this.ngcTxtPos = 0;

            for (int j = 0; j < maxLen; j++)
            {
                if (byData[j] == byJpData[0])
                {
                    findedKey = true;
                    for (int i = 1; i < byJpData.Length; i++)
                    {
                        if (byData[j + i] != byJpData[i])
                        {
                            findedKey = false;
                            this.maxFindLen = Math.Max(this.maxFindLen, i);
                            this.ngcTxtPos = j;
                            break;
                        }
                    }

                    if (findedKey)
                    {
                        return j;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// 取得文本数据
        /// </summary>
        /// <param name="jpFile"></param>
        /// <param name="cnFile"></param>
        /// <param name="addrInfo"></param>
        /// <param name="byJpData"></param>
        /// <param name="byCnData"></param>
        private bool GetTextData(string jpFile, string cnFile, string[] addrInfo, byte[] byJpData, byte[] byCnData)
        {
            FileStream fs = null;

            try
            {
                int startPos = Convert.ToInt32(addrInfo[0], 16);
                int endPos = Convert.ToInt32(addrInfo[1], 16);

                fs = new FileStream(jpFile, FileMode.Open);
                fs.Seek(startPos, SeekOrigin.Begin);
                fs.Read(byJpData, 0, byJpData.Length);
                fs.Close();

                fs = new FileStream(cnFile, FileMode.Open);
                fs.Seek(startPos, SeekOrigin.Begin);
                fs.Read(byCnData, 0, byCnData.Length);
                fs.Close();
            }
            catch
            {
                return false;
            }
            finally
            { 
                if (fs != null)
                {
                    fs.Close();
                }
            }

            return true;
        }

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
        /// 根据配置文件，取得所有需要Copy的文件
        /// </summary>
        /// <param name="ngcFolder"></param>
        /// <returns></returns>
        private List<FilePosInfo> GetNeedCopyFiles(string ngcFolder, bool isComBinFile)
        {
            List<FilePosInfo> needCopyFiles = new List<FilePosInfo>();
            string configFile;

            if (isComBinFile)
            {
                configFile = ngcFolder + @"\BinAddr.txt";
            }
            else
            {
                configFile = ngcFolder + @"\TextAddr.txt";
            }

            if (File.Exists(configFile))
            {
                string[] files = File.ReadAllLines(configFile);
                for (int i = 0; i < files.Length; i += 2)
                {
                    FilePosInfo fileInfo = new FilePosInfo(files[i], files[i + 1].Split(' '));
                    needCopyFiles.Add(fileInfo);
                }
            }

            return needCopyFiles;
        }

        #endregion
    }
}
