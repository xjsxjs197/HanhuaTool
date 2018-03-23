using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hanhua.TextSearchTools
{
    /// <summary>
    /// Ngc字库类型的查找
    /// </summary>
    public class NgcFontTypeSearch : SearchBase
    {
        #region " 初始化 "

        /// <summary>
        /// 初始化
        /// </summary>
        public NgcFontTypeSearch(DataGridView gridSearch, string fileName, string keyWords)
            : base(gridSearch, fileName, keyWords, SearchType.NgcFont)
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
            if (this.FileName.EndsWith(".bfn", StringComparison.OrdinalIgnoreCase))
            {
                this.AddFindedRowInfo("0", string.Empty, 0);
            }
            else
            {
                byte[] keyBytes = new byte[] { 0x46, 0x4F, 0x4E, 0x54, 0x62, 0x66, 0x6E };

                // 二进制检索
                bool findedKey = true;
                int maxLen = byData.Length - 0x20;

                for (int j = 0; j < maxLen; j++)
                {
                    if (byData[j] == keyBytes[0])
                    {
                        // 匹配第一个关键字：RFNT
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
                            this.AddFindedRowInfo(j + " - " + (j + 0x20), string.Empty, 0);

                            return true;
                        }
                    }
                }
            }

            return true;
        }

        #endregion
    }
}
