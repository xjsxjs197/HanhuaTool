using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Hanhua.Common;

namespace Hanhua.ImgEditTools
{
    /// <summary>
    /// Ps Tim图片编辑器
    /// </summary>
    public class ImgEditorTim : ImgEditorBase
    {
        #region " 私有变量 "

        /// <summary>
        /// 图片类型 2：16BPP，3：24BPP，8：4BPP，9：8BPP
        /// </summary>
        private static int timType;

        /// <summary>
        /// 调色板数据
        /// </summary>
        private Color[] paletteColor;

        /// <summary>
        /// 图片数据
        /// </summary>
        private byte[] byImg;

        /// <summary>
        /// 图片的宽
        /// </summary>
        private int picWidth;

        /// <summary>
        /// 图片的高
        /// </summary>
        private int picHeight;

        #endregion

        /// <summary>
        /// 构造方法
        /// </summary>
        public ImgEditorTim(string file) : base(file)
        {
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
            List<byte[]> imgList = new List<byte[]>();

            try
            {
                // 分析文件内部是否包括Tim文件
                byte[] byCheck = new byte[8];
                long seekStart = 0;
                long checkLen = byData.Length - 1024;
                while (seekStart < checkLen)
                {
                    Array.Copy(byData, seekStart, byCheck, 0, byCheck.Length);
                    int timType = this.IsTimData(byCheck);
                    if (timType > -1)
                    {
                        Array.Copy(byData, seekStart + 4, byCheck, 0, byCheck.Length);
                        int fileSize = 0;
                        if (timType == 2)
                        {
                            fileSize = (byCheck[7] << 24) | (byCheck[6] << 16) | (byCheck[5] << 8) | byCheck[4];
                            fileSize += 8;
                        }
                        else
                        {
                            int imgDataStart = (byCheck[7] << 24) | (byCheck[6] << 16) | (byCheck[5] << 8) | byCheck[4];
                            imgDataStart += 0x14;

                            Array.Copy(byData, seekStart + imgDataStart - 12, byCheck, 0, byCheck.Length);

                            fileSize = (byCheck[3] << 24) | (byCheck[2] << 16) | (byCheck[1] << 8) | byCheck[0];
                            fileSize += imgDataStart - 0x14 + 8;
                        }

                        byte[] byTimData = new byte[fileSize];
                        Array.Copy(byData, seekStart, byTimData, 0, byTimData.Length);

                        imgList.Add(byTimData);
                        imgInfos.Add(Util.GetShortName(file) + "　" + seekStart.ToString("x") + "--" + (seekStart + fileSize).ToString("x"));

                        seekStart += fileSize;
                    }
                    else
                    {
                        seekStart += 4;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return imgList;
        }

        /// <summary>
        /// 从图片数据中获取图片
        /// </summary>
        /// <param name="byData"></param>
        /// <returns></returns>
        public override Image[] ImageDecode(byte[] byData, string fileInfo)
        {
            if (this.IsTimData(byData) == -1)
            {
                return null;
            }

            // 取得类型
            timType = byData[4];

            // 读取Tim图片信息
            switch (timType)
            {
                case 2:
                    return new Image[] { this.Read16bppTimPic(byData) };

                case 3:
                    return new Image[] { this.Read24bppTimPic(byData) };

                case 8:
                    return new Image[] { this.Read4bppTimPic(byData) };

                case 9:
                    return new Image[] { this.Read8bppTimPic(byData) };
            }

            return null;
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
            }

            return this.editingFile + " " +  bppInfo +  " W：" + (img == null ? 0 : img.Width) + " H：" + (img == null ? 0 : img.Height);
        }

        /// <summary>
        /// 导入图片
        /// </summary>
        public override byte[] ImportImg(string fileName, Image oldImg, byte[] byOldImg, string fileInfo)
        {
            return this.ConvertToTim(fileName, oldImg, byOldImg);
        }

        #endregion

        #region " 共有方法 "

        /// <summary>
        /// 导入图片
        /// </summary>
        public byte[] ConvertToTim(string fileName, Image oldImg, byte[] byOldImg)
        {
            Bitmap impImg = (Bitmap)Image.FromFile(fileName);
            if (impImg.Width != oldImg.Width || impImg.Height != oldImg.Height)
            {
                throw new Exception("导入图片大小不一致");
            }

            // 取得类型
            timType = byOldImg[4];

            if (timType == 0x8)
            {
                return Import4bppTim(impImg, oldImg, byOldImg);
            }
            else if (timType == 0x9)
            {
                return Import8bppTim(impImg, oldImg, byOldImg);
            }
            else
            {
                return Import16bppTim(impImg, oldImg, byOldImg);
            }
        }

        #endregion

        #region " 私有方法 "

        /// <summary>
        /// 判断是否是Tim类型数据
        /// </summary>
        /// <param name="byData"></param>
        /// <returns></returns>
        private int IsTimData(byte[] byData)
        {
            if (byData != null && byData.Length >= 8)
            {
                if (byData[0] == 0x10 && byData[1] == 0 && byData[2] == 0 && byData[3] == 0)
                {
                    if (byData[4] == 2 && byData[5] == 0 && byData[6] == 0 && byData[7] == 0)
                    {
                        return 2;
                    }
                    else if (byData[4] == 3 && byData[5] == 0 && byData[6] == 0 && byData[7] == 0)
                    {
                        return 3;
                    }
                    else if (byData[4] == 8 && byData[5] == 0 && byData[6] == 0 && byData[7] == 0)
                    {
                        return 8;
                    }
                    else if (byData[4] == 9 && byData[5] == 0 && byData[6] == 0 && byData[7] == 0)
                    {
                        return 9;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// 读取调色板颜色
        /// </summary>
        private void ReadPaletteColor(byte[] byData)
        {
            int clutBytes = (byData[0x11] << 8 | byData[0x10]) * 2;
            int paletteNum = byData[0x12];
            byte[] byPalette = new byte[clutBytes * paletteNum];
            Array.Copy(byData, 0x14, byPalette, 0, byPalette.Length);
            this.paletteColor = new Color[byPalette.Length / 2];

            int colorIndex = 0;
            for (int i = 0; i < byPalette.Length; i += 2)
            {
                int pixelColor = byPalette[i + 1] << 8 | byPalette[i];
                int colorR = Util.Convert5To8((byte)(pixelColor & 0x1F));
                int colorG = Util.Convert5To8((byte)((pixelColor >> 5) & 0x1F));
                int colorB = Util.Convert5To8((byte)((pixelColor >> 10) & 0x1F));
                this.paletteColor[colorIndex++] = Color.FromArgb(colorR, colorG, colorB);
            }

            // 根据选择的调色板Index，重新设置调色板
            this.paletteCount = paletteNum;
            if (this.paletteIndex > 0)
            {
                int startPos = 0;
                if (timType == 8)
                {
                    startPos += this.paletteIndex * 16;
                }
                else if (timType == 9)
                {
                    startPos += this.paletteIndex * 256;                   
                }

                Color[] newPaletteColor = new Color[this.paletteColor.Length - startPos];
                Array.Copy(this.paletteColor, startPos, newPaletteColor, 0, newPaletteColor.Length);

                this.paletteColor = newPaletteColor;
            }
        }

        /// <summary>
        /// 读取16位Tim图片信息
        /// </summary>
        /// <returns></returns>
        private Bitmap Read16bppTimPic(byte[] byData)
        {
            // 设置图片数据
            int picStart = 0x14;
            this.byImg = new byte[byData.Length - picStart];
            Array.Copy(byData, picStart, this.byImg, 0, this.byImg.Length);

            this.picWidth = (byData[0x11] << 8) | (byData[0x10]);
            this.picHeight = (byData[0x13] << 8) | (byData[0x12]);

            // 初始化图片
            Bitmap img = new Bitmap(this.picWidth, this.picHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            // 生成图片
            int byIndex = 0;
            for (int y = 0; y < this.picHeight; y++)
            {
                for (int x = 0; x < this.picWidth; x++)
                {
                    int pixelColor = this.byImg[byIndex + 1] << 8 | this.byImg[byIndex];
                    int colorR = Util.Convert5To8((byte)(pixelColor & 0x1F));
                    int colorG = Util.Convert5To8((byte)((pixelColor >> 5) & 0x1F));
                    int colorB = Util.Convert5To8((byte)((pixelColor >> 10) & 0x1F));
                    img.SetPixel(x, y, Color.FromArgb(colorR, colorG, colorB));

                    byIndex += 2;
                }
            }

            return img;
        }

        /// <summary>
        /// 读取24位Tim图片信息
        /// </summary>
        /// <returns></returns>
        private Bitmap Read24bppTimPic(byte[] byData)
        {
            // 设置图片数据
            int picStart = 0x14;
            this.byImg = new byte[byData.Length - picStart];
            Array.Copy(byData, picStart, this.byImg, 0, this.byImg.Length);

            this.picWidth = (int)(((byData[0x11] << 8) | (byData[0x10])) * 2 / 3);
            this.picHeight = (byData[0x13] << 8) | (byData[0x12]);

            // 初始化图片
            Bitmap img = new Bitmap(this.picWidth, this.picHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            // 生成图片
            int byIndex = 0;
            for (int y = 0; y < this.picHeight; y++)
            {
                for (int x = 0; x < this.picWidth; x++)
                {
                    img.SetPixel(x, y, Color.FromArgb(this.byImg[byIndex + 2], this.byImg[byIndex + 1], this.byImg[byIndex]));

                    byIndex += 3;
                }
            }

            return img;
        }

        /// <summary>
        /// 读取4位图片信息
        /// </summary>
        /// <returns></returns>
        private Bitmap Read4bppTimPic(byte[] byData)
        {
            // 取得调色板数据
            this.ReadPaletteColor(byData);

            // 设置图片数据
            int picStart = 0x14;
            picStart += (byData[9] << 8) | (byData[8]);
            this.byImg = new byte[byData.Length - picStart];
            Array.Copy(byData, picStart, this.byImg, 0, this.byImg.Length);

            this.picWidth = ((byData[picStart - 3] << 8) | (byData[picStart - 4])) * 4;
            this.picHeight = (byData[picStart - 1] << 8) | (byData[picStart - 2]);

            // 初始化图片
            Bitmap img = new Bitmap(this.picWidth, this.picHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            // 生成图片
            int byIndex = 0;
            for (int y = 0; y < this.picHeight; y++)
            {
                for (int x = 0; x < this.picWidth; x++)
                {
                    int pixelNum = y * this.picWidth + x;
                    Color pixelColor;
                    if (pixelNum % 2 == 0)
                    {
                        pixelColor = this.paletteColor[this.byImg[byIndex] & 0xF];
                    }
                    else
                    {
                        pixelColor = this.paletteColor[(this.byImg[byIndex] >> 4) & 0xF];
                        byIndex++;
                    }

                    img.SetPixel(x, y, pixelColor);
                }
            }

            return img;
        }

        /// <summary>
        /// 读取8位图片信息
        /// </summary>
        /// <returns></returns>
        private Bitmap Read8bppTimPic(byte[] byData)
        {
            // 取得调色板数据
            this.ReadPaletteColor(byData);

            // 设置图片数据
            int picStart = 0x14;
            picStart += (byData[9] << 8) | (byData[8]);
            this.byImg = new byte[byData.Length - picStart];
            Array.Copy(byData, picStart, this.byImg, 0, this.byImg.Length);

            this.picWidth = ((byData[picStart - 3] << 8) | (byData[picStart - 4])) * 2;
            this.picHeight = (byData[picStart - 1] << 8) | (byData[picStart - 2]);

            // 初始化图片
            Bitmap img = new Bitmap(this.picWidth, this.picHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            for (int y = 0; y < this.picHeight; y++)
            {
                for (int x = 0; x < this.picWidth; x++)
                {
                    int pixelNum = y * this.picWidth + x;
                    Color pixelColor = this.paletteColor[this.byImg[pixelNum]];
                    img.SetPixel(x, y, pixelColor);
                }
            }

            return img;
        }

        /// <summary>
        /// 导入图片
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="oldImg"></param>
        /// <param name="byOldImg"></param>
        /// <returns></returns>
        private byte[] Import16bppTim(Bitmap impImg, Image oldImg, byte[] byOldImg)
        {
            byte[] byNewImg = new byte[byOldImg.Length];
            byte[] byImgData = new byte[oldImg.Width * oldImg.Height * 2];
            int byIndex = 0;
            for (int y = 0; y < impImg.Height; y++)
            {
                for (int x = 0; x < impImg.Width; x++)
                {
                    Color color = impImg.GetPixel(x, y);
                    int pixelColor = (Util.Convert8To5(color.B) << 10) | (Util.Convert8To5(color.G) << 5) | Util.Convert8To5(color.R);
                    byImgData[byIndex] = (byte)(pixelColor & 0xFF);
                    byImgData[byIndex + 1] = (byte)((pixelColor >> 8) & 0xFF);

                    byIndex += 2;
                }
            }

            Array.Copy(byOldImg, 0, byNewImg, 0, 0x14);
            Array.Copy(byImgData, 0, byNewImg, 0x14, byImgData.Length);

            return byNewImg;
        }

        /// <summary>
        /// 导入图片
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="oldImg"></param>
        /// <param name="byOldImg"></param>
        /// <returns></returns>
        private byte[] Import4bppTim(Bitmap impImg, Image oldImg, byte[] byOldImg)
        {
            // 取得调色板数据
            this.ReadPaletteColor(byOldImg);

            // 只有一种调色板数据时，重新根据图片设置调色板数据
            byte[] byPalette = this.ResetPaletteData(16, impImg);

            byte[] byNewImg = new byte[byOldImg.Length];
            byte[] byImgData = new byte[oldImg.Width * oldImg.Height / 2];
            int byIndex = 0;
            for (int y = 0; y < impImg.Height; y++)
            {
                for (int x = 0; x < impImg.Width; x++)
                {
                    int pixelNum = y * impImg.Width + x;
                    int paletteIndex = this.GetPaletteIndex(impImg.GetPixel(x, y), 16);
                    if (pixelNum % 2 == 0)
                    {
                        byImgData[byIndex] = (byte)(paletteIndex & 0xF);
                    }
                    else
                    {
                        byImgData[byIndex] = (byte)(((paletteIndex << 4) & 0xF0) | byImgData[byIndex]);
                        byIndex++;
                    }
                }
            }

            // 设置图片数据
            int picStart = 0x14;
            picStart += (byOldImg[9] << 8) | (byOldImg[8]);
            Array.Copy(byOldImg, 0, byNewImg, 0, picStart);
            Array.Copy(byImgData, 0, byNewImg, picStart, byImgData.Length);

            // 设置调色板
            if (byPalette != null)
            {
                Array.Copy(byPalette, 0, byNewImg, 0x14, byPalette.Length);
            }

            return byNewImg;
        }

        /// <summary>
        /// 导入图片
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="oldImg"></param>
        /// <param name="byOldImg"></param>
        /// <returns></returns>
        private byte[] Import8bppTim(Bitmap impImg, Image oldImg, byte[] byOldImg)
        {
            // 取得调色板数据
            this.ReadPaletteColor(byOldImg);

            // 只有一种调色板数据时，重新根据图片设置调色板数据
            byte[] byPalette = this.ResetPaletteData(256, impImg);

            byte[] byNewImg = new byte[byOldImg.Length];
            byte[] byImgData = new byte[oldImg.Width * oldImg.Height];
            int byIndex = 0;
            for (int y = 0; y < impImg.Height; y++)
            {
                for (int x = 0; x < impImg.Width; x++)
                {
                    int paletteIndex = this.GetPaletteIndex(impImg.GetPixel(x, y), 256);
                    byImgData[byIndex++] = (byte)paletteIndex;
                }
            }

            // 设置图片数据
            int picStart = 0x14;
            picStart += (byOldImg[9] << 8) | (byOldImg[8]);
            Array.Copy(byOldImg, 0, byNewImg, 0, picStart);
            Array.Copy(byImgData, 0, byNewImg, picStart, byImgData.Length);

            // 设置调色板
            if (byPalette != null)
            {
                Array.Copy(byPalette, 0, byNewImg, 0x14, byPalette.Length);
            }

            return byNewImg;
        }

        /// <summary>
        /// 取得颜色在调色板位置
        /// </summary>
        /// <param name="color"></param>
        /// <param name="colorCount"></param>
        /// <returns></returns>
        private int GetPaletteIndex(Color color, int colorCount)
        {
            int minVal = 9999;
            int minValIndex = 9999;

            for (int i = 0; i < colorCount; i++)
            {
                if (this.paletteColor[i].ToArgb() == color.ToArgb())
                {
                    return i;
                }
                else
                {
                    int diffVal = Math.Abs(this.paletteColor[i].R - color.R) + Math.Abs(this.paletteColor[i].G - color.G) + Math.Abs(this.paletteColor[i].B - color.B);
                    if (diffVal < minVal)
                    {
                        minVal = diffVal;
                        minValIndex = i;
                    }
                }
            }

            return minValIndex;
        }

        /// <summary>
        /// 只有一种调色板数据时，重新根据图片设置调色板数据
        /// </summary>
        /// <param name="checkCount"></param>
        private byte[] ResetPaletteData(int checkCount, Bitmap impImg)
        {
            if (this.paletteColor.Length <= checkCount)
            {
                List<int> newColors = new List<int>();
                for (int y = 0; y < impImg.Height; y++)
                {
                    for (int x = 0; x < impImg.Width; x++)
                    {
                        if (!newColors.Contains(impImg.GetPixel(x, y).ToArgb()))
                        {
                            newColors.Add(impImg.GetPixel(x, y).ToArgb());
                        }
                    }
                }

                if (newColors.Count > checkCount)
                {
                    throw new Exception("调色板数据过多！");
                }

                this.paletteColor = new Color[checkCount];
                byte[] byPalette = new byte[checkCount * 2];
                int paletteIndex = 0;
                for (int i = 0; i < newColors.Count; i++)
                {
                    this.paletteColor[i] = Color.FromArgb(newColors[i]);
                    int color = (Util.Convert8To5(this.paletteColor[i].B) << 10) | (Util.Convert8To5(this.paletteColor[i].G) << 5) | Util.Convert8To5(this.paletteColor[i].R);
                    byPalette[paletteIndex++] = (byte)(color & 0xFF);
                    byPalette[paletteIndex++] = (byte)((color >> 8) & 0xFF);
                }

                return byPalette;
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
