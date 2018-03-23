using System;

namespace Hanhua.Common
{
    /// <summary>
    /// RFNT Header 
    /// </summary>
    public class RfntHeader
    {
        /// <summary>
        /// File magic, always 'RFNT' in ASCII 
        /// </summary>
        public string FileMagic { get; set; }

        /// <summary>
        /// Endianess (always Big-Endian (FEFF)
        /// </summary>
        public string Endianess { get; set; }

        /// <summary>
        /// Version Minor 
        /// </summary>
        public string VersionMinor { get; set; }

        /// <summary>
        /// Length of file in bytes
        /// </summary>
        public int LengthOfFileInBytes { get; set; }

        /// <summary>
        /// Offset to the beginning of the FINF header  
        /// </summary>
        public int OffsetToFinfHeader { get; set; }

        /// <summary>
        /// Number of sections 
        /// </summary>
        public int NumberOfSections { get; set; }
    }

    /// <summary>
    /// FINF Header 
    /// </summary>
    public class FinfHeader
    {
        /// <summary>
        /// Magic, always 'FINF' in ASCII  
        /// </summary>
        public string Magic { get; set; }

        /// <summary>
        /// Length of section in bytes
        /// </summary>
        public int LengthOfSectionInBytes { get; set; }

        /// <summary>
        /// Fonttype (Exact meanings unknown)
        /// </summary>
        public string Fonttype { get; set; }

        /// <summary>
        /// Leading (Space between lines) unsure 
        /// </summary>
        public string Leading { get; set; }

        /// <summary>
        /// Char returned for exceptions 
        /// </summary>
        public string DefaultChar { get; set; }

        /// <summary>
        /// Leftmargin 
        /// </summary>
        public int Leftmargin { get; set; }

        /// <summary>
        /// char width
        /// </summary>
        public int CharWidth { get; set; }

        /// <summary>
        /// full width
        /// </summary>
        public int FullWidth { get; set; }

        /// <summary>
        /// Encoding  In order - UTF-8, UTF-16, SJIS, CP1252, COUNT
        /// </summary>
        public int Encoding { get; set; }

        /// <summary>
        /// X = TGLP data offset (0x38) 
        /// </summary>
        public int TglpDataOffset { get; set; }

        /// <summary>
        /// Y = CWDH data offset (X + TLGP size) 
        /// </summary>
        public int CwdhDataOffset { get; set; }

        /// <summary>
        /// CMAP data offset (Y + CWDH size)
        /// </summary>
        public int CMapDataOffset { get; set; }

        /// <summary>
        /// Height 
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Width 
        /// </summary>
        public int Width { get; set; }
    }

    /// <summary>
    /// TGLP Header 
    /// </summary>
    public class TglpHeader
    {
        /// <summary>
        /// Magic, always 'TGLP'  in ASCII  
        /// </summary>
        public string Magic { get; set; }

        /// <summary>
        /// DWORD length of 'TGLP' section 
        /// </summary>
        public int LengthOfSection { get; set; }

        /// <summary>
        /// BYTE font width - 1
        /// </summary>
        public int CellWidth { get; set; }

        /// <summary>
        /// BYTE font height  - 1
        /// </summary>
        public int CellHeight { get; set; }

        /// <summary>
        /// BYTE character width - 1
        /// </summary>
        public int FontCharacterWidth { get; set; }

        /// <summary>
        /// BYTE character height  - 1
        /// </summary>
        public int FontCharacterHeight { get; set; }

        /// <summary>
        /// DWORD length of 1 image (texture Size) 
        /// </summary>
        public int TextureSize { get; set; }

        /// <summary>
        /// WORD images count (texture Number)
        /// </summary>
        public int TextureNum { get; set; }

        /// <summary>
        /// WORD image format  
        /// </summary>
        public string ImageFormat { get; set; }

        /// <summary>
        /// characters per row
        /// </summary>
        public int CharactersPerRow { get; set; }

        /// <summary>
        /// characters per row
        /// </summary>
        public int CharactersPerColumn { get; set; }

        /// <summary>
        /// Height Of image 
        /// </summary>
        public int ImageHeight { get; set; }

        /// <summary>
        /// Width Of image 
        /// </summary>
        public int ImageWidth { get; set; }

        /// <summary>
        /// position of data 
        /// </summary>
        public int PositionOfData { get; set; }
    }

