using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hanhua.TextSearchTools
{
    /// <summary>
    /// 查找的类型
    /// </summary>
    public enum SearchType
    {
        /// <summary>
        /// 文本查找
        /// </summary>
        Text = 0,

        /// <summary>
        /// 二进制查找
        /// </summary>
        Bin = 1,

        /// <summary>
        /// 单字节差值查找
        /// </summary>
        DiffOneByte = 2,

        /// <summary>
        /// 双字节差值查找
        /// </summary>
        DiffTwoByte = 3,

        /// <summary>
        /// Wii字库
        /// </summary>
        WiiFont = 4,

        /// <summary>
        /// Ngc字库
        /// </summary>
        NgcFont = 5
    }
}
