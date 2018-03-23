using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Hanhua.Common;

namespace Hanhua.FontEditTools
{
    /// <summary>
    /// Wii 字库信息
    /// </summary>
    public partial class FontInfo : Form
    {
        #region " 全局变量 "

        /// <summary>
        /// 字符映射表
        /// </summary>
        /// <remarks>Key 字符在图片中的位置，Value 字符的编码</remarks>
        List<KeyValuePair<int, int>> cmapList;
        List<byte[]> oldCmapList = new List<byte[]>();
        List<byte[]> oldCmapEntriesList = new List<byte[]>();
        List<string> oldCharList = new List<string>();
        private byte[] oldImgData;
        private byte[] oldFileData;
        //SortedDictionary<int, int> cmapDictionary;

        /// <summary>
        /// CWDH entries信息
        /// </summary>
        List<CwdhEntries> cwdhEntriesList;

        private RfntHeader rfntHeader = new RfntHeader();
        private byte[] byRfntHeader = new byte[0x10];

        private FinfHeader finfHeader = new FinfHeader();
        private byte[] byPinfHeader = new byte[0x20];

        private TglpHeader tglpHeader = new TglpHeader();
        private byte[] byTgplHeader = new byte[0x30];

        private CwdhSection cwdhSection = new CwdhSection();
        private byte[] byteCwdhSection = new byte[16];

        string strNewFontPath = string.Empty;

        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        public FontInfo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public FontInfo(byte[] byteData)
        {
            InitializeComponent();

            // 取得并显示各种Header信息
            this.oldFileData = byteData;
            this.oldImgData = this.SetHeadersInfo(byteData);
        }

