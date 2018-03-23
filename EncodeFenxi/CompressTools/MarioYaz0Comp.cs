using System;
using System.Collections.Generic;
using System.IO;
using Hanhua.Common;
using System.Text;

namespace Hanhua.CompressTools
{
    /// <summary>
    /// Yaz0压缩文件处理类
    /// </summary>
    public class MarioYaz0Comp : BaseComp
    {
        #region " 定数 "

        /// <summary>
        /// yaz0 压缩文件的后缀
        /// </summary>
        private const string COMP_FILE_SUFFIX = ".szs";

        /// <summary>
        /// yaz0 解压缩文件的后缀
        /// </summary>
        private const string DESC_FILE_SUFFIX = ".yaz0Dec";

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
            return this.Yaz0Decode(file);
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="compFile"></param>
        /// <returns></returns>
        public override byte[] Compress(string file)
        {
            byte[] byData = File.ReadAllBytes(file);
            return this.Yaz0Encode(byData);
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
        /// 解压缩Yaz0格式文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private byte[] Yaz0Decode(string file)
        {
            FileStream fs = null;

            try
            {
                // 将文件中的数据，循环读取到byData中
                fs = new FileStream(file, FileMode.Open);
                byte[] byHeaderData = new byte[16];
                byte[] byData = new byte[fs.Length - 16];
                fs.Read(byHeaderData, 0, byHeaderData.Length);
                fs.Read(byData, 0, byData.Length);

                string strMagic = Util.GetHeaderString(byHeaderData, 0, 3);
                if (!"Yaz0".Equals(strMagic))
                {
                    return null;
                }

                int uncompressedSize = Util.GetOffset(byHeaderData, 4, 7);
                byte[] byUncompressed = new byte[uncompressedSize];

                // 开始解压缩
                int srcPlace = 0, dstPlace = 0; // current read/write positions 
                UInt32 validBitCount = 0; // number of valid bits left in "code" byte 
                byte currCodeByte = 0;
                while (dstPlace < uncompressedSize)
                {
                    // read new "code" byte if the current one is used up 
                    if (validBitCount == 0)
                    {
                        currCodeByte = byData[srcPlace];
                        ++srcPlace;
                        validBitCount = 8;
                    }

                    if ((currCodeByte & 0x80) != 0)
                    {
                        // straight copy 
                        byUncompressed[dstPlace] = byData[srcPlace];
                        dstPlace++;
                        srcPlace++;
                    }
                    else
                    {
                        // RLE part 
                        byte byte1 = byData[srcPlace];
                        byte byte2 = byData[srcPlace + 1];
                        srcPlace += 2;
                        int dist = ((byte1 & 0xF) << 8) + byte2;
                        int copySource = dstPlace - (dist + 1);
                        int numBytes = byte1 >> 4;
                        if (numBytes == 0)
                        {
                            numBytes = byData[srcPlace] + 0x12;
                            srcPlace++;
                        }
                        else
                            numBytes += 2;
                        // copy run 
                        for (int i = 0; i < numBytes; ++i)
                        {
                            byUncompressed[dstPlace] = byUncompressed[copySource];
                            copySource++;
                            dstPlace++;
                        }
                    }

                    // use next bit from "code" byte 
                    currCodeByte <<= 1;
                    validBitCount -= 1;
                }

                return byUncompressed;
            }
            catch (Exception me)
            {
                throw me;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// simple and straight encoding scheme for Yaz0
        /// </summary>
        /// <param name="src"></param>
        /// <param name="size"></param>
        /// <param name="pos"></param>
        /// <param name="pMatchPos"></param>
        /// <returns></returns>
        private int SimpleEnc(byte[] src, int size, int pos, ref int pMatchPos)
        {
            int startPos = pos - 0x1000;
            int numBytes = 1;
            int matchPos = 0;
            int j = 0;

            if (startPos < 0)
            {
                startPos = 0;
            }
            for (int i = startPos; i < pos; i++)
            {
                for (j = 0; j < size - pos; j++)
                {
                    if (src[i + j] != src[j + pos])
                    {
                        break;
                    }
                }
                if (j > numBytes)
                {
                    numBytes = j;
                    matchPos = i;
                }
            }

            pMatchPos = matchPos;
            if (numBytes == 2)
            {
                numBytes = 1;
            }
            return numBytes;
        }

        /// <summary>
        /// a lookahead encoding scheme for ngc Yaz0
        /// </summary>
        /// <param name="src"></param>
        /// <param name="size"></param>
        /// <param name="pos"></param>
        /// <param name="pMatchPos"></param>
        /// <returns></returns>
        private int NintendoEnc(byte[] src, int size, int pos, ref int pMatchPos)
        {
            int startPos = pos - 0x1000;
            int numBytes = 1;
            int numBytes1 = 0;
            int matchPos = 0;
            int prevFlag = 0;

            // if prevFlag is set, it means that the previous position was determined by look-ahead try.
            // so just use it. this is not the best optimization, but nintendo's choice for speed.
            if (prevFlag == 1)
            {
                pMatchPos = matchPos;
                prevFlag = 0;
                return numBytes1;
            }

            prevFlag = 0;
            numBytes = this.SimpleEnc(src, size, pos, ref matchPos);
            pMatchPos = matchPos;

            // if this position is RLE encoded, then compare to copying 1 byte and next position(pos+1) encoding
            if (numBytes >= 3)
            {
                numBytes1 = this.SimpleEnc(src, size, pos + 1, ref matchPos);
                // if the next position encoding is +2 longer than current position, choose it.
                // this does not guarantee the best optimization, but fairly good optimization with speed.
                if (numBytes1 >= numBytes + 2)
                {
                    numBytes = 1;
                    prevFlag = 1;
                }
            }

            return numBytes;
        }

        /// <summary>
        /// 压缩Yaz0格式文件
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        private byte[] Yaz0Encode(byte[] src)
        {
            int srcSize = src.Length;
            List<byte> dstFile = new List<byte>();
            byte[] dst = new byte[24];    // 8 codes * 3 bytes maximum
            int dstSize = 0;
            int percent = -1;

            int validBitCount = 0; //number of valid bits left in "code" byte
            byte currCodeByte = 0;
            int srcPos = 0;
            int dstPos = 0;
            while (srcPos < srcSize)
            {
                int numBytes = 0;
                int matchPos = 0;
                int srcPosBak = 0;

                numBytes = this.NintendoEnc(src, srcSize, srcPos, ref matchPos);
                if (numBytes < 3)
                {
                    // straight copy
                    dst[dstPos] = src[srcPos];
                    dstPos++;
                    srcPos++;
                    // set flag for straight copy
                    currCodeByte = (byte)(currCodeByte | (0x80 >> validBitCount));
                }
                else
                {
                    // RLE part
                    int dist = srcPos - matchPos - 1;
                    byte byte1, byte2, byte3;

                    if (numBytes >= 0x12)  // 3 byte encoding
                    {
                        byte1 = (byte)(0 | (dist >> 8));
                        byte2 = (byte)(dist & 0xff);
                        dst[dstPos++] = byte1;
                        dst[dstPos++] = byte2;
                        // maximum runlength for 3 byte encoding
                        if (numBytes > 0xff + 0x12)
                        {
                            numBytes = 0xff + 0x12;
                        }
                        byte3 = (byte)(numBytes - 0x12);
                        dst[dstPos++] = byte3;
                    }
                    else  // 2 byte encoding
                    {
                        byte1 = (byte)(((numBytes - 2) << 4) | (dist >> 8));
                        byte2 = (byte)(dist & 0xff);
                        dst[dstPos++] = byte1;
                        dst[dstPos++] = byte2;
                    }
                    srcPos += numBytes;
                }
                validBitCount++;
                // write eight codes
                if (validBitCount == 8)
                {
                    //fwrite(&currCodeByte, 1, 1, dstFile);
                    //fwrite(dst, 1, dstPos, dstFile);
                    dstFile.Add(currCodeByte);
                    this.CopyArray(dst, dstPos, dstFile);
                    dstSize += dstPos + 1;

                    srcPosBak = srcPos;
                    currCodeByte = 0;
                    validBitCount = 0;
                    dstPos = 0;
                }
                if ((srcPos + 1) * 100 / srcSize != percent)
                {
                    percent = (srcPos + 1) * 100 / srcSize;
                }
            }

            if (validBitCount > 0)
            {
                //fwrite(&currCodeByte, 1, 1, dstFile);
                //fwrite(dst, 1, dstPos, dstFile);
                dstFile.Add(currCodeByte);
                this.CopyArray(dst, dstPos, dstFile);
                dstSize += dstPos + 1;

                currCodeByte = 0;
                validBitCount = 0;
                dstPos = 0;
            }

            // 生成Header
            byte[] header = new byte[8];
            byte[] byMagic = Encoding.ASCII.GetBytes("Yaz0");
            Array.Copy(byMagic, 0, header, 0, 4);
            header[4] = (byte)(srcSize >> 24 & 0xFF);
            header[5] = (byte)(srcSize >> 16 & 0xFF);
            header[6] = (byte)(srcSize >> 8 & 0xFF);
            header[7] = (byte)(srcSize & 0xFF);

            dstFile.InsertRange(0, header);

            return dstFile.ToArray();
        }

        /// <summary>
        /// CopyArray
        /// </summary>
        /// <param name="dst"></param>
        /// <param name="copyLen"></param>
        /// <param name="dstFile"></param>
        private void CopyArray(byte[] dst, int copyLen, List<byte> dstFile)
        {
            for (int i = 0; i < copyLen; i++)
            {
                dstFile.Add(dst[i]);
            }
        }

        #endregion
    }
}
