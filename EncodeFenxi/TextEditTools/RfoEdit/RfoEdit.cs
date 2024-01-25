using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Hanhua.Common.TextEditTools.RfoEdit
{
    public partial class RfoEdit : Form
    {
        private string baseFolder = @"G:\Study\MySelfProject\Hanhua\TodoCn\Rfo\";
        private int fontImgCnt = 26;

        public RfoEdit()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 符文工房字库做成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateFont_Click(object sender, EventArgs e)
        {
            int imgWH = 256;
            byte[] by00530 = File.ReadAllBytes(this.baseFolder + @"00530Old.bin");
            byte[] byNewFont = new byte[0x20 + 0x20 + 0x10 + (imgWH * imgWH / 2 + 0x20 + 0x10) * this.fontImgCnt];

            // set header
            Array.Copy(by00530, byNewFont, 0x20);
            byNewFont[0xf] = (byte)(this.fontImgCnt);
            int footerStart = byNewFont.Length - 0x20;
            int footerEnd = footerStart + 0x20;
            byNewFont[0x18] = (byte)((footerStart >> 24) & 0xff);
            byNewFont[0x19] = (byte)((footerStart >> 16) & 0xff);
            byNewFont[0x1a] = (byte)((footerStart >> 8) & 0xff);
            byNewFont[0x1b] = (byte)((footerStart >> 0) & 0xff);
            byNewFont[0x1c] = (byte)((footerEnd >> 24) & 0xff);
            byNewFont[0x1d] = (byte)((footerEnd >> 16) & 0xff);
            byNewFont[0x1e] = (byte)((footerEnd >> 8) & 0xff);
            byNewFont[0x1f] = (byte)((footerEnd >> 0) & 0xff);

            // font info
            byte[] byFontInfo = new byte[] { 0x46, 0x4f, 0x4e, 0x54, 0x30, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                             0xCA, 0x00, 0xD1, 0x4F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
            // font img pos info
            byte[] byFontImgPos = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x80, 0x00 };

            for (int i = 0; i < this.fontImgCnt; i++)
            {
                int tmpFontPos = 0x20 + i * 0x20;
                int tmpImgPos = 0x20 + this.fontImgCnt * 0x20 + i * 0x10;

                Array.Copy(byFontInfo, 0, byNewFont, tmpFontPos, byFontInfo.Length);
                Array.Copy(byFontImgPos, 0, byNewFont, tmpImgPos, byFontImgPos.Length);

                // font info
                if (i <= 9)
                {
                    byNewFont[tmpFontPos + 4] = (byte)(0x30 + i);

                    byNewFont[tmpFontPos + 0x13] = (byte)(0x4F + i);
                }
                else if (i >= 10 && i <= 19)
                {
                    byNewFont[tmpFontPos + 4] = (byte)(0x31);
                    byNewFont[tmpFontPos + 5] = (byte)(0x30 + i - 10);

                    byNewFont[tmpFontPos + 0x10] = (byte)(0x1A);
                    byNewFont[tmpFontPos + 0x11] = (byte)(0x70);
                    byNewFont[tmpFontPos + 0x12] = (byte)(0x04);
                    byNewFont[tmpFontPos + 0x13] = (byte)(i - 10);
                }
                else
                {
                    byNewFont[tmpFontPos + 4] = (byte)(0x32);
                    byNewFont[tmpFontPos + 5] = (byte)(0x30 + i - 20);

                    byNewFont[tmpFontPos + 0x10] = (byte)(0x1F);
                    byNewFont[tmpFontPos + 0x11] = (byte)(0x70);
                    byNewFont[tmpFontPos + 0x12] = (byte)(0x04);
                    byNewFont[tmpFontPos + 0x13] = (byte)(i - 10);
                }

                byNewFont[tmpFontPos + 0x16] = (byte)((tmpImgPos >> 8) & 0xff);
                byNewFont[tmpFontPos + 0x17] = (byte)((tmpImgPos >> 0) & 0xff);

                // font img pos info
                int fontInfoPos = 0x20 + 0x10 + (0x20 + 0x10) * this.fontImgCnt + i * (imgWH * imgWH / 2);
                byNewFont[tmpImgPos + 0x0] = (byte)((fontInfoPos >> 24) & 0xff);
                byNewFont[tmpImgPos + 0x1] = (byte)((fontInfoPos >> 16) & 0xff);
                byNewFont[tmpImgPos + 0x2] = (byte)((fontInfoPos >> 8) & 0xff);
                byNewFont[tmpImgPos + 0x3] = (byte)((fontInfoPos >> 0) & 0xff);

                // set image data
                byte[] byAddedImgData = Util.ImageEncode((Bitmap)(Bitmap.FromFile(this.baseFolder + @"Font\cnimportNew\tmpCnFont" + i + ".png")), "I4");
                Array.Copy(byAddedImgData, 0, byNewFont, fontInfoPos, byAddedImgData.Length);
            }

            // set footer
            Array.Copy(by00530, 0x983c0, byNewFont, byNewFont.Length - 0x20, 0x20);

            File.WriteAllBytes(this.baseFolder + @"\00530.bin", byNewFont);
        }

        /// <summary>
        /// 符文工房字库图片做成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFontImg_Click(object sender, EventArgs e)
        {
            // 生成所有BigEndian的中文字符
            string[] allFontChar = File.ReadAllLines(this.baseFolder + @"fontChar.txt", Encoding.UTF8);
            List<string> lstBuf = new List<string>();
            lstBuf.AddRange(allFontChar);

            ImgInfo imgInfo = new ImgInfo(256, 256);
            imgInfo.BlockImgH = 23;
            imgInfo.BlockImgW = 23;
            imgInfo.NeedBorder = false;
            imgInfo.FontStyle = FontStyle.Regular;
            imgInfo.FontSize = 19;
            imgInfo.Brush = Brushes.White;
            imgInfo.Pen = new Pen(Color.White, 0.1F);
            imgInfo.Grp.Clear(Color.Black);
            int fontIdx = 0;

            for (int i = 0; i < lstBuf.Count; i += 121)
            {
                imgInfo.Grp.Clear(Color.Black);
                imgInfo.Grp.SmoothingMode = SmoothingMode.HighQuality;
                imgInfo.Grp.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                Bitmap tmpFontImg = ImgUtil.WriteFontImg(imgInfo, lstBuf, i);
                tmpFontImg.Save(this.baseFolder + @"Font\cnimportOld\tmpCnFont" + (fontIdx++) + ".png");
            }
        }

        /// <summary>
        /// 符文工房字库的字符映射
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCharMap_Click(object sender, EventArgs e)
        {
            string[] allFontChar = File.ReadAllLines(this.baseFolder + @"fontChar.txt", Encoding.UTF8);
            int fileSize = allFontChar.Length * 12 + 10;
            byte[] byNewCharMap = new byte[fileSize];
            int minTmp = 255;
            byNewCharMap[0] = 0;
            byNewCharMap[1] = 0;
            byNewCharMap[2] = 0;
            byNewCharMap[3] = 0xa;
            byNewCharMap[4] = (byte)((allFontChar.Length >> 8) & 0xff);
            byNewCharMap[5] = (byte)((allFontChar.Length >> 0) & 0xff);
            byNewCharMap[6] = 0x01;
            byNewCharMap[7] = 0x00;
            byNewCharMap[8] = 0x00;
            byNewCharMap[9] = (byte)(this.fontImgCnt & 0xff);

            int bufPos = 10;
            int fontImgIdx = 0;
            int startX = 1;
            int startY = 1;
            Bitmap fontBmp = null;
            bool isLoadBmp = false;
            for (int i = 0; i < allFontChar.Length; i++)
            {
                string tmpChar = allFontChar[i];
                byte[] byCharMap = new byte[12];
                byte[] byChar = Encoding.BigEndianUnicode.GetBytes(tmpChar);
                byCharMap[0] = byChar[0];
                byCharMap[1] = byChar[1];
                byCharMap[2] = 0;
                byCharMap[3] = (byte)(fontImgIdx & 0xff);
                if (!isLoadBmp)
                {
                    fontBmp = (Bitmap)(Bitmap.FromFile(this.baseFolder + @"font\cnimportNew\tmpCnFont" + fontImgIdx + ".png"));
                    isLoadBmp = true;
                }

                if ((byChar[0] == 0 && tmpChar != " " && tmpChar != "　")
                    || "·×αβγװ—―‘’“”…※┃┏└┗┝┠□△◆◇○★☆♪、。々《》「」『』【】！％＆（）＊＋，－／０１２３４５６７８９：；＜＞？＠ＡＢＣＤＦＧＨＫＬＭＯＰＲＳＸＺａｂｃｇｊｍｑｒｕｖｘｙ～".IndexOf(tmpChar) >= 0)
                {
                    // 半角字符宽度需要动态计算，其他的直接写死
                    int leftX = startX;
                    int rightX = startX + 22;
                    bool leftXOk = false;
                    bool rightXOk = false;
                    for (int x = startX; x <= startX + 22; x++)
                    {
                        for (int y = startY; y <= startY + 22; y++)
                        {
                            if (fontBmp.GetPixel(x, y).R > 0)
                            {
                                leftX = x;
                                if (x > startX)
                                {
                                    leftX--;
                                }
                                leftXOk = true;
                                break;
                            }
                        }
                        if (leftXOk)
                        {
                            break;
                        }
                    }
                    for (int x = startX + 22; x >= startX; x--)
                    {
                        for (int y = startY; y <= startY + 22; y++)
                        {
                            if (fontBmp.GetPixel(x, y).R > 0)
                            {
                                rightX = x;
                                if (x < (startX + 22))
                                {
                                    rightX++;
                                }
                                rightXOk = true;
                                break;
                            }
                        }
                        if (rightXOk)
                        {
                            break;
                        }
                    }

                    if ("0123456789".IndexOf(tmpChar) >= 0)
                    {
                        int fixWidth = 13;
                        if (leftX > startX)
                        {
                            leftX--;
                        }
                        if (tmpChar == "1")
                        {
                            leftX -= 2;
                        }
                        byCharMap[4] = (byte)((leftX >> 8) & 0xff);
                        byCharMap[5] = (byte)((leftX >> 0) & 0xff);
                        byCharMap[8] = (byte)(((leftX + fixWidth) >> 8) & 0xff);
                        byCharMap[9] = (byte)(((leftX + fixWidth) >> 0) & 0xff);
                    }
                    else if (tmpChar == "【")
                    {
                        leftX -= 8;
                        byCharMap[4] = (byte)((leftX >> 8) & 0xff);
                        byCharMap[5] = (byte)((leftX >> 0) & 0xff);
                        byCharMap[8] = (byte)(((rightX) >> 8) & 0xff);
                        byCharMap[9] = (byte)(((rightX) >> 0) & 0xff);
                    }
                    else if (tmpChar == "】")
                    {
                        rightX += 2;
                        byCharMap[4] = (byte)((leftX >> 8) & 0xff);
                        byCharMap[5] = (byte)((leftX >> 0) & 0xff);
                        byCharMap[8] = (byte)(((rightX) >> 8) & 0xff);
                        byCharMap[9] = (byte)(((rightX) >> 0) & 0xff);
                    }
                    else
                    {
                        byCharMap[4] = (byte)((leftX >> 8) & 0xff);
                        byCharMap[5] = (byte)((leftX >> 0) & 0xff);
                        byCharMap[8] = (byte)(((rightX) >> 8) & 0xff);
                        byCharMap[9] = (byte)(((rightX) >> 0) & 0xff);
                    }
                }
                else if (tmpChar == " ")
                {
                    byCharMap[4] = (byte)((startX >> 8) & 0xff);
                    byCharMap[5] = (byte)((startX >> 0) & 0xff);
                    byCharMap[8] = (byte)(((startX + 10) >> 8) & 0xff);
                    byCharMap[9] = (byte)(((startX + 10) >> 0) & 0xff);
                }
                else
                {
                    byCharMap[4] = (byte)((startX >> 8) & 0xff);
                    byCharMap[5] = (byte)((startX >> 0) & 0xff);
                    byCharMap[8] = (byte)(((startX + 22) >> 8) & 0xff);
                    byCharMap[9] = (byte)(((startX + 22) >> 0) & 0xff);
                }

                byCharMap[6] = (byte)((startY >> 8) & 0xff);
                byCharMap[7] = (byte)((startY >> 0) & 0xff);
                byCharMap[10] = (byte)(((startY + 22) >> 8) & 0xff);
                byCharMap[11] = (byte)(((startY + 22) >> 0) & 0xff);
                Array.Copy(byCharMap, 0, byNewCharMap, bufPos, byCharMap.Length);
                bufPos += 12;

                startX += 23;
                if (startX >= 253)
                {
                    startX = 1;
                    startY += 23;
                    if (startY >= 253)
                    {
                        startX = 1;
                        startY = 1;
                        fontImgIdx++;
                        isLoadBmp = false;
                    }
                }
            }

            File.WriteAllBytes(this.baseFolder + @"00529.bin", byNewCharMap);
        }

        /// <summary>
        /// 导入符文工房中文文本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAllTxt_Click(object sender, EventArgs e)
        {
            byte[] byOldTxt = File.ReadAllBytes(this.baseFolder + @"00905Old.bin");
            int txtTableStart = 0x054e38; // 当前位置的值(0x02ba18) + 0x054e18)
            int txtTableEnd = 0x080824;
            int maxCnLen = 0x1a0f50 - 0x080830;
            int chTxtPos = 0x080830; // 减去0x054e18，结果放入Table表

            string[] cnTxtList = File.ReadAllLines(this.baseFolder + @"cnTxt00905.txt", Encoding.UTF8);

            StringBuilder sb = new StringBuilder();
            List<byte> byCnData = new List<byte>();
            for (int i = 0; i < cnTxtList.Length; i += 2)
            {
                string strCnTxt = cnTxtList[i];
                if (string.IsNullOrEmpty(strCnTxt))
                {
                    MessageBox.Show("长度有问题，需要加空格 " + (i + 1));
                    break;
                }
                if (strCnTxt.Length <= 11)
                {
                    MessageBox.Show("这里有问题，需要加空格 " + (i + 1));
                    continue;
                }
                string chkKey = strCnTxt.Substring(11, 1);
                int chStartPos;
                if (chkKey == "," || chkKey == "，")
                {
                    chStartPos = 11;
                }
                else
                {
                    chkKey = strCnTxt.Substring(10, 1);
                    if (chkKey == "," || chkKey == "，")
                    {
                        chStartPos = 10;
                    }
                    else
                    {
                        chkKey = strCnTxt.Substring(12, 1);
                        if (chkKey == "," || chkKey == "，")
                        {
                            chStartPos = 12;
                        }
                        else
                        {
                            MessageBox.Show("没有找到开始位置 " + strCnTxt);
                            break;
                        }
                    }
                }

                string newChTxt = strCnTxt.Substring(chStartPos + 1);
                //sb.Append(newChTxt).Append("\r\n");
                //byCnData.AddRange(this.EncodeLineText(newChTxt));
                // 写入中文文本
                byte[] curCnData = this.EncodeLineText(newChTxt);
                Array.Copy(curCnData, 0, byOldTxt, chTxtPos, curCnData.Length);

                // 写入中文Index位置
                txtTableStart += 8;
                chTxtPos += curCnData.Length;
                int tableIdx = chTxtPos - 0x054e18;
                byOldTxt[txtTableStart + 0] = (byte)((tableIdx >> 24) & 0xff);
                byOldTxt[txtTableStart + 1] = (byte)((tableIdx >> 16) & 0xff);
                byOldTxt[txtTableStart + 2] = (byte)((tableIdx >> 8) & 0xff);
                byOldTxt[txtTableStart + 3] = (byte)((tableIdx >> 0) & 0xff);
            }

            if (byCnData.Count > maxCnLen)
            {
                MessageBox.Show("中文个数超长了 " + (byCnData.Count - maxCnLen));
            }
            else
            {
                File.WriteAllBytes(this.baseFolder + @"00905.bin", byOldTxt);
                MessageBox.Show("正常写入中文翻译 ");
            }
        }

        /// <summary>
        /// 导入符文工房系统中文文本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSysTxt_Click(object sender, EventArgs e)
        {
            byte[] byOldTxt = File.ReadAllBytes(this.baseFolder + @"01718Old.bin");
            int txtTableStart = 0xf8;
            int maxCnLen = 0x1a0f50 - 0x080830;
            int chTxtPos = 0xc8c;

            string[] cnTxtList = File.ReadAllLines(this.baseFolder + @"cnTxt01718.txt", Encoding.UTF8);

            StringBuilder sb = new StringBuilder();
            List<byte> byCnData = new List<byte>();
            for (int i = 0; i < cnTxtList.Length; i += 2)
            {
                string strCnTxt = cnTxtList[i];
                if (string.IsNullOrEmpty(strCnTxt))
                {
                    MessageBox.Show("这里有问题，需要加空格 " + (i + 1));
                    break;
                }
                if (strCnTxt.Length <= 11)
                {
                    MessageBox.Show("这里有问题，需要加空格 " + (i + 1));
                    continue;
                }
                string chkKey = strCnTxt.Substring(11, 1);
                int chStartPos;
                if (chkKey == "," || chkKey == "，")
                {
                    chStartPos = 11;
                }
                else
                {
                    chkKey = strCnTxt.Substring(10, 1);
                    if (chkKey == "," || chkKey == "，")
                    {
                        chStartPos = 10;
                    }
                    else
                    {
                        chkKey = strCnTxt.Substring(12, 1);
                        if (chkKey == "," || chkKey == "，")
                        {
                            chStartPos = 12;
                        }
                        else
                        {
                            MessageBox.Show("没有找到开始位置 " + strCnTxt);
                            break;
                        }
                    }
                }

                string newChTxt = strCnTxt.Substring(chStartPos + 1);
                //sb.Append(newChTxt).Append("\r\n");
                //byCnData.AddRange(this.EncodeLineText(newChTxt));
                // 写入中文文本
                byte[] curCnData = this.EncodeLineText(newChTxt);
                chTxtPos = Convert.ToInt32(strCnTxt.Substring(0, 8), 16);
                Array.Copy(curCnData, 0, byOldTxt, chTxtPos, curCnData.Length);

                // 写入中文Index位置
                //txtTableStart += 4;
                //chTxtPos += curCnData.Length;
                //int tableIdx = chTxtPos + 0x20;
                //byOldTxt[txtTableStart + 0] = (byte)((tableIdx >> 24) & 0xff);
                //byOldTxt[txtTableStart + 1] = (byte)((tableIdx >> 16) & 0xff);
                //byOldTxt[txtTableStart + 2] = (byte)((tableIdx >> 8) & 0xff);
                //byOldTxt[txtTableStart + 3] = (byte)((tableIdx >> 0) & 0xff);
            }

            // 检查映射表内容不能变更
            byte[] byOldChk = File.ReadAllBytes(this.baseFolder + @"01718Old.bin");
            int chkStart = 0x18;
            for (int x = 0; x < 26;  x++)
            {
                int startPos = Util.GetOffset(byOldChk, chkStart, chkStart + 3) + 0xE8;
                int lenInfoPos = startPos + Util.GetOffset(byOldChk, startPos + 4, startPos + 7);
                int lenInfo = Util.GetOffset(byOldChk, lenInfoPos, lenInfoPos + 3);
                bool chkOk = true;
                int tmp = startPos;
                while (tmp < (startPos + lenInfo))
                {
                    if (byOldChk[tmp] != byOldTxt[tmp])
                    {
                        chkOk = false;
                        break;
                    }
                    tmp++;
                }
                if (!chkOk)
                {
                    MessageBox.Show("这个区域的内容不能动 " + startPos.ToString("X") + " " + (startPos + lenInfo).ToString("X"));
                    break;
                }


                chkStart += 8;
            }

            if (byCnData.Count > maxCnLen)
            {
                MessageBox.Show("中文个数超长了 " + (byCnData.Count - maxCnLen));
            }
            else
            {
                File.WriteAllBytes(this.baseFolder + @"01718.bin", byOldTxt);
                MessageBox.Show("正常写入中文翻译 ");
            }
        }

        /// <summary>
        /// 将当前行文本编码
        /// </summary>
        /// <param name="text">当前行文本</param>
        /// <returns>中文编码后的文本</returns>
        private byte[] EncodeLineText(string text)
        {
            List<byte> byData = new List<byte>();

            string currentChar;
            string nextChar;
            int charIndex;
            StringBuilder keyWordsSb = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                currentChar = text.Substring(i, 1);
                if ("^" == currentChar)
                {
                    // 关键字的解码
                    keyWordsSb.Length = 0;
                    while ((nextChar = text.Substring(++i, 1)) != "^")
                    {
                        keyWordsSb.Append(nextChar);
                    }

                    string[] keyWords = keyWordsSb.ToString().Split(' ');
                    foreach (string keyWord in keyWords)
                    {
                        charIndex = Convert.ToInt32(keyWord, 16);
                        byData.Add((byte)(charIndex & 0xFF));
                    }

                    continue;
                }
                else
                {
                    byData.AddRange(Encoding.BigEndianUnicode.GetBytes(currentChar));
                }
            }

            byData.Add(0);
            byData.Add(0);

            return byData.ToArray();
        }

        /// <summary>
        /// 导出符文工房原文本
        /// </summary>
        private void ExportRfoText()
        {
            byte[] byOldTxt = File.ReadAllBytes(this.baseFolder + @"01718.bin");
            int txtTableStart = 0x054e38; // 当前位置的值(0x02ba18) + 0x054e18)
            int txtTableEnd = 0x080828;

            List<string> jpTxt = new List<string>();
            for (int i = txtTableStart; i < txtTableEnd; i += 8)
            {
                int jpTxtPos = Util.GetOffset(byOldTxt, i, i + 3) + 0x054e18;
                int jpEndPos = 0;
                if (i < 0x080820)
                {
                    jpEndPos = Util.GetOffset(byOldTxt, i + 8, i + 8 + 3) + 0x054e18;
                }
                else
                {
                    jpEndPos = 0x1a0f4a;
                }

                string jpLine = Encoding.BigEndianUnicode.GetString(byOldTxt, jpTxtPos, jpEndPos - jpTxtPos);
                jpTxt.Add(jpTxtPos.ToString("x").ToUpper().PadLeft(8, '0') + "," + (jpEndPos - jpTxtPos) + "," + jpLine.Replace("\n", "^00 0a^") + "\n");
            }

            File.WriteAllLines(this.baseFolder + @"jpTxt.txt", jpTxt.ToArray(), Encoding.UTF8);
        }

        /// <summary>
        /// 字库文字排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSortChar_Click(object sender, EventArgs e)
        {
            string[] cnTxtList = File.ReadAllLines(this.baseFolder + @"allCnText.txt", Encoding.UTF8);
            List<string> lstCnTxt = new List<string>();
            for (int i = 0; i < cnTxtList.Length; i += 2)
            {
                string strCnTxt = cnTxtList[i];
                if (string.IsNullOrEmpty(strCnTxt))
                {
                    MessageBox.Show("长度有问题，需要加空格 " + (i + 1));
                    break;
                }
                if (strCnTxt.Length <= 11)
                {
                    MessageBox.Show("长度有问题，需要加空格 " + (i + 1));
                    continue;
                }


                string chkKey = strCnTxt.Substring(11, 1);
                int chStartPos;
                if (chkKey == "," || chkKey == "，")
                {
                    chStartPos = 11;
                }
                else
                {
                    chkKey = strCnTxt.Substring(10, 1);
                    if (chkKey == "," || chkKey == "，")
                    {
                        chStartPos = 10;
                    }
                    else
                    {
                        chkKey = strCnTxt.Substring(12, 1);
                        if (chkKey == "," || chkKey == "，")
                        {
                            chStartPos = 12;
                        }
                        else
                        {
                            MessageBox.Show("没有找到开始位置 " + strCnTxt);
                            break;
                        }
                    }
                }

                string newChTxt = strCnTxt.Substring(chStartPos + 1);
                for (int j = 0; j < newChTxt.Length; j++)
                {
                    string tmpStr = newChTxt.Substring(j, 1);
                    if (!lstCnTxt.Contains(tmpStr))
                    {
                        lstCnTxt.Add(tmpStr);
                    }
                }
            }

            lstCnTxt.Sort(this.BigEndianUnicodeCompare);

            File.WriteAllLines(this.baseFolder + @"fontChar.txt", lstCnTxt.ToArray(), Encoding.UTF8);
        }

        /// <summary>
        /// BigEndianUnicode对象比较
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private int BigEndianUnicodeCompare(string a, string b)
        {
            byte[] byA = Encoding.BigEndianUnicode.GetBytes(a);
            byte[] byB = Encoding.BigEndianUnicode.GetBytes(b);
            int aTmp = (byA[0] << 8) + byA[1];
            int bTmp = (byB[0] << 8) + byB[1];

            return aTmp - bTmp;
        }

        /// <summary>
        /// 修正汉化图片格式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChgPic_Click(object sender, EventArgs e)
        {
            this.ChgOldHanhPic(this.baseFolder + @"rfo\rfo\PicHanhua\logo_fuyu\Old16ColorTif\");
            this.ChgOldHanhPic(this.baseFolder + @"rfo\rfo\PicHanhua\logo_fuyu\OldOtherColorTif\");
            MessageBox.Show("图片转换完成");
        }

        /// <summary>
        /// 修正汉化图片格式
        /// </summary>
        /// <param name="folder"></param>
        private void ChgOldHanhPic(string folder)
        {
            List<FilePosInfo> allFc = Util.GetAllFiles(folder).Where(p => !p.IsFolder).ToList();
            foreach (FilePosInfo fi in allFc)
            {
                if (fi.File.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                {
                    File.Copy(fi.File, fi.File.Replace(".tif", ".png").Replace("Tif", "Png"), true);
                }
                else
                {
                    Image img = Image.FromFile(fi.File);
                    img.Save(fi.File.Replace(".tif", ".png").Replace("Tif", "Png"), ImageFormat.Png);
                }
            }
        }

        /// <summary>
        /// 功能测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTest_Click(object sender, EventArgs e)
        {
            byte[] byOldTxt = File.ReadAllBytes(this.baseFolder + @"01718Old.bin");
            int txtTableStart = 0xF8;
            int txtTableEnd = 0xC8C;

            List<string> jpTxt = new List<string>();
            for (int i = txtTableStart; i < txtTableEnd; i += 4)
            {
                int jpTxtPos = Util.GetOffset(byOldTxt, i, i + 3);
                int jpEndPos = 0;
                if (i < 0xC88)
                {
                    jpEndPos = Util.GetOffset(byOldTxt, i + 4, i + 4 + 3);
                }
                else
                {
                    jpEndPos = 0xEF20;
                }

                string jpLine = Encoding.BigEndianUnicode.GetString(byOldTxt, jpTxtPos, jpEndPos - jpTxtPos);
                jpTxt.Add(jpTxtPos.ToString("x").ToUpper().PadLeft(8, '0') + "," + (jpEndPos - jpTxtPos) + "," + jpLine.Replace("\n", "^00 0a^") + "\n");
            }

            File.WriteAllLines(this.baseFolder + @"jpTxt01718.txt", jpTxt.ToArray(), Encoding.UTF8);
        }

        /// <summary>
        /// 一键打包
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPatchFile_Click(object sender, EventArgs e)
        {
            // 复制字库，翻译文件
            File.Copy(this.baseFolder + @"00529.bin", this.baseFolder + @"rfo\rfo\RUNEFACTORY\00529.bin", true);
            File.Copy(this.baseFolder + @"00530.bin", this.baseFolder + @"rfo\rfo\RUNEFACTORY\00530.bin", true);
            File.Copy(this.baseFolder + @"00905.bin", this.baseFolder + @"rfo\rfo\RUNEFACTORY\00905.bin", true);
            File.Copy(this.baseFolder + @"01718.bin", this.baseFolder + @"rfo\rfo\RUNEFACTORY\01718.bin", true);

            // 复制图片文件
            List<FilePosInfo> allPicFiles = Util.GetAllFiles(this.baseFolder + @"rfo\rfo\PicHanhua\OKBin\").Where(p => !p.IsFolder).ToList();
            foreach (FilePosInfo fi in allPicFiles)
            {
                File.Copy(fi.File, fi.File.Replace(@"\PicHanhua\bin\", @"\RUNEFACTORY\"), true);
            }

            // 打包
            System.Diagnostics.Process exep = new System.Diagnostics.Process();
            exep.StartInfo.FileName = this.baseFolder + @"rfo\rfo\rfo.exe";
            exep.StartInfo.CreateNoWindow = true;
            exep.StartInfo.UseShellExecute = false;
            exep.StartInfo.Arguments = @" -r " + this.baseFolder + @"\rfo\rfo\RUNEFACTORY";
            exep.Start();
            exep.WaitForExit();

            // 复制补丁
            File.Copy(this.baseFolder + @"rfo\rfo\RUNEFACTORY.repack.bin", this.baseFolder + @"测试补丁\files\RUNEFACTORY.bin", true);
            File.Copy(this.baseFolder + @"rfo\rfo\RUNEFACTORY.repack.dat", this.baseFolder + @"测试补丁\files\RUNEFACTORY.dat", true);

            MessageBox.Show("一键打包完成");
        }
    }
}
