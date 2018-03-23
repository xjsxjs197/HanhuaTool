using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Hanhua.Common;

namespace Hanhua.ImgEditTools
{
    /// <summary>
    /// Bio0 mhp图片编辑器
    /// </summary>
    public class ImgEditorMhp : ImgEditorBase
    {
        #region " 私有变量 "

        /// <summary>
        /// THP头的长度信息
        /// </summary>
        private const int HEADER_SIZE = 0x20;

        #endregion

        /// <summary>
        /// 构造方法
        /// </summary>
        public ImgEditorMhp(string file)
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

            // 判断MTHP
            if (byData[0] == 0x4D && byData[1] == 0x54 && byData[2] == 0x48 && byData[3] == 0x50)
            {
                // 分析文件内部是否包括Mhp文件
                byte[] byCheck = new byte[4];
                int maxLen = byData.Length - HEADER_SIZE;
                for (int i = HEADER_SIZE; i < maxLen; )
                {
                    // 判断THP
                    if (byData[i] == 0x54 && byData[i + 1] == 0x48 && byData[i + 2] == 0x50 && byData[i + 3] == 0x20)
                    {
                        // 正常的Thp
                        int startPos = Util.GetOffset(byData, i + 0x10, i + 0x13);
                        int thpSize = Util.GetOffset(byData, i + 0x4, i + 0x7);
                        byte[] byThpData = new byte[thpSize];
                        Array.Copy(byData, startPos, byThpData, 0, thpSize);

                        imgList.Add(byThpData);
                        imgInfos.Add(Util.GetShortName(file) + "　" + startPos.ToString("x") + "--" + (startPos + thpSize).ToString("x"));
                        i += HEADER_SIZE;
                    }
                    else
                    {
                        i++;
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
            // 处理成真正的Jpeg数据
            byte[] byRealJpeg = this.GetRealJpegData(byData);

            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream(byRealJpeg);
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
        /// 导入图片
        /// </summary>
        public override byte[] ImportImg(string fileName, Image oldImg, byte[] byOldImg, string fileInfo)
        {
            Bitmap impImg = (Bitmap)Image.FromFile(fileName);

            //if (impImg.Width != oldImg.Width || impImg.Height != oldImg.Height)
            //{
            //    throw new Exception("导入图片和原图片大小不一致");
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

            // 处理成Thp格式的数据
            byImgData = this.GetThpJpegData(byImgData);

            // 处理长度信息
            byte[] byThpData = new byte[byOldImg.Length];
            Array.Copy(byImgData, 0, byThpData, 0, byImgData.Length);

            return byThpData;
        }

        /// <summary>
        /// 导出选择节点的图片
        /// </summary>
        public override void ExportSelectedImg(Image img, string fileName, byte[] byImgData)
        {
            // 处理成真正的Jpeg数据
            byte[] byRealJpeg = this.GetRealJpegData(byImgData);

            // 开始保存文件
            File.WriteAllBytes(fileName, byRealJpeg);
        }

        #endregion

        #region " 私有方法 "

        /// <summary>
        /// 取得Jpeg的开始位置
        /// </summary>
        /// <param name="byJpeg"></param>
        /// <param name="lstJpeg"></param>
        /// <returns></returns>
        private int GetJpegStartPos(byte[] byJpeg, List<byte> lstJpeg)
        {
            int startPos = 0;
            int maxLen = byJpeg.Length;
            for (int i = 0; i < maxLen; i++)
            {
                if (byJpeg[i] == 0xff && byJpeg[i + 1] == 0xda && byJpeg[i + 2] == 0)
                {
                    startPos = i + 2;
                }
            }

            for (int i = 0; i < startPos; i++)
            {
                lstJpeg.Add(byJpeg[i]);
            }

            return startPos;
        }

        /// <summary>
        /// 取得Jpeg图片的结束位置
        /// </summary>
        /// <param name="byJpeg"></param>
        /// <returns></returns>
        private int GetJpegEndPos(byte[] byJpeg)
        {
            int endPos = 0;
            int maxLen = byJpeg.Length;
            for (int i = maxLen - 1; i >= 0; i--)
            {
                if (byJpeg[i] == 0xd9)
                {
                    endPos = i - 2;
                    break;
                }
            }

            return endPos;
        }

        /// <summary>
        /// 处理成Thp格式的数据
        /// </summary>
        /// <param name="byData"></param>
        /// <returns></returns>
        private byte[] GetThpJpegData(byte[] byJpeg)
        {
            List<byte> lstJpeg = new List<byte>();
            int maxLen = byJpeg.Length;
            int startPos = this.GetJpegStartPos(byJpeg, lstJpeg);
            int endPos = this.GetJpegEndPos(byJpeg);

            for (int i = startPos; i <= endPos; i++)
            {
                lstJpeg.Add(byJpeg[i]);

                if (byJpeg[i] == 0xff && byJpeg[i + 1] == 0)
                {
                    i++;
                }
            }

            lstJpeg.Add(0xff);
            lstJpeg.Add(0xd9);

            return lstJpeg.ToArray();
        }

        /// <summary>
        /// 处理成Jpeg格式的图片数据
        /// </summary>
        /// <param name="byData"></param>
        /// <returns></returns>
        private byte[] GetRealJpegData(byte[] byJpeg)
        {
            List<byte> lstJpeg = new List<byte>();
            int maxLen = byJpeg.Length;
            int startPos = this.GetJpegStartPos(byJpeg, lstJpeg);
            int endPos = this.GetJpegEndPos(byJpeg);

            for (int i = startPos; i <= endPos; i++)
            {
                lstJpeg.Add(byJpeg[i]);

                if (byJpeg[i] == 0xff)
                {
                    lstJpeg.Add(0);
                }
            }

            lstJpeg.Add(0xff);
            lstJpeg.Add(0xd9);

            return lstJpeg.ToArray();
        }

        /// <summary>
        /// Jpeg的各个Section信息
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetJpgSection()
        {
            Dictionary<string, string> jpgSection = new Dictionary<string, string>();
            jpgSection.Add("ff d8", "ff d8 ： Start");
            jpgSection.Add("ff c0", "ff c0 ： Start Of Frame（Baseline DCT）");
            jpgSection.Add("ff c2", "ff c2 ： Start Of Frame（Progressive DCT）");
            jpgSection.Add("ff c4", "ff c4 ： Define Huffman Table(s)");
            jpgSection.Add("ff db", "ff db ： Define Quantization Table(s)");
            jpgSection.Add("ff dd", "ff dd ： Define Restart Interval");
            jpgSection.Add("ff da", "ff da ： Start Of Scan");
            jpgSection.Add("ff d0", "ff d0 ： Restart");
            jpgSection.Add("ff d1", "ff d1 ： Restart");
            jpgSection.Add("ff d2", "ff d2 ： Restart");
            jpgSection.Add("ff d3", "ff d3 ： Restart");
            jpgSection.Add("ff d4", "ff d4 ： Restart");
            jpgSection.Add("ff d5", "ff d5 ： Restart");
            jpgSection.Add("ff d6", "ff d6 ： Restart");
            jpgSection.Add("ff d7", "ff d7 ： Restart");
            jpgSection.Add("ff fe", "ff fe ： Comment");
            jpgSection.Add("ff d9", "ff d9 ： End Of Image");

            return jpgSection;
        }

        #endregion
    }
}