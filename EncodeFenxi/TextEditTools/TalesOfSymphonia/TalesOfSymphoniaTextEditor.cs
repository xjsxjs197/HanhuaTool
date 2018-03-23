using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Hanhua.Common;

namespace Hanhua.TextEditTools.TalesOfSymphonia
{
    /// <summary>
    /// 仙乐传说文本编辑器
    /// </summary>
    public partial class TalesOfSymphoniaTextEditor : BaseTextEditor
    {
        #region " 本地变量 "

        #region " 字库 "

        /// <summary>
        /// 原字库
        /// </summary>
        private string[] oldJpFontRange = {
            "15亜蔭", "03院円", "01園改", "03魁樫", "01橿棄", "03機救", "01朽屈", "03掘鯨", "01劇向", "03后降", "01項刷",
            "03察止", "01死周", "03宗淳", "01準節", "03拭厨", "01逗線", "03繊掻", "01操只", "03叩蓄", "01逐逓", "03邸冬",
            "01凍入", "03如梅", "01楳美", "03鼻敷", "01斧朋", "03法盆", "01摩癒", "03諭欲", "01沃聯", "03蓮腕"
            //"13儚遥", "02傲領", "18佇鼬", "01琥竄"
        };

        #endregion

        /// <summary>
        /// 中文字库
        /// </summary>
        private string[] cnFontChars = null;

        /// <summary>
        /// 日文字库
        /// </summary>
        private string[] jpFontChars = null;

        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        public TalesOfSymphoniaTextEditor()
        {
            InitializeComponent();

            this.ResetHeight();

            this.gameName = "TalesOfSymphonia";
            this.baseFolder = @"E:\Study\MySelfProject\Hanhua\TodoCn\TalesOfSymphonia";
            //this.baseFolder = @"G:\游戏汉化\仙乐传说";
            
            // 初始化
            this.EditorInit();
        }

        #region " 事件 "

        /// <summary>
        /// 打包
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPatch_Click(object sender, EventArgs e)
        {
            this.EditorInit();
        }

        /// <summary>
        /// 重新加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReLoad_Click(object sender, EventArgs e)
        {
            this.EditorInit();
        }

        /// <summary>
        /// 字库做成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFont_Click(object sender, EventArgs e)
        {
            // 生成旧字库图片
            //this.CreateOldFontImg("fontb0");
            //this.CreateOldFontImg("fontb1");

            //this.CreateOldFontImg("gage_fontb0");
            //this.CreateOldFontImg("gage_fontb1");

            //this.CreateOldFontImg("dep_fontb0");
            //this.CreateOldFontImg("dep_fontb1");

            //this.CreateOldFontImg("pop_fontb0");
            //this.CreateOldFontImg("pop_fontb1");

            //this.CreateOldFontImg("yug_fontb0");

            //List<string> charInfo = new List<string>();
            //Encoding encoding = Encoding.GetEncoding("Shift-jis");
            //int charCount = 0;
            ////string txt = encoding.GetString(new byte[] { 0x99, 0x6f });
            ////txt = encoding.GetString(new byte[] { 0x97, 0xfc });

            //for (int i = 0; i < this.oldJpFontRange.Length; i++)
            //{
            //    string range = this.oldJpFontRange[i];
            //    string startChar = range.Substring(2, 1);
            //    string endChar = range.Substring(3, 1);
            //    byte[] byStart = encoding.GetBytes(startChar);
            //    byte[] byEnd = encoding.GetBytes(endChar);
            //    charCount += Util.GetOffset(byEnd, 0, 1) - Util.GetOffset(byStart, 0, 1) + 1;

            //    charInfo.Add(range + " " + byStart[0].ToString("x") + byStart[1].ToString("x") + "--" + byEnd[0].ToString("x") + byEnd[1].ToString("x"));
            //}

            //File.WriteAllLines(@"E:\Study\MySelfProject\Hanhua\TodoCn\TalesOfSymphonia\Font\FontCharInfo.txt", charInfo.ToArray(), Encoding.UTF8);

            //this.CopyCnSkpTxt();
            //this.AddCnSkpTxtZero();
            //this.Get3DmText();
            //this.CheckEnMapAddrInfo();
            //this.CopyCnMapTxt();
        }

