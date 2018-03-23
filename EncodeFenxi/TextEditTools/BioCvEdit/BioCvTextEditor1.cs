using System;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Reflection;
using Hanhua.Common;

namespace Hanhua.BioCvEdit
{
    public partial class BioCvTextEditor1 : BaseForm
    {
        #region " 本地变量 "

        /// <summary>
        /// 记录当前使用字库
        /// </summary>
        private Dictionary<int, string[]> jpFontCharPage = new Dictionary<int,string[]>();

        /// <summary>
        /// 记录当前使用的中文字库
        /// </summary>
        private Dictionary<int, string[]> cnFontCharPage = new Dictionary<int,string[]>();

        /// <summary>
        /// 记录所有文本文件
        /// </summary>
        private List<FilePosInfo> textFiles = new List<FilePosInfo>();

        /// <summary>
        /// 记录Message的Entry
        /// </summary>
        private List<int> textEntrys = new List<int>();

        /// <summary>
        /// 保存旧的文本的长度，为了保存时验证中文的文本长度是否变化
        /// </summary>
        private int oldTextLen = 0;

        /// <summary>
        /// 是ngc还是ps2
        /// </summary>
        private bool isNgc = false;

        #endregion

        /// <summary>
        /// 生化危机维罗妮卡文本编辑器
        /// </summary>
        public BioCvTextEditor1(string folder)
        {
            InitializeComponent();

            this.ResetHeight();

            this.txtCn.OtherRichTextBox = this.txtJp;

            // 判断是Ps2还是Ngc汉化
            if (folder.IndexOf("Ngc", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                this.isNgc = true;
            }

            // 初始化文本开始位置
            this.baseFolder = folder;
            this.InitStartPos(folder);

            // 读取字库信息
            this.ReadFontChar();

            // 选中第一个文件
            this.fileList.SelectedIndex = 0;
        }

        #region " 事件 "

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.Save())
                {
                    MessageBox.Show("保存成功。");
                }
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
            }
        }

        /// <summary>
        /// 文件变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 设置文本
            FilePosInfo filePosInfo = this.textFiles[this.fileList.SelectedIndex];
            this.txtJp.Text = this.ChangeFile(filePosInfo.File, filePosInfo.TextStart, filePosInfo.TextEnd, this.jpFontCharPage);
            this.txtCn.Text = this.ChangeFile(filePosInfo.File + "_cn", filePosInfo.TextStart, filePosInfo.TextEnd, this.cnFontCharPage);
        }

        /// <summary>
        /// 导出文本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            this.ExoprtText();
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImport_Click(object sender, EventArgs e)
        {
            // 打开要导入的文件
            this.baseFile = Util.SetOpenDailog("生化危机维罗妮卡 翻译文件（*.xls）|*.xls", string.Empty);
            if (string.IsNullOrEmpty(this.baseFile))
            {
                return;
            }

            // 设定保存的文件名
            this.ImportText(this.baseFile);
            //this.CheckImportTextCharCount(this.strFileOpen);
        }

        /// <summary>
        /// 根据输入文字转换成字节数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string key = this.txtKey.Text.Trim();
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            char[] keys = key.ToCharArray();
            List<byte> keyByte = new List<byte>();
            foreach (char keyChar in keys)
            {
                byte[] keyValue = this.GetCharIndex(keyChar.ToString());
                keyByte.Add(keyValue[0]);
                keyByte.Add(keyValue[1]);
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < keyByte.Count; i++)
            {
                sb.Append(keyByte[i].ToString("x") + " ");
            }

            this.txtKey.Text = sb.ToString().Trim();
        }

        #endregion

        #region " 私有方法 "

        /// <summary>
        /// 保存文本
        /// </summary>
        /// <returns></returns>
        private bool Save()
        {
            // 长度及关键字检查
            if (!this.CheckCnText())
            {
                return false;
            }

            // 将中文文本转换成二进制数据
            List<byte> cnBytes = new List<byte>();
            if (!this.EncodeCnText(cnBytes, this.txtJp.Text, this.txtCn.Text, this.hasEntry))
            {
                return false;
            }

            // 检查最大的字节数
            int maxByteCount = cnBytes.Count;
            if (this.hasEntry)
            {
                maxByteCount += (this.textEntrys.Count + 1) * 4;
            }
            if (this.oldTextLen < maxByteCount)
            {
                throw new Exception("最大字节数超出 ：" + (maxByteCount - this.oldTextLen));
            }

            // 修正最大长度不足的字节
            if (this.oldTextLen != cnBytes.Count)
            {
                int diffLen = this.oldTextLen - cnBytes.Count;
                if (this.hasEntry)
                {
                    diffLen -= (this.textEntrys.Count + 1) * 4;
                }
                while (diffLen > 0)
                {
                    cnBytes.Add(0);
                    diffLen--;
                }
            }

            // 保存二进制数据
            this.Save(cnBytes, this.hasEntry);

            return true;
        }

        /// <summary>
        /// 初始化文本开始位置
        /// </summary>
        private void InitStartPos(string folder)
        {
            this.fileList.Items.Clear();
            this.textFiles.Clear();

            // 添加sysmes.ald文本
            FilePosInfo posInfo = new FilePosInfo(folder + @"\sysmes.ald");
            posInfo.TextStart = 0;
            posInfo.TextEnd = 0xdc4;

            this.textFiles.Add(posInfo);
            this.fileList.Items.Add("sysmes.ald" + " 0x0--0xdc4");

            posInfo = new FilePosInfo(folder + @"\sysmes.ald");
            posInfo.TextStart = 0xdc4;
            posInfo.SubIndex = "_1";
            if (this.isNgc)
            {
                posInfo.TextEnd = 0x52f0;
            }
            else
            {
                posInfo.TextEnd = 0x4ab8;
            }

            this.textFiles.Add(posInfo);
            this.fileList.Items.Add("sysmes.ald" + " 0xdc4--0x" + posInfo.TextEnd.ToString("x"));
        }

        /// <summary>
        /// 变更文件
        /// </summary>
        /// <param name="file"></param>
        private string ChangeFile(string file, int startPos, int endPos, Dictionary<int, string[]> fontCharPage)
        {
            try
            {
                // 将文件中的数据，循环读取到byData中
                byte[] byData = File.ReadAllBytes(file);

                // 开始解码
                return this.DecodeText(byData, startPos, endPos, fontCharPage);
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
                return string.Empty;
            }
        }

        /// <summary>
        /// 开始解码
        /// </summary>
        /// <param name="byData"></param>
        /// <param name="startPos"></param>
        private string DecodeText(byte[] byData, int startPos, int endPos, Dictionary<int, string[]> fontCharPage)
        {
            StringBuilder sb = new StringBuilder();
            int textCount = 0;
            if (this.isNgc)
            {
                textCount = Util.GetOffset(byData, startPos + 4, startPos + 7);
            }
            else
            {
                textCount = byData[startPos + 4] | (byData[startPos + 5] << 8) | (byData[startPos + 6] << 16) | (byData[startPos + 7] << 24);
            }

            // 如果是Ps2，交换字节位置
            if (!this.isNgc)
            {
                int firstPos = startPos + (byData[startPos + 8] | (byData[startPos + 9] << 8) | (byData[startPos + 10] << 16) | (byData[startPos + 11] << 24)) + 4;
                for (int i = startPos; i < firstPos; i += 4)
                {
                    byte temp = byData[i];
                    byData[i] = byData[i + 3];
                    byData[i + 3] = temp;

                    temp = byData[i + 1];
                    byData[i + 1] = byData[i + 2];
                    byData[i + 2] = temp;
                }

                for (int i = firstPos; i < endPos; i += 2)
                {
                    byte temp = byData[i];
                    byData[i] = byData[i + 1];
                    byData[i + 1] = temp;
                }
            }

            for (int j = 0; j < textCount; j++)
            {
                int pos = startPos + Util.GetOffset(byData, startPos + (j + 2) * 4, startPos + (j + 2) * 4 + 3) + 4;
                int nextPos = endPos;
                if (j < textCount - 1)
                {
                    nextPos = startPos + Util.GetOffset(byData, startPos + (j + 3) * 4, startPos + (j + 3) * 4 + 3) + 4;
                }

                while (pos < nextPos)
                {
                    if (fontCharPage.ContainsKey(byData[pos]))
                    {
                        sb.Append(fontCharPage[byData[pos]][byData[pos + 1]]);
                    }
                    else
                    {
                        sb.Append("^" + byData[pos].ToString("x") + " " + byData[pos + 1].ToString("x") + "^");
                    }

                    pos += 2;
                }

                sb.Append("\n");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 读取字库信息
        /// </summary>
        private void ReadFontChar()
        {
            // 读取日文字库
            this.ReadFontFile(@"..\EncodeFenxi\BioTools\BioCvEdit\JpFontMap.txt", this.jpFontCharPage);

            // 读取中文字库
            this.ReadFontFile(@"..\EncodeFenxi\BioTools\BioCvEdit\CnFontMap.txt", this.cnFontCharPage);
        }

        /// <summary>
        /// 读取字库信息
        /// </summary>
        private void ReadFontFile(string fontInfoFile, Dictionary<int, string[]> fontCharPage)
        {
            fontCharPage.Clear();

            try
            {
                // 读取字符信息
                string[] charMaps = File.ReadAllLines(fontInfoFile);
                for (int i = 0; i < charMaps.Length; i++)
                {
                    if (string.IsNullOrEmpty(charMaps[i]))
                    {
                        continue;
                    }

                    string[] valueChar = charMaps[i].Split('=');
                    int pageIndex = Convert.ToInt32(valueChar[0].Substring(2));
                    int charIndex = Convert.ToInt32(valueChar[0].Substring(0, 2), 16);
                    if (!fontCharPage.ContainsKey(pageIndex))
                    {
                        fontCharPage.Add(pageIndex, new string[256]);
                    }

                    fontCharPage[pageIndex][charIndex] = valueChar[1];
                }
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
                return;
            }
        }

        /// <summary>
        /// 显示Title信息
        /// </summary>
        /// <param name="jpBytes">日语的字节数</param>
        /// <param name="cnBytes">中文的字节数</param>
        private void DisplayTitle(int jpBytes, int cnBytes)
        {
            if (jpBytes < cnBytes)
            {
                this.Text = "Bio0文本编辑 翻译的文本数增加了：" + (cnBytes - jpBytes) + "文字！游戏可能无法运行！";
            }
            else if (jpBytes > cnBytes)
            {
                this.Text = "Bio0文本编辑 翻译的文本数减少了：" + (cnBytes - jpBytes) + "文字！游戏可能无法运行！";
            }
            else
            {
                this.Text = "Bio0文本编辑 翻译的文本数刚好。";
            }
        }

        /// <summary>
        /// 检查输入的中文长度是否正确
        /// </summary>
        /// <returns>输入的中文长度是否正确</returns>
        private bool CheckCnText()
        {
            int jpLen = this.txtJp.Text.Replace("^", string.Empty).Length;
            int cnLen = this.txtCn.Text.Replace("^", string.Empty).Length;

            if (jpLen < cnLen)
            {
                this.DisplayTitle(jpLen, cnLen);
                return false;
            }
            else
            {
                string[] jpTexts = Regex.Split(this.txtJp.Text, @"\n");
                string[] cnTexts = Regex.Split(this.txtCn.Text, @"\n");
                string jpText = string.Empty;
                string cnText = string.Empty;
                int selectionStart = 0;
                int maxLen = cnTexts.Length;
                while (string.IsNullOrEmpty(cnTexts[maxLen - 1]))
                {
                    maxLen--;
                }
                for (int i = 0; i < maxLen; i++)
                {
                    jpText = jpTexts[i];
                    cnText = cnTexts[i];

                    // 关键字是否被删除的判断
                    string currentChar;
                    string nextChar;
                    StringBuilder keyWordsSb = new StringBuilder();
                    for (int j = 0; j < jpText.Length - 1; j++)
                    {
                        currentChar = jpText.Substring(j, 1);
                        if ("^" == currentChar)
                        {
                            // 关键字的解码
                            keyWordsSb = new StringBuilder();
                            keyWordsSb.Append("^");
                            while ((nextChar = jpText.Substring(++j, 1)) != "^")
                            {
                                keyWordsSb.Append(nextChar);
                            }
                            keyWordsSb.Append("^");

                            if (cnText.IndexOf(keyWordsSb.ToString()) == -1)
                            {
                                this.txtCn.SelectionStart = selectionStart;
                                this.txtCn.SelectionLength = cnText.Length;
                                this.txtCn.SelectionColor = Color.Red;
                                this.txtCn.ScrollToCaret();
                                MessageBox.Show("关键字：" + keyWordsSb.ToString() + "不能被删除！");
                                return false;
                            }
                        }
                    }

                    selectionStart += cnText.Length + 1;
                }

                this.DisplayTitle(0, 0);
                this.txtCn.BackColor = SystemColors.Window;
                return true;
            }
        }

        /// <summary>
        /// 将当前行文本编码
        /// </summary>
        /// <param name="text">当前行文本</param>
        /// <returns>中文编码后的文本</returns>
        private byte[] EncodeLineText(string text)
        {
            List<byte> byData = new List<byte>();

            string currentChar;
            string nextChar;
            int charIndex;
            StringBuilder keyWordsSb = new StringBuilder();
            for (int i = 0; i < text.Length - 1; i++)
            {
                currentChar = text.Substring(i, 1);
                if ("^" == currentChar)
                {
                    // 关键字的解码
                    keyWordsSb = new StringBuilder();
                    while ((nextChar = text.Substring(++i, 1)) != "^")
                    {
                        keyWordsSb.Append(nextChar);
                    }

                    string[] keyWords = keyWordsSb.ToString().Split(' ');
                    foreach (string keyWord in keyWords)
                    {
                        charIndex = Convert.ToInt32(keyWord, 16);
                        byData.Add((byte)(charIndex & 0xFF));
                    }

                    continue;
                }
                else
                {
                    byData.AddRange(this.GetCharIndex(currentChar));
                }
            }

            return byData.ToArray();
        }

        /// <summary>
        /// 取得当前文字的索引
        /// </summary>
        /// <param name="currenChar"></param>
        /// <returns></returns>
        private byte[] GetCharIndex(string currentChar)
        {
            // 在字库中查找
            foreach (int fontPage in this.jpFontCharPage.Keys)
            {
                string[] pageFonts = this.jpFontCharPage[fontPage];
                for (int i = 0; i < pageFonts.Length; i++)
                {
                    if (currentChar == pageFonts[i])
                    {
                        return new byte[] { (byte)fontPage, (byte)i };
                    }
                }
            }

            throw new Exception("未查询到相应的中文字符！");
        }

        /// <summary>
        /// 将中文文本转换成二进制数据
        /// </summary>
        /// <param name="cnBytes">中文的字节数据</param>
        /// <returns></returns>
        private bool EncodeCnText(List<byte> cnBytes, string jpAllText, string cnAllText, bool hasEntry)
        {
            string[] jpTexts = Regex.Split(jpAllText.Replace("\n", string.Empty), "<BR>");
            string[] cnTexts = Regex.Split(cnAllText.Replace("\n", string.Empty), "<BR>");
            this.textEntrys.Clear();

            int maxLen = cnTexts.Length;
            while (string.IsNullOrEmpty(cnTexts[maxLen - 1]))
            {
                maxLen--;
            }

            for (int i = 0; i < maxLen; i++)
            {
                string jpText = jpTexts[i];
                string cnText = cnTexts[i];

                if (hasEntry)
                {
                    // 保存所有的Entry的偏移
                    this.textEntrys.Add((maxLen + 1) * 4 + cnBytes.Count);
                }

                // 将当前行文本编码
                cnBytes.AddRange(this.EncodeLineText(cnText));
            }

            return true;
        }

        /// <summary>
        /// 保存翻译
        /// </summary>
        /// <param name="cnBytes">翻译的字节数据</param>
        private void Save(List<byte> cnBytes, bool hasEntry)
        {
            FileStream fs = null;

            try
            {
                // 将文件中的数据，循环读取到byData中
                //fs = new FileStream(this.cnText, FileMode.Open);
                //byte[] byData = new byte[fs.Length];
                //fs.Read(byData, 0, byData.Length);
                //fs.Close();

                //int startPos = this.inputCnStartPos;
                //int maxLen = this.inputCnEndPos;

                //byte[] byCnData = null;
                //if (hasEntry)
                //{
                //    // 如果是带Entry的Message，先保存修改后的各个Entry
                //    int entrysCount = this.textEntrys.Count;
                //    byCnData = new byte[(entrysCount + 1) * 4 + cnBytes.Count];
                //    byCnData[0] = (byte)((entrysCount >> 24) & 0xFF);
                //    byCnData[1] = (byte)((entrysCount >> 16) & 0xFF);
                //    byCnData[2] = (byte)((entrysCount >> 8) & 0xFF);
                //    byCnData[3] = (byte)(entrysCount & 0xFF);
                //    for (int i = 0; i < entrysCount; i++)
                //    {
                //        int entryPos = this.textEntrys[i];
                //        byCnData[4 + i * 4] = (byte)((entryPos >> 24) & 0xFF);
                //        byCnData[4 + i * 4 + 1] = (byte)((entryPos >> 16) & 0xFF);
                //        byCnData[4 + i * 4 + 2] = (byte)((entryPos >> 8) & 0xFF);
                //        byCnData[4 + i * 4 + 3] = (byte)(entryPos & 0xFF);
                //    }

                //    // 再保存文本数据
                //    Array.Copy(cnBytes.ToArray(), 0, byCnData, (entrysCount + 1) * 4, cnBytes.Count);
                //}
                //else
                //{
                //    byCnData = new byte[cnBytes.Count];
                //    Array.Copy(cnBytes.ToArray(), 0, byCnData, 0, cnBytes.Count);
                //}

                //// 复制修改的部分
                //Array.Copy(byCnData, 0, byData, startPos, byCnData.Length);

                //// 翻译后的字节数组写入文件
                //File.WriteAllBytes(this.cnText, byData);

                //// 显示Title信息
                //this.DisplayTitle(this.inputCnEndPos - this.inputCnStartPos, cnBytes.Count);

                //// 写字库文件
                //List<Bio0CharInfo> cnFont = new List<Bio0CharInfo>();
                //foreach (int fontPage in this.cnFontCharPage.Keys)
                //{
                //    Bio0CharInfo[] pageFonts = this.cnFontCharPage[fontPage];
                //    for (int i = 0; i < pageFonts.Length; i++)
                //    {
                //        if (pageFonts[i].IsUseSecondImg)
                //        {
                //            cnFont.Add(pageFonts[i]);
                //        }
                //    }
                //}

                //string[] cnFontNew = File.ReadAllLines(this.cnFont, Encoding.UTF8);
                //for (int i = 0; i < cnFont.Count; i++)
                //{
                //    Bio0CharInfo charInfo = cnFont[i];
                //    int charIndex = charInfo.Y / 0x1C * 32 + charInfo.X / 0x1C;
                //    if (charIndex < cnFontNew.Length)
                //    {
                //        cnFontNew[charIndex] = charInfo.FontChar;
                //    }
                //}

                //File.WriteAllLines(this.cnFont, cnFontNew, Encoding.UTF8);
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
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
        /// 导出文本
        /// </summary>
        private void ExoprtText()
        {
            Microsoft.Office.Interop.Excel.Application xApp = null;
            Microsoft.Office.Interop.Excel.Workbook xBook = null;
            Microsoft.Office.Interop.Excel.Worksheet xSheet = null;

            // 设定保存的文件名
            string fileName = @"E:\My\Hanhua\testFile\bioCv\BioCvTextNgc.xls";
            //string fileName = @"D:\game\iso\wii\生化危机维罗妮卡汉化\Bio0Text_" + this.exportName + ".xls";

            // 先删除原来的文件
            File.Delete(fileName);

            // 显示进度条
            this.ResetProcessBar(this.fileList.Items.Count);

            try
            {
                // 创建Application对象 
                xApp = new Microsoft.Office.Interop.Excel.ApplicationClass();
                //xApp.Visible = true;

                // 追加一个WorkBook
                xBook = xApp.Workbooks.Add(Missing.Value);

                for (int j = 0; j < this.fileList.Items.Count; j++)
                {
                    // 追加一个Sheet
                    FilePosInfo filePosInfo = this.textFiles[j];

                    // 更新当前文本
                    this.fileList.SelectedIndex = j;

                    // 取得日文、中文文本
                    string jpText = this.txtJp.Text;
                    string cnText = this.txtCn.Text;

                    string sheetName = Util.GetShortFileName(filePosInfo.File);
                    int sameNameCount = 0;
                    for (int i = 0; i < j; i++)
                    {
                        if (Util.GetShortFileName(this.textFiles[i].File).IndexOf(sheetName) >= 0)
                        {
                            sameNameCount++;
                        }
                    }

                    xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    xSheet.Name = sheetName + (sameNameCount > 0 ? "_" + sameNameCount.ToString().PadLeft(2, '0') : string.Empty);

                    // 将每行文本保存到Sheet中
                    string[] jpTexts = jpText.Split('\n');
                    string[] cnTexts = cnText.Split('\n');

                    for (int i = 0; i < jpTexts.Length; i++)
                    {
                        // 写入日文文本
                        Microsoft.Office.Interop.Excel.Range rngJp = xSheet.get_Range("A" + (i + 1), Missing.Value);
                        rngJp.Value2 = jpTexts[i];
                    }
                    for (int i = 0; i < cnTexts.Length; i++)
                    {
                        // 写入中文文本
                        Microsoft.Office.Interop.Excel.Range rngCn = xSheet.get_Range("G" + (i + 1), Missing.Value);
                        rngCn.Value2 = cnTexts[i];
                    }

                    // 更新进度条
                    this.ProcessBarStep();
                }

                // 保存
                xSheet.SaveAs(
                    fileName,
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                // 隐藏进度条
                this.CloseProcessBar();

                // 显示保存完成信息
                MessageBox.Show("导出完成！");

            }
            catch (Exception me)
            {
                MessageBox.Show(this.baseFile + "\n" + me.Message);
            }
            finally
            {
                // 隐藏进度条
                this.CloseProcessBar();

                // 清空各种对象
                xSheet = null;
                xBook = null;
                if (xApp != null)
                {
                    xApp.Quit();
                    xApp = null;
                }
            }
        }

        /// <summary>
        /// 导入文本
        /// </summary>
        private void ImportText(string fileName)
        {
            Microsoft.Office.Interop.Excel.Application xApp = null;
            Microsoft.Office.Interop.Excel.Workbook xBook = null;
            Microsoft.Office.Interop.Excel.Worksheet xSheet = null;

            // 显示进度条
            this.ResetProcessBar(this.fileList.Items.Count);

            try
            {
                StringBuilder failFiles = new StringBuilder();

                // 创建Application对象 
                xApp = new Microsoft.Office.Interop.Excel.ApplicationClass();

                // 得到WorkBook对象, 打开已有的文件 
                xBook = xApp.Workbooks._Open(
                    fileName,
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                for (int i = xBook.Sheets.Count; i >= 1; i--)
                {
                    // 取得相应的Sheet
                    xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets[i];
                    int sheetIndex = -1;
                    string sheetName = string.Empty;
                    for (int j = 0; j < this.fileList.Items.Count; j++)                    
                    {
                        sheetName = Util.GetShortFileName(this.textFiles[j].File);
                        if (xSheet.Name.IndexOf(sheetName) >= 0)
                        {
                            if (xSheet.Name.IndexOf("main.dol") >= 0)
                            {
                                if ((xSheet.Name == "main.dol" && string.IsNullOrEmpty(this.textFiles[j].SubIndex))
                                    || (!string.IsNullOrEmpty(this.textFiles[j].SubIndex) && xSheet.Name.IndexOf(this.textFiles[j].SubIndex) >= 0))
                                {
                                    sheetIndex = j;
                                    break;
                                }
                            }
                            else
                            {
                                sheetIndex = j;
                                break;
                            }
                        }
                    }

                    if (sheetIndex > -1)
                    {
                        // 更新当前文本
                        this.fileList.SelectedIndex = sheetIndex;

                        // 取得当前Sheet的中文文本
                        int lineNum = 1;
                        int blankNum = 0;
                        StringBuilder sb = new StringBuilder();
                        while (blankNum < 4)
                        {
                            string cellValue = xSheet.get_Range("G" + lineNum, Missing.Value).Value2 as string;
                            sb.Append(cellValue).Append("\n");

                            if (string.IsNullOrEmpty(cellValue))
                            {
                                blankNum++;
                            }
                            else
                            {
                                blankNum = 0;
                            }

                            lineNum++;
                        }

                        sb = sb.Replace("\n\n\n\n\n", "\n");

                        this.txtCn.Text = sb.ToString();

                        // 保存
                        if (!this.Save())
                        {
                            //return;
                            throw new Exception("有文件长度检查失败");
                            //failFiles.Append("\n").Append(sheetName);
                        }
                    }

                    // 更新进度条
                    this.ProcessBarStep();
                }

                // 隐藏进度条
                this.CloseProcessBar();

                // 显示保存完成信息
                if (failFiles.Length == 0)
                {
                    MessageBox.Show("完全成功导入！");
                }
                else
                {
                    MessageBox.Show("导入完成，下面文件失败" + failFiles.ToString());
                }

                // 重新读取字库信息
                this.ReadFontChar();
            }
            catch (Exception me)
            {
                MessageBox.Show(this.baseFile + "\n" + me.Message);
            }
            finally
            {
                // 隐藏进度条
                this.CloseProcessBar();

                // 清空各种对象
                xSheet = null;
                xBook = null;
                if (xApp != null)
                {
                    xApp.Quit();
                    xApp = null;
                }
            }
        }

        #endregion
    }
}
