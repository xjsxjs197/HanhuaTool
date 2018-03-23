using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using Hanhua.Common;

namespace Hanhua.TextEditTools.BioCvEdit
{
    /// <summary>
    /// 生化CvRdx工具
    /// </summary>
    public partial class BioCvTextAlignEdit : BaseForm
    {
        #region "全局变量"

        /// <summary>
        /// 字符宽度信息
        /// </summary>
        private string[] charLenInfo = null;

        /// <summary>
        /// 中文字库
        /// </summary>
        private string[] cnFontChars = null;

        private string subDisk;

        #endregion

        /// <summary>
        /// 构造方法
        /// </summary>
        public BioCvTextAlignEdit(string subDisk)
        {
            this.InitializeComponent();

            this.ResetHeight();

            //this.baseFolder = @"E:\Study\Hanhua\TodoCn\BioCv";
            this.baseFolder = @"E:\游戏汉化\NgcBioCv";
            this.subDisk = subDisk;

            this.charLenInfo = File.ReadAllLines(this.baseFolder + @"\BioCvNgcCn\Pic\NewFont\" + subDisk + @"\NewFontWidInfo.txt");
            this.cnFontChars = File.ReadAllLines(@"..\EncodeFenxi\BioTools\BioCvEdit\CnFontMap" + subDisk + ".txt", Encoding.GetEncoding("GB2312"));
        }

        #region " 页面事件 "

        #endregion

        #region " 共有方法 "

        /// <summary>
        /// 显示文本
        /// </summary>
        /// <param name="cnText"></param>
        public void AddText(string cnAllText)
        {
            string[] cnTexts = Regex.Split(cnAllText, "\n");
            int maxLen = cnTexts.Length;

            while (string.IsNullOrEmpty(cnTexts[maxLen - 1]))
            {
                maxLen--;
            }

            Bitmap backImg = new Bitmap(640, 1024);
            Graphics g = Graphics.FromImage(backImg);
            g.Clear(Color.Black);
            this.picText.Image = backImg;

            int curY = 0;
            for (int i = 0; i < maxLen; i++)
            {
                string cnText = cnTexts[i];

                // 将当前行文本编码
                curY = this.DrawText(cnText, backImg, curY);
            }
        }

        #endregion

        #region " 私有方法 "

        /// <summary>
        /// 将当前行文本编码
        /// </summary>
        /// <param name="text">当前行文本</param>
        /// <returns>中文编码后的文本</returns>
        private int DrawText(string text, Bitmap backImg, int curY)
        {
            List<byte> byData = new List<byte>();

            string currentChar;
            string nextChar;
            string[] keyWordLine;
            int charIndex;
            int curX = 0;

            StringBuilder keyWordsSb = new StringBuilder();
            for (int i = 0; i < text.Length - 1; i++)
            {
                currentChar = text.Substring(i, 1);
                if ("^" == currentChar)
                {
                    // 关键字的解码
                    keyWordsSb = new StringBuilder();
                    while ((nextChar = text.Substring(++i, 1)) != "^")
                    {
                        keyWordsSb.Append(nextChar);
                    }

                    keyWordLine = keyWordsSb.ToString().Split(' ');
                    for (int j = 0; j < keyWordLine.Length; j++)
                    {
                        if (keyWordLine[j] == "ff")
                        {
                            if (keyWordLine[j + 1] == "0" || keyWordLine[j + 1] == "00")
                            {
                                curY += 28;
                                curX = 0;
                                j++;
                            }
                            else if (keyWordLine[j + 1] == "1" || keyWordLine[j + 1] == "01")
                            {
                                curX += 28;
                                j++;
                            }
                            else if (keyWordLine[j + 1] == "2" || keyWordLine[j + 1] == "02")
                            {
                                curY += 28;
                                curX = 0;
                                j += 3;
                            }
                            else if (keyWordLine[j + 1] == "3" || keyWordLine[j + 1] == "03")
                            {
                                curX += 28;
                                j += 3;
                            }
                            else
                            {
                                j++;
                            }
                        }
                        else
                        {
                            j++;
                        }
                    }
                }
                else
                {
                    charIndex = this.EncodeChar(currentChar);
                    this.DrawChar(backImg, ref curX, curY, charIndex);
                }
            }

            return curY;
        }

        /// <summary>
        /// 取得当前文字的编码
        /// </summary>
        /// <param name="currenChar">当前文字</param>
        /// <returns>当前文字的编码</returns>
        private int EncodeChar(string currentChar)
        {
            // 在字库中查找
            for (int i = 0; i < this.cnFontChars.Length; i++)
            {
                if (this.cnFontChars[i].IndexOf(currentChar) >= 0)
                {
                    return i;
                }
            }

            throw new Exception("未查询到相应的中文字符 : " + currentChar);
        }

        /// <summary>
        /// 画字符图片
        /// </summary>
        /// <param name="backImg"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="charIndex"></param>
        private void DrawChar(Bitmap backImg, ref int bakX, int bakY, int charIndex)
        {
            int pageChars = 18 * 18;
            int currentIndex = charIndex % pageChars;
            string[] charWidInfo = this.charLenInfo[charIndex].Split(' ');
            int oldBakX = bakX;
            int maxX = 0;

            Bitmap pageImg = new Bitmap(this.baseFolder + @"\BioCvNgcCn\Pic\NewFont\" + this.subDisk + @"\NewFont" + (charIndex / pageChars) + ".png");
            int xPos = (currentIndex % 18) * 28 + Convert.ToInt32(charWidInfo[0]);
            int yPos = (currentIndex / 18) * 28;

            for (int y = yPos; y < yPos + 28 && bakY < backImg.Height; y++)
            {
                maxX = xPos + Convert.ToInt32(charWidInfo[1]);
                oldBakX = bakX;
                for (int x = xPos; x < maxX && bakX < backImg.Width && oldBakX < backImg.Width; x++)
                {
                    backImg.SetPixel(oldBakX++, bakY, pageImg.GetPixel(x, y));
                }
                bakY++;
            }

            bakX += Convert.ToInt32(charWidInfo[1]);
        }

        #endregion
    }
}
