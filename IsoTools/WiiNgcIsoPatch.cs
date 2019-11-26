using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using Hanhua.Common;

namespace IsoTools
{
    /// <summary>
    /// Wii/Ngc Iso打补丁工具
    /// </summary>
    public partial class WiiNgcIsoPatch : BaseForm
    {
        /// <summary>
        /// 每次复制的字节数
        /// </summary>
        private const int COPY_BUFFER = 1024 * 80;

        /// <summary>
        /// 每次复制的字节数
        /// </summary>
        private const int COPY_BLOCK = 1024 * 1024 * 15;

        /// <summary>
        /// Ngc Raw Size
        /// </summary>
        private const int RAW_SIZE = 4;

        /// <summary>
        /// Ngc Iso容量
        /// </summary>
        private const int NGC_ISO_LEN = 1459978240;

        /// <summary>
        /// Wii临时目录
        /// </summary>
        private const string WII_ISO_TEMP = "WiiPatchTemp";

        /// <summary>
        /// 初始化
        /// </summary>
        public WiiNgcIsoPatch()
        {
            InitializeComponent();

            this.ResetHeight();
        }

        /// <summary>
        /// 选择日文Iso文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnJpSelect_Click(object sender, EventArgs e)
        {
            // 打开要分析的文件
            this.baseFile = Util.SetOpenDailog("日文 Iso 游戏文件（*.wbfs,*.iso,*.gcm,）|*.wbfs;*.iso;*.gcm", string.Empty);
            if (string.IsNullOrEmpty(this.baseFile))
            {
                return;
            }

            this.txtJpIso.Text = this.baseFile;
        }

        /// <summary>
        /// 选择中文补丁文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCnSelect_Click(object sender, EventArgs e)
        {
            // 打开目录
            this.baseFolder = Util.OpenFolder(string.Empty);
            if (string.IsNullOrEmpty(this.baseFolder))
            {
                return;
            }

            this.txtCnFolder.Text = this.baseFolder;
        }

        /// <summary>
        /// 开始打补丁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopy_Click(object sender, EventArgs e)
        {
            //int isSame = Util.isFilesSame(@"E:\Study\MySelfProject\Hanhua\TodoCn\Bio3\NgcBio3Jp\biohazard3_old.iso", @"E:\Study\MySelfProject\Hanhua\TodoCn\Bio3\NgcBio3Jp\biohazard3_old_CN.iso");
            //int a = 10;
            string isoFile = this.txtJpIso.Text;
            if (string.IsNullOrEmpty(isoFile))
            {
                MessageBox.Show("请选择需要打补丁的日文Iso文件！");
                this.txtJpIso.Focus();
                return;
            }

            if (string.IsNullOrEmpty(this.txtCnFolder.Text))
            {
                MessageBox.Show("请选择需要中文补丁的目录（解压过的）！");
                this.txtCnFolder.Focus();
                return;
            }

            if (this.IsWiiIso(isoFile))
            {
                this.Do(this.PatchWii, this.txtJpIso.Text.Trim(), this.txtCnFolder.Text.Trim());
            }
            else
            {
                this.Do(this.PatchNgc, this.txtJpIso.Text.Trim(), this.txtCnFolder.Text.Trim(), !this.chkSameSize.Checked);
            }
        }

        /// <summary>
        /// 取得当前文件的目录
        /// </summary>
        /// <param name="folderInfo"></param>
        /// <param name="fileIndex"></param>
        /// <returns></returns>
        private string GetCurrentFolder(List<KeyValuePair<string, int>> folderInfo, int fileIndex)
        {
            int folderIndex = folderInfo.Count - 1;
            while (folderIndex >= 0)
            {
                KeyValuePair<string, int> lastFolder = folderInfo[folderIndex];
                if (fileIndex < lastFolder.Value)
                {
                    return lastFolder.Key + @"\";
                }

                folderIndex--;
            }

            return @"root\";
        }

