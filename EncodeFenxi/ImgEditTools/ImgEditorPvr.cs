using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Hanhua.Common;
using VrSharp.PvrTexture;

namespace Hanhua.ImgEditTools
{
    /// <summary>
    /// Dc Pvr图片编辑器
    /// </summary>
    public class ImgEditorPvr : ImgEditorBase
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
        public ImgEditorPvr(string file)
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

            // 分析文件内部是否包括Pvr文件
            for (int i = 0; i < byData.Length - 1024; i++)
            {
                if (byData[i] == 0x50 && byData[i + 1] == 0x56 && byData[i + 2] == 0x52 && byData[i + 3] == 0x54
                    && byData[i - 0x10] == 0x47 && byData[i - 0xf] == 0x42 && byData[i - 0xe] == 0x49 && byData[i - 0xd] == 0x58)
                {
                    PvrPixelFormat pixelFormat = (PvrPixelFormat)byData[i + 0x08];
                    PvrDataFormat dataFormat = (PvrDataFormat)byData[i + 0x09];

                    PvrPixelCodec pixelCodec = PvrPixelCodec.GetPixelCodec(pixelFormat);
                    PvrDataCodec dataCodec = PvrDataCodec.GetDataCodec(dataFormat);
                    if (dataCodec == null)
                    {
                        continue;
                    }
                    dataCodec.PixelCodec = pixelCodec;
                    int byCountPalette = 0;
                    if (pixelCodec == null && byData[i + 0x08] == 0x6)
                    {
                        byCountPalette = 4;
                    }
                    else
                    {
                        byCountPalette = pixelCodec.Bpp >> 3;
                    }

                    // 取得图片大小
                    int width = (byData[i + 0xD] << 8) | byData[i + 0xC];
                    int height = (byData[i + 0xF] << 8) | byData[i + 0xE];

                    int imgByCount = (width * height * dataCodec.Bpp / 8) + (dataCodec.PaletteEntries * byCountPalette);
                    if (imgByCount == 0 || imgByCount > byData.Length)
                    {
                        throw new Exception("图片容量异常格式 : " + imgByCount);
                    }

                    byte[] byGvrData = new byte[0x20 + imgByCount];
                    Array.Copy(byData, i - 0x10, byGvrData, 0, byGvrData.Length);

                    string imgFileInfo = Util.GetShortName(file) + "　" + (i - 0x10).ToString("x") + "--" + (i - 0x10 + byGvrData.Length).ToString("x");
                    imgList.Add(byGvrData);
                    imgInfos.Add(imgFileInfo);

                    // 取得调色板大小
                    if (dataCodec.NeedsExternalPalette)
                    {
                        int paletteCount = dataCodec.PaletteEntries * byCountPalette;
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
                        Array.Copy(byData, paletteDataStart, byPalette, 0, byPalette.Length);

                        this.paletteData.Add(imgFileInfo + " " + paletteDataStart.ToString(), byPalette);
                    }
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
            PvrTexture pvrDecode = new PvrTexture(byData);
            if (pvrDecode.NeedsExternalPalette)
            {
                // 取得调色板数据
                KeyValuePair<string, byte[]> paletteKeyValue = this.paletteData.FirstOrDefault(p => p.Key.IndexOf(fileInfo) >= 0);
                if (string.IsNullOrEmpty(paletteKeyValue.Key))
                {
                    throw new Exception("没有找到调色板数据");
                }

                PvpPalette palette = new PvpPalette(paletteKeyValue.Value);
                pvrDecode.SetPalette(palette);
            }

            return new Image[] { pvrDecode.ToBitmap() };
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
                byte[] byNewImg = Util.PaletteImageEncode(impImg, Util.GetImageFormat(this.imgFormat), paletteList, this.paletteFormat);

                // 写入数据                
                byte[] byPalette = paletteList.ToArray();
                Array.Copy(byPalette, 0, byGvpl, 0x10, byPalette.Length);

                return byNewImg;
            }
            else if (this.imgFormat == 0xe)
            {
                return Util.CmprImageEncode(impImg);
            }
            else
            {
                return Util.ImageEncode(impImg, Util.GetImageFormat(this.imgFormat));
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
            switch (this.paletteType)
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