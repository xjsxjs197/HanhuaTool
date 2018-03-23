using System;
using System.Collections.Generic;

namespace Hanhua.Common
{
    /// <summary>
    /// 文件、地址信息类
    /// </summary>
    public class FilePosInfo
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// 是否是目录
        /// </summary>
        public bool IsFolder { get; set; }

        /// <summary>
        /// 目录的级别
        /// </summary>
        public int FolderIndex { get; set; }

        /// <summary>
        /// 文件的位置
        /// </summary>
        public int FilePos { get; set; }

        /// <summary>
        /// 文件的大小
        /// </summary>
        public int FileSize { get; set; }

        /// <summary>
        /// 汉化文本开始、结束位置
        /// </summary>
        public string[] PosInfo { get; set; }

        /// <summary>
        /// 汉化文本开始位置
        /// </summary>
        public int TextCopyStart { get; set; }

        /// <summary>
        /// 汉化文本开始位置
        /// </summary>
        public int TextStart { get; set; }

        /// <summary>
        /// 汉化文本结束位置
        /// </summary>
        public int TextEnd { get; set; }

        /// <summary>
        /// 文本最大长度
        /// </summary>
        public int MaxLen { get; set; }

        /// <summary>
        /// Entry开始位置
        /// </summary>
        public int EntryPos { get; set; }

        /// <summary>
        /// 记录文本的Entry信息
        /// </summary>
        public List<int> TextEntrys { get; set; }

        /// <summary>
        /// 同一文件时，内部编号
        /// </summary>
        public string SubIndex { get; set; }

        /// <summary>
        /// 同一文件内的子文件名称
        /// </summary>
        public string SubName { get; set; }

        /// <summary>
        /// Bio0文本类型
        /// </summary>
        public Bio0TextType TextType { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="file"></param>
        /// <param name="posInfo"></param>
        public FilePosInfo(string file, string[] posInfo)
        {
            this.File = file;
            this.PosInfo = posInfo;
            this.TextEntrys = new List<int>();

            this.TextStart = Convert.ToInt32(posInfo[0], 16);
            this.TextEnd = Convert.ToInt32(posInfo[1], 16);
            this.TextCopyStart = this.TextStart;
            if (posInfo.Length > 2) 
            {
                this.EntryPos = Convert.ToInt32(posInfo[2], 16);
                if (this.EntryPos < this.TextStart)
                {
                    this.TextCopyStart = this.EntryPos;
                }
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="file"></param>
        /// <param name="posInfo"></param>
        public FilePosInfo(string file)
        {
            this.File = file;
            this.TextEntrys = new List<int>();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public FilePosInfo(string fileName, bool isFolder, int folderIndex)
        {
            this.File = fileName;
            this.IsFolder = isFolder;
            this.FolderIndex = folderIndex;
            this.TextEntrys = new List<int>();
        }
    }
}
