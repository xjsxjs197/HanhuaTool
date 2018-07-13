using System;
using System.IO;
using Hanhua.Common;

namespace Hanhua.CompressTools
{
    /// <summary>
    /// Yay0压缩文件处理类
    /// </summary>
    public class MarioYay0Comp : BaseComp
    {
        #region " 定数 "

        /// <summary>
        /// yaz0 压缩文件的后缀
        /// </summary>
        private const string COMP_FILE_SUFFIX = ".szp";

        /// <summary>
        /// yaz0 解压缩文件的后缀
        /// </summary>
        private const string DESC_FILE_SUFFIX = ".yay0Dec";

        #endregion

        #region " 子类重写父类方法 "

        /// <summary>
        /// 取得当前Form的Title
        /// </summary>
        /// <returns></returns>
        public override string GetTitle()
        {
            return "阳光马里奥";
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
            return this.Yay0Decode(file);
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="compFile"></param>
        /// <returns></returns>
        public override byte[] Compress(string file)
        {
            throw new Exception("压缩方法未实现");
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

        #region " 私有方法 "

        /// <summary>
        /// 解压缩Yay0格式文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private byte[] Yay0Decode(string file)
        {
            try
            {
                // 将文件中的数据，循环读取到byData中
                byte[] byData = File.ReadAllBytes(file);

                string strMagic = Util.GetHeaderString(byData, 0, 3);
                if (!"Yay0".Equals(strMagic))
                {
                    return null;
                }

                int uncompressedSize = Util.GetOffset(byData, 4, 7);
                byte[] byUncompressed = new byte[uncompressedSize];

                // 开始解压缩
                UInt32 i, j, k;
                UInt32 p, q, r5;
                UInt32 cnt;
                UInt32 r22;
                i = (UInt32)uncompressedSize; // size of decoded data
                j = (UInt32)Util.GetOffset(byData, 8, 0xB); // link table
                k = (UInt32)Util.GetOffset(byData, 0xC, 0xF); // byte chunks and count modifiers
                q = 0; // current offset in dest buffer
                cnt = 0; // mask bit counter
                p = 16; // current offset in mask table
                r22 = 0;

                do
                {
                    // if all bits are done, get next mask
                    if (cnt == 0)
                    {
                        // read word from mask data block
                        //r22 = (UInt32)(byData[p] << 24 | byData[p + 1] << 16 | byData[p + 2] << 8 | byData[p + 3]);
                        r22 = (UInt32)Util.GetOffset(byData, (int)p, (int)(p + 3));
                        p += 4;
                        cnt = 32; // bit counter
                    }
                    // if next bit is set, chunk is non-linked
                    if ((r22 & 0x80000000) == 0x80000000)
                    {
                        // get next byte
                        byUncompressed[q] = byData[k];
                        k++;
                        q++;
                    }
                    // do copy, otherwise
                    else
                    {
                        // read 16-bit from link table
                        //UInt16 r26 = (UInt16)(byData[j] << 8 | byData[j + 1]);
                        UInt16 r26 = (UInt16)Util.GetOffset(byData, (int)j, (int)(j + 1));
                        j += 2;
                        // ’offset’
                        UInt32 r25 = (UInt32)(q - (r26 & 0xfff));
                        // ’count’
                        UInt32 r30 = (UInt32)((r26 >> 12) & 0xf);
                        if (r30 == 0)
                        {
                            // get ’count’ modifier
                            r5 = byData[k];
                            k++;
                            r30 = r5 + 18;
                        }
                        else
                        {
                            r30 += 2;
                        }
                        // do block copy
                        r5 = r25;
                        for (UInt32 tmp = 0; tmp < r30; tmp++)
                        {
                            byUncompressed[q] = byUncompressed[r5 - 1];
                            q++;
                            r5++;
                        }
                    }

                    // next bit in mask
                    r22 <<= 1;
                    cnt--;
                } while (q < i);

                return byUncompressed;
            }
            catch (Exception me)
            {
                throw me;
            }
        }

        #endregion
    }
}
