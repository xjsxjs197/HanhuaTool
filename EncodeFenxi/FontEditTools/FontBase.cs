using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hanhua.FontEditTools
{
    /// <summary>
    /// 字库处理基类
    /// </summary>
    public abstract class FontBase
    {
        #region " 全局变量 "

        /// <summary>
        /// 字库类型
        /// </summary>
        private FontType fontType = FontType.Other;

        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="fontType"></param>
        public FontBase(FontType fontType)
        {
            this.fontType = fontType;
        }

        #region " 子类可以继承的方法 "



        #endregion

        #region " 私有方法 "



        #endregion
    }
}