        /// <summary>
        /// 开始打包(重新组织每个文件大小，位置)
        /// </summary>
        private void PatchNgc(params object[] param)
        {
            string jpIsoFile = (string)param[0];
            string cnPatchFolder = (string)param[1];
            bool isReSize = (bool)param[2];

            // 改变标题，按钮状态
            string oldTitle = this.Text;
            this.SetWinStatus(false, this.Text + " 打补丁中，请稍等......");

            // 开始打补丁
            List<FilePosInfo> jpFiInfos = new List<FilePosInfo>();
            List<FilePosInfo> jpFiInfosForSort = new List<FilePosInfo>();
            List<KeyValuePair<string, int>> folderInfo = new List<KeyValuePair<string, int>>();
            BufferedStream jpFs = null;
            BufferedStream cnFs = null;
            FileStream jpReader = null;
            FileStream cnWriter = null;
            string cnIsoFile = string.Empty;
            // 设置第一个文件开始位置(默认最大,挨个比较取最小的)
            int firstFilePos = NGC_ISO_LEN;
            // 当前目录
            string currentFolder = "root";

            try
            {
                // 将文件中的数据，读取到byData中
                jpReader = File.OpenRead(jpIsoFile);
                jpFs = new BufferedStream(jpReader, COPY_BLOCK);

                // 读取Dol位置信息
                jpFs.Seek(0x420, SeekOrigin.Begin);
                byte[] byData = new byte[4];
                jpFs.Read(byData, 0, byData.Length);
                int dolOffset = Util.GetOffset(byData, 0, 3);

                // 读取FST位置信息
                jpFs.Seek(0x424, SeekOrigin.Begin);
                byData = new byte[4];
                jpFs.Read(byData, 0, byData.Length);
                int fstOffset = Util.GetOffset(byData, 0, 3);

                jpFs.Seek(0x4, SeekOrigin.Current);
                jpFs.Read(byData, 0, byData.Length);
                int fstSize = Util.GetOffset(byData, 0, 3);

                jpFs.Seek(0x4, SeekOrigin.Current);
                jpFs.Read(byData, 0, byData.Length);
                int maxFstSize = Util.GetOffset(byData, 0, 3);

                jpFs.Seek(0x4, SeekOrigin.Current);
                jpFs.Read(byData, 0, byData.Length);
                int userPos = Util.GetOffset(byData, 0, 3);

                jpFs.Seek(0x4, SeekOrigin.Current);
                jpFs.Read(byData, 0, byData.Length);
                int userLen = Util.GetOffset(byData, 0, 3);

                // 读取FST信息
                jpFs.Seek(fstOffset, SeekOrigin.Begin);
                byte[] fstData = new byte[fstSize];
                jpFs.Read(fstData, 0, fstData.Length);

                // 设置Dol信息
                FilePosInfo fileInfo = new FilePosInfo("Start.dol", false, fstOffset - dolOffset);
                fileInfo.FilePos = dolOffset;
                fileInfo.FileSize = fstOffset - dolOffset;
                fileInfo.TextStart = fileInfo.FilePos;
                fileInfo.TextEnd = fileInfo.FileSize;
                jpFiInfos.Add(fileInfo);
                jpFiInfosForSort.Add(fileInfo);

                // 循环读取文件信息
                int fstNum = Util.GetOffset(fstData, 0x8, 0xB);
                byte[] fileNameTab = new byte[fstSize - fstNum * 0xc];
                Array.Copy(fstData, fstNum * 0xc, fileNameTab, 0, fileNameTab.Length);
                int fileNameOffset = Util.GetOffset(fstData, 0x1, 0x3);

                String rootName = Util.GetFileNameFromStringTable(fileNameTab, fileNameOffset);
                FilePosInfo rootFile = new FilePosInfo("root", true, 0);
                rootFile.FileSize = fstNum;
                folderInfo.Add(new KeyValuePair<string, int>("root", fstNum));
                jpFiInfos.Add(rootFile);

                // 循环读取文件信息
                for (int i = 1; i < fstNum; i++)
                {
                    int fstTabOffset = i * 0xc;
                    fileNameOffset = Util.GetOffset(fstData, fstTabOffset + 0x1, fstTabOffset + 0x3);
                    String name = Util.GetFileNameFromStringTable(fileNameTab, fileNameOffset);
                    bool isFolder = fstData[fstTabOffset] == 1;
                    int filePos = Util.GetOffset(fstData, fstTabOffset + 0x4, fstTabOffset + 0x7);
                    int fileSize = Util.GetOffset(fstData, fstTabOffset + 0x8, fstTabOffset + 0xB);
                    fileInfo = new FilePosInfo(name, isFolder, fileSize);
                    fileInfo.FilePos = filePos;
                    fileInfo.FileSize = fileSize;
                    fileInfo.TextStart = filePos;
                    fileInfo.TextEnd = fileSize;


                    // 重新设置当前的目录及文件名
                    if (isFolder)
                    {
                        currentFolder = jpFiInfos[filePos + 1].File + @"\" + name;
                        fileInfo.File = currentFolder;
                        folderInfo.Add(new KeyValuePair<string, int>(currentFolder, fileSize));
                    }
                    else
                    {

                        fileInfo.File = this.GetCurrentFolder(folderInfo, i) + fileInfo.File;

                        if (filePos < firstFilePos)
                        {
                            firstFilePos = filePos;
                        }
                        jpFiInfosForSort.Add(fileInfo);
                    }

                    jpFiInfos.Add(fileInfo);
                }

                // 将地址信息List重新排序
                jpFiInfosForSort.Sort(this.PosInfoCompare);

                // 为了防止文件过大，超过容量，尽量减少firstFilePos
                if (isReSize)
                {
                    int startFilePos = Util.ResetPos(userPos + userLen, RAW_SIZE);
                    if (startFilePos < firstFilePos)
                    {
                        int diffSize = Util.ResetPos(startFilePos - firstFilePos, RAW_SIZE);
                        this.ResetAllPos(jpFiInfos, firstFilePos, diffSize);

                        firstFilePos = startFilePos;
                    }
                }

                // 读取所有补丁文件信息
                Util.IsGetAllFile = true;
                List<FilePosInfo> allCnFiles = Util.GetAllFiles(cnPatchFolder).Where(p => !p.IsFolder).ToList();
                Util.IsGetAllFile = false;

                // 重新生成中文Iso文件
                string isoShortName = Util.GetShortNameWithoutType(jpIsoFile);
                cnIsoFile = jpIsoFile.Replace(isoShortName + ".", isoShortName + "_CN.");

                if (File.Exists(cnIsoFile))
                {
                    File.Delete(cnIsoFile);
                }

                cnWriter = File.Open(cnIsoFile, FileMode.CreateNew);
                cnFs = new BufferedStream(cnWriter, COPY_BLOCK);
                cnFs.SetLength(NGC_ISO_LEN);

                // Copy第一个文件之前的数据信息
                jpFs.Seek(0, SeekOrigin.Begin);
                cnFs.Seek(0, SeekOrigin.Begin);
                this.FileStreamCopy(cnFs, jpFs, firstFilePos);

                // 显示进度条
                this.ResetProcessBar(jpFiInfos.Count);

                // 循环所有的原始文件，替换成相应中文文件，写入中文Iso
                int copyFile = 0;
                if (isReSize)
                {
                    copyFile = WriteNgcTypeA(jpFiInfosForSort, jpFiInfos, allCnFiles, cnFs, jpFs);
                }
                else
                {
                    copyFile = WriteNgcTypeB(jpFiInfosForSort, jpFiInfos, allCnFiles, cnFs, jpFs);
                }

                // 写入文件的位置信息
                // 只有在改变了单个文件大小，位置的情况下才写入
                if (isReSize)
                {
                    for (int i = 1; i < fstNum; i++)
                    {
                        int fstTabOffset = i * 0xc;
                        FilePosInfo filePosInfo = jpFiInfos[i + 1];

                        fstData[fstTabOffset + 0x4] = (byte)((filePosInfo.TextStart >> 24) & 0xFF);
                        fstData[fstTabOffset + 0x5] = (byte)((filePosInfo.TextStart >> 16) & 0xFF);
                        fstData[fstTabOffset + 0x6] = (byte)((filePosInfo.TextStart >> 8) & 0xFF);
                        fstData[fstTabOffset + 0x7] = (byte)(filePosInfo.TextStart & 0xFF);

                        fstData[fstTabOffset + 0x8] = (byte)((filePosInfo.TextEnd >> 24) & 0xFF);
                        fstData[fstTabOffset + 0x9] = (byte)((filePosInfo.TextEnd >> 16) & 0xFF);
                        fstData[fstTabOffset + 0xA] = (byte)((filePosInfo.TextEnd >> 8) & 0xFF);
                        fstData[fstTabOffset + 0xB] = (byte)(filePosInfo.TextEnd & 0xFF);
                    }
                    cnFs.Seek(fstOffset, SeekOrigin.Begin);
                    cnFs.Write(fstData, 0, fstData.Length);
                }

                // 隐藏进度条
                this.CloseProcessBar();

                MessageBox.Show("打汉化补丁完成！\n一共成功导入了：" + copyFile + " 个文件");
            }
            finally
            {
                if (jpFs != null)
                {
                    jpFs.Close();
                }

                if (cnFs != null)
                {
                    cnFs.Close();
                }

                if (jpReader != null)
                {
                    jpReader.Close();
                }

                if (cnWriter != null)
                {
                    cnWriter.Close();
                }

                // 恢复按钮状态
                this.SetWinStatus(true, oldTitle);
            }
        }

