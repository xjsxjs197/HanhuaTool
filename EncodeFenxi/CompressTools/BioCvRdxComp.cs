using System.IO;
using FraGag.Compression;

namespace Hanhua.CompressTools
{
    /// <summary>
    /// 生化危机Cv Rdx压缩文件处理类
    /// </summary>
    public class BioCvRdxComp : BaseComp
    {
        #region " 定数 "

        /// <summary>
        /// rdx 压缩文件的后缀
        /// </summary>
        private const string COMP_FILE_SUFFIX = ".rdx@";

        /// <summary>
        /// rdx 解压缩文件的后缀
        /// </summary>
        private const string DESC_FILE_SUFFIX = ".bioCvDec";

        #endregion

        #region "全局变量"

        /// <summary>
        /// 外部Exe工具
        /// </summary>
        System.Diagnostics.Process exep = new System.Diagnostics.Process();

        #endregion

        #region " 子类重写父类方法 "

        /// <summary>
        /// 取得当前Form的Title
        /// </summary>
        /// <returns></returns>
        public override string GetTitle()
        {
            return "生化危机Cv";
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
            return Prs.Decompress(file);
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="compFile"></param>
        /// <returns></returns>
        public override byte[] Compress(string file)
        {
            return Prs.Compress(file);
        }

        /// <summary>
        /// 取得默认的路径
        /// </summary>
        /// <returns></returns>
        public override string GetDefaultPath()
        {
            return @"E:\Study\MySelfProject\Hanhua\TodoCn\BioCv\";
            //return @"G:\游戏汉化\红侠乔伊\cn\root\";
        }

        #endregion

        #region " 私有方法（其实没有使用） "

        /// <summary>
        /// 初始化共通的EXE
        /// </summary>
        private void InitExep()
        {
            exep.StartInfo.FileName = @".\PrsUtil.exe";
            exep.StartInfo.CreateNoWindow = true;
            exep.StartInfo.UseShellExecute = false;
        }

        /// <summary>
        /// 解压缩文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private byte[] DecompressFile(string file)
        {
            exep.StartInfo.Arguments = "d " + file + @" temp.prs";
            exep.Start();
            exep.WaitForExit();

            if (File.Exists("temp.prs"))
            {
                return File.ReadAllBytes("temp.prs");
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private byte[] CompressFile(string file)
        {
            exep.StartInfo.Arguments = "c " + file + @" temp.bin";
            exep.Start();
            exep.WaitForExit();

            if (File.Exists("temp.bin"))
            {
                return File.ReadAllBytes("temp.bin");
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
