using System;
using System.Collections.Generic;
using System.IO;

namespace Hanhua.CompressTools
{
    public class SK_ASCDecompressor
    {
        private byte[] input;
        private int inputPos;
        private uint value;
        private uint range;

        private ushort[] freqTable = new ushort[258];
        private ushort[] decodeTable = new ushort[512];

        private List<byte> output = new List<byte>();

        public SK_ASCDecompressor(byte[] compressed)
        {
            input = compressed;
            inputPos = 0;
        }

        public byte[] Decompress(int expectedOutputSize)
        {
            output.Clear();
            value = ReadByte();
            value = (value << 8) | ReadByte();
            value = (value << 8) | ReadByte();
            value = (value << 8) | ReadByte();
            range = 0xFFFFFFFF;

            InitializeFreqTable();

            while (output.Count < expectedOutputSize)
            {
                ushort symbol = DecodeSymbol();
                if (symbol == 256)
                    break;

                output.Add((byte)symbol);
                UpdateFreqTable(symbol);
            }

            return output.ToArray();
        }

        private void InitializeFreqTable()
        {
            for (int i = 0; i < 258; i++)
                freqTable[i] = 1;
        }

        private void UpdateFreqTable(ushort symbol)
        {
            if (freqTable[symbol] < 0x3FFF)
                freqTable[symbol]++;
        }

        private ushort DecodeSymbol()
        {
            uint total = 0;
            for (int i = 0; i < 258; i++)
                total += freqTable[i];

            uint scaledValue = (uint)(((ulong)(value - 0) + 1) * total - 1) / range;
            ushort symbol = 0;
            uint sum = 0;
            for (symbol = 0; symbol < 258; symbol++)
            {
                if (sum + freqTable[symbol] > scaledValue)
                    break;
                sum += freqTable[symbol];
            }

            uint lowCount = sum;
            uint highCount = sum + freqTable[symbol];

            range /= total;
            value -= range * lowCount;
            range *= (highCount - lowCount);

            while ((range & 0xFF000000) == 0)
            {
                value = (value << 8) | ReadByte();
                range <<= 8;
            }

            return symbol;
        }

        private byte ReadByte()
        {
            if (inputPos >= input.Length)
                return 0;
            return input[inputPos++];
        }
    }
}
