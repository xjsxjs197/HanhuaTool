using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Hanhua.Common;

namespace Hanhua.CompressTools
{
    /// <summary>
    /// 红侠乔伊 Rle压缩文件处理类
    /// </summary>
    public class ViewtifulJoeRleComp : BaseComp
    {
        #region " 定数 "

        /// <summary>
        /// rle 压缩文件的后缀
        /// </summary>
        private const string COMP_FILE_SUFFIX = ".datgc";

        /// <summary>
        /// rle 解压缩文件的后缀
        /// </summary>
        private const string DESC_FILE_SUFFIX = ".rleDec";

         /// <summary>
        /// 最大压缩长度
        /// </summary>
        private const int MAX_COMP_LEN = 0x7f;

        /// <summary>
        /// 记录压缩块个数的位置
        /// </summary>
        private const int BLOCK_NUM_POS = 0x41;

        #endregion

        #region " 本地变量 "

        /// <summary>
        /// 是否使用第二种压缩方式
        /// </summary>
        private bool isBlock2 = false;

        #endregion

        #region " 子类重写父类方法 "

        /// <summary>
        /// 取得当前Form的Title
        /// </summary>
        /// <returns></returns>
        public override string GetTitle()
        {
            return "红侠乔伊";
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
            return this.RleDeCompress(file);
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="compFile"></param>
        /// <returns></returns>
        public override byte[] Compress(string file)
        {
            return this.RleCompress(file);
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
        /// 查找所有的压缩数据块
        /// </summary>
        /// <param name="byData"></param>
        /// <returns></returns>
        private List<int> GetDecompPosInfo(byte[] byData)
        {
            List<int> posInfo = new List<int>();
            int endPos = byData.Length - 0x800;
            for (int i = 0x800; i < endPos; i += 1)
            {
                // 以RUNLENGTH为关键字
                if (byData[i] == 0x72
                    && byData[i + 1] == 0x75
                    && byData[i + 2] == 0x6E
                    && byData[i + 3] == 0x6C
                    && byData[i + 4] == 0x65
                    && byData[i + 5] == 0x6E
                    && byData[i + 6] == 0x67)
                {
                    posInfo.Add(i);
                }
            }

            return posInfo;
        }

        /// <summary>
        /// 查找所有的压缩数据块
        /// </summary>
        /// <param name="comBlock"></param>
        /// <returns></returns>
        private List<int> GetCompPosInfo(int comBlock)
        {
            List<int> posInfo = new List<int>();
            for (int i = 0; i < comBlock; i++)
            {
                posInfo.Add(i * 0x80000 + 0x800);
            }

            return posInfo;
        }

        /// <summary>
        /// 根据当前循环Index，取得当前Block块的最大压缩到的字节数
        /// </summary>
        /// <param name="posInfo"></param>
        /// <param name="i"></param>
        /// <param name="byData"></param>
        /// <returns></returns>
        private int GetMaxLen(List<int> posInfo, int i, byte[] byData)
        {
            int endPos = 0;
            if (i == posInfo.Count - 1)
            {
                endPos = byData.Length;
            }
            else
            {
                endPos = posInfo[i + 1];
            }

            return endPos;
        }

        /// <summary>
        /// 根据当前循环Index，取得压缩块数据的结束位置
        /// </summary>
        /// <param name="posInfo"></param>
        /// <param name="i"></param>
        /// <param name="byData"></param>
        /// <returns></returns>
        private int GetDecompEndPos(List<int> posInfo, int i, byte[] byData)
        {
            int endPos = 0;
            if (i == posInfo.Count - 1)
            {
                int point1 = Util.GetOffset(byData, 0x34, 0x37) + 0x800;
                int point2 = Util.GetOffset(byData, 0x18, 0x1b);
                if (point2 == 0)
                {
                    endPos = point1 - 1;
                }
                else
                {
                    int temp = point1 & 0xFFF;

                    if (temp > 0x800)
                    {
                        endPos = point1 + (0x1000 - temp) + point2;
                    }
                    else
                    {
                        endPos = point1 + (0x800 - temp) + point2;
                    }
                }
            }
            else
            {
                endPos = posInfo[i + 1];
                // 解压缩时，中间的数据块，最后的0x80不是压缩数据，是Padding信息，不需要解压
                while (byData[endPos - 1] == 0x80)
                {
                    endPos--;
                }
            }

            return endPos;
        }

        /// <summary>
        /// 解压缩文件
        /// </summary>
        /// <param name="rleFile"></param>
        /// <returns></returns>
        private byte[] RleDeCompress(string rleFile)
        {
            // 将文件中的数据，读取到byData中
            byte[] byData = File.ReadAllBytes(rleFile);

            // 查找所有的压缩数据块
            List<int> posInfo = this.GetDecompPosInfo(byData);
            if (posInfo.Count == 0)
            {
                return null;
            }

            // 复制头部数据
            List<byte> decDataList = new List<byte>();
            byte[] byHeader = new byte[posInfo[0]];
            Array.Copy(byData, 0, byHeader, 0, byHeader.Length);
            decDataList.AddRange(byHeader);

            // 将压缩块个数写入0x41
            decDataList[BLOCK_NUM_POS] = (byte) posInfo.Count;

            // 解压缩数据块
            int startPos = 0;
            int endPos = 0;
            int notNeedDecomp = Util.GetOffset(byData, 0x34, 0x37) + 0x800;
            for (int i = 0; i < posInfo.Count; i++)
            {
                // 计算压缩数据开始、结束位置
                startPos = posInfo[i] + 0x20;
                endPos = this.GetDecompEndPos(posInfo, i, byData);

                // 开始解压缩
                List<byte> byList = this.RleDeCompress(byData, startPos, endPos, notNeedDecomp, decDataList);

                // 复制解压缩的数据
                decDataList.AddRange(byList);
            }

            return decDataList.ToArray();
        }

        /// <summary>
        /// 解压缩数据
        /// </summary>
        /// <param name="byData"></param>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <param name="notNeedDecomp">从当前位置开始是Padding数据，不需要解压缩，前一个位置的数据肯定是0x80</param>
        /// <returns></returns>
        private List<byte> RleDeCompress(byte[] byData, int startPos, int endPos, int notNeedDecomp, List<byte> decDataList)
        {
            List<byte> byList = new List<byte>();

            // 重新计算结束的位置，因为有不需要解压缩的数据
            if (startPos < notNeedDecomp && notNeedDecomp < endPos)
            {
                int newEndPos = notNeedDecomp;
                if (byData[newEndPos - 1] == 0x80)
                {
                    newEndPos -= 1;
                }
                byList.AddRange(this.RleDeCompress(byData, startPos, newEndPos));

                // 继续判断，后面是否还存在需要解压缩的数据
                if (endPos - notNeedDecomp > 0x1000)
                {
                    int temp = notNeedDecomp & 0xFFF;
                    int len = 0;
                    if (temp > 0x800)
                    {
                        len = 0x1000 - temp;
                    }
                    else
                    {
                        len = 0x800 - temp;
                    }
                    startPos = notNeedDecomp + len;
                    byList.AddRange(this.RleDeCompress(byData, startPos, endPos));
                }
            }
            else
            {
                byList.AddRange(this.RleDeCompress(byData, startPos, endPos));
            }

            return byList;
        }

        /// <summary>
        /// 解压缩数据
        /// </summary>
        /// <param name="byData"></param>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <returns></returns>
        private List<byte> RleDeCompress(byte[] byData, int startPos, int endPos)
        {
            List<byte> byList = new List<byte>();
            int len = 0;

            for (int j = startPos; j < endPos; j++)
            {
                if (byData[j] == 0 || byData[j] == 0x80)
                {
                    byList.Add(byData[j]);
                    continue;
                }

                if ((byData[j] & 0x80) == 0x80)
                {
                    // 最高位为1，未压缩数据
                    len = byData[j] & 0x7F;
                    while (len-- > 0)
                    {
                        j++;
                        byList.Add(byData[j]);

                        if (j >= endPos - 1)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    // 最高位为0，压缩数据
                    len = byData[j] & 0x7F;
                    j++;
                    while (len-- > 0)
                    {
                        byList.Add(byData[j]);
                    }
                }
            }

            return byList;
        }

        /// <summary>
        /// 写压缩的地址信息
        /// </summary>
        /// <param name="compData"></param>
        private void WriteCompAddrInfo(List<byte> compData, int padding)
        {
            // 写入0x18--0x1b
            int oldOther = (compData[0x18] << 24) | (compData[0x19] << 16) | (compData[0x1a] << 8) | compData[0x1b];
            if (oldOther > 0)
            {
                int total = compData.Count + padding;
                int needPaddingPos = ((compData[0x34] << 24) | (compData[0x35] << 16) | (compData[0x36] << 8) | compData[0x37]) + 0x800;
                int temp = needPaddingPos & 0xFFF;
                int len = 0;
                if (temp > 0x800)
                {
                    len = 0x1000 - temp;
                }
                else
                {
                    len = 0x800 - temp;
                }
                int other = total - (needPaddingPos + len);
                compData[0x18] = (byte)((other >> 24) & 0xFF);
                compData[0x19] = (byte)((other >> 16) & 0xFF);
                compData[0x1a] = (byte)((other >> 8) & 0xFF);
                compData[0x1b] = (byte)(other & 0xFF);
            }
        }

        /// <summary>
        /// 添加Padding字节
        /// 如果是中间的（isLastBlock==false），添加到0xXXXX800，字符：0x80
        /// 如果是结尾的（isLastBlock==true），添加到0xXXX1000或0xXXXX800，字符：0
        ///     然后在追加0x800个字节（Padding字节标识是：-DUMMY DATA 32BYTE for GC ALIGN-）
        /// </summary>
        /// <param name="compData"></param>
        private void PaddZero(List<byte> compData, bool isLastBlock, int endDummyDataCount)
        {
            // 先补一个0x80或0
            if (isLastBlock && endDummyDataCount > 0)
            {
                // 最后的Padding，如果标识文字列个数不为0，补一个0
                int temp = compData.Count & 0xFFF;
                if (!(temp == 0x800 || temp == 0))
                {
                    compData.Add(0);
                }
            }
            else
            {
                if ((compData.Count & 0xFFF) == 0x800)
                {
                    if (isLastBlock && endDummyDataCount == 0)
                    {
                        // 写入0x18--0x1b
                        //this.WriteCompAddrInfo(compData, 0);
                    }
                    return;
                }
                // 中间的压缩数据块Padding，第一个补0x80；
                // 或者最后的Padding，并且标识文字列个数为0，补一个0x80
                compData.Add(0x80);
            }

            // 开始Padding
            if (isLastBlock)
            {
                // 写入0x18--0x1b
                //this.WriteCompAddrInfo(compData, -1);

                // 最后的Padding 0x0；
                this.PaddZero(compData, 0);

                // 如果是结尾Padding，追加0x800个标识字节
                if (endDummyDataCount > 0)
                {
                    int len = endDummyDataCount;
                    while (len-- > 0)
                    {
                        compData.AddRange(Encoding.UTF8.GetBytes("-DUMMY DATA 32BYTE for GC ALIGN-"));
                    }

                    this.PaddZero(compData, 0);
                }
            }
            else
            {
                // 中间的压缩数据块Padding 0x80；
                this.PaddZero(compData, 0x80);
            }
        }

        /// <summary>
        /// 添加Padding字节
        /// Padding到0xXXXX800
        /// </summary>
        /// <param name="compData"></param>
        private void PaddZero(List<byte> compData, byte byPadding)
        {
            // 先取得需要补的字节的个数
            int temp = compData.Count & 0xFFF;
            int len = 0;
            if (temp > 0x800)
            {
                len = 0x1000 - temp;
            }
            else
            {
                len = 0x800 - temp;
            }

            // 开始Padding
            while (len-- > 0)
            {
                compData.Add(byPadding);
            }
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="rleFile"></param>
        /// <returns></returns>
        private byte[] RleCompress(string rleFile)
        {
            // 将文件中的数据，读取到byData中
            byte[] byData = File.ReadAllBytes(rleFile);

            // 取得压缩数据个数
            int comBlock = byData[BLOCK_NUM_POS];
            byData[BLOCK_NUM_POS] = 0;
            List<int> posInfo = this.GetCompPosInfo(comBlock);
            if (posInfo.Count == 0)
            {
                return null;
            }

            // 复制头部数据
            List<byte> compData = new List<byte>();
            byte[] byHeader = new byte[posInfo[0]];
            Array.Copy(byData, 0, byHeader, 0, byHeader.Length);
            compData.AddRange(byHeader);

            // 压缩数据块
            int startPos = 0x800;
            int maxCompLen = 0;
            int needPaddingPos = Util.GetOffset(byData, 0x8, 0xB) + 0x800;
            int endDummyDataCount = Util.GetOffset(byData, 0x1C, 0x1F);
            this.isBlock2 = false;
            for (int i = 0; i < posInfo.Count; i++)
            {
                // 复制标识数据段
                compData.AddRange(this.GetRunLenMarkRange(byData, i));

                // 计算压缩数据开始、结束位置
                maxCompLen = this.GetMaxLen(posInfo, i, byData);

                // 开始压缩
                startPos = this.RleCompress(byData, startPos, maxCompLen, needPaddingPos, compData);
                if (startPos == -1)
                {
                    // 添加剩余的原始的压缩数据
                    this.AddOldCompressData(compData, rleFile);
                }

                // 添加Padding字节
                this.PaddZero(compData, i == (posInfo.Count - 1), endDummyDataCount);
            }

            return compData.ToArray();
        }

        /// <summary>
        /// 添加原来的压缩过的数据
        /// </summary>
        /// <param name="compData"></param>
        /// <param name="rleFile"></param>
        private void AddOldCompressData(List<byte> compData, string rleFile)
        {
            string oldFile = rleFile.Replace(DESC_FILE_SUFFIX, string.Empty);
            byte[] byOld = File.ReadAllBytes(oldFile);
            int needPaddingPos = Util.GetOffset(byOld, 0x34, 0x37) + 0x800;
            int oldOther = Util.GetOffset(byOld, 0x18, 0x1b);
            if (oldOther > 0)
            {
                needPaddingPos = this.ResetPaddingPos(needPaddingPos);
                byte[] byTemp = new byte[oldOther];
                Array.Copy(byOld, needPaddingPos, byTemp, 0, byTemp.Length);
                compData.AddRange(byTemp);
            }
        }

        /// <summary>
        /// 设置以0x800为单位的数据
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private int ResetPaddingPos(int pos)
        {
            int temp = pos & 0xFFF;
            int len = 0;
            if (temp > 0x800)
            {
                len = 0x1000 - temp;
            }
            else
            {
                len = 0x800 - temp;
            }

            return pos + len;
        }

        /// <summary>
        /// 复制标识数据段
        /// </summary>
        /// <param name="byData"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private byte[] GetRunLenMarkRange(byte[] byData, int index)
        {
            List<byte> markRange = new List<byte>();
            markRange.AddRange(Encoding.UTF8.GetBytes("runlength comp"));
            markRange.Add(0x2E);
            markRange.Add(0);
            markRange.Add(byData[0x8]);
            markRange.Add(byData[0x9]);
            markRange.Add(byData[0xA]);
            markRange.Add(byData[0xB]);
            for (int i = 0; i < 11; i++)
            {
                markRange.Add(0);
            }
            if (index == 0)
            {
                markRange.Add(1);
            }
            else
            {
                markRange.Add(0);
            }

            return markRange.ToArray();
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="byData"></param>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <param name="needPaddingPos">需要添加Padding的位置</param>
        /// <returns></returns>
        private int RleCompress(byte[] byData, int startPos, int maxCompLen, int needPaddingPos, List<byte> compData)
        {
            List<byte> byList = new List<byte>();
            if (startPos < needPaddingPos)
            {
                // 压缩
                compData.AddRange(this.RleCompress(byData, ref startPos, needPaddingPos, maxCompLen, compData));

                if (startPos == (needPaddingPos - 1))
                {
                    // 需要追加Padding信息(0x80 0x00 0x00 ......)
                    // 为了后面的解压缩标识，
                    // 将不需要解压的数据段位置，写入压缩的数据中（0x34--0x37）
                    compData.Add(0x80);
                    int notNeedCompPos = compData.Count - 0x800;
                    compData[0x34] = (byte)((notNeedCompPos >> 24) & 0xFF);
                    compData[0x35] = (byte)((notNeedCompPos >> 16) & 0xFF);
                    compData[0x36] = (byte)((notNeedCompPos >> 8) & 0xFF);
                    compData[0x37] = (byte)(notNeedCompPos & 0xFF);

                    // 追加Padding信息(0x00 0x00 ......)
                    this.PaddZero(compData, 0);

                    return -1;
                    //if ((byData.Length - needPaddingPos) > 0x1000)
                    //{
                    //    // 后面还有需要压缩的数据
                    //    this.isBlock2 = true;
                    //    startPos = needPaddingPos;
                    //    compData.AddRange(this.RleCompress(byData, ref startPos, byData.Length, maxCompLen, compData));
                    //}
                }
            }
            else
            {
                // 压缩
                compData.AddRange(this.RleCompress(byData, ref startPos, byData.Length, maxCompLen, compData));

                // 如果
                if (needPaddingPos == byData.Length)
                {
                    // 为了后面的解压缩标识，
                    // 将不需要解压的数据段位置，写入压缩的数据中（0x34--0x37）
                    // 因为最后的Padding还要补个0x80，所以总数量还要加1
                    int notNeedCompPos = (compData.Count + 1) - 0x800;
                    compData[0x34] = (byte)((notNeedCompPos >> 24) & 0xFF);
                    compData[0x35] = (byte)((notNeedCompPos >> 16) & 0xFF);
                    compData[0x36] = (byte)((notNeedCompPos >> 8) & 0xFF);
                    compData[0x37] = (byte)(notNeedCompPos & 0xFF);
                }
            }

            return startPos;
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="byData"></param>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <returns></returns>
        private List<byte> RleCompress(byte[] byData, ref int startPos, int endPos, int maxCompLen, List<byte> compData)
        {
            List<byte> byRetList = new List<byte>();
            List<byte> byList = new List<byte>();
            int len = 0;
            int maxPos = endPos - 1;
            int oldPos = 0;

            for (int j = startPos; j < maxPos; )
            {
                len = 0;
                oldPos = j;
                byList.Clear();
                if (byData[j + 1] == byData[j])
                {
                    // 查找相同的长度，压缩
                    while (j < maxPos && byData[j + 1] == byData[j] && len < MAX_COMP_LEN)
                    {
                        len++;
                        j++;
                    }

                    if (j == maxPos)
                    {
                        // 数据已经处理到头了
                        byList.Add((byte)(len + 1));
                        byList.Add(byData[j]);
                    }
                    else if (len == MAX_COMP_LEN)
                    {
                        // 到达最大压缩长度
                        byList.Add(0x7F);
                        byList.Add(byData[j]);
                    }
                    else
                    {
                        // 出现了不同的字符
                        byList.Add((byte)(len + 1));
                        byList.Add(byData[j++]);

                        if (j == maxPos)
                        {
                            // 数据已经处理到头了
                            byList.Add(0x81);
                            byList.Add(byData[j]);
                        }
                    }
                }
                else
                {
                    // 查找不相同的，不压缩
                    while (true)
                    {
                        byList.Add(byData[j]);
                        len++;
                        j++;

                        if (j == maxPos)
                        {
                            // 数据已经处理到头了
                            len++;
                            byList.Add(byData[j]);
                            byList.Insert(byList.Count - len, (byte)(len | 0x80));
                            break;
                        }
                        else if (len == MAX_COMP_LEN)
                        {
                            // 到达最大压缩长度
                            byList.Insert(byList.Count - len, 0xFF);
                            break;
                        }
                        //else if ((this.isBlock2 == false && byData[j + 1] == byData[j])
                        //    || (this.isBlock2 && byData[j + 1] == byData[j] && byData[j + 1] == byData[j + 2]))
                        else if (byData[j + 1] == byData[j])
                        {
                            // 出现了相同的字符
                            byList.Insert(byList.Count - len, (byte)(len | 0x80));
                            break;
                        }
                    }
                }

                // 判断是否超过最大压缩长度
                if ((compData.Count + byRetList.Count + byList.Count) > maxCompLen)
                {
                    startPos = oldPos;
                    return byRetList;
                }
                else
                {
                    byRetList.AddRange(byList);
                    startPos = j;
                }
            }

            return byRetList;
        }

        #endregion
    }
}
