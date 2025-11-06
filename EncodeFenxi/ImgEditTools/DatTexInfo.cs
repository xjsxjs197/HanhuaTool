
namespace Hanhua.ImgEditTools
{
    /// <summary>
    /// Dat文件中纹理大小信息类
    /// </summary>
    public class DatTexInfo
    {
        /// <summary>
        /// 纹理总的宽度
        /// </summary>
        public int width { get; set; }

        /// <summary>
        /// 纹理总的高度
        /// </summary>
        public int height { get; set; }

        /// <summary>
        /// 纹理中一个Block的宽度
        /// </summary>
        public int blockWidth { get; set; }

        /// <summary>
        /// 纹理中一个Block的高度
        /// </summary>
        public int blockHeight { get; set; }
    }
}
