using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using nQuant;

namespace Hanhua.Common
{
    /// <summary>
    /// 共通方法
    /// </summary>
    public class Util
    {
        /// <summary>
        /// 日中字符对照表文件名
        /// </summary>
        public static string jpCnCharMapFileName = "./JpCnCharMap.txt";

        /// <summary>
        /// 要过滤的文件（不需要查询）的后缀名
        /// </summary>
        public static Dictionary<string, string> notSearchFile = Util.GetNotSearchFile();

        /// <summary>
        /// 是否需要检查Tpl文件
        /// </summary>
        public static bool NeedCheckTpl = false;

        /// <summary>
        /// 是否需要取得所有文件
        /// </summary>
        public static bool IsGetAllFile = false;

        /// <summary>
        /// 每次复制的字节数
        /// </summary>
        private const int COPY_BLOCK = 1024 * 1024 * 15;

        #region " 文件处理共通 "

        /// <summary>
        /// 取得文件名
        /// </summary>
        /// <param name="byData"></param>
        /// <param name="fileNameOffset"></param>
        /// <returns></returns>
        public static string GetFileNameFromStringTable(byte[] byData, int fileNameOffset)
        {
            int fileNameStartPos = fileNameOffset;
            while (byData[fileNameOffset] != 0)
            {
                fileNameOffset++;
            }
            fileNameOffset--;

            return Util.GetHeaderString(byData, fileNameStartPos, fileNameOffset);
        }

        /// <summary>
        /// 取得文件名
        /// </summary>
        /// <param name="byData"></param>
        /// <param name="fileNameOffset"></param>
        /// <returns></returns>
        public static string GetFileNameFromStringTable(byte[] byData, int fileNameOffset, Encoding encoding)
        {
            int fileNameStartPos = fileNameOffset;
            while (byData[fileNameOffset] != 0)
            {
                fileNameOffset++;
            }
            fileNameOffset--;

            return Util.GetHeaderString(byData, fileNameStartPos, fileNameOffset, encoding);
        }

