using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lz77Test
{
    /// <summary>
    /// Lz77压缩、解压缩C#版
    /// 使用在自己的堆中分配索引节点，不滑动窗口
    /// 每次最多压缩 65536 字节数据
    /// 的优化版本
    /// </summary>
    public class Lz77Sample
    {
        /// <summary>
        /// 滑动窗口的字节大小
        /// </summary>
        private const int MAX_WINDOW_SIZE = 65536;

        /// <summary>
        /// 当前窗口
        /// </summary>
        private byte[] pWnd;

        /// <summary>
        /// 窗口大小最大为 64k ，并且不做滑动
        /// 每次最多只压缩 64k 数据，这样可以方便从文件中间开始解压
        /// 当前窗口的长度
        /// </summary>
        private int nWndSize;

        /// <summary>
        /// 当前分配位置
        /// </summary>
        private int HeapPos;

        /// <summary>
        /// 当前输出位置(字节偏移)
        /// </summary>
        private int CurByte;

        /// <summary>
        /// 当前输出位置(位偏移)
        /// </summary>
        private int CurBit;

        /// <summary>
        /// 256 * 256 指向SortHeap中下标的指针
        /// </summary>
        private int[] SortTable = new int[65536];

        /// <summary>
        /// 因为窗口不滑动，没有删除节点的操作，所以节点可以在SortHeap 中连续分配
        /// </summary>
        private StidxNode[] SortHeap;

        /// <summary>
        /// 压缩一段字节流
        /// </summary>
        /// <param name="src">源数据区</param>
        /// <param name="srclen">源数据区字节长度, srclen 小于 65536</param>
        /// <param name="dest">压缩数据区，调用前分配srclen字节内存</param>
        /// <returns>
        /// 返回值 > 0 压缩数据长度
        /// 返回值 = 0 数据无法压缩
        /// 返回值 小于 0 压缩中异常错误
        /// </returns>
        public int Compress(byte[] src, int srclen, byte[] dest)
        {
            return 0;
        }

        /// <summary>
        /// 解压缩一段字节流
        /// </summary>
        /// <param name="src">接收原始数据的内存区</param>
        /// <param name="srclen">源数据区字节长度, srclen 小于 65536</param>
        /// <param name="dest">压缩数据区</param>
        /// <returns>成功与否</returns>
        public bool Decompress(byte[] src, int srclen, byte[] dest)
        {
            int i;
            CurByte = 0; 
            CurBit = 0;

            // 初始化窗口
            pWnd = src;		
            nWndSize = 0;
            byte[] newDest;
            byte[] newSrc;

            if (srclen > 65536)
            {
                return false;
            }

            for (i = 0; i < srclen; i++)
            {
                byte b = GetBit(dest[CurByte], CurBit);
                this.MovePos(ref CurByte, ref CurBit, 1);

                if (b == 0) 
                {
                    // 单个字符
                    newSrc = new byte[src.Length - i];
                    newDest = new byte[dest.Length - CurByte];
                    Array.Copy(dest, CurByte, newDest, 0, newDest.Length);
                    Array.Copy(src, i, newSrc, 0, newSrc.Length);

                    this.CopyBits(newSrc, 0, newDest, CurBit, 8);

                    Array.Copy(newSrc, 0, src, i, newSrc.Length);

                    this.MovePos(ref CurByte, ref CurBit, 8);
                    nWndSize++;
                }
                else		
                {
                    // 窗口内的术语
                    int q = -1;
                    while (b != 0)
                    {
                        q++;
                        b = this.GetBit(dest[CurByte], CurBit);
                        this.MovePos(ref CurByte, ref CurBit, 1);
                    }

                    int len, off;
                    byte[] dw = new byte[4];
                    if (q > 0)
                    {
                        newSrc = new byte[4 - ((32 - q) / 8)];
                        newDest = new byte[dest.Length - CurByte];
                        Array.Copy(dest, CurByte, newDest, 0, newDest.Length);
                        Array.Copy(dw, (32 - q) / 8, newSrc, 0, newSrc.Length);

                        this.CopyBits(newSrc, (32 - q) % 8, newDest, CurBit, q);

                        Array.Copy(newSrc, 0, dw, (32 - q) / 8, newSrc.Length);

                        this.MovePos(ref CurByte, ref CurBit, q);
                        this.InvertDWord(dw);
                        len = 1;
                        len <<= q;
                        len += this.GetOffset(dw, 0, 3);
                        len += 1;
                    }
                    else
                    {
                        len = 2;
                    }

                    // 在窗口不满64k大小时，不需要16位存储偏移
                    dw = new byte[4];
                    int bits = this.UpperLog2(nWndSize);
                    newSrc = new byte[4 - ((32 - bits) / 8)];
                    newDest = new byte[dest.Length - CurByte];

                    Array.Copy(dw, (32 - bits) / 8, newSrc, 0, newSrc.Length);
                    Array.Copy(dest, CurByte, newDest, 0, newDest.Length);

                    this.CopyBits(newSrc, (32 - bits) % 8, newDest, CurBit, bits);

                    Array.Copy(newSrc, 0, dw, (32 - bits) / 8, newSrc.Length);

                    this.MovePos(ref CurByte, ref CurBit, bits);
                    this.InvertDWord(dw);
                    off = this.GetOffset(dw, 0, 3);
                    
                    // 输出术语
                    for (int j = 0; j < len; j++)
                    {
                        //_ASSERT(i + j < srclen);
                        //_ASSERT(off + j < MAX_WINDOW_SIZE);
                        if ((i + j) >= (src.Length - 1)
                            || (off + j) >= (src.Length - 1))
                        {
                            int cc = 9;
                        }
                        src[i + j] = pWnd[off + j];
                    }
                    nWndSize += len;
                    i += len - 1;
                }

                // 滑动窗口
                ////if (nWndSize > MAX_WINDOW_SIZE)
                ////{
                ////    pWnd += nWndSize - MAX_WINDOW_SIZE;
                ////    nWndSize = MAX_WINDOW_SIZE;
                ////}
            }

            return true;
        }

        #region " 私有方法 "

        /// <summary>
        /// 根据开始、结束位置取得字节数组中的offset
        /// </summary>
        /// <param name="byData">字节数组</param>
        /// <param name="startPos">开始字节位置</param>
        /// <param name="endPos">结束字节位置</param>
        /// <returns></returns>
        private int GetOffset(byte[] byData, int startPos, int endPos)
        {
            int intRetValue = 0;
            int intBytePos = endPos - startPos;

            for (int i = startPos; i <= endPos; i++)
            {
                intRetValue += (int)((uint)(byData[i]) << (intBytePos * 8));
                intBytePos--;
            }

            return intRetValue;
        }

        /// <summary>
        /// CopyBitsInAByte : 在一个字节范围内复制位流
        /// 此函数由 CopyBits 调用，不做错误检查，即
        /// 假定要复制的位都在一个字节范围内
        /// </summary>
        /// <remarks>起始位的表示约定为从字节的高位至低位（由左至右）
        /// 依次为 0，1，... , 7
        /// 要复制的两块数据区不能有重合
        /// </remarks>
        /// <param name="memDest">目标数据区</param>
        /// <param name="nDestPos">目标数据区第一个字节中的起始位</param>
        /// <param name="memSrc">源数据区</param>
        /// <param name="nSrcPos">源数据区第一个字节的中起始位</param>
        /// <param name="nBits">要复制的位数</param>
        private void CopyBitsInAByte(ref byte memDest, int nDestPos, byte memSrc, int nSrcPos, int nBits)
        {
            byte b1, b2;
            b1 = memSrc;

            // 将不用复制的位清0
            b1 <<= nSrcPos; 
            b1 >>= 8 - nBits;

            // 将源和目的字节对齐
            b1 <<= 8 - nBits - nDestPos;

            // 复制值为1的位
            memDest |= b1;
    
            // 将不用复制的位置1
	        b2 = 0xff; 
            b2 <<= 8 - nDestPos;		

            b1 |= b2;
            b2 = 0xff; 
            b2 >>= nDestPos + nBits;
            b1 |= b2;

            // 复制值为0的位
            memDest &= b1;		
        }

        /// <summary>
        /// 复制内存中的位流
        /// </summary>
        /// <remarks>起始位的表示约定为从字节的高位至低位（由左至右）
        /// 依次为 0，1，... , 7
        /// 要复制的两块数据区不能有重合
        /// </remarks>
        /// <param name="memDest">目标数据区</param>
        /// <param name="nDestPos">目标数据区第一个字节中的起始位</param>
        /// <param name="memSrc">源数据区</param>
        /// <param name="nSrcPos">源数据区第一个字节的中起始位</param>
        /// <param name="nBits">要复制的位数</param>
        private void CopyBits(byte[] memDest, int nDestPos, byte[] memSrc, int nSrcPos, int nBits)
        {
            int iByteDest = 0, iBitDest;
            int iByteSrc = 0, iBitSrc = nSrcPos;

            int nBitsToFill, nBitsCanFill;

            byte byRefTemp;

            while (nBits > 0)
            {
                // 计算要在目标区当前字节填充的位数
                nBitsToFill = Math.Min(nBits, iByteDest > 0 ? 8 : 8 - nDestPos);

                // 目标区当前字节要填充的起始位
                iBitDest = iByteDest > 0 ? 0 : nDestPos;
                
                // 计算可以一次从源数据区中复制的位数
                nBitsCanFill = Math.Min(nBitsToFill, 8 - iBitSrc);
                
                // 字节内复制
                byRefTemp = memDest[iByteDest];
                if (iByteSrc >= memSrc.Length - 1)
                {
                    int aa = 0;
                }
                this.CopyBitsInAByte(ref byRefTemp, iBitDest,
                    memSrc[iByteSrc], iBitSrc, nBitsCanFill);
                memDest[iByteDest] = byRefTemp;
                
                // 如果还没有复制完 nBitsToFill 个
                if (nBitsToFill > nBitsCanFill)
                {
                    iByteSrc++; 
                    iBitSrc = 0; 
                    iBitDest += nBitsCanFill;
                    byRefTemp = memDest[iByteDest];
                    if (iByteSrc >= memSrc.Length - 1)
                    {
                        int bb = 0;
                    }
                    this.CopyBitsInAByte(ref byRefTemp, iBitDest,
                            memSrc[iByteSrc], iBitSrc,
                            nBitsToFill - nBitsCanFill);
                    memDest[iByteDest] = byRefTemp;
                    iBitSrc += nBitsToFill - nBitsCanFill;
                }
                else
                {
                    iBitSrc += nBitsCanFill;
                    if (iBitSrc >= 8)
                    {
                        iByteSrc++; iBitSrc = 0;
                    }
                }

                // 已经填充了nBitsToFill位
                nBits -= nBitsToFill;	
                iByteDest++;
            }	
        }

        /// <summary>
        /// 将DWORD值从高位字节到低位字节排列
        /// </summary>
        /// <param name="pDW">DWORD值</param>
        private void InvertDWord(byte[] pDW)
        {
            byte temp = pDW[0];
            pDW[0] = pDW[3];
            pDW[3] = temp;

            temp = pDW[1];
            pDW[2] = pDW[1];
            pDW[1] = temp;
        }

        /// <summary>
        /// 设置byte的第iBit位为aBit
        /// iBit顺序为高位起从0记数（左起）
        /// </summary>
        /// <param name="byteData"></param>
        /// <param name="iBit"></param>
        /// <param name="aBit"></param>
        private void SetBit(ref byte byteData, int iBit, byte aBit)
        {
            if (aBit > 0)
            {
		        byteData |= (byte)(1 << (7 - iBit));
            }
	        else
            {
                byteData &= (byte)~(1 << (7 - iBit));
            }
        }

        /// <summary>
        /// 得到字节byte第pos位的值
        /// pos顺序为高位起从0记数（左起）
        /// </summary>
        /// <param name="byteData"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private byte GetBit(byte byteData, int pos)
        {
            return (byte)((byteData >> (7 - pos)) & 1);
        }

        /// <summary>
        /// 将位指针*piByte(字节偏移)
        /// </summary>
        /// <param name="piByte"></param>
        /// <param name="piBit">字节内位偏移</param>
        /// <param name="num">后移num位</param>
        private void MovePos(ref int piByte, ref int piBit, int num)
        {
            num += piBit;
            piByte += num / 8;
            piBit = num % 8;
        }

        /// <summary>
        /// 取log2(n)的upper_bound
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private int UpperLog2(int n)
        {
            int i = 0;
            if (n > 0)
            {
                int m = 1;
                while (true)
                {
                    if (m >= n)
                    {
                        return i;
                    }
                    m <<= 1;
                    i++;
                }
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 取log2(n)的lower_bound
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private int LowerLog2(int n)
        {
            int i = 0;
            if (n > 0)
            {
                int m = 1;
                while (true)
                {
                    if (m == n)
                    {
                        return i;
                    }
                    if (m > n)
                    {
                        return i - 1;
                    }
                    m <<= 1;
                    i++;
                }
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 输出压缩码
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="code">要输出的数</param>
        /// <param name="bits">要输出的位数(对isGamma=TRUE时无效)</param>
        /// <param name="isGamma">是否输出为γ编码</param>
        private void OutCode(byte[] dest, int code, int bits, bool isGamma)
        {
        }

        /// <summary>
        /// 在滑动窗口中查找术语
        /// </summary>
        /// <param name="src"></param>
        /// <param name="srclen"></param>
        /// <param name="nSeekStart"> 从何处开始匹配</param>
        /// <param name="offset">用于接收结果，表示在滑动窗口内的偏移和长度</param>
        /// <param name="len">用于接收结果，表示在滑动窗口内的偏移和长度</param>
        /// <returns>是否查到长度为3或3以上的匹配字节串</returns>
        private bool SeekPhase(byte[] src, int srclen, int nSeekStart, int[] offset, int[] len)
        {
            return true;
        }

        /// <summary>
        /// 得到已经匹配了3个字节的窗口位置offset共能匹配多少个字节
        /// </summary>
        /// <param name="src"></param>
        /// <param name="srclen"></param>
        /// <param name="nSeekStart"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private int GetSameLen(byte[] src, int srclen, int nSeekStart, int offset)
        {
            return 0;
        }

        /// <summary>
        /// 将窗口向右滑动n个字节
        /// </summary>
        /// <param name="n"></param>
        private void ScrollWindow(int n)
        {
        }

        /// <summary>
        /// 向索引中添加一个2字节串
        /// </summary>
        /// <param name="off"></param>
        private void InsertIndexItem(int off)
        {
        }

        /// <summary>
        /// 初始化索引表，释放上次压缩用的空间
        /// </summary>
        private void InitSortTable()
        {
        }

        #endregion
    }

    /// <summary>
    /// 对滑动窗口中每一个2字节串排序
    /// 排序是为了进行快速术语匹配
    /// 排序的方法是用一个64k大小的指针数组
    /// 数组下标依次对应每一个2字节串：(00 00) (00 01) ... (01 00) (01 01) ...
    /// 每一个指针指向一个链表，链表中的节点为该2字节串的每一个出现位置
    /// </summary>
    public class StidxNode
    {
        /// <summary>
        /// 在src中的偏移
        /// </summary>
        public int off { set; get; }

        /// <summary>
        /// 用于对应的2字节串为重复字节的节点
        /// 指从 off 到 off2 都对应了该2字节串
        /// </summary>
        public int off2 { set; get; }

        /// <summary>
        /// 在SortHeap中的指针
        /// </summary>
        public int next { set; get; }
    }
}