    /// <summary>
    /// CWDH Section  
    /// </summary>
    public class CwdhSection
    {
        /// <summary>
        /// magic, always 'CWDH' in ASCII 
        /// </summary>
        public string Magic { get; set; }

        /// <summary>
        /// DWORD length of this section
        /// </summary>
        public int LengthOfSection { get; set; }

        /// <summary>
        /// Num Entries  
        /// </summary>
        public int NumEntries { get; set; }

        /// <summary>
        /// DWORD first character ? (= 0)   
        /// </summary>
        public int FirstCharacter { get; set; }
    }

    /// <summary>
    /// CWDH entries
    /// </summary>
    public class CwdhEntries
    {
        public CwdhEntries(int unknown1, int unknown2, int unknown3)
        {
            this.Unknown1 = unknown1;
            this.Unknown2 = unknown2;
            this.Unknown3 = unknown3;
        }

        /// <summary>
        /// unknown1
        /// </summary>
        public int Unknown1 { get; set; }

        /// <summary>
        /// unknown2
        /// </summary>
        public int Unknown2 { get; set; }

        /// <summary>
        /// unknown3
        /// </summary>
        public int Unknown3 { get; set; }
    }

    /// <summary>
    /// 字符映射表
    /// </summary>
    public class Cmap
    {
        /// <summary>
        /// magic, always 'CMAP' in ASCII 
        /// </summary>
        public string Magic { get; set; }

        /// <summary>
        /// DWORD length of this section
        /// </summary>
        public int LengthOfSection { get; set; }

        /// <summary>
        /// denotes the starting bound of all chars in the char map (inclusive)
        /// 字符映射表里所有字符的第一个字符
        /// </summary>
        public int FirstChar { get; set; }

        /// <summary>
        /// denotes the ending bound of all chars in the char map (inclusive)
        /// 字符映射表里所有字符的最后一个字符
        /// </summary>
        public int LastChar { get; set; }

        /// <summary>
        /// 0 is a sequential range, 1 is list for non-contiguous mappings
        /// 0代表是连续的映射，1代表不是连续的
        /// </summary>
        public int CmapType { get; set; }

        /// <summary>
        /// 下一个字符映射表的偏移
        /// </summary>
        public int OffsetToNextCMAPdata { get; set; }

        /// <summary>
        ///  If type is 0, sequential after this mark. If type is one, each character in the range will have an entry
        /// </summary>
        public int TextureEntry { get; set; }
    }

    /// <summary>
    /// RARC文件的Node
    /// </summary>
    public class RarcNode
    {
        /// <summary>
        /// nodeType
        /// </summary>
        public int NodeType { get; set; }

        /// <summary>
        /// fileNameOffset
        /// </summary>
        public int FileNameOffset { get; set; }

        /// <summary>
        /// fileNameOffset
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// file type
        /// </summary>
        public string FileType { get; set; }

        /// <summary>
        /// file data
        /// </summary>
        public byte[] FileData { get; set; }

        /// <summary>
        /// numFileEntries,
        /// </summary>
        public int NumFileEntries { get; set; }

        /// <summary>
        /// firstFileEntriesOffset
        /// </summary>
        public int FirstFileEntriesOffset { get; set; }

        /// <summary>
        /// fileEntriesOffset
        /// </summary>
        public int FileEntriesOffset { get; set; }

        /// <summary>
        /// stringTablePos
        /// </summary>
        public int StringTablePos { get; set; }

        /// <summary>
        /// dataStartPos
        /// </summary>
        public int DataStartPos { get; set; }
    }

    /// <summary>
    /// Tres文件中的字符串节点信息
    /// </summary>
    public class TresSection
    {
        /// <summary>
        /// 当前字符串的位置（是第几个字符串）
        /// </summary>
        public int StrNum { get; set; }

        /// <summary>
        /// 未知1
        /// </summary>
        public int Padding1 { get; set; }

        /// <summary>
        /// 未知2
        /// </summary>
        public int Padding2 { get; set; }

        /// <summary>
        /// 当前字符串的开始地址（相对于文件开头）
        /// </summary>
        public int StrOffset { get; set; }

        /// <summary>
        /// 当前字符串相对的下一个的开始地址（相对于文件开头）
        /// 和声音有关的文件
        /// </summary>
        public int StrNextOffset { get; set; }

        /// <summary>
        /// 文本
        /// </summary>
        public string Text { get; set; }
    }
}
