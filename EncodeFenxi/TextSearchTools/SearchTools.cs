using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Hanhua.TextEditTools.Bio1Edit;
using Hanhua.TextEditTools.ViewtifulJoe;
using Hanhua.TextSearchTools;

namespace Hanhua.Common
{
    /// <summary>
    /// 文件分析，将文件里的日文找出
    /// </summary>
    public partial class Fenxi : BaseForm
    {
        #region " 私有变量 "

        /// <summary>
        /// 分析的结果
        /// </summary>
        private List<ResultLine> resultList = new List<ResultLine>();

        /// <summary>
        /// 页面选择的解码器（可以多选）
        /// </summary>
        private List<Encoding> encodingList = new List<Encoding>();

        /// <summary>
        /// 要过滤的文件（不需要查询）的后缀名
        /// </summary>
        private Dictionary<string, string> notSearchFile;

        /// <summary>
        /// 查询引擎
        /// </summary>
        private SearchBase searchItem = null;

        #endregion

        #region " 构造方法 "

        /// <summary>
        /// 初始化
        /// </summary>
        public Fenxi()
        {
            // 控件初始化
            InitializeComponent();

            this.ResetHeight();

            // 绑定再查询按钮事件
            this.gridSearchResult.CellContentClick += new DataGridViewCellEventHandler(gridSearchResult_CellContentClick);

            this.cmbUnicodeType.SelectedIndex = 0;
            this.cmbTxtToByte.SelectedIndex = 0;
        }

        #endregion

        #region " 页面事件 "

        /// <summary>
        /// 重新分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridSearchResult_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 6 || this.gridSearchResult[e.ColumnIndex, e.RowIndex].Value == null)
            {
                return;
            }

            // 打开新的窗口
            DataGridViewCellCollection lineCollection = this.gridSearchResult.Rows[e.RowIndex].Cells;
            SingleFileResult reFenxi = new SingleFileResult(
                lineCollection[0].Value.ToString(),
                Convert.ToInt32(lineCollection[5].Value),
                lineCollection[2].Value.ToString(),
                lineCollection[3].Value.ToString());

            reFenxi.Show();
        }

        /// <summary>
        /// 开始单个文件分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartFenxi_Click(object sender, EventArgs e)
        {
            // 输入条件检查
            this.baseKeyWords = InputCheck();
            if (string.IsNullOrEmpty(this.baseKeyWords))
            {
                return;
            }

            // 打开要分析的文件
            this.baseFile = Util.SetOpenDailog(string.Empty, string.Empty);
            if (string.IsNullOrEmpty(this.baseFile))
            {
                return;
            }

            // 清空Grid
            this.gridSearchResult.Rows.Clear();

            // 设置不需要查找的文件
            this.notSearchFile = Util.GetNotSearchFile();

            // 设置查询引擎
            this.searchItem = this.GetSearchItem(this.baseFile, this.baseKeyWords);
            if (this.searchItem == null)
            {
                return;
            }

            // 查找单个文件
            this.Do(this.SearchFile);
        }

        /// <summary>
        /// 分析整个目录，提高效率
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMuluFenxi_Click(object sender, EventArgs e)
        {
            // 输入条件检查
            this.baseKeyWords = InputCheck();
            if (string.IsNullOrEmpty(this.baseKeyWords))
            {
                return;
            }

            // 取得目录信息
            this.baseFolder = Util.OpenFolder(string.Empty);
            if (string.IsNullOrEmpty(this.baseFolder))
            {
                return;
            }

            // 清空Grid
            this.gridSearchResult.Rows.Clear();

            // 设置查询引擎
            this.searchItem = this.GetSearchItem(this.baseFile, this.baseKeyWords);
            if (this.searchItem == null)
            {
                return;
            }

            // 查找目录
            this.Do(this.SearchFolder);
        }

        /// <summary>
        /// 查看已经保存的结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLook_Click(object sender, EventArgs e)
        {
            SingleFileResult reFenxi = new SingleFileResult();

            reFenxi.Show();
        }

        /// <summary>
        /// 二进制查询控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoBin_CheckedChanged(object sender, EventArgs e)
        {
            this.gbxDecoder.Enabled = !this.rdoBin.Checked;
        }

        /// <summary>
        /// 差值格式查找
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoDiffSearch_CheckedChanged(object sender, EventArgs e)
        {
            this.gbxDecoder.Enabled = !this.rdoDiffSearch.Checked;
        }