        /// <summary>
        /// 写入Ngc补丁（文件大小、格式可以不固定）
        /// </summary>
        /// <param name="jpFiInfosForSort"></param>
        /// <param name="jpFiInfos"></param>
        /// <param name="allCnFiles"></param>
        /// <param name="cnFs"></param>
        /// <param name="jpFs"></param>
        /// <returns></returns>
        private int WriteNgcTypeA(List<FilePosInfo> jpFiInfosForSort, List<FilePosInfo> jpFiInfos, List<FilePosInfo> allCnFiles,
            BufferedStream cnFs, BufferedStream jpFs)
        {
            int copyFile = 0;
            int copyLen = 0;
            foreach (FilePosInfo jpFiInfo in jpFiInfosForSort)
            {
                // 更新进度条
                this.ProcessBarStep();

                // 如果是目录，不做任何处理
                if (jpFiInfo.IsFolder)
                {
                    continue;
                }

                // 查找原始文件对应的中文文件
                FilePosInfo cnFiInfo = allCnFiles.FirstOrDefault<FilePosInfo>(cnFile => cnFile.File.EndsWith(jpFiInfo.File, StringComparison.OrdinalIgnoreCase));
                if (cnFiInfo == null)
                {
                    // 如果没有找到中文文件，写入原始的文件
                    jpFs.Seek(jpFiInfo.FilePos, SeekOrigin.Begin);
                    cnFs.Seek(jpFiInfo.TextStart, SeekOrigin.Begin);
                    this.FileStreamCopy(cnFs, jpFs, jpFiInfo.FileSize);
                }
                else
                {
                    cnFs.Seek(jpFiInfo.TextStart, SeekOrigin.Begin);
                    copyLen = this.FileStreamCopy(cnFs, cnFiInfo.File);
                    copyFile++;

                    if (copyLen != jpFiInfo.FileSize)
                    {
                        jpFiInfo.TextEnd = copyLen;

                        if (cnFiInfo.File.ToLower().IndexOf("start.dol") >= 0)
                        {
                            throw new Exception("Start.dol是重要系统文件，大小不能变化！");
                        }

                        // 文件大小变化，改变这个文件以后的所有文件的位置信息
                        int diffSize = Util.ResetPos(copyLen - jpFiInfo.FileSize, RAW_SIZE);
                        this.ResetAllPos(jpFiInfos, jpFiInfo.FilePos, diffSize);
                    }
                }
            }

            return copyFile;
        }

