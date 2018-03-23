using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hanhua.TextSearchTools
{
    /// <summary>
    /// 字符宽度类型的查找
    /// </summary>
    public class CharWidthTypeSearch : SearchBase
    {
        #region " 初始化 "

        /// <summary>
        /// 初始化
        /// </summary>
        public CharWidthTypeSearch(DataGridView gridSearch, string fileName, string keyWords)
            : base(gridSearch, fileName, keyWords, SearchType.DiffTwoByte)
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
            int searchLen = 100;
            int searchStep = 2;
            int maxLength = byData.Length - 1024;
            for (int i = 80; i < maxLength; i++)
            {
                int char1 = byData[i];
                if (char1 >= 25 && char1 <= 32)
                {
                    bool find = true;
                    for (int j = i + searchStep; j < i + searchStep + searchLen; j += searchStep)
                    {
                        if (byData[j] < 25 || byData[j] > 32)
                        {
                            find = false;
                        }
                    }

                    if (find)
                    {
                        this.AddResultRow(i - 80, searchLen, byData);
                        i += searchLen;
                    }
                }
            }

            return true;
        }

        #endregion

        #region " 私有方法 "

        private void AddResultRow(int byIndex, int searchLen, byte[] byData)
        {
            StringBuilder sb = new StringBuilder();
            for (int j = byIndex; j < byIndex + searchLen; j++)
            {
                sb.Append(byData[j].ToString("x") + " ");
            }

            this.AddFindedRowInfo(byIndex.ToString("x") + " - " + (byIndex + searchLen).ToString("x"), sb.ToString(), 0);
        }

        #endregion
    }
}
