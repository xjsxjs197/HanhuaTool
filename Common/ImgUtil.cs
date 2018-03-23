using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Hanhua.Common
{
    /// <summary>
    /// 图片处理共通
    /// </summary>
    public class ImgUtil
    {
        /// <summary>
        /// 取得字库列表
        /// </summary>
        /// <returns>字库列表</returns>
        public static List<KeyValuePair<string, string>> GetFontList()
        {
            List<KeyValuePair<string, string>> fontList = new List<KeyValuePair<string, string>>();
            string[] fontInfo = File.ReadAllLines(@"..\Common\FontList.txt");
            foreach (string fontTxt in fontInfo)
            {
                string[] fontRow = fontTxt.Split('　');
                fontList.Add(new KeyValuePair<string, string>(fontRow[0], fontRow[1]));
            }

            return fontList;
        }

        /// <summary>
        /// 将文字写入小块图片
        /// </summary>
        /// <param name="imgInfo">写文字需要的信息</param>
        public static void WriteBlockImg(ImgInfo imgInfo)
        {
            GraphicsPath graphPath = new GraphicsPath();
            graphPath.AddString(imgInfo.CharTxt, 
                new FontFamily(imgInfo.FontName),
                (int)imgInfo.FontStyle, 
                imgInfo.FontSize,
                new Rectangle(imgInfo.PosX + imgInfo.XPadding, 
                    imgInfo.PosY + imgInfo.YPadding,
                    imgInfo.BlockImgW - imgInfo.XPadding, 
                    imgInfo.BlockImgH - imgInfo.YPadding), 
                imgInfo.Sf);
            imgInfo.Grp.FillPath(imgInfo.Brush, graphPath);
            if (imgInfo.NeedBorder)
            {
                imgInfo.Grp.DrawPath(imgInfo.Pen, graphPath);
            }
        }

        /// <summary>
        /// 写字库图片
        /// </summary>
        /// <param name="imgInfo">图片信息</param>
        /// <param name="txtList">文字信息</param>
        /// <returns>写好文字的图片</returns>
        public static Bitmap WriteFontImg(ImgInfo imgInfo, List<string> txtList)
        {
            // 参数检查
            ImgUtil.ImgInfoCheck(imgInfo);

            // 计算行、列信息
            int xNum = imgInfo.ImgW / imgInfo.BlockImgW;
            int yNum = imgInfo.ImgH / imgInfo.BlockImgH;
            int maxCharCount = xNum * yNum;
            for (int i = 0; i < maxCharCount; i++)
            {
                // 设置当前字符
                imgInfo.CharTxt = txtList[i];

                // 计算位置
                imgInfo.PosX = (i % xNum);
                imgInfo.PosY = (i / xNum);

                // 生成当前块图片
                ImgUtil.WriteBlockImg(imgInfo);
            }

            return imgInfo.Bmp;
        }

        /// <summary>
        /// 写整行文字的图片
        /// </summary>
        /// <param name="imgInfo">图片信息</param>
        /// <param name="txtList">文字信息</param>
        /// <returns>写好文字的图片</returns>
        public static Bitmap WriteLineTxtImg(ImgInfo imgInfo, string txt)
        {
            // 参数检查
            ImgUtil.ImgInfoCheck(imgInfo);

            // 拆分字符
            char[] txtList = txt.ToCharArray();

            for (int i = 0; i < txtList.Length; i++)
            {
                // 设置当前字符
                imgInfo.CharTxt = txtList[i].ToString();

                // 计算当前位置
                imgInfo.PosX += imgInfo.BlockImgW;

                // 生成当前块图片
                ImgUtil.WriteBlockImg(imgInfo);
            }

            return imgInfo.Bmp;
        }

        /// <summary>
        /// 参数检查
        /// </summary>
        /// <param name="imgInfo">图片参数</param>
        private static void ImgInfoCheck(ImgInfo imgInfo)
        { 
            if (imgInfo.ImgW == 0
                || imgInfo.ImgH == 0
                || imgInfo.BlockImgW == 0
                || imgInfo.BlockImgH == 0
                || imgInfo.BlockImgW > imgInfo.ImgW
                || imgInfo.BlockImgH > imgInfo.ImgH)
            {
                throw new Exception("图片的宽、高信息不正确！");
            }
        }
    }
}
