using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Hanhua.Common;
using System.Drawing;

namespace Hanhua.Common
{
    /// <summary>
    /// 再次单个分析
    /// </summary>
    public partial class SingleFileResult : BaseForm
    {
        // 保存的文件
        private string strSaveFile = string.Empty;

        /// <summary>
        /// 保存查询位置
        /// </summary>
        private int resuleIndex = 0;

        /// <summary>
        /// 初始化
        /// </summary>
        public SingleFileResult()
        {
            InitializeComponent();

            this.ResetHeight();

            // 设置默认参数
            this.ddlDecoder.SelectedIndex = 0;
            this.chkRange.Checked = false;
            this.txtResult.SelectionBackColor = Color.YellowGreen;
        }

        /// <summary>
        /// 初始化(带参数，从上一个Form传过来)
        /// </summary>
        public SingleFileResult(string fileName, int decoderIndex, string startEndPos, string result)
        {
            InitializeComponent();

            // 接收参数
            this.txtFileName.Text = fileName;
            this.ddlDecoder.SelectedIndex = decoderIndex;
            this.txtResult.Text = result;

            string[] pos = startEndPos.Split('-');
            this.txtStartPos.Text = pos[0].Trim();
            this.txtEndPos.Text = pos[1].Trim();
        }

        /// <summary>
        /// 输入框检查
        /// </summary>
        /// <returns></returns>
        private bool InputCheck(bool checkFile)
        {
            if (checkFile)
            {
                if (string.IsNullOrEmpty(this.txtFileName.Text.Trim()))
                {
                    // 打开要分析的文件
                    this.baseFile = Util.SetOpenDailog("请选择要分析的文件（*.*）|*.*", string.Empty);
                    if (string.IsNullOrEmpty(this.baseFile))
                    {
                        this.txtFileName.Focus();
                        return false;
                    }

                    this.txtFileName.Text = this.baseFile;
                }
            }
            else
            {
                // 打开要分析的文件
                this.baseFile = Util.SetOpenDailog("请选择要分析的文件（*.*）|*.*", string.Empty);
                if (string.IsNullOrEmpty(this.baseFile))
                {
                    this.txtFileName.Focus();
                    return false;
                }

                this.txtFileName.Text = this.baseFile;
            }

            if (this.ddlDecoder.SelectedIndex < 0)
            {
                MessageBox.Show("请选择编码格式！");
                this.ddlDecoder.Focus();
                return false;
            }

            if (!this.chkRange.Checked)
            {
                return true;
            }

            string strStartPos = this.txtStartPos.Text.Trim();
            try
            {
                Convert.ToInt32(strStartPos, 16);
            }
            catch
            {
                MessageBox.Show("要输入整数！");
                this.txtStartPos.Focus();
                return false;
            }

            string strEndPos = this.txtEndPos.Text.Trim();
            try
            {
                Convert.ToInt32(strEndPos, 16);
            }
            catch
            {
                MessageBox.Show("要输入整数！");
                this.txtEndPos.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 取得解码器
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Encoding GetEncodingByIndex(int index)
        {
            switch (index)
            {
                case 0:
                    return Encoding.GetEncoding("Shift-Jis");

                case 1:
                    if (this.cmbUnicodeType.SelectedIndex == 0)
                    {
                        return Encoding.BigEndianUnicode;
                    }
                    else
                    {
                        return Encoding.Unicode;
                    }

                case 2:
                    return Encoding.UTF8;

                case 3:
                    // Jis 0208-1990-0212-1990
                    return Encoding.GetEncoding(20932);

                case 4:
                    // Jis
                    return Encoding.GetEncoding(50220);

                case 5:
                    // Euc_jp
                    return Encoding.GetEncoding(51932);

                case 6:
                    // Mac_jp
                    return Encoding.GetEncoding(10001);

                case 7:
                    // Jis1 Allow 1 byte Kana
                    return Encoding.GetEncoding(50221);

                case 8:
                    // Jis1 Allow 1 byte Kana - SO/SI
                    return Encoding.GetEncoding(50222);

                case 9:
                    // Utf32
                    return Encoding.UTF32;

                case 10:
                    // Utf7
                    return Encoding.UTF7;
            }

            return Encoding.GetEncoding("Shift-Jis");
        }

        /// <summary>
        /// 开始再分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, System.EventArgs e)
        {
            this.StartFenxi(true);
        }

        /// <summary>
        /// 开始分析文件
        /// </summary>
        /// <param name="checkFile"></param>
        private void StartFenxi(bool checkFile)
        {
            // 输入框检查
            if (!InputCheck(checkFile))
            {
                return;
            }

            this.txtResult.Text = string.Empty;
            int startPos = 0;
            int len = 0;

            FileStream fs = null;

            try
            {
                // 将文件中的数据，循环读取到byData中
                fs = new FileStream(this.txtFileName.Text.Trim(), FileMode.Open);
                if (this.chkRange.Checked)
                {
                    startPos = Convert.ToInt32(this.txtStartPos.Text.Trim(), 16);
                    len = Convert.ToInt32(this.txtEndPos.Text.Trim(), 16);
                    if (startPos + len > fs.Length)
                    {
                        len = (int)fs.Length - startPos;
                    }
                }
                else
                {
                    len = (int)fs.Length;
                }
                byte[] byData = new byte[len];
                fs.Seek(startPos, SeekOrigin.Begin);
                fs.Read(byData, 0, byData.Length);

                // 将当前文件解码成字符串
                string fileChar = Util.DecodeByteArray(byData, this.GetEncodingByIndex(this.ddlDecoder.SelectedIndex).GetDecoder());
                this.txtResult.Text = fileChar;
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
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
        /// 取得文件名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            strSaveFile = ((FileDialog)sender).FileName;
        }

        /// <summary>
        /// 导入保存的文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImport_Click(object sender, EventArgs e)
        {
            strSaveFile = string.Empty;
            DialogResult diaResult = this.openFileDialog.ShowDialog();
            if (diaResult == DialogResult.Cancel)
            {
                return;
            }

            if (string.IsNullOrEmpty(strSaveFile))
            {
                MessageBox.Show("要选择导入的文件！");
                return;
            }

            try
            {
                StreamReader sr = new StreamReader(strSaveFile);
                this.txtFileName.Text = sr.ReadLine();
                this.ddlDecoder.SelectedIndex = Convert.ToInt32(sr.ReadLine());
                this.txtStartPos.Text = sr.ReadLine();
                this.txtEndPos.Text = sr.ReadLine();
                if ("True".Equals(sr.ReadLine()))
                {
                    this.chkRange.Checked = true;
                }
                else
                {
                    this.chkRange.Checked = false;
                }
                sr.Close();
            }
            catch (Exception my)
            {
                MessageBox.Show(my.Message);
            }
        }

        /// <summary>
        /// 选择保存的文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            strSaveFile = ((FileDialog)sender).FileName;
        }

        /// <summary>
        /// 切换Unicode时设置类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ddlDecoder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlDecoder.SelectedIndex == 1)
            {
                this.cmbUnicodeType.SelectedIndex = 0;
                this.cmbUnicodeType.Visible = true;
            }
            else
            {
                this.cmbUnicodeType.Visible = false;
            }
        }

        /// <summary>
        /// 打开其他的文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            this.StartFenxi(false);
        }

        /// <summary>
        /// 结果中再查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReSearch_Click(object sender, EventArgs e)
        {
            string key = this.txtReSearch.Text.Trim();
            if (string.IsNullOrEmpty(key))
            {
                MessageBox.Show("需要输入查询关键字");
                this.txtReSearch.Focus();
                return;
            }

            this.resuleIndex = this.txtResult.Find(key, this.resuleIndex, RichTextBoxFinds.None);
            if (this.resuleIndex > 0)
            {
                this.txtResult.Select(this.resuleIndex, key.Length);
                this.txtResult.ScrollToCaret();
            }
            else
            {
                MessageBox.Show("没有找到结果！");
                this.resuleIndex = 0;
            }
        }
    }
}
