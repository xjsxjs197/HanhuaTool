using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Hanhua.Common;

namespace Hanhua.TextEditTools.TxtresEdit
{
    /// <summary>
    /// Txtres编辑器
    /// </summary>
    public partial class TresEditer : BaseForm
    {
        #region " 全局变量 "

        /// <summary>
        /// 翻译的文件
        /// </summary>
        private string tresFile;

        /// <summary>
        /// 原日文原始文件
        /// </summary>
        private string oldTresFile;

        /// <summary>
        /// 原英文原始文件
        /// </summary>
        private string oldTresFileEn;

        /// <summary>
        /// 文件的总长度
        /// </summary>
        private int fileLen;

        /// <summary>
        /// 字符串Section的开始位置
        /// </summary>
        private int stringSectionStartPos;

        /// <summary>
        /// 当前选中的解码器
        /// </summary>
        private Encoding encoding;

        /// <summary>
        /// 文件类型
        /// </summary>
        private int tresType;

        /// <summary>
        /// 类型二的字符串Entries开始位置
        /// 对于类型二，就是0
        /// </summary>
        private int strEntries;

        /// <summary>
        /// 日中字符对照表
        /// </summary>
        private Dictionary<string, string> charJpCnList = new Dictionary<string,string>();

        /// <summary>
        /// 中日字符对照表
        /// </summary>
        private Dictionary<string, string> charCnJpList = new Dictionary<string, string>();

        /// <summary>
        /// 日中对照表文件
        /// </summary>
        private string charJpCnFile;

        /// <summary>
        /// 一级汉字
        /// </summary>
        private string oneLevelCnChar = Util.CreateOneLevelHanzi();

        /// <summary>
        /// 保存上次修改的位置
        /// </summary>
        private int modifyRow;

        /// <summary>
        /// 保存修改前的值
        /// </summary>
        private int oldPadding2;

        /// <summary>
        /// 保存查询的行数
        /// </summary>
        private int searchIndex = -1;

        /// <summary>
        /// 保存上次查询的关键字
        /// </summary>
        private string oldSearchKey = string.Empty;

        /// <summary>
        /// 保存英文的文本
        /// </summary>
        private Dictionary<int, string> enTextList = new Dictionary<int,string>();

        /// <summary>
        /// 记录完成度的文件
        /// </summary>
        private string completeFile = "完成度.txt";
        private List<bool> completeList = new List<bool>();
        private int completeNum = 0;

        #endregion

        #region " 构造方法 "

        /// <summary>
        /// 初始化
        /// </summary>
        public TresEditer(string tresFile)
        {
            InitializeComponent();

            this.ResetHeight();

            this.ddlDecoder.SelectedIndex = 0;
            this.tresFile = tresFile;
            this.btnCopy.Enabled = false;
            this.btnSave.Enabled = false;
            this.enTextList.Clear();

            // 读取日中字符对照表
            this.LoadJpCnCharList();
        }

        #endregion

        #region " 页面事件 "

        /// <summary>
        /// 保存日中字符映射表
        /// </summary>
        private void SaveCharJpCnTable()
        {
            List<string> charJpCnList = new List<string>();
            foreach (KeyValuePair<string, string> charJpCn in this.charJpCnList)
            {
                charJpCnList.Add(charJpCn.Key + charJpCn.Value);
            }

            // 保存文件
            File.WriteAllLines(this.charJpCnFile, charJpCnList.ToArray(), Encoding.UTF8);

            // Copy一份到执行文件目录，以便于生成字库
            string[] names = this.charJpCnFile.Split('\\');
            File.Copy(this.charJpCnFile, names[names.Length - 1], true);
        }

        /// <summary>
        /// Utf8以外的编码格式的文件，要先另存成Utf8格式的
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveAsUtf8_Click(object sender, EventArgs e)
        {
            // 从新计算保存需要的字节数
            this.encoding = Encoding.UTF8;
            if (this.tresType == 2)
            {
                this.fileLen = 0x10;
                for (int i = 0; i < this.tresGrid.Rows.Count - 1; i++)
                {
                    // offset表
                    this.fileLen += 0x10;

                    // 字符串
                    this.fileLen += this.encoding.GetByteCount(this.tresGrid.Rows[i].Cells[1].Value as string) + 1;
                }
            }

            // 以utf8形式保存【_汉化】文件
            this.ddlDecoder.SelectedIndex = 1;
            this.btnSave_Click(sender, e);

            // 以utf8形式保存【_日文】文件
            File.Copy(this.tresFile, this.oldTresFile, true);
        }

        /// <summary>
        /// 点击回车后进入下一个可编辑的单元格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tresGrid_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    int row = this.tresGrid.SelectedCells[0].RowIndex;
                    int col = this.tresGrid.SelectedCells[0].ColumnIndex;
                    if (row < this.tresGrid.Rows.Count - 1)
                    {
                        this.tresGrid.BeginEdit(true);
                        DataGridViewCell cell = this.tresGrid.Rows[row].Cells[7];
                        Point p = this.tresGrid.PointToClient(Control.MousePosition);
                        //this.textBef.Show(cell.Value as string, this, p.X, p.Y + this.tresGrid.Rows[row].Height * 2);

                        this.txtJp.Text = cell.Value as string;
                        this.txtEn.Text = this.enTextList[Convert.ToInt32(this.tresGrid.Rows[row].Cells[9].Value)];
                    }
                    e.Handled = true;
                    break;

                case Keys.Down:
                    break;

