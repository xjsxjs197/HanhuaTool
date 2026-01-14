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
                    || "·×αβγװ—―‘’“”…※□△◆◇○★☆♪、。々《》「」『』【】！＃％（）＊＋，－．／０１２３４５６７８９：；＜＝＞？＠ＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺ＼＾＿ａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚ～￣".IndexOf(tmpChar) >= 0)
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
                string strCnTxt = cnTxtList[i].Trim();
                if (string.IsNullOrEmpty(strCnTxt))
                {
                    MessageBox.Show("长度有问题，需要加空行 " + (i + 1));
                    break;
                }
                if (strCnTxt.Length <= 11)
                {
                    //MessageBox.Show("这里有问题，需要加空格 " + (i + 1));
                    //continue;
                    strCnTxt = strCnTxt + " ";
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
            List<int> minPos = new List<int>();
            List<int> maxPos = new List<int>();
            List<int> txtCount = new List<int>();
            List<int> txtStartPos = new List<int>();
            Dictionary<int, int> txtTableInfo = new Dictionary<int, int>();
            Dictionary<int, int> txtTableFistLineInfo = new Dictionary<int, int>();

            // 先取得所有区域文本映射表信息
            for (int i = 0x18; i < 0xE8; i += 8)
            {
                int txtAreaStartPos = Util.GetOffset(byOldTxt, i, i + 3) + 0xE8;
                int txtAreaSizePos = txtAreaStartPos + Util.GetOffset(byOldTxt, txtAreaStartPos + 4, txtAreaStartPos + 4 + 3);

                int txtAreaStrCount = Util.GetOffset(byOldTxt, txtAreaSizePos - 16, txtAreaSizePos - 16 + 3);
                int txtAresMinPos = txtAreaStartPos + Util.GetOffset(byOldTxt, txtAreaSizePos, txtAreaSizePos + 3);
                int txtAresMaxPos = txtAreaStartPos + (txtAreaStrCount - 1) * 4;
                
                txtAresMaxPos = txtAreaStartPos + Util.GetOffset(byOldTxt, txtAresMaxPos, txtAresMaxPos + 3);
                minPos.Add(txtAresMinPos);
                maxPos.Add(txtAresMaxPos);
                txtCount.Add(txtAreaStrCount);
                txtStartPos.Add(txtAreaStartPos);

                txtTableFistLineInfo.Add(txtAresMinPos, txtAresMinPos - txtAreaStartPos);
                txtTableInfo.Add(txtAresMinPos, txtAresMinPos - txtAreaStartPos);
                txtTableInfo.Add(txtAreaStartPos + Util.GetOffset(byOldTxt, txtAreaSizePos + 4, txtAreaSizePos + 7), txtAreaSizePos + 4);
                txtTableInfo.Add(txtAreaStartPos + Util.GetOffset(byOldTxt, txtAreaSizePos + 8, txtAreaSizePos + 11), txtAreaSizePos + 8);
                txtTableInfo.Add(txtAreaStartPos + Util.GetOffset(byOldTxt, txtAreaSizePos + 12, txtAreaSizePos + 15), txtAreaSizePos + 12);
                for (int j = 4; j < txtAreaStrCount; j++)
                {
                    txtTableInfo.Add(txtAreaStartPos + Util.GetOffset(byOldTxt, txtAreaStartPos + j * 4, txtAreaStartPos + j * 4 + 3), txtAreaStartPos + j * 4);
                }
            }

            int txtTableidx = -1;
            int txtCurMinPos = 0;
            int txtCurMaxPos = 0;
            int lastOverLen = 0;
            int chTxtPos = 0;

            string[] cnTxtList = File.ReadAllLines(this.baseFolder + @"cnTxt01718.txt", Encoding.UTF8);

            StringBuilder sb = new StringBuilder();
            List<byte> byCnData = new List<byte>();
            for (int i = 0; i < cnTxtList.Length; i += 2)
            {
                string strCnTxt = cnTxtList[i].Trim();
                if (string.IsNullOrEmpty(strCnTxt))
                {
                    MessageBox.Show("这里有问题，需要加空行 " + (i + 1));
                    break;
                }
                if (strCnTxt.Length <= 11)
                {
                    //MessageBox.Show("这里有问题，需要加空格 " + (i + 1));
                    //continue;
                    strCnTxt = strCnTxt + " ";
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

                string[] lines = strCnTxt.Split(',');
                int jpLineLen = Convert.ToInt32(lines[1]) + 2;
                string newChTxt = strCnTxt.Substring(chStartPos + 1);
                
                // 写入中文文本
                byte[] curCnData = this.EncodeLineText(newChTxt);
                chTxtPos = Convert.ToInt32(strCnTxt.Substring(0, 8), 16);

                // 判断当前文本是否是一个文本块的开始行，如果是，位置保持不变
                if (txtTableFistLineInfo.ContainsKey(chTxtPos))
                {
                    lastOverLen = 0;
                }

                Array.Copy(curCnData, 0, byOldTxt, chTxtPos + lastOverLen, curCnData.Length);
                if (lastOverLen != 0)
                {
                    // 上一行文本和日文长度不一样，需要修改后面行的位置映射表信息
                    if (txtTableInfo.ContainsKey(chTxtPos))
                    {
                        int curLineTblStartPos = txtTableInfo[chTxtPos];
                        int befPos = Util.GetOffset(byOldTxt, curLineTblStartPos, curLineTblStartPos + 3);
                        befPos += lastOverLen;
                        byOldTxt[curLineTblStartPos + 0] = (byte)((befPos >> 24) & 0xFF);
                        byOldTxt[curLineTblStartPos + 1] = (byte)((befPos >> 16) & 0xFF);
                        byOldTxt[curLineTblStartPos + 2] = (byte)((befPos >> 8) & 0xFF);
                        byOldTxt[curLineTblStartPos + 3] = (byte)((befPos >> 0) & 0xFF);
                    }
                    else
                    {
                        MessageBox.Show("上一行文本超长，但是本行没有找到位置信息 " + strCnTxt.ToUpper());
                        return;
                    }
                }

                // 当前行文字变更的个数的累加
                lastOverLen += curCnData.Length - jpLineLen;
            }

            File.WriteAllBytes(this.baseFolder + @"01718.bin", byOldTxt);
            MessageBox.Show("正常写入中文翻译 ");
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
                string strCnTxt = cnTxtList[i].Trim();
                if (string.IsNullOrEmpty(strCnTxt))
                {
                    MessageBox.Show("长度有问题，需要加空行 " + (i + 1));
                    break;
                }
                if (strCnTxt.Length <= 11)
                {
                    //MessageBox.Show("长度有问题，需要加空格 " + (i + 1));
                    //continue;
                    strCnTxt = strCnTxt + " ";
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

            // 追加日语的假名，全角字母，数字
            string fixStr = "ぁあぃいぅうぇえぉおかがきぎくぐけげこごさざしじすずせぜそぞただちぢっつづてでとどなにぬねのはばぱひびぴふぶぷへべぺほぼぽまみむめもゃやゅゆょよらりるれろゎわゐゑをん"
                + "ァアィイゥウェエォオカガキギクグケゲコゴサザシジスズセゼソゾタダチヂッツヅテデトドナニヌネノハバパヒビピフブプヘベペホボポマミムメモャヤュユョヨラリルレロヮワヰヱヲンヴヵヶ"
                + "ＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚ"
                + "0123456789０１２３４５６７８９ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < fixStr.Length - 1; i++)
            {
                string tmpStr = fixStr.Substring(i, 1);
                if (!lstCnTxt.Contains(tmpStr))
                {
                    lstCnTxt.Add(tmpStr);
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
            //byte[] by00529 = File.ReadAllBytes(this.baseFolder + @"00529Old.bin");
            //int tmpPos = 0xa;
            //StringBuilder sb = new StringBuilder();

            //while (by00529[tmpPos + 2] == 0 && by00529[tmpPos + 3] == 0)
            //{
            //    tmpPos += 12;
            //    continue;
            //}

            //for (int i = 0; i < 18; i++)
            //{
            //    int tmp = ((by00529[tmpPos + 0] << 8) + by00529[tmpPos + 1]);
            //    sb.Append(Encoding.BigEndianUnicode.GetString(by00529, tmpPos, 2)).Append(tmp.ToString("X")).Append(" ");
            //    tmpPos += 12;
            //}
            //tmpPos = 0;

            //string[] allLines = File.ReadAllLines(this.baseFolder + @"cnTxt00905.txt", Encoding.UTF8);
            //string[] allLinesJp = File.ReadAllLines(this.baseFolder + @"jpTxt00905_20250604.txt", Encoding.UTF8);
            //StringBuilder sbCn = new StringBuilder();
            //StringBuilder sbJp = new StringBuilder();
            //for (int i = 0; i < allLines.Length; i += 2)
            //{
            //    string curLine = allLines[i].Trim();
            //    string curLineJp = allLinesJp[i].Trim();
            //    if (string.IsNullOrEmpty(curLine))
            //    {
            //        MessageBox.Show("当前行为空！" + (i + 1));
            //        break;
            //    }

            //    string[] lines = curLine.Split(',');
            //    string[] linesJp = curLineJp.Split(',');

            //    if (lines[0] != linesJp[0] || lines[1] != linesJp[1])
            //    {
            //        //MessageBox.Show("地址不一致 " + lines[0]);
            //        sbCn.Append(curLine).Append("\r\n");
            //        sbCn.Append(curLineJp).Append("\r\n\r\n");
            //    }

            //    //int curLineLen = Convert.ToInt32(lines[1]);
            //    //int curLineLenJp = Convert.ToInt32(linesJp[1]);

            //    //if (curLineLen > curLineLenJp)
            //    //{
            //    //    sbCn.Append("============").Append("\r\n");
            //    //    sbJp.Append("============").Append("\r\n");
            //    //}
            //    //sbCn.Append(curLine).Append("\r\n\r\n");
            //    //sbJp.Append(curLineJp).Append("\r\n\r\n");
            //}

            //File.WriteAllText(this.baseFolder + @"cnTxt00905ImpChk2016.txt", sbCn.ToString(), Encoding.UTF8);
            //File.WriteAllText(this.baseFolder + @"jpTxt01718ImpChk2.txt", sbJp.ToString(), Encoding.UTF8);

            //MessageBox.Show("OK");
            //File.WriteAllText(this.baseFolder + @"cnTxt01718LenChk.txt", sb.ToString(), Encoding.UTF8);

            byte[] byCnTxt = File.ReadAllBytes(this.baseFolder + @"01718.bin");
            byte[] byJpTxt = File.ReadAllBytes(this.baseFolder + @"01718Old.bin");
            StringBuilder sb = new StringBuilder();


            // 先取得所有区域文本映射表信息
            for (int i = 0x18; i < 0xE8; i += 8)
            {
                int txtAreaStartPos = Util.GetOffset(byCnTxt, i, i + 3) + 0xE8;
                int txtAreaSizePos = txtAreaStartPos + Util.GetOffset(byCnTxt, txtAreaStartPos + 4, txtAreaStartPos + 4 + 3);

                int txtAreaStrCount = Util.GetOffset(byCnTxt, txtAreaSizePos - 16, txtAreaSizePos - 16 + 3);
                int txtAresMinPos = txtAreaStartPos + Util.GetOffset(byCnTxt, txtAreaSizePos, txtAreaSizePos + 3);

                this.DecodeCnTxt(byCnTxt, txtAresMinPos, sb);
                this.DecodeCnTxt(byCnTxt, txtAreaStartPos + Util.GetOffset(byCnTxt, txtAreaSizePos + 4, txtAreaSizePos + 7), sb);
                this.DecodeCnTxt(byCnTxt, txtAreaStartPos + Util.GetOffset(byCnTxt, txtAreaSizePos + 8, txtAreaSizePos + 11), sb);
                this.DecodeCnTxt(byCnTxt, txtAreaStartPos + Util.GetOffset(byCnTxt, txtAreaSizePos + 12, txtAreaSizePos + 15), sb);
                for (int j = 4; j < txtAreaStrCount; j++)
                {
                    //if ((txtAreaStartPos + j * 4 + 3) < byCnTxt.Length)
                    //{
                    //    this.DecodeCnTxt(byCnTxt, txtAreaStartPos + Util.GetOffset(byCnTxt, txtAreaStartPos + j * 4, txtAreaStartPos + j * 4 + 3), sb);
                    //}
                    //else
                    //{
                    //    sb.Append("发生错误\r\n\r\n");
                    //}
                    try
                    {
                        this.DecodeCnTxt(byCnTxt, txtAreaStartPos + Util.GetOffset(byCnTxt, txtAreaStartPos + j * 4, txtAreaStartPos + j * 4 + 3), sb);
                    }
                    catch (Exception exp)
                    {
                        sb.Append("发生错误 ").Append(txtAreaStartPos.ToString("X").ToUpper().PadLeft(8, '0')).Append("\r\n\r\n");
                        break;
                    }
                }

                sb.Append("==============================\r\n\r\n");
            }

            File.WriteAllText(this.baseFolder + @"cnTxt01718ImpChk2016.txt", sb.ToString(), Encoding.UTF8);

            //byte[] by00530 = File.ReadAllBytes(this.baseFolder + @"00530Old.bin");

            //for (int i = 0; i < 19; i++)
            //{
            //    int tmpFontPos = 0x280 + i * 0x10;
            //    int tmpFontPicPos = Util.GetOffset(by00530, tmpFontPos, tmpFontPos + 3);
            //    byte[] byTmpFontPic = new byte[256 * 256 / 2];

            //    Array.Copy(by00530, tmpFontPicPos, byTmpFontPic, 0, byTmpFontPic.Length);

                
            //    Bitmap bmp = Util.ImageDecode(new Bitmap(256, 256), byTmpFontPic, "I4");

            //    // set image data
            //    bmp.Save(this.baseFolder + @"Font\jpfont\tmpJpFont" + i + ".png");
            //}


            MessageBox.Show("OK");
        }

        private void DecodeCnTxt(byte[] byCnTxt, int txtPos, StringBuilder sb)
        {
            List<byte> lineInfo = new List<byte>();
            sb.Append(txtPos.ToString("X").ToUpper().PadLeft(8, '0')).Append(",");
            while (true)
            {
                byte tmp1 = byCnTxt[txtPos];
                byte tmp2 = byCnTxt[txtPos + 1];

                if (tmp1 == 0 && tmp2 == 0)
                {
                    break;
                }

                lineInfo.Add(tmp1);
                lineInfo.Add(tmp2);
                txtPos += 2;
            }

            sb.Append(lineInfo.Count).Append(",");
            sb.Append(Encoding.BigEndianUnicode.GetString(lineInfo.ToArray()).Replace("\n", "^00 0a^"));
            sb.Append("\r\n\r\n");
        }

        private void CheckTmpPic(int idx, byte[] byTmp, int imgIdx, int width, int height, string fileName)
        {
            byte[] byImg = new byte[width * height];
            Array.Copy(byTmp, idx, byImg, 0, byImg.Length);

            byte[] byPalette = new byte[512];
            Array.Copy(byTmp, idx + width * height, byPalette, 0, byPalette.Length);

            Bitmap testBmp = new Bitmap(width, height);
            testBmp = Util.PaletteImageDecode(testBmp, byImg, "C8_CI8", byPalette, 2);

            testBmp.Save(this.baseFolder + @"rfo\rfo\PicHanhua\Special\SpecialOldChk\" + fileName + "_" + imgIdx + ".png");
        }

        private void CheckTmpPic2(int idx, byte[] byTmp, int width, int height, string fileName)
        {
            byte[] byImg = new byte[width * height];
            Array.Copy(byTmp, idx, byImg, 0, byImg.Length);

            byte[] byPalette = new byte[512];
            Array.Copy(byTmp, idx + width * height, byPalette, 0, byPalette.Length);

            Bitmap testBmp = new Bitmap(width, height);
            testBmp = Util.PaletteImageDecode(testBmp, byImg, "C8_CI8", byPalette, 2);

            testBmp.Save(this.baseFolder + @"Font\1410PicCnChk\" + fileName + ".png");
        }

        private void CheckTmpPicI4(int startPos, byte[] byTmp, int maxImgIdx, int width, int height, string fileName)
        {
            int bufSize = width * height / 2;
            byte[] byImg = new byte[bufSize];

            for (int i = 0; i < maxImgIdx; i++)
            {
                Array.Copy(byTmp, startPos + i * bufSize, byImg, 0, byImg.Length);
                Bitmap testBmp = new Bitmap(width, height);
                testBmp = Util.ImageDecode(testBmp, byImg, "I4");

                testBmp.Save(this.baseFolder + @"rfo\rfo\PicHanhua\Special\SpecialOldChk\" + fileName + "_" + i + ".png");
            }
        }

        private void CheckTmpPicCmpr(int startPos, byte[] byTmp, int maxImgIdx, int width, int height, string fileName)
        {
            int bufSize = width * height / 2;
            byte[] byImg = new byte[bufSize];

            for (int i = 0; i < maxImgIdx; i++)
            {
                Array.Copy(byTmp, startPos + i * bufSize, byImg, 0, byImg.Length);
                Bitmap testBmp = new Bitmap(width, height);
                testBmp = Util.CmprImageDecode(testBmp, byImg);

                testBmp.Save(this.baseFolder + @"rfo\rfo\PicHanhua\Special\SpecialOldChk\" + fileName + "_" + i + ".png");
            }
        }

        private void CheckTmpPic565(int startPos, byte[] byTmp, int maxImgIdx, int width, int height, string fileName)
        {
            int bufSize = width * height * 2;
            byte[] byImg = new byte[bufSize];

            for (int i = 0; i < maxImgIdx; i++)
            {
                Array.Copy(byTmp, startPos + i * bufSize, byImg, 0, byImg.Length);
                Bitmap testBmp = new Bitmap(width, height);
                testBmp = Util.ImageDecode(testBmp, byImg, "RGB565");

                testBmp.Save(this.baseFolder + @"rfo\rfo\PicHanhua\Special\SpecialOldChk\" + fileName + "_" + i + ".png");
            }
        }

        private void CheckTmpC4CI4(int idx, byte[] byTmp, int imgIdx, int width, int height, string fileName)
        {
            byte[] byImg = new byte[width * height / 2];
            Array.Copy(byTmp, idx, byImg, 0, byImg.Length);

            byte[] byPalette = new byte[32];
            Array.Copy(byTmp, idx + (width * height / 2), byPalette, 0, byPalette.Length);

            Bitmap testBmp = new Bitmap(width, height);
            testBmp = Util.PaletteImageDecode(testBmp, byImg, "C4_CI4", byPalette, 1);

            testBmp.Save(this.baseFolder + @"rfo\rfo\PicHanhua\Special\SpecialOldChk\" + fileName + "_" + imgIdx + ".png");
        }

        private void CheckTmpC4CI4_2(int idx, byte[] byTmp, int imgIdx, int width, int height, string fileName)
        {
            byte[] byImg = new byte[width * height / 2];
            Array.Copy(byTmp, idx, byImg, 0, byImg.Length);

            byte[] byPalette = new byte[32];
            Array.Copy(byTmp, idx + (width * height / 2), byPalette, 0, byPalette.Length);

            Bitmap testBmp = new Bitmap(width, height);
            testBmp = Util.PaletteImageDecode(testBmp, byImg, "C4_CI4", byPalette, 2);

            testBmp.Save(this.baseFolder + @"rfo\rfo\PicHanhua\Special\SpecialOldChk\" + fileName + "_" + imgIdx + ".png");
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
                File.Copy(fi.File, fi.File.Replace(@"\PicHanhua\OKBin\", @"\RUNEFACTORY\"), true);
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

        /// <summary>
        /// 导出特殊图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExpSpecialImg_Click(object sender, EventArgs e)
        {
            byte[] byTmp;
            for (int i = 01391; i <= 01455; i++)
            {
                string fileName = i.ToString().PadLeft(5, '0');
                byTmp = File.ReadAllBytes(this.baseFolder + @"Font\1410Cn\" + fileName + ".bin");
                this.CheckTmpPic2(0x60, byTmp, 432, 136, fileName);
            }

            //byTmp = File.ReadAllBytes(this.baseFolder + @"rfo\rfo\PicHanhua\OKBin\01467.bin"); // OK
            //this.CheckTmpPic(0x2A0, byTmp, 0, 0xB0, 0x20, "01467");
            //this.CheckTmpPic(0x1AA0, byTmp, 1, 0xB0, 0x20, "01467");
            //this.CheckTmpPic(0x32A0, byTmp, 2, 0xB0, 0x20, "01467");
            //this.CheckTmpPic(0x4AA0, byTmp, 3, 0xB0, 0x20, "01467");
            //this.CheckTmpPic(0x62A0, byTmp, 4, 0xB0, 0x20, "01467");
            //this.CheckTmpPic(0x7AA0, byTmp, 5, 0xB0, 0x20, "01467");
            //this.CheckTmpPic(0x92A0, byTmp, 6, 0xB0, 0x20, "01467");
            //this.CheckTmpPic(0xAAA0, byTmp, 7, 0xB0, 0x20, "01467");
            //this.CheckTmpPic(0xC2A0, byTmp, 8, 0xB0, 0x20, "01467");
            //this.CheckTmpPic(0xDAA0, byTmp, 9, 0xB0, 0x20, "01467");

            //byTmp = File.ReadAllBytes(this.baseFolder + @"rfo\rfo\PicHanhua\OKBin\01502.bin"); // OK
            //this.CheckTmpPic(0x220, byTmp, 0, 0xB0, 0x20, "01502");
            //this.CheckTmpPic(0x1A20, byTmp, 1, 0xB0, 0x20, "01502");
            //this.CheckTmpPic(0x3220, byTmp, 2, 0xB0, 0x20, "01502");
            //this.CheckTmpPic(0x4A20, byTmp, 3, 0xB0, 0x20, "01502");
            //this.CheckTmpPic(0x6220, byTmp, 4, 0xB0, 0x20, "01502");
            //this.CheckTmpPic(0x7A20, byTmp, 5, 0xB0, 0x20, "01502");
            //this.CheckTmpPic(0x9220, byTmp, 6, 0xB0, 0x20, "01502");
            //this.CheckTmpPic(0xAA20, byTmp, 7, 0xB0, 0x20, "01502");

            //byTmp = File.ReadAllBytes(this.baseFolder + @"rfo\rfo\PicHanhua\OKBin\01503.bin"); // OK
            //this.CheckTmpPic(0x1E0, byTmp, 0, 0xB0, 0x1C, "01503");
            //this.CheckTmpPic(0x1720, byTmp, 1, 0xB0, 0x1C, "01503");
            //this.CheckTmpPic(0x2C60, byTmp, 2, 0xB0, 0x1C, "01503");
            //this.CheckTmpPic(0x41A0, byTmp, 3, 0xB0, 0x1C, "01503");
            //this.CheckTmpPic(0x56E0, byTmp, 4, 0xB0, 0x1C, "01503");
            //this.CheckTmpPic(0x6C20, byTmp, 5, 0xB0, 0x1C, "01503");
            //this.CheckTmpPic(0x8160, byTmp, 6, 0xB0, 0x20, "01503");

            //byTmp = File.ReadAllBytes(this.baseFolder + @"rfo\rfo\PicHanhua\OKBin\01504.bin"); // OK
            //this.CheckTmpPic(0x60, byTmp, 0, 0xB0, 0x20, "01504");

            //byTmp = File.ReadAllBytes(this.baseFolder + @"rfo\rfo\PicHanhua\OKBin\01580.bin"); // OK
            //this.CheckTmpPic(0x260, byTmp, 0, 0xB0, 0x20, "01580");
            //this.CheckTmpPic(0x1A60, byTmp, 1, 0xB0, 0x20, "01580");
            //this.CheckTmpPic(0x3260, byTmp, 2, 0xB0, 0x20, "01580");
            //this.CheckTmpPic(0x4A60, byTmp, 3, 0xB0, 0x20, "01580");
            //this.CheckTmpPic(0x6260, byTmp, 4, 0xB0, 0x20, "01580");
            //this.CheckTmpPic(0x7A60, byTmp, 5, 0xB0, 0x20, "01580");
            //this.CheckTmpPic(0x9260, byTmp, 6, 0xB0, 0x20, "01580");
            //this.CheckTmpPic(0xAA60, byTmp, 7, 0xB0, 0x20, "01580");
            //this.CheckTmpPic(0xC260, byTmp, 8, 0xB0, 0x20, "01580");

            //byTmp = File.ReadAllBytes(this.baseFolder + @"rfo\rfo\PicHanhua\OKBin\01614.bin"); // OK
            //this.CheckTmpPic(0xA0, byTmp, 0, 0xB0, 0x20, "01614");
            //this.CheckTmpPic(0x18A0, byTmp, 1, 0xB0, 0x20, "01614");


            //byTmp = File.ReadAllBytes(this.baseFolder + @"rfo\rfo\PicHanhua\OKBin\01367.bin"); // OK
            //this.CheckTmpPicI4(0x120, byTmp, 5, 0x48, 0x18, "01367");

            //byTmp = File.ReadAllBytes(this.baseFolder + @"rfo\rfo\PicHanhua\OKBin\01387.bin"); // OK
            //this.CheckTmpPicI4(0x38E0, byTmp, 57, 0x48, 0x18, "01387");

            //byTmp = File.ReadAllBytes(this.baseFolder + @"rfo\rfo\PicHanhua\OKBin\01477.bin"); // OK
            //this.CheckTmpPicI4(0x4E0, byTmp, 25, 0x48, 0x18, "01477");

            //// 不需要，是通关图片
            ////byTmp = File.ReadAllBytes(this.baseFolder + @"rfo\rfo\RUNEFACTORY\01540.bin");
            ////this.CheckTmpPicCmpr(0x2A0, byTmp, 13, 0x80, 0x60, "01540");

            //// 不需要
            ////byTmp = File.ReadAllBytes(this.baseFolder + @"rfo\rfo\RUNEFACTORY\01781.bin");
            ////this.CheckTmpPic565(0x60, byTmp, 1, 0x280, 0x1E0, "01781");

            //byTmp = File.ReadAllBytes(this.baseFolder + @"rfo\rfo\PicHanhua\OKBin\01373.bin"); // OK
            //this.CheckTmpPicCmpr(0x60, byTmp, 1, 0x280, 0x1E0, "01373");

            // 不需要
            //byTmp = File.ReadAllBytes(this.baseFolder + @"rfo\rfo\RUNEFACTORY\01383.bin");
            //this.CheckTmpPic(0xA0, byTmp, 0, 0x80, 0x80, "01383");
            //this.CheckTmpPic(0x42A0, byTmp, 1, 0x140, 0x50, "01383");

            // 不需要
            //byTmp = File.ReadAllBytes(this.baseFolder + @"rfo\rfo\RUNEFACTORY\01385.bin");
            //this.CheckTmpPic(0x120, byTmp, 0, 0x100, 0x100, "01385");
            //this.CheckTmpPic(0x10320, byTmp, 1, 0x80, 0x20, "01385");

            // 不需要
            //byTmp = File.ReadAllBytes(this.baseFolder + @"rfo\rfo\RUNEFACTORY\01387.bin");
            //this.CheckTmpC4CI4(0x23C0, byTmp, 1, 0x20, 0x20, "01387_1");
            //this.CheckTmpPic(0x25E0, byTmp, 2, 0x88, 0x20, "01387_1");

            // 不需要
            //byTmp = File.ReadAllBytes(this.baseFolder + @"rfo\rfo\RUNEFACTORY\01636.bin");
            //this.CheckTmpC4CI4(0x60, byTmp, 1, 0xC8, 0x18, "01636");

            // 不需要
            //byTmp = File.ReadAllBytes(this.baseFolder + @"rfo\rfo\RUNEFACTORY\01610.bin");
            //this.CheckTmpPic(0x7E0, byTmp, 0, 0x28, 0x28, "01610");

            //byTmp = File.ReadAllBytes(this.baseFolder + @"rfo\rfo\PicHanhua\OKBin\01582.bin");
            //this.CheckTmpC4CI4_2(0x120, byTmp, 0, 0x68, 0x18, "01582");
            //this.CheckTmpC4CI4_2(0x620, byTmp, 1, 0x68, 0x18, "01582");
            //this.CheckTmpC4CI4_2(0xB20, byTmp, 2, 0x68, 0x18, "01582");
            //this.CheckTmpC4CI4_2(0x1020, byTmp, 3, 0x68, 0x18, "01582");
        }

        /// <summary>
        /// 导入特殊图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImpSpecialImg_Click(object sender, EventArgs e)
        {
            byte[] byBin;
            List<byte> byPalette = new List<byte>();
            for (int i = 01391; i <= 01455; i++)
            {
                string fileName = i.ToString().PadLeft(5, '0');
                byBin = File.ReadAllBytes(this.baseFolder + @"Font\1410Jp\" + fileName + ".bin");

                Bitmap testBmp = (Bitmap)Bitmap.FromFile(this.baseFolder + @"Font\1410PicCnReduce\" + fileName.Substring(1) + ".png");
                byPalette.Clear();
                byte[] byImg = Util.PaletteImageEncode(testBmp, "C8_CI8", byPalette, 2);

                Array.Copy(byImg, 0, byBin, 0x60, byImg.Length);

                byte[] impPalette = byPalette.ToArray();
                Array.Copy(impPalette, 0, byBin, 0xe5e0, impPalette.Length);

                File.WriteAllBytes(this.baseFolder + @"Font\1410Cn\" + fileName + ".bin", byBin);
            }

            //byTmp = File.ReadAllBytes(this.baseFolder + @"rfo\rfo\RUNEFACTORY\01367.bin");
            //this.ImportTmpPicI4(0x120, byTmp, 5, "01367");

            //byTmp = File.ReadAllBytes(this.baseFolder + @"rfo\rfo\RUNEFACTORY\01387.bin"); // OK
            //this.ImportTmpPicI4(0x38E0, byTmp, 57, "01387");

            //byTmp = File.ReadAllBytes(this.baseFolder + @"rfo\rfo\RUNEFACTORY\01477.bin"); // OK
            //this.ImportTmpPicI4(0x4E0, byTmp, 25, "01477");

            //byTmp = File.ReadAllBytes(this.baseFolder + @"rfo\rfo\RUNEFACTORY\01467.bin"); // OK
            //this.ImportTmpPicC4_C8(0x2A0, byTmp, 10, "01467", "C8_CI8");

            //byTmp = File.ReadAllBytes(this.baseFolder + @"rfo\rfo\RUNEFACTORY\01502.bin"); // OK
            //this.ImportTmpPicC4_C8(0x220, byTmp, 8, "01502", "C8_CI8");

            //byTmp = File.ReadAllBytes(this.baseFolder + @"rfo\rfo\RUNEFACTORY\01503.bin"); // OK
            //this.ImportTmpPicC4_C8(0x1E0, byTmp, 7, "01503", "C8_CI8");

            //byTmp = File.ReadAllBytes(this.baseFolder + @"rfo\rfo\RUNEFACTORY\01504.bin"); // OK
            //this.ImportTmpPicC4_C8(0x60, byTmp, 1, "01504", "C8_CI8");

            //byTmp = File.ReadAllBytes(this.baseFolder + @"rfo\rfo\RUNEFACTORY\01580.bin"); // OK
            //this.ImportTmpPicC4_C8(0x260, byTmp, 9, "01580", "C8_CI8");

            //byTmp = File.ReadAllBytes(this.baseFolder + @"rfo\rfo\RUNEFACTORY\01614.bin"); // OK
            //this.ImportTmpPicC4_C8(0xA0, byTmp, 2, "01614", "C8_CI8");

            //byTmp = File.ReadAllBytes(this.baseFolder + @"rfo\rfo\RUNEFACTORY\01582.bin"); // 不能使用原始的方式
            //this.ImportTmpPicC4_C8(0x120, byTmp, 4, "01582", "C4_CI4");

            //byTmp = File.ReadAllBytes(this.baseFolder + @"rfo\rfo\RUNEFACTORY\01582.bin");
            //this.ImportTmp01582(0x120, byTmp, 4);

            //byTmp = File.ReadAllBytes(this.baseFolder + @"rfo\rfo\RUNEFACTORY\01373.bin"); // OK
            //this.ImportTmpPicCMPR(0x60, byTmp, 1, "01373");
        }

        private void ImportTmpPicI4(int startPos, byte[] byTmp, int maxImgIdx, string fileName)
        {
            for (int i = 0; i < maxImgIdx; i++)
            {
                Bitmap testBmp = (Bitmap)Bitmap.FromFile(this.baseFolder + @"rfo\rfo\PicHanhua\Special\SpecialNew\Aft\" + fileName + "_" + i + ".png");
                byte[] byImg = Util.ImageEncode(testBmp, "I4");

                Array.Copy(byImg, 0, byTmp, startPos + i * byImg.Length, byImg.Length);
            }

            File.WriteAllBytes(this.baseFolder + @"rfo\rfo\PicHanhua\OKBin\" + fileName + ".bin", byTmp);
        }

        private void ImportTmpPicC4_C8(int startPos, byte[] byTmp, int maxImgIdx, string fileName, string imgFormat)
        {
            List<byte> byPalette = new List<byte>();
            int lastImgLen = 0;
            for (int i = 0; i < maxImgIdx; i++)
            {
                Bitmap testBmp = (Bitmap)Bitmap.FromFile(this.baseFolder + @"rfo\rfo\PicHanhua\Special\SpecialNew\Aft\" + fileName + "_" + i + ".png");
                byte[] byImg = Util.PaletteImageEncode(testBmp, imgFormat, byPalette, 2);

                Array.Copy(byImg, 0, byTmp, startPos + lastImgLen, byImg.Length);
                
                byte[] impPalette = byPalette.ToArray();
                Array.Copy(impPalette, 0, byTmp, startPos + lastImgLen + byImg.Length, impPalette.Length);

                lastImgLen += byImg.Length + impPalette.Length;
            }

            File.WriteAllBytes(this.baseFolder + @"rfo\rfo\PicHanhua\OKBin\" + fileName + ".bin", byTmp);
        }

        private void ImportTmp01582(int startPos, byte[] byTmp, int maxImgIdx)
        {
            byte[] by01582 = new byte[byTmp.Length + 768];
            List<byte> byPalette = new List<byte>();
            int lastImgLen = 0;

            // 复制头部信息
            Array.Copy(byTmp, 0, by01582, 0, 0x120);
            int totalLength = by01582.Length;
            by01582[0x1A] = (byte)(((totalLength - 0x20) >> 8) & 0xFF);
            by01582[0x1B] = (byte)(((totalLength - 0x20) >> 0) & 0xFF);
            by01582[0x1E] = (byte)((totalLength >> 8) & 0xFF);
            by01582[0x1F] = (byte)((totalLength >> 0) & 0xFF);

            by01582[0xA2] = 0x01;
            by01582[0xA3] = 0x20;
            by01582[0xAB] = 0x14;
            by01582[0xAF] = 0xE0;
            by01582[0xE2] = 0x06;
            by01582[0xE3] = 0x00;

            by01582[0xB2] = 0x06;
            by01582[0xB3] = 0x20;
            by01582[0xBB] = 0x14;
            by01582[0xBF] = 0xE0;
            by01582[0xF2] = 0x0B;
            by01582[0xF3] = 0x00;

            by01582[0xC2] = 0x0B;
            by01582[0xC3] = 0x20;
            by01582[0xCB] = 0x14;
            by01582[0xCF] = 0xE0;
            by01582[0x102] = 0x10;
            by01582[0x103] = 0x00;

            by01582[0xD2] = 0x10;
            by01582[0xD3] = 0x20;
            by01582[0xDB] = 0x14;
            by01582[0xDF] = 0xE0;
            by01582[0x112] = 0x15;
            by01582[0x113] = 0x00;


            for (int i = 0; i < maxImgIdx; i++)
            {
                Bitmap testBmp = (Bitmap)Bitmap.FromFile(this.baseFolder + @"rfo\rfo\PicHanhua\Special\SpecialNew\Aft\01582_" + i + ".png");
                byte[] byImg = Util.PaletteImageEncode(testBmp, "C4_CI4", byPalette, 2);

                Array.Copy(byImg, 0, by01582, startPos + lastImgLen, byImg.Length);

                byte[] impPalette = byPalette.ToArray();
                Array.Copy(impPalette, 0, by01582, startPos + lastImgLen + byImg.Length, impPalette.Length);

                lastImgLen += byImg.Length + impPalette.Length;
            }

            File.WriteAllBytes(this.baseFolder + @"rfo\rfo\PicHanhua\OKBin\01582.bin", by01582);
        }

        private void ImportTmpPicCMPR(int startPos, byte[] byTmp, int maxImgIdx, string fileName)
        {
            List<byte> byPalette = new List<byte>();
            int lastImgLen = 0;
            for (int i = 0; i < maxImgIdx; i++)
            {
                Bitmap testBmp = (Bitmap)Bitmap.FromFile(this.baseFolder + @"rfo\rfo\PicHanhua\Special\SpecialNew\Aft\" + fileName + "_" + i + ".png");
                byte[] byImg = Util.CmprImageEncode(testBmp);

                Array.Copy(byImg, 0, byTmp, startPos + lastImgLen, byImg.Length);

                lastImgLen += byImg.Length;
            }

            File.WriteAllBytes(this.baseFolder + @"rfo\rfo\PicHanhua\OKBin\" + fileName + ".bin", byTmp);
        }
    }
}
