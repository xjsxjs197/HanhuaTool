using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Hanhua.Common
{
    public partial class TexTestView : Form
    {
        private string basePath = @"D:\WiiStationDebug\TexTest\";

        public TexTestView(string[] logLines)
        {
            InitializeComponent();

            this.ChkLogTex(logLines);
        }

        private void ChkLogTex(string[] logLines)
        {
            this.gdvLog.Rows.Clear();

            int lineIdx = 0;
            int imgCount = 0;
            int errImgCnt = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append(this.Text).Append(" (");
            Dictionary<int, Bitmap> allTexImg = this.GetAllTexImg();
            Bitmap screenImg = new Bitmap(320, 240);
            using (Graphics g = Graphics.FromImage(screenImg))
            {
                g.FillRectangle(Brushes.Black, 0, 0, screenImg.Width, screenImg.Height);
            }
            foreach (string logText in logLines)
            {
                if (string.Empty.Equals(logText))
                {
                    continue;
                }

                int rowIndex = this.gdvLog.Rows.Add();
                this.gdvLog.Rows[rowIndex].Cells["noCol"].Value = (rowIndex + 1);
                this.gdvLog.Rows[rowIndex].Cells["textCol"].Value = logText;

                if ((logText.StartsWith("primSprtS") || logText.StartsWith("primSprt16") || logText.StartsWith("primSprt8")) && logLines[lineIdx + 1].StartsWith("XY"))
                {
                    string[] xyInfo = logLines[lineIdx + 1].Replace("XY ", "").Replace("(", "").Replace(")", "").Split(' ');
                    string[] texInfo = logLines[lineIdx + 2].Replace("TEX ", "").Replace("(", "").Replace(")", "").Split(' ');
                    string[] texIdxInfo = logLines[lineIdx + 3].Replace("draw ", "").Split(' ');
                    int texImgIdx = Convert.ToInt32(texIdxInfo[texIdxInfo.Length - 1]);
                    //if (texImgIdx == 6 && allTexImg.ContainsKey(texImgIdx))
                    if (allTexImg.ContainsKey(texImgIdx))
                    {
                        Bitmap subTexImg = this.GetSubTexImg(allTexImg[texImgIdx]
                            , Convert.ToInt32(texInfo[0])
                            , Convert.ToInt32(texInfo[2])
                            , Convert.ToInt32(texInfo[1])
                            , Convert.ToInt32(texInfo[5]));
                        if (subTexImg != null)
                        {
                            screenImg = (Bitmap)screenImg.Clone();
                            this.CopySubTexImgToScreenImg(subTexImg, screenImg
                                , Convert.ToInt32(xyInfo[0])
                                , Convert.ToInt32(xyInfo[2])
                                , Convert.ToInt32(xyInfo[1])
                                , Convert.ToInt32(xyInfo[5])
                                , Convert.ToInt32(texIdxInfo[3]));

                            this.gdvLog.Rows[rowIndex].Cells["imgCol"].Value = screenImg;
                            imgCount++;

                            this.gdvLog.Rows[rowIndex].Cells["textCol"].Style.BackColor = Color.Pink;
                            this.gdvLog.Rows[rowIndex].Height = 240;
                        }
                        else
                        {
                            errImgCnt++;
                            sb.Append(rowIndex + 1).Append(" ");

                            Bitmap curTexImg = allTexImg[texImgIdx];
                            this.gdvLog.Rows[rowIndex].Cells["imgCol"].Value = curTexImg;
                            imgCount++;

                            this.gdvLog.Rows[rowIndex].Cells["textCol"].Style.BackColor = Color.Yellow;
                            this.gdvLog.Rows[rowIndex].Height = curTexImg.Height;

                            this.gdvLog.Rows[rowIndex].Cells["textCol"].Value = logText + " ( " + curTexImg.Width + "  " + curTexImg.Height + " )";
                        }
                    }
                }

                lineIdx++;
            }

            sb.Append(")").Append(" 错误的图片个数：").Append(errImgCnt);
            this.Text = sb.ToString();
        }

        private Dictionary<int, Bitmap> GetAllTexImg()
        {
            List<FilePosInfo> allTexFiles = Util.GetAllFiles(this.basePath).Where(p => !p.IsFolder && p.File.EndsWith(".png")).ToList();
            Dictionary<int, Bitmap> allTexImg = new Dictionary<int, Bitmap>();

            foreach (FilePosInfo fileInfo in allTexFiles)
            {
                int texIdx = Convert.ToInt32(fileInfo.File.Substring(fileInfo.File.Length - 10, 2)); // txtDebug_256_256_12.bin.png
                allTexImg.Add(texIdx, new Bitmap(fileInfo.File));
            }

            return allTexImg;
        }

        private Bitmap GetSubTexImg(Bitmap curTexImg, int x1, int x2, int y1, int y2)
        {
            if ((x2 - x1 + 1) > curTexImg.Width || (y2 - y1 + 1) > curTexImg.Height)
            {
                return null;
            }

            Rectangle cropRect = new Rectangle(x1, y1, x2 - x1 + 1, y2 - y1 + 1);
            return curTexImg.Clone(cropRect, curTexImg.PixelFormat);
        }

        private void CopySubTexImgToScreenImg(Bitmap subTexImg, Bitmap screenImg, int x1, int x2, int y1, int y2, int blendType)
        {
            using (Graphics g = Graphics.FromImage(screenImg))
            {
                switch (blendType)
                {
                    case 1:
                        this.Blend1(screenImg, subTexImg, new Point(x1, y1));
                        break;

                    case 2:
                        this.Blend2(screenImg, subTexImg, new Point(x1, y1));
                        break;

                    case 3:
                        this.Blend3(screenImg, subTexImg, new Point(x1, y1));
                        break;

                    case 4:
                        this.Blend4(screenImg, subTexImg, new Point(x1, y1));
                        break;

                    default:
                        // 直接将小图绘制到大图的指定区域
                        g.DrawImage(subTexImg, new Rectangle(x1, y1, subTexImg.Width, subTexImg.Height));
                        break;
                }
            }
        }

        /// <summary>
        /// 0.5F + 0.5B
        /// </summary>
        /// <param name="largeBitmap"></param>
        /// <param name="smallBitmap"></param>
        /// <param name="position"></param>
        private unsafe void Blend1(Bitmap largeBitmap, Bitmap smallBitmap, Point position)
        {
            // 锁定位图数据（获取直接内存访问权限）
            BitmapData largeData = largeBitmap.LockBits(
                new Rectangle(0, 0, largeBitmap.Width, largeBitmap.Height),
                ImageLockMode.ReadWrite,  // 读写模式
                PixelFormat.Format32bppArgb);  // 32位ARGB格式

            BitmapData smallData = smallBitmap.LockBits(
                new Rectangle(0, 0, smallBitmap.Width, smallBitmap.Height),
                ImageLockMode.ReadOnly,  // 只读模式
                PixelFormat.Format32bppArgb);

            try
            {
                int bytesPerPixel = 4; // 每个像素4字节（ARGB）
                int largeStride = largeData.Stride; // 大图每行字节数（含填充）
                int smallStride = smallData.Stride; // 小图每行字节数

                // 获取位图数据的起始指针
                byte* largePtr = (byte*)largeData.Scan0;
                byte* smallPtr = (byte*)smallData.Scan0;

                // 遍历小图的每一行
                for (int y = 0; y < smallBitmap.Height; y++)
                {
                    // 计算小图当前行对应在大图中的Y坐标
                    int largeY = position.Y + y;

                    // 边界检查：如果超出大图范围则跳过
                    if (largeY >= largeBitmap.Height) break;

                    // 获取当前行的起始指针
                    byte* largeRow = largePtr + (largeY * largeStride);
                    byte* smallRow = smallPtr + (y * smallStride);

                    // 遍历当前行的每个像素
                    for (int x = 0; x < smallBitmap.Width; x++)
                    {
                        // 计算小图当前像素对应在大图中的X坐标
                        int largeX = position.X + x;

                        // 边界检查：如果超出大图范围则跳过
                        if (largeX >= largeBitmap.Width) break;

                        // 计算像素位置（每个像素4字节）
                        int largePos = largeX * bytesPerPixel;
                        int smallPos = x * bytesPerPixel;

                        // 加法混合每个颜色通道（B、G、R）
                        // 注意：跳过Alpha通道（第4个字节）
                        for (int i = 0; i < 3; i++)
                        {
                            // 将两个像素的通道值相加，并限制最大值255
                            int sum = (largeRow[largePos + i] >> 1) + (smallRow[smallPos + i] >> 1);
                            //largeRow[largePos + i] = (byte)(sum > 255 ? 255 : sum);
                            largeRow[largePos + i] = (byte)(sum);
                        }

                        // Alpha通道保持不变（i=3时跳过）
                    }
                }
            }
            finally
            {
                // 解锁位图数据（必须执行，否则资源泄漏）
                largeBitmap.UnlockBits(largeData);
                smallBitmap.UnlockBits(smallData);
            }
        }

        /// <summary>
        /// 1.0F + 1.0B
        /// </summary>
        /// <param name="largeBitmap"></param>
        /// <param name="smallBitmap"></param>
        /// <param name="position"></param>
        private unsafe void Blend2(Bitmap largeBitmap, Bitmap smallBitmap, Point position)
        {
            // 锁定位图数据（获取直接内存访问权限）
            BitmapData largeData = largeBitmap.LockBits(
                new Rectangle(0, 0, largeBitmap.Width, largeBitmap.Height),
                ImageLockMode.ReadWrite,  // 读写模式
                PixelFormat.Format32bppArgb);  // 32位ARGB格式

            BitmapData smallData = smallBitmap.LockBits(
                new Rectangle(0, 0, smallBitmap.Width, smallBitmap.Height),
                ImageLockMode.ReadOnly,  // 只读模式
                PixelFormat.Format32bppArgb);

            try
            {
                int bytesPerPixel = 4; // 每个像素4字节（ARGB）
                int largeStride = largeData.Stride; // 大图每行字节数（含填充）
                int smallStride = smallData.Stride; // 小图每行字节数

                // 获取位图数据的起始指针
                byte* largePtr = (byte*)largeData.Scan0;
                byte* smallPtr = (byte*)smallData.Scan0;

                // 遍历小图的每一行
                for (int y = 0; y < smallBitmap.Height; y++)
                {
                    // 计算小图当前行对应在大图中的Y坐标
                    int largeY = position.Y + y;

                    // 边界检查：如果超出大图范围则跳过
                    if (largeY >= largeBitmap.Height) break;

                    // 获取当前行的起始指针
                    byte* largeRow = largePtr + (largeY * largeStride);
                    byte* smallRow = smallPtr + (y * smallStride);

                    // 遍历当前行的每个像素
                    for (int x = 0; x < smallBitmap.Width; x++)
                    {
                        // 计算小图当前像素对应在大图中的X坐标
                        int largeX = position.X + x;

                        // 边界检查：如果超出大图范围则跳过
                        if (largeX >= largeBitmap.Width) break;

                        // 计算像素位置（每个像素4字节）
                        int largePos = largeX * bytesPerPixel;
                        int smallPos = x * bytesPerPixel;

                        // 加法混合每个颜色通道（B、G、R）
                        // 注意：跳过Alpha通道（第4个字节）
                        for (int i = 0; i < 3; i++)
                        {
                            // 将两个像素的通道值相加，并限制最大值255
                            int sum = largeRow[largePos + i] + smallRow[smallPos + i];
                            largeRow[largePos + i] = (byte)(sum > 255 ? 255 : sum);
                        }

                        // Alpha通道保持不变（i=3时跳过）
                    }
                }
            }
            finally
            {
                // 解锁位图数据（必须执行，否则资源泄漏）
                largeBitmap.UnlockBits(largeData);
                smallBitmap.UnlockBits(smallData);
            }
        }

        /// <summary>
        /// B - F
        /// </summary>
        /// <param name="largeBitmap"></param>
        /// <param name="smallBitmap"></param>
        /// <param name="position"></param>
        private unsafe void Blend3(Bitmap largeBitmap, Bitmap smallBitmap, Point position)
        {
            // 锁定位图数据（获取直接内存访问权限）
            BitmapData largeData = largeBitmap.LockBits(
                new Rectangle(0, 0, largeBitmap.Width, largeBitmap.Height),
                ImageLockMode.ReadWrite,  // 读写模式
                PixelFormat.Format32bppArgb);  // 32位ARGB格式

            BitmapData smallData = smallBitmap.LockBits(
                new Rectangle(0, 0, smallBitmap.Width, smallBitmap.Height),
                ImageLockMode.ReadOnly,  // 只读模式
                PixelFormat.Format32bppArgb);

            try
            {
                int bytesPerPixel = 4; // 每个像素4字节（ARGB）
                int largeStride = largeData.Stride; // 大图每行字节数（含填充）
                int smallStride = smallData.Stride; // 小图每行字节数

                // 获取位图数据的起始指针
                byte* largePtr = (byte*)largeData.Scan0;
                byte* smallPtr = (byte*)smallData.Scan0;

                // 遍历小图的每一行
                for (int y = 0; y < smallBitmap.Height; y++)
                {
                    // 计算小图当前行对应在大图中的Y坐标
                    int largeY = position.Y + y;

                    // 边界检查：如果超出大图范围则跳过
                    if (largeY >= largeBitmap.Height) break;

                    // 获取当前行的起始指针
                    byte* largeRow = largePtr + (largeY * largeStride);
                    byte* smallRow = smallPtr + (y * smallStride);

                    // 遍历当前行的每个像素
                    for (int x = 0; x < smallBitmap.Width; x++)
                    {
                        // 计算小图当前像素对应在大图中的X坐标
                        int largeX = position.X + x;

                        // 边界检查：如果超出大图范围则跳过
                        if (largeX >= largeBitmap.Width) break;

                        // 计算像素位置（每个像素4字节）
                        int largePos = largeX * bytesPerPixel;
                        int smallPos = x * bytesPerPixel;

                        // 加法混合每个颜色通道（B、G、R）
                        // 注意：跳过Alpha通道（第4个字节）
                        for (int i = 0; i < 3; i++)
                        {
                            // 将两个像素的通道值相加，并限制最大值255
                            int sum = largeRow[largePos + i] - smallRow[smallPos + i];
                            largeRow[largePos + i] = (byte)(sum < 0 ? 0 : sum);
                        }

                        // Alpha通道保持不变（i=3时跳过）
                    }
                }
            }
            finally
            {
                // 解锁位图数据（必须执行，否则资源泄漏）
                largeBitmap.UnlockBits(largeData);
                smallBitmap.UnlockBits(smallData);
            }
        }

        /// <summary>
        /// 0.25F + 1.0B
        /// </summary>
        /// <param name="largeBitmap"></param>
        /// <param name="smallBitmap"></param>
        /// <param name="position"></param>
        private unsafe void Blend4(Bitmap largeBitmap, Bitmap smallBitmap, Point position)
        {
            // 锁定位图数据（获取直接内存访问权限）
            BitmapData largeData = largeBitmap.LockBits(
                new Rectangle(0, 0, largeBitmap.Width, largeBitmap.Height),
                ImageLockMode.ReadWrite,  // 读写模式
                PixelFormat.Format32bppArgb);  // 32位ARGB格式

            BitmapData smallData = smallBitmap.LockBits(
                new Rectangle(0, 0, smallBitmap.Width, smallBitmap.Height),
                ImageLockMode.ReadOnly,  // 只读模式
                PixelFormat.Format32bppArgb);

            try
            {
                int bytesPerPixel = 4; // 每个像素4字节（ARGB）
                int largeStride = largeData.Stride; // 大图每行字节数（含填充）
                int smallStride = smallData.Stride; // 小图每行字节数

                // 获取位图数据的起始指针
                byte* largePtr = (byte*)largeData.Scan0;
                byte* smallPtr = (byte*)smallData.Scan0;

                // 遍历小图的每一行
                for (int y = 0; y < smallBitmap.Height; y++)
                {
                    // 计算小图当前行对应在大图中的Y坐标
                    int largeY = position.Y + y;

                    // 边界检查：如果超出大图范围则跳过
                    if (largeY >= largeBitmap.Height) break;

                    // 获取当前行的起始指针
                    byte* largeRow = largePtr + (largeY * largeStride);
                    byte* smallRow = smallPtr + (y * smallStride);

                    // 遍历当前行的每个像素
                    for (int x = 0; x < smallBitmap.Width; x++)
                    {
                        // 计算小图当前像素对应在大图中的X坐标
                        int largeX = position.X + x;

                        // 边界检查：如果超出大图范围则跳过
                        if (largeX >= largeBitmap.Width) break;

                        // 计算像素位置（每个像素4字节）
                        int largePos = largeX * bytesPerPixel;
                        int smallPos = x * bytesPerPixel;

                        // 加法混合每个颜色通道（B、G、R）
                        // 注意：跳过Alpha通道（第4个字节）
                        for (int i = 0; i < 3; i++)
                        {
                            // 将两个像素的通道值相加，并限制最大值255
                            int sum = largeRow[largePos + i] + (smallRow[smallPos + i] >> 2);
                            largeRow[largePos + i] = (byte)(sum > 255 ? 255 : sum);
                        }

                        // Alpha通道保持不变（i=3时跳过）
                    }
                }
            }
            finally
            {
                // 解锁位图数据（必须执行，否则资源泄漏）
                largeBitmap.UnlockBits(largeData);
                smallBitmap.UnlockBits(smallData);
            }
        }

    }
}
