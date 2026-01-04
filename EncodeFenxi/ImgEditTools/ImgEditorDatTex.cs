using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Hanhua.Common;
using System.IO;

namespace Hanhua.ImgEditTools
{
    public enum GEntryType
    {
        GET_DATA,       // generic literal data
        GET_TEXTURE,    // stripped TIM pixel
        GET_PALETTE,    // stripped TIM clut
        GET_SNDH,       // VAG header 'Gian'
        GET_SNDB,       // VAG body
        GET_SNDE,       // configuration for sound samples?
        GET_UNK,
        GET_LZSS0,
        GET_LZSS1       // compressed texture
    }

    // Generic entry (32 bytes)
    public class DC2_ENTRY_GENERIC
    {
        public uint type;
        public uint size;
        public uint[] reserve = new uint[6];
    }

    // GFX entry
    public class DC2_ENTRY_GFX
    {
        public uint type;
        public uint size;
        public ushort x, y;
        public ushort w, h;
    }

    /// <summary>
    /// Ps Dat中图片编辑器
    /// </summary>
    public class ImgEditorDatTex : ImgEditorBase
    {
        #region " 私有变量 "

        /// <summary>
        /// 图片类型 2：16BPP，3：24BPP，8：4BPP，9：8BPP
        /// </summary>
        private static int timType;

        /// <summary>
        /// 调色板数据
        /// </summary>
        private List<Color[]> paletteColor = new List<Color[]>();

        /// <summary>
        /// 固定的4bp调色板数据
        /// </summary>
        private List<Color> palGreyscale4bp = new List<Color>();

        /// <summary>
        /// 固定的8bp调色板数据
        /// </summary>
        private List<Color> palGreyscale8bp = new List<Color>();

        /// <summary>
        /// 图片数据
        /// </summary>
        private List<byte[]> byAllImg = new List<byte[]>();

        /// <summary>
        /// 图片的宽高信息
        /// </summary>
        private List<DatTexInfo> texInfo = new List<DatTexInfo>();

        /// <summary>
        /// 图片和调色板的映射
        /// </summary>
        private Dictionary<int, int> texPalMap = new Dictionary<int, int>();

        #endregion

        /// <summary>
        /// 构造方法
        /// </summary>
        public ImgEditorDatTex(string file)
            : base(file)
        {
            this.CreatePalGreyscale();
        }

        #region " 重写父类的虚方法 "

        /// <summary>
        /// 查找当前类型的图片
        /// </summary>
        /// <param name="byData">当前打开文件的字节数据</param>
        /// <param name="file">当前文件</param>
        /// <param name="imgInfos">查找到的图片的信息</param>
        /// <returns>是否查找成功</returns>
        public override List<byte[]> SearchImg(byte[] byData, string file, List<string> imgInfos)
        {
            return this.OpenDat(byData, file, imgInfos);
        }

