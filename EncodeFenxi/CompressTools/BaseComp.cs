using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hanhua.CompressTools
{
    /// <summary>
    /// 压缩处理共通类
    /// </summary>
    abstract public class BaseComp
    {
        #region " 子类必须继承的公有方法 "

        /// <summary>
        /// 取得当前Form的Title
        /// </summary>
        /// <returns></returns>
        public abstract string GetTitle();

        /// <summary>
        /// 取得压缩文件后缀名
        /// </summary>
        /// <returns></returns>
        public abstract string GetCompFileSuffix();

        /// <summary>
        /// 取得解压缩文件后缀名
        /// </summary>
        /// <returns></returns>
        public abstract string GetDecomFileSuffix();

        /// <summary>
        /// 解压缩文件
        /// </summary>
        /// <param name="compFile"></param>
        /// <returns></returns>
        public abstract byte[] Decompress(string file);

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="compFile"></param>
        /// <returns></returns>
        public abstract byte[] Compress(string file);

        #endregion

        #region " 子类可以继承的公有方法 "

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="olbFileLen"></param>
        /// <returns></returns>
        public virtual byte[] Compress(string file, int olbFileLen)
        {
            return this.Compress(file);
        }

        /// <summary>
        /// 取得默认的路径
        /// </summary>
        /// <returns></returns>
        public virtual string GetDefaultPath()
        {
            return ".";
        }

        #endregion
    }
}