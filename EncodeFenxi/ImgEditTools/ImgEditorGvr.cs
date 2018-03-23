using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using VrSharp.GvrTexture;
using Hanhua.Common;

namespace Hanhua.ImgEditTools
{
    /// <summary>
    /// Dc Gvr图片编辑器
    /// </summary>
    public class ImgEditorGvr : ImgEditorBase
    {
        #region " 私有变量 "

        /// <summary>
        /// 图片类型
        /// </summary>
        private int imgFormat;

        /// <summary>
        /// 调色板类型
        /// </summary>
        private int paletteType;

        /// <summary>
        /// 调色板颜色
        /// </summary>
        private int paletteFormat;

        /// <summary>
        /// 调色板数据
        /// </summary>
        private Dictionary<string, byte[]> paletteData = new Dictionary<string, byte[]>();

        #endregion

        /// <summary>
        /// 构造方法
        /// </summary>
        public ImgEditorGvr(string file)
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

            // 分析文件内部是否包括Gvr文件
            for (int i = 0; i < byData.Length - 1024; i++)
            {
                if (byData[i] == 0x47 && byData[i + 1] == 0x56 && byData[i + 2] == 0x52 && byData[i + 3] == 0x54)
                {
                    int imgFormat = byData[i + 0xB];
                    string wiiImgFormat = Util.GetImageFormat(imgFormat);
                    if (imgFormat == 0xa)
                    {
                        throw new Exception("不支持的图片格式 : " + wiiImgFormat);
                    }

                    // 取得图片大小
                    int width = (byData[i + 0xC] << 8) | byData[i + 0xD];
                    int height = (byData[i + 0xE] << 8) | byData[i + 0xF];
                    int imgByCount = Util.GetImageByteCount(height, width, wiiImgFormat);
                    if (imgByCount == 0 || imgByCount > byData.Length)
                    {
                        throw new Exception("图片容量异常格式 : " + imgByCount);
                    }

                    byte[] byGvrData = new byte[0x20 + imgByCount];
                    Array.Copy(byData, i - 0x10, byGvrData, 0, byGvrData.Length);

                    string imgFileInfo = Util.GetShortName(file) + "　" + (i - 0x10).ToString("x") + "--" + (i - 0x10 + byGvrData.Length).ToString("x");

                    // 取得调色板大小
                    if (imgFormat == 0x8 || imgFormat == 0x9)
                    {
                        int paletteCount = imgFormat == 0x8 ? 32 : 512;
                        byte[] byPalette = new byte[paletteCount + 0x10];
                        int paletteDataStart = i - 0x30;
                        if (byData[paletteDataStart] == 0x50 && byData[paletteDataStart + 1] == 0x50
                            && byData[paletteDataStart + 2] == 0x56 && byData[paletteDataStart + 3] == 0x52)
                        {
                            paletteDataStart = i - 0x20 - paletteCount - 0x10 - 0x20;
                        }
                        else
                        {
                            paletteDataStart = i - 0x20 - paletteCount - 0x10;
                        }

                        //int paletteDataStart = imgByCount + i + 0x10;

                        //if (byData[paletteDataStart] == 0x50 && byData[paletteDataStart + 1] == 0x50
                        //    && byData[paletteDataStart + 2] == 0x56 && byData[paletteDataStart + 3] == 0x50)
                        //{
                        //    paletteDataStart += 0x20;
                        //}

                        Array.Copy(byData, paletteDataStart, byPalette, 0, byPalette.Length);

                        this.paletteData.Add(imgFileInfo + " " + paletteDataStart.ToString(), byPalette);
                    }

                    imgList.Add(byGvrData);
                    imgInfos.Add(imgFileInfo);
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
            //GvrTexture gvrTexture = new GvrTexture(byData);
            //if (gvrTexture.NeedsExternalPalette)
            //{
            //    // 取得调色板数据
            //    KeyValuePair<string, byte[]> paletteKeyValue = this.paletteData.FirstOrDefault(p => p.Key.IndexOf(fileInfo) >= 0);
            //    if (string.IsNullOrEmpty(paletteKeyValue.Key))
            //    {
            //        throw new Exception("没有找到调色板数据");
            //    }

            //    GvpPalette palette = new GvpPalette(paletteKeyValue.Value);
            //    gvrTexture.SetPalette(palette);
            //}

            //return gvrTexture.ToBitmap();
            int width = (byData[0x1C] << 8) | byData[0x1D];
            int height = (byData[0x1E] << 8) | byData[0x1F];
            this.imgFormat = byData[0x1B];
            if (imgFormat == 0xa)
            {
                throw new Exception("不支持的图片格式 : " + Util.GetImageFormat(this.imgFormat));
            }

            Bitmap img = new Bitmap(width, height);
            Image gvrImg = null;

            // 取得调色板大小
            this.paletteType = -1;
            this.paletteFormat = -1;
            if (this.imgFormat == 0x8 || this.imgFormat == 0x9)
            {
                // 取得调色板类型
                this.paletteType = byData[0x1A] & 0xF;

                // 取得调色板数据
                KeyValuePair<string, byte[]> paletteKeyValue = this.paletteData.FirstOrDefault(p => p.Key.IndexOf(fileInfo) >= 0);
                if (string.IsNullOrEmpty(paletteKeyValue.Key))
                {
                    throw new Exception("没有找到调色板数据");
                }

                byte[] byGvpl = paletteKeyValue.Value;
                this.paletteFormat = byGvpl[0x9];
                byte[] byPalette = new byte[byGvpl.Length - 0x10];
                Array.Copy(byGvpl, 0x10, byPalette, 0, byPalette.Length);
                //Array.Copy(byGvpl, 0x20, byPalette, 0, 0x10);

                byte[] byImgData = new byte[byData.Length - 0x20];
                Array.Copy(byData, 0x20, byImgData, 0, byImgData.Length);

                gvrImg = Util.PaletteImageDecode(img, byImgData, Util.GetImageFormat(this.imgFormat), byPalette, this.paletteFormat);
            }
            else if (this.imgFormat == 0xe)
            {
                byte[] byImgData = new byte[byData.Length - 0x20];
                Array.Copy(byData, 0x20, byImgData, 0, byImgData.Length);

                gvrImg = Util.CmprImageDecode(img, byImgData);
            }
            else
            {
                byte[] byImgData = new byte[byData.Length - 0x20];
                Array.Copy(byData, 0x20, byImgData, 0, byImgData.Length);

                gvrImg = Util.ImageDecode(img, byImgData, Util.GetImageFormat(this.imgFormat));
            }

            return new Image[] { gvrImg };
        }

        /// <summary>
        /// 设置编辑器的Title
        /// </summary>
        /// <param name="newTitle"></param>
        public override string GetEditorTitle(Image img)
        {
            return this.editingFile + " " + Util.GetImageFormat(this.imgFormat) + this.GetPaletteColor() + this.GetPaletteType() + " W：" + img.Width + " H：" + img.Height;
        }

        /// <summary>
        /// 导入图片
        /// </summary>
        public override byte[] ImportImg(string fileName, Image oldImg, byte[] byOldImg, string fileInfo)
        {
            Bitmap impImg = (Bitmap)Image.FromFile(fileName);

            //GvrTextureEncoder gvrEncoder = new GvrTextureEncoder(impImg, (GvrPixelFormat)this.paletteFormat, (GvrDataFormat)this.imgFormat);
            //gvrEncoder.GbixType = GvrGbixType.Gcix;
            //byte[] byEncodedImg = gvrEncoder.ToArray();

            //if (this.imgFormat == 0x8 || this.imgFormat == 0x9)
            //{
            //    // 取得调色板数据
            //    KeyValuePair<string, byte[]> paletteKeyValue = this.paletteData.FirstOrDefault(p => p.Key.IndexOf(fileInfo) >= 0);
            //    if (string.IsNullOrEmpty(paletteKeyValue.Key))
            //    {
            //        throw new Exception("没有找到调色板数据");
            //    }
            //    byte[] byGvpl = paletteKeyValue.Value;

            //    // 写入调色板数据
            //    int paletteLen = byGvpl.Length - 0x10;
            //    Array.Copy(byEncodedImg, 0x20, byGvpl, 0x10, paletteLen);

            //    // 去掉数据中的调色板数据
            //    byte[] byNewImg = new byte[byOldImg.Length];
            //    Array.Copy(byOldImg, 0, byNewImg, 0, 0x20);
            //    Array.Copy(byEncodedImg, 0x20 + paletteLen, byNewImg, 0x20, byNewImg.Length - 0x20);
            //    byEncodedImg = byNewImg;
            //}

            //return byEncodedImg;

            byte[] byNewImg = null;
            if (this.imgFormat == 0x8 || this.imgFormat == 0x9)
            {
                // 取得调色板数据
                KeyValuePair<string, byte[]> paletteKeyValue = this.paletteData.FirstOrDefault(p => p.Key.IndexOf(fileInfo) >= 0);
                if (string.IsNullOrEmpty(paletteKeyValue.Key))
                {
                    throw new Exception("没有找到调色板数据");
                }
                byte[] byGvpl = paletteKeyValue.Value;

                // 图片编码
                List<byte> paletteList = new List<byte>();
                byNewImg = Util.PaletteImageEncode(impImg, Util.GetImageFormat(this.imgFormat), paletteList, this.paletteFormat);

                // 写入调色板数据
                byte[] byPalette = paletteList.ToArray();
                Array.Copy(byPalette, 0, byGvpl, 0x10, byPalette.Length);
            }
            else if (this.imgFormat == 0xe)
            {
                byNewImg = Util.CmprImageEncode(impImg);
            }
            else
            {
                byNewImg = Util.ImageEncode(impImg, Util.GetImageFormat(this.imgFormat));
            }

            if (byNewImg != null)
            {
                byte[] impedImg = new byte[byOldImg.Length];
                Array.Copy(byOldImg, 0, impedImg, 0, 0x20);
                Array.Copy(byNewImg, 0, impedImg, 0x20, byNewImg.Length);

                return impedImg;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="byData"></param>
        /// <param name="fileInfo"></param>
        public override void Save(string fileName, byte[] byData, string fileInfo)
        {
            // 保存调色板数据
            KeyValuePair<string, byte[]> paletteKeyValue = this.paletteData.FirstOrDefault(p => p.Key.IndexOf(fileInfo) >= 0);
            if (!string.IsNullOrEmpty(paletteKeyValue.Key))
            {
                byte[] byGvpl = paletteKeyValue.Value;
                string[] palettePosInfo = paletteKeyValue.Key.Split(' ');
                int paletteDataStart = Convert.ToInt32(palettePosInfo[palettePosInfo.Length - 1]);

                Array.Copy(byGvpl, 0, byData, paletteDataStart, byGvpl.Length);
            }

            File.WriteAllBytes(fileName, byData);
        }

        #endregion

        #region " 私有方法 "

        /// <summary>
        /// 取得调色板类型
        /// </summary>
        /// <returns></returns>
        private string GetPaletteType()
        {
            switch (this.paletteType)
            {
                case 1:
                    return " Mipmaps";

                case 2:
                    return " ExternalPalette";

                case 3:
                    return " InternalPalette";
            }

            return string.Empty;
        }

        /// <summary>
        /// 取得调色板颜色
        /// </summary>
        /// <returns></returns>
        private string GetPaletteColor()
        {
            switch (this.paletteFormat)
            {
                case 0:
                    return " IA8";

                case 1:
                    return " Rgb565";

                case 2:
                    return " Rgb5a3";
            }

            return string.Empty;
        }

        #endregion
    }
}