                case Keys.Up:
                    break;
            }
        }

        /// <summary>
        /// 保存相关信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tresGrid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // 保存要修改的行号
            this.modifyRow = e.RowIndex;

            // 保存Padding2
            if (e.ColumnIndex == 4
                && this.tresType == 1)
            {
                string padding2 = this.tresGrid.Rows[e.RowIndex].Cells[4].Value as string;
                if (!string.IsNullOrEmpty(padding2))
                {
                    this.oldPadding2 = Convert.ToInt32(this.tresGrid.Rows[e.RowIndex].Cells[4].Value);
                }
            }
        }

        /// <summary>
        /// 当前Cell翻译后，对比翻译前后字节长度
        /// 如果不一样，改变颜色，并且保存变更标志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tresGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            this.textBef.Hide(this);

            // 修改Cell后处理
            this.CellEndEdit(e.RowIndex, e.ColumnIndex);
        }

        /// <summary>
        /// 复制
        /// 重新设置中日字符对照表（临时，只执行一次）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopy_Click(object sender, EventArgs e)
        {
            //// 打开要分析的文件
            //this.openFile.Filter = "TXTRES文件（*.txtres, *.dat）|*.txtres;*.dat|所有文件|*.*";
            //DialogResult rs = this.openFile.ShowDialog(this);
            //if (string.IsNullOrEmpty(this.copyTresFile) || rs == DialogResult.Cancel)
            //{
            //    return;
            //}

            //this.CopyTres();

            // 取得所有使用中的字符
            IList<string> cnList = new List<string>();
            for (int i = 0; i < this.tresGrid.Rows.Count - 1; i++)
            {
                string line = this.tresGrid.Rows[i].Cells[1].Value as string;
                if (!string.IsNullOrEmpty(line))
                {
                    char[] lineChar = line.ToCharArray();
                    foreach (char item in lineChar)
                    {
                        if (!cnList.Contains(item.ToString()))
                        {
                            cnList.Add(item.ToString());
                        }
                    }
                }
            }

            // 去掉没有使用的中文字符
            Dictionary<string, string> newJpCnList = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> charJpCn in this.charJpCnList)
            {
                if (!string.IsNullOrEmpty(charJpCn.Value)
                    && !cnList.Contains(charJpCn.Value))
                {
                    newJpCnList.Add(charJpCn.Key, string.Empty);
                }
                else
                {
                    newJpCnList.Add(charJpCn.Key, charJpCn.Value);
                }
            }

            // 重新保存字符对照表
            this.charJpCnList = newJpCnList;
            this.SaveCharJpCnTable();
        }

        /// <summary>
        /// 查询字符串
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string key = this.txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(key))
            {
                MessageBox.Show("请输入查询的字符串！");
                this.txtSearch.Focus();
                return;
            }
            else if (!this.oldSearchKey.Equals(key))
            {
                this.oldSearchKey = key;
                this.searchIndex = -1;
            }

            for (int i = this.searchIndex + 1; i < this.tresGrid.Rows.Count - 1; i++)
            {
                // 取得当前行文本
                string textCn = this.tresGrid.Rows[i].Cells[1].Value as string;
                if (!string.IsNullOrEmpty(textCn)
                    && textCn.IndexOf(key) != -1)
                {
                    this.searchIndex = i;
                    // 重新设置滚动条位置
                    this.tresGrid.Rows[i].Visible = true;
                    this.tresGrid.FirstDisplayedScrollingRowIndex = i;
                    this.tresGrid.Focus();
                    this.tresGrid.Rows[i].Cells[1].Selected = true;
                    return;
                }
            }
        }

        /// <summary>
        /// 隐藏或显示所有完成的行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHide_Click(object sender, EventArgs e)
        {
            // 设置等待条相关信息
            this.ResetProcessBar(this.tresGrid.Rows.Count - 1);

            if ("显示所有".Equals(this.btnHide.Text))
            {
                this.tresGrid.SuspendLayout();
                for (int i = 0; i < this.tresGrid.Rows.Count - 1; i++)
                {
                    if (this.tresGrid.Rows[i].Visible == false)
                    {
                        this.tresGrid.Rows[i].Visible = true;
                    }

                    this.ProcessBarStep();
                }
                this.tresGrid.ResumeLayout(true);

                this.btnHide.Text = "隐藏完成";
            }
            else
            {
                this.tresGrid.SuspendLayout();
                for (int i = 0; i < this.tresGrid.Rows.Count - 1; i++)
                {
                    // 隐藏当前行
                    DataGridViewCheckBoxCell cell = this.tresGrid.Rows[i].Cells[10] as DataGridViewCheckBoxCell;
                    if (cell != null && (bool)cell.Value)
                    {
                        this.tresGrid.Rows[i].Visible = false;
                    }

                    this.ProcessBarStep();
                }
                this.tresGrid.ResumeLayout(true);

                this.btnHide.Text = "显示所有";
            }

            this.CloseProcessBar();
        }

        /// <summary>
        /// 隐藏选择的行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tresGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex == 10)
            {
                // 隐藏当前行
                DataGridViewCheckBoxCell cell = this.tresGrid.Rows[e.RowIndex].Cells[10] as DataGridViewCheckBoxCell;
                if ((bool)cell.EditingCellFormattedValue)
                {
                    this.tresGrid.Rows[e.RowIndex].Visible = false;
                    this.btnHide.Text = "显示所有";
                    completeNum++;
                }
                else
                {
                    completeNum--;
                }

                this.SetTitleInfo();
            }
        }

        /// <summary>
        /// 变换解码器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChgDecoder_Click(object sender, EventArgs e)
        {
            // 取得当前选中解码器
            switch (this.ddlDecoder.SelectedIndex)
            {
                case 0:
                    this.encoding = Encoding.GetEncoding("Shift-Jis");
                    break;

                case 1:
                    this.encoding = Encoding.UTF8;
                    break;

                case 3:
                    this.encoding = Encoding.GetEncoding(20932);
                    break;

                case 4:
                    this.encoding = Encoding.GetEncoding(50220);
                    break;

                case 5:
                    this.encoding = Encoding.GetEncoding(51932);
                    break;

                default:
                    this.encoding = Encoding.GetEncoding("Shift-Jis");
                    break;
            }

            // 开始查看Tres文件
            this.ViewTres();
            this.btnSave.Enabled = true;
            this.btnCopy.Enabled = true;
        }

        /// <summary>
        /// 直接进入编辑状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tresGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (e.ColumnIndex == 1)
                {
                    // 进入编辑状态，弹出日文原文
                    this.tresGrid.BeginEdit(true);
                    DataGridViewCell cell = this.tresGrid.Rows[e.RowIndex].Cells[7];
                    this.txtJp.Text = cell.Value as string;
                    this.txtEn.Text = this.enTextList[Convert.ToInt32(this.tresGrid.Rows[e.RowIndex].Cells[9].Value)];
                    //Point p = this.tresGrid.PointToClient(Control.MousePosition);
                    //this.textBef.Show(cell.Value as string, this, p.X, p.Y + this.tresGrid.Rows[e.RowIndex].Height * 2);
                }
            }
        }

        /// <summary>
        /// 保存翻译
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // 定义新文件的字节数据
            byte[] byEdited = new byte[this.fileLen];

            FileStream fs = null;
            int i = 0;
            try
            {
                // 将文件中的数据，读取到byData中
                fs = new FileStream(this.tresFile, FileMode.Open);
                byte[] byData = new byte[this.stringSectionStartPos];
                fs.Read(byData, 0, byData.Length);
                fs.Close();
                fs = null;

                // 保存没有变化的数据
                // 保存字符串Section的开始位置以前的所有数据
                int saveDataPos = 0;
                Array.Copy(byData, 0, byEdited, 0, this.stringSectionStartPos);
                saveDataPos += this.stringSectionStartPos;

                // 保存变化的数据
                for (i = 0; i < this.tresGrid.Rows.Count - 1; )
                {
                    int strNum = Convert.ToInt32(this.tresGrid.Rows[i].Cells[5].Value);
                    int padding1 = Convert.ToInt32(this.tresGrid.Rows[i].Cells[3].Value);
                    int padding2 = Convert.ToInt32(this.tresGrid.Rows[i].Cells[4].Value);
                    int intOffset = Convert.ToInt32(this.tresGrid.Rows[i].Cells[0].Value as string, 16);
                    int nextStrOffset = Convert.ToInt32(this.tresGrid.Rows[i].Cells[6].Value as string);

                    // 保存字符串Section信息
                    byte[] stringSection = this.GetStringSection(strNum, padding1, padding2, intOffset, nextStrOffset);
                    Array.Copy(stringSection, 0, byEdited, saveDataPos, stringSection.Length);
                    saveDataPos += stringSection.Length;

                    // 保存Padding1字符串信息
                    if (!string.IsNullOrEmpty(this.tresGrid.Rows[i].Cells[0].Value as string)
                        && padding1 != 0 && this.tresType == 1)
                    {
                        byte[] byText = this.encoding.GetBytes(this.tresGrid.Rows[i].Cells[8].Value as string);
                        Array.Copy(byText, 0, byEdited, this.strEntries + padding1, byText.Length);
                        byEdited[this.strEntries + padding1 + byText.Length] = 0;
                    }

                    if (padding2 == 0)
                    {
                        i++;
                        continue;
                    }

                    // 保存字符串信息
                    while (padding2 > 0)
                    {
                        byte[] byText = this.encoding.GetBytes(this.tresGrid.Rows[i].Cells[2].Value as string);
                        Array.Copy(byText, 0, byEdited, this.strEntries + intOffset, byText.Length);
                        byEdited[this.strEntries + intOffset + byText.Length] = 0;

                        intOffset += byText.Length + 1;
                        padding2--;
                        i++;
                    }
                }

                // 保存文件
                if (this.tresType == 2)
                {
                    Array.Copy(byData, 0, byEdited, 0, this.stringSectionStartPos);
                }
                File.WriteAllBytes(this.tresFile, byEdited);

                // 保存字符映射表
                this.SaveCharJpCnTable();

                // 保存完成度
                this.SaveCompleteList();

                // 整理一下可用文字
                //this.btnCopy_Click(this.btnCopy, new EventArgs());

                // 重新打开Tres文件
                this.ViewTres();

                // 重新设置滚动条位置
                this.tresGrid.FirstDisplayedScrollingRowIndex = this.tresGrid.Rows[this.modifyRow].Index;
                this.tresGrid.Focus();
            }
            catch (Exception me)
            {
                MessageBox.Show(i + " \n" + me.Message + "\n" + me.StackTrace);
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExp_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("开始位置\t文本\tPadding1\tPadding2\tNextOffset\n");

            for (int i = 0; i < this.tresGrid.Rows.Count - 1; i++)
            {
                DataGridViewRow row = this.tresGrid.Rows[i];
                sb.Append(row.Cells[0].Value as string + "\t");
                sb.Append(row.Cells[1].Value as string + "\t");
                sb.Append(row.Cells[3].Value as string + "\t");
                sb.Append(row.Cells[4].Value as string + "\t");
                sb.Append(row.Cells[6].Value as string);

                sb.Append("\n");
            }

            // 把Copy字符串写入剪貼板
            Clipboard.SetDataObject(sb.ToString(), true);
        }

        #endregion

        #region " 私有方法 "

        /// <summary>
        /// 读取日中字符对照表
        /// </summary>
        private void LoadJpCnCharList()
        {
            // 取得日中字符对照表文件名
            string[] names = this.tresFile.Split('\\');
            StringBuilder nameSb = new StringBuilder();
            for (int i = 0; i < names.Length - 1; i++)
            {
                nameSb.Append(names[i]).Append("\\");
            }
            nameSb.Append(Util.jpCnCharMapFileName);
            this.charJpCnFile = nameSb.ToString();

            // 取得对照表中的日中汉字
            if (File.Exists(this.charJpCnFile))
            {
                string[] jpChCharTable = File.ReadAllLines(this.charJpCnFile);

                for (int i = 0; i < jpChCharTable.Length; i++)
                {
                    string currentChar = jpChCharTable[i];
                    string jpChar = currentChar.Substring(0, 1);
                    string cnChar = string.Empty;
                    if (currentChar.Length > 1)
                    {
                        cnChar = currentChar.Substring(1);
                    }

                    this.charJpCnList.Add(jpChar, cnChar);
                    if (!string.Empty.Equals(cnChar))
                    {
                        this.charCnJpList.Add(cnChar, jpChar);
                    }

                    // 在所有的ShiftJis字符中，去掉已有的字符
                    shiftJisCharList = shiftJisCharList.Replace(jpChar, "");
                }

                this.tresGrid.ReadOnly = false;
            }
            else
            {
                this.tresGrid.ReadOnly = true;
                this.btnSave.Enabled = false;
                this.btnCopy.Enabled = false;
                this.SetTitleInfo();
                MessageBox.Show("没有找到日中字符对照表，不能修改或保存翻译。");
            }
        }

        /// <summary>
        /// 取得完成度列的值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool GetCheckValue(int index)
        {
            if (completeList.Count == 0)
            {
                return false;
            }

            bool value = completeList[index];
            if (value)
            {
                this.tresGrid.Rows[index].Visible = false;
            }

            return value;
        }

        /// <summary>
        /// 设置title提示信息
        /// 中文可用和已经使用个数
        /// </summary>
        private void SetTitleInfo()
        {
            if (this.charJpCnList.Count == 0
                || this.charCnJpList.Count == 0)
            {
                this.Text = "未取得日中字符映射表！";
            }
            else
            {
                this.Text = "总字符个数：" + this.charJpCnList.Count
                    + "，已使用中文字符：" + this.charCnJpList.Count + this.GetCompleteContent();
            }
        }

        /// <summary>
        /// 计算完成度字符串
        /// </summary>
        /// <returns></returns>
        private string GetCompleteContent()
        {
            if (this.completeNum == 0)
            {
                return string.Empty;
            }

            return " 完成度：" + ((decimal)(this.completeNum) / (decimal)(this.tresGrid.Rows.Count - 1) * 100).ToString("###,##0.#0") + " %";
        }

        /// <summary>
        /// 按照日中对照表
        /// 将日文字符替换成中文字符
        /// </summary>
        /// <param name="jpCharList"></param>
        /// <returns></returns>
        private string GetCnCharFromJp(string jpCharList)
        {
            if (string.IsNullOrEmpty(jpCharList)
                || this.charJpCnList.Count == 0)
            {
                return jpCharList;
            }

            StringBuilder cnCharSb = new StringBuilder();
            for (int i = 0; i < jpCharList.Length; i++)
            {
                string jpChar = jpCharList.Substring(i, 1);
                if (this.charJpCnList.ContainsKey(jpChar))
                {
                    string cnChar = this.charJpCnList[jpChar];
                    if (string.IsNullOrEmpty(cnChar))
                    {
                        cnCharSb.Append(jpChar);
                    }
                    else
                    {
                        cnCharSb.Append(cnChar);
                    }
                }
                else
                {
                    cnCharSb.Append(jpChar);
                }
            }

            return cnCharSb.ToString();
        }

        /// <summary>
        /// 按照日中对照表
        /// 将中文字符替换成日文字符
        /// </summary>
        /// <param name="jpCharList"></param>
        /// <returns></returns>
        private string GetJpCharFromCn(string cnCharList)
        {
            if (string.IsNullOrEmpty(cnCharList)
                || this.charCnJpList.Count == 0)
            {
                return cnCharList;
            }

            StringBuilder jpCharSb = new StringBuilder();
            for (int i = 0; i < cnCharList.Length; i++)
            {
                string cnChar = cnCharList.Substring(i, 1);
                if (!this.charCnJpList.ContainsKey(cnChar))
                {
                    if (this.oneLevelCnChar.Contains(cnChar))
                    {
                        // 是中文字符，则追加
                        jpCharSb.Append(this.AddCharJpCnList(cnChar));

                        // 重新设置Title提示信息
                        this.SetTitleInfo();
                    }
                    else
                    {
                        // 是日文字符，不变
                        jpCharSb.Append(cnChar);
                    }
                }
                else
                {
                    jpCharSb.Append(this.charCnJpList[cnChar]);
                }
            }

            return jpCharSb.ToString();
        }

        /// <summary>
        /// 追加日中参照表字符
        /// </summary>
        /// <param name="charCn"></param>
        /// <returns></returns>
        private string AddCharJpCnList(string charCn)
        {
            foreach (KeyValuePair<string, string> charJpCn in this.charJpCnList)
            {
                if (string.IsNullOrEmpty(charJpCn.Value))
                {
                    this.charJpCnList[charJpCn.Key] = charCn;
                    this.charCnJpList.Add(charCn, charJpCn.Key);
                    return charJpCn.Key;
                }
            }

            // 扩展字符
            //if (!string.IsNullOrEmpty(shiftJisCharList))
            //{
            //    string addedJpChar = shiftJisCharList.Substring(0, 1);
            //    shiftJisCharList = shiftJisCharList.Substring(1);
            //    this.charJpCnList.Add(addedJpChar, charCn);
            //    this.charCnJpList.Add(charCn, addedJpChar);

            //    return addedJpChar;
            //}

            MessageBox.Show("中文字符已经到使用上限！");
            return string.Empty;
        }

        /// <summary>
        /// 开始查看Tres文件
        /// </summary>
        /// <param name="tresFile"></param>
        private void ViewTres()
        {
            FileStream fs = null;
            this.enTextList.Clear();
            this.completeNum = 0;
            try
            {
                // 查找原始文件
                string newFile = string.Empty;
                int keyWordIndex = this.tresFile.IndexOf("_汉化");
                if (keyWordIndex != -1)
                {
                    this.oldTresFile = this.tresFile.Substring(0, keyWordIndex) + "_日文" + this.tresFile.Substring(keyWordIndex + 3);
                    newFile = this.tresFile;
                }
                else if (this.tresFile.IndexOf("_日文") != -1)
                {
                    keyWordIndex = this.tresFile.IndexOf("_日文");
                    this.oldTresFile = this.tresFile;
                    newFile = this.tresFile.Substring(0, keyWordIndex) + "_汉化" + this.tresFile.Substring(keyWordIndex + 3);
                }
                else
                {
                    string[] names = this.tresFile.Split('.');
                    this.oldTresFile = names[0] + "_日文." + names[names.Length - 1];
                    newFile = names[0] + "_汉化." + names[names.Length - 1];
                }

                // 如果原始日文文件和翻译文件不存在，则新建一个
                if (!File.Exists(this.oldTresFile))
                {
                    File.Copy(this.tresFile, this.oldTresFile);
                }
                if (!File.Exists(newFile))
                {
                    File.Copy(this.tresFile, newFile);
                }
                this.tresFile = newFile;

                // 将文件中的数据，读取到byData中
                fs = new FileStream(this.tresFile, FileMode.Open);
                byte[] byData = new byte[fs.Length];
                fs.Read(byData, 0, byData.Length);
                fs.Close();
                fs = null;
                this.fileLen = byData.Length;

                // 读取原始日文文件的内容，以便于对比翻译
                fs = new FileStream(this.oldTresFile, FileMode.Open);
                byte[] byOldData = new byte[fs.Length];
                fs.Read(byOldData, 0, byOldData.Length);
                fs.Close();
                fs = null;

                // 取英文文件，以便于翻译
                byte[] byOldDataEn = null;
                this.oldTresFileEn = this.tresFile.Replace("_汉化", "_en");
                if (File.Exists(this.oldTresFileEn))
                {
                    fs = new FileStream(this.oldTresFileEn, FileMode.Open);
                    byOldDataEn = new byte[fs.Length];
                    fs.Read(byOldDataEn, 0, byOldDataEn.Length);
                    fs.Close();
                    fs = null;
                }

                // 取得完成度相关信息
                if (File.Exists(this.completeFile))
                {
                    string[] completeStr = File.ReadAllLines(this.completeFile);
                    completeList.Clear();
                    foreach (string item in completeStr)
                    {
                        if ("0".Equals(item))
                        {
                            completeList.Add(false);
                        }
                        else
                        {
                            completeList.Add(true);
                            completeNum++;
                        }
                    }
                }

                // 取得字符串Offset表开始位置，以及Offset表个数
                int offsetTableNum = Util.GetOffset(byData, 0x8, 0xB);
                int offsetTablePos = Util.GetOffset(byData, 0xC, 0xF);
                this.tresGrid.SuspendLayout();
                if (offsetTableNum != offsetTablePos)
                {
                    // 第一种类型
                    this.tresType = 1;
                    this.strEntries = 0;
                    this.ViewTresTypeOne(byData, byOldData, byOldDataEn, offsetTableNum, offsetTablePos);
                }
                else
                {
                    // 第二种类型
                    this.tresType = 2;
                    offsetTableNum = Util.GetOffset(byData, 0x4, 0x7);
                    this.strEntries = offsetTablePos + 0x10;
                    this.ViewTresTypeTwo(byData, byOldData, byOldDataEn, offsetTableNum, 0x10);
                }
                this.tresGrid.ResumeLayout(true);

                this.SetTitleInfo();
                this.CloseProcessBar();
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
        /// 取得类型一的字符串offset表
        /// </summary>
        /// <param name="byData"></param>
        /// <returns></returns>
        private List<int> GetTypeOneOffsetTable(byte[] byData, int offsetTableNum, int offsetTablePos)
        {
            List<int> offsetTable = new List<int>();
            for (int i = 0; i < offsetTableNum; i++)
            {
                offsetTable.Add(Util.GetOffset(byData, offsetTablePos, offsetTablePos + 3));
                offsetTablePos += 4;
            }

            return offsetTable;
        }

        /// <summary>
        /// 取得类型二的字符串offset表
        /// </summary>
        /// <param name="byData"></param>
        /// <returns></returns>
        private List<TresSection> GetTypeTwoOffsetTable(byte[] byData, int offsetTableNum, int offsetTablePos)
        {
            List<TresSection> offsetTable = new List<TresSection>();
            TresSection node;
            for (int i = 0; i < offsetTableNum; i++)
            {
                node = new TresSection();
                node.StrOffset = Util.GetOffset(byData, offsetTablePos, offsetTablePos + 3);
                node.Padding1 = Util.GetOffset(byData, offsetTablePos + 4, offsetTablePos + 7);
                node.Padding2 = Util.GetOffset(byData, offsetTablePos + 8, offsetTablePos + 11);
                node.StrNextOffset = Util.GetOffset(byData, offsetTablePos + 12, offsetTablePos + 15);
                offsetTablePos += 0x10;

                offsetTable.Add(node);
            }

            return offsetTable;
        }

        /// <summary>
        /// 查看第一种类型的Txtres文件
        /// </summary>
        /// <param name="byData"></param>
        /// <param name="offsetNum"></param>
        /// <param name="offsetTablePos"></param>
        private void ViewTresTypeOne(byte[] byData, byte[] byOldData, byte[] byOldDataEn, int offsetTableNum, int offsetTablePos)
        {
            // 取得字符串offset表
            List<int> offsetTable = this.GetTypeOneOffsetTable(byData, offsetTableNum, offsetTablePos);
            List<int> oldOffsetTable = this.GetTypeOneOffsetTable(byOldData, offsetTableNum, offsetTablePos);
            List<int> oldOffsetTableEn = this.GetTypeOneOffsetTable(byOldDataEn, offsetTableNum, offsetTablePos);
            
            // 特殊处理
            int tempEn = oldOffsetTableEn[1810];
            oldOffsetTableEn.RemoveAt(1810);
            oldOffsetTableEn.Add(tempEn);

            // 保存字符串Section的开始位置
            this.stringSectionStartPos = offsetTable[0];

            // 设置等待条相关信息
            this.ResetProcessBar(offsetTableNum);

            // 根据字符串offset表，取得字符串Section信息
            this.tresGrid.Rows.Clear();
            int checkValueIndex = 0;
            for (int i = 0; i < offsetTable.Count; i++)
            {
                int offsetPos = offsetTable[i];
                int oldOffsetPos = oldOffsetTable[i];
                int oldOffsetPosEn = oldOffsetTableEn[i];

                int strNum = Util.GetOffset(byData, offsetPos, offsetPos + 3);
                int padding1 = Util.GetOffset(byData, offsetPos + 4, offsetPos + 7);
                string padding1Text = string.Empty;
                if (padding1 != 0)
                {
                    padding1Text = Util.GetFileNameFromStringTable(byData, padding1, this.encoding);
                }
                int padding2 = Util.GetOffset(byData, offsetPos + 8, offsetPos + 11);
                int strOffset = Util.GetOffset(byData, offsetPos + 12, offsetPos + 15);
                int strNextOffset = Util.GetOffset(byData, offsetPos + 16, offsetPos + 19);
                string jpText = Util.GetFileNameFromStringTable(byData, strOffset, padding2, this.encoding);
                string[] splitJpText = jpText.Split('\n');

                // 取得原始日文文本
                int oldStrOffset = Util.GetOffset(byOldData, oldOffsetPos + 12, oldOffsetPos + 15);
                int padding2Jp = Util.GetOffset(byOldData, oldOffsetPos + 8, oldOffsetPos + 11);
                string oldText = Util.GetFileNameFromStringTable(byOldData, oldStrOffset, padding2Jp, this.encoding);

                // 取得原始英文文本
                int oldStrOffsetEn = Util.GetOffset(byOldDataEn, oldOffsetPosEn + 12, oldOffsetPosEn + 15);
                int padding2En = Util.GetOffset(byOldDataEn, oldOffsetPosEn + 8, oldOffsetPosEn + 11);
                string oldTextEn = Util.GetFileNameFromStringTable(byOldDataEn, oldStrOffsetEn, padding2En, this.encoding);
                this.enTextList.Add(i, oldTextEn);

                // 判断多行的情况
                for (int j = 0; j < splitJpText.Length; j++)
                {
                    // 将取得的数据追加到Grid中
                    // 开始位置、文本、原文本、不明1、不明2、第几个字符串、下一个字符串位置、原日文文本
                    if (j == 0)
                    {
                        int newRow = this.tresGrid.Rows.Add();
                        DataGridViewCellCollection lineCollection = this.tresGrid.Rows[newRow].Cells;

                        lineCollection[0].Value = strOffset.ToString("x");
                        lineCollection[1].Value = this.GetCnCharFromJp(splitJpText[j]);
                        lineCollection[2].Value = splitJpText[j];
                        lineCollection[3].Value = padding1.ToString();
                        lineCollection[4].Value = padding2.ToString(); 
                        lineCollection[5].Value = strNum.ToString();
                        lineCollection[6].Value = strNextOffset.ToString();
                        lineCollection[7].Value = oldText;
                        lineCollection[8].Value = padding1Text;
                        lineCollection[9].Value = i.ToString();
                        lineCollection[10].Value = this.GetCheckValue(checkValueIndex++);
                    }
                    else
                    {
                        int newRow = this.tresGrid.Rows.Add();
                        DataGridViewCellCollection lineCollection = this.tresGrid.Rows[newRow].Cells;

                        lineCollection[0].Value = "";
                        lineCollection[1].Value = this.GetCnCharFromJp(splitJpText[j]);
                        lineCollection[2].Value = splitJpText[j];
                        lineCollection[3].Value = "0";
                        lineCollection[4].Value = "0";
                        lineCollection[5].Value = "";
                        lineCollection[6].Value = "";
                        lineCollection[7].Value = oldText;
                        lineCollection[8].Value = "";
                        lineCollection[9].Value = i.ToString();
                        lineCollection[10].Value = this.GetCheckValue(checkValueIndex++);
                    }
                }

                this.ProcessBarStep();
            }
        }

        /// <summary>
        /// 查看第二种类型的Txtres文件
        /// </summary>
        /// <param name="byData"></param>
        /// <param name="offsetNum"></param>
        /// <param name="offsetTablePos"></param>
        private void ViewTresTypeTwo(byte[] byData, byte[] byOldData, byte[] byOldDataEn, int offsetTableNum, int offsetTablePos)
        {
            // 取得字符串offset表
            List<TresSection> offsetTable = this.GetTypeTwoOffsetTable(byData, offsetTableNum, offsetTablePos);
            List<TresSection> oldOffsetTable = this.GetTypeTwoOffsetTable(byOldData, offsetTableNum, offsetTablePos);
            List<TresSection> oldOffsetTableEn = this.GetTypeTwoOffsetTable(byOldDataEn, offsetTableNum, offsetTablePos);

            // 保存字符串Section的开始位置
            this.stringSectionStartPos = 0x10;

            // 设置等待条相关信息
            this.ResetProcessBar(offsetTableNum);

            // 根据字符串offset表，取得字符串Section信息
            this.tresGrid.Rows.Clear();
            for (int i = 0; i < offsetTable.Count; i++)
            {
                int offsetPos = offsetTable[i].StrOffset;
                int oldOffsetPos = oldOffsetTable[i].StrOffset;
                string text = this.GetCnCharFromJp(Util.GetFileNameFromStringTable(byData, this.strEntries + offsetPos, this.encoding));
                string oldText = Util.GetFileNameFromStringTable(byOldData, this.strEntries + oldOffsetPos, this.encoding);

                // 取得原始英文文本
                int oldOffsetPosEn = oldOffsetTableEn[i].StrOffset;
                string oldTextEn = Util.GetFileNameFromStringTable(byOldDataEn, this.strEntries + oldOffsetPosEn, this.encoding);
                this.enTextList.Add(i, oldTextEn);

                // 将取得的数据追加到Grid中
                // 开始位置、文本、原文本、Padding1、Padding2、空、空、原日文文本
                object[] rowData = new object[] { 
                        offsetPos.ToString("x"), 
                        text, 
                        text, 
                        offsetTable[i].Padding1.ToString(), 
                        offsetTable[i].Padding2.ToString(), 
                        i.ToString(), 
                        offsetTable[i].StrNextOffset.ToString(),
                        oldText,
                        "",
                        i.ToString(),
                        false
                };

                this.tresGrid.Rows.Add(rowData);
                this.ProcessBarStep();
            }
        }

        /// <summary>
        /// 单元格编辑后处理
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        private void CellEndEdit(int row, int col)
        {
            // 如果是改变偏移列
            if (col == 0)
            {
                return;
            }
            else if (col == 1)
            {
                // 检查是是否变更
                string afterEdit = this.GetJpCharFromCn(this.tresGrid.Rows[row].Cells[col].Value as string);
                string beforeEdit = this.tresGrid.Rows[row].Cells[2].Value as string;
                int lenChanged = this.encoding.GetByteCount(afterEdit);
                int lenOld = this.encoding.GetByteCount(beforeEdit);
                int diff = lenChanged - lenOld;

                // 检查是否删除了关键字
                if (this.tresType == 2)
                {
                    List<string> fixedStrList = this.GetTresTypeTwoFixedStr(beforeEdit);
                    foreach (string keyStr in fixedStrList)
                    {
                        // 删除了关键字
                        if (afterEdit.IndexOf(keyStr) == -1)
                        {
                            MessageBox.Show("不能修改控制字符，例如:<L>");
                            this.tresGrid.Rows[row].Cells[2].Value = beforeEdit;
                            return;
                        }
                    }
                }

                // 改变文件总长度
                this.fileLen += diff;

                // 长度变化
                if (diff != 0)
                {
                    // 设置颜色
                    this.tresGrid.Rows[row].Cells[col].Style.BackColor = Color.Red;

                    // 重新计算后面的字符串的所有偏移
                    string strPos;
                    int startPos;
                    for (int i = row + 1; i < this.tresGrid.Rows.Count - 1; i++)
                    {
                        // 改变字符串位置
                        strPos = this.tresGrid.Rows[i].Cells[0].Value as string;
                        if (!string.IsNullOrEmpty(strPos))
                        {
                            startPos = Convert.ToInt32(strPos, 16);
                            startPos += diff;
                            this.tresGrid.Rows[i].Cells[0].Value = startPos.ToString("x");

                            // 改变Padding1字符串位置
                            strPos = this.tresGrid.Rows[i].Cells[3].Value as string;
                            if (this.tresType == 1
                                && !string.IsNullOrEmpty(strPos))
                            {
                                startPos = Convert.ToInt32(strPos);
                                if (startPos != 0)
                                {
                                    startPos += diff;
                                    this.tresGrid.Rows[i].Cells[3].Value = startPos.ToString();
                                }
                            }
                        }
                    }
                }

                // 保存修改后的值
                this.tresGrid.Rows[row].Cells[2].Value = afterEdit;
            }
            else if (col == 4
                && this.tresType == 1)
            {
                try
                {
                    // 修改了多行的情况
                    int newPadding2 = Convert.ToInt32(this.tresGrid.Rows[row].Cells[4].Value);
                    if (newPadding2 >= this.oldPadding2
                        || newPadding2 <= 0)
                    {
                        MessageBox.Show("修正不正确，恢复成原来的值！");
                        this.tresGrid.Rows[row].Cells[4].Value = this.oldPadding2;
                        return;
                    }
                    else
                    {
                        int delRows = this.oldPadding2 - newPadding2;
                        int delRowIndex = row + newPadding2;
                        while (delRows > 0)
                        {
                            this.tresGrid.Rows.RemoveAt(delRowIndex);
                            delRows--;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("这里必须是数值类型，并且要大于0并且小于原来的值！");
                    this.tresGrid.Rows[row].Cells[4].Value = this.oldPadding2;
                }
            }
        }

        /// <summary>
        /// CopyTres文件
        /// </summary>
        private void CopyTres()
        {
            FileStream fs = null;
            try
            {
                // 将文件中的数据，读取到byData中
                fs = new FileStream(this.baseFile, FileMode.Open);
                byte[] byData = new byte[fs.Length];
                fs.Read(byData, 0, byData.Length);
                fs.Close();
                fs = null;

                // 取得字符串Offset表开始位置，以及Offset表个数
                int offsetTableNum = Util.GetOffset(byData, 0x8, 0xB);
                int offsetTablePos = Util.GetOffset(byData, 0xC, 0xF);

                // 设置等待条相关信息
                this.ResetProcessBar(offsetTableNum);
                
                // 取得字符串offset表
                List<int> offsetTable = this.GetTypeOneOffsetTable(byData, offsetTableNum, offsetTablePos);

                // 根据字符串offset表，取得字符串Section信息
                int rowIndex = 0;
                for (int i = 0; i < 160; i++)
                {
                    int offsetPos = offsetTable[i];

                    int padding2 = Util.GetOffset(byData, offsetPos + 8, offsetPos + 11);
                    int strOffset = Util.GetOffset(byData, offsetPos + 12, offsetPos + 15);
                    string jpText = Util.GetFileNameFromStringTable(byData, strOffset, padding2, this.encoding);
                    string[] splitJpText = jpText.Split('\n');

                    if (padding2 == 0)
                    {
                        rowIndex++;
                        continue;
                    }

                    // 判断多行的情况
                    for (int j = 0; j < splitJpText.Length; j++)
                    {
                        // 将取得的数据追加到Grid中
                        if (j == 0)
                        {
                            // 设置Padding2的值
                            if (padding2 != Convert.ToInt32(this.tresGrid.Rows[rowIndex].Cells[4].Value as string))
                            {
                                this.tresGrid.Rows[rowIndex].Cells[4].Value = padding2.ToString();
                                this.CellEndEdit(rowIndex, 4);
                            }
                        }
                        
                        // 设置修改的文本
                        if (!splitJpText[j].Equals(this.tresGrid.Rows[rowIndex].Cells[1].Value as string))
                        {
                            this.tresGrid.Rows[rowIndex].Cells[1].Value = this.GetCnCharFromJp(splitJpText[j]);
                            this.CellEndEdit(rowIndex, 1);
                        }

                        rowIndex++;
                    }

                    this.ProcessBarStep();
                }
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
        /// 保存完成度
        /// </summary>
        private void SaveCompleteList()
        {
            string[] saveInfo = new string[this.tresGrid.Rows.Count - 1];
            for (int i = 0; i < this.tresGrid.Rows.Count - 1; i++)
            {
                saveInfo[i] = ((bool)this.tresGrid.Rows[i].Cells[10].Value) ? "1" : "0";
            }

            File.WriteAllLines(this.completeFile, saveInfo);
        }

        /// <summary>
        /// 返回字符串Section字节信息
        /// </summary>
        /// <param name="strNum"></param>
        /// <param name="padding1"></param>
        /// <param name="padding2"></param>
        /// <param name="strOffset"></param>
        /// <param name="nextStrOffset"></param>
        /// <returns></returns>
        private byte[] GetStringSection(int strNum, int padding1, int padding2, int strOffset, int nextStrOffset)
        {
            if (this.tresType == 1)
            {
                byte[] stringSection = new byte[20];

                stringSection[0] = (byte)((strNum >> 24) & 0xFF);
                stringSection[1] = (byte)((strNum >> 16) & 0xFF);
                stringSection[2] = (byte)((strNum >> 8) & 0xFF);
                stringSection[3] = (byte)(strNum & 0xFF);

                stringSection[4] = (byte)((padding1 >> 24) & 0xFF);
                stringSection[5] = (byte)((padding1 >> 16) & 0xFF);
                stringSection[6] = (byte)((padding1 >> 8) & 0xFF);
                stringSection[7] = (byte)(padding1 & 0xFF);

                stringSection[8] = (byte)((padding2 >> 24) & 0xFF);
                stringSection[9] = (byte)((padding2 >> 16) & 0xFF);
                stringSection[10] = (byte)((padding2 >> 8) & 0xFF);
                stringSection[11] = (byte)(padding2 & 0xFF);

                stringSection[12] = (byte)((strOffset >> 24) & 0xFF);
                stringSection[13] = (byte)((strOffset >> 16) & 0xFF);
                stringSection[14] = (byte)((strOffset >> 8) & 0xFF);
                stringSection[15] = (byte)(strOffset & 0xFF);

                stringSection[16] = (byte)((nextStrOffset >> 24) & 0xFF);
                stringSection[17] = (byte)((nextStrOffset >> 16) & 0xFF);
                stringSection[18] = (byte)((nextStrOffset >> 8) & 0xFF);
                stringSection[19] = (byte)(nextStrOffset & 0xFF);

                return stringSection;
            }
            else if (this.tresType == 2)
            {
                byte[] stringSection = new byte[16];

                stringSection[0] = (byte)((strOffset >> 24) & 0xFF);
                stringSection[1] = (byte)((strOffset >> 16) & 0xFF);
                stringSection[2] = (byte)((strOffset >> 8) & 0xFF);
                stringSection[3] = (byte)(strOffset & 0xFF);

                stringSection[4] = (byte)((padding1 >> 24) & 0xFF);
                stringSection[5] = (byte)((padding1 >> 16) & 0xFF);
                stringSection[6] = (byte)((padding1 >> 8) & 0xFF);
                stringSection[7] = (byte)(padding1 & 0xFF);

                stringSection[8] = (byte)((padding2 >> 24) & 0xFF);
                stringSection[9] = (byte)((padding2 >> 16) & 0xFF);
                stringSection[10] = (byte)((padding2 >> 8) & 0xFF);
                stringSection[11] = (byte)(padding2 & 0xFF);

                stringSection[12] = (byte)((nextStrOffset >> 24) & 0xFF);
                stringSection[13] = (byte)((nextStrOffset >> 16) & 0xFF);
                stringSection[14] = (byte)((nextStrOffset >> 8) & 0xFF);
                stringSection[15] = (byte)(nextStrOffset & 0xFF);

                return stringSection;
            }

            return new byte[0];
        }

        /// <summary>
        /// 取得字符串中不能修改的部分
        /// 比如<li>、<L:X>等
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private List<string> GetTresTypeTwoFixedStr(string text)
        {
            List<string> fixedStr = new List<string>();

            if (!string.IsNullOrEmpty(text))
            {
                // 例如
                // 黄色部分は任天堂提供の<L>メッセージリスト準拠です。<L>消すな！！！<L><L>翻訳不要
                string[] splitLeft = text.Split('<');
                for (int i = 0; i < splitLeft.Length; i++)
                {
                    if (splitLeft[i].IndexOf(">") != -1)
                    {
                        string[] splitRight = splitLeft[i].Split('>');
                        fixedStr.Add("<" + splitRight[0] + ">");
                    }
                }
            }

            return fixedStr;
        }

        #endregion
    }
}
