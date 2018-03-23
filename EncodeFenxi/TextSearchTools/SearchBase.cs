using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Hanhua.TextSearchTools
{
    /// <summary>
    /// 文本查找的基类
    /// </summary>
    public class SearchBase
    {
        #region " 私有变量 "

        /// <summary>
        /// 画面的Grid
        /// </summary>
        protected DataGridView gridSearch;

        /// <summary>
        /// 查找的关键字
        /// </summary>
        protected string keyWords;

        /// <summary>
        /// 查找的类型
        /// </summary>
        protected SearchType searchType;

        #endregion

        #region " 初始化 "

        /// <summary>
        /// 初始化
        /// </summary>
        public SearchBase()
        {
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public SearchBase(DataGridView gridSearch, string fileName, string keyWords, SearchType searchType)
        {
            this.gridSearch = gridSearch;
            this.FileName = fileName;
            this.keyWords = keyWords;
            this.searchType = searchType;
        }

        #endregion

        #region " 公共属性 "

        /// <summary>
        /// 查找的文件
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 页面选择的解码器（可以多选）
        /// </summary>
        public List<Encoding> EncodingList { get; set; }

        #endregion

        #region " 公共方法 "

        /// <summary>
        /// 开始查找
        /// </summary>
        public void StartSearch()
        {
            // 查找的文本
            this.Search(File.ReadAllBytes(this.FileName));
        }

        #endregion

        #region " 保护方法 "

        /// <summary>
        /// 根据ID活动解码器名称
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        protected string GetDecoderName(int id)
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
        /// 追加行信息
        /// </summary>
        /// <param name="findedPosInfo">位置信息</param>
        /// <param name="keyWords">关键字</param>
        /// <param name="decoderIndex">解码器索引</param>
        protected void AddFindedRowInfo(string findedPosInfo, string keyWords, int decoderIndex)
        {
            this.gridSearch.Invoke((MethodInvoker)delegate()
            {
                int newRow = this.gridSearch.Rows.Add();
                DataGridViewCellCollection lineCollection = this.gridSearch.Rows[newRow].Cells;
                string[] strNames = this.FileName.Split('\\');
                lineCollection[0].Value = this.FileName;
                lineCollection[1].Value = strNames[strNames.Length - 1];
                lineCollection[2].Value = findedPosInfo;
                lineCollection[3].Value = keyWords;
                lineCollection[4].Value = this.GetDecoderName(decoderIndex);
                lineCollection[5].Value = decoderIndex;
                lineCollection[6].Value = "再分析";
            });
        }

        #endregion

        #region " 子类需要重写的虚方法 "

        /// <summary>
        /// 查找文本
        /// </summary>
        /// <param name="byData">当前查找文件的字节数据</param>
        /// <returns>是否查找成功</returns>
        protected virtual bool Search(byte[] byData)
        {
            return true;
        }

        #endregion
    }
}
