using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using VrSharp.GvrTexture;
using Hanhua.ImgEditTools;
using Hanhua.Common;

namespace Hanhua.ImgEditTools
{
    /// <summary>
    /// Ngc Tpl图片编辑器
    /// </summary>
    public class ImgEditorTpl : ImgEditorBase
    {
        #region " 私有变量 "

        /// <summary>
        /// Tpl文件共通
        /// </summary>
        private TplFileManager tplUtil = new TplFileManager();

        /// <summary>
        /// Tpl图片Header信息
        /// </summary>
        private List<ImageHeader> tplImageInfo = new List<ImageHeader>();

        #endregion

        /// <summary>
        /// 构造方法
        /// </summary>
        public ImgEditorTpl(string file)
            : base(file)
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
            List<int> tplPosList = new List<int>();

            byte[] byTplData = this.tplUtil.IsTplFile(file, tplPosList);
            if (byTplData != null)
            {
                byte[] byTpl;

                for (int i = 0; i < tplPosList.Count; i++)
                {
                    try
                    {
                        int tplSize = this.tplUtil.GetTplSize(byTplData, tplPosList[i]);
                        if (i < tplPosList.Count - 1)
                        {
                            if (tplPosList[i] + tplSize > tplPosList[i + 1])
                            {
                                tplSize = tplPosList[i + 1] - tplPosList[i];
                            }
                        }
                        else
                        {
                            if (tplPosList[i] + tplSize >= byTplData.Length)
                            {
                                tplSize = byTplData.Length - tplPosList[i];
                            }
                        }

                        byTpl = new byte[tplSize];
                        Array.Copy(byTplData, tplPosList[i], byTpl, 0, byTpl.Length);

                        string imgFileInfo = Util.GetShortName(file) + "　" + tplPosList[i].ToString("x") + "--" + (tplPosList[i] + byTpl.Length).ToString("x");

                        imgList.Add(byTpl);
                        imgInfos.Add(imgFileInfo);

                    }
                    catch (Exception me)
                    {
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
            tplImageInfo.Clear();
            return tplUtil.TplDecode(byData, tplImageInfo);
        }

        /// <summary>
        /// 设置编辑器的Title
        /// </summary>
        /// <param name="newTitle"></param>
        public override string GetEditorTitle(Image img)
        {
            return this.editingFile + " " + this.tplImageInfo[this.imageIndex].Format + " W：" + (img == null ? 0 : img.Width) + " H：" + (img == null ? 0 : img.Height);
        }

        /// <summary>
        /// 导入图片
        /// </summary>
        public override byte[] ImportImg(string fileName, Image oldImg, byte[] byOldImg, string fileInfo)
        {
            ImageHeader headerInfo = this.tplImageInfo[this.imageIndex];
            Bitmap importImg = new Bitmap(fileName);
            importImg.RotateFlip(this.rotateFlipType);

            byte[] byImg;
            List<byte> byPalette = new List<byte>();

            if (headerInfo.Format.Equals("C4_CI4")
                || headerInfo.Format.Equals("C8_CI8")
                || headerInfo.Format.Equals("C14X2_CI14x2"))
            {
                byImg = Util.PaletteImageEncode(importImg, headerInfo.Format, byPalette, headerInfo.PaletteFormat);
            }
            else
            {
                byImg = Util.ImageEncode(importImg, headerInfo.Format);
            }

            // 替换Tpl文件数据
            return tplUtil.TplImgImport(byOldImg, this.imageIndex, byImg, byPalette.ToArray());
        }

        #endregion

        #region " 私有方法 "

        #endregion
    }
}