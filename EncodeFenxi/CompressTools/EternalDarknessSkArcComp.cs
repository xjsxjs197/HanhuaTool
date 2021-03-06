﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Hanhua.Common;

namespace Hanhua.CompressTools
{
    /// <summary>
    /// 永恒黑暗 SkAsc压缩文件处理类
    /// </summary>
    public class EternalDarknessSkArcComp : BaseComp
    {
        #region " 定数 "

        /// <summary>
        /// SkAsc 压缩文件的后缀
        /// </summary>
        private const string COMP_FILE_SUFFIX = ".cmp";

        /// <summary>
        /// SkAsc 解压缩文件的后缀
        /// </summary>
        private const string DESC_FILE_SUFFIX = ".skAscDec";

        #endregion

        #region " 本地变量 "

        #endregion

        #region " 子类重写父类方法 "

        /// <summary>
        /// 取得当前Form的Title
        /// </summary>
        /// <returns></returns>
        public override string GetTitle()
        {
            return "永恒黑暗";
        }

        /// <summary>
        /// 取得压缩文件后缀名
        /// </summary>
        /// <returns></returns>
        public override string GetCompFileSuffix()
        {
            return COMP_FILE_SUFFIX;
        }

        /// <summary>
        /// 取得解压缩文件后缀名
        /// </summary>
        /// <returns></returns>
        public override string GetDecomFileSuffix()
        {
            return DESC_FILE_SUFFIX;
        }

        /// <summary>
        /// 解压缩文件
        /// </summary>
        /// <param name="compFile"></param>
        /// <returns></returns>
        public override byte[] Decompress(string file)
        {
            return this.SkAscDeCompress(file);
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="compFile"></param>
        /// <returns></returns>
        public override byte[] Compress(string file)
        {
            return null;
        }

        /// <summary>
        /// 取得默认的路径
        /// </summary>
        /// <returns></returns>
        public override string GetDefaultPath()
        {
            return @"E:\Study\MySelfProject\Hanhua\TodoCn\ViewtifulJoe\cn\root\";
            //return @"G:\游戏汉化\红侠乔伊\cn\root\";
        }

        #endregion

        #region " 私有方法 "

        /// <summary>
        /// 解压缩文件
        /// </summary>
        /// <param name="rleFile"></param>
        /// <returns></returns>
        private byte[] SkAscDeCompress(string rleFile)
        {
            // 将文件中的数据，读取到byData中
            byte[] byData = File.ReadAllBytes(rleFile);
            if (!"*SK_ASC*".Equals(Util.GetHeaderString(byData, 0, 7, Encoding.ASCII))) 
            {
                return null;
            }

            

            return null;
        }

        #endregion
    }
}
