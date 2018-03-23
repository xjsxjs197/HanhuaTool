using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Hanhua.Common;

namespace Hanhua.ImgEditTools
{
    /// <summary>
    /// Ps2 Tim2图片编辑器
    /// </summary>
    public class ImgEditorTim2 : ImgEditorBase
    {
        #region " 私有变量 "

        /// <summary>
        /// 图片类型 1：16BPP，2：24BPP，3：32BPP，4：4BPP，5：8BPP
        /// </summary>
        private int timType;

        /// <summary>
        /// 调色板数据
        /// </summary>
        private Dictionary<string, byte[]> paletteData = new Dictionary<string, byte[]>();

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

        /// <summary>
        /// 图片数据开始位置
        /// </summary>
        private int picStart = 0x100;

        /// <summary>
        /// 选中的文件信息
        /// </summary>
        private string selecedFileInfo;

        #endregion

        /// <summary>
        /// 构造方法
        /// </summary>
        public ImgEditorTim2(string file)
            : base(file)
        {
        }

        #region " 重写父类的虚方法 "

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Reset()
        {
            this.paletteData.Clear();
        }

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

            // 分析文件内部是否包括Tim文件
            byte[] byCheck = new byte[picStart];
            long seekStart = 0;
            long checkLen = byData.Length - 1024;
            while (seekStart < checkLen)
            {
                Array.Copy(byData, seekStart, byCheck, 0, byCheck.Length);
                this.timType = this.IsTim2Data(byCheck);
                if (this.timType > -1)
                {
                    int fileSize = this.GetImgDataSize(
                        (byData[seekStart + 0x95] << 8) | byData[seekStart + 0x94], 
                        (byData[seekStart + 0x97] << 8) | byData[seekStart + 0x96]);
                    if (fileSize == 0)
                    {
                        //throw new Exception("图片数据大小不正确！");
                        seekStart += 4;
                        continue;
                    }

                    string imgFileInfo = Util.GetShortName(file) + " " + seekStart.ToString("x") + "--" + (seekStart + fileSize).ToString("x");

                    if (this.timType == 4 || this.timType == 5)
                    {
                        // 调色板类型图片，查找调色板数据
                        int paletteDataStart = 0;
                        byte[] byPalette = this.GetPaletteData(byData, (int)seekStart, fileSize, ref paletteDataStart);
                        if (byPalette == null)
                        {
                            throw new Exception("调色板数据不正确！");
                        }

                        this.paletteData.Add(imgFileInfo + " " + paletteDataStart.ToString(), byPalette);
                    }

                    byte[] byTimData = new byte[this.picStart + fileSize];
                    Array.Copy(byData, seekStart, byTimData, 0, byTimData.Length);

                    imgList.Add(byTimData);
                    imgInfos.Add(imgFileInfo);

                    seekStart += fileSize;
                }
                else
                {
                    seekStart += 4;
                }
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
            // 取得类型
            this.timType = this.IsTim2Data(byData);

            if (this.timType == -1)
            {
                return null;
            }

            // 读取Tim图片信息
            this.selecedFileInfo = fileInfo;
            switch (this.timType)
            {
                case 1:
                    return new Image[] { this.Read16bppTimPic(byData) };

                case 2:
                    return new Image[] { this.Read24bppTimPic(byData) };

                case 3:
                    return new Image[] { this.Read32bppTimPic(byData) };

                case 4:
                    return new Image[] { this.Read4bppTimPic(byData) };

                case 5:
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
            switch (this.timType)
            {
                case 1:
                    bppInfo = "16BPP";
                    break;

                case 2:
                    bppInfo = "24BPP";
                    break;

                case 3:
                    bppInfo = "32BPP";
                    break;

                case 4:
                    bppInfo = "4BPP";
                    break;

                case 5:
                    bppInfo = "8BPP";
                    break;
            }

            return this.editingFile + " " +  bppInfo +  " W：" + img.Width + " H：" + img.Height;
        }

        #endregion

        #region " 私有方法 "

        /// <summary>
        /// 取得当前图片的调色板数据
        /// </summary>
        /// <param name="byData"></param>
        /// <param name="tim2Start"></param>
        /// <returns></returns>
        private byte[] GetPaletteData(byte[] byData, int tim2Start, int fileSize, ref int paletteStart)
        {
            byte[] byPalette = null;
            if (this.timType == 5)
            {
                byPalette = new byte[256 * 4];
                paletteStart = tim2Start + this.picStart + fileSize;
                Array.Copy(byData, paletteStart, byPalette, 0, byPalette.Length);
                return byPalette;
            }
            else
            {
                byPalette = new byte[16 * 4];
                paletteStart = tim2Start + this.picStart + fileSize;
                Array.Copy(byData, paletteStart, byPalette, 0, byPalette.Length);
                return byPalette;
            }

            //// 先查找外部的调色板数据
            //int endPos = tim2Start - 0x10;
            //int oldStart = tim2Start;

            //while (tim2Start > 0x10)
            //{
            //    tim2Start -= 0x10;
            //    if (byData[tim2Start] == 0x50
            //        && byData[tim2Start + 1] == 0x56
            //        && byData[tim2Start + 2] == 0x50
            //        && byData[tim2Start + 3] == 0x4C
            //        && (byData[tim2Start + 4] == 0x08
            //            || byData[tim2Start + 4] == 0x48))
            //    {
            //        paletteStart = tim2Start;
            //        byPalette = new byte[endPos - tim2Start];
            //        Array.Copy(byData, tim2Start, byPalette, 0, byPalette.Length);
            //        return byPalette;
            //    }
            //}

            //// 再查找内部的调色板数据
            //int paletteColor = this.timType == 4 ? 32 : 256;
            //byPalette = new byte[paletteColor * 4];
            //paletteStart = oldStart + this.picStart + fileSize;
            //Array.Copy(byData, paletteStart, byPalette, 0, byPalette.Length);
            //return byPalette;
        }

        /// <summary>
        /// 取得图片数据大小
        /// </summary>
        /// <returns></returns>
        private int GetImgDataSize(int width, int height)
        {
            this.picWidth = width;
            this.picHeight = height;

            switch (this.timType)
            {
                case 1:
                    return this.picWidth * this.picHeight * 2;

                case 2:
                    return this.picWidth * this.picHeight * 3;

                case 3:
                    return this.picWidth * this.picHeight * 4;

                case 4:
                    return this.picWidth * this.picHeight / 2;

                case 5:
                    return this.picWidth * this.picHeight;
            }

            return 0;
        }

        /// <summary>
        /// 判断是否是Tim2类型数据
        /// </summary>
        /// <param name="byData"></param>
        /// <returns>Tim2类型</returns>
        private int IsTim2Data(byte[] byData)
        {
            if (byData != null && byData.Length >= picStart)
            {
                if (byData[0] == 0x54 && byData[1] == 0x49 && byData[2] == 0x4D && byData[3] == 0x32)
                {
                    return byData[0x93];
                }
            }

            return -1;
        }

        /// <summary>
        /// 读取调色板颜色
        /// </summary>
        private void ReadPaletteColor(byte[] byData)
        {
            // 取得调色板数据
            KeyValuePair<string, byte[]> paletteKeyValue = this.paletteData.FirstOrDefault(p => p.Key.IndexOf(this.selecedFileInfo) >= 0);
            if (string.IsNullOrEmpty(paletteKeyValue.Key))
            {
                throw new Exception("没有找到调色板数据");
            }

            int colorStep = 4;
            byte[] byPalette = paletteKeyValue.Value;
            //byte[] byPalette;
            //byte[] byPvpl = paletteKeyValue.Value;
            //if (byPvpl[0] == 0x50
            //        && byPvpl[1] == 0x56
            //        && byPvpl[2] == 0x50
            //        && byPvpl[3] == 0x4C
            //        && (byPvpl[4] == 0x08
            //            || byPvpl[4] == 0x48))
            //{
            //    byPalette = new byte[byPvpl.Length - 0x10];
            //    Array.Copy(byPvpl, 0x10, byPalette, 0, byPalette.Length);
            //    colorStep = byPvpl[5];
            //    if (colorStep == 0)
            //    {
            //        colorStep = 4;
            //    }
            //}
            //else
            //{
            //    byPalette = byPvpl;
            //}

            this.paletteColor = new Color[byPalette.Length / colorStep];
            int colorIndex = 0;
            for (int i = 0; i < byPalette.Length; i += colorStep)
            {
                if (colorStep == 2)
                {
                    int pixelColor = byPalette[i + 1] << 8 | byPalette[i];
                    int colorB = Util.Convert5To8((byte)(pixelColor & 0x1F));
                    int colorG = Util.Convert5To8((byte)((pixelColor >> 5) & 0x1F));
                    int colorR = Util.Convert5To8((byte)((pixelColor >> 10) & 0x1F));

                    this.paletteColor[colorIndex++] = Color.FromArgb(colorR, colorG, colorB);
                }
                else
                {
                    int colorA = byPalette[i + 3];
                    int colorB = byPalette[i + 2];
                    int colorG = byPalette[i + 1];
                    int colorR = byPalette[i];
                    this.paletteColor[colorIndex++] = Color.FromArgb(colorA, colorR, colorG, colorB);
                }
            }

            // 特殊格式调整调色板顺序
            if (this.timType == 5)
            {
                Color[] newColors = new Color[this.paletteColor.Length];
                int parts = this.paletteColor.Length / 32;
                int newIndex = 0;

                for (int part = 0; part < parts; part++)
                {
                    colorIndex = part * 32;
                    for (int i = colorIndex; i < colorIndex + 8; i++)
                    {
                        newColors[newIndex++] = this.paletteColor[i];
                    }

                    colorIndex = part * 32 + 16;
                    for (int i = colorIndex; i < colorIndex + 8; i++)
                    {
                        newColors[newIndex++] = this.paletteColor[i];
                    }

                    colorIndex = part * 32 + 8;
                    for (int i = colorIndex; i < colorIndex + 8; i++)
                    {
                        newColors[newIndex++] = this.paletteColor[i];
                    }

                    colorIndex = part * 32 + 24;
                    for (int i = colorIndex; i < colorIndex + 8; i++)
                    {
                        newColors[newIndex++] = this.paletteColor[i];
                    }
                }

                this.paletteColor = newColors;
            }
        }

        /// <summary>
        /// 设置图片的基本信息
        /// </summary>
        /// <param name="byData"></param>
        private void SetTim2BaseInfo(byte[] byData)
        {
            this.byImg = new byte[byData.Length - this.picStart];
            Array.Copy(byData, this.picStart, this.byImg, 0, this.byImg.Length);

            this.picWidth = (byData[0x95] << 8) | (byData[0x94]);
            this.picHeight = (byData[0x97] << 8) | (byData[0x96]);
        }

        /// <summary>
        /// 读取16位Tim2图片信息
        /// </summary>
        /// <returns></returns>
        private Bitmap Read16bppTimPic(byte[] byData)
        {
            // 设置图片数据
            this.SetTim2BaseInfo(byData);

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
        /// 读取24位Tim2图片信息
        /// </summary>
        /// <returns></returns>
        private Bitmap Read24bppTimPic(byte[] byData)
        {
            // 设置图片数据
            this.SetTim2BaseInfo(byData);

            // 初始化图片
            Bitmap img = new Bitmap(this.picWidth, this.picHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            // 生成图片
            int byIndex = 0;
            for (int y = 0; y < this.picHeight; y++)
            {
                for (int x = 0; x < this.picWidth; x++)
                {
                    int colorR = this.byImg[byIndex];
                    int colorG = this.byImg[byIndex + 1];
                    int colorB = this.byImg[byIndex + 2];
                    img.SetPixel(x, y, Color.FromArgb(colorR, colorG, colorB));

                    byIndex += 3;
                }
            }

            return img;
        }

        /// <summary>
        /// 读取32位Tim2图片信息
        /// </summary>
        /// <returns></returns>
        private Bitmap Read32bppTimPic(byte[] byData)
        {
            // 设置图片数据
            this.SetTim2BaseInfo(byData);

            // 初始化图片
            Bitmap img = new Bitmap(this.picWidth, this.picHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            // 生成图片
            int byIndex = 0;
            for (int y = 0; y < this.picHeight; y++)
            {
                for (int x = 0; x < this.picWidth; x++)
                {
                    int colorR = this.byImg[byIndex];
                    int colorG = this.byImg[byIndex + 1];
                    int colorB = this.byImg[byIndex + 2];
                    int colorA = this.byImg[byIndex + 3];
                    img.SetPixel(x, y, Color.FromArgb(colorA, colorR, colorG, colorB));

                    byIndex += 4;
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
            // 设置图片数据
            this.SetTim2BaseInfo(byData);

            // 取得调色板数据
            this.ReadPaletteColor(byData);

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
            // 设置图片数据
            this.SetTim2BaseInfo(byData);

            // 取得调色板数据
            this.ReadPaletteColor(byData);

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

        #endregion
    }
}
