using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hanhua.TextEditTools.Bio0Edit
{
    /// <summary>
    /// 生化0字符信息
    /// </summary>
    public class Bio0CharInfo
    {
        /// <summary>
        /// 字符的数据
        /// </summary>
        public byte[] ByCharInfo { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int CharPage { get; set; }

        public string FontChar { get; set; }

        /// <summary>
        /// 是否使用第二张字库图片
        /// </summary>
        public bool IsUseSecondImg { get; set; }
    }
}