        /// <summary>
        /// 取得文件名
        /// </summary>
        /// <param name="byData"></param>
        /// <param name="fileNameOffset"></param>
        /// <returns></returns>
        public static string GetFileNameFromStringTable(byte[] byData, int fileNameOffset, int num, Encoding encoding)
        {
            int fileNameStartPos = fileNameOffset;
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < num; i++)
            {
                while (byData[fileNameOffset] != 0)
                {
                    fileNameOffset++;
                }
                fileNameOffset--;

                sb.Append(Util.GetHeaderString(byData, fileNameStartPos, fileNameOffset, encoding));
                if (i < (num - 1))
                {
                    fileNameOffset += 2;
                    fileNameStartPos = fileNameOffset;
                    sb.Append("\n");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// RARC 解码
        /// </summary>
        /// <param name="byRarc"></param>
        public static TreeNode RarcDecode(byte[] byRarc)
        {
            RarcNode rootNode = new RarcNode();
            TreeNode root = new TreeNode();

            // 取得Header信息
            string aracMagic = Util.GetHeaderString(byRarc, 0, 3);
            int size = Util.GetOffset(byRarc, 4, 7);
            rootNode.DataStartPos = Util.GetOffset(byRarc, 12, 15) + 0x20;
            int numNodes = Util.GetOffset(byRarc, 32, 35);
            rootNode.FileEntriesOffset = Util.GetOffset(byRarc, 44, 47) + 0x20;
            rootNode.StringTablePos = Util.GetOffset(byRarc, 52, 55) + 0x20;

            // 取得Root Node
            int nodeStartPos = 64;
            rootNode.NodeType = Util.GetOffset(byRarc, nodeStartPos, nodeStartPos + 3);
            rootNode.FileNameOffset = Util.GetOffset(byRarc, nodeStartPos + 4, nodeStartPos + 7);
            rootNode.FileName = Util.GetFileNameFromStringTable(byRarc, rootNode.StringTablePos + rootNode.FileNameOffset);
            rootNode.NumFileEntries = Util.GetOffset(byRarc, nodeStartPos + 10, nodeStartPos + 11);
            rootNode.FirstFileEntriesOffset = Util.GetOffset(byRarc, nodeStartPos + 12, nodeStartPos + 15);

            // 取得Node的File Entries
            root.Text = rootNode.FileName;
            Util.GetRarcFileInfo(rootNode, byRarc, root);

            return root;
        }

        /// <summary>
        /// 生化危机0特殊的Arc解码
        /// </summary>
        /// <param name="byRarc"></param>
        public static TreeNode Bio0ArcDecode(byte[] byBio0)
        {
            RarcNode rootNode = new RarcNode();
            TreeNode root = new TreeNode();
            root.Text = "";

            // 取得Header信息
            int nodeNum = Util.GetOffset(byBio0, 4, 7);
            int offsetNode = Util.GetOffset(byBio0, 8, 11);

            // 循环取得Node信息
            for (int i = 0; i < nodeNum; i++)
            {
                // 取得各个Node信息
                TreeNode childTreeNode = new TreeNode();
                RarcNode childNode = new RarcNode();
                int offsetFile = Util.GetOffset(byBio0, offsetNode, offsetNode + 3);
                int fileSize = Util.GetOffset(byBio0, offsetNode + 4, offsetNode + 7);
                childTreeNode.Text = Util.GetFileNameFromStringTable(byBio0, offsetNode + 8);

                // 设置子Node信息
                byte[] byChildTreeNodeData = new byte[fileSize];
                Array.Copy(byBio0, offsetFile, byChildTreeNodeData, 0, fileSize);
                childNode.FileData = byChildTreeNodeData;
                childTreeNode.Tag = childNode;
                if (childTreeNode.Text.ToLower().EndsWith("lz"))
                {
                    childNode.FileType = "lz";
                }
                else
                {
                    childNode.FileType = "nolz";
                }
                
                root.Nodes.Add(childTreeNode);

                offsetNode += 0x20;
            }
            
            return root;
        }

        /// <summary>
        /// U8 解码
        /// </summary>
        /// <param name="byRarc"></param>
        public static TreeNode U8Decode(byte[] byU8)
        {
            RarcNode rootNode = new RarcNode();
            TreeNode root = new TreeNode();

            // 取得Header信息
            string aracMagic = Util.GetHeaderString(byU8, 0, 3);
            int offsetFirstNode = Util.GetOffset(byU8, 4, 7);
            int size = Util.GetOffset(byU8, 8, 11);
            rootNode.DataStartPos = Util.GetOffset(byU8, 12, 15);

            // 取得Root Node
            rootNode.NodeType = Util.GetOffset(byU8, offsetFirstNode, offsetFirstNode);
            rootNode.FileNameOffset = Util.GetOffset(byU8, offsetFirstNode + 1, offsetFirstNode + 3);
            int nodeNum = Util.GetOffset(byU8, offsetFirstNode + 8, offsetFirstNode + 11);
            rootNode.StringTablePos = nodeNum * 12 + offsetFirstNode;
            rootNode.FileName = Util.GetFileNameFromStringTable(byU8, rootNode.StringTablePos + rootNode.FileNameOffset);
            rootNode.NumFileEntries = nodeNum;

            rootNode.FirstFileEntriesOffset = offsetFirstNode + 12;

            // 取得Node的File Entries
            root.Text = rootNode.FileName;
            Util.GetU8FileInfo(rootNode, byU8, root);

            return root;
        }

        /// <summary>
        /// 取得U8文件信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="byRarc"></param>
        /// <param name="rarcFileList"></param>
        public static int GetU8FileInfo(RarcNode node, byte[] byU8, TreeNode treeNode)
        {
            int nodeIndex = 1;
            int fileEntriesPos = node.FirstFileEntriesOffset;
            while (nodeIndex < node.NumFileEntries)
            {
                TreeNode childTreeNode = new TreeNode();
                RarcNode childNode = new RarcNode();
                int id = Util.GetOffset(byU8, fileEntriesPos, fileEntriesPos);
                int fileNameOffset = Util.GetOffset(byU8, fileEntriesPos + 1, fileEntriesPos + 3);
                childTreeNode.Text = Util.GetFileNameFromStringTable(byU8, node.StringTablePos + fileNameOffset);

                int startPos = Util.GetOffset(byU8, fileEntriesPos + 4, fileEntriesPos + 7);
                int size = Util.GetOffset(byU8, fileEntriesPos + 8, fileEntriesPos + 11);
                
                if (id == 1)
                {
                    // subdirectory
                    // 取得Child Node
                    childNode.NodeType = id;
                    childNode.FileNameOffset = fileNameOffset;
                    childNode.FileName = childTreeNode.Text;
                    childNode.NumFileEntries = size;

                    childNode.StringTablePos = node.StringTablePos;
                    childNode.FirstFileEntriesOffset = node.FirstFileEntriesOffset + 12;
                    childNode.DataStartPos = node.DataStartPos;

                    // 追加Child文件信息
                    childTreeNode.Text += " , Pos : " + startPos + " , size : " + size;
                    treeNode.Nodes.Add(childTreeNode);
                    //nodeIndex += CommonUtil.GetU8FileInfo(childNode, byU8, childTreeNode);
                    //fileEntriesPos += nodeIndex * 12;
                    
                }
                else
                {
                    // 添加Node信息
                    if ((startPos + size) > byU8.Length)
                    {
                        size = byU8.Length - startPos;
                    }
                    byte[] byChildTreeNodeData = new byte[size];
                    Array.Copy(byU8, startPos, byChildTreeNodeData, 0, byChildTreeNodeData.Length);

                    // 绑定数据
                    childNode.FileData = byChildTreeNodeData;
                    childNode.DataStartPos = startPos;

                    // 保存文件的数据到节点上
                    string[] names = childTreeNode.Text.Split('.');
                    childNode.FileType = names[names.Length - 1];
                    childNode.FileName = childTreeNode.Text;
                    childTreeNode.Tag = childNode;
                    treeNode.Nodes.Add(childTreeNode);
                }

                nodeIndex++;
                fileEntriesPos += 12;
            }

            return node.NumFileEntries;
        }

        /// <summary>
        /// 取得RARC文件信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="byRarc"></param>
        /// <param name="rarcFileList"></param>
        public static void GetRarcFileInfo(RarcNode node, byte[] byRarc, TreeNode treeNode)
        {
            int fileEntriesPos = node.FileEntriesOffset + node.FirstFileEntriesOffset * 20;
            for (int i = 0; i < node.NumFileEntries; i++)
            {
                int id = Util.GetOffset(byRarc, fileEntriesPos, fileEntriesPos + 1);
                int fileNameOffset = Util.GetOffset(byRarc, fileEntriesPos + 6, fileEntriesPos + 7);
                string fileName = Util.GetFileNameFromStringTable(byRarc, node.StringTablePos + fileNameOffset);
                int fileDataStartPos = Util.GetOffset(byRarc, fileEntriesPos + 8, fileEntriesPos + 11);
                int fileDataSize = Util.GetOffset(byRarc, fileEntriesPos + 12, fileEntriesPos + 15);
                TreeNode childTreeNode = new TreeNode();

                if (fileNameOffset == 0 || fileNameOffset == 2)
                {
                    continue;
                }

                if (id == 0xFFFF)
                {
                    // subdirectory
                    // don't go to "." and ".."
                    // 取得Child Node
                    RarcNode childNode = new RarcNode();
                    int nodeStartPos = 64 + 16 * fileDataStartPos;
                    childNode.NodeType = Util.GetOffset(byRarc, nodeStartPos, nodeStartPos + 3);
                    childNode.FileNameOffset = Util.GetOffset(byRarc, nodeStartPos + 4, nodeStartPos + 7);
                    childNode.FileName = Util.GetFileNameFromStringTable(byRarc, node.StringTablePos + childNode.FileNameOffset);
                    childNode.NumFileEntries = Util.GetOffset(byRarc, nodeStartPos + 10, nodeStartPos + 11);
                    childNode.FirstFileEntriesOffset = Util.GetOffset(byRarc, nodeStartPos + 12, nodeStartPos + 15);

                    childNode.StringTablePos = node.StringTablePos;
                    childNode.FileEntriesOffset = node.FileEntriesOffset;
                    childNode.DataStartPos = node.DataStartPos;

                    // 追加Child文件信息
                    childTreeNode.Text = fileName;
                    treeNode.Nodes.Add(childTreeNode);
                    Util.GetRarcFileInfo(childNode, byRarc, childTreeNode);
                }
                else
                {
                    // 添加Node信息
                    childTreeNode.Text = fileName;
                    RarcNode fileInfo = new RarcNode();
                    byte[] byChildTreeNodeData = new byte[fileDataSize];
                    Array.Copy(byRarc, node.DataStartPos + fileDataStartPos, byChildTreeNodeData, 0, byChildTreeNodeData.Length);
                    
                    string[] fileNameArray = fileName.ToLower().Split('.');
                    fileName = fileNameArray[fileNameArray.Length - 1];

                    if ("bmg".Equals(fileName)
                        || "bfn".Equals(fileName)
                        || "bti".Equals(fileName))
                    {
                        // 绑定数据
                        fileInfo.DataStartPos = node.DataStartPos + fileDataStartPos;
                        fileInfo.FileData = byChildTreeNodeData;

                        // 保存文件的数据到节点上
                        fileInfo.FileType = fileName;
                        childTreeNode.Tag = fileInfo;
                    }
                    treeNode.Nodes.Add(childTreeNode);
                }
                
                fileEntriesPos += 20;
            }
        }

        /// <summary>
        /// 将字节数组，通过特定的Encoder转换成字符串
        /// 里面的结束符转换成自定义字符
        /// </summary>
        /// <param name="byData"></param>
        /// <param name="encoder"></param>
        /// <returns></returns>
        public static string GetStringFromByte(byte[] byData, Encoding encoder)
        {
            string strCurrentLine = encoder.GetString(byData);
            int endCharIndex = strCurrentLine.IndexOf('\0');
            if (endCharIndex == -1)
            {
                return strCurrentLine;
            }

            string strRet = encoder.GetString(byData);
            return strRet.Replace("\0", "\n");

            //StringBuilder sb = new StringBuilder();
            //int intEndCharNum = 0;

            //// 将结束符变成自定义的字符串，以便于编辑
            //while (endCharIndex != -1)
            //{
            //    // 找到挨着的所有结束符
            //    intEndCharNum++;
            //    while ((endCharIndex + intEndCharNum) < strCurrentLine.Length
            //        && "\0".Equals(strCurrentLine.Substring(endCharIndex + intEndCharNum, 1)))
            //    {
            //        intEndCharNum++;
            //    }

            //    // 重新拼字符串
            //    sb.Append(strCurrentLine.Substring(0, endCharIndex));
            //    sb.Append("[E*" + intEndCharNum.ToString().PadLeft(2, '0') + "]\n");
            //    strCurrentLine = strCurrentLine.Substring(endCharIndex + intEndCharNum);

            //    intEndCharNum = 0;
            //    endCharIndex = strCurrentLine.IndexOf('\0');
            //}
            //sb.Append(strCurrentLine);

            //return sb.ToString();
        }

        /// <summary>
        /// 取得不需要检索的文件列表
        /// </summary>
        /// <returns>不需要检索的文件列表</returns>
        public static Dictionary<string, string> GetNotSearchFile()
        {
            Dictionary<string, string> notSearchFile = new Dictionary<string, string>();

            notSearchFile.Add("BRSAR", "WII音频文件");
            notSearchFile.Add("BRSTM", "WII音频文件");
            notSearchFile.Add("STR", "PS上使用的视频压缩格式");
            notSearchFile.Add("PSS", "PS2上动画文件");
            notSearchFile.Add("SFD", "WII动画文件");
            notSearchFile.Add("EDH", "和声音文件放在一起，不知道作用");
            notSearchFile.Add("TPL", "WII图片文件");
            notSearchFile.Add("GPL", "WII几何画板文件");
            notSearchFile.Add("ANM", "WII Animation Bank文件");
            notSearchFile.Add("ACT", "WII Actor Hierarchy文件");
            notSearchFile.Add("BRFNT", "WII 字库文件");
            notSearchFile.Add("THP", "WII 动画文件");
            notSearchFile.Add("AUD", "WII 声音文件");
            notSearchFile.Add("ADP", "WII 声音文件");
            notSearchFile.Add("AVI", "动画文件");
            notSearchFile.Add("H4M", "Ngc 动画文件");

            return notSearchFile;
        }

        /// <summary>
        /// 得到目录的所有文件
        /// </summary>
        /// <param name="strFolder">目录</param>
        /// <returns>目录的所有文件</returns>
        public static List<FilePosInfo> GetAllFiles(string strFolder)
        {
            List<FilePosInfo> fileNameInfo = new List<FilePosInfo>();
            Util.GetAllFilesInfo(strFolder, fileNameInfo, 0);

            return fileNameInfo;
        }

        /// <summary>
        /// 设置打开文件对话框的Filter
        /// </summary>
        /// <param name="filter"></param>
        public static string SetOpenDailog(string filter, string defaultFile)
        {
            // 打开要分析的文件
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = filter;
            if (string.IsNullOrEmpty(defaultFile))
            {
                openFile.FileName = System.IO.Path.GetFullPath(@"..\..\");
            }
            else 
            {
                openFile.FileName = defaultFile;
            }

            DialogResult rs = openFile.ShowDialog();
            if (rs == DialogResult.Cancel || string.IsNullOrEmpty(openFile.FileName))
            {
                return string.Empty;
            }

            return openFile.FileName;
        }

        /// <summary>
        /// 设置保存文件对话框的Filter
        /// </summary>
        /// <param name="filter"></param>
        public static string SetSaveDailog(string filter, string defaultFile)
        {
            // 打开要分析的文件
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = filter;
            if (string.IsNullOrEmpty(defaultFile))
            {
                saveFileDialog.FileName = System.IO.Path.GetFullPath(@"..\..\"); 
            }
            else 
            {
                saveFileDialog.FileName = defaultFile;
            }

            DialogResult rs = saveFileDialog.ShowDialog();
            if (rs == DialogResult.Cancel || string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                return string.Empty;
            }

            return saveFileDialog.FileName;
        }

        /// <summary>
        /// 取得目录信息
        /// </summary>
        /// <returns>目录信息</returns>
        public static string OpenFolder(string defaultPath)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            if (string.IsNullOrEmpty(defaultPath))
            {
                folderDlg.SelectedPath = System.IO.Path.GetFullPath(@"..\..\");
            }
            else
            {
                folderDlg.SelectedPath = defaultPath;
            }
            DialogResult dr = folderDlg.ShowDialog();

            if (dr == DialogResult.Cancel || string.IsNullOrEmpty(folderDlg.SelectedPath))
            {
                return string.Empty;
            }
            else
            {
                return folderDlg.SelectedPath;
            }
        }

        /// <summary>
        /// 取得短文件名（带文件类型）
        /// </summary>
        /// <param name="fileFullName"></param>
        /// <returns></returns>
        public static string GetShortName(string fileFullName)
        {
            string[] names = fileFullName.Split('\\');
            return names[names.Length - 1];
        }

        /// <summary>
        /// 取得短文件名（不文件类型）
        /// </summary>
        /// <param name="fileFullName"></param>
        /// <returns></returns>
        public static string GetShortNameWithoutType(string fileFullName)
        {
            string[] names = Util.GetShortName(fileFullName).Split('.');
            return names[0];
        }

        /// <summary>
        /// Bmg类型文件解码
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string BmgDecode(string file)
        {
            if (string.IsNullOrEmpty(file) || !File.Exists(file))
            {
                return string.Empty;
            }

            return Util.BmgDecode(File.ReadAllBytes(file));
        }

        /// <summary>
        /// Bmg类型文件解码
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string BmgDecode(byte[] byBmgData)
        {
            // MESGbmg1开头的判断
            if (byBmgData[0] == 0x4d
                && byBmgData[1] == 0x45
                && byBmgData[2] == 0x53
                && byBmgData[3] == 0x47
                && byBmgData[4] == 0x62
                && byBmgData[5] == 0x6d
                && byBmgData[6] == 0x67
                && byBmgData[7] == 0x31)
            {
                // 取得有多少句文本
                int txtCount = Util.GetOffset(byBmgData, 0x28, 0x29);
                int txtStep = Util.GetOffset(byBmgData, 0x2a, 0x2b);
                List<int> txtOffsets = new List<int>();
                for (int i = 0; i < txtCount; i++)
                {
                    txtOffsets.Add(Util.GetOffset(byBmgData, i * txtStep + 0x30, i * txtStep + 3 + 0x30));
                }

                // 计算文本位置
                int headerLen = 0x20;
                int infLen = Util.GetOffset(byBmgData, 0x24, 0x27);
                int datStart = headerLen + infLen;
                int datEnd = datStart + Util.GetOffset(byBmgData, datStart + 4, datStart + 7);

                // 循环取得文本
                StringBuilder sb = new StringBuilder();
                Encoding shiftJis = Encoding.GetEncoding(932);
                for (int i = 0; i < txtCount; i++)
                {
                    int txtOffset = txtOffsets[i];
                    int txtStart = datStart + 8 + txtOffset;
                    int nextTxtStart = datEnd;
                    if (i < txtCount - 1)
                    {
                        nextTxtStart = datStart + 8 + txtOffsets[i + 1];
                    }

                    if (txtStep == 4)
                    {
                        sb.Append(Util.GetHeaderString(byBmgData, txtStart, nextTxtStart - 1, shiftJis));
                        sb.Append("<BR>\n");
                    }
                    else if (txtStep == 8)
                    {
                        // 特殊处理里面的关键字
                        for (int j = txtStart + 1; j < nextTxtStart; j++)
                        {
                            if (byBmgData[j] == 0x1a)
                            {
                                // 追加前面正常的文本
                                sb.Append(Util.GetHeaderString(byBmgData, txtStart, j - 1, shiftJis));
                                
                                // 追加后面的关键字
                                sb.Append("^");
                                while ((byBmgData[j] & 0x80) != 0x80)
                                {
                                    sb.Append(byBmgData[j].ToString("x") + " ");
                                    j++;
                                }
                                sb.Append("^");

                                txtStart = j;
                            }
                        }

                        sb.Append("<BR>\n");
                    }
                }

                return sb.ToString().Replace("\0", string.Empty);
            }

            return string.Empty;
        }

        /// <summary>
        /// Bti类型图片解码
        /// </summary>
        /// <returns></returns>
        public static Image BtiDecode(string file)
        {
            if (string.IsNullOrEmpty(file) || !File.Exists(file))
            {
                return null; ;
            }

            return Util.BtiDecode(File.ReadAllBytes(file));
        }

        /// <summary>
        /// Bti类型图片解码
        /// </summary>
        /// <returns></returns>
        public static Image BtiDecode(byte[] byBtiData)
        {
            string imgFormat = Util.GetImageFormat(byBtiData[0]);
            int imgDataStart = Util.GetOffset(byBtiData, 0x1c, 0x1f);
            int imgWidth = Util.GetOffset(byBtiData, 2, 3);
            int imgHeight = Util.GetOffset(byBtiData, 4, 5);
            int imgLen = Util.GetImageByteCount(imgWidth, imgHeight, imgFormat);

            byte[] byImg = new byte[imgLen];
            Array.Copy(byBtiData, imgDataStart, byImg, 0, Math.Min(byBtiData.Length - imgDataStart, imgLen));

            Bitmap img;
            if ("CMPR".Equals(imgFormat))
            {
                img = Util.CmprImageDecode(
                    new Bitmap(imgWidth, imgHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb), byImg);
            }
            else if (imgFormat.Equals("C4_CI4")
                || imgFormat.Equals("C8_CI8")
                || imgFormat.Equals("C14X2_CI14x2"))
            {
                int paletteFormat = byBtiData[9];//Util.GetOffset(byBtiData, 8, 9);
                int paletteStart = Util.GetOffset(byBtiData, 0xc, 0xf);
                int paletteLen = 32;
                if (imgFormat.Equals("C8_CI8"))
                {
                    paletteLen = 512;
                }
                else if (imgFormat.Equals("C14X2_CI14x2"))
                {
                    paletteLen = 256 * 256 * 2;
                }
                byte[] byPalette = new byte[paletteLen];
                Array.Copy(byBtiData, paletteStart, byPalette, 0, Math.Min(byPalette.Length, byBtiData.Length - paletteStart));

                img = Util.PaletteImageDecode(
                    new Bitmap(imgWidth, imgHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb),
                    byImg, imgFormat, byPalette, paletteFormat);
            }
            else
            {
                img = Util.ImageDecode(
                    new Bitmap(imgWidth, imgHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb),
                    byImg, imgFormat);
            }

            return img;
        }

        #endregion

        #region " 图像处理共通 "

        /// <summary>
        /// CMPR图像格式的像素解码
        /// </summary>
        /// <param name="blockByte"></param>
        /// <returns></returns>
        public static List<Color> GetCmprBlockPixel(byte[] blockByte)
        {
            List<Color> cmprPixel = new List<Color>();
            List<Color[]> cmprBlockPixel = new List<Color[]>();

            // 开始循环4个子Block
            for (int subBlockNum = 0; subBlockNum < 4; subBlockNum++)
            {
                // 取得当前子Block的数据
                byte[] subBlock = new byte[8];
                Array.Copy(blockByte, subBlockNum * 8, subBlock, 0, subBlock.Length);

                // 取得调色板数据
                Color[] paletteEntries = new Color[4];
                Color firstPaletteEntries = Util.GetPixelFromImageByte(subBlock, 0, "RGB565", null);
                Color secondPaletteEntries = Util.GetPixelFromImageByte(subBlock, 1, "RGB565", null);
                paletteEntries[0] = firstPaletteEntries;
                paletteEntries[1] = secondPaletteEntries;

                int intRed;
                int intGreen;
                int intBlue;
                if (Util.GetOffset(subBlock, 0, 1) > Util.GetOffset(subBlock, 2, 3))
                {
                    intRed = ((secondPaletteEntries.R - firstPaletteEntries.R) >> 1) - ((secondPaletteEntries.R - firstPaletteEntries.R) >> 3);
                    intGreen = ((secondPaletteEntries.G - firstPaletteEntries.G) >> 1) - ((secondPaletteEntries.G - firstPaletteEntries.G) >> 3);
                    intBlue = ((secondPaletteEntries.B - firstPaletteEntries.B) >> 1) - ((secondPaletteEntries.B - firstPaletteEntries.B) >> 3);
                    paletteEntries[2] = Color.FromArgb(0xFF, firstPaletteEntries.R + intRed, firstPaletteEntries.G + intGreen, firstPaletteEntries.B + intBlue);
                    paletteEntries[3] = Color.FromArgb(0xFF, secondPaletteEntries.R - intRed, secondPaletteEntries.G - intGreen, secondPaletteEntries.B - intBlue);
                }
                else
                {
                    intRed = (secondPaletteEntries.R + firstPaletteEntries.R + 1) / 2;
                    intGreen = (secondPaletteEntries.G + firstPaletteEntries.G + 1) / 2;
                    intBlue = (secondPaletteEntries.B + firstPaletteEntries.B + 1) / 2;

                    paletteEntries[2] = Color.FromArgb(0xFF, intRed, intGreen, intBlue);
                    paletteEntries[3] = Color.FromArgb(0, secondPaletteEntries.R, secondPaletteEntries.G, secondPaletteEntries.B);
                }

                // 取得调色板顺序
                List<Color> subBlockPixel = new List<Color>();
                for (int i = 4; i < 8; i++)
                {
                    subBlockPixel.Add(paletteEntries[(subBlock[i] >> 6) & 3]);
                    subBlockPixel.Add(paletteEntries[(subBlock[i] >> 4) & 3]);
                    subBlockPixel.Add(paletteEntries[(subBlock[i] >> 2) & 3]);
                    subBlockPixel.Add(paletteEntries[subBlock[i] & 3]);
                }

                cmprBlockPixel.Add(subBlockPixel.ToArray());
            }

            #region " 生成整个Block颜色数据 "
            // 生成整个Block颜色数据
            cmprPixel.Add(cmprBlockPixel[0][0]);
            cmprPixel.Add(cmprBlockPixel[0][1]);
            cmprPixel.Add(cmprBlockPixel[0][2]);
            cmprPixel.Add(cmprBlockPixel[0][3]);
            cmprPixel.Add(cmprBlockPixel[1][0]);
            cmprPixel.Add(cmprBlockPixel[1][1]);
            cmprPixel.Add(cmprBlockPixel[1][2]);
            cmprPixel.Add(cmprBlockPixel[1][3]);
            cmprPixel.Add(cmprBlockPixel[0][4]);
            cmprPixel.Add(cmprBlockPixel[0][5]);
            cmprPixel.Add(cmprBlockPixel[0][6]);
            cmprPixel.Add(cmprBlockPixel[0][7]);
            cmprPixel.Add(cmprBlockPixel[1][4]);
            cmprPixel.Add(cmprBlockPixel[1][5]);
            cmprPixel.Add(cmprBlockPixel[1][6]);
            cmprPixel.Add(cmprBlockPixel[1][7]);

            cmprPixel.Add(cmprBlockPixel[0][8]);
            cmprPixel.Add(cmprBlockPixel[0][9]);
            cmprPixel.Add(cmprBlockPixel[0][10]);
            cmprPixel.Add(cmprBlockPixel[0][11]);
            cmprPixel.Add(cmprBlockPixel[1][8]);
            cmprPixel.Add(cmprBlockPixel[1][9]);
            cmprPixel.Add(cmprBlockPixel[1][10]);
            cmprPixel.Add(cmprBlockPixel[1][11]);
            cmprPixel.Add(cmprBlockPixel[0][12]);
            cmprPixel.Add(cmprBlockPixel[0][13]);
            cmprPixel.Add(cmprBlockPixel[0][14]);
            cmprPixel.Add(cmprBlockPixel[0][15]);
            cmprPixel.Add(cmprBlockPixel[1][12]);
            cmprPixel.Add(cmprBlockPixel[1][13]);
            cmprPixel.Add(cmprBlockPixel[1][14]);
            cmprPixel.Add(cmprBlockPixel[1][15]);

            cmprPixel.Add(cmprBlockPixel[2][0]);
            cmprPixel.Add(cmprBlockPixel[2][1]);
            cmprPixel.Add(cmprBlockPixel[2][2]);
            cmprPixel.Add(cmprBlockPixel[2][3]);
            cmprPixel.Add(cmprBlockPixel[3][0]);
            cmprPixel.Add(cmprBlockPixel[3][1]);
            cmprPixel.Add(cmprBlockPixel[3][2]);
            cmprPixel.Add(cmprBlockPixel[3][3]);
            cmprPixel.Add(cmprBlockPixel[2][4]);
            cmprPixel.Add(cmprBlockPixel[2][5]);
            cmprPixel.Add(cmprBlockPixel[2][6]);
            cmprPixel.Add(cmprBlockPixel[2][7]);
            cmprPixel.Add(cmprBlockPixel[3][4]);
            cmprPixel.Add(cmprBlockPixel[3][5]);
            cmprPixel.Add(cmprBlockPixel[3][6]);
            cmprPixel.Add(cmprBlockPixel[3][7]);

            cmprPixel.Add(cmprBlockPixel[2][8]);
            cmprPixel.Add(cmprBlockPixel[2][9]);
            cmprPixel.Add(cmprBlockPixel[2][10]);
            cmprPixel.Add(cmprBlockPixel[2][11]);
            cmprPixel.Add(cmprBlockPixel[3][8]);
            cmprPixel.Add(cmprBlockPixel[3][9]);
            cmprPixel.Add(cmprBlockPixel[3][10]);
            cmprPixel.Add(cmprBlockPixel[3][11]);
            cmprPixel.Add(cmprBlockPixel[2][12]);
            cmprPixel.Add(cmprBlockPixel[2][13]);
            cmprPixel.Add(cmprBlockPixel[2][14]);
            cmprPixel.Add(cmprBlockPixel[2][15]);
            cmprPixel.Add(cmprBlockPixel[3][12]);
            cmprPixel.Add(cmprBlockPixel[3][13]);
            cmprPixel.Add(cmprBlockPixel[3][14]);
            cmprPixel.Add(cmprBlockPixel[3][15]);
            #endregion

            return cmprPixel;
        }

        /// <summary>
        /// 取得颜色的模板（为了C4, C8 and C14X2）
        /// </summary>
        /// <param name="paletteType">颜色的模板类型</param>
        /// <returns></returns>
        public static Color[] GetPalette(int paletteType, byte[] byPalette)
        {
            // 设置模板大小
            int paletteSize = byPalette.Length / 2;
            Color[] paletteColor = new Color[paletteSize];
            int intRed;
            int intGreen;
            int intBlue;
            int intAlpha;
            int intColor;

            for (int i = 0; i < paletteSize; i++)
            {
                switch (paletteType)
                {
                    // IA8 
                    case 0:
                        paletteColor[i] = Color.FromArgb(byPalette[i * 2], byPalette[i * 2 + 1], byPalette[i * 2 + 1], byPalette[i * 2 + 1]);
                        break;

                    // RGB565
                    case 1:
                        intColor = byPalette[i * 2] << 8 | byPalette[i * 2 + 1];
                        intRed = Util.Convert5To8((byte)((intColor >> 11) & 0x1F));
                        intGreen = Util.Convert6To8((byte)((intColor >> 5) & 0x3F));
                        intBlue = Util.Convert5To8((byte)(intColor & 0x1F));
                        paletteColor[i] = Color.FromArgb(0xFF, intRed, intGreen, intBlue);
                        break;

                    // RGB5A3
                    case 2:
                        intColor = byPalette[i * 2] << 8 | byPalette[i * 2 + 1];
                        if ((intColor & 0x8000) == 0x8000)
                        {
                            // not use alpha 5,5,5
                            intRed = Convert5To8((byte)((intColor >> 10) & 0x1F));
                            intGreen = Convert5To8((byte)((intColor >> 5) & 0x1F));
                            intBlue = Convert5To8((byte)(intColor & 0x1F));
                            paletteColor[i] = Color.FromArgb(0xFF, intRed, intGreen, intBlue);
                        }
                        else
                        {
                            // use alpha 3,4,4,4
                            intAlpha = Util.Convert3To8((byte)((intColor >> 12) & 0x7));
                            intRed = Util.Convert4To8((byte)((intColor >> 8) & 0xF));
                            intGreen = Util.Convert4To8((byte)((intColor >> 4) & 0xF));
                            intBlue = Util.Convert4To8((byte)(intColor & 0xF));
                            paletteColor[i] = Color.FromArgb(intAlpha, intRed, intGreen, intBlue);
                        }
                        break;
                }
            }

            return paletteColor;
        }

        /// <summary>
        /// 像素解码
        /// </summary>
        /// <param name="blockByte">Block的字节数据</param>
        /// <param name="pixelNum">像素位置</param>
        /// <param name="imageFormat">图片类型</param>
        /// <returns>当前的像素</returns>
        public static Color GetPixelFromImageByte(byte[] imageByte, int pixelNum, string imageFormat, Color[] paletteColor)
        {
            // 取得当前像素对应的字节数据
            int intColor;
            int intAlpha;
            int intRed;
            int intGreen;
            int intBlue;
            int intPalette;

            switch (imageFormat)
            {
                case "I4":
                    if (pixelNum % 2 == 0)
                    {
                        // 取前4位
                        intColor = Util.Convert4To8((byte)(imageByte[pixelNum / 2] >> 4));
                    }
                    else
                    {
                        // 取后4位
                        intColor = Util.Convert4To8((byte)(imageByte[pixelNum / 2] & 0x0F));
                    }
                    return Color.FromArgb(0xFF, intColor, intColor, intColor);

                case "I8":
                    intColor = imageByte[pixelNum];
                    return Color.FromArgb(0xFF, intColor, intColor, intColor);

                case "IA4":
                    intAlpha = Util.Convert4To8((byte)(imageByte[pixelNum] >> 4));
                    intColor = Util.Convert4To8((byte)(imageByte[pixelNum] & 0x0F));
                    return Color.FromArgb(intAlpha, intColor, intColor, intColor);

                case "IA8":
                    intAlpha = imageByte[pixelNum * 2];
                    intColor = imageByte[pixelNum * 2 + 1];
                    return Color.FromArgb(intAlpha, intColor, intColor, intColor);

                case "RGB565":
                    intColor = imageByte[pixelNum * 2] << 8 | imageByte[pixelNum * 2 + 1];
                    //intColor = imageByte[pixelNum * 2 + 1] << 8 | imageByte[pixelNum * 2];
                    intRed = Util.Convert5To8((byte)((intColor >> 11) & 0x1F));
                    intGreen = Util.Convert6To8((byte)((intColor >> 5) & 0x3F));
                    intBlue = Util.Convert5To8((byte)(intColor & 0x1F));
                    return Color.FromArgb(0xFF, intRed, intGreen, intBlue);

                case "RGB5A3":
                    intColor = imageByte[pixelNum * 2] << 8 | imageByte[pixelNum * 2 + 1];
                    if ((intColor & 0x8000) == 0x8000)
                    {
                        // not use alpha 5,5,5
                        intRed = Convert5To8((byte)((intColor >> 10) & 0x1F));
                        intGreen = Convert5To8((byte)((intColor >> 5) & 0x1F));
                        intBlue = Convert5To8((byte)(intColor & 0x1F));
                        return Color.FromArgb(0xFF, intRed, intGreen, intBlue);
                    }
                    else
                    {
                        // use alpha 3,4,4,4
                        intAlpha = Util.Convert3To8((byte)((intColor >> 12) & 0x7));
                        intRed = Util.Convert4To8((byte)((intColor >> 8) & 0xF));
                        intGreen = Util.Convert4To8((byte)((intColor >> 4) & 0xF));
                        intBlue = Util.Convert4To8((byte)(intColor & 0xF));
                        return Color.FromArgb(intAlpha, intRed, intGreen, intBlue);
                    }

                case "RGBA32_RGBA8":
                    int blockStartNum = pixelNum / 16 * 32;
                    intAlpha = imageByte[blockStartNum + pixelNum * 2];
                    intRed = imageByte[blockStartNum + pixelNum * 2 + 1];
                    intGreen = imageByte[blockStartNum + pixelNum * 2 + 32];
                    intBlue = imageByte[blockStartNum + pixelNum * 2 + 1 + 32];
                    return Color.FromArgb(intAlpha, intRed, intGreen, intBlue);

                case "C4_CI4":
                    if (pixelNum % 2 == 0)
                    {
                        // 取前4位
                        intPalette = imageByte[pixelNum / 2] >> 4;
                    }
                    else
                    {
                        // 取后4位
                        intPalette = imageByte[pixelNum / 2] & 0x0F;
                    }
                    return paletteColor[intPalette];

                case "C8_CI8":
                    intPalette = imageByte[pixelNum];
                    return paletteColor[intPalette];

                case "C14X2_CI14x2":
                    intPalette = ((imageByte[pixelNum * 2] << 8) & 0x3F) | imageByte[pixelNum * 2 + 1];
                    return paletteColor[intPalette];
            }

            return Color.FromArgb(0, 0, 0, 0);
        }

        /// <summary>
        /// 调色版格式Image图像数据解码（C4, C8 and C14X2）
        /// </summary>
        /// <param name="image">字库Image</param>
        /// <param name="byteData">当前Image的Byte数据</param>
        /// <param name="imageFormat">图片类型</param>
        public static Bitmap PaletteImageDecode(Bitmap image, byte[] byteData, string imageFormat, byte[] byPalette, int paletteType)
        {
            Color[] paletteColor = Util.GetPalette(paletteType, byPalette);

            // 取得一个Block的基本信息
            int[] blockHeightWidth = Util.GetBlockWidthHeight(imageFormat);
            int blockHeight = blockHeightWidth[0];
            int blockWidth = blockHeightWidth[1];

            // 开始循环Image的高(每次递增一个Block的高)
            int intPexelNum = -1;
            for (int y = 0; y < image.Height; y += blockHeight)
            {
                // 开始循环Image的宽(每次递增一个Block的宽)
                for (int x = 0; x < image.Width; x += blockWidth)
                {
                    // 开始一个Block的高
                    for (int yPixel = y; yPixel < y + blockHeight; yPixel++)
                    {
                        // 开始一个Block的宽
                        for (int xPixel = x; xPixel < x + blockWidth; xPixel++)
                        {
                            intPexelNum++;
                            if (xPixel >= image.Width || yPixel >= image.Height)
                            {
                                continue;
                            }

                            image.SetPixel(xPixel, yPixel, Util.GetPixelFromImageByte(byteData, intPexelNum, imageFormat, paletteColor));
                        }
                    }
                }
            }
            
            return image;
        }

        /// <summary>
        /// 调色版格式Image图像数据解码（C4, C8 and C14X2）(不使用Block)
        /// </summary>
        /// <param name="image">字库Image</param>
        /// <param name="byteData">当前Image的Byte数据</param>
        /// <param name="imageFormat">图片类型</param>
        public static Bitmap PaletteImageDecodeNoUseBlock(Bitmap image, byte[] byteData, string imageFormat, byte[] byPalette, int paletteType)
        {
            Color[] paletteColor = Util.GetPalette(paletteType, byPalette);

            // 取得一个Block的基本信息
            int[] blockHeightWidth = Util.GetBlockWidthHeight(imageFormat);
            int blockHeight = blockHeightWidth[0];
            int blockWidth = blockHeightWidth[1];

            // 开始循环Image的高(每次递增一个Block的高)
            int intPexelNum = -1;
            for (int y = 0; y < image.Height; y += 1)
            {
                // 开始循环Image的宽(每次递增一个Block的宽)
                for (int x = 0; x < image.Width; x += 1)
                {
                    intPexelNum++;
                    image.SetPixel(x, y, Util.GetPixelFromImageByte(byteData, intPexelNum, imageFormat, paletteColor));
                }
            }

            return image;
        }

        /// <summary>
        /// 普通Image图像数据解码
        /// </summary>
        /// <param name="image">字库Image</param>
        /// <param name="byteData">当前Image的Byte数据</param>
        /// <param name="imageFormat">图片类型</param>
        public static Bitmap ImageDecode(Bitmap image, byte[] byteData, string imageFormat)
        {
            // 取得一个Block的基本信息
            int[] blockHeightWidth = Util.GetBlockWidthHeight(imageFormat);
            int blockHeight = blockHeightWidth[0];
            int blockWidth = blockHeightWidth[1];

            // 开始循环Image的高(每次递增一个Block的高)
            int intPexelNum = -1;
            for (int y = 0; y < image.Height; y += blockHeight)
            {
                // 开始循环Image的宽(每次递增一个Block的宽)
                for (int x = 0; x < image.Width; x += blockWidth)
                {
                    // 开始一个Block的高
                    for (int yPixel = y; yPixel < y + blockHeight; yPixel++)
                    {
                        // 开始一个Block的宽
                        for (int xPixel = x; xPixel < x + blockWidth; xPixel++)
                        {
                            intPexelNum++;
                            if (xPixel >= image.Width || yPixel >= image.Height)
                            {
                                continue;
                            }

                            image.SetPixel(xPixel, yPixel, Util.GetPixelFromImageByte(byteData, intPexelNum, imageFormat, null));
                        }
                    }
                }
            }
            
            return image;
        }

        /// <summary>
        /// 普通Image图像数据解码(不使用Block)
        /// </summary>
        /// <param name="image">字库Image</param>
        /// <param name="byteData">当前Image的Byte数据</param>
        /// <param name="imageFormat">图片类型</param>
        public static Bitmap ImageDecodeNoUseBlock(Bitmap image, byte[] byteData, string imageFormat)
        {
            // 取得一个Block的基本信息
            int[] blockHeightWidth = Util.GetBlockWidthHeight(imageFormat);
            int blockHeight = blockHeightWidth[0];
            int blockWidth = blockHeightWidth[1];

            // 开始循环Image的高(每次递增一个Block的高)
            int intPexelNum = -1;

            for (int y = 0; y < image.Height; y += 1)
            {
                // 开始循环Image的宽(每次递增一个Block的宽)
                for (int x = 0; x < image.Width; x += 1)
                {
                    intPexelNum++;
                    image.SetPixel(x, y, Util.GetPixelFromImageByte(byteData, intPexelNum, imageFormat, null));
                }
            }

            return image;
        }

        /// <summary>
        /// CMPR格式Image图像数据解码
        /// </summary>
        /// <param name="image">字库Image</param>
        /// <param name="byteData">当前Image的Byte数据</param>
        /// <param name="imageFormat">图片类型</param>
        public static Bitmap CmprImageDecode(Bitmap image, byte[] byteData)
        {
            // 开始循环Image的高(每次递增一个Block的高)
            int intBlockNum = 0;
            for (int y = 0; y < image.Height; y += 8)
            {
                // 开始循环Image的宽(每次递增一个Block的宽)
                for (int x = 0; x < image.Width; x += 8)
                {
                    // 取得Block的数据
                    byte[] currentBlockData = new byte[32];
                    Array.Copy(byteData, intBlockNum * 32, currentBlockData, 0, currentBlockData.Length);
                    List<Color> paletteEntries = Util.GetCmprBlockPixel(currentBlockData);
                    intBlockNum++;

                    // 开始一个Block的高
                    int intPexelNum = -1;
                    for (int yPixel = y; yPixel < y + 8; yPixel++)
                    {
                        // 开始一个Block的宽
                        for (int xPixel = x; xPixel < x + 8; xPixel++)
                        {
                            intPexelNum++;
                            if (xPixel >= image.Width || yPixel >= image.Height)
                            {
                                continue;
                            }

                            image.SetPixel(xPixel, yPixel, paletteEntries[intPexelNum]);
                        }
                    }
                }
            }

            return image;
        }

        /// <summary>
        /// 将字库图片转换成行列形式的一个个小格子图片
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static List<Bitmap[]> GetRowColImage(Bitmap image, int fontWidth, int fontHeight)
        {
            List<Bitmap[]> rowColImage = new List<Bitmap[]>();
            // 开始循环Image的高(每次递增一个Block的高)
            for (int y = 0; y < image.Height; y += fontHeight)
            {
                List<Bitmap> rowImage = new List<Bitmap>();
                // 开始循环Image的宽(每次递增一个Block的宽)
                for (int x = 0; x < image.Width; x += fontWidth)
                {
                    Bitmap blockImg;
                    if ((x + fontWidth) >= image.Width && (y + fontHeight) >= image.Height)
                    {
                        int newFontWidth = fontWidth - (x + fontWidth - image.Width);
                        int newFontHeight = fontHeight - (y + fontHeight - image.Height);
                        blockImg = new Bitmap(newFontWidth, newFontHeight, image.PixelFormat);
                    }
                    else if ((x + fontWidth) >= image.Width && (y + fontHeight) < image.Height)
                    {
                        int newFontWidth = fontWidth - (x + fontWidth - image.Width);
                        blockImg = new Bitmap(newFontWidth, fontHeight, image.PixelFormat);
                    }
                    else if ((x + fontWidth) < image.Width && (y + fontHeight) >= image.Height)
                    {
                        int newFontHeight = fontHeight - (y + fontHeight - image.Height);
                        blockImg = new Bitmap(fontWidth, newFontHeight, image.PixelFormat);
                    }
                    else
                    {
                        blockImg = new Bitmap(fontWidth, fontHeight, image.PixelFormat);
                    }

                    int newImgY = 0;
                    // 开始一个Block的高
                    for (int yPixel = y; yPixel < y + fontHeight; yPixel++)
                    {
                        int newImgX = 0;
                        // 开始一个Block的宽
                        for (int xPixel = x; xPixel < x + fontWidth; xPixel++)
                        {
                            if (xPixel >= image.Width || yPixel >= image.Height)
                            {
                                continue;
                            }

                            blockImg.SetPixel(newImgX++, newImgY, image.GetPixel(xPixel, yPixel));
                        }
                        newImgY++;
                    }

                    rowImage.Add(blockImg);
                }
                rowColImage.Add(rowImage.ToArray());
            }

            return rowColImage;
        }

        /// <summary>
        /// 将Image图像按照指定的格式，进行编码
        /// </summary>
        /// <param name="image">字库Image</param>
        /// <param name="imageFormat">图片类型</param>
        public static byte[] PaletteImageEncode(Bitmap image, string imageFormat, List<byte> byPalette, int paletteType)
        {
            byte[] imageData = new byte[Util.GetImageByteCount(image.Height, image.Width, imageFormat)];

            // 取得一个Block的基本信息
            int[] blockHeightWidth = Util.GetBlockWidthHeight(imageFormat);
            int blockHeight = blockHeightWidth[0];
            int blockWidth = blockHeightWidth[1];

            #region " 取得调色板数据 "

            int intPexelNum = -1;
            List<int> palette = new List<int>();
            if (byPalette.Count == 0)
            {
                UInt16 temp;
                for (int y = 0; y < image.Height; y++)
                {
                    // 开始循环Image的宽(每次递增一个Block的宽)
                    for (int x = 0; x < image.Width; x++)
                    {
                        Color color = image.GetPixel(x, y);
                        if (!palette.Contains(color.ToArgb()))
                        {
                            palette.Add(color.ToArgb());
                        }
                    }
                }

                int paletteCount = 0;
                int maxCount = 32;
                if (imageFormat == "C8_CI8")
                {
                    maxCount = 512;
                }
                else if (imageFormat == "C14X2_CI14x2")
                {
                    maxCount = 256 * 256 * 2;
                }

                foreach (int item in palette)
                {
                    Color color = Color.FromArgb(item);
                    switch (paletteType)
                    {
                        // IA8 
                        case 0:
                            byPalette.Add(color.A);
                            byPalette.Add(color.R);
                            break;

                        // RGB565
                        case 1:
                            temp = (UInt16)(Convert8To5(color.R) << 11 | Convert8To6(color.G) << 5 | Convert8To5(color.B));
                            byPalette.Add((byte)(temp >> 8));
                            byPalette.Add((byte)(temp & 0xFF));
                            break;

                        // RGB5A3
                        case 2:
                            if (color.A > 0xDA)
                            {
                                // not use alpha 5,5,5
                                temp = (UInt16)(Convert8To5(color.R) << 10 | Convert8To5(color.G) << 5 | Convert8To5(color.B));
                                byPalette.Add((byte)((temp >> 8) | 0x80));
                                byPalette.Add((byte)(temp & 0xFF));
                            }
                            else
                            {
                                // use alpha 3,4,4,4
                                byPalette.Add((byte)(Convert8To3(color.A) << 4 | Convert8To4(color.R) & 0x7F));
                                byPalette.Add((byte)(Convert8To4(color.G) << 4 | Convert8To4(color.B)));
                            }
                            break;
                    }

                    paletteCount += 2;
                }

                if (paletteCount > maxCount)
                {
                    throw new Exception("调色板颜色数(" + (paletteCount / 2) + ")超出范围(" + (maxCount / 2) + ")，请减少图片颜色数量");
                }

                if (paletteCount < maxCount)
                {
                    int blankPalette = maxCount - paletteCount;
                    while (blankPalette > 0)
                    {
                        byPalette.Add(0);
                        blankPalette--;
                    }
                }
            }
            else
            {
                Color[] paletteColor = Util.GetPalette(paletteType, byPalette.ToArray());
                foreach (Color color in paletteColor)
                {
                    palette.Add(color.ToArgb());
                }
            }

            #endregion

            // 开始循环Image的高(每次递增一个Block的高)
            int intByIndex = 0;
            intPexelNum = -1;
            int intPexelIndex = -1;
            int minPexelIndex = -1;
            int minTemp = 0xFF;
            for (int y = 0; y < image.Height; y += blockHeight)
            {
                // 开始循环Image的宽(每次递增一个Block的宽)
                for (int x = 0; x < image.Width; x += blockWidth)
                {
                    // 开始一个Block的高
                    for (int yPixel = y; yPixel < y + blockHeight; yPixel++)
                    {
                        // 开始一个Block的宽
                        for (int xPixel = x; xPixel < x + blockWidth; xPixel++)
                        {
                            intPexelNum++;
                            if (xPixel >= image.Width || yPixel >= image.Height)
                            {
                                intByIndex = SetNullPixelByteToList(intPexelNum, imageData, intByIndex, imageFormat);
                            }
                            else
                            {
                                intPexelIndex = -1;
                                minTemp = 0xFF;
                                minPexelIndex = -1;
                                Color color = image.GetPixel(xPixel, yPixel);
                                for (int i = 0; i < palette.Count; i++)
                                {
                                    if (color.ToArgb() == palette[i])
                                    {
                                        intPexelIndex = i;
                                        break;
                                    }
                                    else
                                    { 
                                        Color tempColor = Color.FromArgb(palette[i]);
                                        int temp = Math.Abs(color.A - tempColor.A) + Math.Abs(color.R - tempColor.R)
                                            + Math.Abs(color.G - tempColor.G) + Math.Abs(color.B - tempColor.B);
                                        if (temp < minTemp)
                                        {
                                            minTemp = temp;
                                            minPexelIndex = i;
                                        }
                                    }
                                }

                                if (intPexelIndex == -1)
                                {
                                    //throw new Exception("没找到索引像素！");
                                    intPexelIndex = minPexelIndex;
                                }

                                switch (imageFormat)
                                {
                                    case "C4_CI4":
                                        if (intPexelNum % 2 == 0)
                                        {
                                            // 设置前4位
                                            imageData[intByIndex] = (byte)(intPexelIndex << 4);
                                        }
                                        else
                                        {
                                            // 设置后4位
                                            byte intColor = imageData[intByIndex];
                                            imageData[intByIndex] = (byte)(intColor | intPexelIndex);
                                            intByIndex++;
                                        }
                                        break;

                                    case "C8_CI8":
                                        imageData[intByIndex++] = (byte)(intPexelIndex & 0xFF);
                                        break;

                                    case "C14X2_CI14x2":
                                        imageData[intByIndex++] = (byte)(intPexelIndex >> 4);
                                        imageData[intByIndex++] = (byte)(intPexelIndex & 0xFF);
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            return imageData;
        }

        /// <summary>
        /// 将Image图像按照指定的格式，进行编码
        /// </summary>
        /// <param name="image">字库Image</param>
        /// <param name="imageFormat">图片类型</param>
        public static byte[] ImageEncodeNoBlock(Bitmap image, string imageFormat)
        {
            if ("CMPR".Equals(imageFormat))
            {
                throw new Exception("ImageEncodeNoBlock CMPR方式未实现");
            }

            byte[] imageData = new byte[Util.GetImageByteCountNoBlock(image.Height, image.Width, imageFormat)];

            // 开始循环Image的高
            int intPexelNum = 0;
            int intByIndex = 0;
            for (int y = 0; y < image.Height; y++)
            {
                // 开始循环Image的宽
                for (int x = 0; x < image.Width; x++)
                {
                    intByIndex = SetPixelByteToList(image.GetPixel(x, y), intPexelNum, imageData, intByIndex, imageFormat);
                    intPexelNum++;

                    if ("RGBA32_RGBA8".Equals(imageFormat))
                    {
                        intByIndex += 32;
                    }
                }
            }

            return imageData;
        }

        /// <summary>
        /// 将Image图像按照指定的格式，进行编码
        /// </summary>
        /// <param name="image">字库Image</param>
        /// <param name="imageFormat">图片类型</param>
        public static byte[] ImageEncode(Bitmap image, string imageFormat)
        {
            if ("CMPR".Equals(imageFormat))
            {
                return Util.CmprImageEncode(image);
            }

            byte[] imageData = new byte[Util.GetImageByteCount(image.Height, image.Width, imageFormat)];
            // 取得一个Block的基本信息
            int[] blockHeightWidth = Util.GetBlockWidthHeight(imageFormat);
            int blockHeight = blockHeightWidth[0];
            int blockWidth = blockHeightWidth[1];

            // 开始循环Image的高(每次递增一个Block的高)
            int intPexelNum = 0;
            int intByIndex = 0;
            for (int y = 0; y < image.Height; y += blockHeight)
            {
                // 开始循环Image的宽(每次递增一个Block的宽)
                for (int x = 0; x < image.Width; x += blockWidth)
                {
                    // 开始一个Block的高
                    for (int yPixel = y; yPixel < y + blockHeight; yPixel++)
                    {
                        // 开始一个Block的宽
                        for (int xPixel = x; xPixel < x + blockWidth; xPixel++)
                        {
                            if (xPixel >= image.Width || yPixel >= image.Height)
                            {
                                intByIndex = SetNullPixelByteToList(intPexelNum, imageData, intByIndex, imageFormat);
                            }
                            else
                            {
                                intByIndex = SetPixelByteToList(image.GetPixel(xPixel, yPixel), intPexelNum, imageData, intByIndex, imageFormat);
                            }
                            intPexelNum++;
                        }
                    }

                    if ("RGBA32_RGBA8".Equals(imageFormat))
                    {
                        intByIndex += 32;
                    }
                }
            }

            return imageData;
        }

        /// <summary>
        /// CmprImage图像进行编码
        /// </summary>
        /// <param name="image">字库Image</param>
        public static byte[] CmprImageEncode(Bitmap image)
        {
            byte[] imageData = new byte[Util.GetImageByteCount(image.Height, image.Width, "CMPR")];
            List<Color> blockColor = new List<Color>();            

            // 取得一个Block的基本信息
            int blockHeight = 8;
            int blockWidth = 8;
            int byIndex = 0;

            // 开始循环Image的高(每次递增一个Block的高)
            for (int y = 0; y < image.Height; y += blockHeight)
            {
                // 开始循环Image的宽(每次递增一个Block的宽)
                for (int x = 0; x < image.Width; x += blockWidth)
                {
                    // 先清空当前Block
                    blockColor.Clear();

                    // 开始一个Block的高
                    for (int yPixel = y; yPixel < y + blockHeight; yPixel++)
                    {
                        // 开始一个Block的宽
                        for (int xPixel = x; xPixel < x + blockWidth; xPixel++)
                        {
                            if (xPixel >= image.Width || yPixel >= image.Height)
                            {
                                blockColor.Add(Color.FromArgb(0xFF, 0, 0, 0));
                            }
                            else
                            {
                                blockColor.Add(image.GetPixel(xPixel, yPixel));
                            }
                        }
                    }

                    // 处理当前Block的颜色数据
                    byte[] byBlock = Util.GetCmprBlockInfo(ResetColorOrder(blockColor));
                    Array.Copy(byBlock, 0, imageData, byIndex, byBlock.Length);
                    byIndex += 32;
                }
            }

            return imageData;
        }

        /// <summary>
        /// 重新将Cmpr一个Block的颜色排序
        /// </summary>
        /// <param name="blockColor"></param>
        /// <returns></returns>
        private static List<Color> ResetColorOrder(List<Color> blockColor)
        {
            List<Color> newBlockColor = new List<Color>();
            newBlockColor.Add(blockColor[0]);
            newBlockColor.Add(blockColor[1]);
            newBlockColor.Add(blockColor[2]);
            newBlockColor.Add(blockColor[3]);

            newBlockColor.Add(blockColor[8]);
            newBlockColor.Add(blockColor[9]);
            newBlockColor.Add(blockColor[10]);
            newBlockColor.Add(blockColor[11]);

            newBlockColor.Add(blockColor[16]);
            newBlockColor.Add(blockColor[17]);
            newBlockColor.Add(blockColor[18]);
            newBlockColor.Add(blockColor[19]);

            newBlockColor.Add(blockColor[24]);
            newBlockColor.Add(blockColor[25]);
            newBlockColor.Add(blockColor[26]);
            newBlockColor.Add(blockColor[27]);

            newBlockColor.Add(blockColor[4]);
            newBlockColor.Add(blockColor[5]);
            newBlockColor.Add(blockColor[6]);
            newBlockColor.Add(blockColor[7]);

            newBlockColor.Add(blockColor[12]);
            newBlockColor.Add(blockColor[13]);
            newBlockColor.Add(blockColor[14]);
            newBlockColor.Add(blockColor[15]);

            newBlockColor.Add(blockColor[20]);
            newBlockColor.Add(blockColor[21]);
            newBlockColor.Add(blockColor[22]);
            newBlockColor.Add(blockColor[23]);

            newBlockColor.Add(blockColor[28]);
            newBlockColor.Add(blockColor[29]);
            newBlockColor.Add(blockColor[30]);
            newBlockColor.Add(blockColor[31]);

            newBlockColor.Add(blockColor[32]);
            newBlockColor.Add(blockColor[33]);
            newBlockColor.Add(blockColor[34]);
            newBlockColor.Add(blockColor[35]);

            newBlockColor.Add(blockColor[40]);
            newBlockColor.Add(blockColor[41]);
            newBlockColor.Add(blockColor[42]);
            newBlockColor.Add(blockColor[43]);

            newBlockColor.Add(blockColor[48]);
            newBlockColor.Add(blockColor[49]);
            newBlockColor.Add(blockColor[50]);
            newBlockColor.Add(blockColor[51]);

            newBlockColor.Add(blockColor[56]);
            newBlockColor.Add(blockColor[57]);
            newBlockColor.Add(blockColor[58]);
            newBlockColor.Add(blockColor[59]);

            newBlockColor.Add(blockColor[36]);
            newBlockColor.Add(blockColor[37]);
            newBlockColor.Add(blockColor[38]);
            newBlockColor.Add(blockColor[39]);

            newBlockColor.Add(blockColor[44]);
            newBlockColor.Add(blockColor[45]);
            newBlockColor.Add(blockColor[46]);
            newBlockColor.Add(blockColor[47]);

            newBlockColor.Add(blockColor[52]);
            newBlockColor.Add(blockColor[53]);
            newBlockColor.Add(blockColor[54]);
            newBlockColor.Add(blockColor[55]);

            newBlockColor.Add(blockColor[60]);
            newBlockColor.Add(blockColor[61]);
            newBlockColor.Add(blockColor[62]);
            newBlockColor.Add(blockColor[63]);

            return newBlockColor;
        }

        /// <summary>
        /// 将一个Block的颜色数据转成字节数据
        /// </summary>
        /// <param name="blockColor"></param>
        /// <returns></returns>
        private static byte[] GetCmprBlockInfo(List<Color> blockColor)
        {
            byte[] byBlock = new byte[32];
            int index = 0;
            bool isRTrans = false;
            // 每次循环一个子Block（4*4的16个像素）
            while (index < blockColor.Count)
            {
                // 判断有无透明像素
                isRTrans = false;
                List<Color> diffColor = new List<Color>();
                for (int i = index; i <= index + 15; i++)
                {
                    if (blockColor[i].A == 0)
                    {
                        isRTrans = true;
                    }
                    else
                    {
                        bool isAdded = false;
                        for (int j = 0; j < diffColor.Count; j++)
                        {
                            if (diffColor[j].ToArgb() == blockColor[i].ToArgb())
                            {
                                isAdded = true;
                            }
                        }
                        
                        if (!isAdded)
                        {
                            diffColor.Add(blockColor[i]);
                        }
                    }
                }

                // 取得前两个颜色的值
                int color1;
                int color2;
                if (diffColor.Count == 0)
                {
                    color1 = GetRGB565Value(blockColor[0]);
                    color2 = GetRGB565Value(blockColor[1]);
                }
                else if (diffColor.Count == 1)
                {
                    color1 = GetRGB565Value(diffColor[0]);
                    color2 = GetRGB565Value(diffColor[0]);
                }
                else if (diffColor.Count == 2)
                {
                    color1 = GetRGB565Value(diffColor[0]);
                    color2 = GetRGB565Value(diffColor[1]);
                }
                else
                {
                    int[] rangeValue = GetRangeValue(diffColor, 0, isRTrans);
                    color1 = rangeValue[0];
                    color2 = rangeValue[1];
                }

                // 取得调色板数据
                int intRed;
                int intGreen;
                int intBlue;
                Color[] paletteEntries = new Color[4];
                if (isRTrans)
                {
                    if (color1 == color2)
                    {
                        // 使Color1 < Color2
                        color1 &= 0xFFFE;
                        color2 |= 1;
                    }
                    else if (color1 > color2)
                    {
                        int old = color1;
                        color1 = color2;
                        color2 = old;
                    }

                    paletteEntries[0] = Color.FromArgb(0xFF, Convert5To8((byte)(color1 >> 11)), Convert6To8((byte)(color1 >> 5 & 0x3F)), Convert5To8((byte)(color1 & 0x1F)));
                    paletteEntries[1] = Color.FromArgb(0xFF, Convert5To8((byte)(color2 >> 11)), Convert6To8((byte)(color2 >> 5 & 0x3F)), Convert5To8((byte)(color2 & 0x1F)));
                    intRed = (paletteEntries[0].R + paletteEntries[1].R) / 2;
                    intGreen = (paletteEntries[0].G + paletteEntries[1].G) / 2;
                    intBlue = (paletteEntries[0].B + paletteEntries[1].B) / 2;

                    paletteEntries[2] = Color.FromArgb(0xFF, intRed, intGreen, intBlue);
                    paletteEntries[3] = Color.FromArgb(0, 0, 0, 0);
                }
                else
                {
                    if (color1 == color2)
                    {
                        // 使Color1 > Color2
                        color1 |= 1;
                        color2 &= 0xFFFE;
                    }
                    else if (color1 < color2)
                    {
                        int old = color1;
                        color1 = color2;
                        color2 = old;
                    }

                    paletteEntries[0] = Color.FromArgb(0xFF, Convert5To8((byte)(color1 >> 11)), Convert6To8((byte)(color1 >> 5 & 0x3F)), Convert5To8((byte)(color1 & 0x1F)));
                    paletteEntries[1] = Color.FromArgb(0xFF, Convert5To8((byte)(color2 >> 11)), Convert6To8((byte)(color2 >> 5 & 0x3F)), Convert5To8((byte)(color2 & 0x1F)));

                    intRed = (paletteEntries[0].R * 2 + paletteEntries[1].R) / 3;
                    intGreen = (paletteEntries[0].G * 2 + paletteEntries[1].G) / 3;
                    intBlue = (paletteEntries[0].B * 2 + paletteEntries[1].B) / 3;
                    paletteEntries[2] = Color.FromArgb(0xFF, intRed, intGreen, intBlue);

                    intRed = (paletteEntries[1].R * 2 + paletteEntries[0].R) / 3;
                    intGreen = (paletteEntries[1].G * 2 + paletteEntries[0].G) / 3;
                    intBlue = (paletteEntries[1].B * 2 + paletteEntries[0].B) / 3;
                    paletteEntries[3] = Color.FromArgb(0xFF, intRed, intGreen, intBlue);
                    //intRed = ((paletteEntries[1].R - paletteEntries[0].R) >> 1) - ((paletteEntries[1].R - paletteEntries[0].R) >> 3);
                    //intGreen = ((paletteEntries[1].G - paletteEntries[0].G) >> 1) - ((paletteEntries[1].G - paletteEntries[0].G) >> 3);
                    //intBlue = ((paletteEntries[1].B - paletteEntries[0].B) >> 1) - ((paletteEntries[1].B - paletteEntries[0].B) >> 3);
                    //paletteEntries[2] = Color.FromArgb(0xFF, paletteEntries[0].R + intRed, paletteEntries[0].G + intGreen, paletteEntries[0].B + intBlue);
                    //paletteEntries[3] = Color.FromArgb(0xFF, paletteEntries[1].R - intRed, paletteEntries[1].G - intGreen, paletteEntries[1].B - intBlue);
                }

                // 生成8个字节的子Block的字节数据
                byte[] bySubBlock = new byte[8];
                int bySubBlockIndex = 4;
                //SetPixelByteToList(paletteEntries[0], 0, bySubBlock, 0, "RGB565");
                //SetPixelByteToList(paletteEntries[1], 0, bySubBlock, 2, "RGB565");
                bySubBlock[0] = (byte)(color1 >> 8);
                bySubBlock[1] = (byte)(color1 & 0xFF);
                bySubBlock[2] = (byte)(color2 >> 8);
                bySubBlock[3] = (byte)(color2 & 0xFF);
                
                // 保存16个像素的index
                for (int i = index; i <= index + 15;)
                {
                    byte byColorIndex = 0;
                    for (int j = 3; j >= 0; j--)
                    {
                        byColorIndex = (byte)(byColorIndex | (GetCmpColorIndex(paletteEntries, blockColor[i++], isRTrans) << (j * 2)));
                    }

                    bySubBlock[bySubBlockIndex++] = byColorIndex;
                }

                // 设置数据
                Array.Copy(bySubBlock, 0, byBlock, (index / 16) * 8, bySubBlock.Length);
                index += 16;
            }

            return byBlock;
        }

        /// <summary>
        /// 取得当前颜色的索引
        /// </summary>
        /// <param name="paletteEntries"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        private static int GetCmpColorIndex(Color[] paletteEntries, Color color, bool isRTrans)
        {
            if (isRTrans)
            {
                if (color.A == 0)
                {
                    return 3;
                }
                else
                {
                    int retVal = 0;
                    int diff1 = CalcDistance(paletteEntries[0], color);
                    int diff2 = CalcDistance(paletteEntries[1], color);
                    if (diff1 > diff2)
                    {
                        diff1 = diff2;
                        retVal = 1;
                    }
                    diff2 = CalcDistance(paletteEntries[2], color);
                    if (diff1 > diff2)
                    {
                        retVal = 2;
                    }
                    return retVal;
                }
            }
            else
            {
                int retVal = 0;
                int diff1 = CalcDistance(paletteEntries[0], color);
                int diff2 = CalcDistance(paletteEntries[1], color);
                if (diff1 > diff2)
                {
                    diff1 = diff2;
                    retVal = 1;
                }
                diff2 = CalcDistance(paletteEntries[2], color);
                if (diff1 > diff2)
                {
                    retVal = 2;
                }
                diff2 = CalcDistance(paletteEntries[3], color);
                if (diff1 > diff2)
                {
                    retVal = 3;
                }
                return retVal;
            }
        }

        /// <summary>
        /// 计算两个颜色的差值
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        private static int CalcDistance(Color c1, Color c2)
        {
            return Math.Abs(c1.R - c2.R) + Math.Abs(c1.G - c2.G) + Math.Abs(c1.B - c2.B);
        }

        /// <summary>
        /// 取得子Block的最合适的两个颜色值
        /// </summary>
        /// <param name="blockColor"></param>
        /// <param name="blockIndex"></param>
        /// <returns></returns>
        private static int[] GetRangeValue(List<Color> blockColor, int blockIndex, bool isRTrans)
        {
            int[] rangeValue = new int[2];

            int max_dist = int.MaxValue;
            Color best0 = blockColor[0];
            Color best1 = blockColor[1];

            if (isRTrans)
            {
                // 存在透明像素，只需要三个像素
                for (int i = 0; i < blockColor.Count; i++)
                {
                    Color color1 = blockColor[i];
                    for (int j = i + 1; j < blockColor.Count; j++)
                    {
                        Color color2 = blockColor[j];
                        Color color3 = Color.FromArgb((color1.R + color2.R) / 2, (color1.G + color2.G) / 2, (color1.B + color2.B) / 2);

                        // 计算所有像素与这三个像素的最小值
                        int dist = 0;
                        for (int k = 0; k < blockColor.Count; k++)
                        {
                            if (blockColor[k].A != 0)
                            {
                                int d0 = CalcDistance(blockColor[k], color1);
                                int d1 = CalcDistance(blockColor[k], color2);
                                int d2 = CalcDistance(blockColor[k], color3);

                                if (d0 <= d1)
                                {
                                    dist += d0 < d2 ? d0 : d2;
                                }
                                else
                                {
                                    dist += d1 < d2 ? d1 : d2;
                                }
                            }
                        }

                        // 取最小值
                        if (max_dist > dist)
                        {
                            max_dist = dist;
                            best0 = color1;
                            best1 = color2;
                        }
                    }
                }
            }
            else
            {
                // 不存在透明像素，需要四个像素
                for (int i = 0; i < blockColor.Count; i++)
                {
                    Color color1 = blockColor[i];
                    for (int j = i + 1; j < blockColor.Count; j++)
                    {
                        Color color2 = blockColor[j];
                        Color color3 = Color.FromArgb((color1.R * 2 + color2.R) / 3, (color1.G * 2 + color2.G) / 3, (color1.B * 2 + color2.B) / 3);
                        Color color4 = Color.FromArgb((color1.R + color2.R * 2) / 3, (color1.G + color2.G * 2) / 3, (color1.B + color2.B * 2) / 3);

                        // 计算所有像素与这四个像素的最小值
                        int dist = 0;
                        for (int k = 0; k < blockColor.Count; k++)
                        {
                            int d0 = CalcDistance(blockColor[k], color1);
                            int d1 = CalcDistance(blockColor[k], color2);
                            int d2 = CalcDistance(blockColor[k], color3);
                            int d3 = CalcDistance(blockColor[k], color4);

                            if (d0 <= d1)
                            {
                                if (d2 <= d3)
                                    dist += d0 < d2 ? d0 : d2;
                                else
                                    dist += d0 < d3 ? d0 : d3;
                            }
                            else
                            {
                                if (d2 <= d3)
                                    dist += d1 <= d2 ? d1 : d2;
                                else
                                    dist += d1 <= d3 ? d1 : d3;
                            }  
                        }

                        // 取最小值
                        if (max_dist > dist)
                        {
                            max_dist = dist;
                            best0 = color1;
                            best1 = color2;
                        }
                    }
                }
            }

            rangeValue[0] = GetRGB565Value(best0);
            rangeValue[1] = GetRGB565Value(best1);
            return rangeValue;
        }

        /// <summary>
        /// 将RGB565颜色转换成整数
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private static int GetRGB565Value(Color color)
        {
            return (Convert8To5(color.R) << 11)
                | (Convert8To6(color.G) << 5)
                | Convert8To5(color.B);
        }

        #region " Copy From PuyoTools VrTextureEncoder(效果和原来的一样......) "

        /// <summary>
        /// 图片转成字节数据
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private static byte[] BitmapToRaw(Bitmap source)
        {
            Bitmap img = source;
            byte[] destination = new byte[img.Width * img.Height * 4];

            // If this is not a 32-bit ARGB bitmap, convert it to one
            if (img.PixelFormat != PixelFormat.Format32bppArgb)
            {
                Bitmap newImage = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(newImage))
                {
                    g.DrawImage(img, 0, 0, img.Width, img.Height);
                }
                img = newImage;
            }

            // Copy over the data to the destination. It's ok to do it without utilizing Stride
            // since each pixel takes up 4 bytes (aka Stride will always be equal to Width)
            BitmapData bitmapData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadOnly, img.PixelFormat);
            Marshal.Copy(bitmapData.Scan0, destination, 0, destination.Length);
            img.UnlockBits(bitmapData);

            return destination;
        }

        /// <summary>
        /// Cmpr类型图片导入
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static byte[] CmprImageEncode2(Bitmap source)
        {
            byte[] input = Util.BitmapToRaw(source);
            int width = source.Width;
            int height = source.Height;
            int offset = 0;
            byte[] output = new byte[width * height / 2];

            byte[] subBlock;
            byte[] result;

            result = new byte[32];
            subBlock = new byte[64];

            for (int y = 0; y < height; y += 8)
            {
                for (int x = 0; x < width; x += 8)
                {
                    for (int y2 = 0; y2 < 8; y2 += 4)
                    {
                        for (int x2 = 0; x2 < 8; x2 += 4)
                        {
                            int i = 0;

                            for (int y3 = 0; y3 < 4; y3++)
                            {
                                for (int x3 = 0; x3 < 4; x3++)
                                {
                                    subBlock[i + 3] = input[((((y + y2 + y3) * width) + (x + x2 + x3)) * 4) + 3];
                                    subBlock[i + 2] = input[((((y + y2 + y3) * width) + (x + x2 + x3)) * 4) + 2];
                                    subBlock[i + 1] = input[((((y + y2 + y3) * width) + (x + x2 + x3)) * 4) + 1];
                                    subBlock[i + 0] = input[((((y + y2 + y3) * width) + (x + x2 + x3)) * 4) + 0];

                                    i += 4;
                                }
                            }

                            ConvertBlockToQuaterCmpr(subBlock).CopyTo(output, offset);
                            offset += 8;
                        }
                    }
                }
            }

            return output;
        }

        // Methods below from CTools Wii
        private static byte[] ConvertBlockToQuaterCmpr(byte[] block)
        {
            int col1, col2, dist, temp;
            bool alpha;
            byte[][] palette;
            byte[] result;

            dist = col1 = col2 = -1;
            alpha = false;
            result = new byte[8];

            for (int i = 0; i < 15; i++)
            {
                if (block[i * 4 + 3] < 16)
                    alpha = true;
                else
                {
                    for (int j = i + 1; j < 16; j++)
                    {
                        temp = Distance(block, i * 4, block, j * 4);

                        if (temp > dist)
                        {
                            dist = temp;
                            col1 = i;
                            col2 = j;
                        }
                    }
                }
            }

            if (dist == -1)
            {
                palette = new byte[][] { new byte[] { 0, 0, 0, 0xff }, new byte[] { 0xff, 0xff, 0xff, 0xff }, null, null };
            }
            else
            {
                palette = new byte[4][];
                palette[0] = new byte[4];
                palette[1] = new byte[4];

                Array.Copy(block, col1 * 4, palette[0], 0, 3);
                palette[0][3] = 0xff;
                Array.Copy(block, col2 * 4, palette[1], 0, 3);
                palette[1][3] = 0xff;

                if (palette[0][0] >> 3 == palette[1][0] >> 3 && palette[0][1] >> 2 == palette[1][1] >> 2 && palette[0][2] >> 3 == palette[1][2] >> 3)
                    if (palette[0][0] >> 3 == 0 && palette[0][1] >> 2 == 0 && palette[0][2] >> 3 == 0)
                        palette[1][0] = palette[1][1] = palette[1][2] = 0xff;
                    else
                        palette[1][0] = palette[1][1] = palette[1][2] = 0x0;
            }

            result[0] = (byte)(palette[0][2] & 0xf8 | palette[0][1] >> 5);
            result[1] = (byte)(palette[0][1] << 3 & 0xe0 | palette[0][0] >> 3);
            result[2] = (byte)(palette[1][2] & 0xf8 | palette[1][1] >> 5);
            result[3] = (byte)(palette[1][1] << 3 & 0xe0 | palette[1][0] >> 3);

            if ((result[0] > result[2] || (result[0] == result[2] && result[1] >= result[3])) == alpha)
            {
                Array.Copy(result, 0, result, 4, 2);
                Array.Copy(result, 2, result, 0, 2);
                Array.Copy(result, 4, result, 2, 2);

                palette[2] = palette[0];
                palette[0] = palette[1];
                palette[1] = palette[2];
            }

            if (!alpha)
            {
                palette[2] = new byte[] { (byte)(((palette[0][0] << 1) + palette[1][0]) / 3), (byte)(((palette[0][1] << 1) + palette[1][1]) / 3), (byte)(((palette[0][2] << 1) + palette[1][2]) / 3), 0xff };
                palette[3] = new byte[] { (byte)((palette[0][0] + (palette[1][0] << 1)) / 3), (byte)((palette[0][1] + (palette[1][1] << 1)) / 3), (byte)((palette[0][2] + (palette[1][2] << 1)) / 3), 0xff };
            }
            else
            {
                palette[2] = new byte[] { (byte)((palette[0][0] + palette[1][0]) >> 1), (byte)((palette[0][1] + palette[1][1]) >> 1), (byte)((palette[0][2] + palette[1][2]) >> 1), 0xff };
                palette[3] = new byte[] { 0, 0, 0, 0 };
            }

            for (int i = 0; i < block.Length >> 4; i++)
            {
                result[4 + i] = (byte)(LeastDistance(palette, block, i * 16 + 0) << 6 | LeastDistance(palette, block, i * 16 + 4) << 4 | LeastDistance(palette, block, i * 16 + 8) << 2 | LeastDistance(palette, block, i * 16 + 12));
            }

            return result;
        }
        private static int LeastDistance(byte[][] palette, byte[] colour, int offset)
        {
            int dist, best, temp;

            if (colour[offset + 3] < 8)
                return 3;

            dist = int.MaxValue;
            best = 0;

            for (int i = 0; i < palette.Length; i++)
            {
                if (palette[i][3] != 0xff)
                    break;

                temp = Distance(palette[i], 0, colour, offset);

                if (temp < dist)
                {
                    if (temp == 0)
                        return i;

                    dist = temp;
                    best = i;
                }
            }

            return best;
        }
        private static int Distance(byte[] colour1, int offset1, byte[] colour2, int offset2)
        {
            int temp, val;

            temp = 0;

            for (int i = 0; i < 3; i++)
            {
                val = colour1[offset1 + i] - colour2[offset2 + i];
                temp += val * val;
            }

            return temp;
        }

        #endregion

        /// <summary>
        /// 根据图像种类取得相应图像的宽和高
        /// </summary>
        /// <param name="imageFormat">图像种类</param>
        /// <returns>Block的高、宽数组</returns>
        public static int[] GetBlockWidthHeight(string imageFormat)
        {
            int[] pixelHeightWidth = new int[2];
            switch (imageFormat)
            {
                case "I4":
                    pixelHeightWidth[0] = 8;
                    pixelHeightWidth[1] = 8;
                    break;

                case "I8":
                    pixelHeightWidth[0] = 4;
                    pixelHeightWidth[1] = 8;
                    break;

                case "IA4":
                    pixelHeightWidth[0] = 4;
                    pixelHeightWidth[1] = 8;
                    break;

                case "IA8":
                    pixelHeightWidth[0] = 4;
                    pixelHeightWidth[1] = 4;
                    break;

                case "RGB565":
                    pixelHeightWidth[0] = 4;
                    pixelHeightWidth[1] = 4;
                    break;

                case "RGB5A3":
                    pixelHeightWidth[0] = 4;
                    pixelHeightWidth[1] = 4;
                    break;

                case "RGBA32_RGBA8":
                    pixelHeightWidth[0] = 4;
                    pixelHeightWidth[1] = 4;
                    break;

                case "C4_CI4":
                    pixelHeightWidth[0] = 8;
                    pixelHeightWidth[1] = 8;
                    break;

                case "C8_CI8":
                    pixelHeightWidth[0] = 4;
                    pixelHeightWidth[1] = 8;
                    break;

                case "C14X2_CI14x2":
                    pixelHeightWidth[0] = 4;
                    pixelHeightWidth[1] = 4;
                    break;

                case "CMPR":
                    pixelHeightWidth[0] = 8;
                    pixelHeightWidth[1] = 8;
                    break;
            }

            return pixelHeightWidth;
        }

        /// <summary>
        /// 取得字体文件的Image格式
        /// </summary>
        /// <param name="imageFormat">Image格式</param>
        /// <returns>Image格式</returns>
        public static string GetImageFormat(int imageFormat)
        {
            switch (imageFormat)
            {
                case 0x00:
                    return "I4";

                case 0x01:
                    return "I8";

                case 0x02:
                    return "IA4";

                case 0x03:
                    return "IA8";

                case 0x04:
                    return "RGB565";

                case 0x05:
                    return "RGB5A3";

                case 0x06:
                    return "RGBA32_RGBA8";

                case 0x08:
                    return "C4_CI4";

                case 0x09:
                    return "C8_CI8";

                case 0x0a:
                    return "C14X2_CI14x2";

                case 0x0e:
                    return "CMPR";
            }

            return "UNKNOWN";
        }

        /// <summary>
        /// 取得图片的字节数据数量
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static int GetImageByteCount(int height, int width, string format)
        {
            // 总体的像素
            int intTotalPixel = height * width;

            // 取得一个Block的基本信息
            int[] blockHeightWidth = Util.GetBlockWidthHeight(format);
            int blockPixel = blockHeightWidth[0] * blockHeightWidth[1];

            // 计算需要的Block数
            int intWidthBlockNum = width / blockHeightWidth[1];
            if ((width % blockHeightWidth[1]) != 0)
            {
                intWidthBlockNum += 1;
            }
            int intHeightBlockNum = height / blockHeightWidth[0];
            if ((height % blockHeightWidth[0]) != 0)
            {
                intHeightBlockNum += 1;
            }

            // 计算需要的像素
            int pixelNum = intWidthBlockNum * intHeightBlockNum * blockPixel;

            switch (format)
            {
                case "I4":
                    return pixelNum / 2;

                case "I8":
                    return pixelNum;

                case "IA4":
                    return pixelNum;

                case "IA8":
                    return pixelNum * 2;

                case "RGB565":
                    return pixelNum * 2;

                case "RGB5A3":
                    return pixelNum * 2;

                case "RGBA32_RGBA8":
                    return pixelNum * 4;

                case "C4_CI4":
                    return pixelNum / 2;

                case "C8_CI8":
                    return pixelNum;

                case "C14X2_CI14x2":
                    return pixelNum * 2;

                case "CMPR":
                    return pixelNum / 2;
            }

            return 0;
        }

        /// <summary>
        /// 取得图片的字节数据数量
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static int GetImageByteCountNoBlock(int height, int width, string format)
        {
            // 总体的像素
            int intTotalPixel = height * width;

            switch (format)
            {
                case "I4":
                    return intTotalPixel / 2;

                case "I8":
                    return intTotalPixel;

                case "IA4":
                    return intTotalPixel;

                case "IA8":
                    return intTotalPixel * 2;

                case "RGB565":
                    return intTotalPixel * 2;

                case "RGB5A3":
                    return intTotalPixel * 2;

                case "RGBA32_RGBA8":
                    return intTotalPixel * 4;

                case "C4_CI4":
                    return intTotalPixel / 2;

                case "C8_CI8":
                    return intTotalPixel;

                case "C14X2_CI14x2":
                    return intTotalPixel * 2;

                case "CMPR":
                    return intTotalPixel / 2;
            }

            return 0;
        }

        /// <summary>
        /// 取得调色板格式图像的名称
        /// </summary>
        /// <param name="paletteFormat"></param>
        /// <returns></returns>
        public static string GetPaletteFormat(int paletteFormat)
        {
            switch (paletteFormat)
            {
                case 0:
                    return "IA8";

                case 1:
                    return "RGB565";

                case 2:
                    return "RGB5A3";
            }

            return string.Empty;
        }

        /// <summary>
        /// 将一个空像素拆解成相应的字节数据
        /// </summary>
        /// <param name="pixel"></param>
        /// <param name="imageByteList"></param>
        /// <param name="imageFormat"></param>
        public static int SetNullPixelByteToList(int pixelNum, byte[] imageData, int byIndex, string imageFormat)
        {
            // 取得当前像素对应的字节数据

            switch (imageFormat)
            {
                case "I4":
                case "CMPR":
                    if (pixelNum % 2 == 0)
                    {
                        // 设置前4位
                        imageData[byIndex] = (byte)(imageData[byIndex] & 0x0F);
                    }
                    else
                    {
                        // 设置后4位
                        imageData[byIndex] = (byte)(imageData[byIndex] & 0xF0);
                        byIndex++;
                    }
                    break;

                case "I8":
                    imageData[byIndex++] = 0;
                    break;

                case "IA4":
                    imageData[byIndex++] = 0;
                    break;

                case "IA8":
                    imageData[byIndex++] = 0;
                    imageData[byIndex++] = 0;
                    break;

                case "RGB565":
                    imageData[byIndex++] = 0;
                    imageData[byIndex++] = 0;
                    break;

                case "RGB5A3":
                    imageData[byIndex++] = 0;
                    imageData[byIndex++] = 0;
                    break;

                case "RGBA32_RGBA8":
                    //int blockStartNum = pixelNum / 16 * 32;
                    //imageData[blockStartNum + pixelNum * 2] = 0;
                    //imageData[blockStartNum + pixelNum * 2 + 1] = 0;
                    //imageData[blockStartNum + pixelNum * 2 + 32] = 0;
                    //imageData[blockStartNum + pixelNum * 2 + 32 + 1] = 0;
                    byIndex += 2;
                    break;

                case "C4_CI4":
                    if (pixelNum % 2 != 0)
                    {
                        byIndex++;
                    }
                    break;

                case "C8_CI8":
                    byIndex++;
                    break;

                case "C14X2_CI14x2":
                    byIndex += 2;
                    break;
            }

            return byIndex;
        }

        /// <summary>
        /// 将一个像素拆解成相应的字节数据（TODO）
        /// </summary>
        /// <param name="pixel"></param>
        /// <param name="imageByteList"></param>
        /// <param name="imageFormat"></param>
        public static int SetPixelByteToList(Color pixel, int pixelNum, byte[] imageData, int byIndex, string imageFormat)
        {
            // 取得当前像素对应的字节数据
            byte intColor;
            byte intAlpha = pixel.A;
            byte intRed = pixel.R;
            byte intGreen = pixel.G;
            byte intBlue = pixel.B;

            switch (imageFormat)
            {
                case "I4":
                    if (pixelNum % 2 == 0)
                    {
                        // 设置前4位
                        imageData[byIndex] = (byte)(intRed & 0xF0);
                    }
                    else
                    {
                        // 设置后4位
                        intColor = imageData[byIndex];
                        imageData[byIndex] = (byte)(intColor | (intRed & 0xF0 >> 4));
                        byIndex++;
                    }
                    break;

                case "I8":
                    imageData[byIndex++] = intRed;
                    break;

                case "IA4":
                    imageData[byIndex++] = (byte)((intAlpha & 0xF0) | ((intRed & 0xF0) >> 4));
                    break;

                case "IA8":
                    imageData[byIndex++] = intAlpha;
                    imageData[byIndex++] = intRed;
                    break;

                case "RGB565":
                    imageData[byIndex++] = (byte)((intRed >> 3 << 3) | (intGreen >> 5));
                    imageData[byIndex++] = (byte)((((intGreen >> 2) & 0x7) << 5) | (intBlue >> 3));
                    break;

                case "RGB5A3":
                    if (intAlpha > 0xDA)
                    {
                        // not use alpha 5,5,5
                        UInt16 temp = (UInt16)(Convert8To5(intRed) << 10 | Convert8To5(intGreen) << 5 | Convert8To5(intBlue));
                        imageData[byIndex++] = (byte)((temp >> 8) | 0x80);
                        imageData[byIndex++] = (byte)(temp & 0xFF);
                    }
                    else
                    {
                        // use alpha 3,4,4,4
                        imageData[byIndex++] = (byte)((Convert8To3(intAlpha) << 4 | Convert8To4(intRed)) & 0x7F);
                        imageData[byIndex++] = (byte)(Convert8To4(intGreen) << 4 | Convert8To4(intBlue));
                    }
                    break;

                case "RGBA32_RGBA8":
                    //int blockStartNum = pixelNum / 16 * 32;
                    //imageData[blockStartNum + pixelNum * 2] = intAlpha;
                    //imageData[blockStartNum + pixelNum * 2 + 1] = intRed;
                    //imageData[blockStartNum + pixelNum * 2 + 32] = intGreen;
                    //imageData[blockStartNum + pixelNum * 2 + 32 + 1] = intBlue;
                    imageData[byIndex] = intAlpha;
                    imageData[byIndex + 1] = intRed;
                    imageData[byIndex + 32] = intGreen;
                    imageData[byIndex + 32 + 1] = intBlue;
                    byIndex += 2;
                    break;
            }

            return byIndex;
        }

        /// <summary>
        /// 保存指定质量的Jpeg图片
        /// </summary>
        /// <param name="sourceImage"></param>
        /// <param name="savePath"></param>
        /// <param name="imageQualityValue"></param>
        /// <returns></returns>
        public static bool EncodeJpgImage(System.Drawing.Image sourceImage, byte[] byTarget, int imageQualityValue)
        {
            // 以下代码为保存图片时，设置压缩质量
            EncoderParameters encoderParameters = new EncoderParameters();
            EncoderParameter encoderParameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, imageQualityValue);
            encoderParameters.Param[0] = encoderParameter;
            MemoryStream ms = null;

            try
            {
                ImageCodecInfo[] ImageCodecInfoArray = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegImageCodecInfo = null;
                for (int i = 0; i < ImageCodecInfoArray.Length; i++)
                {
                    if (ImageCodecInfoArray[i].FormatDescription.Equals("JPEG"))
                    {
                        jpegImageCodecInfo = ImageCodecInfoArray[i];
                        break;
                    }
                }

                ms = new MemoryStream();
                ms.Position = 0;
                sourceImage.Save(ms, jpegImageCodecInfo, encoderParameters);
                //sourceImage.Dispose();

                byte[] byImp = ms.ToArray();
                if (byImp.Length > byTarget.Length)
                {
                    throw new Exception("导入图片和原图片大小不一致");
                }
                else
                {
                    Array.Copy(byImp, 0, byTarget, 0, byImp.Length);
                }

                return true;
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
                return false;
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
        /// 图片转换成N位颜色的索引图片
        /// </summary>
        /// <param name="source"></param>
        /// <param name="maxColors"></param>
        /// <param name="paletteColor"></param>
        /// <returns></returns>
        public static unsafe byte[] BitmapToRawIndexed(Bitmap source, int maxColors, out int[] palette)
        {
            Bitmap img = source;
            byte[] destination = new byte[img.Width * img.Height];

            // If this is not a 32-bit ARGB bitmap, convert it to one
            if (img.PixelFormat != PixelFormat.Format32bppArgb)
            {
                Bitmap newImage = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(newImage))
                {
                    g.DrawImage(img, 0, 0, img.Width, img.Height);
                }
                img = newImage;
            }

            // Quantize the image
            WuQuantizer quantizer = new WuQuantizer();
            img = (Bitmap)quantizer.QuantizeImage(img, maxColors);

            // Copy over the data to the destination. We need to use Stride in this case, as it may not
            // always be equal to Width.
            BitmapData bitmapData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadOnly, img.PixelFormat);

            byte* pointer = (byte*)bitmapData.Scan0;
            for (int y = 0; y < bitmapData.Height; y++)
            {
                for (int x = 0; x < bitmapData.Width; x++)
                {
                    destination[(y * img.Width) + x] = pointer[(y * bitmapData.Stride) + x];
                }
            }

            img.UnlockBits(bitmapData);

            // Copy over the palette
            palette = new int[maxColors];
            for (int i = 0; i < maxColors; i++)
            {
                palette[i] = img.Palette.Entries[i].ToArgb();
            }

            return destination;
        }

        #endregion

        #region " 通常共通 "

        /// <summary>
        /// 改变目录
        /// </summary>
        /// <param name="baseFolder"></param>
        /// <param name="gameName"></param>
        /// <returns></returns>
        public static string ChgToGitHubPath(string baseFolder, string gameName)
        {
            return (baseFolder.Replace(gameName, "") + @"HanhuaProject\" + gameName);
        }

        /// <summary>
        /// 判断两个文件是否相同
        /// </summary>
        /// <param name="fileA"></param>
        /// <param name="fileB"></param>
        /// <returns></returns>
        public static int isFilesSame(string fileA, string fileB)
        {
            FileStream fsA = File.OpenRead(fileA);
            FileStream fsB = File.OpenRead(fileB);
            BufferedStream fsBufA = new BufferedStream(fsA, COPY_BLOCK);
            BufferedStream fsBufB = new BufferedStream(fsB, COPY_BLOCK);
            int sameLen = 0;

            using (fsA)
            {
                using (fsB)
                {
                    using (fsBufA)
                    {
                        using (fsBufB)
                        {
                            if (fsA.Length != fsB.Length)
                            {
                                return 0;
                            }

                            long len = fsA.Length;
                            fsA.Seek(0, SeekOrigin.Begin);
                            fsB.Seek(0, SeekOrigin.Begin);

                            while (len-- > 0)
                            {
                                if (fsBufA.ReadByte() != fsBufB.ReadByte())
                                {
                                    return sameLen;
                                }
                                sameLen++;
                            }
                        }
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// 从包括路径的文件名中取得文件名
        /// </summary>
        /// <param name="fullFileName"></param>
        /// <returns></returns>
        public static string GetShortFileName(string fullFileName)
        {
            if (string.IsNullOrEmpty(fullFileName))
            {
                return string.Empty;
            }

            string[] strNames = fullFileName.Split('\\');
            return strNames[strNames.Length - 1];
        }

        /// <summary>
        /// 生成一级汉字
        /// </summary>
        /// <returns></returns>
        public static string CreateOneLevelHanzi()
        {
            List<byte> hanziByteList = new List<byte>();
            // 国标一级字(共3755个): 区:16-55, 位:01-94, 55区最后5位为空位
            for (int x = 16; x <= 55; x++)
            {
                for (int y = 1; y <= 94; y++)
                {
                    if (x == 55 && y >= 89)
                    {
                        break;
                    }
                    hanziByteList.Add((byte)(x + 0xA0));
                    hanziByteList.Add((byte)(y + 0xA0));
                }
            }

            //return Encoding.GetEncoding("GB2312").GetString(hanziByteList.ToArray()) + "弩驽浣蝙蝠圣阱悚蚯蚓骼蜷鳄桥蟑螂蜻蜓骼魅";
            return Encoding.GetEncoding("GB2312").GetString(hanziByteList.ToArray());
        }

        /// <summary>
        /// 生成二级汉字
        /// </summary>
        /// <returns></returns>
        public static string CreateTwoLevelHanzi()
        {
            List<byte> hanziByteList = new List<byte>();
            // 国标二级汉字(共3008个): 区:56-87, 位:01-94
            for (int x = 56; x <= 87; x++)
            {
                for (int y = 1; y <= 94; y++)
                {
                    hanziByteList.Add((byte)(x + 0xA0));
                    hanziByteList.Add((byte)(y + 0xA0));
                }
            }

            return Encoding.GetEncoding("GB2312").GetString(hanziByteList.ToArray());
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="cmap1"></param>
        /// <param name="cmap2"></param>
        /// <returns></returns>
        public static int Comparison(KeyValuePair<int, int> cmap1, KeyValuePair<int, int> cmap2)
        {
            return cmap1.Key - cmap2.Key;
        }

        /// <summary>
        /// 根据Utf8字符的编码数字取得相应的字符
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GetUtf8StrFromNumber(int number)
        {
            byte[] charByte = new byte[1];

            if (number <= 127)
            {
                // 1字节 0xxxxxxx
                charByte = new byte[] { (byte)number };
            }
            else if (number > 127 && number <= 2047)
            {
                // 2字节 110xxxxx 10xxxxxx
                charByte = new byte[] { (byte)(((number >> 6) & 31) + 192), (byte)((number & 63) + 128) };
            }
            else if (number > 2047 && number <= 65535)
            {
                // 3字节 1110xxxx 10xxxxxx 10xxxxxx
                charByte = new byte[] { (byte)(((number >> 12) & 15) + 224), (byte)(((number >> 6) & 63) + 128), (byte)((number & 63) + 128) };
            }

            return Encoding.UTF8.GetString(charByte);
        }

        /// <summary>
        /// 根据Shift-jis字符的编码数字取得相应的字符
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GetShiftJisStrFromNumber(int number)
        {
            byte[] charByte;
            if (number <= 0xFF)
            {
                charByte = new byte[] { (byte)(number & 0xFF) };
            }
            else
            {
                charByte = new byte[] { (byte)((number >> 8) & 0xFF), (byte)(number & 0xFF) };
            }

            return Encoding.GetEncoding("Shift-Jis").GetString(charByte);
        }

        /// <summary>
        /// 根据Utf16字符的编码数字取得相应的字符
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GetUtf16StrFromNumber(int number, string endianess)
        {
            byte[] charByte;
            if (number <= 0xFF)
            {
                charByte = new byte[] { (byte)(number & 0xFF) };
            }
            else
            {
                charByte = new byte[] { (byte)((number >> 8) & 0xFF), (byte)(number & 0xFF) };
            }

            if ("FFFE".Equals(endianess.ToUpper()))
            {
                return Encoding.BigEndianUnicode.GetString(charByte);
            }
            else
            {
                return Encoding.Unicode.GetString(charByte);
            }
        }

        /// <summary>
        /// 根据字符的编码数字取得相应的字符
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GetStrFromNumber(int number, int encoding, string endianess)
        {
            switch (encoding)
            {
                case 0:
                    return Util.GetUtf8StrFromNumber(number);

                case 1:
                    return Util.GetUtf16StrFromNumber(number, endianess);

                case 2:
                    return Util.GetShiftJisStrFromNumber(number);

                case 3:
                    return Encoding.ASCII.GetString(new byte[] { (byte)number} );

                default:
                    return Util.GetUtf8StrFromNumber(number);
            }
        }

        /// <summary>
        /// 根据汉字取得当前汉字的Unicode编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetUnicodeFromStr(string str)
        {
            byte[] hanziByte = Encoding.UTF8.GetBytes(str);
            
            switch (hanziByte.Length)
            {
                case 1:
                    hanziByte[0] = (byte)(hanziByte[0] & 0x7F);
                    return hanziByte[0];

                case 2:
                    hanziByte[0] = (byte)(hanziByte[0] >> 2 & 0x07);
                    hanziByte[1] = (byte)((hanziByte[0] & 0x3) << 6 | (hanziByte[1] & 0x3F));
                    return Util.GetOffset(hanziByte, 0, 1);

                case 3:
                    hanziByte[0] = (byte)((hanziByte[0] & 0xF) << 4| (hanziByte[1] >> 2 & 0x0F));
                    hanziByte[1] = (byte)((hanziByte[1] & 0x3) << 6 | (hanziByte[2] & 0x3F));
                    return Util.GetOffset(hanziByte, 0, 1);
            }

            return Util.GetOffset(hanziByte, 0, hanziByte.Length - 1);
        }

        /// <summary>
        /// 3位的字节数据变成8位
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte Convert3To8(byte value)
        {
            // Swizzle bits: 00000123 -> 12312312
            return (byte)((value << 5) | (value << 2) | (value >> 1));
        }
        
        /// <summary>
        /// 8位的字节数据变成3位
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte Convert8To3(byte value)
        {
            // Swizzle bits: 12312312 -> 00000123
            return (byte)(value >> 5);
        }

        /// <summary>
        /// 4位的字节数据变成8位
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte Convert4To8(byte value)
        {
            // Swizzle bits: 00001234 -> 12341234
            return (byte)((value << 4) | value);
        }

        /// <summary>
        /// 8位的字节数据变成4位
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte Convert8To4(byte value)
        {
            // Swizzle bits: 12341234 -> 00001234
            return (byte)(value >> 4);
        }

        /// <summary>
        /// 5位的字节数据变成8位
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte Convert5To8(byte value)
        {
            // Swizzle bits: 00012345 -> 12345123
            return (byte)((value << 3) | (value >> 2));
        }

        /// <summary>
        /// 8位的字节数据变成5位
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte Convert8To5(byte value)
        {
            // Swizzle bits: 12345123 -> 00012345
            return (byte)(value >> 3);
        }

        /// <summary>
        /// 6位的字节数据变成8位
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte Convert6To8(byte value)
        {
            // Swizzle bits: 00123456 -> 12345612
            return (byte)((value << 2) | (value >> 4));
        }

        /// <summary>
        /// 8位的字节数据变成6位
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte Convert8To6(byte value)
        {
            // Swizzle bits: 12345612 -> 00123456
            return (byte)(value >> 2);
        }

        /// <summary>
        /// 根据开始、结束位置取得字节数组中的offset
        /// </summary>
        /// <param name="byData">字节数组</param>
        /// <param name="startPos">开始字节位置</param>
        /// <param name="endPos">结束字节位置</param>
        /// <returns></returns>
        public static int GetOffset(byte[] byData, int startPos, int endPos)
        {
            int intRetValue = 0;
            int intBytePos = endPos - startPos;

            for (int i = startPos; i <= endPos; i++)
            {
                intRetValue += (int)((uint)(byData[i]) << (intBytePos * 8));
                intBytePos--;
            }

            return intRetValue;
        }

        /// <summary>
        /// 取得字库编码格式
        /// </summary>
        /// <param name="encodeing"></param>
        /// <returns></returns>
        public static string GetFontEncodingStr(int encodeing)
        {
            switch (encodeing)
            {
                case 1:
                    return "UTF-16BE";

                case 2:
                    return "SJIS";

                case 3:
                    return "windows-1252";

                case 4:
                    return "hex";
            }

            return "UTF-8";
        }

        /// <summary>
        /// 取得字库编码器
        /// </summary>
        /// <param name="encodeing"></param>
        /// <returns></returns>
        public static Encoding GetFontEncoding(int encodeing, string endianess)
        {
            switch (encodeing)
            {
                case 1:
                    if ("FFFE".Equals(endianess.ToUpper()))
                    {
                        return Encoding.BigEndianUnicode;
                    }
                    else
                    {
                        return Encoding.Unicode;
                    }

                case 2:
                    return Encoding.GetEncoding("Shift-Jis");

                case 3:
                    return Encoding.ASCII;

                case 4:
                    MessageBox.Show("不支持这种格式编码！\n暂且使用Utf8编码.");
                    return Encoding.UTF8;
            }

            return Encoding.UTF8;
        }

        /// <summary>
        /// 根据开始、结束位置取得字节数组中的字符串
        /// </summary>
        /// <param name="byData">字节数组</param>
        /// <param name="startPos">开始字节位置</param>
        /// <param name="endPos">结束字节位置</param>
        /// <returns></returns>
        public static string GetBytesString(byte[] byData, int startPos, int endPos)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = startPos; i <= endPos; i++)
            {
                sb.Append(Convert.ToString(byData[i], 16));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 根据开始、结束位置取得字节数组中的字符串
        /// </summary>
        /// <param name="byData">字节数组</param>
        /// <param name="startPos">开始字节位置</param>
        /// <param name="endPos">结束字节位置</param>
        /// <returns></returns>
        public static string GetHeaderString(byte[] byData, int startPos, int endPos)
        {
            return Encoding.GetEncoding(932).GetString(byData, startPos, endPos - startPos + 1);
        }

        /// <summary>
        /// 根据开始、结束位置取得字节数组中的字符串
        /// </summary>
        /// <param name="byData">字节数组</param>
        /// <param name="startPos">开始字节位置</param>
        /// <param name="endPos">结束字节位置</param>
        /// <returns></returns>
        public static string GetHeaderString(byte[] byData, int startPos, int endPos, Encoding encoding)
        {
            return encoding.GetString(byData, startPos, endPos - startPos + 1);
            //byte[] byTxt = new byte[endPos - startPos + 1];
            //Array.Copy(byData, startPos, byTxt, 0, byTxt.Length);
            //return encoding.GetString(byTxt);
        }

        /// <summary>
        /// 将字节数据解码成字符串
        /// </summary>
        /// <param name="byData"></param>
        /// <returns></returns>
        public static string DecodeByteArray(byte[] byData, Decoder decoder)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder endCharSb = new StringBuilder();

            // 将当前文件解码成字符串
            char[] charData = new char[decoder.GetCharCount(byData, 0, byData.Length, true)];
            decoder.GetChars(byData, 0, byData.Length, charData, 0);

            foreach (char itemChar in charData)
            {
                if (itemChar != '\0')
                {
                    if (endCharSb.Length > 0)
                    {
                        sb.Append("^" + endCharSb.ToString().Trim() + "^");
                        endCharSb.Length = 0;
                    }
                    sb.Append(itemChar);
                }
                else
                {
                    endCharSb.Append("0 ");
                }
            }

            if (endCharSb.Length > 0)
            {
                sb.Append("^" + endCharSb.ToString().Trim() + "^");
                endCharSb.Length = 0;
            }

            return sb.ToString().Replace("\n", "^0a^\n").Replace("\r", "^0d^\n");
        }

        /// <summary>
        /// 将字节数据解码成字符串
        /// </summary>
        /// <param name="byData"></param>
        /// <returns></returns>
        public static string EncodeByteArray(byte[] byData, Encoding encoding)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder endCharSb = new StringBuilder();

            // 将当前文件解码成字符串
            byte byCur;
            for (int i = 0; i < byData.Length; i++)
            {
                byCur = byData[i];
                if (byCur == 0)
                {
                    sb.Append("^0^");
                }
                else if (byCur >= 0x20 && byCur <= 0x7e)
                {
                    sb.Append(encoding.GetString(new byte[] { byCur }));
                }
                else if ((byCur >= 0x81 && byCur <= 0x9f)
                    || (byCur >= 0xe0 && byCur <= 0xef))
                {
                    sb.Append(encoding.GetString(new byte[] { byCur, byData[i + 1] }));
                    i++;
                }
                else
                {
                    sb.Append("^").Append(byCur.ToString("x").PadLeft(2, '0')).Append("^");
                }
            }

            return sb.ToString().Replace("^0a^", "^0a^\n");
        }

        /// <summary>
        /// 判断字符串是否是数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumber(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }

            try
            {
                Convert.ToDecimal(str);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 去掉文件名中的后缀数字(file_01 -> file)
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string TrimFileNo(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return string.Empty;
            }

            return Regex.Replace(fileName, @"_\d+$", string.Empty);
        }

        /// <summary>
        /// 根据固定长度，重新设置位置
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="fixSize"></param>
        /// <returns></returns>
        public static int ResetPos(int pos, int fixSize)
        {
            int temp = pos % fixSize;
            if (temp > 0)
            {
                return pos + (fixSize - temp);
            }
            else
            {
                return pos;
            }
        }

        #endregion

        #region " 私有方法 "

        /// <summary>
        /// 分析目录
        /// </summary>
        /// <param name="folder">目录</param>
        /// <param name="fileNameInfo">文件信息列表</param>
        /// <param name="folderIndex">目录的级别</param>
        private static void GetAllFilesInfo(string folder, List<FilePosInfo> fileNameInfo, int folderIndex)
        {
            // 追加当前目录
            fileNameInfo.Add(new FilePosInfo(folder, true, folderIndex));
            folderIndex++;

            DirectoryInfo di = new DirectoryInfo(folder);
            FileInfo[] fiList = di.GetFiles(); // 取得当前目录下所有文件
            DirectoryInfo[] diA = di.GetDirectories(); // 取得当前目录下所有目录

            // 追加当前目录的文件
            foreach (FileInfo fi in fiList)
            {
                if (Util.IsGetAllFile || Util.NeedCheckFile(fi.FullName))
                {
                    fileNameInfo.Add(new FilePosInfo(fi.FullName, false, folderIndex));
                }
            }

            // 递归分析当前目录下子目录的文件
            foreach (DirectoryInfo childDi in diA)
            {
                Util.GetAllFilesInfo(childDi.FullName, fileNameInfo, folderIndex);
            }
        }

        /// <summary>
        /// 根据后缀名判断是否需要检查
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>是否需要检查</returns>
        private static bool NeedCheckFile(string fileName)
        {
            string[] paths = fileName.Split('.');
            string endName = paths[paths.Length - 1].ToUpper();

            if (endName == "TPL")
            {
                return Util.NeedCheckTpl;
            }
            else if (Util.notSearchFile.ContainsKey(endName)
                || fileName.IndexOf(@"\audio\") != -1
                || fileName.IndexOf(@"\door\") != -1
                || fileName.IndexOf(@"\movie\") != -1
                || fileName.IndexOf(@"\sound\") != -1
                || fileName.IndexOf(@"\bgm\") != -1)
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