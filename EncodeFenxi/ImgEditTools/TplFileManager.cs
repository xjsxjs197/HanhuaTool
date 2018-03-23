using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Hanhua.Common;
using Hanhua.FileViewer;

namespace Hanhua.ImgEditTools
{
    /// <summary>
    /// Tpl Header
    /// </summary>
    public class TplFileHeader
    {
        /// <summary>
        /// File magic
        /// </summary>
        private string strFileMagic;

        /// <summary>
        /// File magic
        /// </summary>
        public string FileMagic
        {
            get { return this.strFileMagic; }
            set { this.strFileMagic = value; }
        }

        /// <summary>
        /// Number of images
        /// </summary>
        private int intNumberOfImages;

        /// <summary>
        /// Number of images
        /// </summary>
        public int NumberOfImages
        {
            get { return this.intNumberOfImages; }
            set { this.intNumberOfImages = value; }
        }

        /// <summary>
        /// Offset of the Image Table
        /// </summary>
        private int intOffsetOfImageTable;

        /// <summary>
        /// Offset of the Image Table
        /// </summary>
        public int OffsetOfImageTable
        {
            get { return this.intOffsetOfImageTable; }
            set { this.intOffsetOfImageTable = value; }
        }
    }

    /// <summary>
    /// Image Offset Table 
    /// </summary>
    public class ImageOffsetTable
    {
        /// <summary>
        /// Offset of image header
        /// </summary>
        private int intOffsetOfImageHeader;

        /// <summary>
        /// Offset of image header
        /// </summary>
        public int OffsetOfImageHeader
        {
            get { return this.intOffsetOfImageHeader; }
            set { this.intOffsetOfImageHeader = value; }
        }

        /// <summary>
        /// Offset of palette header 
        /// </summary>
        private int intOffsetOfPaletteHeader;

        /// <summary>
        /// Offset of palette header 
        /// </summary>
        public int OffsetOfPaletteHeader
        {
            get { return this.intOffsetOfPaletteHeader; }
            set { this.intOffsetOfPaletteHeader = value; }
        }
    }

    /// <summary>
    /// Palette Header 
    /// </summary>
    public class PaletteHeader
    {
        /// <summary>
        /// Entry Count 
        /// </summary>
        private int intEntryCount;

        /// <summary>
        /// Entry Count  
        /// </summary>
        public int EntryCount
        {
            get { return this.intEntryCount; }
            set { this.intEntryCount = value; }
        }

        /// <summary>
        /// Unknown  
        /// </summary>
        private int intUnknown;

        /// <summary>
        /// Unknown  
        /// </summary>
        public int Unknown
        {
            get { return this.intUnknown; }
            set { this.intUnknown = value; }
        }

        /// <summary>
        /// Palette Format   
        /// </summary>
        private int intPaletteFormat;

        /// <summary>
        /// Palette Format   
        /// </summary>
        public int PaletteFormat
        {
            get { return this.intPaletteFormat; }
            set { this.intPaletteFormat = value; }
        }

        /// <summary>
        /// Palette Data Address   
        /// </summary>
        private int intPaletteDataAddress;

        /// <summary>
        /// Palette Data Address    
        /// </summary>
        public int PaletteDataAddress
        {
            get { return this.intPaletteDataAddress; }
            set { this.intPaletteDataAddress = value; }
        }
    }

    /// <summary>
    /// Image Header  
    /// </summary>
    public class ImageHeader 
    {
        /// <summary>
        /// Height   
        /// </summary>
        private int intHeight;

        /// <summary>
        /// Height     
        /// </summary>
        public int Height 
        {
            get { return this.intHeight; }
            set { this.intHeight = value; }
        }

        /// <summary>
        /// Height   
        /// </summary>
        private int intWidth;

        /// <summary>
        /// Height     
        /// </summary>
        public int Width 
        {
            get { return this.intWidth; }
            set { this.intWidth = value; }
        }

        /// <summary>
        /// Format    
        /// </summary>
        private string intFormat;

        /// <summary>
        /// Format      
        /// </summary>
        public string Format 
        {
            get { return this.intFormat; }
            set { this.intFormat = value; }
        }

        /// <summary>
        /// paletteFormat    
        /// </summary>
        private int paletteFormat;

