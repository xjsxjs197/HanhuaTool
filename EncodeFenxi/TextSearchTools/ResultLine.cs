
namespace Hanhua.Common
{
    /// <summary>
    /// 分析的结果类
    /// </summary>
    public class ResultLine
    {
        /// <summary>
        /// 分析的文件名
        /// </summary>
        private string strFileName;

        /// <summary>
        /// 分析的文件名
        /// </summary>
        public string FileName
        {
            get { return this.strFileName; }
            set { this.strFileName = value; }
        }

        /// <summary>
        /// 开始字节数
        /// </summary>
        private int intByteStartPos;

        /// <summary>
        /// 开始字节数
        /// </summary>
        public int ByteStartPos
        {
            get { return this.intByteStartPos; }
            set { this.intByteStartPos = value; }
        }

        /// <summary>
        /// 结束字节数
        /// </summary>
        private int intByteEndPos;

        /// <summary>
        /// 结束字节数
        /// </summary>
        public int ByteEndPos
        {
            get { return this.intByteEndPos; }
            set { this.intByteEndPos = value; }
        }

        /// <summary>
        /// 开始数到结束字节数所组成的字符串
        /// </summary>
        private string strJpLine;

        /// <summary>
        /// 开始数到结束字节数所组成的字符串
        /// </summary>
        public string JpLine
        {
            get { return this.strJpLine; }
            set { this.strJpLine = value; }
        }
    }
}
