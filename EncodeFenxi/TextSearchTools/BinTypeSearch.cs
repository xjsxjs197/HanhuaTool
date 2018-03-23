using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hanhua.TextSearchTools
{
    /// <summary>
    /// 二进制类型的查找
    /// </summary>
    public class BinTypeSearch : SearchBase
    {
        #region " 初始化 "

        /// <summary>
        /// 初始化
        /// </summary>
        public BinTypeSearch(DataGridView gridSearch, string fileName, string keyWords)
            : base(gridSearch, fileName, keyWords, SearchType.Bin)
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
            byte[] keyBytes = new byte[keys.Length];
            for (int j = 0; j < keys.Length; j++)
            {
                keyBytes[j] = (byte)Convert.ToInt32(keys[j], 16);
            }

            // 二进制检索
            bool findedKey = true;
            int maxLen = byData.Length - 4;

            for (int j = 0; j < maxLen; j++)
            {
                if (byData[j] == keyBytes[0])
                {
                    findedKey = true;
                    for (int i = 1; i < keyBytes.Length; i++)
                    {
                        if (byData[j + i] != keyBytes[i])
                        {
                            findedKey = false;
                            break;
                        }
                    }

                    if (findedKey)
                    {
                        this.AddFindedRowInfo(j.ToString("x") + " - " + keyBytes.Length, this.keyWords, 0);

                        j += keyBytes.Length - 1;
                    }
                }
            }

            return true;
        }

        #endregion
    }
}
