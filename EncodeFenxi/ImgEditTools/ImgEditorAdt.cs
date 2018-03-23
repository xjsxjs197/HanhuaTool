using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Threading;

namespace Hanhua.ImgEditTools
{
    /// <summary>
    /// Ps Adt压缩图片编辑器
    /// </summary>
    public class ImgEditorAdt : ImgEditorBase
    {
        #region " 私有变量 "

        #endregion

        /// <summary>
        /// 构造方法
        /// </summary>
        public ImgEditorAdt(string file)
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
            // 解压Adt文件
            System.Diagnostics.Process exep = new System.Diagnostics.Process();
            exep.StartInfo.FileName = @".\AdtDec.exe";
            exep.StartInfo.CreateNoWindow = true;
            exep.StartInfo.UseShellExecute = false;
            string timFile = file.ToLower().Replace(".adt", ".tim");
            if (File.Exists(timFile))
            {
                File.Delete(timFile);
            }

            exep.StartInfo.Arguments = file + " " + timFile;
            exep.Start();
            exep.WaitForExit();

            int loopCount = 0;
            while (!File.Exists(timFile) && loopCount < 7)
            {
                Thread.Sleep(500);
                loopCount++;
            }

            if (!File.Exists(timFile))
            {
                return null;
            }

            ImgEditorTim timEditor = new ImgEditorTim(timFile);
            return timEditor.SearchImg(File.ReadAllBytes(timFile), timFile, imgInfos);
        }

        /// <summary>
        /// 从图片数据中获取图片
        /// </summary>
        /// <param name="byData"></param>
        /// <returns></returns>
        public override Image[] ImageDecode(byte[] byData, string fileInfo)
        {
            ImgEditorTim timEditor = new ImgEditorTim(fileInfo);
            return timEditor.ImageDecode(byData, fileInfo);
        }

        /// <summary>
        /// 导入图片
        /// </summary>
        public override byte[] ImportImg(string fileName, Image oldImg, byte[] byOldImg, string fileInfo)
        {
            ImgEditorTim timEditor = new ImgEditorTim(fileInfo);
            return timEditor.ImportImg(fileName, oldImg, byOldImg, fileInfo);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="byData"></param>
        /// <param name="fileInfo"></param>
        public override void Save(string fileName, byte[] byData, string fileInfo)
        {
            base.Save(fileName, byData, fileInfo);

            // 压缩Tim图片到Adt格式
            System.Diagnostics.Process exep = new System.Diagnostics.Process();
            exep.StartInfo.FileName = @".\AdtCom.exe";
            exep.StartInfo.CreateNoWindow = true;
            exep.StartInfo.UseShellExecute = false;

            exep.StartInfo.Arguments = "e " + fileName + " " + fileName.ToLower().Replace(".tim", ".adt");
            exep.Start();
            exep.WaitForExit();
        }

        #endregion

        #region " 私有方法 "

        #endregion
    }
}