        /// <summary>
        /// 从图片数据中获取图片
        /// </summary>
        /// <param name="byData"></param>
        /// <returns></returns>
        public override Image[] ImageDecode(byte[] byData, string fileInfo)
        {
            this.OpenDat(File.ReadAllBytes(base.editingFile), string.Empty, null);

            List<Image> dcImg = new List<Image>();
            string[] fileInfos = fileInfo.Split(' ');
            int imgIdx = Convert.ToInt32(fileInfos[1]);
            DatTexInfo texItem = texInfo[imgIdx];
            //foreach (DatTexInfo texItem in texInfo)
            {
                Bitmap img = new Bitmap(texItem.width, texItem.height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                
                Color[] palColor = null;
                if (this.paletteIndex > 0)
                {
                    palColor = this.paletteColor[this.paletteIndex - 1];
                }
                
                byte[] byImgData = this.byAllImg[imgIdx];
                int byImgIdx = 0;

                if (1 == 1)
                {
                    for (int h = 0; h < texItem.height; h += 1)
                    {
                        for (int w = 0; w < texItem.width; w += 2)
                        {
                            img.SetPixel(w, h, this.palGreyscale4bp[byImgData[byImgIdx] & 0xF]);
                            img.SetPixel(w + 1, h, this.palGreyscale4bp[(byImgData[byImgIdx] >> 4) & 0xF]);

                            byImgIdx++;
                        }
                    }
                }
                else if (0 == 1)
                {
                    for (int h = 0; h < texItem.height; h += 1)
                    {
                        for (int w = 0; w < texItem.width; w += 1)
                        {
                            if (byImgIdx < byImgData.Length)
                            {
                                int colIdx = byImgData[byImgIdx];
                                if (palColor != null && colIdx < palColor.Length)
                                {
                                    img.SetPixel(w, h, palColor[colIdx]);
                                }
                            }

                            byImgIdx++;
                        }
                    }
                }
                

                dcImg.Add(img);
                imgIdx++;
            }

            return dcImg.ToArray();
        }

        /// <summary>
        /// 设置编辑器的Title
        /// </summary>
        /// <param name="newTitle"></param>
        public override string GetEditorTitle(Image img)
        {
            string bppInfo = "";
            switch (timType)
            {
                case 2:
                    bppInfo = "16BPP";
                    break;

                case 3:
                    bppInfo = "24BPP";
                    break;

                case 8:
                    bppInfo = "4BPP";
                    break;

                case 9:
                    bppInfo = "8BPP";
                    break;

                case 10:
                    bppInfo = "XBPP";
                    break;
            }

            return this.editingFile + " " +  bppInfo +  " W：" + (img == null ? 0 : img.Width) + " H：" + (img == null ? 0 : img.Height);
        }

        #endregion

        #region " 共有方法 "

        #endregion

        #region " 私有方法 "

        /// <summary>
        /// 创建固定位数的调色版
        /// </summary>
        private void CreatePalGreyscale()
        {
            this.palGreyscale4bp.Clear();
            this.palGreyscale8bp.Clear();

            //for (int i = 0; i < 16; i++)
            //{
            //    this.palGreyscale4bp.Add(Color.FromArgb(i * 16, i * 16, i * 16));
            //}
            //for (int i = 0; i < 4; i++)
            {
                //this.palGreyscale4bp.Add(Color.FromArgb(i * 16, i * 16, i * 16));

                //this.palGreyscale4bp.Add(Color.FromArgb(0, 0, 0));
                //int pixelColor;
                //int colorR;
                //int colorG;
                //int colorB;
                //pixelColor = 0x0842;
                //colorR = ((byte)(pixelColor & 0x1F)) << 3;
                //colorG = ((byte)((pixelColor >> 5) & 0x1F)) << 3;
                //colorB = ((byte)((pixelColor >> 10) & 0x1F)) << 3;
                //this.palGreyscale4bp.Add(Color.FromArgb(colorR, colorG, colorB));

                //pixelColor = 0x318C;
                //colorR = ((byte)(pixelColor & 0x1F)) << 3;
                //colorG = ((byte)((pixelColor >> 5) & 0x1F)) << 3;
                //colorB = ((byte)((pixelColor >> 10) & 0x1F)) << 3;
                //this.palGreyscale4bp.Add(Color.FromArgb(colorR, colorG, colorB));

                //pixelColor = 0x5AD6;
                //colorR = ((byte)(pixelColor & 0x1F)) << 3;
                //colorG = ((byte)((pixelColor >> 5) & 0x1F)) << 3;
                //colorB = ((byte)((pixelColor >> 10) & 0x1F)) << 3;
                //this.palGreyscale4bp.Add(Color.FromArgb(colorR, colorG, colorB));

                this.palGreyscale4bp.Add(Color.FromArgb(0, 0, 0));
                this.palGreyscale4bp.Add(Color.FromArgb(0, 0, 0));
                this.palGreyscale4bp.Add(Color.FromArgb(0, 0, 0));
                this.palGreyscale4bp.Add(Color.FromArgb(0, 0, 0));

                int pixelColor;
                int colorR;
                int colorG;
                int colorB;
                pixelColor = 0x0842;
                colorR = ((byte)(pixelColor & 0x1F)) << 3;
                colorG = ((byte)((pixelColor >> 5) & 0x1F)) << 3;
                colorB = ((byte)((pixelColor >> 10) & 0x1F)) << 3;
                this.palGreyscale4bp.Add(Color.FromArgb(colorR, colorG, colorB));
                this.palGreyscale4bp.Add(Color.FromArgb(colorR, colorG, colorB));
                this.palGreyscale4bp.Add(Color.FromArgb(colorR, colorG, colorB));
                this.palGreyscale4bp.Add(Color.FromArgb(colorR, colorG, colorB));

                pixelColor = 0x318C;
                colorR = ((byte)(pixelColor & 0x1F)) << 3;
                colorG = ((byte)((pixelColor >> 5) & 0x1F)) << 3;
                colorB = ((byte)((pixelColor >> 10) & 0x1F)) << 3;
                this.palGreyscale4bp.Add(Color.FromArgb(colorR, colorG, colorB));
                this.palGreyscale4bp.Add(Color.FromArgb(colorR, colorG, colorB));
                this.palGreyscale4bp.Add(Color.FromArgb(colorR, colorG, colorB));
                this.palGreyscale4bp.Add(Color.FromArgb(colorR, colorG, colorB));

                pixelColor = 0x5AD6;
                colorR = ((byte)(pixelColor & 0x1F)) << 3;
                colorG = ((byte)((pixelColor >> 5) & 0x1F)) << 3;
                colorB = ((byte)((pixelColor >> 10) & 0x1F)) << 3;
                this.palGreyscale4bp.Add(Color.FromArgb(colorR, colorG, colorB));
                this.palGreyscale4bp.Add(Color.FromArgb(colorR, colorG, colorB));
                this.palGreyscale4bp.Add(Color.FromArgb(colorR, colorG, colorB));
                this.palGreyscale4bp.Add(Color.FromArgb(colorR, colorG, colorB));

            }
            for (int i = 0; i < 256; i++)
            {
                this.palGreyscale8bp.Add(Color.FromArgb(i, i, i));
            }
        }

        private List<byte[]> OpenDat(byte[] data, string file, List<string> imgInfos)
        {
            List<byte[]> imgList = new List<byte[]>();
            const int Type_DC1 = 0;
            const int Type_DC2 = 1;
            this.paletteColor.Clear();
            this.byAllImg.Clear();
            this.texInfo.Clear();
            this.texPalMap.Clear();
            this.paletteCount = 1;
            int imgIdx = 0;
            int palIdx = 0;

            try
            {
                int entrySize = 16;
                int PackType = Type_DC1;

                // 判断是否是DC2结构（32字节entry）
                uint[] check = new uint[4];
                Buffer.BlockCopy(data, 16, check, 0, 16);
                if (check[0] == 0 && check[1] == 0 && check[2] == 0 && check[3] == 0)
                {
                    PackType = Type_DC2;
                    entrySize = 32;
                }

                int pos = 2048;
                int si = 2048 / entrySize;
                int i = 0;

                while (true)
                {
                    DC2_ENTRY_GENERIC entry = ReadEntry(data, i * entrySize, entrySize);
                    if (entry == null) break;

                    byte[] buffer = null;
                    int ssize = Align((int)entry.size, 2048);
                    if (pos + ssize > data.Length) break;

                    byte[] segmentData = new byte[entry.size];
                    Array.Copy(data, pos, segmentData, 0, entry.size);

                    switch ((GEntryType)entry.type)
                    {
                        case GEntryType.GET_TEXTURE:
                            {
                                buffer = new byte[ssize];
                                Array.Clear(buffer, 0, ssize);
                                Array.Copy(segmentData, buffer, entry.size);
                                entry.size = (uint)ssize;
                                UnswizzleGfx(buffer, entry);

                                DatTexInfo datTexInfo = this.GetDatTexInfo(entry);
                                //if (datTexInfo.height == 16 || datTexInfo.height == 32 || datTexInfo.height == 48 || base.editingFile.EndsWith("core.dat", StringComparison.OrdinalIgnoreCase)
                                //     || file.EndsWith("core.dat", StringComparison.OrdinalIgnoreCase))
                                {
                                    imgList.Add(buffer);
                                    this.byAllImg.Add(buffer);
                                    this.texInfo.Add(datTexInfo);
                                    if (imgInfos != null)
                                    {
                                        imgInfos.Add(Util.GetShortName(file) + " " + imgIdx + " " + datTexInfo.width + "X" + datTexInfo.height // + " " + datTexInfo.blockWidth + "X" + datTexInfo.blockHeight
                                            + "  " + buffer.Length);
                                    }
                                    this.texPalMap.Add(imgIdx++, palIdx);
                                }
                            }
                            break;

                        case GEntryType.GET_LZSS0:
                            {
                                byte[] dst;
                                entry.size = Dc2LzssDec(segmentData, out dst);
                                buffer = dst;
                            }
                            break;

                        case GEntryType.GET_LZSS1:
                            {
                                byte[] temp;
                                entry.size = Dc2LzssDec(segmentData, out temp);
                                buffer = new byte[Align((int)entry.size, 2048)];
                                Array.Clear(buffer, 0, buffer.Length);
                                Array.Copy(temp, buffer, entry.size);
                                entry.size = (uint)buffer.Length;
                                UnswizzleGfx(buffer, entry);

                                DatTexInfo datTexInfo = this.GetDatTexInfo(entry);
                                //if (datTexInfo.height == 16 || datTexInfo.height == 32 || datTexInfo.height == 48 || base.editingFile.EndsWith("core.dat", StringComparison.OrdinalIgnoreCase)
                                //    || file.EndsWith("core.dat", StringComparison.OrdinalIgnoreCase))
                                {
                                    imgList.Add(buffer);
                                    this.byAllImg.Add(buffer);
                                    this.texInfo.Add(datTexInfo);
                                    if (imgInfos != null)
                                    {
                                        imgInfos.Add(Util.GetShortName(file) + " " + imgIdx + " " + datTexInfo.width + "X" + datTexInfo.height // + " " + datTexInfo.blockWidth + "X" + datTexInfo.blockHeight
                                            + "  " + buffer.Length);
                                    }
                                    this.texPalMap.Add(imgIdx++, palIdx);
                                }
                            }
                            break;

                        case GEntryType.GET_PALETTE:
                            buffer = segmentData;
                            this.AddPaletteColor(buffer);
                            this.paletteCount++;
                            palIdx++;
                            break;

                        case GEntryType.GET_DATA:
                        case GEntryType.GET_SNDH:
                        case GEntryType.GET_SNDB:
                        case GEntryType.GET_SNDE:
                        case GEntryType.GET_UNK:
                            buffer = segmentData;
                            break;

                        default:
                            i = si;
                            break;
                    }

                    if (i == si) break;

                    pos += ssize;
                    i++;
                }
            }
            catch (Exception ex)
            {
            }

            return imgList;
        }

        private DatTexInfo GetDatTexInfo(DC2_ENTRY_GENERIC entry)
        {
            DatTexInfo texInfo = new DatTexInfo();

            // 解释entry为DC2_ENTRY_GFX
            ushort x = (ushort)(entry.reserve[0] & 0xFFFF);
            ushort y = (ushort)((entry.reserve[0] >> 16) & 0xFFFF);
            texInfo.blockWidth = (ushort)(entry.reserve[1] & 0xFFFF);
            texInfo.blockHeight = (ushort)((entry.reserve[1] >> 16) & 0xFFFF);

            //texInfo.blockWidth = texInfo.blockWidth * 4;
            texInfo.width = texInfo.blockWidth * 4;
            texInfo.height = texInfo.blockHeight;

            return texInfo;
        }

        private void AddPaletteColor(byte[] byPalette)
        {
            List<Color> curPalette = new List<Color>();
            for (int i = 0; i < byPalette.Length; i += 2)
            {
                int pixelColor = byPalette[i + 1] << 8 | byPalette[i];
                int colorR = ((byte)(pixelColor & 0x1F)) << 3;
                int colorG = ((byte)((pixelColor >> 5) & 0x1F)) << 3;
                int colorB = ((byte)((pixelColor >> 10) & 0x1F)) << 3;
                curPalette.Add(Color.FromArgb(colorR, colorG, colorB));

            }
            this.paletteColor.Add(curPalette.ToArray());
        }


        private DC2_ENTRY_GENERIC ReadEntry(byte[] data, int offset, int size)
        {
            if (offset + size > data.Length) return null;
            DC2_ENTRY_GENERIC e = new DC2_ENTRY_GENERIC();
            e.type = BitConverter.ToUInt32(data, offset);
            e.size = BitConverter.ToUInt32(data, offset + 4);

            int reserveCount = (size - 8) / 4;
            e.reserve = new uint[reserveCount];
            for (int i = 0; i < reserveCount; i++)
            {
                e.reserve[i] = BitConverter.ToUInt32(data, offset + 8 + i * 4);
            }
            return e;
        }

        private int Align(int val, int align)
        {
            return (val + align - 1) / align * align;
        }

        /// <summary>
        /// 解交错图像数据 (unswizzle)
        /// </summary>
        private void UnswizzleGfx(byte[] buf, DC2_ENTRY_GENERIC entry)
        {
            // 解释entry为DC2_ENTRY_GFX
            ushort x = (ushort)(entry.reserve[0] & 0xFFFF);
            ushort y = (ushort)((entry.reserve[0] >> 16) & 0xFFFF);
            ushort w = (ushort)(entry.reserve[1] & 0xFFFF);
            ushort h = (ushort)((entry.reserve[1] >> 16) & 0xFFFF);

            int tw = w / 32;
            int bw = tw * 64;
            byte[] buffer = new byte[entry.size];
            Array.Copy(buf, buffer, entry.size);

            int bIndex = 0;
            for (int yi = 0; yi < h; yi += 32)
            {
                for (int xi = 0; xi < tw; xi++)
                {
                    int scanlineIndex = yi * bw + xi * 64;
                    for (int j = 0; j < 32; j++)
                    {
                        Array.Copy(buffer, bIndex, buf, scanlineIndex, 64);
                        bIndex += 64;
                        scanlineIndex += bw;
                    }
                }
            }
        }

        /// <summary>
        /// LZSS解压
        /// </summary>
        private uint Dc2LzssDec(byte[] src, out byte[] dst)
        {
            int flag = 1;
            dst = new byte[src.Length * 8];
            int srcIndex = 0;
            int dstIndex = 0;

            while (srcIndex < src.Length)
            {
                if (flag == 1)
                {
                    if (srcIndex >= src.Length) break;
                    flag = src[srcIndex++] | 0x100;
                }

                if (srcIndex >= src.Length) break;
                byte ch = src[srcIndex++];

                if ((flag & 1) != 0)
                {
                    dst[dstIndex++] = ch;
                }
                else
                {
                    if (srcIndex >= src.Length) break;
                    byte t = src[srcIndex++];

                    int jump = ((t & 0xF) << 8) | ch;
                    int size = (t >> 4) + 2;

                    int srcDecIndex = dstIndex - jump;
                    for (int i = 0; i < size; i++)
                    {
                        if (srcDecIndex < 0 || srcDecIndex >= dst.Length) break;
                        dst[dstIndex++] = dst[srcDecIndex++];
                    }
                }
                flag >>= 1;
            }

            Array.Resize(ref dst, dstIndex);
            return (uint)dstIndex;
        }



        #endregion
    }
}
