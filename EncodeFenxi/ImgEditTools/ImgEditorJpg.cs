using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Hanhua.Common;

namespace Hanhua.ImgEditTools
{
    /// <summary>
    /// Jpeg图片编辑器
    /// </summary>
    public class ImgEditorJpg : ImgEditorBase
    {
        #region " 私有变量 "

        #endregion

        /// <summary>
        /// 构造方法
        /// </summary>
        public ImgEditorJpg(string file)
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

            if (file.IndexOf("disc_chg.dat") != -1)
            {
                byte[] byJpgData = new byte[0x010420 - 0x60];
                Array.Copy(byData, 0x60, byJpgData, 0, byJpgData.Length);
                imgList.Add(byJpgData);
                imgInfos.Add(Util.GetShortName(file) + "　" + "60--10420");

                byJpgData = new byte[0x020240 - 0x010420];
                Array.Copy(byData, 0x010420, byJpgData, 0, byJpgData.Length);
                imgList.Add(byJpgData);
                imgInfos.Add(Util.GetShortName(file) + "　" + "10420--20240");

                byJpgData = new byte[0x030680 - 0x020240];
                Array.Copy(byData, 0x020240, byJpgData, 0, byJpgData.Length);
                imgList.Add(byJpgData);
                imgInfos.Add(Util.GetShortName(file) + "　" + "20240--30680");

                byJpgData = new byte[0x040540 - 0x030680];
                Array.Copy(byData, 0x030680, byJpgData, 0, byJpgData.Length);
                imgList.Add(byJpgData);
                imgInfos.Add(Util.GetShortName(file) + "　" + "30680--40540");
            }
            else
            {
                // 分析文件内部是否包括Tim文件
                byte[] byCheck = new byte[4];
                for (int i = 0; i < byData.Length - 4; i++)
                {
                    if (byData[i] == 0xFF && byData[i + 1] == 0xD8 && byData[i + 2] == 0xFF)
                    {
                        if (byData[i + 3] == 0xE0)
                        {
                            // 正常的jpeg
                            for (int j = i + 4; j < byData.Length - 2; j++)
                            {
                                if (byData[j] == 0xFF && byData[j + 1] == 0xD9)
                                {
                                    byte[] byJpgData = new byte[j + 2 - i];
                                    Array.Copy(byData, i, byJpgData, 0, byJpgData.Length);

                                    imgList.Add(byJpgData);
                                    imgInfos.Add(Util.GetShortName(file) + "　" + i.ToString("x") + "--" + (j + 2).ToString("x"));
                                    break;
                                }
                            }
                        }
                        else if (byData[i + 3] == 0xDB && file.EndsWith(".mhp", StringComparison.OrdinalIgnoreCase))
                        {
                            // bio0 mhp文件
                            byte[] byJpgData = new byte[byData.Length - i];
                            Array.Copy(byData, i, byJpgData, 0, byJpgData.Length);

                            imgList.Add(byJpgData);
                            imgInfos.Add(Util.GetShortName(file) + "　" + i.ToString("x") + "--" + byData.Length.ToString("x"));

                            break;
                        }
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
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream(byData);
                ms.Position = 0;
                Image img = Image.FromStream(ms);

                return new Image[] { img };
            }
            finally
            {
                if (ms != null)
                {
                    ms.Close();
                }
            }
        }

        /// <summary>
        /// 导出选择节点的图片
        /// </summary>
        public override void ExportSelectedImg(Image img, string fileName, byte[] byImgData)
        {
            // 开始保存文件
            File.WriteAllBytes(fileName, byImgData);
        }

        /// <summary>
        /// 导入图片
        /// </summary>
        public override byte[] ImportImg(string fileName, Image oldImg, byte[] byOldImg, string fileInfo)
        {
            Bitmap impImg = (Bitmap)Image.FromFile(fileName);

            //if (impImg.Width != oldImg.Width || impImg.Height != oldImg.Height)
            //{
            //    throw new Exception("导入图片和原图片大小不一致");
            //}

            //for (int y = 0; y < oldImg.Height; y++)
            //{
            //    for (int x = 0; x < oldImg.Width; x++)
            //    {
            //        ((Bitmap)oldImg).SetPixel(x, y, impImg.GetPixel(x, y));
            //    }
            //}

            byte[] byImgData = new byte[byOldImg.Length];
            if (!Util.EncodeJpgImage(impImg, byImgData, 73))
            {
                return null;
            }
            else
            {
                impImg.Dispose();
            }

            return byImgData;
        }

        #endregion

        #region " 私有方法 "

        #endregion
    }
}
