using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Hanhua.CompressTools
{
    public class SKASCCompressor
    {
        public static byte[] Compress(byte[] inputData)
        {
            var encoder = new AdaptiveArithmeticEncoder();
            encoder.Encode(inputData);
            byte[] compressed = encoder.GetEncoded();

            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                // Header (64 bytes)
                bw.Write(Encoding.ASCII.GetBytes("SK_ASC")); // 6 bytes
                bw.Write((ushort)0); // padding
                bw.Write((uint)0);   // unknown
                bw.Write(ToBE((uint)compressed.Length)); // compressed size
                bw.Write(ToBE((uint)inputData.Length));  // original size
                bw.Write(new byte[0x30]); // padding

                // Compressed data
                bw.Write(compressed);
                return ms.ToArray();
            }
        }

        private static uint ToBE(uint val)
        {
            return BitConverter.IsLittleEndian
                ? (uint)((val >> 24) | ((val >> 8) & 0xFF00) | ((val << 8) & 0xFF0000) | (val << 24))
                : val;
        }
    }
}
