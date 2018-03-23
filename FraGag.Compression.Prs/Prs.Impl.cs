// Copyright (c) 2012, Francis Gagné
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of the <organization> nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.

namespace FraGag.Compression
{
    using System.IO;
    using System.Text;

    public static partial class Prs
    {
        private static void Encode(byte[] source, Stream destination)
        {
            byte bitPos = 0;
            byte controlByte = 0;

            int position = 0;
            int srcLen = source.Length;
            int currentLookBehindPosition, currentLookBehindLength;
            int lookBehindOffset, lookBehindLength;
            int startPos = 0;
            int lenPosTemp = 0;

            MemoryStream data = new MemoryStream();

            while (position < srcLen)
            {
                currentLookBehindLength = 0;
                lookBehindOffset = 0;
                lookBehindLength = 0;

                //for (currentLookBehindPosition = position - 1; (currentLookBehindPosition >= 0) && (currentLookBehindPosition >= position - 0x1FF0) && (lookBehindLength < 256); currentLookBehindPosition--)
                //{
                //    currentLookBehindLength = 1;
                //    if (source[currentLookBehindPosition] == source[position])
                //    {
                //        do
                //        {
                //            currentLookBehindLength++;
                //        } while ((currentLookBehindLength <= 256) &&
                //            (position + currentLookBehindLength <= source.Length) &&
                //            source[currentLookBehindPosition + currentLookBehindLength - 1] == source[position + currentLookBehindLength - 1]);

                //        currentLookBehindLength--;
                //        if (((currentLookBehindLength >= 2 && currentLookBehindPosition - position >= -0x100) || currentLookBehindLength >= 3) && currentLookBehindLength > lookBehindLength)
                //        {
                //            lookBehindOffset = currentLookBehindPosition - position;
                //            lookBehindLength = currentLookBehindLength;
                //        }
                //    }
                //}

                //if (lookBehindLength == 0)
                //{
                //    data.WriteByte(source[position++]);
                //    PutControlBit(1, ref controlByte, ref bitPos, data, destination);
                //}
                //else
                //{
                //    Copy(lookBehindOffset, lookBehindLength, ref controlByte, ref bitPos, data, destination);
                //    position += lookBehindLength;
                //}

                //for (currentLookBehindPosition = position - 1; (currentLookBehindPosition >= 0) && (currentLookBehindPosition >= position - 0x1FF0) && (lookBehindLength < 256); currentLookBehindPosition--)
                //{
                //    currentLookBehindLength = 1;
                //    if (source[currentLookBehindPosition] == source[position])
                //    {
                //        do
                //        {
                //            currentLookBehindLength++;
                //            lenPosTemp = currentLookBehindLength - 1;
                //        } while ((currentLookBehindLength <= 256) &&
                //            (position + currentLookBehindLength <= srcLen) &&
                //            source[currentLookBehindPosition + lenPosTemp] == source[position + lenPosTemp]);

                //        currentLookBehindLength--;
                //        if (currentLookBehindLength > lookBehindLength)
                //        {
                //            lenPosTemp = currentLookBehindPosition - position;
                //            if (currentLookBehindLength >= 3)
                //            {
                //                lookBehindOffset = lenPosTemp;
                //                lookBehindLength = currentLookBehindLength;
                //            }
                //            else if (currentLookBehindLength >= 2 && lenPosTemp >= -0x100)
                //            {
                //                lookBehindOffset = lenPosTemp;
                //                lookBehindLength = currentLookBehindLength;
                //            }
                //        }
                //    }
                //}

                //if (lookBehindLength == 0)
                //{
                //    data.WriteByte(source[position++]);
                //    PutControlBit(1, ref controlByte, ref bitPos, data, destination);
                //}
                //else
                //{
                //    Copy(lookBehindOffset, lookBehindLength, ref controlByte, ref bitPos, data, destination);
                //    position += lookBehindLength;
                //}

                startPos = position - 8191;
                if (startPos < 0)
                {
                    startPos = 0;
                }
                for (currentLookBehindPosition = position - 1; currentLookBehindPosition >= startPos; currentLookBehindPosition--)
                {
                    if (source[currentLookBehindPosition] == source[position])
                    {
                        currentLookBehindLength = 0;

                        do
                        {
                            currentLookBehindLength++;

                        } while ((currentLookBehindLength < 256) &&
                            (position + currentLookBehindLength < srcLen) &&
                            source[currentLookBehindPosition + currentLookBehindLength] == source[position + currentLookBehindLength]);

                        if (currentLookBehindLength > lookBehindLength)
                        {
                            lookBehindOffset = currentLookBehindPosition - position;
                            lookBehindLength = currentLookBehindLength;

                            if (currentLookBehindLength >= 255)
                            {
                                break;
                            }
                        }
                    }
                }

                if (lookBehindLength >= 3 || (lookBehindLength >= 2 && lookBehindOffset >= -0x100))
                {
                    Copy(lookBehindOffset, lookBehindLength, ref controlByte, ref bitPos, data, destination);
                    position += lookBehindLength;
                }
                else
                {
                    data.WriteByte(source[position++]);
                    PutControlBit(1, ref controlByte, ref bitPos, data, destination);
                }
            }

            PutControlBit(0, ref controlByte, ref bitPos, data, destination);
            PutControlBit(1, ref controlByte, ref bitPos, data, destination);
            if (bitPos != 0)
            {
                //controlByte = (byte)((controlByte << bitPos) >> 8);
                Flush(ref controlByte, ref bitPos, data, destination);
            }

            destination.WriteByte(0);
            destination.WriteByte(0);
        }

