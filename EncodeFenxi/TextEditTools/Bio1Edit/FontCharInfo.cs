using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hanhua.TextEditTools.Bio1Edit
{
    /// <summary>
    /// 字库文字映射类
    /// </summary>
    public class FontCharInfo
    {
        public string Char { get; set; }

        public int Index { get; set; }

        public int LeftPadding { get; set; }

        public int Width { get; set; }

        public FontCharInfo()
        {
        }

        public FontCharInfo(string fontChar, int index)
        {
            this.Char = fontChar;
            this.Index = index;
        }
    }
}