        #endregion

        #region " 重写父类方法 "

        /// <summary>
        /// 读取需要汉化的文件
        /// </summary>
        protected override void LoadAllFiles()
        {
            base.LoadAllFiles();

            // 根据配置文件，取得所有汉化的文件
            List<FilePosInfo> allFiles = this.LoadFiles();
            if (allFiles.Count == 0)
            {
                MessageBox.Show("路径错误，没有找到需要Copy的文件！");
                return;
            }

            // 添加文件
            string jpFile = string.Empty;
            foreach (FilePosInfo fileInfo in allFiles)
            {
                jpFile = this.baseFolder + @"\en\root\" + fileInfo.File;
                // 取得各个文件名
                if (File.Exists(jpFile))
                {
                    this.AddFile(jpFile, fileInfo);
                }
            }
        }

        /// <summary>
        /// 开始解码文本
        /// </summary>
        /// <param name="currentFileInfo">当前选择的文件</param>
        /// <param name="isCnTxt">是否是中文</param>
        /// <returns>解码的文本</returns>
        protected override string DecodeText(FilePosInfo currentFileInfo, bool isCnTxt)
        {
            if (isCnTxt)
            {
                return this.DecodeText(File.ReadAllBytes(this.cnFile), currentFileInfo);
            }
            else
            {
                return this.DecodeText(File.ReadAllBytes(currentFileInfo.File), currentFileInfo);
            }
        }

        /// <summary>
        /// 取得当前文字的编码
        /// </summary>
        /// <param name="currenChar">当前文字</param>
        /// <returns>当前文字的编码</returns>
        protected override byte[] EncodeChar(string currentChar)
        {
            for (int i = 0; i < this.cnFontChars.Length; i++)
            {
                string fontChar = this.cnFontChars[i];
                if (fontChar.Equals(currentChar))
                {
                    return new byte[] { (byte)(i >> 8 & 0xFF), (byte)(i & 0xFF) };
                }
            }

            throw new Exception("未查询到相应的中文字符 : " + currentChar);
        }

        /// <summary>
        /// 重新设置带Entry信息的翻译后的数据
        /// </summary>
        /// <param name="currentFileInfo">当前选择的文件</param>
        /// <param name="byData">当前选择的文件的字节数据</param>
        /// <param name="cnBytes">翻译后的字节数据</param>
        /// <returns>带Entry信息的翻译后的数据</returns>
        protected override byte[] ResetCnDataWithEnrty(FilePosInfo currentFileInfo, byte[] byData, List<byte> cnBytes)
        {
            // 带Entry的文本，先保存修改后的各个Entry
            int entryLen = currentFileInfo.TextEntrys.Count * 2;
            byte[] byCnData = new byte[entryLen + 2 + cnBytes.Count];
            int idx = 0;
            for (int i = 0; i < currentFileInfo.TextEntrys.Count; i += 2)
            {
                int startPos = currentFileInfo.TextEntrys[i] / 2;
                int lenInfo = currentFileInfo.TextEntrys[i + 1] / 2;

                byCnData[idx * 4] = (byte)((startPos >> 8) & 0xFF);
                byCnData[idx * 4 + 1] = (byte)(startPos & 0xFF);

                byCnData[idx * 4 + 2] = (byte)((lenInfo >> 8) & 0xFF);
                byCnData[idx * 4 + 3] = (byte)(lenInfo & 0xFF);

                idx++;
            }

            // 再保存文本数据
            byCnData[entryLen] = 0x80;
            byCnData[entryLen + 1] = 0;
            Array.Copy(cnBytes.ToArray(), 0, byCnData, entryLen + 2, cnBytes.Count);

            return byCnData;
        }