        /// <summary>
        /// 查找特定格式文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoSpecial_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdoSpecial.Checked)
            {
                this.cboSpecial.Enabled = true;
                this.cboSpecial.SelectedIndex = 0;
                this.txtKeyWord.Text = "00 20 AF 30";
            }
            else
            {
                this.cboSpecial.Enabled = false;
            }
        }

        /// <summary>
        /// 特殊格式选择变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboSpecial_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.cboSpecial.SelectedIndex)
            {
                // Tpl格式图片
                case 0:
                    this.txtKeyWord.Text = "00 20 AF 30";
                    this.rdoBin.Checked = true;
                    break;

                // REFF格式图片
                case 1:
                    this.txtKeyWord.Text = "FEFF";
                    break;

                // REFT格式图片
                case 2:
                    this.txtKeyWord.Text = "FEFT";
                    break;

                // TEX0格式图片
                case 3:
                    this.txtKeyWord.Text = "TEX0";
                    break;

                // Wii字库文件
                case 4:
                    this.txtKeyWord.Text = "FONT";
                    break;

                // NGC字库文件
                case 5:
                    this.txtKeyWord.Text = "FNT";
                    break;

                // RARC文件
                case 6:
                    this.txtKeyWord.Text = "RARC";
                    break;

                // Uｪ8-文件
                case 7:
                    this.txtKeyWord.Text = "Uｪ8-";
                    break;

                // Yaz0压缩格式文件
                case 8:
                    this.txtKeyWord.Text = this.GetByteFromStr("Yaz0");
                    this.rdoBin.Checked = true;
                    break;
            }
        }

        /// <summary>
        /// 文本转换协助
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTextChg_Click(object sender, EventArgs e)
        {
            if (!this.CheckKeyWord())
            {
                return;
            }

            this.Do(this.TextChgHelp);
        }

        /// <summary>
        /// 转换类型变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTxtToByte_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.cmbTxtToByte.SelectedIndex)
            {
                // 通常文本转换（Shift-Jis）
                // 通常文本转换（Utf-8）
                case 0:
                case 1:
                    this.rdoToPos.Enabled = true;
                    this.rdoToByte.Enabled = true;
                    break;

                // 生化1文件文本转换
                // 生化1通常文本转换
                case 2:
                case 3:
                    this.rdoToPos.Checked = true;
                    this.rdoToPos.Enabled = false;
                    this.rdoToByte.Enabled = false;
                    break;
            }
        }

        /// <summary>
        /// 转换成二进制时，直接设置二进制查找
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoToByte_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdoToByte.Checked)
            {
                this.rdoBin.Checked = true;
            }
        }

        #endregion

        #region " 私有方法 "

        /// <summary>
        /// 字符串转16进制字节数据
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string GetByteFromStr(string text)
        {
            byte[] byText = System.Text.Encoding.GetEncoding("Shift-Jis").GetBytes(text);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < byText.Length; i++)
            {
                sb.Append(" " + byText[i].ToString("x"));
            }

            return sb.ToString().Substring(1);
        }

        /// <summary>
        /// 检查输入
        /// </summary>
        /// <returns></returns>
        private bool CheckKeyWord()
        {
            if (string.IsNullOrEmpty(this.txtKeyWord.Text.Trim()))
            {
                MessageBox.Show("请输入待转换的文本");
                this.txtKeyWord.Focus();
                return false;
            }
            else
            {
                this.baseKeyWords = this.txtKeyWord.Text.Trim();
                return true;
            }
        }

        /// <summary>
        /// 取得查找的引擎
        /// </summary>
        /// <returns></returns>
        private SearchBase GetSearchItem(string fileName, string keyWords)
        {
            SearchBase searchItem = null;
            if (this.rdoBin.Checked || (this.rdoSpecial.Checked && this.cboSpecial.SelectedIndex == 0))
            {
                // 二进制格式查询
                searchItem = new BinTypeSearch(this.gridSearchResult, fileName, keyWords);
            }
            else if (this.rdoText.Checked)
            {
                // 文本格式查找
                searchItem = new TextTypeSearch(this.gridSearchResult, fileName, keyWords);
            }
            else if (this.cboSpecial.SelectedIndex == 4)
            {
                // Wii字库类型的查找
                searchItem = new WiiFontTypeSearch(this.gridSearchResult, fileName, keyWords);
            }
            else if (this.cboSpecial.SelectedIndex == 5)
            {
                // Ngc字库类型的查找
                searchItem = new NgcFontTypeSearch(this.gridSearchResult, fileName, keyWords);
            }
            else
            {
                if (keyWords.Length < 3)
                {
                    MessageBox.Show("关键字太短，无法查找！");
                    return null;
                }

                // 差值格式查找
                if (this.rdoDiffSearch.Checked)
                {
                    searchItem = new DiffOneByteTypeSearch(this.gridSearchResult, fileName, keyWords);
                }
                else
                {
                    searchItem = new DiffTwoByteTypeSearch(this.gridSearchResult, fileName, keyWords);
                    //searchItem = new CharWidthTypeSearch(this.gridSearchResult, fileName, keyWords);
                }
            }

            searchItem.EncodingList = this.encodingList;
            return searchItem;
        }

        /// <summary>
        /// 根据ID活动解码器名称
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        private string GetDecoderName(int id)
        {
            string decoderName = string.Empty;
            switch (id)
            {
                case 0:
                    decoderName = "Shift-Jis";
                    break;

                case 1:
                    decoderName = "Unicode";
                    break;

                case 2:
                    decoderName = "UTF8";
                    break;

                case 3:
                    decoderName = "JIS(20932)";
                    break;

                case 4:
                    decoderName = "iso-2022-jp(50220)";
                    break;

                case 5:
                    decoderName = "euc-jp(51932)";
                    break;
            }

            return decoderName;
        }

        /// <summary>
        /// 取得Shift-jis字符集
        /// </summary>
        /// <returns></returns>
        private string GetShiftJisCharSet()
        {
            StringBuilder sb = new StringBuilder();

            // 读取配置文件中Shift-jis字符集信息
            string strPath = Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName.Replace(
                Assembly.GetExecutingAssembly().ManifestModule.Name, string.Empty);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(strPath + "Shift-JisEncode.xml");

            XmlNode xmlInfo = xmlDoc.SelectSingleNode("/MyEncode/UseShift-JisEncode");

            foreach (XmlNode item in xmlInfo.ChildNodes)
            {
                if (item.NodeType == XmlNodeType.Element)
                {
                    sb.Append(item.InnerText.Replace(" ", "").Replace("\n", "").Replace("\r", ""));
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 输入条件检查
        /// </summary>
        /// <returns>输入的检索关键字</returns>
        private string InputCheck()
        {
            // 检索关键字不能为空
            if (string.IsNullOrEmpty(this.txtKeyWord.Text.Trim()))
            {
                MessageBox.Show("要输入检索关键字！");
                this.txtKeyWord.Focus();
                return string.Empty;
            }

            string strKeyWord = this.txtKeyWord.Text.Trim();
            char[] keyArray = strKeyWord.ToCharArray();
            //string strShiftJisWord = this.GetShiftJisCharSet();

            // 输入的要是日文
            //foreach (char keyChar in keyArray)
            //{
            //    if (strShiftJisWord.IndexOf(keyChar) == -1)
            //    {
            //        MessageBox.Show("这是游戏汉化程序，要输入[日本語]检索关键字！");
            //        this.txtKeyWord.Focus();
            //        return string.Empty;
            //    }
            //}

            // 保存页面选择的解码器
            this.encodingList.Clear();
            if (this.chkShiftJis.Checked)
            {
                this.encodingList.Add(Encoding.GetEncoding("Shift-Jis"));
            }
            if (this.chkUnicode.Checked)
            {
                if (this.cmbUnicodeType.SelectedIndex == 0)
                {
                    this.encodingList.Add(Encoding.BigEndianUnicode);
                }
                else
                {
                    this.encodingList.Add(Encoding.Unicode);
                }
            }
            if (this.chkUtf8.Checked)
            {
                this.encodingList.Add(Encoding.UTF8);
            }
            if (this.chk10001.Checked)
            {
                this.encodingList.Add(Encoding.GetEncoding(10001));
            }
            if (this.chk20932.Checked)
            {
                this.encodingList.Add(Encoding.GetEncoding(20932));
            }
            if (this.chk50220.Checked)
            {
                this.encodingList.Add(Encoding.GetEncoding(50220));
            }
            if (this.chk50221.Checked)
            {
                this.encodingList.Add(Encoding.GetEncoding(50221));
            }
            if (this.chk50222.Checked)
            {
                this.encodingList.Add(Encoding.GetEncoding(50222));
            }
            if (this.chk51932.Checked)
            {
                this.encodingList.Add(Encoding.GetEncoding(51932));
            }

            // 如果解码器未选择报错
            if (this.encodingList.Count == 0)
            {
                MessageBox.Show("请选择解码方式！");
                return string.Empty;
            }

            return strKeyWord;
        }

        /// <summary>
        /// 查找单个文件
        /// </summary>
        private void SearchFile()
        {
            searchItem.StartSearch();

            // 显示完成的提示信息
            this.ShowResultDialog();
        }

        /// <summary>
        /// 查找目录
        /// </summary>
        private void SearchFolder()
        {
            // 开始分析目录
            List<FilePosInfo> allFiles = Util.GetAllFiles(this.baseFolder).Where(p => !p.IsFolder).ToList();
            this.ResetProcessBar(allFiles.Count);

            foreach (FilePosInfo fileItem in allFiles)
            {
                this.ProcessBarStep();

                searchItem.FileName = fileItem.File;
                searchItem.StartSearch();
            }

            // 显示完成的提示信息
            this.CloseProcessBar();

            this.ShowResultDialog();
        }

        /// <summary>
        /// 文本转换协助
        /// </summary>
        private void TextChgHelp()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(this.TextChgHelp));
            }
            else
            {
                string chgText = string.Empty;

                switch (this.cmbTxtToByte.SelectedIndex)
                {
                    // 通常文本转换（Shift-Jis）
                    case 0:
                        chgText = this.TextChgHelpCom(Encoding.GetEncoding("Shift-Jis"), this.rdoToPos.Checked, this.rdoChgStr.Checked);
                        break;

                    // 通常文本转换（Utf-8）
                    case 1:
                        chgText = this.TextChgHelpCom(Encoding.UTF8, this.rdoToPos.Checked, this.rdoChgStr.Checked);
                        break;

                    // 生化1文件文本转换
                    case 2:
                        chgText = Bio1TextEditor.GetDiffData(this.baseKeyWords, false);
                        //chgText = Bio2TextEditor.GetDiffData(this.baseKeyWords);
                        break;

                    // 生化1通常文本转换
                    case 3:
                        chgText = Bio1TextEditor.GetDiffData(this.baseKeyWords, true);
                        break;

                    // 红侠乔伊文本转换
                    case 4:
                        chgText = ViewtifulJoeTextEditor.GetDiffData(this.baseKeyWords);
                        break;

                    // 通常文本转换（Unicode Big end）
                    case 5:
                        chgText = this.TextChgHelpCom(Encoding.BigEndianUnicode, this.rdoToPos.Checked, this.rdoChgStr.Checked);
                        break;

                    // 通常文本转换（Unicode Little end）
                    case 6:
                        chgText = this.TextChgHelpCom(Encoding.Unicode, this.rdoToPos.Checked, this.rdoChgStr.Checked);
                        break;
                }

                this.txtKeyWord.Text = chgText;
            }
        }

        /// <summary>
        /// 通常文本变换协助
        /// </summary>
        /// <param name="encoder">编码格式</param>
        /// <param name="isChgPos">是否转位置</param>
        /// <param name="isChgStr">是否转字符串</param>
        /// <returns></returns>
        private string TextChgHelpCom(Encoding encoder, bool isChgPos, bool isChgStr)
        {
            StringBuilder sb = new StringBuilder();

            if (isChgPos)
            {
                string test = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                for (int i = 0; i < this.baseKeyWords.Length; i++)
                {
                    sb.Append(" " + test.IndexOf(this.baseKeyWords.Substring(i, 1)).ToString());
                }
            }
            else if (isChgStr)
            {
                List<byte> byList = new List<byte>();
                string[] keys = this.baseKeyWords.Trim().Split(' ');
                foreach (string key in keys)
                {
                    byList.Add(Convert.ToByte(key, 16));
                }
                return encoder.GetString(byList.ToArray());
            }
            else
            {
                byte[] byTxt = encoder.GetBytes(this.baseKeyWords);
                for (int i = 0; i < byTxt.Length; i++)
                {
                    sb.Append(" " + byTxt[i].ToString("x"));
                }
            }

            return sb.ToString().Substring(1);
        }

        /// <summary>
        /// 显示完成的提示信息
        /// </summary>
        private void ShowResultDialog()
        {
            int intResultLines = 0;
            this.Invoke((MethodInvoker)delegate()
            {
                intResultLines = this.gridSearchResult.Rows.Count;
            });

            if (intResultLines > 1)
            {
                MessageBox.Show("太好了，找到要汉化的文本了！");
            }
            else
            {
                MessageBox.Show("哎，没有找到任何文本！");
            }
        }

        #endregion
    }
}