        /// <summary>
        /// 显示各种Header信息,并返回字符Image的数据
        /// </summary>
        /// <param name="byteData"></param>
        private byte[] SetHeadersInfo(byte[] byData)
        {
            try
            {
                #region " 读取Rfnt Header "

                // 先读取Rfnt Header
                Array.Copy(byData, 0, byRfntHeader, 0, byRfntHeader.Length);
                this.rfntHeader.FileMagic = Util.GetHeaderString(byData, 0, 3);
                this.rfntHeader.Endianess = Util.GetBytesString(byData, 0x4, 0x5);
                this.rfntHeader.VersionMinor = Util.GetBytesString(byData, 0x6, 0x7);
                this.rfntHeader.LengthOfFileInBytes = Util.GetOffset(byData, 0x8, 0xb);
                this.rfntHeader.OffsetToFinfHeader = Util.GetOffset(byData, 0xc, 0xd);
                this.rfntHeader.NumberOfSections = Util.GetOffset(byData, 0xe, 0xf);

                #endregion

                #region " 获取FINF Section信息 "

                // 获取FINF Section信息
                Array.Copy(byData, this.rfntHeader.OffsetToFinfHeader, byPinfHeader, 0, byPinfHeader.Length);

                // 获取FINF Section中相关信息
                this.finfHeader.Magic = Util.GetHeaderString(byPinfHeader, 0, 3);
                this.finfHeader.LengthOfSectionInBytes = Util.GetOffset(byPinfHeader, 0x4, 0x7);
                this.finfHeader.Fonttype = Util.GetBytesString(byPinfHeader, 0x8, 0x8);
                this.finfHeader.Leading = Util.GetBytesString(byPinfHeader, 0x9, 0x9);
                this.finfHeader.Leftmargin = Util.GetOffset(byPinfHeader, 0xc, 0xc);
                this.finfHeader.CharWidth = Util.GetOffset(byPinfHeader, 0xd, 0xd);
                this.finfHeader.FullWidth = Util.GetOffset(byPinfHeader, 0xe, 0xe);
                this.finfHeader.Encoding = Util.GetOffset(byPinfHeader, 0xf, 0xf);
                this.finfHeader.DefaultChar = Util.GetStrFromNumber(Util.GetOffset(byPinfHeader, 0xa, 0xb),
                    this.finfHeader.Encoding, this.rfntHeader.Endianess);
                this.finfHeader.TglpDataOffset = Util.GetOffset(byPinfHeader, 0x10, 0x13);
                this.finfHeader.CwdhDataOffset = Util.GetOffset(byPinfHeader, 0x14, 0x17);
                this.finfHeader.CMapDataOffset = Util.GetOffset(byPinfHeader, 0x18, 0x1b);
                this.finfHeader.Height = Util.GetOffset(byPinfHeader, 0x1c, 0x1c);
                this.finfHeader.Width = Util.GetOffset(byPinfHeader, 0x1d, 0x1d);

                #endregion

                #region " 获取TGLP Header的字节信息 "

                // 获取TGLP Header的字节信息
                Array.Copy(byData, this.finfHeader.TglpDataOffset - 8, byTgplHeader, 0, byTgplHeader.Length);

                // 获取TGLP Header的具体信息
                this.tglpHeader.Magic = Util.GetHeaderString(byTgplHeader, 0, 3);
                this.tglpHeader.LengthOfSection = Util.GetOffset(byTgplHeader, 0x4, 0x7);
                this.tglpHeader.CellWidth = Util.GetOffset(byTgplHeader, 0x8, 0x8);
                this.tglpHeader.CellHeight = Util.GetOffset(byTgplHeader, 0x9, 0x9);
                this.tglpHeader.FontCharacterWidth = Util.GetOffset(byTgplHeader, 0xa, 0xa);
                this.tglpHeader.FontCharacterHeight = Util.GetOffset(byTgplHeader, 0xb, 0xb);
                this.tglpHeader.TextureSize = Util.GetOffset(byTgplHeader, 0xc, 0xf);
                this.tglpHeader.TextureNum = Util.GetOffset(byTgplHeader, 0x10, 0x11);
                this.tglpHeader.ImageFormat = Util.GetImageFormat(Util.GetOffset(byTgplHeader, 0x12, 0x13));
                this.tglpHeader.CharactersPerRow = Util.GetOffset(byTgplHeader, 0x14, 0x15);
                this.tglpHeader.CharactersPerColumn = Util.GetOffset(byTgplHeader, 0x16, 0x17);
                this.tglpHeader.ImageWidth = Util.GetOffset(byTgplHeader, 0x18, 0x19);
                this.tglpHeader.ImageHeight = Util.GetOffset(byTgplHeader, 0x1a, 0x1b);
                this.tglpHeader.PositionOfData = Util.GetOffset(byTgplHeader, 0x1c, 0x1f);

                #endregion

                #region " 取得TGLP Data信息 "

                // 取得TGLP Data信息
                byte[] byteTglpData = new byte[tglpHeader.LengthOfSection - 48];
                Array.Copy(byData, tglpHeader.PositionOfData, byteTglpData, 0, byteTglpData.Length);

                #endregion

                #region " 取得CWDH Section信息 "

                // 取得CWDH Section信息
                this.cwdhSection = new CwdhSection();
                Array.Copy(byData, this.finfHeader.CwdhDataOffset - 8, byteCwdhSection, 0, byteCwdhSection.Length);

                this.cwdhSection.Magic = Util.GetHeaderString(byteCwdhSection, 0, 3);
                this.cwdhSection.LengthOfSection = Util.GetOffset(byteCwdhSection, 4, 7);
                this.cwdhSection.NumEntries = Util.GetOffset(byteCwdhSection, 0x8, 0xb);
                this.cwdhSection.FirstCharacter = Util.GetOffset(byteCwdhSection, 0xc, 0xf);
                if (!"CWDH".Equals(this.cwdhSection.Magic))
                {
                    MessageBox.Show("错误的CWDH！");
                    return null;
                }

                #endregion

                #region " 获取CWDH entries信息 "

                this.cwdhEntriesList = new List<CwdhEntries>();
                int intCwdhEntriesPos = finfHeader.CwdhDataOffset + 8;
                for (int i = 0; i <= cwdhSection.NumEntries; i++)
                {
                    byte[] cwdhEntriesItem = new byte[3];
                    Array.Copy(byData, intCwdhEntriesPos, cwdhEntriesItem, 0, cwdhEntriesItem.Length);

                    CwdhEntries item = new CwdhEntries((sbyte)cwdhEntriesItem[0], cwdhEntriesItem[1], (sbyte)cwdhEntriesItem[2]);
                    this.cwdhEntriesList.Add(item);

                    intCwdhEntriesPos += 3;
                }
                this.cwdhEntriesList.Add(new CwdhEntries(0, 0, 0));

                #endregion

                #region " 取得CMap(字符映射表)信息 "

                this.cmapList = new List<KeyValuePair<int, int>>();
                //cmapDictionary = new SortedDictionary<int, int>();
                int cmapPos = finfHeader.CMapDataOffset - 8;
                while (cmapPos != -8)
                {
                    // 取得Cmap基本信息
                    byte[] cmapData = new byte[24];
                    Array.Copy(byData, cmapPos, cmapData, 0, cmapData.Length);

                    Cmap cmapItem = new Cmap();
                    cmapItem.Magic = Util.GetHeaderString(cmapData, 0, 3);
                    if (!"CMAP".Equals(cmapItem.Magic))
                    {
                        MessageBox.Show("错误的Cmap格式！");
                        return null;
                    }
                    cmapItem.LengthOfSection = Util.GetOffset(cmapData, 4, 7);
                    cmapItem.FirstChar = Util.GetOffset(cmapData, 8, 9);
                    cmapItem.LastChar = Util.GetOffset(cmapData, 10, 11);
                    cmapItem.CmapType = Util.GetOffset(cmapData, 12, 13);
                    cmapItem.OffsetToNextCMAPdata = Util.GetOffset(cmapData, 16, 19);
                    cmapItem.TextureEntry = Util.GetOffset(cmapData, 20, 21);
                    oldCmapList.Add(cmapData);

                    // 开始循环判断Cmap信息
                    int charIndex;
                    switch (cmapItem.CmapType)
                    {
                        case 0:
                            charIndex = cmapItem.TextureEntry;
                            for (int i = cmapItem.FirstChar; i <= cmapItem.LastChar; i++)
                            {
                                this.cmapList.Add(new KeyValuePair<int, int>(charIndex++, i));
                                //cmapDictionary.Add(charIndex++, i);
                                //cmapDictionary.Add(i, charIndex++);
                            }
                            break;

                        case 1:
                            byte[] indexByte = new byte[(cmapItem.LastChar - cmapItem.FirstChar + 1) * 2];
                            Array.Copy(byData, cmapPos + 20, indexByte, 0, indexByte.Length);
                            this.oldCmapEntriesList.Add(indexByte);

                            int[] indexEntries = new int[indexByte.Length / 2];
                            int byteIndex = 0;
                            for (int i = 0; i < indexEntries.Length; i++)
                            {
                                indexEntries[i] = Util.GetOffset(indexByte, byteIndex, byteIndex + 1);
                                byteIndex += 2;
                            }

                            int entriesIndex = 0;
                            for (int i = cmapItem.FirstChar; i <= cmapItem.LastChar; i++)
                            {
                                charIndex = indexEntries[entriesIndex++];

                                if (charIndex == 0xFFFF)
                                {
                                    this.cmapList.Add(new KeyValuePair<int, int>(charIndex, i));
                                }
                                else
                                {
                                    this.cmapList.Add(new KeyValuePair<int, int>(charIndex, i));
                                    //cmapDictionary.Add(charIndex, i);
                                    //cmapDictionary.Add(i, charIndex);
                                }
                            }
                            break;

                        case 2:
                            byte[] entriesByte = new byte[cmapItem.TextureEntry * 2 * 2];
                            Array.Copy(byData, cmapPos + 0xE + 8, entriesByte, 0, entriesByte.Length);
                            this.oldCmapEntriesList.Add(entriesByte);
                            entriesIndex = 0;
                            for (int i = 0; i < cmapItem.TextureEntry; i++)
                            {
                                this.cmapList.Add(new KeyValuePair<int, int>(
                                    Util.GetOffset(entriesByte, entriesIndex + 2, entriesIndex + 3),
                                    Util.GetOffset(entriesByte, entriesIndex, entriesIndex + 1)));

                                //cmapDictionary.Add(
                                //    CommonUtil.GetOffset(entriesByte, entriesIndex + 2, entriesIndex + 3),
                                //    CommonUtil.GetOffset(entriesByte, entriesIndex, entriesIndex + 1));
                                //cmapDictionary.Add(
                                //    (int)CommonUtil.GetOffset(entriesByte, entriesIndex, entriesIndex + 1),
                                //    (int)CommonUtil.GetOffset(entriesByte, entriesIndex + 2, entriesIndex + 3)
                                //    );
                                entriesIndex += 4;
                            }
                            break;

                        default:
                            MessageBox.Show("错误的Cmap格式！");
                            return null;
                    }

                    cmapPos = cmapItem.OffsetToNextCMAPdata - 8;
                }

                // CMap(字符映射表)信息排序
                this.cmapList.Sort(Util.Comparison);

                // 保存原来的Cmap信息
                foreach (KeyValuePair<int, int> item in this.cmapList)
                {
                    if (item.Key != 0xFFFF)
                    {
                        this.oldCharList.Add(Util.GetStrFromNumber(item.Value, this.finfHeader.Encoding, this.rfntHeader.Endianess));
                    }
                }

                #endregion

                #region " 显示信息 "

                // 显示RfntHeader信息
                this.lblEndianess.Text = this.rfntHeader.Endianess;
                this.lblVersionMinor.Text = this.rfntHeader.VersionMinor;
                this.lblLengthOfFileInBytes.Text = this.rfntHeader.LengthOfFileInBytes.ToString();
                this.lblOffsetToFinfHeader.Text = this.rfntHeader.OffsetToFinfHeader.ToString();
                this.lblNumberOfSections.Text = this.rfntHeader.NumberOfSections.ToString();

                // 显示FinfHeader信息
                this.lblEncoding.Text = Util.GetFontEncodingStr(this.finfHeader.Encoding);
                this.lblFonttype.Text = this.finfHeader.Fonttype;
                this.lblLeading.Text = this.finfHeader.Leading;
                this.lblLeftmargin.Text = this.finfHeader.Leftmargin.ToString();
                this.lblTglpDataOffset.Text = this.finfHeader.TglpDataOffset.ToString();
                this.lblCwdhDataOffset.Text = this.finfHeader.CwdhDataOffset.ToString();
                this.lblCMapDataOffset.Text = this.finfHeader.CMapDataOffset.ToString();

                // 显示TglpHeader信息
                this.lblLengthOfTglpSection.Text = this.tglpHeader.LengthOfSection.ToString();
                this.lblFontHeight.Text = this.tglpHeader.CellHeight.ToString();
                this.lblFontWidth.Text = this.tglpHeader.CellWidth.ToString();
                this.lblFontCharacterHeight.Text = this.tglpHeader.FontCharacterHeight.ToString();
                this.lblFontCharacterWidth.Text = this.tglpHeader.FontCharacterWidth.ToString();
                this.lblLengthOfOneImage.Text = this.tglpHeader.TextureSize.ToString();
                this.lblImageCount.Text = this.tglpHeader.TextureNum.ToString();
                this.lblImageFormat.Text = this.tglpHeader.ImageFormat;
                this.lblCharactersPerRow.Text = this.tglpHeader.CharactersPerRow.ToString();
                this.lblCharactersPerColumn.Text = this.tglpHeader.CharactersPerColumn.ToString();
                this.lblImageHeight.Text = this.tglpHeader.ImageHeight.ToString();
                this.lblImageWidth.Text = this.tglpHeader.ImageWidth.ToString();
                this.lblPositionOfData.Text = this.tglpHeader.PositionOfData.ToString();

                #endregion

                return byteTglpData;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n" + e.StackTrace);
                return null;
            }
        }
    }
}