        /// <summary>
        /// 重新设置Entry位置信息
        /// </summary>
        /// <param name="currentFileInfo">当前选择的文件</param>
        /// <param name="cnTxtLen">当前行中文文本字节长度</param>
        /// <param name="prevEntryPos">前一个Entry位置信息</param>
        /// <returns>当前Entry位置信息</returns>
        protected override int ResetEntryPosInfo(FilePosInfo currentFileInfo, int cnTxtLen, int prevEntryPos)
        {
            // 先保存文本的长度
            currentFileInfo.TextEntrys.Add(cnTxtLen);

            // 保存下一个Entry的开始位置
            currentFileInfo.TextEntrys.Add(prevEntryPos + cnTxtLen);

            return prevEntryPos + cnTxtLen;
        }

        /// <summary>
        /// 重新设置Entry位置信息
        /// </summary>
        /// <param name="currentFileInfo">当前选择的文件</param>
        /// <param name="cnTxtLen">当前行中文文本字节长度</param>
        /// <param name="prevEntryPos">前一个Entry位置信息</param>
        /// <returns>当前Entry位置信息</returns>
        protected override int ResetLastEntryPosInfo(FilePosInfo currentFileInfo, int cnTxtLen, int prevEntryPos)
        {
            // 保存文本的长度
            currentFileInfo.TextEntrys.Add(cnTxtLen);

            return prevEntryPos + cnTxtLen;
        }

        /// <summary>
        /// 检查输入的中文长度是否正确
        /// </summary>
        /// <param name="chkKeyWords">是否需要检查关键字</param>
        /// <returns>输入的中文长度是否正确</returns>
        protected override bool CheckCnText(bool chkKeyWords)
        {
            return base.CheckCnText(false);
        }

        /// <summary>
        /// 读取字库信息
        /// </summary>
        protected override void ReadFontChar()
        {
        }

        #endregion

        #region " 公有方法 "

        #endregion

        #region " 私有方法 "