        private static void Copy(int offset, int size, ref byte controlByte, ref byte bitPos, MemoryStream data, Stream destination)
        {
            if ((offset >= -0x100) && (size <= 5))
            {
                size -= 2;
                PutControlBit(0, ref controlByte, ref bitPos, data, destination);
                PutControlBit(0, ref controlByte, ref bitPos, data, destination);
                PutControlBit((size >> 1) & 1, ref controlByte, ref bitPos, data, destination);
                data.WriteByte((byte)(offset & 0xFF));
                PutControlBit(size & 1, ref controlByte, ref bitPos, data, destination);
            }
            else
            {
                if (size <= 9)
                {
                    PutControlBit(0, ref controlByte, ref bitPos, data, destination);
                    data.WriteByte((byte)(((offset << 3) & 0xF8) | ((size - 2) & 0x07)));
                    data.WriteByte((byte)((offset >> 5) & 0xFF));
                    PutControlBit(1, ref controlByte, ref bitPos, data, destination);
                }
                else
                {
                    PutControlBit(0, ref controlByte, ref bitPos, data, destination);
                    data.WriteByte((byte)((offset << 3) & 0xF8));
                    data.WriteByte((byte)((offset >> 5) & 0xFF));
                    data.WriteByte((byte)(size - 1));
                    PutControlBit(1, ref controlByte, ref bitPos, data, destination);
                }
            }
        }

        private static void PutControlBit(int bit, ref byte controlByte, ref byte bitPos, MemoryStream data, Stream destination)
        {
            //controlByte >>= 1;
            //controlByte |= (byte)(bit << 7);
            controlByte |= (byte)(bit << bitPos);
            bitPos++;
            if (bitPos >= 8)
            {
                Flush(ref controlByte, ref bitPos, data, destination);
            }
        }

        private static void Flush(ref byte controlByte, ref byte bitPos, MemoryStream data, Stream destination)
        {
            destination.WriteByte(controlByte);
            controlByte = 0;
            bitPos = 0;

            //byte[] bytes = data.ToArray();
            //destination.Write(bytes, 0, bytes.Length);
            //tmpBytes = data.ToArray();
            //destination.Write(tmpBytes, 0, tmpBytes.Length);
            destination.Write(data.ToArray(), 0, (int)data.Length);

            data.SetLength(0);
        }

        private static void Decode(Stream source, Stream destination)
        {
            int bitPos = 9;
            byte currentByte;
            int lookBehindOffset, lookBehindLength;
            StringBuilder sb = new StringBuilder();


            currentByte = ReadByte(source);
            for (; ; )
            {
                //if (sb.Length > 2048)
                //{
                //    File.WriteAllText(@"E:\游戏汉化\NgcBioCv\BioCvNgcCn\RdxDecTst.txt", sb.ToString());
                //    break;
                //}

                if (GetControlBit(ref bitPos, ref currentByte, source) != 0)
                {
                    // Direct byte
                    byte curByte = ReadByte(source);
                    destination.WriteByte(curByte);

                    //sb.Append("未压缩：" + curByte.ToString("x") + "\r\n");

                    continue;
                }

                if (GetControlBit(ref bitPos, ref currentByte, source) != 0)
                {
                    lookBehindOffset = ReadByte(source);
                    lookBehindOffset |= ReadByte(source) << 8;
                    if (lookBehindOffset == 0)
                    {
                        // End of the compressed data
                        break;
                    }

                    lookBehindLength = lookBehindOffset & 7;
                    lookBehindOffset = (lookBehindOffset >> 3) | -0x2000; // 0xFFFFE000 (-8191 <= lookBehindOffset <= -1)
                    if (lookBehindLength == 0)
                    {
                        lookBehindLength = ReadByte(source) + 1; // 2 <= lookBehindLength <= 256(1 <= lookBehindLength <= 255)

                        //sb.Append("已压缩1：" + lookBehindOffset.ToString("x") + " " + lookBehindLength.ToString("x") + "\r\n");
                    }
                    else
                    {
                        lookBehindLength += 2; // 3 <= lookBehindLength <= 9(1 <= lookBehindLength <= 7)
                        //sb.Append("已压缩2：" + lookBehindOffset.ToString("x") + " " + lookBehindLength.ToString("x") + "\r\n");
                    }
                }
                else
                {
                    lookBehindLength = 0;
                    lookBehindLength = (lookBehindLength << 1) | GetControlBit(ref bitPos, ref currentByte, source);
                    lookBehindLength = (lookBehindLength << 1) | GetControlBit(ref bitPos, ref currentByte, source);
                    lookBehindOffset = ReadByte(source) | -0x100; // 0xFFFFFF00 (-255 <= lookBehindOffset <= -1)
                    lookBehindLength += 2; // 3 <= lookBehindLength <= 5(1 <= lookBehindLength <= 3)

                    //sb.Append("已压缩3：" + lookBehindOffset.ToString("x") + " " + lookBehindLength.ToString("x") + "\r\n");
                }

                for (int i = 0; i < lookBehindLength; i++)
                {
                    long writePosition = destination.Position;
                    destination.Seek(writePosition + lookBehindOffset, SeekOrigin.Begin);
                    byte b = ReadByte(destination);
                    destination.Seek(writePosition, SeekOrigin.Begin);
                    destination.WriteByte(b);
                }
            }
        }

        private static int GetControlBit(ref int bitPos, ref byte currentByte, Stream source)
        {
            bitPos--;
            if (bitPos == 0)
            {
                currentByte = ReadByte(source);
                bitPos = 8;
            }

            int flag = currentByte & 1;
            currentByte >>= 1;
            return flag;
        }

        private static byte ReadByte(Stream stream)
        {
            int value = stream.ReadByte();
            if (value == -1)
            {
                throw new EndOfStreamException();
            }

            return (byte)value;
        }
    }
}
