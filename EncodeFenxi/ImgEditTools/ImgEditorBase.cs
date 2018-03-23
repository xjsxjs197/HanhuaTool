using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace Hanhua.ImgEditTools
{
    /// <summary>
    /// 图片编辑基类
    /// </summary>
    public class ImgEditorBase
    {
        /// <summary>
        /// 当前编辑的文件
        /// </summary>
        public string editingFile { get; set; }

        /// <summary>
        /// 当前调色板Index
        /// </summary>
        public int paletteIndex { get; set; }

        /// <summary>
        /// 调色板个数
        /// </summary>
        public int paletteCount { get; set; }

        /// <summary>
        /// 是否是替换整个文件
        /// </summary>
        public bool isReplaceFile { get; set; }

        /// <summary>
        /// 当前图片的Index
        /// </summary>
        public int imageIndex { get; set; }

        /// <summary>
        /// 图片的反转信息
        /// </summary>
        public RotateFlipType rotateFlipType { get; set; }

        /// <summary>
        /// 构造方法
        /// </summary>
        public ImgEditorBase(string file)
        {
            this.editingFile = file;
        }

        #region " 子类可以重写的虚方法 "

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Reset()
        { 
        }

        /// <summary>
        /// 设置编辑器的Title
        /// </summary>
        /// <param name="newTitle"></param>
        public virtual string GetEditorTitle(Image img)
        {
            if (img != null)
            {
                return this.editingFile + " W：" + img.Width + " H：" + img.Height;
            }
            else
            {
                return this.editingFile;
            }
        }

        /// <summary>
        /// 查找当前类型的图片
        /// </summary>
        /// <param name="byData">当前打开文件的字节数据</param>
        /// <param name="file">当前文件</param>
        /// <param name="imgInfos">查找到的图片的信息</param>
        /// <returns>是否查找成功</returns>
        public virtual List<byte[]> SearchImg(byte[] byData, string file, List<string> imgInfos)
        {
            return new List<byte[]>();
        }

        /// <summary>
        /// 从图片数据中获取图片
        /// </summary>
        /// <param name="byData"></param>
        /// <returns></returns>
        public virtual Image[] ImageDecode(byte[] byData, string fileInfo)
        {
            return null;
        }

        /// <summary>
        /// 导出选择节点的图片
        /// </summary>
        public virtual void ExportSelectedImg(Image img, string fileName, byte[] byImgData)
        {
            img.RotateFlip(this.rotateFlipType);

            // 开始保存文件
            img.Save(fileName);
        }

        /// <summary>
        /// 导入图片
        /// </summary>
        public virtual byte[] ImportImg(string fileName, Image oldImg, byte[] byOldImg, string fileInfo)
        {
            throw new Exception("导入图片逻辑未实现");
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="byData"></param>
        /// <param name="fileInfo"></param>
        public virtual void Save(string fileName, byte[] byData, string fileInfo)
        {
            File.WriteAllBytes(fileName, byData);
        }

        #endregion
    }
}
