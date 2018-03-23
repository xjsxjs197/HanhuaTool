using System.Collections.Generic;
using System.Text;

namespace Hanhua.FontEditTools
{
    /// <summary>
    /// Ngc字体相关信息
    /// </summary>
    public class NgcFontInfo
    {
        /// <summary>
        /// 字符映射表(转换后)
        /// </summary>
        /// <remarks>Key 字符在图片中的位置，Value 字符的编码</remarks>
        public List<KeyValuePair<int, int>> cmapList = new List<KeyValuePair<int,int>>();

        /// <summary>
        /// 字库所有字符
        /// </summary>
        public List<string> charList = new List<string>();

        /// <summary>
        /// 日文、字符对照表
        /// 为了给中文字符穿马甲
        /// </summary>
        public List<KeyValuePair<string, string>> charJpCnList = new List<KeyValuePair<string, string>>();

        /// <summary>
        /// 日文、字符对照表开始位置
        /// </summary>
        public int charJpCnStartPos = 0;

        /// <summary>
        /// 日文、字符对照表结束位置
        /// </summary>
        public int charJpCnEndPos = 0;

        /// <summary>
        /// 字库中所有文字图片数据
        /// </summary>
        public byte[] imgData;

        /// <summary>
        /// 一副字库图片的宽度
        /// </summary>
        public int ImageWidth;

        /// <summary>
        /// 一副字库图片的高度
        /// </summary>
        public int ImageHeight;

        /// <summary>
        /// 图片类型
        /// </summary>
        public int ImageFormat;

        /// <summary>
        /// 图片的个数
        /// </summary>
        public int TextureNum;

        /// <summary>
        /// 一副字库图片每行多少个字符
        /// </summary>
        public int CharactersPerRow;

        /// <summary>
        /// 一副字库图片每列多少个字符
        /// </summary>
        public int CharactersPerColumn;

        /// <summary>
        /// 一个字符格子的宽度
        /// </summary>
        public int CellWidth;

        /// <summary>
        /// 一个字符格子的高度
        /// </summary>
        public int CellHeight;

        /// <summary>
        /// 最原始字库文件数据
        /// </summary>
        public byte[] fileData;

        /// <summary>
        /// 各种Section信息
        /// </summary>
        public byte[] byBfnHeader = new byte[0x20];

        public byte[] byInfHeader = new byte[0x20];

        public byte[] byWidData;

        public List<byte[]> byMapData = new List<byte[]>();

        public byte[] byGlyHeader = new byte[0x20];
    }
}
