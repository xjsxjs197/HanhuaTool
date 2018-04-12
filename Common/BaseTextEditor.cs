using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Hanhua.Common
{
    /// <summary>
    /// 文本编辑器基类
    /// </summary>
    public partial class BaseTextEditor : BaseForm
    {
        #region " 本地变量 "

        /// <summary>
        /// 记录所有文本文件
        /// </summary>
        protected List<FilePosInfo> textFiles = new List<FilePosInfo>();

        /// <summary>
        /// 当前编辑的文件信息
        /// </summary>
        protected FilePosInfo currentFileInfo;

        /// <summary>
        /// 当前的中文文件
        /// </summary>
        protected string cnFile = string.Empty;

        /// <summary>
        /// 分为A，B盘时使用
        /// </summary>
        protected string subDisk = string.Empty;

        /// <summary>
        /// 当前汉化游戏名称
        /// </summary>
        protected string gameName = string.Empty;

        /// <summary>
        /// 当前选择文件的位置
        /// </summary>
        protected int fileIndex = -1;

        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        public BaseTextEditor()
        {
            InitializeComponent();

            this.txtCn.OtherRichTextBox = this.txtJp;
            this.Resize += new EventHandler(this.BaseTextEditor_Resize);
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

                    // 重新加载翻译的文本
                    this.FileChanged(this.currentFileInfo);
                    this.txtCn.Text = this.DecodeText(this.currentFileInfo, true);
                }
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
            }
        }

        /// <summary>
        /// 文件变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.fileList.SelectedIndex == -1
                || this.fileList.SelectedIndex >= this.textFiles.Count)
            {
                return;
            }

            this.fileIndex = this.fileList.SelectedIndex;

            // 取得当前编辑文件
            this.currentFileInfo = this.textFiles[this.fileList.SelectedIndex];

            // 选择的文件变更
            this.FileChanged(this.currentFileInfo);

            // 设置中文文件
            this.cnFile = this.currentFileInfo.File.ToLower().Replace("jp", "cn");

            // 解码文本
            this.txtJp.Text = this.DecodeText(this.currentFileInfo, false);
            this.txtCn.Text = this.DecodeText(this.currentFileInfo, true);
        }

        /// <summary>
        /// 导出文本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            // 导出前的操作
            bool canNext = this.BeforeExport();

            // 导出
            if (canNext)
            {
                this.Do(this.ExoprtText);
            }
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImport_Click(object sender, EventArgs e)
        {
            // 导入前的操作
            bool canNext = this.BeforeImport();

            // 导入
            if (canNext)
            {
                // 取得所有List的名称
                List<string> listItems = new List<string>();
                foreach (object item in this.fileList.Items)
                {
                    listItems.Add(item.ToString());
                }

                this.Do(this.ImportText, listItems);
            }
        }

        /// <summary>
        /// 画面大小变更后，重新设置三个区域的大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseTextEditor_Resize(object sender, EventArgs e)
        {
            int editAreaHeight = this.pnlEditor.Height;
            this.txtCn.Height = editAreaHeight / 2;
            this.txtJp.Height = editAreaHeight / 2;

            this.fileList.Width = (int)(this.pnlEditor.Width * 0.23);
        }

        #endregion

        #region " 子类可以继承实现的方法 "

        /// <summary>
        /// 画面初始化
        /// </summary>
        protected virtual void EditorInit()
        {
            // 设置标题
            if (this.Text.IndexOf(this.gameName) == -1)
            {
                this.Text = this.gameName + this.Text;
            }

            // 读取字库信息
            this.ReadFontChar();

            // 读取需要汉化的文件
            this.LoadAllFiles();

            // 选中第一个文件
            if (this.fileList.Items.Count > 0)
            {
                this.fileList.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 读取字库信息
        /// </summary>
        protected virtual void ReadFontChar()
        { 
        }

        /// <summary>
        /// 读取需要汉化的文件
        /// </summary>
        protected virtual void LoadAllFiles()
        {
            this.fileList.Items.Clear();
            this.textFiles.Clear();
        }

        /// <summary>
        /// 选择的文件变更
        /// </summary>
        /// <param name="currentFileInfo">当前选择的文件</param>
        protected virtual void FileChanged(FilePosInfo currentFileInfo)
        { 
        }

        /// <summary>
        /// 开始解码文本
        /// </summary>
        /// <param name="currentFileInfo">当前选择的文件</param>
        /// <param name="isCnTxt">是否是中文</param>
        /// <returns>解码的文本</returns>
        protected virtual string DecodeText(FilePosInfo currentFileInfo, bool isCnTxt)
        {
            return string.Empty;
        }

        /// <summary>
        /// 导出前的操作
        /// </summary>
        /// <returns>是否可以继续</returns>
        protected virtual bool BeforeExport()
        {
            this.baseFile = Util.SetSaveDailog(this.gameName + "翻译后文件（*.xlsx）|*.xlsx|所有文件|*.*", this.baseFolder);
            if (string.IsNullOrEmpty(this.baseFile))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 导入前的操作
        /// </summary>
        /// <returns>是否可以继续</returns>
        protected virtual bool BeforeImport()
        {
            this.baseFile = Util.SetOpenDailog(this.gameName + "翻译后文件（*.xls,*.xlsx）|*.xls;*.xlsx|所有文件|*.*", this.baseFolder);
            if (string.IsNullOrEmpty(this.baseFile))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 导入成功后的处理
        /// </summary>
        protected virtual void AfterImport()
        { 
        }

        /// <summary>
        /// 保存成功后的处理
        /// </summary>
        protected virtual void AfterSave()
        {
        }

        /// <summary>
        /// 保存存前的处理
        /// </summary>
        /// <param name="byCnData">要保存的数据</param>
        /// <param name="fileInfo">当前文件信息</param>
        protected virtual void BeforeSave(byte[] byCnData, FilePosInfo fileInfo)
        {
        }

        /// <summary>
        /// 检查输入的中文长度是否正确
        /// </summary>
        /// <param name="chkKeyWords">是否需要检查关键字</param>
        /// <returns>输入的中文长度是否正确</returns>
        protected virtual bool CheckCnText(bool chkKeyWords)
        {
            int jpLen = this.txtJp.Text.Replace("^", string.Empty).Length;
            int cnLen = this.txtCn.Text.Replace("^", string.Empty).Length;

            if (jpLen < cnLen)
            {
                this.DisplayTitle(jpLen, cnLen);
                return false;
            }
            else if (chkKeyWords)
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
            }

            return true;
        }

        /// <summary>
        /// 取得当前文字的编码
        /// </summary>
        /// <param name="currenChar">当前文字</param>
        /// <returns>当前文字的编码</returns>
        protected virtual byte[] EncodeChar(string currentChar)
        {
            throw new Exception("未查询到相应的中文字符 : " + currentChar);
        }

        /// <summary>
        /// 重新设置Entry位置信息
        /// </summary>
        /// <param name="currentFileInfo">当前选择的文件</param>
        /// <param name="cnTxtLen">当前行中文文本字节长度</param>
        /// <param name="prevEntryPos">前一个Entry位置信息</param>
        /// <returns>当前Entry位置信息</returns>
        protected virtual int ResetEntryPosInfo(FilePosInfo currentFileInfo, int cnTxtLen, int prevEntryPos)
        {
            // 保存Entry的偏移
            int nextEntryPos = prevEntryPos + cnTxtLen;
            currentFileInfo.TextEntrys.Add(nextEntryPos);

            return nextEntryPos;
        }

        /// <summary>
        /// 重新设置Entry位置信息
        /// </summary>
        /// <param name="currentFileInfo">当前选择的文件</param>
        /// <param name="cnTxtLen">当前行中文文本字节长度</param>
        /// <param name="prevEntryPos">前一个Entry位置信息</param>
        /// <returns>当前Entry位置信息</returns>
        protected virtual int ResetLastEntryPosInfo(FilePosInfo currentFileInfo, int cnTxtLen, int prevEntryPos)
        {
            return prevEntryPos + cnTxtLen;
        }

        /// <summary>
        /// 重新设置带Entry信息的翻译后的数据
        /// </summary>
        /// <param name="currentFileInfo">当前选择的文件</param>
        /// <param name="byData">当前选择的文件的字节数据</param>
        /// <param name="cnBytes">翻译后的字节数据</param>
        /// <returns>带Entry信息的翻译后的数据</returns>
        protected virtual byte[] ResetCnDataWithEnrty(FilePosInfo currentFileInfo, byte[] byData, List<byte> cnBytes)
        {
            throw new Exception("需要重新设置带Entry信息的字节信息！");
        }

        /// <summary>
        /// 追加文件
        /// </summary>
        protected virtual void AddFile(string fileName, FilePosInfo fileInfo)
        {
            this.textFiles.Add(fileInfo);
            this.fileList.Items.Add(Util.GetShortFileName(fileInfo.File) + " " + fileInfo.TextStart.ToString("x") + "--" + fileInfo.TextEnd.ToString("x"));

            // 重新设置全路径的文件
            fileInfo.File = fileName;
        }

        /// <summary>
        /// 根据配置文件，读入需要汉化的文件
        /// </summary>
        /// <param name="configFile"></param>
        /// <returns></returns>
        protected virtual List<FilePosInfo> LoadFiles(string configFile)
        {
            List<FilePosInfo> needCopyFiles = new List<FilePosInfo>();

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

        #region " 私有方法 "

        /// <summary>
        /// 保存文本
        /// </summary>
        /// <returns></returns>
        private bool Save()
        {
            // 长度及关键字检查
            if (!this.CheckCnText(true))
            {
                return false;
            }

            // 将中文文本转换成二进制数据
            List<byte> cnBytes = new List<byte>();
            if (!this.EncodeCnText(cnBytes, this.txtCn.Text))
            {
                return false;
            }

            // 检查最大的字节数
            //int oldTextLen = this.currentFileInfo.TextEnd - 
            //    (this.currentFileInfo.TextStart >= this.currentFileInfo.EntryPos ? this.currentFileInfo.TextStart : this.currentFileInfo.EntryPos);
            int oldTextLen = this.currentFileInfo.TextEnd - this.currentFileInfo.TextStart;
            if (oldTextLen < cnBytes.Count)
            {
                throw new Exception("最大字节数超出 ：" + (cnBytes.Count - oldTextLen));
            }

            // 修正最大长度不足的字节
            int diffLen = oldTextLen - cnBytes.Count;
            while (diffLen > 0)
            {
                cnBytes.Add(0);
                diffLen--;
            }

            // 保存二进制数据
            this.Save(cnBytes);

            return true;
        }

        /// <summary>
        /// 保存翻译
        /// </summary>
        /// <param name="cnBytes">翻译的字节数据</param>
        private void Save(List<byte> cnBytes)
        {
            // 将文件中的数据，读取到byData中
            byte[] byData = File.ReadAllBytes(this.cnFile);


            byte[] byCnData = null;
            if (this.currentFileInfo.EntryPos > 0)
            {
                // 设置带Entry的文本数据
                byCnData = this.ResetCnDataWithEnrty(this.currentFileInfo, byData, cnBytes);
            }
            else
            {
                byCnData = new byte[cnBytes.Count];
                Array.Copy(cnBytes.ToArray(), 0, byCnData, 0, cnBytes.Count);
            }

            // 复制修改的部分
            Array.Copy(byCnData, 0, byData, this.currentFileInfo.TextCopyStart, byCnData.Length);

            // 保存存前的处理
            this.BeforeSave(byData, this.currentFileInfo);

            // 翻译后的字节数组写入文件
            File.WriteAllBytes(this.cnFile, byData);

            // 显示Title信息
            this.DisplayTitle(this.currentFileInfo.TextEnd - this.currentFileInfo.TextCopyStart, byCnData.Length);
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
                this.Text = this.gameName + "文本编辑 翻译的文本数增加了：" + (cnBytes - jpBytes) + "文字！游戏可能无法运行！";
            }
            else if (jpBytes > cnBytes)
            {
                this.Text = this.gameName + "文本编辑 翻译的文本数减少了：" + (cnBytes - jpBytes) + "文字！游戏可能无法运行！";
            }
            else
            {
                this.Text = this.gameName + "文本编辑 翻译的文本数刚好。";
            }
        }

        /// <summary>
        /// 将中文文本转换成二进制数据
        /// </summary>
        /// <param name="cnBytes">中文的字节数据</param>
        /// <returns>成功与否</returns>
        private bool EncodeCnText(List<byte> cnBytes, string cnAllText)
        {
            string[] cnTexts = Regex.Split(cnAllText, "\n");
            int maxLen = cnTexts.Length;
            int currentEntryPos = 0;
            this.currentFileInfo.TextEntrys.Clear();
            this.currentFileInfo.TextEntrys.Add(0);

            while (string.IsNullOrEmpty(cnTexts[maxLen - 1]))
            {
                maxLen--;
            }

            for (int i = 0; i < maxLen; i++)
            {
                string cnText = cnTexts[i];

                // 将当前行文本编码
                byte[] cnEndodedBytes = this.EncodeLineText(cnText);
                cnBytes.AddRange(cnEndodedBytes);

                if (this.currentFileInfo.EntryPos > 0)
                {
                    // 保存所有的Entry的偏移
                    if (i != (maxLen - 1))
                    {
                        currentEntryPos = this.ResetEntryPosInfo(this.currentFileInfo, cnEndodedBytes.Length, currentEntryPos);
                    }
                    else
                    {
                        this.ResetLastEntryPosInfo(this.currentFileInfo, cnEndodedBytes.Length, currentEntryPos);
                    }
                }
            }

            return true;
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
                        //try
                        //{
                        //    Convert.ToInt32(keyWord, 16);
                        //}
                        //catch 
                        //{
                        //}
                        charIndex = Convert.ToInt32(keyWord, 16);
                        byData.Add((byte)(charIndex & 0xFF));
                    }

                    continue;
                }
                else
                {
                    byData.AddRange(this.EncodeChar(currentChar));
                }
            }

            return byData.ToArray();
        }
        
        /// <summary>
        /// 导出文本
        /// </summary>
        private void ExoprtText()
        {
            Microsoft.Office.Interop.Excel.Workbook xBook = null;
            Microsoft.Office.Interop.Excel.Worksheet xSheet = null;
            Microsoft.Office.Interop.Excel.Worksheet beforeSheet = null;

            // 设定保存的文件名
            string fileName = this.baseFile;
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = @"TextExport.xlsx";
            }

            // 先删除原来的文件
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            // 显示进度条
            this.ResetProcessBar(this.textFiles.Count);

            try
            {
                // 创建Application对象 
                this.xApp = new Microsoft.Office.Interop.Excel.ApplicationClass();

                // 追加一个WorkBook
                xBook = this.xApp.Workbooks.Add(Missing.Value);

                for (int j = 0; j < this.textFiles.Count; j++)
                {
                    // 追加一个Sheet
                    FilePosInfo filePosInfo = this.textFiles[j];

                    string jpText = string.Empty;
                    string cnText = string.Empty;

                    this.Invoke((MethodInvoker)delegate()
                    {
                        // 更新当前文本
                        this.fileList.SelectedIndex = j;

                        // 取得日文、中文文本
                        jpText = this.txtJp.Text;
                        cnText = this.txtCn.Text;
                    });

                    // 设置当前Sheet名（如果有重复的就累加编号）
                    string sheetName = Util.GetShortFileName(filePosInfo.File);
                    int sameNameCount = 0;
                    for (int i = 0; i < j; i++)
                    {
                        if (Util.GetShortFileName(this.textFiles[i].File).IndexOf(sheetName) >= 0)
                        {
                            sameNameCount++;
                        }
                    }

                    if (beforeSheet == null)
                    {
                        xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    }
                    else
                    {
                        xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets.Add(beforeSheet, Missing.Value, Missing.Value, Missing.Value);
                    }
                    xSheet.Name = sheetName + (sameNameCount > 0 ? "_" + sameNameCount.ToString() : string.Empty);
                    beforeSheet = xSheet;

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
            }
            finally
            {
                // 保存
                xSheet.SaveAs(
                    fileName,
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                // 隐藏进度条
                this.CloseProcessBar();

                // 清空各种对象
                xSheet = null;
                xBook = null;
                if (this.xApp != null)
                {
                    this.xApp.Quit();
                    this.xApp = null;
                }

                // 显示保存完成信息
                MessageBox.Show("导出完成！");
            }
        }

        /// <summary>
        /// 导入文本
        /// </summary>
        private void ImportText(params object[] param)
        {
            List<string> listItems = (List<string>)param[0]; 

            Microsoft.Office.Interop.Excel.Workbook xBook = null;
            Microsoft.Office.Interop.Excel.Worksheet xSheet = null;

            // 显示进度条
            this.ResetProcessBar(listItems.Count);

            try
            {
                StringBuilder failFiles = new StringBuilder();

                // 创建Application对象 
                this.xApp = new Microsoft.Office.Interop.Excel.ApplicationClass();

                // 得到WorkBook对象, 打开已有的文件 
                xBook = this.xApp.Workbooks._Open(
                    this.baseFile,
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                for (int i = xBook.Sheets.Count; i >= 1; i--)
                {
                    // 取得相应的Sheet
                    xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets[i];
                    int sheetIndex = -1;
                    string sheetName = string.Empty;
                    for (int j = 0; j < listItems.Count; j++)
                    {
                        sheetName = listItems[j].Split(' ')[0];
                        if (xSheet.Name.Equals(sheetName, StringComparison.OrdinalIgnoreCase))
                        {
                            sheetIndex = j;
                            break;
                        }
                    }

                    if (sheetIndex > -1)
                    {
                        // 更新当前文本
                        this.fileList.Invoke((MethodInvoker)delegate()
                        {
                            this.fileList.SelectedIndex = sheetIndex;
                        });

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

                        this.Invoke((MethodInvoker)delegate()
                        {
                            this.txtCn.Text = sb.ToString();

                            // 保存
                            if (!this.Save())
                            {
                                //throw new Exception("有文件长度检查失败");
                                failFiles.Append("\n").Append(sheetName);
                            }
                        });
                    }
                    else
                    {
                        failFiles.Append("\n未找到 ").Append(xSheet.Name);
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

                // 导入成功后的处理
                this.AfterImport();
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
                if (this.xApp != null)
                {
                    this.xApp.Quit();
                    this.xApp = null;
                }
            }
        }

        #endregion
    }
}