        /// <summary>
        /// 写入Ngc补丁（文件大小、格式固定）
        /// </summary>
        /// <param name="jpFiInfosForSort"></param>
        /// <param name="jpFiInfos"></param>
        /// <param name="allCnFiles"></param>
        /// <param name="cnFs"></param>
        /// <param name="jpFs"></param>
        /// <returns></returns>
        private int WriteNgcTypeB(List<FilePosInfo> jpFiInfosForSort, List<FilePosInfo> jpFiInfos, List<FilePosInfo> allCnFiles,
            BufferedStream cnFs, BufferedStream jpFs)
        {
            int copyLen = 0;
            int copyFile = 0;
            for (int i = 0; i < jpFiInfosForSort.Count; i++)
            {
                FilePosInfo jpFiInfo = jpFiInfosForSort[i];

                // 更新进度条
                this.ProcessBarStep();

                // 调整位置
                jpFs.Seek(jpFiInfo.FilePos, SeekOrigin.Begin);
                cnFs.Seek(jpFiInfo.TextStart, SeekOrigin.Begin);

                if (i < jpFiInfosForSort.Count - 1)
                {
                    copyLen = jpFiInfosForSort[i + 1].TextStart - jpFiInfo.TextStart;
                }
                else
                {
                    copyLen = NGC_ISO_LEN - jpFiInfo.TextStart;
                }

                // 写入原始的文件
                this.FileStreamCopy(cnFs, jpFs, copyLen);

                // 查找原始文件对应的中文文件
                FilePosInfo cnFiInfo = allCnFiles.FirstOrDefault<FilePosInfo>(cnFile => cnFile.File.EndsWith(jpFiInfo.File, StringComparison.OrdinalIgnoreCase));
                if (cnFiInfo != null)
                {
                    cnFs.Seek(jpFiInfo.TextStart, SeekOrigin.Begin);
                    copyLen = this.FileStreamCopy(cnFs, cnFiInfo.File);
                    copyFile++;

                    if (copyLen > jpFiInfo.FileSize)
                    {
                        string errInfo = cnFiInfo.File + "\r\n" + "补丁文件Size超过原来的文件，无法保持一致\r\n"
                            + "请去掉【保持内部文件大小、位置一致】的选项";
                        throw new Exception(errInfo);

                    }
                    else if (copyLen < jpFiInfo.FileSize)
                    {
                        if (cnFiInfo.File.ToLower().IndexOf("start.dol") >= 0)
                        {
                            throw new Exception("Start.dol是重要系统文件，大小不能变化！");
                        }

                        // 补丁文件小于原文件，补0
                        int emptyLen = jpFiInfo.FileSize - copyLen;
                        while (emptyLen-- > 0)
                        {
                            cnFs.WriteByte(0);
                        }
                    }
                }
            }

            return copyFile;
        }