        /// <summary>
        /// PaletteFormat      
        /// </summary>
        public int PaletteFormat
        {
            get { return this.paletteFormat; }
            set { this.paletteFormat = value; }
        }

        /// <summary>
        /// PaletteData
        /// </summary>
        public byte[] PaletteData { get; set; }

        /// <summary>
        /// Image Data Address     
        /// </summary>
        private int intImageDataAddress ;

        /// <summary>
        /// Image Data Address       
        /// </summary>
        public int ImageDataAddress
        {
            get { return this.intImageDataAddress; }
            set { this.intImageDataAddress = value; }
        }
    }

    /// <summary>
    /// Tpl 文件编辑
    /// </summary>
    public class TplFileManager
    {
        /// <summary>
        /// 当前Tpl文件的大小
        /// </summary>
        public int TplFileSize { get; set; }

        /// <summary>
        /// 取得Tpl文件的图像
        /// </summary>
        /// <param name="strFile"></param>
        /// <returns></returns>
        public Image[] GetTplFileImage(string strFile, List<ImageHeader> tplImageInfo)
        {
            FileStream fs = null;

            try
            {
                // 将文件中的数据，循环读取到byData中
                fs = new FileStream(strFile, FileMode.Open);
                byte[] byData = new byte[fs.Length];
                fs.Read(byData, 0, byData.Length);
                fs.Close();
                fs = null;

                return this.TplDecode(byData, tplImageInfo);
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
                return null;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// 取得Tpl信息
        /// </summary>
        /// <param name="byData"></param>
        /// <param name="tplImageInfo"></param>
        /// <returns></returns>
        public Image[] TplDecode(byte[] byData, List<ImageHeader> tplImageInfo)
        {
            this.TplFileSize = 0;

            // File Header 
            TplFileHeader tplFileHeader = new TplFileHeader();
            tplFileHeader.FileMagic = Util.GetHeaderString(byData, 0x0, 0x3);
            tplFileHeader.NumberOfImages = Util.GetOffset(byData, 0x4, 0x7);
            tplFileHeader.OffsetOfImageTable = Util.GetOffset(byData, 0x8, 0xB);

            // 循环处理
            List<Image> imageList = new List<Image>();
            int intImageOffsetTablePos = 0xC;
            for (int i = 0; i < tplFileHeader.NumberOfImages; i++)
            {
                // 取得Image Header信息
                int intOffsetOfImageHeader = Util.GetOffset(byData, intImageOffsetTablePos, intImageOffsetTablePos + 3);
                int intOffsetOfPaletteHeader = Util.GetOffset(byData, intImageOffsetTablePos + 4, intImageOffsetTablePos + 7);
                intImageOffsetTablePos += 8;

                int imageHeight = Util.GetOffset(byData, intOffsetOfImageHeader, intOffsetOfImageHeader + 1);
                int imageWidth = Util.GetOffset(byData, intOffsetOfImageHeader + 2, intOffsetOfImageHeader + 3);
                int imageFormat = Util.GetOffset(byData, intOffsetOfImageHeader + 4, intOffsetOfImageHeader + 7);
                int imageDataAddress = Util.GetOffset(byData, intOffsetOfImageHeader + 8, intOffsetOfImageHeader + 11);

                ImageHeader headerItem = new ImageHeader();
                tplImageInfo.Add(headerItem);
                headerItem.Width = imageWidth;
                headerItem.Height = imageHeight;
                headerItem.Format = Util.GetImageFormat(imageFormat);

                // 取得图像
                Image image;
                byte[] imageData = new byte[Util.GetImageByteCount(imageHeight, imageWidth, headerItem.Format)];
                Array.Copy(byData, imageDataAddress, imageData, 0, imageData.Length);

                // 设置Tpl大小
                if (i == tplFileHeader.NumberOfImages - 1)
                {
                    // 取得图像
                    this.TplFileSize = imageDataAddress + imageData.Length;

                }

                if (imageFormat == 0x08
                    || imageFormat == 0x09
                    || imageFormat == 0x0A)
                {
                    // C4_CI4、C8_CI8、C14X2_CI14x2
                    // 取得模板信息
                    int paletteEntryCount = Util.GetOffset(byData, intOffsetOfPaletteHeader, intOffsetOfPaletteHeader + 1);
                    int paletteUnknown = Util.GetOffset(byData, intOffsetOfPaletteHeader + 2, intOffsetOfPaletteHeader + 3);
                    int paletteFormat = Util.GetOffset(byData, intOffsetOfPaletteHeader + 4, intOffsetOfPaletteHeader + 7);
                    int paletteDataAddress = Util.GetOffset(byData, intOffsetOfPaletteHeader + 8, intOffsetOfPaletteHeader + 11);
                    byte[] byPalette = new byte[paletteEntryCount * 2];
                    Array.Copy(byData, paletteDataAddress, byPalette, 0, byPalette.Length);
                    headerItem.PaletteData = byPalette;

                    image = Util.PaletteImageDecode(
                        new Bitmap(imageWidth, imageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb),
                        imageData,
                        Util.GetImageFormat(imageFormat),
                        byPalette,
                        paletteFormat);

                    headerItem.PaletteFormat = paletteFormat;
                }
                else if (imageFormat == 0x0e)
                {
                    // CMPR
                    image = Util.CmprImageDecode(
                        new Bitmap(imageWidth, imageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb),
                        imageData);
                }
                else
                {
                    // 通常类型
                    image = Util.ImageDecode(
                        new Bitmap(imageWidth, imageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb),
                        imageData,
                        Util.GetImageFormat(imageFormat));
                }

                imageList.Add(image);
            }

            return imageList.ToArray();
        }

        /// <summary>
        /// 导入图片到Tpl文件
        /// </summary>
        /// <param name="tplFile"></param>
        /// <param name="rowIndex"></param>
        /// <param name="byImage"></param>
        public byte[] TplImgImport(byte[] byData, int rowIndex, byte[] byImage, byte[] byPalette)
        {
            try
            {
                // File Header 
                TplFileHeader tplFileHeader = new TplFileHeader();
                tplFileHeader.FileMagic = Util.GetHeaderString(byData, 0x0, 0x3);
                tplFileHeader.NumberOfImages = Util.GetOffset(byData, 0x4, 0x7);
                tplFileHeader.OffsetOfImageTable = Util.GetOffset(byData, 0x8, 0xB);

                // 循环处理
                int intImageOffsetTablePos = 0xC;
                for (int i = 0; i < tplFileHeader.NumberOfImages; i++)
                {
                    // 取得Image Header信息
                    int intOffsetOfImageHeader = Util.GetOffset(byData, intImageOffsetTablePos, intImageOffsetTablePos + 3);
                    int intOffsetOfPaletteHeader = Util.GetOffset(byData, intImageOffsetTablePos + 4, intImageOffsetTablePos + 7);
                    intImageOffsetTablePos += 8;

                    int imageHeight = Util.GetOffset(byData, intOffsetOfImageHeader, intOffsetOfImageHeader + 1);
                    int imageWidth = Util.GetOffset(byData, intOffsetOfImageHeader + 2, intOffsetOfImageHeader + 3);
                    int imageFormat = Util.GetOffset(byData, intOffsetOfImageHeader + 4, intOffsetOfImageHeader + 7);
                    int imageDataAddress = Util.GetOffset(byData, intOffsetOfImageHeader + 8, intOffsetOfImageHeader + 11);

                    // 取得图像
                    byte[] imageData = new byte[Util.GetImageByteCount(imageHeight, imageWidth, Util.GetImageFormat(imageFormat))];
                    Array.Copy(byData, imageDataAddress, imageData, 0, imageData.Length);

                    if (rowIndex == i)
                    {
                        //// 由于C4_CI4目前无法正确编码，暂时将C4_CI4各式转成CMPR各式
                        //if (imageFormat == 0x08)
                        //{
                        //    imageFormat = 0x0e;
                        //    byData[intOffsetOfImageHeader + 7] = 0x0e;
                        //}

                        if (imageFormat == 0x08
                            || imageFormat == 0x09
                            || imageFormat == 0x0A)
                        {
                            // C4_CI4、C8_CI8、C14X2_CI14x2
                            // 取得模板信息
                            int paletteDataAddress = Util.GetOffset(byData, intOffsetOfPaletteHeader + 8, intOffsetOfPaletteHeader + 11);
                            Array.Copy(byImage, 0, byData, imageDataAddress, byImage.Length);
                            Array.Copy(byPalette, 0, byData, paletteDataAddress, byPalette.Length);
                        }
                        else
                        {
                            // 通常类型、CMPR
                            Array.Copy(byImage, 0, byData, imageDataAddress, byImage.Length);
                            return byData;
                        }
                    }
                }

                return byData;
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
                return byData;
            }
        }

        /// <summary>
        /// 从文件中找到所有Tpl信息
        /// </summary>
        /// <param name="file"></param>
        public void FindTplFromFile(string file)
        {
            if (file.EndsWith(".tpl"))
            {
                // 将文件中的数据，循环读取到byData中
                byte[] byData = File.ReadAllBytes(file);

                List<ImageHeader> tplImageInfo = new List<ImageHeader>();
                Image[] tplImages = new TplFileManager().TplDecode(byData, tplImageInfo);

                ImageViewer frmTplImage = new ImageViewer(tplImages, tplImageInfo, byData);
                frmTplImage.baseFile = file;
                frmTplImage.Show();
            }
            else
            {
                List<FilePosInfo> fileNameInfo = new List<FilePosInfo>();
                fileNameInfo.Add(new FilePosInfo(file, false, 0));

                this.SearchTplInfo(fileNameInfo, file);
            }
        }

        /// <summary>
        /// 从文件中查询所有Tpl信息
        /// </summary>
        /// <param name="fileNameInfo"></param>
        private void SearchTplInfo(List<FilePosInfo> fileNameInfo, string folder)
        {
            TreeNode root = new TreeNode();
            root.Text = folder;

            foreach (FilePosInfo item in fileNameInfo)
            {
                if (item.IsFolder)
                {
                    continue;
                }

                if (item.File.EndsWith(".tpl", StringComparison.CurrentCultureIgnoreCase))
                {
                    // 如果是Tpl文件，直接追加
                    root.Nodes.Add(this.GetTplNodeType1(item.File));
                }
                else
                {
                    // 如果不是Tpl文件结尾，查找是否包括Tpl信息
                    List<int> tplList = new List<int>();
                    byte[] byData = this.IsTplFile(item.File, tplList);
                    if (byData != null)
                    {
                        root.Nodes.Add(this.GetTplNodeType2(item.File, byData, tplList));
                    }
                }
            }

            SzsViewer szsViewForm = new SzsViewer(root, null, null);
            szsViewForm.Show();
        }

        /// <summary>
        /// 追加Tpl节点
        /// </summary>
        private TreeNode GetTplNodeType1(string file)
        {
            // 生成数据节点
            RarcNode fileInfo = new RarcNode();
            fileInfo.FileData = this.GetFileByte(file);
            fileInfo.FileType = "tpl";
            fileInfo.FileName = file;
            fileInfo.DataStartPos = 0;

            // 将数据节点绑定到Tree节点上
            TreeNode childTreeNode = new TreeNode();
            childTreeNode.Text = file;
            childTreeNode.Tag = fileInfo;

            return childTreeNode;
        }

        /// <summary>
        /// 追加Tpl节点
        /// </summary>
        private TreeNode GetTplNodeType2(string file, byte[] byData, List<int> tplList)
        {
            TreeNode fartherNode = new TreeNode();
            fartherNode.Text = file;
            byte[] byTpl;

            for (int i = 0; i < tplList.Count; i++)
            {
                try
                {
                    int tplSize = this.GetTplSize(byData, tplList[i]);
                    if (i < tplList.Count - 1)
                    {
                        if (tplList[i] + tplSize > tplList[i + 1])
                        {
                            tplSize = tplList[i + 1] - tplList[i];
                        }
                    }
                    else
                    {
                        if (tplList[i] + tplSize >= byData.Length)
                        {
                            tplSize = byData.Length - tplList[i];
                        }
                    }

                    byTpl = new byte[tplSize];
                    Array.Copy(byData, tplList[i], byTpl, 0, byTpl.Length);

                    // 生成数据节点
                    RarcNode fileInfo = new RarcNode();
                    fileInfo.FileData = byTpl;
                    fileInfo.FileName = file;
                    fileInfo.FileType = "tpl";
                    fileInfo.DataStartPos = tplList[i];

                    // 将数据节点绑定到Tree节点上
                    TreeNode childTreeNode = new TreeNode();
                    childTreeNode.Text = "0x" + tplList[i].ToString("x") + " -- " + "0x" + (tplList[i] + byTpl.Length).ToString("x");                    
                    childTreeNode.Tag = fileInfo;

                    fartherNode.Nodes.Add(childTreeNode);
                }
                catch (Exception me)
                {
                    MessageBox.Show(me.Message + "\n" + me.StackTrace);
                }
            }

            return fartherNode;
        }

        /// <summary>
        /// 获得文件的字节数据
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private byte[] GetFileByte(string file)
        {
            // 将文件中的数据，读取到byData中
            FileStream fs = new FileStream(file, FileMode.Open);
            byte[] byData = new byte[fs.Length];
            fs.Read(byData, 0, byData.Length);
            fs.Close();
            fs = null;

            return byData;
        }

        /// <summary>
        /// 查找Tpl信息
        /// </summary>
        /// <param name="file">当前文件</param>
        /// <param name="tplList">返回的Tpl信息位置</param>
        /// <returns>返回的文件字节数据</returns>
        public byte[] IsTplFile(string file, List<int> tplList)
        {
            // 将文件中的数据，读取到byData中
            byte[] byData = this.GetFileByte(file);

            // Tpl文件的标识数据
            byte[] keyBytes = new byte[4];
            keyBytes[0] = 0x00;
            keyBytes[1] = 0x20;
            keyBytes[2] = 0xAF;
            keyBytes[3] = 0x30;

            // 二进制检索
            bool findedKey = true;
            int maxLen = byData.Length - 40;

            for (int j = 0; j < maxLen; j++)
            {
                if (byData[j] == keyBytes[0])
                {
                    findedKey = true;
                    for (int i = 1; i < keyBytes.Length; i++)
                    {
                        if (byData[j + i] != keyBytes[i])
                        {
                            findedKey = false;
                            break;
                        }
                    }

                    if (findedKey)
                    {
                        tplList.Add(j);
                    }
                }
            }

            if (tplList.Count == 0)
            {
                return null;
            }
            else
            {
                return byData;
            }
        }

        /// <summary>
        /// 取得Tpl图片的大小信息
        /// </summary>
        /// <param name="byData"></param>
        /// <param name="startPos"></param>
        /// <returns></returns>
        public int GetTplSize(byte[] byData, int startPos)
        {
            int tplFileSize = 0;

            // File Header 
            TplFileHeader tplFileHeader = new TplFileHeader();
            tplFileHeader.NumberOfImages = Util.GetOffset(byData, startPos + 0x4, startPos + 0x7);
            tplFileHeader.OffsetOfImageTable = Util.GetOffset(byData, startPos + 0x8, startPos + 0xB);

            // 循环处理
            List<Image> imageList = new List<Image>();
            int intImageOffsetTablePos = startPos + 0xC;
            for (int i = 0; i < tplFileHeader.NumberOfImages; i++)
            {
                // 取得Image Header信息
                int intOffsetOfImageHeader = startPos + Util.GetOffset(byData, intImageOffsetTablePos, intImageOffsetTablePos + 3);
                int intOffsetOfPaletteHeader = startPos + Util.GetOffset(byData, intImageOffsetTablePos + 4, intImageOffsetTablePos + 7);
                intImageOffsetTablePos += 8;

                int imageHeight = Util.GetOffset(byData, intOffsetOfImageHeader, intOffsetOfImageHeader + 1);
                int imageWidth = Util.GetOffset(byData, intOffsetOfImageHeader + 2, intOffsetOfImageHeader + 3);
                int imageFormat = Util.GetOffset(byData, intOffsetOfImageHeader + 4, intOffsetOfImageHeader + 7);
                int imageDataAddress = Util.GetOffset(byData, intOffsetOfImageHeader + 8, intOffsetOfImageHeader + 11);

                if (i == tplFileHeader.NumberOfImages - 1)
                {
                    // 取得图像
                    tplFileSize = imageDataAddress + Util.GetImageByteCount(imageHeight, imageWidth, Util.GetImageFormat(imageFormat));

                }
            }

            return tplFileSize;
        }
    }
}
