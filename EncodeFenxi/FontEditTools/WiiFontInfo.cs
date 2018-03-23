using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Hanhua.Common;

namespace Hanhua.FontEditTools
{
    /// <summary>
    /// Wii字体相关信息
    /// </summary>
    public class WiiFontInfo
    {
        /// <summary>
        /// 字库编码格式
        /// </summary>
        public Encoding encoding;

        /// <summary>
        /// 字符映射表(转换后)
        /// </summary>
        /// <remarks>Key 字符在图片中的位置，Value 字符的编码</remarks>
        public List<KeyValuePair<int, int>> cmapList = new List<KeyValuePair<int,int>>();

        /// <summary>
        /// 字符映射表(转换前)
        /// </summary>
        public List<byte[]> byCmapList = new List<byte[]>();

        /// <summary>
        /// 字符映射表(转换前)
        /// </summary>
        public List<byte[]> byCmapEntriesList = new List<byte[]>();

        /// <summary>
        /// 字库所有字符
        /// </summary>
        public List<string> charList = new List<string>();

        /// <summary>
        /// 字符映射（Code，日文）
        /// </summary>
        public List<KeyValuePair<string, string>> codeCharMap = new List<KeyValuePair<string, string>>();

        /// <summary>
        /// 字库中所有文字图片数据
        /// </summary>
        public byte[] imgData;

        /// <summary>
        /// 最原始字库文件数据
        /// </summary>
        public byte[] fileData;

        /// <summary>
        /// CWDH entries信息(转换后)
        /// </summary>
        public List<CwdhEntries> cwdhEntriesList = new List<CwdhEntries>();

        /// <summary>
        /// 各种Header信息
        /// </summary>
        public RfntHeader rfntHeader = new RfntHeader();
        public byte[] byRfntHeader = new byte[0x10];

        public FinfHeader finfHeader = new FinfHeader();
        public byte[] byPinfHeader = new byte[0x20];

        public TglpHeader tglpHeader = new TglpHeader();
        public byte[] byTgplHeader = new byte[0x30];

        public CwdhSection cwdhSection = new CwdhSection();
        public byte[] byCwdhSection = new byte[16];
    }
}
