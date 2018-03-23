using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hanhua.TextSearchTools
{
    /// <summary>
    /// 单字节差值类型的查找
    /// </summary>
    public class DiffOneByteTypeSearch : SearchBase
    {
        #region " 初始化 "

        /// <summary>
        /// 初始化
        /// </summary>
        public DiffOneByteTypeSearch(DataGridView gridSearch, string fileName, string keyWords)
            : base(gridSearch, fileName, keyWords, SearchType.DiffOneByte)
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
            string[] keys = this.keyWords.Split(' ');
            List<int> diffList = new List<int>();

            // 取得关键字的差值列表
            for (int i = 1; i < keys.Length; i++)
            {
                diffList.Add(Convert.ToInt32(keys[i]) - Convert.ToInt32(keys[i - 1]));
            }

            int diffIndex = 0;
            int maxLength = byData.Length - diffList.Count - 2;
            for (int i = 2; i < maxLength; i++)
            {
                diffIndex = 0;
                while (diffIndex < diffList.Count)
                {
                    int char2 = byData[i];
                    int char1 = byData[i - 1];
                    if (char2 - char1 == diffList[diffIndex])
                    {
                        diffIndex++;
                        i += 1;
                    }
                    else
                    {
                        break;
                    }
                }

                if (diffIndex == diffList.Count)
                {
                    int keyIndex = i - (diffList.Count + 1);
                    int len = diffList.Count + 1;
                    StringBuilder sb = new StringBuilder();
                    for (int j = keyIndex; j < keyIndex + len; j++)
                    {
                        sb.Append(byData[j].ToString("x") + " ");
                    }

                    this.AddFindedRowInfo(keyIndex.ToString("x") + " - " + (keyIndex + this.keyWords.Length).ToString("x"), sb.ToString(), 0);

                    i--;
                }
                else
                {
                    i -= diffIndex;
                }
            }

            return true;
        }

        #endregion
    }
}
