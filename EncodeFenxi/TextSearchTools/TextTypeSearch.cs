using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hanhua.Common;

namespace Hanhua.TextSearchTools
{
    /// <summary>
    /// 文本类型的查找
    /// </summary>
    public class TextTypeSearch : SearchBase
    {
        #region " 初始化 "

        /// <summary>
        /// 初始化
        /// </summary>
        public TextTypeSearch(DataGridView gridSearch, string fileName, string keyWords)
            : base(gridSearch, fileName, keyWords, SearchType.Text)
        {
        }

        #endregion

        #region " 子类重写父类的虚方法 "

        /// <summary>
        /// 查找文本
        /// </summary>
        /// <param name="byData">当前查找文件的字节数据</param>
        /// <returns>是否查找成功</returns>
        protected override bool Search(byte[] byData)
        {
            string[] keys = this.keyWords.Split(',');

            // 文本检索
            for (int j = 0; j < this.EncodingList.Count; j++)
            {
                string fileStr = Util.DecodeByteArray(byData, this.EncodingList[j].GetDecoder());
                foreach (string key in keys)
                {
                    if (fileStr.Contains(key))
                    {
                        int keyIndex = fileStr.IndexOf(key);
                        this.AddFindedRowInfo(keyIndex + " - " + key.Length, 
                            fileStr.Substring(keyIndex - 10, Math.Min(key.Length + 50, fileStr.Length - (keyIndex - 10))), 
                            j);
                    }
                }
            }

            return true;
        }

        #endregion
    }
}
