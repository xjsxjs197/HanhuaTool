using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Hanhua.Common;

namespace Hanhua.CompressTools
{
    /// <summary>
    /// 生化危机0 Lz压缩文件处理类
    /// </summary>
    public class Bio0LzComp : BaseComp
    {
        #region " 定数 "

        /// <summary>
        /// lz 压缩文件的后缀
        /// </summary>
        private const string COMP_FILE_SUFFIX = ".alz";

        /// <summary>
        /// lz 解压缩文件的后缀
        /// </summary>
        private const string DESC_FILE_SUFFIX = ".alzDec";

        #endregion

        #region " 本地变量 "

        /// <summary>
        /// Lz压缩解压缩用长度信息
        /// </summary>
        private int[] bitcounts = new int[] { 0x02, 0x04, 0x06, 0x0A };

        #endregion

        #region " 子类重写父类方法 "

        /// <summary>
        /// 取得当前Form的Title
        /// </summary>
        /// <returns></returns>
        public override string GetTitle()
        {
            return "生化危机0";
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
            return this.LzDeCompress(File.ReadAllBytes(file));
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="compFile"></param>
        /// <returns></returns>
        public override byte[] Compress(string file)
        {
            return this.LzCompress(File.ReadAllBytes(file));
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="compFile"></param>
        /// <returns></returns>
        public override byte[] Compress(string file, int olbFileLen)
        {
            return this.LzCompress(File.ReadAllBytes(file), olbFileLen);
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
        /// 解压缩
        /// </summary>
        /// <param name="byData"></param>
        /// <returns></returns>
        private byte[] LzDeCompress(byte[] byCompressedData)
        {
            // 判断是否正确
            int alztype = byCompressedData[0];
            if (alztype != 1 && alztype != 2)
            {
                throw new Exception("不是正确正确的Lz文件！");
            }

            int destsize = byCompressedData[1] | (byCompressedData[2] << 8) | (byCompressedData[3] << 16) | (byCompressedData[4] << 24);
            byte[] dest = new byte[destsize];

            int alz_bitnum = 0, tmp_bitnum, tmp_index;
            int tmp_offset, tmp_size;
            int alzaddr = 5, destaddr = 0;

            while (destaddr < destsize)
            {
                if (this.get_bit(byCompressedData, ref alzaddr, ref alz_bitnum) == 0)
                {
                    tmp_offset = 0;
                    if (alztype == 1)
                    {
                        for (tmp_bitnum = 0; tmp_bitnum < 0x0A; tmp_bitnum++)
                        {
                            tmp_offset |= (this.get_bit(byCompressedData, ref alzaddr, ref alz_bitnum) << tmp_bitnum);
                        }
                    }
                    else
                    {
                        for (tmp_index = 0; this.get_bit(byCompressedData, ref alzaddr, ref alz_bitnum) == 0; tmp_index++)
                        {
                        }

                        if (tmp_index > 3)
                        {
                            throw new Exception("ERROR: Bitcount index out of range at 0x" + alzaddr.ToString("x"));
                        }

                        for (tmp_bitnum = 0; tmp_bitnum < bitcounts[tmp_index]; tmp_bitnum++)
                        {
                            tmp_offset |= (this.get_bit(byCompressedData, ref alzaddr, ref alz_bitnum) << tmp_bitnum);
                        }
                    }

                    if ((destaddr - tmp_offset - 1) < 0)
                    {
                        throw new Exception("ERROR: Invalid source offset at 0x" + alzaddr.ToString("x"));
                    }

                    tmp_size = 0;
                    for (tmp_index = 0; this.get_bit(byCompressedData, ref alzaddr, ref alz_bitnum) == 0; tmp_index++)
                    {
                    }

                    if (tmp_index > 3)
                    {
                        throw new Exception("ERROR: Bitcount index out of range at 0x" + alzaddr.ToString("x"));
                    }

                    for (tmp_bitnum = 0; tmp_bitnum < bitcounts[tmp_index]; tmp_bitnum++)
                    {
                        tmp_size |= (this.get_bit(byCompressedData, ref alzaddr, ref alz_bitnum) << tmp_bitnum);
                    }

                    if (tmp_size > (destsize - (destaddr - tmp_offset - 1)))
                    {
                        throw new Exception("ERROR: Invalid source offset at 0x" + alzaddr.ToString("x"));
                    }

                    Array.Copy(dest, destaddr - tmp_offset - 1, dest, destaddr, tmp_size);

                    destaddr += tmp_size;
                }
                else
                {
                    dest[destaddr] = 0;
                    for (tmp_bitnum = 0; tmp_bitnum < 8; tmp_bitnum++)
                    {
                        dest[destaddr] |= (byte)((this.get_bit(byCompressedData, ref alzaddr, ref alz_bitnum) << tmp_bitnum) & 0xFF);
                    }

                    destaddr += 1;
                }
            }

            return dest;
        }

        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="byData"></param>
        /// <returns></returns>
        private byte[] LzCompress(byte[] byData)
        {
            // 写文件头
            int dataLen = byData.Length;
            List<byte> compressData = new List<byte>();
            compressData.Add(02);
            compressData.Add((byte)(dataLen & 0xFF));
            compressData.Add((byte)((dataLen >> 8) & 0xFF));
            compressData.Add((byte)((dataLen >> 16) & 0xFF));
            compressData.Add((byte)((dataLen >> 24) & 0xFF));
            compressData.Add(0);

            int index = 0;
            int[] compressedInfo = new int[2];
            int bitnum = 0;
            int compressDataAddr = 5;

            // 开始压缩
            while (index < dataLen)
            {
                if (this.CheckCompressData(byData, index, compressedInfo))
                {
                    int compressStartIndex = compressedInfo[0];
                    int sameByteLen = compressedInfo[1];

                    // 写入标志位0
                    compressData[compressDataAddr] &= (byte)(~(byte)(1 << bitnum));

                    // bitnum增加1位
                    this.MoveBitNum(compressData, ref compressDataAddr, ref bitnum);

                    // 写入压缩的开始位置
                    this.WriteCompressedInfo(compressData, this.GetCompressedInfo(compressStartIndex), ref compressDataAddr, ref bitnum);

                    // 写入压缩的长度信息
                    this.WriteCompressedInfo(compressData, this.GetCompressedInfo(sameByteLen), ref compressDataAddr, ref bitnum);

                    index += sameByteLen;
                }
                else
                {
                    // 写入标志位1
                    compressData[compressDataAddr] |= (byte)(1 << bitnum);

                    // 添加未压缩的字节数据
                    this.WriteByte(compressData, byData[index], ref compressDataAddr, ref bitnum);
                    index++;
                }
            }

            return compressData.ToArray();
        }

        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="byData"></param>
        /// <returns></returns>
        private byte[] LzCompress(byte[] byData, int oldFileLen)
        {
            byte[] byCompressData = this.LzCompress(byData);
            if (oldFileLen == 0)
            {
                return byCompressData;
            }
            else
            {
                byte[] byCompressed = new byte[oldFileLen];
                Array.Copy(byCompressData, 0, byCompressed, 0, byCompressData.Length);
                return byCompressed;
            }
        }

        /// <summary>
        /// 检查从Index位置开始的数据，是否是可以压缩的数据
        /// 如果不是返回False
        /// 如果是返回True，compressedInfo[0]是开始位置，compressedInfo[1]是可以压缩的长度
        /// </summary>
        /// <param name="byData"></param>
        /// <param name="index"></param>
        /// <param name="compressedInfo"></param>
        /// <returns></returns>
        private bool CheckCompressData(byte[] byData, int index, int[] compressedInfo)
        {
            if (index < 2 || index >= byData.Length - 1)
            {
                return false;
            }

            int startPos = index - 1023;
            if (startPos < 0)
            {
                startPos = 0;
            }

            // 二进制检索
            int sameByteLen = 0;
            int maxSameByteLen = 0;
            int compressStartIndex = 0;
            for (int j = startPos; j < index; j++)
            {
                if (byData[j] == byData[index])
                {
                    sameByteLen = 1;
                    for (int befIndex = j + 1, aftIndex = index + 1; befIndex < index && aftIndex < byData.Length; befIndex++, aftIndex++)
                    {
                        if (byData[befIndex] == byData[aftIndex])
                        {
                            sameByteLen++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (sameByteLen >= maxSameByteLen)
                    {
                        maxSameByteLen = sameByteLen;
                        compressStartIndex = j;
                    }

                    //if (sameByteLen >= 2)
                    //{
                    //    maxSameByteLen = sameByteLen;
                    //    compressStartIndex = j;
                    //    break;
                    //}
                }
            }

            //if (sameByteLen > 1)
            //{
            //    compressedInfo[0] = index - compressStartIndex - 1;
            //    compressedInfo[1] = sameByteLen;

            //    return true;
            //}
            //else
            //{
            //    return false;
            //}

            //if (maxSameByteLen > 1 && (index - compressStartIndex - 1) <= 1023)
            //{
            //    compressedInfo[0] = index - compressStartIndex - 1;
            //    compressedInfo[1] = maxSameByteLen;

            //    startIndex += maxSameByteLen;

            //    return true;
            //}
            //else
            //{
            //    return false;
            //}

            if (maxSameByteLen > 1)
            {
                compressedInfo[0] = index - compressStartIndex - 1;
                compressedInfo[1] = maxSameByteLen;

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据整形的位置或长度信息，得到压缩的相关信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private string GetCompressedInfo(int info)
        {
            if (info <= 3)
            {
                // 2位的info，加1位的1
                return Convert.ToString(info, 2).PadLeft(2, '0') + "1";
            }
            else if (info <= 15)
            {
                // 加4位的info，加2位的01
                return Convert.ToString(info, 2).PadLeft(4, '0') + "10";
            }
            else if (info <= 63)
            {
                // 6位的info，加3位的001
                return Convert.ToString(info, 2).PadLeft(6, '0') + "100";
            }
            else if (info <= 1023)
            {
                // 10位的info，加4位的0001
                return Convert.ToString(info, 2).PadLeft(10, '0') + "1000";
            }

            throw new Exception("压缩的信息有误 ： " + info);
        }

        /// <summary>
        /// 将压缩信息Info写入压缩的字节数组
        /// </summary>
        /// <param name="compressData"></param>
        /// <param name="info"></param>
        /// <param name="addr"></param>
        /// <param name="bitnum"></param>
        private void WriteCompressedInfo(List<byte> compressData, string info, ref int addr, ref int bitnum)
        {
            if (info.Length < 8)
            {
                byte byInfo = Convert.ToByte(info, 2);
                this.WriteCompressedInfo(compressData, byInfo, info.Length, ref addr, ref bitnum);
            }
            else
            {
                byte byInfo = Convert.ToByte(info.Substring(info.Length - 8), 2);
                this.WriteCompressedInfo(compressData, byInfo, 8, ref addr, ref bitnum);

                byInfo = Convert.ToByte(info.Substring(0, info.Length - 8), 2);
                this.WriteCompressedInfo(compressData, byInfo, info.Length - 8, ref addr, ref bitnum);
            }
        }

        /// <summary>
        /// 将压缩信息Info写入压缩的字节数组
        /// </summary>
        /// <param name="compressData"></param>
        /// <param name="info"></param>
        /// <param name="addr"></param>
        /// <param name="bitnum"></param>
        private void WriteCompressedInfo(List<byte> compressData, byte info, int infoLen, ref int addr, ref int bitnum)
        {
            if (bitnum == 0)
            {
                // bitnum是最开始的位置（第0位）
                compressData[addr] = info;
                bitnum += infoLen;

                if (bitnum == 8)
                {
                    bitnum = 0;
                    addr++;
                    if (addr == compressData.Count)
                    {
                        compressData.Add(0);
                    }
                }
            }
            else
            {
                // 写入低位的字节数据
                int lowByteLen = 8 - bitnum;
                byte lowByte = (byte)(info & Convert.ToByte("1".PadLeft(lowByteLen, '1'), 2));
                compressData[addr] |= (byte)(lowByte << bitnum);

                if (infoLen >= lowByteLen)
                {
                    bitnum = bitnum + infoLen - 8;
                    addr++;
                    if (addr == compressData.Count)
                    {
                        compressData.Add(0);
                    }

                    if (infoLen > lowByteLen)
                    {
                        // 写入高位的字节数据
                        int highByteLen = infoLen - lowByteLen;
                        byte highByte = (byte)((info >> lowByteLen) & Convert.ToByte("1".PadLeft(highByteLen, '1'), 2));
                        compressData[addr] |= highByte;
                    }
                }
                else
                {
                    bitnum += infoLen;
                }
            }
        }

        /// <summary>
        /// bitnum增加一位
        /// </summary>
        /// <param name="compressData"></param>
        /// <param name="addr"></param>
        /// <param name="bitnum"></param>
        private void MoveBitNum(List<byte> compressData, ref int addr, ref int bitnum)
        {
            if (bitnum == 7)
            {
                bitnum = 0;
                addr++;
                if (addr == compressData.Count)
                {
                    compressData.Add(0);
                }
            }
            else
            {
                bitnum++;
            }
        }

        /// <summary>
        /// 将当前data字节数据写入压缩的字节数组
        /// </summary>
        /// <param name="compressData">压缩的字节数组</param>
        /// <param name="data">当前需要写入的字节数据</param>
        /// <param name="addr">压缩的字节数组的位置</param>
        /// <param name="bitnum">当前的位</param>
        private void WriteByte(List<byte> compressData, byte data, ref int addr, ref int bitnum)
        {
            if (bitnum < 7)
            {
                // 写入低位的字节数据
                int lowByteLen = 7 - bitnum;
                byte lowByte = (byte)(data & Convert.ToByte("1".PadLeft(lowByteLen, '1'), 2));
                compressData[addr] |= (byte)(lowByte << (bitnum + 1));

                addr++;
                if (addr == compressData.Count)
                {
                    compressData.Add(0);
                }

                // 写入高位的字节数据
                int highByteLen = 8 - lowByteLen;
                byte highByte = (byte)((data >> lowByteLen) & Convert.ToByte("1".PadLeft(highByteLen, '1'), 2));
                compressData[addr] |= highByte;

                bitnum++;
            }
            else
            {
                // bitnum已经到达当前字节最高位（第8位）
                addr++;
                if (addr == compressData.Count)
                {
                    compressData.Add(0);
                }
                compressData[addr] = data;

                addr++;
                if (addr == compressData.Count)
                {
                    compressData.Add(0);
                }
                bitnum = 0;
            }
        }

        /// <summary>
        /// 取得字节数组中第N个字节的第M位数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="addr"></param>
        /// <param name="bitnum"></param>
        /// <returns></returns>
        private int get_bit(byte[] data, ref int addr, ref int bitnum)
        {
            int bit = (data[addr] >> bitnum) & 1;
            if (bitnum < 7)
            {
                bitnum++;
            }
            else
            {
                bitnum = 0;
                addr++;
            }

            return bit;
        }

        #endregion
    }
}