        /// <summary>
        /// 开始打包(Wii)
        /// </summary>
        private void PatchWii(params object[] param)
        {
            string jpIsoFile = (string)param[0];
            string cnPatchFolder = (string)param[1];

            // 改变标题，按钮状态
            string oldTitle = this.Text;
            this.SetWinStatus(false, this.Text + " Iso文件解压中，请耐心等待......");

            // 开始打补丁
            try
            {
                System.Diagnostics.Process exep = new System.Diagnostics.Process();
                exep.StartInfo.FileName = @".\wit.exe";
                exep.StartInfo.CreateNoWindow = true;
                exep.StartInfo.UseShellExecute = false;

                if (!this.chkNoDec.Checked)
                {
                    // 开始解压Wii iso
                    exep.StartInfo.Arguments = "EXTRACT \"" + jpIsoFile + "\" --dest \"" + WII_ISO_TEMP + "\"  --psel DATA --section --long";
                    exep.Start();
                    exep.WaitForExit();
                }

                // 改变标题，按钮状态
                this.SetWinStatus(false, oldTitle + " 补丁文件复制中，请耐心等待......");

                //// 读取所有日文文件信息
                Util.IsGetAllFile = true;
                List<FilePosInfo> allJpFiles = Util.GetAllFiles(@".\" + WII_ISO_TEMP).Where(p => !p.IsFolder).ToList();
                string wiiIsoTemp = Path.GetFullPath(@".\" + WII_ISO_TEMP);

                // 读取所有补丁文件信息
                List<FilePosInfo> allCnFiles = Util.GetAllFiles(cnPatchFolder).Where(p => !p.IsFolder).ToList();
                Util.IsGetAllFile = false;

                // 将补丁文件Copy到解压的目录中
                foreach (FilePosInfo cnFile in allCnFiles)
                {
                    string jpFile = cnFile.File.Replace(cnPatchFolder, wiiIsoTemp);
                    if (File.Exists(jpFile))
                    {
                        File.Copy(cnFile.File, jpFile, true);
                    }
                    else
                    {
                        DialogResult dr = MessageBox.Show(Util.GetShortName(cnFile.File) + " 对应文件原始不存在，要继续吗？",
                            "请确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dr != DialogResult.Yes)
                        {
                            return;
                        }
                        //throw new Exception("选中的补丁目录不正确，一定要选择补丁的根目录");
                    }
                }

                // 改变标题，按钮状态
                this.SetWinStatus(false, oldTitle + " Iso文件压缩中，请耐心等待......");

                // 重新生成中文Iso文件
                string isoShortName = Util.GetShortNameWithoutType(jpIsoFile);
                string cnIsoFile = jpIsoFile.Replace(isoShortName + ".", isoShortName + "_CN.");
                if (File.Exists(cnIsoFile))
                {
                    File.Delete(cnIsoFile);
                }

                // 开始压缩Wii iso
                exep.StartInfo.Arguments = "COPY \"" + WII_ISO_TEMP + "\"  --iso --dest \"" + cnIsoFile + "\" --psel DATA --section --long";
                exep.Start();
                exep.WaitForExit();

                // 改变标题，按钮状态
                this.SetWinStatus(false, oldTitle + " 删除临时文件，请耐心等待......");

                // 删除临时文件
                DirectoryInfo di = new DirectoryInfo(@".\" + WII_ISO_TEMP);
                di.Delete(true);

                MessageBox.Show("打汉化补丁完成！");
            }
            finally
            {
                // 恢复按钮状态
                this.SetWinStatus(true, oldTitle);
            }
        }

        /// <summary>
        /// 对象比较
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private int PosInfoCompare(FilePosInfo a, FilePosInfo b)
        {
            if (a.IsFolder || b.IsFolder)
            {
                return 0;
            }
            else
            {
                return a.FilePos - b.FilePos;
            }
        }

        /// <summary>
        /// 改变Pos
        /// </summary>
        /// <param name="fileInfos"></param>
        /// <param name="changedPos"></param>
        private void ResetAllPos(List<FilePosInfo> fileInfos, int changedPos, int diff)
        {
            foreach (FilePosInfo newFileInfo in fileInfos)
            {
                if (!newFileInfo.IsFolder && newFileInfo.FilePos > changedPos)
                {
                    newFileInfo.TextStart += diff;
                }
            }
        }

        /// <summary>
        /// 数据流复制
        /// </summary>
        /// <param name="fsTarget"></param>
        /// <param name="fsSource"></param>
        private int FileStreamCopy(BufferedStream fsTarget, BufferedStream fsSource, int len)
        {
            //byte[] buffer = new byte[COPY_BUFFER];
            //int copied = 0;

            //while (copied <= (len - COPY_BUFFER))
            //{
            //    fsSource.Read(buffer, 0, COPY_BUFFER);

            //    fsTarget.Write(buffer, 0, COPY_BUFFER);

            //    copied += COPY_BUFFER;
            //}

            //int left = len - copied;
            //if (left > 0)
            //{
            //    fsSource.Read(buffer, 0, left);

            //    fsTarget.Write(buffer, 0, left);
            //}
            while (len-- > 0)
            {
                fsTarget.WriteByte((byte)fsSource.ReadByte());
            }

            //fsSource.Flush();
            //fsTarget.Flush();

            return len;
        }

        /// <summary>
        /// 数据流复制
        /// </summary>
        /// <param name="fsTarget"></param>
        /// <param name="file"></param>
        private int FileStreamCopy(BufferedStream fsTarget, string file)
        {
            FileStream fs = File.OpenRead(file);
            BufferedStream fsSource = new BufferedStream(fs, COPY_BLOCK);
            int copyLen = 0;

            using (fs)
            {
                using (fsSource)
                {
                    copyLen = (int)fs.Length;

                    this.FileStreamCopy(fsTarget, fsSource, copyLen);
                }
            }

            return copyLen;
        }

        /// <summary>
        /// 判断是否是Wii游戏的Iso
        /// </summary>
        /// <param name="isoFile"></param>
        /// <returns></returns>
        private bool IsWiiIso(string isoFile)
        {
            if (isoFile.EndsWith(".wbfs", StringComparison.OrdinalIgnoreCase)
                || new FileInfo(isoFile).Length > NGC_ISO_LEN)
            {
                return true;
            }

            if (isoFile.EndsWith(".gcm", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return false;
        }

        /// <summary>
        /// 设置当前控件的状态，设置Title
        /// </summary>
        /// <param name="status"></param>
        /// <param name="title"></param>
        private void SetWinStatus(bool status, string title)
        {
            this.Invoke((MethodInvoker)delegate()
            {
                this.btnJpSelect.Enabled = status;
                this.btnCnSelect.Enabled = status;
                this.chkSameSize.Enabled = status;
                this.btnCopy.Enabled = status;

                this.Text = title;
            });
        }
    }
}
