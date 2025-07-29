using System;
using System.Collections.Generic;

namespace Hanhua.CompressTools
{
    public class AdaptiveArithmeticEncoder
    {
        const uint TOP_VALUE = 0xFFFFFFFF;
        const int SYMBOLS = 257; // 256 bytes + EOF

        private uint low = 0;
        private uint high = TOP_VALUE;
        private uint underflow = 0;
        private List<byte> output = new List<byte>();

        private uint[] freq;
        private uint[] cumFreq;
        private uint total;

        public AdaptiveArithmeticEncoder()
        {
            freq = new uint[SYMBOLS];
            cumFreq = new uint[SYMBOLS + 1];
            for (int i = 0; i < SYMBOLS; i++) freq[i] = 1;
            RebuildCumFreq();
        }

        private void RebuildCumFreq()
        {
            cumFreq[0] = 0;
            for (int i = 0; i < SYMBOLS; i++)
            {
                cumFreq[i + 1] = cumFreq[i] + freq[i];
            }
            total = cumFreq[SYMBOLS];
        }

        private void UpdateFreq(int symbol)
        {
            freq[symbol]++;
            if (total >= 0xFFFF)
            {
                for (int i = 0; i < SYMBOLS; i++)
                    freq[i] = (freq[i] + 1) / 2;
            }
            RebuildCumFreq();
        }

        public void Encode(byte[] input)
        {
            foreach (byte b in input)
                EncodeSymbol(b);
            EncodeSymbol(256); // EOF
            for (int i = 0; i < 4; i++) Emit((byte)(low >> 24));
        }

        private void EncodeSymbol(int symbol)
        {
            uint range = high - low + 1;
            uint symLow = cumFreq[symbol];
            uint symHigh = cumFreq[symbol + 1];

            high = low + (range * symHigh) / total - 1;
            low = low + (range * symLow) / total;

            while (true)
            {
                if ((high & 0xFF000000) == (low & 0xFF000000))
                {
                    Emit((byte)(high >> 24));
                    low <<= 8;
                    high = (high << 8) | 0xFF;
                }
                else if ((low & 0x80000000) == 0x40000000 && (high & 0x80000000) == 0x7FFFFFFF)
                {
                    underflow++;
                    low = (low & 0x3FFFFFFF) << 1;
                    high = ((high & 0x3FFFFFFF) << 1) | 1;
                }
                else break;
            }

            UpdateFreq(symbol);
        }

        private void Emit(byte b)
        {
            output.Add(b);
            while (underflow > 0)
            {
                output.Add((byte)~b);
                underflow--;
            }
        }

        public byte[] GetEncoded() 
        { 
            return output.ToArray(); 
        }
    }
}
