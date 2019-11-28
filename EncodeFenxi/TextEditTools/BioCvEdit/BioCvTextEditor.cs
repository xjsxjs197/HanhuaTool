using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hanhua.BioTools.BioCvEdit;
using Hanhua.Common;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Hanhua.TextEditTools.BioCvEdit
{
    /// <summary>
    /// 生化危机维罗妮卡文本编辑器
    /// </summary>
    public partial class BioCvTextEditor : BaseTextEditor
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
        /// 全局变量
        /// </summary>
        private int maxFindLen = 0;

        private BioCvTextAlignEdit alignEdit;

        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        public BioCvTextEditor()
        {
            InitializeComponent();

            this.ResetHeight();

            this.gameName = "BioCv";
            //this.baseFolder = @"E:\Study\Hanhua\TodoCn\BioCv";
            this.baseFolder = @"E:\游戏汉化\NgcBioCv";
            
            this.SetDcLoadStatus(false);

            // 初始化
            this.EditorInit(true);
        }

        #region " 事件 "

        /// <summary>
        /// 切换类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoNgc_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdoNgc.Checked)
            {
                this.SetNgcLoadStatus(true);

                this.SetDcLoadStatus(false);
            }
            else
            {
                this.SetNgcLoadStatus(false);

                this.SetDcLoadStatus(true);
            }
        }

        /// <summary>
        /// 生成字库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateFont_Click(object sender, EventArgs e)
        {
            //this.Do(this.CreateFont);
            //this.Do(this.ImportCnFont);
            this.Do(this.CreateConfigPic);
        }

        /// <summary>
        /// 合成Rdx
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAfsTool_Click(object sender, EventArgs e)
        {
            this.Do(this.CreateAfs);
            //this.alignEdit = new BioCvTextAlignEdit(this.subDisk);
            //this.alignEdit.Show();
            //this.Do(this.AutoAddSpace);
        }

        /// <summary>
        /// 换盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoADisk_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdoADisk.Checked)
            {
                this.subDisk = "A";
            }
            else
            {
                this.subDisk = "B";
            }
        }

        #endregion

        #region " 重写父类方法 "

        /// <summary>
        /// 生成打包文件
        /// </summary>
        protected override void CreatePatch()
        {
            string srcFolder = this.baseFolder + @"\BioCvNgcCn\" + this.subDisk + @"\root\";
            string targetFolder = srcFolder.Replace("BioCvNgcCn", "Patch");
            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            File.Copy(srcFolder + "adv.afs", targetFolder + "adv.afs", true);
            File.Copy(srcFolder + "item.afs", targetFolder + "item.afs", true);
            File.Copy(srcFolder + "mry.afs", targetFolder + "mry.afs", true);
            File.Copy(srcFolder + "system.afs", targetFolder + "system.afs", true);
            File.Copy(srcFolder + "sysmes.ald", targetFolder + "sysmes.ald", true);

            string rdxName = "rdx_lnk" + (this.subDisk.Equals("A", StringComparison.OrdinalIgnoreCase) ? "1" : "2") + ".afs";
            File.Copy(srcFolder + rdxName, targetFolder + rdxName, true);
        }

        /// <summary>
        /// 读取字库信息
        /// </summary>
        protected override void ReadFontChar()
        {
            // 读取日文字库
            this.ReadFontFile(@"..\EncodeFenxi\BioTools\BioCvEdit\JpFontMap.txt", this.jpFontCharPage, Encoding.UTF8);

            // 读取中文字库
            this.ReadFontFile(@"..\EncodeFenxi\BioTools\BioCvEdit\CnFontMap" + this.subDisk + ".txt", this.cnFontCharPage, Encoding.GetEncoding("GB2312"));
        }

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
            if (this.chkNgcSysmes.Checked)
            {
                for (int i = 0; i < 15; i++)
                {
                    if (i <= 1)
                    {
                        jpFile = this.baseFolder + @"\BioCvNgcJp\" + this.subDisk + @"\root\sysmes.ald";
                    }
                    else if (i <= 4)
                    {
                        jpFile = this.baseFolder + @"\BioCvNgcJp\" + this.subDisk + @"\root\adv.afs";
                    }
                    else
                    {
                        jpFile = this.baseFolder + @"\BioCvNgcJp\" + this.subDisk + @"\root\mry.afs";
                    }

                    this.AddFile(jpFile, allFiles[i]);
                }

                allFiles.RemoveRange(0, 15);
            }
            else if (this.chkDcSysmes.Checked)
            {
                jpFile = this.baseFolder + @"\BioCvDcJp\" + this.subDisk + @"\sysmes.ald";
                this.AddFile(jpFile, allFiles[0]);
                allFiles.RemoveAt(0);
            }

            if (this.chkNgcRdx.Checked)
            {
                foreach (FilePosInfo fileInfo in allFiles)
                {
                    jpFile = this.baseFolder + @"\BioCvNgcJp\" + this.subDisk + @"\root\rdx_lnk" + this.subDisk + @"\" + fileInfo.File;

                    // 取得各个文件名
                    if (File.Exists(jpFile))
                    {
                        this.AddFile(jpFile, fileInfo);
                    }
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
                string cnText = this.DecodeText(File.ReadAllBytes(this.cnFile), currentFileInfo, this.cnFontCharPage);
                //return this.DecodeCnText(this.cnFile, cnText, currentFileInfo);
                if (this.alignEdit != null)
                {
                    this.alignEdit.AddText(cnText);
                }
                return cnText;
            }
            else
            {
                //string temp = "刑務所";
                //StringBuilder sb = new StringBuilder();
                //for (int i = 0; i < temp.Length; i++)
                //{
                //    string curChar = temp.Substring(i, 1);
                //    byte[] byChar = this.EncodeChar(curChar);
                //    sb.Append(byChar[0].ToString("x")).Append(" ");
                //    sb.Append(byChar[1].ToString("x")).Append(" ");
                //}

                return this.DecodeText(File.ReadAllBytes(currentFileInfo.File), currentFileInfo, this.jpFontCharPage);
            }
        }

        /// <summary>
        /// 取得当前文字的编码
        /// </summary>
        /// <param name="currenChar">当前文字</param>
        /// <returns>当前文字的编码</returns>
        protected override byte[] EncodeChar(string currentChar)
        {
            if (this.chkNgcRdx.Checked || this.chkNgcSysmes.Checked)
            {
                // 在字库中查找
                foreach (int fontPage in this.cnFontCharPage.Keys)
                {
                    string[] pageFonts = this.cnFontCharPage[fontPage];
                    for (int i = 0; i < pageFonts.Length; i++)
                    {
                        if (currentChar == pageFonts[i])
                        {
                            return new byte[] { (byte)fontPage, (byte)i };
                        }
                    }
                }
            }
            else
            {
                throw new Exception("DcBioCv不能被保存");
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
            // Entry的第一个是个数信息
            currentFileInfo.TextEntrys.Insert(0, currentFileInfo.TextEntrys.Count);

            // 带Entry的文本，先保存修改后的各个Entry
            byte[] byCnData = new byte[currentFileInfo.TextEntrys.Count * 4 + cnBytes.Count];
            for (int i = 0; i < currentFileInfo.TextEntrys.Count; i++)
            {
                int entryPos = currentFileInfo.TextEntrys[i];
                if (i > 0)
                {
                    entryPos += currentFileInfo.TextEntrys.Count * 4;
                }
                byCnData[i * 4] = (byte)((entryPos >> 24) & 0xFF);
                byCnData[i * 4 + 1] = (byte)((entryPos >> 16) & 0xFF);
                byCnData[i * 4 + 2] = (byte)((entryPos >> 8) & 0xFF);
                byCnData[i * 4 + 3] = (byte)(entryPos & 0xFF);
            }

            // 再保存文本数据
            Array.Copy(cnBytes.ToArray(), 0, byCnData, currentFileInfo.TextEntrys.Count * 4, cnBytes.Count);

            // 重新设置文本开始位置
            currentFileInfo.TextStart = currentFileInfo.EntryPos;

            return byCnData;
        }

        /// <summary>
        /// 检查输入的中文长度是否正确
        /// </summary>
        /// <param name="chkKeyWords">是否需要检查关键字</param>
        /// <returns>输入的中文长度是否正确</returns>
        protected override bool CheckCnText(bool chkKeyWords)
        {
            //return base.CheckCnText(false);
            return true;
        }

        #endregion

        #region " 私有方法 "

        private void CheckFont()
        {
            string[] fontChars = File.ReadAllLines(this.baseFolder + @"\DcCnText\A\CharMap.tbl", Encoding.GetEncoding("GB2312"));
            string[] fontCharsB = File.ReadAllLines(this.baseFolder + @"\DcCnText\B\CharMap.tbl", Encoding.GetEncoding("GB2312"));

            StringBuilder sb = new StringBuilder();
            foreach (string charItem in fontCharsB)
            {
                if (charItem.IndexOf("黑") < 0
                    && !fontChars.Contains(charItem))
                {
                    sb.Append(charItem);
                }
            }

            string temp = sb.ToString();
        }

        private void CheckConfigPic()
        {
            Bitmap bmp = new Bitmap(640, 480);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.Transparent);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingMode = CompositingMode.SourceOver;
            g.CompositingQuality = CompositingQuality.GammaCorrected;

            Bitmap confPic = new Bitmap(this.baseFolder + @"\BioCvNgcCn\Pic\optionNgc\adv_09.png");
            int xPos = 0;
            int yPos = 0;

            for (int y = yPos; y < 220; y++)
            {
                for (int x = xPos; x < 320; x++)
                {
                    bmp.SetPixel(x, y, confPic.GetPixel(x, y));
                }
            }

            yPos += 220;
            int yTemp = 120;
            for (int y = yPos; y < yPos + 100; y++)
            {
                for (int x = xPos; x < 320; x++)
                {
                    bmp.SetPixel(x + 320, yTemp, confPic.GetPixel(x, y));
                }
                yTemp++;
            }

            yPos += 100;
            yTemp = 220;
            for (int y = yPos; y < yPos + 192; y++)
            {
                for (int x = xPos; x < 320; x++)
                {
                    bmp.SetPixel(x, yTemp, confPic.GetPixel(x, y));
                }
                yTemp++;
            }

            xPos += 320;
            yPos = 0;
            yTemp = 220;
            for (int y = yPos; y < yPos + 104; y++)
            {
                for (int x = xPos; x < xPos + 80; x++)
                {
                    bmp.SetPixel(x, yTemp, confPic.GetPixel(x, y));
                }
                yTemp++;
            }

            yPos += 104;
            yTemp = 220;
            int xTemp = 400;
            for (int y = yPos; y < yPos + 104; y++)
            {
                xTemp = 400;
                for (int x = xPos; x < xPos + 80; x++)
                {
                    bmp.SetPixel(xTemp++, yTemp, confPic.GetPixel(x, y));
                }
                yTemp++;
            }

            yPos += 104;
            yTemp = 220;
            xTemp = 480;
            for (int y = yPos; y < yPos + 104; y++)
            {
                xTemp = 480;
                for (int x = xPos; x < xPos + 80; x++)
                {
                    bmp.SetPixel(xTemp++, yTemp, confPic.GetPixel(x, y));
                }
                yTemp++;
            }

            yPos += 104;
            yTemp = 220;
            xTemp = 560;
            for (int y = yPos; y < yPos + 100; y++)
            {
                xTemp = 560;
                for (int x = xPos; x < xPos + 80; x++)
                {
                    bmp.SetPixel(xTemp++, yTemp, confPic.GetPixel(x, y));
                }
                yTemp++;
            }

            bmp.Save(this.baseFolder + @"\BioCvNgcCn\Pic\optionNgc\adv_09_cn.png");
        }

        private void CreateConfigPic()
        {
            string strCnPic = this.baseFolder + @"\BioCvNgcCn\Pic\optionNgc\adv_09_cn.png";
            Bitmap confPic = new Bitmap(strCnPic);
            Bitmap bmp = new Bitmap(512, 512);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.Transparent);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingMode = CompositingMode.SourceOver;
            g.CompositingQuality = CompositingQuality.GammaCorrected;

            int xPos = 0;
            int yPos = 0;

            for (int y = yPos; y < 220; y++)
            {
                for (int x = xPos; x < 320; x++)
                {
                    bmp.SetPixel(x, y, confPic.GetPixel(x, y));
                }
            }

            yPos += 220;
            int yTemp = 120;
            for (int y = yPos; y < yPos + 100; y++)
            {
                for (int x = xPos; x < 320; x++)
                {
                    bmp.SetPixel(x, y, confPic.GetPixel(x + 320, yTemp));
                }
                yTemp++;
            }

            yPos += 100;
            yTemp = 220;
            for (int y = yPos; y < yPos + 192; y++)
            {
                for (int x = xPos; x < 320; x++)
                {
                    bmp.SetPixel(x, y, confPic.GetPixel(x, yTemp));
                }
                yTemp++;
            }

            xPos += 320;
            yPos = 0;
            yTemp = 220;
            for (int y = yPos; y < yPos + 104; y++)
            {
                for (int x = xPos; x < xPos + 80; x++)
                {
                    bmp.SetPixel(x, y, confPic.GetPixel(x, yTemp));
                }
                yTemp++;
            }

            yPos += 104;
            yTemp = 220;
            int xTemp = 400;
            for (int y = yPos; y < yPos + 104; y++)
            {
                xTemp = 400;
                for (int x = xPos; x < xPos + 80; x++)
                {
                    bmp.SetPixel(x, y, confPic.GetPixel(xTemp++, yTemp));
                }
                yTemp++;
            }

            yPos += 104;
            yTemp = 220;
            xTemp = 480;
            for (int y = yPos; y < yPos + 104; y++)
            {
                xTemp = 480;
                for (int x = xPos; x < xPos + 80; x++)
                {
                    bmp.SetPixel(x, y, confPic.GetPixel(xTemp++, yTemp));
                }
                yTemp++;
            }

            yPos += 104;
            yTemp = 220;
            xTemp = 560;
            for (int y = yPos; y < yPos + 100; y++)
            {
                xTemp = 560;
                for (int x = xPos; x < xPos + 80; x++)
                {
                    bmp.SetPixel(x, y, confPic.GetPixel(xTemp++, yTemp));
                }
                yTemp++;
            }

            bmp.Save(strCnPic.Replace("adv_09_cn", "adv_09_cnImp"));
        }

        /// <summary>
        /// 检查文本错误
        /// </summary>
        private void CheckImpTextError()
        {
            string chkFile = this.baseFolder + @"\NgcBioCv" + this.subDisk + "_AddSpace.xlsx";

            Microsoft.Office.Interop.Excel.Workbook xBook = null;
            Microsoft.Office.Interop.Excel.Worksheet xSheet = null;

            try
            {
                StringBuilder failFiles = new StringBuilder();

                // 创建Application对象 
                this.xApp = new Microsoft.Office.Interop.Excel.ApplicationClass();

                // 得到WorkBook对象, 打开已有的文件 
                xBook = this.xApp.Workbooks._Open(
                    chkFile,
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                // 显示进度条
                this.ResetProcessBar(xBook.Sheets.Count);

                for (int i = xBook.Sheets.Count; i >= 1; i--)
                {
                    // 取得相应的Sheet
                    xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets[i];


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
                            //if (Regex.IsMatch(cellValue, @"\^ff ff\^\S+$", RegexOptions.RightToLeft))
                            if (Regex.IsMatch(cellValue, @"\^ff 2 \S{1,2} \S{1,2}[^\^]\^\^f", RegexOptions.IgnoreCase))
                            {
                                failFiles.Append(xSheet.Name).Append("\r\n");
                                failFiles.Append(cellValue).Append("\r\n");
                            }

                            blankNum = 0;
                        }

                        lineNum++;
                    }

                    sb = sb.Replace("\n\n\n\n\n", "\n");

                    // 更新进度条
                    this.ProcessBarStep();
                }

                if (failFiles.Length > 0)
                {
                    File.WriteAllText(this.baseFolder + @"\NgcBioCv" + this.subDisk + "_ChkEnd.txt", failFiles.ToString());
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
        /// 自动追加空行
        /// </summary>
        private void AutoAddSpace()
        {
            Microsoft.Office.Interop.Excel.Workbook xBook = null;
            Microsoft.Office.Interop.Excel.Worksheet xSheet = null;
            Microsoft.Office.Interop.Excel.Application xApp = null;
            string errorLine = string.Empty;

            try
            {
                // 创建Application对象 
                xApp = new Microsoft.Office.Interop.Excel.ApplicationClass();

                // 得到WorkBook对象, 打开已有的文件 
                xBook = xApp.Workbooks._Open(
                    this.baseFolder + @"\NgcBioCv" + this.subDisk + ".xlsx",
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                StringBuilder sbTooLang = new StringBuilder();

                // 显示进度条
                this.ResetProcessBar(xBook.Sheets.Count);

                for (int i = xBook.Sheets.Count; i >= 1; i--)
                {
                    // 取得当前Sheet的中文文本
                    int lineNum = 1;
                    int blankNum = 0;
                    xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets[i];
                    sbTooLang.Append(xSheet.Name).Append("\r\n");
                    while (blankNum < 4)
                    {
                        Microsoft.Office.Interop.Excel.Range rngCn = xSheet.get_Range("G" + lineNum, Missing.Value);
                        string cellValue = rngCn.Value2 as string;
                        if (string.IsNullOrEmpty(cellValue))
                        {
                            blankNum++;
                        }
                        else
                        {
                            blankNum = 0;
                            errorLine = cellValue;
                            rngCn.Value2 = this.AutoAddSpace(cellValue, sbTooLang);
                        }

                        lineNum++;
                    }

                    // 更新进度条
                    this.ProcessBarStep();
                }

                // 保存
                xSheet.SaveAs(
                    this.baseFolder + @"\NgcBioCv" + this.subDisk + "_AddSpace.xlsx",
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                // 隐藏进度条
                this.CloseProcessBar();

                File.WriteAllText(this.baseFolder + @"\NgcBioCv" + this.subDisk + "_TooLang.txt", sbTooLang.ToString(), Encoding.UTF8);

                // 显示保存完成信息
                MessageBox.Show("空格处理完成！");
            }
            catch (Exception exp)
            {
                MessageBox.Show(errorLine + "\n" + exp.Message + "\n" + exp.StackTrace);
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

        private string ReplaceKeyWord(Match m)
        {
            return m.Value + "\n";
        }

        /// <summary>
        /// 自动追加空格(每行最多18个汉字)
        /// </summary>
        /// <param name="oldText"></param>
        /// <returns></returns>
        private string AutoAddSpace(string oldText, StringBuilder sbTooLang)
        {
            oldText = oldText.Replace("^ff 0^", "^ff 0^\n");
            oldText = Regex.Replace(oldText, @"\^ff 2 \S+ \S+\^\^fe 8\^", this.ReplaceKeyWord);
            string[] lineTexts = oldText.Split('\n');
            StringBuilder sb = new StringBuilder();
            int Line_type = 0;
            int tempPos = 0;
            int InstrWd = -1;
            int InstrWd2 = -1;
            int InstrWd3 = -1;
            int Blk_F = 0;
            int Blk_B = 0;
            int Chr_NF = 0;
            int Chr_NB = 0;
            string Line_Front = "";
            string Line_F2B = "";
            string Line_F2W = "";
            string Line_Back = "";
            string Line_B2B = "";
            string Line_B2W = "";

            foreach (string Txt_Line in lineTexts)
            {
                if (string.IsNullOrEmpty(Txt_Line)
                    || Txt_Line == "^fe 0^" || Txt_Line == "^ff 0^" || Txt_Line == "^fe 0^^ff 0^" 
                    || Txt_Line.IndexOf("Ｙｅｓ") >= 0 || Txt_Line.IndexOf("^fe 9^^ff 4^") >= 0
                    || Txt_Line.IndexOf("Ｂ１Ｆ") >= 0 || Txt_Line.IndexOf("同时间^ff 1^克里斯·雷") >= 0
                    || Txt_Line.IndexOf("^ff 4^左") >= 0 || Txt_Line.IndexOf("^ff 4^★") >= 0
                    || Txt_Line.IndexOf("^ff 4^Ａ") >= 0 || Txt_Line.IndexOf("^ff 4^蓝") >= 0
                    || Txt_Line == "^ff ff^")
                {
                    sb.Append(Txt_Line);
                    continue;
                }

                Line_type = 0;
                InstrWd = Txt_Line.IndexOf("^ff 2");
                InstrWd2 = Txt_Line.IndexOf("^fe 8");
                InstrWd3 = InstrWd + 10;
                tempPos = InstrWd + 10;

                if (InstrWd >= 0)
                {
                    Line_type = 3; // '含有{02FF:0000}的行
                }

                if (Line_type == 3)
                {
                    if (Txt_Line.EndsWith("^ff 2 0 0^"))
                    {
                        Line_type = 4; // '末尾是{02FF:0000}的行
                    }
                    else if (Regex.IsMatch(Txt_Line, @"\^ff 2 \S+ \S+\^$", RegexOptions.RightToLeft))
                    {
                        Line_type = 4; // '末尾是{02FF:XXXX}的行
                    }
                    else if (Regex.IsMatch(Txt_Line, @"\^ff 2 \S+ \S+\^\^ff ff\^$", RegexOptions.RightToLeft))
                    {
                        Line_type = 4; // '末尾是{02FF:XXXX}{end}的行
                    }
                    else if (Regex.IsMatch(Txt_Line, @"\^ff 2 \S+ \S+\^\^fe 8\^$", RegexOptions.RightToLeft))
                    {
                        Line_type = 4; // '末尾是{02FF:XXXX}{08FE}的行
                    }
                    else if (Regex.IsMatch(Txt_Line, @"\^ff 2 \S+ \S+\^\^ff 0\^$", RegexOptions.RightToLeft))
                    {
                        Line_type = 4; // '末尾是{02FF:XXXX}{00ff}的行
                    }
                    else if (Regex.IsMatch(Txt_Line, @"\^ff 2 \S+ \S+\^\^fe 8\^\^ff ff\^$", RegexOptions.RightToLeft))
                    {
                        Line_type = 4; // '末尾是{02FF:XXXX}{08FE}{end}的行
                    }
                    else if (InstrWd2 >= 0 && Txt_Line.IndexOf("^ff ff^") < 0)
                    {
                        Line_type = 8; // 含有{02FF:XXXX}和{08FE}但是不含有{end}的行
                        InstrWd3 = InstrWd2 + 6;
                        tempPos = InstrWd2 + 6;
                    }
                }
                
                if (Line_type == 0)
                {
                    Line_type = 7; // 正常要加空格的行
                }


                if (Line_type == 3 || Line_type == 8)
                {
                    Line_Front = Txt_Line.Substring(0, InstrWd3);
                    Line_Back = Txt_Line.Substring(tempPos);
                    Chr_NF = 0;
                    Chr_NB = 0;
                    Blk_F = 0;
                    Blk_B = 0;
                    Line_F2B = "";
                    Line_B2B = "";
                    // ================================================================
                    // 数汉字数
                    for (int i = 0; i < Line_Front.Length; i++)
                    {
                        if (System.Text.Encoding.UTF8.GetByteCount(Line_Front.Substring(i, 1)) > 1)
                        {
                            Chr_NF++;
                        }
                    }
                    for (int i = 0; i < Line_Back.Length; i++)
                    {
                        if (System.Text.Encoding.UTF8.GetByteCount(Line_Back.Substring(i, 1)) > 1)
                        {
                            Chr_NB++;
                        }
                    }
                    if (Line_Front.IndexOf("^ff 3 ") >= 0)
                    {
                        Chr_NF += 5;
                    }
                    if (Line_Back.IndexOf("^ff 3 ") >= 0)
                    {
                        Chr_NB += 5;
                    }
                    // ================================================================
                    // 数空格数
                    Blk_F += Regex.Matches(Line_Front, "ff 1").Count;
                    Blk_B += Regex.Matches(Line_Back, "ff 1").Count;
                    // ================================================================
                    if ((Blk_F + Chr_NF) > 18 || (Blk_B + Chr_NB) > 18) 
                    {
                        sbTooLang.Append(Txt_Line).Append("\r\n");
                        sb.Append(Txt_Line);
                        continue;
                    }
                    int diff = 0;
                    if (Chr_NF > 0)
                    {
                        diff = (18 - Chr_NF) / 2 - Blk_F;
                        while (diff-- > 0)
                        {
                            Line_F2B = "^ff 1^" + Line_F2B;
                        }
                        Line_F2W = Line_F2B + Line_Front;
                    }
                    else
                    {
                        Line_F2W = Line_Front;
                    }

                    if (Chr_NB > 0)
                    {
                        diff = (18 - Chr_NB) / 2 - Blk_B;
                        while (diff-- > 0)
                        {
                            Line_B2B = "^ff 1^" + Line_B2B;
                        }
                        Line_B2W = Line_B2B + Line_Back;
                    }
                    else
                    {
                        Line_B2W = Line_Back;
                    }

                    sb.Append(Line_F2W + Line_B2W);
                }
                else if (Line_type == 4 || Line_type == 7)
                {
                    Line_Front = Txt_Line;
                    Chr_NF = 0;
                    Blk_F = 0;
                    Line_F2B = "";
                    // ================================================================
                    // 数汉字数
                    for (int i = 0; i < Line_Front.Length; i++)
                    {
                        if (System.Text.Encoding.UTF8.GetByteCount(Line_Front.Substring(i, 1)) > 1)
                        {
                            Chr_NF++;
                        }
                    }
                    if (Line_Front.IndexOf("^ff 3 ") >= 0)
                    {
                        Chr_NF += 5;
                    }
                    // ================================================================
                    // 数空格数
                    Blk_F += Regex.Matches(Line_Front, "ff 1").Count;
                    // ================================================================
                    if ((Blk_F + Chr_NF) > 18)
                    {
                        sbTooLang.Append(Txt_Line).Append("\r\n");
                        sb.Append(Txt_Line);
                        continue;
                    }
                    else if (Chr_NF == 0)
                    {
                        sb.Append(Txt_Line);
                        continue;
                    }
                    int diff = (18 - Chr_NF) / 2 - Blk_F;
                    while (diff-- > 0)
                    {
                        Line_F2B = "^ff 1^" + Line_F2B;
                    }
                    Line_F2W = Line_F2B + Line_Front;
                    sb.Append(Line_F2W);
                }
            }

            //return sb.ToString().Substring(0, sb.Length - 6);
            return sb.ToString();
        }

        /// <summary>
        /// 导入中文字库
        /// </summary>
        private void ImportCnFont()
        {
            string fontFile = this.baseFolder + @"\BioCvNgcCn\" + this.subDisk + @"\root\system.afs";
            byte[] byFontFile = File.ReadAllBytes(fontFile);
            byte[] byJpFontFile = File.ReadAllBytes(fontFile.Replace("BioCvNgcCn", "BioCvNgcJp"));
            string[] fontPicAddr = new string[] { "3180", "231a0", "23220", "43240", "432c0", "632e0", "63360", "83380" };

            //Bitmap oldPic = new Bitmap(this.baseFolder + @"\BioCvNgcCn\Pic\SysPicNgc\A\system_00.png");
            //List<byte> byPalette = new List<byte>();
            //List<byte> byAllPalette = this.GetPaletteColor(oldPic, byPalette);

            // 显示进度条
            this.ResetProcessBar(4);

            //byte[] byTemp = new byte[0x10];
            //byte[] byBap = new byte[0x50];
            //byBap[0] = 0x62;
            //byBap[1] = 0x61;
            //byBap[2] = 0x70;
            //byBap[8] = 0x10;

            for (int i = 0; i < 4; i++)
            {
                int impStartPos = Convert.ToInt32(fontPicAddr[i * 2], 16);
                int impEndPos = Convert.ToInt32(fontPicAddr[i * 2 + 1], 16);

                //Bitmap cnPic = new Bitmap(this.baseFolder + @"\BioCvNgcCn\Pic\SysPicNgc\" + this.subDisk + @"\system_0" + i + ".png");
                //List<byte> byPalette = this.GetPaletteColor(dcCnPic);
                //List<byte> byAllPalette = this.GetPaletteColor(byPalette);

                Bitmap cnPic = new Bitmap(this.baseFolder + @"\BioCvNgcCn\Pic\NewFont\" + this.subDisk + @"\NewFont" + i + ".png");
                //List<byte> byPalette = new List<byte>();
                //List<byte> byAllPalette = this.GetPaletteColor(pageImg, byPalette);
                //byte[] byNewImg = Util.PaletteImageEncode(pageImg, "C4_CI4", byPalette, 2);
                byte[] byTmpPalette = new byte[0x10];
                byte[] byTmpAllPalette = new byte[0x20];
                Array.Copy(byJpFontFile, impStartPos - 0x50, byTmpPalette, 0, byTmpPalette.Length);
                Array.Copy(byJpFontFile, impStartPos - 0x50, byTmpAllPalette, 0, byTmpAllPalette.Length);
                List<byte> byPalette = byTmpPalette.ToList();
                List<byte> byAllPalette = byTmpAllPalette.ToList();

                byte[] byNewImg = Util.PaletteImageEncode(cnPic, "C4_CI4", byPalette, 2);

                //Array.Copy(byFontFile, impStartPos - 0x50, byTemp, 0, byTemp.Length);
                //Color[] colorPalette = Util.GetPalette(2, byTemp);
                //for (int j = 0; j < colorPalette.Length; j++)
                //{
                //    byBap[0x10 + j * 4 + 0] = colorPalette[j].R;
                //    byBap[0x10 + j * 4 + 1] = colorPalette[j].G;
                //    byBap[0x10 + j * 4 + 2] = colorPalette[j].B;
                //    byBap[0x10 + j * 4 + 3] = colorPalette[j].A;
                //}

                //File.WriteAllBytes(this.baseFolder + @"\BioCvNgcCn\Pic\NewFont\" + this.subDisk + @"\NewFont" + i + ".Bap", byBap);

                Array.Copy(byNewImg, 0, byFontFile, impStartPos + 0x20, byNewImg.Length);
                Array.Copy(byAllPalette.ToArray(), 0, byFontFile, impStartPos - 0x50, byAllPalette.Count);

                // 更新进度条
                this.ProcessBarStep();
            }

            File.WriteAllBytes(fontFile, byFontFile);

            // 关闭进度条
            this.CloseProcessBar();
        }

        /// <summary>
        /// 调色板编码
        /// </summary>
        /// <param name="palette"></param>
        /// <returns></returns>
        private byte[] PaletteEncode(int[] palette)
        {
            UInt16 temp;
            List<byte> byPalette = new List<byte>();
            foreach (int item in palette)
            {
                Color color = Color.FromArgb(item);
                if (color.A > 0xDA)
                {
                    // not use alpha 5,5,5
                    temp = (UInt16)(Util.Convert8To5(color.R) << 10 | Util.Convert8To5(color.G) << 5 | Util.Convert8To5(color.B));
                    byPalette.Add((byte)((temp >> 8) | 0x80));
                    byPalette.Add((byte)(temp & 0xFF));
                }
                else
                {
                    // use alpha 3,4,4,4
                    byPalette.Add((byte)(Util.Convert8To3(color.A) << 4 | Util.Convert8To4(color.R) & 0x7F));
                    byPalette.Add((byte)(Util.Convert8To4(color.G) << 4 | Util.Convert8To4(color.B)));
                }
            }

            for (int i = 0; i < 7; i++)
            {
                byPalette.Add(0xFC);
                byPalette.Add(0);
            }

            byPalette.Add(0xFF);
            byPalette.Add(0xE0);

            return byPalette.ToArray();
        }

        /// <summary>
        /// 字库图片编码
        /// </summary>
        /// <param name="input"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private byte[] EncodeFontImg(byte[] input, int width, int height)
        {
            int offset = 0;
            byte[] output = new byte[(width * height) / 2];

            for (int y = 0; y < height; y += 8)
            {
                for (int x = 0; x < width; x += 8)
                {
                    for (int y2 = 0; y2 < 8; y2++)
                    {
                        for (int x2 = 0; x2 < 8; x2++)
                        {
                            byte entry = (byte)(input[((y + y2) * width) + (x + x2)] & 0x0F);
                            entry = (byte)((output[offset] & (0x0F << (x2 & 0x01) * 4)) | (entry << ((~x2 & 0x01) * 4)));

                            output[offset] = entry;

                            if ((x2 & 0x01) != 0)
                                offset++;
                        }
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// 生成字库
        /// </summary>
        private void CreateFont()
        {
            string charFile = this.baseFolder + @"\DcCnText\" + this.subDisk + @"\CharMap.tbl";
            string[] fontChars = File.ReadAllLines(charFile, Encoding.GetEncoding("GB2312"));
            int fontPicIndex = 0;
            int charIndex = 0;
            int pageCharIndex = 0;
            string useOldPic = "▲▼。：★○◎△▲■＇”";
            Bitmap oldPic = new Bitmap(this.baseFolder + @"\BioCvNgcCn\Pic\SysPicNgc\A\system_00.png");

            Bitmap bmp = null;
            Bitmap bmp2 = null;
            Graphics g = null;
            Graphics g2 = null;
            this.NewFontPic(ref bmp, ref g, ref bmp2, ref g2);
            
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            
            Pen blackPen = new Pen(Color.FromArgb(0x42, 0x42, 0x42), 1F);
            blackPen.SetLineCap(LineCap.Round, LineCap.Round, DashCap.Round);
            blackPen.LineJoin = LineJoin.Round;

            SolidBrush brush = new SolidBrush(Color.FromArgb(0xCE, 0xCE, 0xBD));

            FontFamily ff = new FontFamily("宋体");
            //FontFamily ff = new FontFamily("SimSun");
            Font font = new Font(ff, 23F);

            // 显示进度条
            this.ResetProcessBar(fontChars.Length);

            foreach (string fontCharMap in fontChars)
            {
                if (string.IsNullOrEmpty(fontCharMap))
                {
                    break;
                }

                string fontChar = fontCharMap.Split('=')[1];

                if (fontPicIndex == 0 && useOldPic.Contains(fontChar))
                {
                    this.SetOldCharPic(bmp, oldPic, pageCharIndex);
                }
                else
                {
                    GraphicsPath graphPath = new GraphicsPath();
                    graphPath.FillMode = FillMode.Winding;
                    //graphPath.Widen(blackPen);
                    RectangleF rectangle = new RectangleF((pageCharIndex % 18) * 28 + 1, (pageCharIndex / 18) * 28 + 1, 27f, 27f);
                    graphPath.AddString(fontChar, ff, (int)FontStyle.Bold, 23F, rectangle, sf);
                    g.FillPath(brush, graphPath);
                    //g2.FillPath(brush, graphPath);
                    g.DrawPath(blackPen, graphPath);

                    //GraphicsPath graphPath2 = new GraphicsPath();
                    //graphPath2.AddString(fontChar, ff, (int)FontStyle.Regular, 23F, rectangle, sf);
                    //g.FillPath(brush, graphPath2);

                    //int yPos = (pageCharIndex / 18) * 28;
                    //int xPos = (pageCharIndex % 18) * 28;
                    //for (int y = yPos; y < yPos + 28; y++)
                    //{
                    //    for (int x = xPos; x < xPos + 28; x++)
                    //    {
                    //        if (bmp.GetPixel(x, y).A == 0)
                    //        {
                    //            bmp.SetPixel(x, y, bmp2.GetPixel(x, y));
                    //        }
                    //    }
                    //}
                }

                // 更新进度条
                this.ProcessBarStep();

                charIndex++;
                pageCharIndex++;
                if (charIndex % (18 * 18) == 0)
                {
                    bmp.Save(this.baseFolder + @"\BioCvNgcCn\Pic\NewFont\" + this.subDisk + @"\NewFont" + fontPicIndex + ".png");

                    fontPicIndex++;
                    pageCharIndex = 0;
                    if (fontPicIndex >= 4 || charIndex >= fontChars.Length)
                    {
                        break;
                    }
                    else
                    {
                        this.NewFontPic(ref bmp, ref g, ref bmp2, ref g2);
                    }
                }
            }

            // Test
            //Bitmap pageImg = new Bitmap(this.baseFolder + @"\BioCvNgcCn\Pic\NewFont\" + this.subDisk + @"\NewFont0.png");
            //List<int> colorList = new List<int>();
            //for (int y = 0; y < pageImg.Height; y++)
            //{
            //    for (int x = 0; x < pageImg.Width; x++)
            //    {
            //        int color = pageImg.GetPixel(x, y).ToArgb();
            //        if (!colorList.Contains(color))
            //        {
            //            colorList.Add(color);
            //        }
            //    }
            //}

            //MessageBox.Show(colorList.Count.ToString());

            // 关闭进度条
            this.CloseProcessBar();

            //this.ImportCnFont();
        }

        /// <summary>
        /// 取得调色板颜色
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private List<byte> GetPaletteColor(Bitmap img)
        {
            List<byte> byPalette = new List<byte>();
            List<int> paletteColor = new List<int>();
            UInt16 temp;
            Color color;
            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    color = img.GetPixel(x, y);
                    if (!paletteColor.Contains(color.ToArgb()))
                    {
                        paletteColor.Add(color.ToArgb());

                        if (color.A > 0xDA)
                        {
                            // not use alpha 5,5,5
                            temp = (UInt16)(Util.Convert8To5(color.R) << 10 | Util.Convert8To5(color.G) << 5 | Util.Convert8To5(color.B));
                            byPalette.Add((byte)((temp >> 8) | 0x80));
                            byPalette.Add((byte)(temp & 0xFF));
                        }
                        else
                        {
                            // use alpha 3,4,4,4
                            byPalette.Add((byte)(Util.Convert8To3(color.A) << 4 | Util.Convert8To4(color.R) & 0x7F));
                            byPalette.Add((byte)(Util.Convert8To4(color.G) << 4 | Util.Convert8To4(color.B)));
                        }
                    }
                }
            }

            return byPalette;
        }

        /// <summary>
        /// 设置调色板颜色
        /// </summary>
        /// <param name="byPalette"></param>
        /// <returns></returns>
        private List<byte> GetPaletteColor(List<byte> byPalette)
        {
            List<byte> byAllPaletteColor = new List<byte>();
            byAllPaletteColor.AddRange(byPalette);

            for (int i = 0; i < 7; i++)
            {
                byAllPaletteColor.Add(0xFC);
                byAllPaletteColor.Add(0);
            }

            byAllPaletteColor.Add(0xFF);
            byAllPaletteColor.Add(0xE0);

            return byAllPaletteColor;
        }

        /// <summary>
        /// Copy旧字库的文字
        /// </summary>
        /// <param name="newPic"></param>
        /// <param name="oldPic"></param>
        /// <param name="index"></param>
        private void SetOldCharPic(Bitmap newPic, Bitmap oldPic, int index)
        {
            int startY = (index / 18) * 28;
            int startX = (index % 18) * 28;
            for (int y = startY; y < startY + 28; y++)
            {
                for (int x = startX; x < startX + 28; x++)
                {
                    newPic.SetPixel(x + 1, y, oldPic.GetPixel(x, y));
                }
            }
        }

        /// <summary>
        /// 生成字库的图片
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="g"></param>
        private void NewFontPic(ref Bitmap bmp, ref Graphics g, ref Bitmap bmp2, ref Graphics g2)
        {
            bmp = new Bitmap(512, 512);
            g = Graphics.FromImage(bmp);
            g.Clear(Color.Transparent);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingMode = CompositingMode.SourceOver;
            g.CompositingQuality = CompositingQuality.GammaCorrected;

            g.PixelOffsetMode = PixelOffsetMode.Half;
            //g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            bmp2 = new Bitmap(512, 512);
            g2 = Graphics.FromImage(bmp2);
            g2.Clear(Color.Transparent);
            g2.SmoothingMode = SmoothingMode.HighQuality;
            g2.CompositingQuality = CompositingQuality.HighQuality;
            g2.InterpolationMode = InterpolationMode.HighQualityBicubic;
        }

        private string DecodeCnText(string cnFile, string cnText, FilePosInfo currentFileInfo)
        {
            string cnTextFile = string.Empty;
            if (cnFile.EndsWith("sysmes.ald", StringComparison.OrdinalIgnoreCase))
            {
                if (currentFileInfo.TextStart > 0xdc4)
                {
                    cnTextFile = this.baseFolder + @"\DcCnText\SYSMES.ALD1.txt";
                }
                else
                {
                    cnTextFile = this.baseFolder + @"\DcCnText\SYSMES.ALD0.txt";
                }
            }
            else if (cnFile.Contains("mry.afs"))
            {
                cnTextFile = this.baseFolder + @"\DcCnText\Map\unnamed_0.bin" + (this.fileIndex - 1).ToString().PadLeft(2, '0') + ".txt";
            }
            else
            {
                string cnShortName = Util.GetShortNameWithoutType(cnFile);
                cnTextFile = this.baseFolder + @"\DcCnText\" + this.subDisk + @"\" + cnShortName + ".txt";
            }

            if (!File.Exists(cnTextFile))
            {
                return string.Empty;
            }
            string[] cnTexts = File.ReadAllLines(cnTextFile, Encoding.GetEncoding("GB2312"));
            //StringBuilder lineSb = new StringBuilder();
            List<string> listItem = new List<string>();
            //List<List<string>> listCnTexts = new List<List<string>>();
            string curChar = string.Empty;
            string tempLine = string.Empty;
            int curPos = 0;
            foreach (string chLine in cnTexts)
            {
                if (string.IsNullOrEmpty(chLine))
                {
                    continue;
                }

                if (chLine.StartsWith("####", StringComparison.OrdinalIgnoreCase))
                {
                    //listItem = new List<string>();
                    //listCnTexts.Add(listItem);
                    //lineSb.Length = 0;
                    continue;
                }

                tempLine = chLine.Replace(":", "").Replace("end", "ffff");
                //lineSb.Append(chLine.Replace("{", "^").Replace("}", "^").Replace("02FF:0000", "ff 02 0 0").Replace("03FF:0000", "ff 03 0 0").Replace("01FF", "ff 01").Replace("end", "ff ff\r\n"));

                for (int i = 0; i < tempLine.Length; i++)
                {
                    if (tempLine.Substring(i, 1) == "{")
                    {
                        listItem.Add("^");
                        curPos = 0;
                        while ((curChar = tempLine.Substring(++i, 1)) != "}")
                        {
                            listItem.Add(curChar);
                            curPos++;

                            if (curPos % 4 == 0)
                            {
                                curChar = listItem[listItem.Count - 4];
                                listItem[listItem.Count - 4] = listItem[listItem.Count - 2];
                                listItem[listItem.Count - 2] = curChar;
                                curChar = listItem[listItem.Count - 3];
                                listItem[listItem.Count - 3] = listItem[listItem.Count - 1];
                                listItem[listItem.Count - 1] = curChar;
                                if (curPos > 4)
                                {
                                    listItem.Insert(listItem.Count - 4, " ");
                                }
                                listItem.Insert(listItem.Count - 2, " ");
                            }
                        }
                        listItem.Add("^");
                    }
                    else
                    {
                        //StringBuilder sb = new StringBuilder();
                        while (i < tempLine.Length && (curChar = tempLine.Substring(i, 1)) != "{")
                        {
                            listItem.Add(curChar);
                            i++;
                        }
                        i--;

                        //lineSb.Append(sb.ToString()).Append("\n");
                        //listItem.Add(sb.ToString());
                    }
                }
            }

            return string.Join("", listItem.ToArray()).Replace("^ff ff^", "^ff ff^\r\n");

            //string[] oldCnTexts = cnText.Split('\n');
            //int index = 0;
            //StringBuilder keySb = new StringBuilder();
            //StringBuilder newCnTexts = new StringBuilder();
            //for (int j = 0; j < oldCnTexts.Length; j++)
            //{
            //    string oldLine = oldCnTexts[j];
            //    if (string.IsNullOrEmpty(oldLine))
            //    {
            //        continue;
            //    }
            //    keySb.Length = 0;
            //    index = 0;
            //    for (int i = 0; i < oldLine.Length; i++)
            //    {
            //        if (oldLine.Substring(i, 1) == "^")
            //        {
            //            keySb.Append("^");
            //            string nextChar = string.Empty;
            //            while ((nextChar = oldLine.Substring(++i, 1)) != "^")
            //            {
            //                keySb.Append(nextChar);
            //            }
            //            keySb.Append("^");
            //        }
            //        else
            //        {
            //            newCnTexts.Append(keySb.ToString());
            //            keySb.Length = 0;
            //            if (j < listCnTexts.Count)
            //            {
            //                listItem = listCnTexts[j];
            //                if (index < listItem.Count)
            //                {
            //                    newCnTexts.Append(listItem[index++]);
            //                }
            //            }

            //            while (i < oldLine.Length && oldLine.Substring(i, 1) != "^")
            //            {
            //                i++;
            //            }
            //            i--;
            //        }
            //    }

            //    if (keySb.Length > 0)
            //    {
            //        newCnTexts.Append(keySb.ToString());
            //    }

            //    for (int k = index; k < listItem.Count; k++)
            //    {
            //        newCnTexts.Append(listItem[k++]);
            //    }
                
            //    newCnTexts.Append("\n");
            //}

            //return newCnTexts.ToString();
        }

        /// <summary>
        /// 生成新的Afs文件
        /// </summary>
        private void CreateAfs()
        {
            FileStream fs = null;

            try
            {
                string oldAfsFile = this.baseFolder + @"\BioCvNgcJp\" + this.subDisk
                    + @"\root\rdx_lnk" + (this.subDisk.Equals("A", StringComparison.OrdinalIgnoreCase) ? "1" : "2") + ".afs";

                fs = File.OpenRead(oldAfsFile);
                int oldFileLen = (int)fs.Length;
                byte[] byTemp = new byte[0x10];
                fs.Read(byTemp, 0, byTemp.Length);

                int subFileCount = (byTemp[7] << 24) | (byTemp[6] << 16) | (byTemp[5] << 8) | byTemp[4];
                int entryPos = 8;
                int firstPos = (byTemp[entryPos + 3] << 24) | (byTemp[entryPos + 2] << 16) | (byTemp[entryPos + 1] << 8) | byTemp[entryPos];
                int nameEntryPos = entryPos + subFileCount * 8;

                fs.Seek(nameEntryPos, SeekOrigin.Begin);
                fs.Read(byTemp, 0, byTemp.Length);

                nameEntryPos = (byTemp[3] << 24) | (byTemp[2] << 16) | (byTemp[1] << 8) | byTemp[0];
                int nameEntrySize = (byTemp[7] << 24) | (byTemp[6] << 16) | (byTemp[5] << 8) | byTemp[4];
                int oldNameEntrySize = nameEntrySize;
                if (nameEntrySize % 2048 > 0)
                {
                    nameEntrySize += (2048 - (nameEntrySize % 2048));
                }

                fs.Seek(0, SeekOrigin.Begin);
                fs.Read(byTemp, 0, byTemp.Length);

                byte[] byNameEntry = new byte[nameEntrySize];
                fs.Seek(nameEntryPos, SeekOrigin.Begin);
                fs.Read(byNameEntry, 0, byNameEntry.Length);
                fs.Close();


                string cnAfsFilesPath = this.baseFolder + @"\BioCvNgcCn\" + this.subDisk + @"\root\rdx_lnk" + this.subDisk;
                List<FilePosInfo> fileInfos = Util.GetAllFiles(cnAfsFilesPath).Where(p => !p.IsFolder && p.File.EndsWith(".rdx@", StringComparison.OrdinalIgnoreCase)).ToList();

                if (subFileCount != fileInfos.Count)
                {
                    MessageBox.Show("文件个数不匹配，相差了：" + (subFileCount - fileInfos.Count));
                    return;
                }

                string cnAfsFile = oldAfsFile.Replace("BioCvNgcJp", "BioCvNgcCn");
                if (File.Exists(cnAfsFile))
                {
                    File.Delete(cnAfsFile);
                }

                // 显示进度条
                this.ResetProcessBar(subFileCount);

                fs = File.OpenWrite(cnAfsFile);
                fs.SetLength(firstPos);
                fs.Seek(0, SeekOrigin.Begin);
                fs.Write(byTemp, 0, byTemp.Length);
                fs.Seek(entryPos, SeekOrigin.Begin);

                int nextFilePos = firstPos;
                for (int i = 0; i < subFileCount; i++)
                {
                    // 写入Entry位置、大小信息
                    fs.Seek(entryPos + i * 8, SeekOrigin.Begin);
                    FilePosInfo newItem = fileInfos[i];
                    byte[] cnFile = File.ReadAllBytes(newItem.File);

                    byTemp[0] = (byte)(nextFilePos & 0xFF);
                    byTemp[1] = (byte)((nextFilePos >> 8) & 0xFF);
                    byTemp[2] = (byte)((nextFilePos >> 16) & 0xFF);
                    byTemp[3] = (byte)((nextFilePos >> 24) & 0xFF);

                    byTemp[4] = (byte)(cnFile.Length & 0xFF);
                    byTemp[5] = (byte)((cnFile.Length >> 8) & 0xFF);
                    byTemp[6] = (byte)((cnFile.Length >> 16) & 0xFF);
                    byTemp[7] = (byte)((cnFile.Length >> 24) & 0xFF);

                    fs.Write(byTemp, 0, 8);

                    // 写入Enrty信息
                    fs.Seek(nextFilePos, SeekOrigin.Begin);
                    fs.Write(cnFile, 0, cnFile.Length);

                    // 计算下一个文件位置
                    nextFilePos += cnFile.Length;
                    
                    // 开始位置特殊处理，必须是2048的倍数
                    int chkDiffLen = nextFilePos % 2048;
                    if (chkDiffLen > 0)
                    {
                        nextFilePos += (2048 - chkDiffLen);
                        fs.SetLength(nextFilePos);
                    }

                    // 更新进度条
                    this.ProcessBarStep();
                }

                // 写入NameEntry信息
                fs.Seek(entryPos + subFileCount * 8, SeekOrigin.Begin);

                byTemp[0] = (byte)(nextFilePos & 0xFF);
                byTemp[1] = (byte)((nextFilePos >> 8) & 0xFF);
                byTemp[2] = (byte)((nextFilePos >> 16) & 0xFF);
                byTemp[3] = (byte)((nextFilePos >> 24) & 0xFF);

                byTemp[4] = (byte)(oldNameEntrySize & 0xFF);
                byTemp[5] = (byte)((oldNameEntrySize >> 8) & 0xFF);
                byTemp[6] = (byte)((oldNameEntrySize >> 16) & 0xFF);
                byTemp[7] = (byte)((oldNameEntrySize >> 24) & 0xFF);

                fs.Write(byTemp, 0, 8);

                fs.Seek(nextFilePos, SeekOrigin.Begin);
                fs.Write(byNameEntry, 0, byNameEntry.Length);
                fs.Close();

                // 隐藏进度条
                this.CloseProcessBar();

                MessageBox.Show("OK");
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message + "\n" + exp.StackTrace);
            }
            finally
            { 
                if (fs != null)
                {
                    fs.Close();
                }

                this.CloseProcessBar();
            }
        }

        /// <summary>
        /// 设置当前Dc区域状态
        /// </summary>
        /// <param name="isDc"></param>
        private void SetDcLoadStatus(bool isDc)
        {
            this.chkDcSysmes.Checked = isDc;
            this.chkDcSysmes.Enabled = isDc;
        }

        /// <summary>
        /// 设置当前Ngc区域状态
        /// </summary>
        /// <param name="isNgc"></param>
        private void SetNgcLoadStatus(bool isNgc)
        {
            this.chkNgcRdx.Checked = false;
            this.chkNgcRdx.Enabled = false;

            this.chkNgcSysmes.Checked = isNgc;
            this.chkNgcSysmes.Enabled = isNgc;
        }

        /// <summary>
        /// 保存文本数据
        /// </summary>
        /// <param name="ngcFile"></param>
        /// <param name="byJpData"></param>
        private int getNgcTextStartPos(string ngcFile, byte[] byPsJpData)
        {
            if (File.Exists(ngcFile))
            {
                try
                {
                    // 根据Ps日文文本数据，查找Ngc中的文本数据
                    return this.GetTextStartPos(File.ReadAllBytes(ngcFile), byPsJpData);
                }
                catch
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
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
        /// 根据配置文件，读入需要汉化的文件
        /// </summary>
        /// <returns></returns>
        private List<FilePosInfo> LoadFiles()
        {
            List<FilePosInfo> needCopyFiles = new List<FilePosInfo>();

            if (this.rdoNgc.Checked)
            {
                if (this.chkNgcSysmes.Checked)
                {
                    needCopyFiles.AddRange(this.LoadFiles(this.baseFolder + @"\NgcSysmesAddr.txt"));
                }

                if (this.chkNgcRdx.Checked)
                {
                    //this.AddNgcRdxFiles(needCopyFiles);
                    needCopyFiles.AddRange(this.LoadFiles(this.baseFolder + @"\NgcRdxAddr" + this.subDisk.ToUpper() + ".txt"));
                }
            }
            else
            {
                if (this.chkDcSysmes.Checked)
                {
                    needCopyFiles.AddRange(this.LoadFiles(this.baseFolder + @"\DcSysmesAddr.txt"));
                }
            }

            return needCopyFiles;
        }

        /// <summary>
        /// 添加Ngc Rdx文件
        /// </summary>
        private void AddNgcRdxFiles(List<FilePosInfo> needCopyFiles)
        {
            string path = this.baseFolder + @"\BioCvNgcJp\" + this.subDisk + @"\root\rdx_lnk" + this.subDisk;
            List<FilePosInfo> fileInfos = Util.GetAllFiles(path).Where(p => !p.IsFolder && p.File.EndsWith(".bioCvDec", StringComparison.OrdinalIgnoreCase)).ToList();
            StringBuilder sb = new StringBuilder();
            foreach (FilePosInfo fileInfo in fileInfos)
            {
                byte[] byFile = File.ReadAllBytes(fileInfo.File);
                fileInfo.TextStart = Util.GetOffset(byFile, 0xb8, 0xbb);
                fileInfo.TextEnd = Util.GetOffset(byFile, 0xbc, 0xbf);
                fileInfo.EntryPos = fileInfo.TextStart;

                if (fileInfo.TextStart == 0)
                {
                    continue;
                }

                fileInfo.TextStart = fileInfo.TextStart + Util.GetOffset(byFile, fileInfo.EntryPos + 4, fileInfo.EntryPos + 7);

                sb.Append(Util.GetShortName(fileInfo.File)).Append("\r\n");
                sb.Append(fileInfo.TextStart.ToString("x")).Append(" ");
                sb.Append(fileInfo.TextEnd.ToString("x")).Append(" ");
                sb.Append(fileInfo.EntryPos.ToString("x")).Append("\r\n");
                needCopyFiles.Add(fileInfo);
            }

            File.WriteAllText(this.baseFolder + @"\NgcRdxAddr" + this.subDisk.ToUpper() + ".txt", sb.ToString(), Encoding.UTF8);
        }

        /// <summary>
        /// 开始解码
        /// </summary>
        /// <param name="byData"></param>
        /// <param name="startPos"></param>
        private string DecodeText(byte[] byData, FilePosInfo filePosInfo, Dictionary<int, string[]> fontCharPage)
        {
            if (filePosInfo.EntryPos > 0)
            {
                List<int> entryList = new List<int>();
                int textCount = Util.GetOffset(byData, filePosInfo.EntryPos, filePosInfo.EntryPos + 3);
                for (int j = 0; j < textCount; j++)
                {
                    entryList.Add(filePosInfo.EntryPos + Util.GetOffset(byData, filePosInfo.EntryPos + (j + 1) * 4, filePosInfo.EntryPos + (j + 1) * 4 + 3));
                }

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < entryList.Count - 1; i++)
                {
                    sb.Append(this.DecodeText(byData, fontCharPage, entryList[i], entryList[i + 1]));
                }

                // 取得最后一句
                int textEndPos = entryList[entryList.Count - 1];
                int endPos = filePosInfo.TextEnd;
                if (endPos == 0)
                {
                    endPos = byData.Length;
                }
                while (textEndPos < endPos)
                {
                    if (byData[textEndPos] == 0xFF && byData[textEndPos + 1] == 0xFF)
                    {
                        textEndPos += 2;
                        break;
                    }

                    textEndPos++;
                }

                if (textEndPos > entryList[entryList.Count - 1]) 
                {
                    if (filePosInfo.TextEnd == 0)
                    {
                        filePosInfo.TextEnd = textEndPos;
                    }

                    sb.Append(this.DecodeText(byData, fontCharPage, entryList[entryList.Count - 1], textEndPos));
                }

                return sb.ToString();
            }
            else 
            {
                return this.DecodeText(byData, fontCharPage, filePosInfo.TextStart, filePosInfo.TextEnd);
            }
        }

        /// <summary>
        /// 开始解码
        /// </summary>
        /// <param name="byData"></param>
        /// <param name="startPos"></param>
        private string DecodeText(byte[] byData, Dictionary<int, string[]> fontCharPage, int startPos, int endPos)
        {
            StringBuilder sb = new StringBuilder();
            for (int j = startPos; j < endPos; j += 2)
            {
                if (byData[j] == 0xFF && (byData[j + 1] == 2 || byData[j + 1] == 3))
                {
                    sb.Append("^ff " + byData[j + 1].ToString("x") + " " + byData[j + 2].ToString("x") + " " + byData[j + 3].ToString("x") + "^");
                    j += 2;
                }
                else if (fontCharPage.ContainsKey(byData[j]))
                {
                    sb.Append(fontCharPage[byData[j]][byData[j + 1]]);
                }
                else
                {
                    sb.Append("^" + byData[j].ToString("x") + " " + byData[j + 1].ToString("x") + "^");
                }
            }

            sb.Append("\n");

            return sb.ToString();
        }

        /// <summary>
        /// 读取字库信息
        /// </summary>
        private void ReadFontFile(string fontInfoFile, Dictionary<int, string[]> fontCharPage, Encoding encoding)
        {
            fontCharPage.Clear();

            try
            {
                // 读取字符信息
                string[] charMaps = File.ReadAllLines(fontInfoFile, encoding);
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
        /// 保存文本数据
        /// </summary>
        /// <param name="ngcFile"></param>
        /// <param name="byJpData"></param>
        /// <param name="byCnData"></param>
        /// <param name="saveFaileFiles"></param>
        private void SaveTextData(string ngcCnFile, byte[] byJpData, byte[] byCnData, StringBuilder saveFaileFiles, string sortName)
        {
            this.baseFile = ngcCnFile;
            if (!this.SaveTextData(ngcCnFile, byJpData, byCnData))
            {
                saveFaileFiles.Append(ngcCnFile + " : " + sortName).Append("\n");
            }
        }

        /// <summary>
        /// 保存文本数据
        /// </summary>
        /// <param name="ngcCnFile"></param>
        /// <param name="byJpData"></param>
        /// <param name="byCnData"></param>
        /// <returns></returns>
        private bool SaveTextData(string ngcCnFile, byte[] byJpData, byte[] byCnData)
        {
            try
            {
                // 取得Ngc数据
                byte[] byNgcData = File.ReadAllBytes(ngcCnFile);

                // 根据Dc日文文本数据，查找日文Ngc中的文本数据
                int txtStartPos = this.GetTextStartPos(File.ReadAllBytes(ngcCnFile.Replace("Cn", "Jp")), byJpData);
                if (txtStartPos > -1)
                {
                    // 将中文数据写入Ngc数据
                    Array.Copy(byCnData, 0, byNgcData, txtStartPos, byCnData.Length);

                    // 保存中文数据
                    File.WriteAllBytes(ngcCnFile, byNgcData);
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

            return true;
        }

        #endregion
    }
}