        /// <summary>
        /// 检查英文版Map文本地址
        /// </summary>
        private void CheckEnMapAddrInfo()
        {
            List<FilePosInfo> allFiles = Util.GetAllFiles(this.baseFolder + @"\jp\root\MAP\Decode\").Where(
                p => !p.IsFolder && p.File.EndsWith(".bin", StringComparison.OrdinalIgnoreCase)).ToList();
            List<string> lstAddr = new List<string>();
            List<string> lstNotFindAddr = new List<string>();
            foreach (FilePosInfo fiInfo in allFiles)
            {
                // 设置置文本位置信息
                if (this.SetTextPosInfo(fiInfo))
                {
                    lstAddr.Add(fiInfo.File.Replace(this.baseFolder + @"\jp\root\", string.Empty));
                    lstAddr.Add(fiInfo.TextStart.ToString("x") + " " + fiInfo.TextEnd.ToString("x") + " " + fiInfo.EntryPos.ToString("x"));
                }
                else
                {
                    lstNotFindAddr.Add(fiInfo.File.Replace(this.baseFolder + @"\jp\root\", string.Empty));
                }
            }

            File.WriteAllLines(this.baseFolder + @"\mapAddrInfo_Jp.txt", lstAddr.ToArray(), Encoding.UTF8);
            if (lstNotFindAddr.Count > 0)
            {
                File.WriteAllLines(this.baseFolder + @"\mapAddrInfoNotFind_Jp.txt", lstNotFindAddr.ToArray(), Encoding.UTF8);
            }
        }

        /// <summary>
        /// 取得3dm补丁中的中文文本
        /// </summary>
        private void Get3DmText()
        {
            byte[] byCnTxt = File.ReadAllBytes(@"E:\Study\MySelfProject\Hanhua\TodoCn\TalesOfSymphonia\3DM_TEXT.BIN");
            StringBuilder sb = new StringBuilder();
            Encoding encoding = Encoding.GetEncoding("GB2312");
            bool canChgLine = false;
            for (int i = 0xc0e0; i < byCnTxt.Length; i++)
            {
                if (byCnTxt[i] >= 0xA1 && byCnTxt[i] <= 0xFE)
                {
                    if (canChgLine == false)
                    {
                        sb.Append("\n");
                        canChgLine = true;
                    }
                    sb.Append(encoding.GetString(new byte[] { byCnTxt[i], byCnTxt[i + 1] }));
                    i++;
                }
                else
                {
                    sb.Append("^" + byCnTxt[i].ToString("x") + "^");
                    canChgLine = false;
                }
            }

            File.WriteAllText(@"E:\Study\MySelfProject\Hanhua\TodoCn\TalesOfSymphonia\3dmCnText.txt", sb.ToString(), Encoding.UTF8);
        }

        /// <summary>
        /// 复制Map的中文文本
        /// </summary>
        private void CopyCnMapTxt()
        {
            Microsoft.Office.Interop.Excel.Workbook xBook = null;
            Microsoft.Office.Interop.Excel.Worksheet xSheet = null;
            Microsoft.Office.Interop.Excel.Workbook xBook2 = null;
            Microsoft.Office.Interop.Excel.Worksheet xSheet2 = null;
            List<string> enSb = new List<string>();
            List<string> cnSb = new List<string>();
            Dictionary<string, List<string>> txtMap = new Dictionary<string, List<string>>();

            try
            {
                // 创建Application对象 
                this.xApp = new Microsoft.Office.Interop.Excel.ApplicationClass();

                // 得到WorkBook对象, 打开已有的文件 
                xBook = this.xApp.Workbooks._Open(
                    @"E:\Study\MySelfProject\Hanhua\TodoCn\TalesOfSymphonia\mapText_jp.xlsx",
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                xBook2 = this.xApp.Workbooks._Open(
                    @"E:\Study\MySelfProject\Hanhua\TodoCn\TalesOfSymphonia\mapText_en.xlsx",
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                // 显示进度条
                this.ResetProcessBar(xBook.Sheets.Count);

                int sheetIndex = -1;
                int line = 1;
                for (int i = 1; i <= xBook.Sheets.Count; i++)
                {
                    // 更新进度条
                    this.ProcessBarStep();

                    xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets[i];
                    sheetIndex = -1;
                    for (int j = i; j <= xBook2.Sheets.Count; j++)
                    {
                        xSheet2 = (Microsoft.Office.Interop.Excel.Worksheet)xBook2.Sheets[j];
                        if (xSheet.Name.Equals(xSheet2.Name))
                        {
                            sheetIndex = j;
                            break;
                        }
                    }

                    if (sheetIndex == -1)
                    {
                        continue;
                    }

                    line = 1;
                    while (true)
                    {
                        Microsoft.Office.Interop.Excel.Range rngJp = xSheet.get_Range("G" + line, Missing.Value);
                        Microsoft.Office.Interop.Excel.Range rngEn = xSheet2.get_Range("G" + line, Missing.Value);
                        if (rngEn != null && !string.IsNullOrEmpty(rngEn.Value2 as string))
                        {
                            rngJp.Value2 = rngEn.Value2;
                            line++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                xSheet = null;
                xBook = null;
                xSheet2 = null;
                xBook2 = null;
                if (this.xApp != null)
                {
                    this.xApp.Quit();
                    this.xApp = null;
                }

                // 隐藏进度条
                this.CloseProcessBar();

                // 保存
                xSheet.SaveAs(
                    @"E:\Study\MySelfProject\Hanhua\TodoCn\TalesOfSymphonia\mapText_jp2.xlsx",
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
            }
            finally
            {
                // 隐藏进度条
                this.CloseProcessBar();

                // 清空各种对象
                xSheet = null;
                xBook = null;
                xSheet2 = null;
                xBook2 = null;
                if (this.xApp != null)
                {
                    this.xApp.Quit();
                    this.xApp = null;
                }
            }
        }

        /// <summary>
        /// 复制Skp的中文文本
        /// </summary>
        private void CopyCnSkpTxt()
        {
            Microsoft.Office.Interop.Excel.Workbook xBook = null;
            Microsoft.Office.Interop.Excel.Worksheet xSheet = null;
            List<string> enSb = new List<string>();
            List<string> cnSb = new List<string>();
            Dictionary<string, List<string>> txtMap = new Dictionary<string, List<string>>();

            try
            {
                // 创建Application对象 
                this.xApp = new Microsoft.Office.Interop.Excel.ApplicationClass();

                // 得到WorkBook对象, 打开已有的文件 
                xBook = this.xApp.Workbooks._Open(
                    @"E:\Study\MySelfProject\Hanhua\TodoCn\TalesOfSymphonia\TOS_skp.xls",
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                // 取得相应的Sheet
                xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets[1];
                int lineNum = 1;
                while (lineNum < 3697)
                {
                    enSb.Add(xSheet.get_Range("B" + lineNum, Missing.Value).Value2 as string);
                    cnSb.Add(xSheet.get_Range("C" + lineNum, Missing.Value).Value2 as string);
                    lineNum++;
                }

                xSheet = null;
                xBook = null;
                if (this.xApp != null)
                {
                    this.xApp.Quit();
                    this.xApp = null;
                }

                //// 创建Application对象 
                //this.xApp = new Microsoft.Office.Interop.Excel.ApplicationClass();
                //xBook = this.xApp.Workbooks._Open(
                //   @"E:\Study\MySelfProject\Hanhua\TodoCn\TalesOfSymphonia\chtText_En.xlsx",
                //   Missing.Value, Missing.Value, Missing.Value, Missing.Value
                //   , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                //   , Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                //// 显示进度条
                //this.ResetProcessBar(xBook.Sheets.Count);

                //for (int i = xBook.Sheets.Count; i >= 1; i--)
                //{
                //    // 取得相应的Sheet
                //    xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets[i];
                //    string enTxt = xSheet.get_Range("G1", Missing.Value).Value2 as string;
                //    if (string.IsNullOrEmpty(enTxt))
                //    {
                //        enTxt = string.Empty;
                //    }
                //    enTxt = enTxt.Replace("^0a^", "\n").Replace("^0^", string.Empty);

                //    for (int j = 0; j < enSb.Count; j++)
                //    {
                //        if (!string.IsNullOrEmpty(enSb[j]) && !string.IsNullOrEmpty(enTxt) && enSb[j].StartsWith(enTxt, StringComparison.OrdinalIgnoreCase))
                //        {
                //            List<string> cnItemSb = new List<string>();
                //            for (int k = j; k < j + 35 && k < cnSb.Count; k++)
                //            {
                //                cnItemSb.Add(cnSb[k]);
                //            }
                //            txtMap.Add(xSheet.Name, cnItemSb);
                //            break;
                //        }
                //    }

                //    // 更新进度条
                //    this.ProcessBarStep();
                //}

                //// 隐藏进度条
                //this.CloseProcessBar();

                //xSheet = null;
                //xBook = null;
                //if (this.xApp != null)
                //{
                //    this.xApp.Quit();
                //    this.xApp = null;
                //}

                // 创建Application对象 
                this.xApp = new Microsoft.Office.Interop.Excel.ApplicationClass();
                xBook = this.xApp.Workbooks._Open(
                   @"E:\Study\MySelfProject\Hanhua\TodoCn\TalesOfSymphonia\chtTextCnEnMap.xlsx",
                   Missing.Value, Missing.Value, Missing.Value, Missing.Value
                   , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                   , Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                // 显示进度条
                this.ResetProcessBar(xBook.Sheets.Count);

                for (int i = xBook.Sheets.Count; i >= 1; i--)
                {
                    // 取得相应的Sheet
                    xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets[i];
                    Microsoft.Office.Interop.Excel.Range rngCn = xSheet.get_Range("L1", Missing.Value);
                    string line1Val = rngCn.Value2 as string;
                    rngCn = xSheet.get_Range("L2", Missing.Value);
                    string line2Val = rngCn.Value2 as string;
                    rngCn = xSheet.get_Range("L3", Missing.Value);
                    string line3Val = rngCn.Value2 as string;
                    rngCn = xSheet.get_Range("L4", Missing.Value);
                    string line4Val = rngCn.Value2 as string;
                    rngCn = xSheet.get_Range("L5", Missing.Value);
                    string line5Val = rngCn.Value2 as string;
                    int lineNo = -1;
                    if (!string.IsNullOrEmpty(line1Val) && string.IsNullOrEmpty(line2Val) && string.IsNullOrEmpty(line3Val) && string.IsNullOrEmpty(line4Val) && string.IsNullOrEmpty(line5Val))
                    {
                        lineNo = 1;
                    }
                    else if (string.IsNullOrEmpty(line1Val) && !string.IsNullOrEmpty(line2Val) && string.IsNullOrEmpty(line3Val) && string.IsNullOrEmpty(line4Val) && string.IsNullOrEmpty(line5Val))
                    {
                        lineNo = 2;
                    }
                    else if (string.IsNullOrEmpty(line1Val) && string.IsNullOrEmpty(line2Val) && !string.IsNullOrEmpty(line3Val) && string.IsNullOrEmpty(line4Val) && string.IsNullOrEmpty(line5Val))
                    {
                        lineNo = 3;
                    }
                    else if (string.IsNullOrEmpty(line1Val) && string.IsNullOrEmpty(line2Val) && string.IsNullOrEmpty(line3Val) && !string.IsNullOrEmpty(line4Val) && string.IsNullOrEmpty(line5Val))
                    {
                        lineNo = 4;
                    }
                    else if (string.IsNullOrEmpty(line1Val) && string.IsNullOrEmpty(line2Val) && string.IsNullOrEmpty(line3Val) && string.IsNullOrEmpty(line4Val) && !string.IsNullOrEmpty(line5Val))
                    {
                        lineNo = 5;
                    }

                    if (lineNo >= 0)
                    {
                        rngCn = xSheet.get_Range("L" + lineNo, Missing.Value);
                        string lineVal = rngCn.Value2 as string;
                        int linIndex = 1;
                        for (int j = 0; j < enSb.Count; j++)
                        {
                            if (!string.IsNullOrEmpty(enSb[j]) && enSb[j].IndexOf(lineVal) >= 0)
                            {
                                int startPos = j - lineNo + 1;
                                for (int k = startPos; k < startPos + 30; k++)
                                {
                                    rngCn = xSheet.get_Range("L" + (linIndex++), Missing.Value);
                                    rngCn.Value2 = cnSb[k];
                                }
                                break;
                            }
                        }
                    }

                    // 更新进度条
                    this.ProcessBarStep();
                }

                // 隐藏进度条
                this.CloseProcessBar();

                // 保存
                xSheet.SaveAs(
                    @"E:\Study\MySelfProject\Hanhua\TodoCn\TalesOfSymphonia\chtTextCnEnMap2.xlsx",
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
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

        /// <summary>
        /// Skp的中文文本追加0
        /// </summary>
        private void AddCnSkpTxtZero()
        {
            Microsoft.Office.Interop.Excel.Workbook xBook = null;
            Microsoft.Office.Interop.Excel.Worksheet xSheet = null;

            try
            {
                // 创建Application对象 
                this.xApp = new Microsoft.Office.Interop.Excel.ApplicationClass();

                // 得到WorkBook对象, 打开已有的文件 
                xBook = this.xApp.Workbooks._Open(
                    @"E:\Study\MySelfProject\Hanhua\TodoCn\TalesOfSymphonia\chtTextCnEnMap2.xlsx",
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                // 显示进度条
                this.ResetProcessBar(xBook.Sheets.Count);

                for (int i = xBook.Sheets.Count; i >= 1; i--)
                {
                    // 取得相应的Sheet
                    xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets[i];
                    for (int j = 1; j < 50; j++)
                    {
                        Microsoft.Office.Interop.Excel.Range rngCn = xSheet.get_Range("L" + j, Missing.Value);
                        if (rngCn != null && rngCn.Value2 != null && !string.IsNullOrEmpty(rngCn.Value2 as string))
                        {
                            rngCn.Value2 = rngCn.Value2 + "^0^";
                        }
                    }

                    // 更新进度条
                    this.ProcessBarStep();
                }

                // 隐藏进度条
                this.CloseProcessBar();

                // 保存
                xSheet.SaveAs(
                    @"E:\Study\MySelfProject\Hanhua\TodoCn\TalesOfSymphonia\chtTextCnEnMap3.xlsx",
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
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

        /// <summary>
        /// 生成旧字库图片
        /// </summary>
        /// <param name="name"></param>
        private void CreateOldFontImg(string name)
        {
            string fontPath = this.baseFolder + @"\jp\root\" + name + ".dat";
            byte[] byData = File.ReadAllBytes(fontPath);
            byte[] byBlock = new byte[0x900];
            int blockCnt = byData.Length / 0x900;
            int newBlock = 4;
            int newRow = 0;
            int newCol = 0;
            int maxHeight = blockCnt % newBlock == 0 ? (24 * blockCnt / newBlock) : (24 * blockCnt / newBlock + 24);
            Bitmap fontBmp = new Bitmap(192 * newBlock, maxHeight);
            for (int i = 0; i < blockCnt; i++)
            {
                newRow = i / newBlock;
                newCol = i % newBlock;
                Array.Copy(byData, i * 0x900, byBlock, 0, byBlock.Length);
                Bitmap bmp = Util.ImageDecodeNoUseBlock(new Bitmap(192, 24), byBlock, "I4");
                for (int y = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        fontBmp.SetPixel(newCol * 192 + x, newRow * 24 + y, bmp.GetPixel(x, y));
                    }
                }
            }

            fontBmp.Save(fontPath.Replace(".dat", ".png"), System.Drawing.Imaging.ImageFormat.Png);
        }

        /// <summary>
        /// 根据配置文件，读入需要汉化的文件
        /// </summary>
        /// <returns></returns>
        private List<FilePosInfo> LoadFiles()
        {
            List<FilePosInfo> needCopyFiles = new List<FilePosInfo>();
            needCopyFiles.AddRange(base.LoadFiles(this.baseFolder + @"\mapAddrInfo_en.txt"));
            //needCopyFiles.AddRange(base.LoadFiles(this.baseFolder + @"\chtAddrInfo.txt"));
            //needCopyFiles.AddRange(base.LoadFiles(this.baseFolder + @"\startDolAddrInfo.txt"));
            return needCopyFiles;

            //List<FilePosInfo> allFiles = Util.GetAllFiles(this.baseFolder + @"\en\root\CHT\").Where(
            //    p => !p.IsFolder && p.File.EndsWith(".skp")).ToList();
            //List<string> lstAddr = new List<string>();
            //foreach (FilePosInfo fiInfo in allFiles)
            //{
            //    // 设置置文本位置信息
            //    if (this.SetTextPosInfo(fiInfo))
            //    {
            //        lstAddr.Add(fiInfo.File.Replace(this.baseFolder + @"\en\root\", string.Empty));
            //        lstAddr.Add(fiInfo.TextStart.ToString("x") + " " + fiInfo.TextEnd.ToString("x") + " " + fiInfo.EntryPos.ToString("x"));
            //    }
            //}

            //File.WriteAllLines(this.baseFolder + @"\chtAddrInfo_En.txt", lstAddr.ToArray(), Encoding.UTF8);

            //return allFiles;
        }

        /// <summary>
        /// 设置文本位置信息
        /// </summary>
        /// <param name="fiInfo"></param>
        private bool SetTextPosInfo(FilePosInfo fiInfo)
        {
            byte[] byData = File.ReadAllBytes(fiInfo.File);
            //fiInfo.TextEnd = Util.GetOffset(byData, 0x8, 0xB);

            //for (int i = 0x20; i < byData.Length; i += 2)
            //{
            //    //if (byData[i] == 0x20 && byData[i + 1] == 0xF1 && byData[i + 2] == 0x20 && byData[i + 3] == 0xFF)
            //    //{
            //    //    fiInfo.EntryPos = i + 4;
            //    //    fiInfo.TextStart = fiInfo.EntryPos + Util.GetOffset(byData, fiInfo.EntryPos, fiInfo.EntryPos + 3);
            //    //    return true;
            //    //}
            //    if (byData[i] == 0x20 && byData[i + 1] == 0xFF && byData[i + 2] == 0x0 && byData[i + 3] == 0x0
            //        && (byData[i + 4] != 0x0 || byData[i + 5] != 0x0))
            //    {
            //        fiInfo.EntryPos = i + 2;
            //        fiInfo.TextStart = fiInfo.EntryPos + Util.GetOffset(byData, fiInfo.EntryPos, fiInfo.EntryPos + 3);
            //        return true;
            //    }
            //}

            int txtStartPos = Util.GetOffset(byData, 0x1C, 0x1F);
            int txtEndPos = Util.GetOffset(byData, 0x20, 0x23);
            if (txtStartPos == 0 || txtEndPos == 0 || txtStartPos >= txtEndPos)
            {
                return false;
            }

            fiInfo.TextEnd = txtEndPos;
            for (int i = txtStartPos; i < txtEndPos; i++)
            {
                if (byData[i] == 0x20 && (byData[i + 1] == 0xFF || byData[i + 1] == 0x03) && byData[i + 2] == 0x0 && byData[i + 3] == 0x0
                    && (byData[i + 4] != 0x0 || byData[i + 5] != 0x0) && byData[i + 6] == 0x0 && byData[i + 7] == 0x0)
                {
                    fiInfo.EntryPos = i + 2;
                    fiInfo.TextStart = fiInfo.EntryPos + Util.GetOffset(byData, fiInfo.EntryPos, fiInfo.EntryPos + 3);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 开始解码
        /// </summary>
        /// <param name="byData"></param>
        /// <param name="filePosInfo"></param>
        private string DecodeText(byte[] byData, FilePosInfo filePosInfo)
        {
            if (filePosInfo.File.EndsWith(".dol", StringComparison.OrdinalIgnoreCase))
            {
                Encoding encoding = Encoding.GetEncoding("Shift-jis");
                byte[] byTemp = new byte[filePosInfo.TextEnd - filePosInfo.TextStart];
                Array.Copy(byData, filePosInfo.TextStart, byTemp, 0, byTemp.Length);
                return this.ResetText(Util.EncodeByteArray(byTemp, encoding))
                    .Replace("^0^^0^^0^^0^", "^0 0 0 0^\n")
                    .Replace("^0^^0^", "^0 0^\n");
            }
            else
            {
                return this.ResetText(this.DecodeSkpText(byData, filePosInfo));
            }
        }

        /// <summary>
        /// 开始解码
        /// </summary>
        /// <param name="byData"></param>
        /// <param name="filePosInfo"></param>
        private string DecodeSkpText(byte[] byData, FilePosInfo filePosInfo)
        {
            StringBuilder sb = new StringBuilder();
            int txtStart = filePosInfo.TextStart;
            Encoding encoding = Encoding.GetEncoding("Shift-jis");
            byte[] byTemp = null;

            try
            {
                for (int j = filePosInfo.EntryPos + 4; j < filePosInfo.TextStart; j += 4)
                {
                    int startPos = txtStart;
                    int endPos = filePosInfo.EntryPos + Util.GetOffset(byData, j, j + 3);
                    txtStart = endPos;

                    byTemp = new byte[endPos - startPos];
                    Array.Copy(byData, startPos, byTemp, 0, byTemp.Length);
                    sb.Append(Util.EncodeByteArray(byTemp, encoding));

                    sb.Append("\n");
                }

                if (filePosInfo.TextEnd > txtStart)
                {
                    byTemp = new byte[filePosInfo.TextEnd - txtStart];
                    Array.Copy(byData, txtStart, byTemp, 0, byTemp.Length);
                    sb.Append(Util.EncodeByteArray(byTemp, encoding));
                }
            }
            catch
            {
                return "发生错误：\n" + filePosInfo.EntryPos.ToString("x") + " " + filePosInfo.TextStart.ToString("x");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 重新替换关键字
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        private string ResetText(string txt)
        {
            return txt.Replace("^03^8", "^03 38^")
                .Replace("^03^9", "^03 39^");
        }

        #endregion
       
    }
}
