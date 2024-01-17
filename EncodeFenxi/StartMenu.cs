using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Hanhua.CompressTools;
using Hanhua.FileViewer;
using Hanhua.FontEditTools;
using Hanhua.ImgEditTools;
using Hanhua.TextEditTools.Bio0Edit;
using Hanhua.TextEditTools.Bio1Edit;
using Hanhua.TextEditTools.Bio2Edit;
using Hanhua.TextEditTools.Bio3Edit;
using Hanhua.TextEditTools.BioAdt;
using Hanhua.TextEditTools.BioCvEdit;
using Hanhua.TextEditTools.TalesOfSymphonia;
using Hanhua.TextEditTools.TxtresEdit;
using Hanhua.TextEditTools.ViewtifulJoe;
using System.IO.Compression;
using Ionic.Zip;
using System.Xml;
using Hanhua.Common;
using System.Drawing.Drawing2D;

namespace Hanhua.Common
{
    /// <summary>
    /// 汉化入口
    /// </summary>
    public partial class StartMenu : BaseForm
    {
        #region " 私有变量 "

        /// <summary>
        /// 各种文本编辑的菜单
        /// </summary>
        private ContextMenuStrip txtEditorMenu = new ContextMenuStrip();

        /// <summary>
        /// 各种图片处理的菜单
        /// </summary>
        private ContextMenuStrip imgEditorMenu = new ContextMenuStrip();

        /// <summary>
        /// 各种字库处理的菜单
        /// </summary>
        private ContextMenuStrip fntEditorMenu = new ContextMenuStrip();

        /// <summary>
        /// 各种文件处理的菜单
        /// </summary>
        private ContextMenuStrip fileEditorMenu = new ContextMenuStrip();

        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        public StartMenu()
        {
            InitializeComponent();

            // 重新设置高度
            this.ResetHeight();

            // 设置弹出菜单
            this.SetContextMenu();

        }

        #region " 页面事件 "

        /// <summary>
        /// 字库处理工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFntTool_Click(object sender, EventArgs e)
        {
            Point p = Control.MousePosition;
            this.fntEditorMenu.Show(p);
        }

        /// <summary>
        /// 文件处理工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFileEdit_Click(object sender, EventArgs e)
        {
            Point p = Control.MousePosition;
            this.fileEditorMenu.Show(p);
        }

        /// <summary>
        /// 文本处理工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTxtTool_Click(object sender, EventArgs e)
        {
            Point p = Control.MousePosition;
            this.txtEditorMenu.Show(p);
        }

        /// <summary>
        /// 图片处理工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImgTool_Click(object sender, EventArgs e)
        {
            Point p = Control.MousePosition;
            this.imgEditorMenu.Show(p);
        }

        /// <summary>
        /// Ngc Iso工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNgcIso_Click(object sender, EventArgs e)
        {
            // 开始打补丁
            this.Do(this.ShowNgcIsoPatchView);
        }

        /// <summary>
        /// 文本编辑菜单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtEditorMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            this.txtEditorMenu.Visible = false;
            string strFolder = string.Empty;
            switch (e.ClickedItem.Name)
            {
                case "btnTxtSearch":
                    Fenxi fenxiForm = new Fenxi();
                    fenxiForm.Show(this);
                    break;

                case "btnTxtView":
                    SingleFileResult reFenxi = new SingleFileResult();
                    reFenxi.Show(); 
                    break;

                case "btnBio0Tool":
                    //string strFolder = @"E:\My\Hanhua\testFile\Biohazard_0\Cn";
                    strFolder = @"D:\game\iso\wii\生化危机0汉化\Wii版";
                    //string strFolder = @"D:\game\iso\wii\生化危机0汉化\Ngc版\A\root";
                    Bio0TextEditor bio0MoveEditor = new Bio0TextEditor(strFolder);
                    bio0MoveEditor.Show();
                    break;

                case "btnBio1Tool":
                    Bio1TextEditor bio1TextEditor = new Bio1TextEditor();
                    bio1TextEditor.Show();
                    break;

                case "btnBio2Tool":
                    Bio2TextEditor bio2TextEdit = new Bio2TextEditor();
                    bio2TextEdit.Show();
                    break;

                case "btnBio3Tool":
                    Bio3TextEditor bio3TextEdit = new Bio3TextEditor();
                    bio3TextEdit.Show();
                    break;

                case "btnBioCvTool":
                    BioCvTextEditor bioCvTextEditor = new BioCvTextEditor();
                    bioCvTextEditor.Show();
                    break;

                case "btnViewtifulTool":
                    ViewtifulJoeTextEditor viewtifulTool = new ViewtifulJoeTextEditor();
                    viewtifulTool.Show();
                    break;

                case "btnTos":
                    TalesOfSymphoniaTextEditor tosTool = new TalesOfSymphoniaTextEditor();
                    tosTool.Show();
                    break;

                case "btnChkCnChar":
                    this.baseFile = Util.SetOpenDailog("翻译文件（*.xlsx）|*.xlsx", string.Empty);
                    if (string.IsNullOrEmpty(this.baseFile))
                    {
                        return;
                    }
                    this.Do(this.CheckCnCharCount, new object[] { this.Text });
                    break;
            }
        }

        /// <summary>
        /// 图片编辑菜单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgEditorMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            this.imgEditorMenu.Visible = false;
            switch (e.ClickedItem.Name)
            {
                case "btnImgSearch":
                    ImgEditor imgEditor = new ImgEditor();
                    imgEditor.Show();
                    break;

                case "btnTplView":
                    // 打开要分析的文件
                    this.baseFile = Util.SetOpenDailog("Tpl 图片文件（*.tpl）|*.tpl|所有文件|*.*", string.Empty);
                    if (string.IsNullOrEmpty(this.baseFile))
                    {
                        return;
                    }

                    new TplFileManager().FindTplFromFile(this.baseFile);
                    break;

                case "btnPicEdit":
                    // 打开要分析的文件
                    this.baseFile = Util.SetOpenDailog("图片文件（*.png）|*.png|所有文件|*.*", string.Empty);
                    if (string.IsNullOrEmpty(this.baseFile))
                    {
                        return;
                    }

                    this.ShowPicEditView();
                    break;

                case "btnImgCreate":
                    //// 打开要分析的文件
                    //this.baseFile = Util.SetOpenDailog("图片文件（*.png）|*.png|所有文件|*.*", string.Empty);
                    //if (string.IsNullOrEmpty(this.baseFile))
                    //{
                    //    return;
                    //}

                    //SampleImgCreater imgCreater = new SampleImgCreater(this.baseFile);
                    //imgCreater.Show();
                    BaseImgForm baseImgForm = new BaseImgForm();
                    baseImgForm.Show();
                    break;

                case "btnBio2Adt":
                    BioAdtTool adtTool = new BioAdtTool();
                    adtTool.Show();
                    break;

                case "btnViewtifulTool":
                    ViewtifulJoePicEditor viewtifulJoePicEditor = new ViewtifulJoePicEditor();
                    viewtifulJoePicEditor.Show();
                    break;
            }
        }

        /// <summary>
        /// 字库编辑菜单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fntEditorMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            this.fntEditorMenu.Visible = false;
            string strFolder = string.Empty;
            switch (e.ClickedItem.Name)
            {
                case "btnWiiFntView":
                    // 打开要分析的字库文件
                    this.baseFile = Util.SetOpenDailog("Wii 字库文件（*.brfnt,*.bfn）|*.brfnt;*.bfn|所有文件|*.*", string.Empty);
                    if (string.IsNullOrEmpty(this.baseFile))
                    {
                        return;
                    }

                    // 开始分析字库
                    this.OpenWiiFontView();
                    break;

                case "btnWiiFntCreate":
                    // 打开要分析的字库文件
                    this.baseFile = Util.SetOpenDailog("Wii 字库文件（*.brfnt,*.bfn）|*.brfnt;*.bfn|所有文件|*.*", string.Empty);
                    if (string.IsNullOrEmpty(this.baseFile))
                    {
                        return;
                    }

                    // 开始分析字库
                    this.ShowCreateFontView();
                    break;

                case "btnBio0Fnt":
                    Bio0CnFontEditor bio0FontEditor = new Bio0CnFontEditor();
                    bio0FontEditor.Show();
                    break;

                case "btnBio1Fnt":
                    strFolder = @"D:\game\iso\wii\生化危机1汉化";
                    Bio1FontEditor tplFontEditor = new Bio1FontEditor(strFolder);
                    tplFontEditor.Show();
                    break;
            }
        }

        /// <summary>
        /// 文件编辑菜单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileEditorMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            this.fileEditorMenu.Visible = false;
            string strFolder = string.Empty;
            BaseCompTool compTool = null;
            BaseComp comp = null;
            switch (e.ClickedItem.Name)
            {
                case "btnTresEdit":
                    // 打开要分析的文件
                    this.baseFile = Util.SetOpenDailog("TXTRES文件（*.txtres, *.dat）|*.txtres;*.dat|所有文件|*.*", string.Empty);
                    if (string.IsNullOrEmpty(this.baseFile))
                    {
                        return;
                    }

                    this.Do(this.ShowTresEditerView);
                    break;

                case "btnSzsEdit":
                    // 打开要分析的文件
                    this.baseFile = Util.SetOpenDailog("SZS文件（*.szs）|*.szs|所有文件|*.*", string.Empty);
                    if (string.IsNullOrEmpty(this.baseFile))
                    {
                        return;
                    }

                    comp = new MarioYaz0Comp();
                    byte[] szsData = comp.Decompress(baseFile);
                    string strFileMagic = Util.GetHeaderString(szsData, 0, 3);
                    if ("RARC".Equals(strFileMagic))
                    {
                        TreeNode szsFileInfoTree = Util.RarcDecode(szsData);
                        SzsViewer szsViewForm = new SzsViewer(szsFileInfoTree, szsData, this.baseFile);
                        szsViewForm.Show(this);
                    }
                    else
                    {
                        MessageBox.Show("不是正常的szs文件 ： " + strFileMagic);
                    }
                    break;

                case "btnArcEdit":
                    // 打开要分析的文件
                    this.baseFile = Util.SetOpenDailog("ARC文件（*.arc）|*.arc|所有文件|*.*", string.Empty);
                    if (string.IsNullOrEmpty(this.baseFile))
                    {
                        return;
                    }

                    this.Do(this.ShowRarcView);
                    break;

                case "btnBio0LzEdit":
                    compTool = new BaseCompTool(new Bio0LzComp());
                    compTool.Show();
                    break;

                case "btnBioCvRdxEdit":
                    compTool = new BaseCompTool(new BioCvRdxComp());
                    compTool.Show();
                    break;

                case "btnBioCvAfsEdit":
                    BioCvAfsEditor afsTool = new BioCvAfsEditor();
                    afsTool.Show();
                    break;

                case "btnRleEdit":
                    compTool = new BaseCompTool(new ViewtifulJoeRleComp());
                    compTool.Show();
                    break;

                case "btnYayEdit":
                    compTool = new BaseCompTool(new MarioYay0Comp());
                    compTool.Show();
                    break;
            }
        }

        /// <summary>
        /// 自动编译Wii上模拟器Retroarch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAutoBuild_Click(object sender, EventArgs e)
        {
            this.AutoBuildRetroarch();
        }

        /// <summary>
        /// 测试按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTest_Click(object sender, EventArgs e)
        {
            //string[] enLines = File.ReadAllLines(@"E:\Study\Emu\emuSrc\mdTxtOld.txt");
            //string[] cnLines = File.ReadAllLines(@"E:\Study\Emu\emuSrc\mdTxt.txt");
            //List<string> zhMsg = new List<string>();
            //for (int i = 0; i < enLines.Length; i++)
            //{
            //    zhMsg.Add(enLines[i]);
            //    zhMsg.Add(cnLines[i]);
            //    zhMsg.Add(string.Empty);
            //}

            //File.WriteAllLines(@"E:\Study\Emu\emuSrc\zh.lang", zhMsg.ToArray(), Encoding.UTF8);

            //byte[] byData = File.ReadAllBytes(@"E:\Study\MySelfProject\Hanhua\TodoCn\EternalDarkness\InfoSK_ASC\Breakpoint_in_8013FC58_8014191C_when_JMnMenu.cmp_Loaded_in_5ADEC0.ram_dump");
            //byte[] armCode = new byte[0x13f40c - 0x13f29c];
            //Array.Copy(byData, 0x13f29c, armCode, 0, armCode.Length);
            //File.WriteAllBytes(@"E:\Study\MySelfProject\Hanhua\TodoCn\EternalDarkness\InfoSK_ASC\SourceAsmBin(8013F29C-8013F40C)", armCode);

            //byte[] byData = File.ReadAllBytes(@"E:\Study\MySelfProject\Hanhua\TodoCn\Bio4\main.dol");
            //byte[] armCode = new byte[0x2b73a0 - 0x2b6f00];
            //Array.Copy(byData, 0x2b6f00, armCode, 0, armCode.Length);
            //File.WriteAllBytes(@"E:\Study\MySelfProject\Hanhua\TodoCn\Bio4\\SourceAsmBin(2B6F00-2B73A0)", armCode);

            //byte[] byData = File.ReadAllBytes(@"E:\Study\MySelfProject\Hanhua\TodoCn\TalesOfSymphonia\cn\root\CV\cht_common\cht_321_013_a.ahx");
            //byte[] byCompress = new byte[0x9c5 - 0x24];
            //Array.Copy(byData, 0x24, byCompress, 0, byCompress.Length);
            //byCompress[0] = 2;
            //byCompress[1] = byData[0xf];
            //byCompress[2] = byData[0xe];
            //byCompress[3] = byData[0xd];
            //byCompress[4] = byData[0xc];

            //File.WriteAllBytes(@"E:\Study\MySelfProject\Hanhua\TodoCn\Bio4\Bio4Jp\files\St1\r120.das.decompLz", byCompress);
            //byte[] byTxt = Prs.Decompress(byCompress);
            //System.Diagnostics.Process exep = new System.Diagnostics.Process();
            //exep.StartInfo.FileName = @".\AdtDec.exe";
            //exep.StartInfo.CreateNoWindow = true;
            //exep.StartInfo.UseShellExecute = false;

            //exep.StartInfo.Arguments = @"E:\Study\MySelfProject\Hanhua\TodoCn\Bio4\Bio4Jp\files\St1\r120.das.decomp" + @" E:\dastst.bin";
            //exep.Start();
            //exep.WaitForExit();

            //this.WriteTtfFontPics();
            //this.TestCharPngDat();
            // create retroarch font
            //CheckPsZhTxt();
            //GetN64Name();
            //DecompressN64();
            //CheckColorMap();
            // retroarch
            //CheckNgcCnGameTitle();
            //ResetSfcRomName();
            //ResetFcRomName();
            //CheckSkAscComand();
            //this.CreateCpsEnCnTitle();
            //this.CheckJsonData();
            //this.CreateGameListFromFba();
            //this.WriteMgbaFont();
            //this.CopyBioFontWidthInfo();
            //this.ChangeNgcFileName();
            //CopyTelNo();
            //this.CreatePiGameList();
            //this.GetGodOfHandFont();
            //this.WriteGteConsts();
            //this.CheckTmpFont();
            //this.ImportRfo00905Text();
            this.ImportRfo01718Text();
            //this.CreateRfoFont();
            //this.CreateRfoFontCharMap();
            //this.CheckRfoFontInf();
            //this.CreateRfoFontImg();
            //this.CheckBof4Text();
            //this.ExportRfoText();
            //this.CheckRfoText();

        }

        #endregion

        #region " 私有方法 "

        private void CheckBof4Text()
        {
            List<FilePosInfo> allBof4Files = Util.GetAllFiles(@"G:\游戏汉化\Bof4\BIN");
            List<string> txtFile = new List<string>();
            foreach (FilePosInfo fi in allBof4Files)
            {
                if (fi.IsFolder || !fi.File.EndsWith(".EMI", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                byte[] byFile = File.ReadAllBytes(fi.File);
                int tmpIdx = 0x10;
                int fileCount = (byFile[0] << 0) + (byFile[1] << 8) + (byFile[2] << 16) + (byFile[3] << 24);
                while (fileCount-- >= 0)
                {
                    //if (byFile[tmpIdx + 4] == 0x00 && byFile[tmpIdx + 5] == 0x00 && byFile[tmpIdx + 6] == 0x01 && byFile[tmpIdx + 7] == 0x80) // text
                    if (byFile[tmpIdx + 4] == 0x10 && byFile[tmpIdx + 5] == 0x00 && byFile[tmpIdx + 6] == 0x00 && byFile[tmpIdx + 7] == 0x00) // tim ?
                    {
                        txtFile.Add(fi.File);
                    }
                    tmpIdx += 0x10;
                }
            }

            //File.WriteAllLines(@"G:\游戏汉化\Bof4\jpTextFile.txt", txtFile.ToArray(), Encoding.UTF8);
        }

        private void CheckRfoFontInf()
        {
            byte[] byFontInfo = File.ReadAllBytes(@"D:\游戏汉化\Wii\符文工房边境\00529.bin");
            int lastIdx = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append(lastIdx.ToString("X").PadLeft(2, '0')).Append(" ");
            sb.Append(byFontInfo[0].ToString("X").PadLeft(2, '0')).Append(byFontInfo[1].ToString("X").PadLeft(2, '0')).Append("--");
            for (int i = 0xa; i < byFontInfo.Length; i += 12)
            {
                //sb.Append(byFontInfo[i].ToString("X").PadLeft(2, '0')).Append(" ");
                //sb.Append(byFontInfo[i + 1].ToString("X").PadLeft(2, '0')).Append(" ");
                //sb.Append(byFontInfo[i + 2].ToString("X").PadLeft(2, '0')).Append(" ");
                //sb.Append(byFontInfo[i + 3].ToString("X").PadLeft(2, '0')).Append(" "); // pic pos
                //sb.Append(byFontInfo[i + 4].ToString("X").PadLeft(2, '0')).Append(" ");
                //sb.Append(byFontInfo[i + 5].ToString("X").PadLeft(2, '0')).Append(" ");
                //sb.Append(byFontInfo[i + 6].ToString("X").PadLeft(2, '0')).Append(" ");
                //sb.Append(byFontInfo[i + 7].ToString("X").PadLeft(2, '0')).Append(" ");
                //sb.Append(byFontInfo[i + 8].ToString("X").PadLeft(2, '0')).Append(" ");
                //sb.Append(byFontInfo[i + 9].ToString("X").PadLeft(2, '0')).Append(" ");
                //sb.Append(byFontInfo[i + 10].ToString("X").PadLeft(2, '0')).Append(" ");
                //sb.Append(byFontInfo[i + 11].ToString("X").PadLeft(2, '0')).Append(" ");
                if (byFontInfo[i + 3] != lastIdx)
                {
                    sb.Append(byFontInfo[i - 12].ToString("X").PadLeft(2, '0')).Append(byFontInfo[i - 12 + 1].ToString("X").PadLeft(2, '0')).Append("\r\n");

                    lastIdx = byFontInfo[i + 3];
                    sb.Append(lastIdx.ToString("X").PadLeft(2, '0')).Append(" ");
                    sb.Append(byFontInfo[i].ToString("X").PadLeft(2, '0')).Append(byFontInfo[i + 1].ToString("X").PadLeft(2, '0')).Append("--");
                }
            }
            sb.Append(byFontInfo[byFontInfo.Length - 12].ToString("X").PadLeft(2, '0')).Append(byFontInfo[byFontInfo.Length - 12 + 1].ToString("X").PadLeft(2, '0')).Append("\r\n");
        }

        /// <summary>
        /// 导出符文工房原文本
        /// </summary>
        private void ExportRfoText()
        {
            byte[] byOldTxt = File.ReadAllBytes(@"D:\游戏汉化\Wii\符文工房边境\01718.bin");
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

            File.WriteAllLines(@"D:\游戏汉化\Wii\符文工房边境\jpTxt.txt", jpTxt.ToArray(), Encoding.UTF8);
        }

        /// <summary>
        /// 导入符文工房中文文本
        /// </summary>
        private void ImportRfo00905Text()
        {
            byte[] byOldTxt = File.ReadAllBytes(@"D:\游戏汉化\Wii\符文工房边境\00905Old.bin");
            int txtTableStart = 0x054e38; // 当前位置的值(0x02ba18) + 0x054e18)
            int txtTableEnd   = 0x080824;
            int maxCnLen = 0x1a0f50 - 0x080830;
            int chTxtPos = 0x080830; // 减去0x054e18，结果放入Table表

            string[] cnTxtList = File.ReadAllLines(@"D:\游戏汉化\Wii\符文工房边境\cnTxt00905.txt", Encoding.UTF8);

            StringBuilder sb = new StringBuilder();
            List<byte> byCnData = new List<byte>();
            for (int i = 0; i < cnTxtList.Length; i += 2)
            {
                string strCnTxt = cnTxtList[i];
                if (string.IsNullOrEmpty(strCnTxt))
                {
                    break;
                }
                if (strCnTxt.Length <= 11)
                {
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
                File.WriteAllBytes(@"D:\游戏汉化\Wii\符文工房边境\00905.bin", byOldTxt);
                MessageBox.Show("正常写入中文翻译 ");
            }

        }

        /// <summary>
        /// 导入符文工房中文文本
        /// </summary>
        private void ImportRfo01718Text()
        {
            byte[] byOldTxt = File.ReadAllBytes(@"D:\游戏汉化\Wii\符文工房边境\01718Old.bin");
            int txtTableStart = 0xf8;
            int maxCnLen = 0x1a0f50 - 0x080830;
            int chTxtPos = 0xc8c;

            string[] cnTxtList = File.ReadAllLines(@"D:\游戏汉化\Wii\符文工房边境\cnTxt01718.txt", Encoding.UTF8);

            StringBuilder sb = new StringBuilder();
            List<byte> byCnData = new List<byte>();
            for (int i = 0; i < ((0xc8c - 0xf8) / 4 - 1) * 2; i += 2)
            {
                string strCnTxt = cnTxtList[i];
                if (string.IsNullOrEmpty(strCnTxt))
                {
                    break;
                }
                if (strCnTxt.Length <= 11)
                {
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
                txtTableStart += 4;
                chTxtPos += curCnData.Length;
                int tableIdx = chTxtPos + 0x20;
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
                File.WriteAllBytes(@"D:\游戏汉化\Wii\符文工房边境\01718.bin", byOldTxt);
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
        /// 计算符文工房中文文本的字符
        /// </summary>
        private List<string> CheckRfoText()
        {
            string[] cnTxtList = File.ReadAllLines(@"D:\游戏汉化\Wii\符文工房边境\allCnText.txt", Encoding.UTF8);
            List<string> lstCnTxt = new List<string>();
            for (int i = 0; i < cnTxtList.Length; i += 2)
            {
                string strCnTxt = cnTxtList[i];
                if (string.IsNullOrEmpty(strCnTxt))
                {
                    break;
                }
                if (strCnTxt.Length <= 11)
                {
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

            lstCnTxt.Sort();

            File.WriteAllLines(@"D:\游戏汉化\Wii\符文工房边境\fontChar.txt", lstCnTxt.ToArray(), Encoding.UTF8);

            return lstCnTxt;
        }

        /// <summary>
        /// 符文工房字库的字符映射
        /// </summary>
        private void CreateRfoFontCharMap()
        {
            string[] allFontChar = File.ReadAllLines(@"D:\游戏汉化\Wii\符文工房边境\fontChar.txt", Encoding.UTF8);
            int fileSize = allFontChar.Length * 12 + 10;
            int fontImgCount = 27;
            byte[] byNewCharMap = new byte[fileSize];
            byNewCharMap[0] = 0;
            byNewCharMap[1] = 0;
            byNewCharMap[2] = 0;
            byNewCharMap[3] = 0xa;
            byNewCharMap[4] = (byte)((allFontChar.Length >> 8) & 0xff);
            byNewCharMap[5] = (byte)((allFontChar.Length >> 0) & 0xff);
            byNewCharMap[6] = 0x01;
            byNewCharMap[7] = 0x00;
            byNewCharMap[8] = 0x00;
            byNewCharMap[9] = (byte)(fontImgCount & 0xff);

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
                    fontBmp = (Bitmap)(Bitmap.FromFile(@"D:\游戏汉化\Wii\符文工房边境\font\cnimportNew\tmpCnFont" + fontImgIdx + ".png"));
                    isLoadBmp = true;
                }
                if ((byChar[0] == 0 && tmpChar != " " && tmpChar != "　")
                    || tmpChar.Equals("。"))
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

                    byCharMap[4] = (byte)((leftX >> 8) & 0xff);
                    byCharMap[5] = (byte)((leftX >> 0) & 0xff);
                    byCharMap[8] = (byte)(((rightX) >> 8) & 0xff);
                    byCharMap[9] = (byte)(((rightX) >> 0) & 0xff);
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

            File.WriteAllBytes(@"D:\游戏汉化\Wii\符文工房边境\00529.bin", byNewCharMap);
            
        }
        
        /// <summary>
        /// 符文工房字库做成
        /// </summary>
        private void CreateRfoFont()
        {
            int imgWH = 256;
            byte[] by00530 = File.ReadAllBytes(@"D:\游戏汉化\Wii\符文工房边境\00530Old.bin");
            int fontImgCnt = 27;
            byte[] byNewFont = new byte[0x20 + 0x20 + 0x10 + (imgWH * imgWH / 2 + 0x20 + 0x10) * fontImgCnt];

            // set header
            Array.Copy(by00530, byNewFont, 0x20);
            byNewFont[0xf] = (byte)(fontImgCnt);
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

            for (int i = 0; i < fontImgCnt; i++)
            {
                int tmpFontPos = 0x20 + i * 0x20;
                int tmpImgPos = 0x20 + fontImgCnt * 0x20 + i * 0x10;

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
                int fontInfoPos = 0x20 + 0x10 + (0x20 + 0x10) * fontImgCnt + i * (imgWH * imgWH / 2);
                byNewFont[tmpImgPos + 0x0] = (byte)((fontInfoPos >> 24) & 0xff);
                byNewFont[tmpImgPos + 0x1] = (byte)((fontInfoPos >> 16) & 0xff);
                byNewFont[tmpImgPos + 0x2] = (byte)((fontInfoPos >> 8) & 0xff);
                byNewFont[tmpImgPos + 0x3] = (byte)((fontInfoPos >> 0) & 0xff);

                // set image data
                byte[] byAddedImgData = Util.ImageEncode((Bitmap)(Bitmap.FromFile(@"D:\游戏汉化\Wii\符文工房边境\Font\cnimportNew\tmpCnFont" + i + ".png")), "I4");
                Array.Copy(byAddedImgData, 0, byNewFont, fontInfoPos, byAddedImgData.Length);
            }

            // set footer
            Array.Copy(by00530, 0x983c0, byNewFont, byNewFont.Length - 0x20, 0x20);

            File.WriteAllBytes(@"D:\游戏汉化\Wii\符文工房边境\00530.bin", byNewFont);
        }

        /// <summary>
        /// 符文工房字库图片做成
        /// </summary>
        private void CreateRfoFontImg()
        {
            // 生成所有BigEndian的中文字符
            string[] allFontChar = File.ReadAllLines(@"D:\游戏汉化\Wii\符文工房边境\fontChar.txt", Encoding.UTF8);
            List<string> lstBuf = new List<string>();
            lstBuf.AddRange(allFontChar);
            //lstBuf.Sort((p1, p2) =>
            //{
            //    byte[] byChar = Encoding.BigEndianUnicode.GetBytes(p1);
            //    int charCode1 = (((int)byChar[0] << 8) | (int)byChar[1]);
            //    byChar = Encoding.BigEndianUnicode.GetBytes(p2);
            //    int charCode2 = (((int)byChar[0] << 8) | (int)byChar[1]);
            //    return charCode1 - charCode2;

            //});
            //File.WriteAllLines(@"E:\Game\ISO\Wii\rfo\rfo\fontChar.txt", lstBuf.ToArray(), Encoding.UTF8);
            //char[] chTxt = string.Join("", lstBuf.ToArray()).ToCharArray();

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
                tmpFontImg.Save(@"D:\游戏汉化\Wii\符文工房边境\Font\cnimportOld\tmpCnFont" + (fontIdx++) + ".png");
            }
        }

        private void CheckTmpFont()
        {
            byte[] byData = File.ReadAllBytes(@"E:\Game\ISO\Wii\rfo\rfo\RUNEFACTORY\00532.bin");
            int tmpPicSize = 256 * 256 / 2;
            byte[] byTmpPic = new byte[tmpPicSize];

            Array.Copy(byData, 0x60, byTmpPic, 0, tmpPicSize);
            Bitmap image = new Bitmap(256, 256);
            image = Util.ImageDecode(image, byTmpPic, "I4");
            image.Save(@"E:\Game\tmpFont530\newTmpFont532.png");


            //byte[] byData = File.ReadAllBytes(@"E:\Game\ISO\Wii\rfo\rfo\00530.bin");
            ////byte[] byTmpCn = new byte[] { 0x6c, 0xa1, 0x67, 0x09, 0x76, 0xf8, 0x51, 0x73, 0x5b, 0x58, 0x68, 0x63, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            ////Array.Copy(byTmpCn, 0, byData, 0x2183c, byTmpCn.Length);
            ////File.WriteAllBytes(@"E:\Game\ISO\Wii\rfo\rfo\RUNEFACTORY\01718.bin", byData);

            ////byte[] byData = File.ReadAllBytes(@"E:\Game\0x267e9800.bin");
            //int tmpPicSize = 256 * 256 / 2;
            //byte[] byTmpPic = new byte[tmpPicSize];

            //for (int i = 0; i < 25; i++)
            //{
            //    Array.Copy(byData, 0x4e0 + i * tmpPicSize, byTmpPic, 0, tmpPicSize);
            //    Bitmap image = new Bitmap(256, 256);
            //    image = Util.ImageDecode(image, byTmpPic, "I4");
            //    image.Save(@"E:\Game\tmpFont530\cntest\tmpFont" + i + ".png");
            //}

            //byte[] byData = File.ReadAllBytes(@"E:\Game\00530.bin");
            //int tmpPicSize = 256 * 256 / 2;
            //byte[] byTmpPic = new byte[tmpPicSize];

            //for (int i = 0; i < 19; i++)
            //{
            //    Array.Copy(byData, 0x3c0 + i * tmpPicSize, byTmpPic, 0, tmpPicSize);
            //    Bitmap image = new Bitmap(256, 256);
            //    image = Util.ImageDecode(image, byTmpPic, "I4");
            //    image.Save(@"E:\Game\tmpFont530\tmpFont" + i + ".png");
            //}

            //byte[] byData = File.ReadAllBytes(@"E:\Game\ISO\Wii\符文工房边境\RUNEFACTORY.dat");
            //byte[] byChk = new byte[] { 0x48, 0x46, 0x4e, 0x54, 0x30, 0x30, 0x30, 0x32 };
            //for (int i = 0; i < byData.Length; i++)
            //{
            //    if (byData[i] == byChk[0]
            //        && byData[i + 1] == byChk[1]
            //        && byData[i + 2] == byChk[2]
            //        && byData[i + 3] == byChk[3]
            //        && byData[i + 4] == byChk[4]
            //        && byData[i + 5] == byChk[5]
            //        && byData[i + 6] == byChk[6]
            //        && byData[i + 7] == byChk[7])
            //    {
            //        break;
            //    }
            //}

            //byte[] byData = new byte[] { 0x03, 0xb1, 0x03, 0xb2, 0x03, 0xb3 };
            ////string tmp = Encoding.GetEncoding("shift-jis").GetString(byData);
            //string tmp = Encoding.BigEndianUnicode.GetString(byData);


            //byte[] byPicPos = new byte[] { 0x00, 0x7f, 0x6a, 0x14 };
            //Bitmap image = new Bitmap(@"E:\Game\tmpFont\tmpFont" + byPicPos[0] + ".png");
            //Bitmap tmpImage = new Bitmap(21, 21);
            //int x1, y1;
            //x1 = 0;
            //y1 = 0;
            //for (int x = byPicPos[1]; x < byPicPos[1] + byPicPos[3]; x++)
            //{
            //    y1 = 0;
            //    for (int y = byPicPos[2]; y < byPicPos[2] + 21; y++)
            //    {
            //        tmpImage.SetPixel(x1, y1++, image.GetPixel(x, y));
            //    }
            //    x1++;
            //}
            //tmpImage.Save(@"E:\Game\tmpFont\tmpFont.png");
        }

        private void WriteGteConsts()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("static const u32 table15[] =\r\n");
            sb.Append("{\r\n");
            sb.Append("    ");
            for (int i = 1; i <= 32767; i++)
            {
                uint tmp = (uint)(((UInt64)65536 << 15) / (UInt64)i);
                sb.Append(tmp.ToString()).Append(", ");
                if (i % 16 == 0)
                {
                    sb.Append("\r\n");
                    sb.Append("    ");
                }
            }
            sb.Append("\r\n};\r\n");
            sb.Append("\r\n");

            sb.Append("static const u32 table16[] =\r\n");
            sb.Append("{\r\n");
            sb.Append("    ");
            for (int i = 32768; i <= 65536; i++)
            {
                uint tmp = (uint)(((UInt64)65536 << 16) / (UInt64)i);
                sb.Append(tmp.ToString()).Append(", ");
                if ((i - 32767) % 16 == 0)
                {
                    sb.Append("\r\n");
                    sb.Append("    ");
                }
            }
            sb.Append("\r\n};\r\n");
            sb.Append("\r\n");

            File.WriteAllText(@"G:\GitHub\WiiEmuHanhua\WiiSXRX_Dma\GteRtpsConsts.txt", sb.ToString(), Encoding.UTF8);
        }

        private void GetGodOfHandFont()
        {
            //byte[] tmpFile = File.ReadAllBytes(@"F:\game\iso\ps2\神之手\hanhua\core_jpn.bin");
            //int lastPos = 0;
            //int byStartPos = 0x4;
            //int nameStartPos = 0xa4;
            //for (int i = 0; i < 0x28; i++)
            //{
            //    int startPos = tmpFile[byStartPos] + (tmpFile[byStartPos + 1] << 8)
            //        + (tmpFile[byStartPos + 2] << 16) + (tmpFile[byStartPos + 3] << 24);
            //    byte[] newTim2 = new byte[startPos - lastPos];
            //    Array.Copy(tmpFile, startPos, newTim2, 0, newTim2.Length);

            //    lastPos = startPos;
            //    byStartPos += 4;

            //    string nameExt = Util.GetHeaderString(tmpFile, nameStartPos, nameStartPos + 2);
            //    nameStartPos += 4;
            //    File.WriteAllBytes(@"F:\game\iso\ps2\神之手\hanhua\core_jpn_Item" + i + "." + nameExt, newTim2);
            //}

            byte[] tmpFile = File.ReadAllBytes(@"F:\game\iso\ps2\神之手\hanhua\om00.dat");
            byte[] newTim2 = new byte[tmpFile.Length - 0x60];
            Array.Copy(tmpFile, 0x60, newTim2, 0, newTim2.Length);
            newTim2[3] = 0x32;

            File.WriteAllBytes(@"F:\game\iso\ps2\神之手\om00.tm2", newTim2);
        }

        private void CreatePiGameList()
        {
            string path = @"I:\Games\WiiSd\Roms\Fba\NEOGEO\";
            string imgPath0 = @"I:\Games\WiiSd\WiiSd\retroarch\thumbnails\fba_neogeo\Named_Boxarts\";
            string imgPath1 = @"I:\Games\WiiSd\WiiSd\retroarch\thumbnails\fba_neogeo\Named_Titles\";
            string imgPath2 = @"I:\Games\WiiSd\WiiSd\retroarch\thumbnails\fba_neogeo\Named_Snaps\";
            List<FilePosInfo> allFiles = Util.GetAllFiles(path);
            List<FilePosInfo> allImg0 = Util.GetAllFiles(imgPath0);
            List<FilePosInfo> allImg1 = Util.GetAllFiles(imgPath1);
            List<FilePosInfo> allImg2 = Util.GetAllFiles(imgPath2);
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\"?>\r\n");
            sb.Append("<gameList>\r\n");

            foreach (FilePosInfo file in allFiles)
            {
                if (file.IsFolder)
                {
                    continue;
                }

                string fileName = Util.GetShortFileName(file.File);
                string shortFileName = Util.GetShortNameWithoutType(file.File);
                string imgFile = this.GetImgFile(shortFileName, allImg0);
                if (string.IsNullOrEmpty(imgFile))
                {
                    imgFile = this.GetImgFile(shortFileName, allImg1);
                    if (string.IsNullOrEmpty(imgFile))
                    {
                        imgFile = this.GetImgFile(shortFileName, allImg2);
                    }
                }

                sb.Append("    <game>\r\n");
                sb.Append("        <path>./").Append(fileName).Append("</path>\r\n");
                sb.Append("        <name>").Append(shortFileName).Append("</name>\r\n");

                if (File.Exists(imgFile))
                {
                    string imgName = Util.GetShortFileName(imgFile);
                    File.Move(imgFile, @"I:\Games\WiiSd\images\neogeo\images\" + imgName);
                    sb.Append("        <image>./images/").Append(imgName).Append("</image>\r\n");
                }
                else
                {
                    sb.Append("        <image></image>\r\n");
                }

                sb.Append("    </game>\r\n");
            }

            sb.Append("</gameList>\r\n");
            File.WriteAllText(@"I:\Games\WiiSd\images\neogeo\gamelist.xml", sb.ToString(), Encoding.UTF8);
        }

        private string GetImgFile(string imgName, List<FilePosInfo> allImg)
        {
            foreach (FilePosInfo file in allImg)
            {
                if (file.IsFolder)
                {
                    continue;
                }

                if (file.File.IndexOf(imgName) > 0) 
                {
                    return file.File;
                }
            }

            return string.Empty;
        }

        private void CopyTelNo()
        {
            Microsoft.Office.Interop.Excel.Application xApp = null;
            Microsoft.Office.Interop.Excel.Workbook xBook = null;
            Microsoft.Office.Interop.Excel.Worksheet xSheet = null;

            try
            {
                Dictionary<string, string> telInfo = new Dictionary<string, string>();

                // 创建Application对象 
                xApp = new Microsoft.Office.Interop.Excel.ApplicationClass();

                // 得到WorkBook对象, 打开已有的文件 
                xBook = xApp.Workbooks._Open(
                    @"F:\IPMsg\员工花名册（裕日）.xlsx",
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                // 取得相应的Sheet
                for (int i = 1; i <= xBook.Sheets.Count; i++)
                {
                    xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets[i];
                    if (!"员工花名册".Equals(xSheet.Name))
                    {
                        continue;
                    }

                    for (int j = 3; j <= 154; j++)
                    {
                        string strKey = xSheet.get_Range("C" + j, Missing.Value).Value2 as string;
                        if (string.IsNullOrEmpty(strKey))
                        {
                            continue;
                        }

                        object strID = xSheet.get_Range("I" + j, Missing.Value).Value2;
                        if (strID != null)
                        {
                            telInfo.Add(strKey.Replace(" ", ""), strID.ToString());
                        }
                        else
                        {
                            telInfo.Add(strKey.Replace(" ", ""), "");
                        }
                    }
                }

                xApp.Quit();
                xApp = null;

                // 创建Application对象 
                xApp = new Microsoft.Office.Interop.Excel.ApplicationClass();

                // 得到WorkBook对象, 打开已有的文件 
                xBook = xApp.Workbooks._Open(
                    @"F:\IPMsg\拟复工人员信息表（西安裕日软件有限公司）.xlsx",
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                // 取得相应的Sheet
                for (int i = 1; i <= xBook.Sheets.Count; i++)
                {
                    xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets[i];
                    if (!"Sheet1".Equals(xSheet.Name))
                    {
                        continue;
                    }

                    for (int j = 2; j <= 150; j++)
                    {
                        string strKey = xSheet.get_Range("B" + j, Missing.Value).Value2 as string;
                        if (string.IsNullOrEmpty(strKey))
                        {
                            continue;
                        }

                        if (telInfo.ContainsKey(strKey))
                        {
                            xSheet.get_Range("C" + j, Missing.Value).Value2 = telInfo[strKey];
                        }
                    }
                }

                xBook.Save();
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
            }
            finally
            {
                // 清空各种对象
                xSheet = null;
                xBook = null;
                if (xApp != null)
                {
                    xApp.Quit();
                    xApp = null;
                }
            }
        }

        private void ChangeNgcFileName()
        {
            string path = @"G:\GitHub\HanhuaProject\Bio1\NgcCnB\";
            List<FilePosInfo> allFiles = Util.GetAllFiles(path);
            foreach (FilePosInfo file in allFiles)
            {
                if (file.IsFolder || !file.File.EndsWith("_cn.dat", StringComparison.OrdinalIgnoreCase)
                    || !File.Exists(file.File))
                {
                    continue;
                }

                File.Move(file.File, file.File.Replace("_cn.dat", ".dat"));
            }
        }

        private void CopyBioFontWidthInfo()
        {
            string ngcFile = @"E:\Study\MySelfProject\Hanhua\TodoCn\HanhuaProject\Bio1\NgcCnA\&&systemdata\Start.dol";
            string wiiFile = @"E:\Study\MySelfProject\Hanhua\TodoCn\HanhuaProject\Bio1\WiiCn\sys\main.dol";
            byte[] byNgc = File.ReadAllBytes(ngcFile);
            byte[] byWii = File.ReadAllBytes(wiiFile);

            int ngcStart = 0x17d9d0 + (32 * 34 + 2) * 2;
            int wiiStart = 0x2ee900 + (32 * 34 + 2) * 2;
            int copyLen = (32 * 29 + 6) * 2;

            Array.Copy(byWii, wiiStart, byNgc, ngcStart, copyLen);

            File.WriteAllBytes(ngcFile, byNgc);

            MessageBox.Show("Copy Font width info Ok");
        }

        /// <summary>
        /// 从Fba的列表中导出游戏列表
        /// </summary>
        private void CreateGameListFromFba()
        {
            string[] fbaList = File.ReadAllLines(@"E:\Study\Emu\emuSrc\WiiEmuHanhua\Retroarch_CnSrc\hbc\gameListFba.txt", Encoding.UTF8);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < fbaList.Length; i++)
            {
                int spaceIdx = fbaList[i].IndexOf('	');
                if (spaceIdx > 0)
                {
                    sb.Append("\r\n");
                    sb.Append("msgid \"").Append(fbaList[i].Substring(0, spaceIdx)).Append(".zip\"\r\n");
                    sb.Append("msgstr \"").Append(fbaList[i].Substring(spaceIdx + 1).Replace(" ", "")).Append("\"\r\n");
                }
                
            }

            File.WriteAllText(@"E:\Study\Emu\emuSrc\WiiEmuHanhua\Retroarch_CnSrc\hbc\gameListFba.lang", sb.ToString(), Encoding.UTF8);
        }

        /// <summary>
        /// 生成Cps中文名称列表
        /// </summary>
        private void CreateCpsEnCnTitle()
        {
            string[] enTitles = File.ReadAllLines(@"E:\Study\Emu\emuSrc\WiiEmuHanhua\Retroarch_CnSrc\hbc\Cps1EnTitle.txt");
            string[] cnTitles = File.ReadAllLines(@"E:\Study\Emu\emuSrc\WiiEmuHanhua\Retroarch_CnSrc\hbc\Cps1CnTitle.txt");
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < enTitles.Length; i++)
            {
                sb.Append("\r\n");
                sb.Append("msgid \"").Append(enTitles[i]).Append(".zip\"\r\n");
                sb.Append("msgstr \"").Append(cnTitles[i]).Append("\"\r\n");
            }

            enTitles = File.ReadAllLines(@"E:\Study\Emu\emuSrc\WiiEmuHanhua\Retroarch_CnSrc\hbc\Cps2EnTitle.txt");
            cnTitles = File.ReadAllLines(@"E:\Study\Emu\emuSrc\WiiEmuHanhua\Retroarch_CnSrc\hbc\Cps2CnTitle.txt");
            for (int i = 0; i < enTitles.Length; i++)
            {
                sb.Append("\r\n");
                sb.Append("msgid \"").Append(enTitles[i]).Append(".zip\"\r\n");
                sb.Append("msgstr \"").Append(cnTitles[i]).Append("\"\r\n");
            }

            File.WriteAllText(@"E:\Study\Emu\emuSrc\WiiEmuHanhua\Retroarch_CnSrc\hbc\zh.lang", sb.ToString(), Encoding.UTF8);
        }

        private void CheckJsonData()
        {
            Microsoft.Office.Interop.Excel.Application xApp = null;
            Microsoft.Office.Interop.Excel.Workbook xBook = null;
            Microsoft.Office.Interop.Excel.Worksheet xSheet = null;

            try
            {
                // 创建Application对象 
                xApp = new Microsoft.Office.Interop.Excel.ApplicationClass();

                // 得到WorkBook对象, 打开已有的文件 
                xBook = xApp.Workbooks._Open(
                    @"D:\renkeiJson.xlsx",
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                // 取得相应的Sheet
                for (int i = 1; i <= xBook.Sheets.Count; i++)
                {
                    xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets[i];
                    StringBuilder sb = new StringBuilder();
                    sb.Append("{\"code\":\"000000\",\"message\":\"成功\",\"serialNo\":168,\"data\":");
                    sb.Append("{");
                    //sb.Append("[{");

                    for (int j = 8; j <= 500; j++)
                    {
                        string strKey = xSheet.get_Range("C" + j, Missing.Value).Value2 as string;
                        if (string.IsNullOrEmpty(strKey))
                        {
                            break;
                        }

                        if (j > 8)
                        {
                            sb.Append(",");
                        }

                        sb.Append("\"").Append(strKey).Append("\":");

                        string type = xSheet.get_Range("D" + j, Missing.Value).Value2 as string;
                        object val = xSheet.get_Range("G" + j, Missing.Value).Value2;
                        if (("VARCHAR2".Equals(type) || "DATE".Equals(type)) && val != null)
                        {
                            sb.Append("\"");
                        }
                        
                        if (val == null)
                        {
                            sb.Append("null");
                        }
                        else
                        {
                            sb.Append(val.ToString());
                        }

                        if (("VARCHAR2".Equals(type) || "DATE".Equals(type)) && val != null)
                        {
                            sb.Append("\"");
                        }
                    }

                    sb.Append("}}");
                    //sb.Append("}]}");

                    File.WriteAllText(@"D:\jsonTestData\" + xSheet.Name + ".json", sb.ToString(), Encoding.UTF8);
                }
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
            }
            finally
            {
                // 清空各种对象
                xSheet = null;
                xBook = null;
                if (xApp != null)
                {
                    xApp.Quit();
                    xApp = null;
                }
            }
        }

        private void CheckSkAscComand()
        {
            Microsoft.Office.Interop.Excel.Application xApp = null;
            Microsoft.Office.Interop.Excel.Workbook xBook = null;
            Microsoft.Office.Interop.Excel.Worksheet xSheet = null;

            try
            {
                // 创建Application对象 
                xApp = new Microsoft.Office.Interop.Excel.ApplicationClass();

                // 得到WorkBook对象, 打开已有的文件 
                xBook = xApp.Workbooks._Open(
                    @"E:\Study\MySelfProject\Hanhua\TodoCn\EternalDarkness\8013FC58汇编代码分析.xlsx",
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                // 取得相应的Sheet
                for (int i = 1; i < xBook.Sheets.Count; i++)
                {
                    if (((Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets[i]).Name.Equals("All"))
                    {
                        xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets[i];
                        break;
                    }
                }

                int lineNum = 1;
                List<string> cmdList = new List<string>();

                for (int i = lineNum; i <= 2661; i++)
                {
                    string cellValue = xSheet.get_Range("B" + i, Missing.Value).Value2 as string;
                    if (string.IsNullOrEmpty(cellValue))
                    {
                        continue;
                    }

                    string[] cmds = cellValue.Split(' ');
                    if (!cmdList.Contains(cmds[0]))
                    {
                        cmdList.Add(cmds[0]);
                    }
                }
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
            }
            finally
            {
                // 隐藏进度条
                this.CloseProcessBar();

                // 清空各种对象
                xSheet = null;
                xBook = null;
                if (xApp != null)
                {
                    xApp.Quit();
                    xApp = null;
                }
            }
        }

        private void ResetSfcRomName()
        {
            Dictionary<string, string> cnNameMap = new Dictionary<string, string>();
            Dictionary<string, string> enNameMap = new Dictionary<string, string>();
            //this.baseFolder = @"E:\Study\Emu\Roms\Sfc\SFC_Jp(1444个)\snes\";
            this.baseFolder = @"H:\down\game\emu\Roms\SFC\SFC_Jp(1444个)\";
            //string pngPath = @"E:\Study\Emu\AllThumbnails\Sfc\";
            string pngPath = @"E:\Game\AllThumbnails\Sfc\";
            XmlDocument xmlDoc = new XmlDocument();

            this.LoadSfcNameMap(xmlDoc, enNameMap, @"gamelist.xml.en");
            this.LoadSfcNameMap(xmlDoc, cnNameMap, @"gamelist.xml.cn");

            foreach (KeyValuePair<string, string> pathName in enNameMap)
            {
                string pngFile = pngPath + @"Named_Titles\" + pathName.Value + ".png";
                if (File.Exists(pngFile))
                {
                    File.Copy(pngFile, pngFile.Replace(@"Sfc\", @"thumbnails\nintendo_sfc\").Replace(pathName.Value, cnNameMap[pathName.Key]), true);
                }

                pngFile = pngPath + @"Named_Snaps\" + pathName.Value + ".png";
                if (File.Exists(pngFile))
                {
                    File.Copy(pngFile, pngFile.Replace(@"Sfc\", @"thumbnails\nintendo_sfc\").Replace(pathName.Value, cnNameMap[pathName.Key]), true);
                }

                pngFile = pngPath + @"Named_Boxarts\" + pathName.Value + ".png";
                if (File.Exists(pngFile))
                {
                    File.Copy(pngFile, pngFile.Replace(@"Sfc\", @"thumbnails\nintendo_sfc\").Replace(pathName.Value, cnNameMap[pathName.Key]), true);
                }
            }
        }

        private void ResetFcRomName()
        {
            this.baseFolder = @"E:\Study\Emu\Roms\Fc\FcAll\unZip\";
            List<FilePosInfo> allFc = Util.GetAllFiles(baseFolder).Where(p => !p.IsFolder && p.File.EndsWith(".nes", StringComparison.OrdinalIgnoreCase)).ToList();
            string pngPath = @"E:\Study\Emu\AllThumbnails\Fc\";

            Dictionary<string, string> cnNameMap = new Dictionary<string, string>();
            this.baseFolder = @"E:\Study\Emu\Roms\Fc\FcAll\nes\";
            XmlDocument xmlDoc = new XmlDocument();

            this.LoadSfcNameMap(xmlDoc, cnNameMap, @"gamelist(cn).xml");

            foreach (FilePosInfo file in allFc)
            {
                string enName = Util.GetShortNameWithoutType(file.File);
                string zipFile = file.File.Substring(file.File.IndexOf(enName) - 6, 5) + ".zip";
                string pngFile = pngPath + @"Named_Titles\" + enName + ".png";
                if (File.Exists(pngFile))
                {
                    File.Copy(pngFile, pngFile.Replace(@"Fc\", @"thumbnails\nintendo_fc\").Replace(enName, cnNameMap[zipFile]), true);
                }

                pngFile = pngPath + @"Named_Snaps\" + enName + ".png";
                if (File.Exists(pngFile))
                {
                    File.Copy(pngFile, pngFile.Replace(@"Fc\", @"thumbnails\nintendo_fc\").Replace(enName, cnNameMap[zipFile]), true);
                }

                pngFile = pngPath + @"Named_Boxarts\" + enName + ".png";
                if (File.Exists(pngFile))
                {
                    File.Copy(pngFile, pngFile.Replace(@"Fc\", @"thumbnails\nintendo_fc\").Replace(enName, cnNameMap[zipFile]), true);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="cnNameMap"></param>
        private void LoadSfcNameMap(XmlDocument xmlDoc, Dictionary<string, string> nameMap, string xml)
        {
            xmlDoc.Load(this.baseFolder + xml);

            XmlNode xmlInfo = xmlDoc.SelectSingleNode("/gameList");
            // 显示进度条
            //this.ResetProcessBar(xmlInfo.ChildNodes.Count);

            foreach (XmlNode item in xmlInfo.ChildNodes)
            {
                if (item.NodeType == XmlNodeType.Element)
                {
                    string filePath = string.Empty;
                    string fileName = string.Empty;
                    string fileImg = string.Empty;
                    foreach (XmlNode subItem in item.ChildNodes)
                    {
                        if ("path".Equals(subItem.Name))
                        {
                            filePath = subItem.InnerText;
                        }
                        else if ("name".Equals(subItem.Name))
                        {
                            fileName = subItem.InnerText;
                        }
                        else if ("image".Equals(subItem.Name))
                        {
                            fileImg = subItem.InnerText;
                        }
                    }

                    filePath = filePath.Replace("./", @"\");
                    if (filePath.IndexOf(fileName) < 0)
                    {
                        nameMap.Add(filePath, fileName);
                    }
                    //fileImg = fileImg.Replace("./", @"\").Replace("/", @"\");
                    //if (File.Exists(this.baseFolder + filePath))
                    //{
                    //    File.Copy(this.baseFolder + filePath, (this.baseFolder + filePath).Replace("snes", "snesCn").Replace(filePath, @"\" + fileName + ".zip"), true);

                    //    if (File.Exists(this.baseFolder + fileImg))
                    //    {
                    //        File.Copy(this.baseFolder + fileImg, (this.baseFolder + fileImg).Replace("snes", "snesCn").Replace(fileImg, @"\images\" + fileName + @".jpg"), true);
                    //    }
                    //}
                }

                // 更新进度条
                //this.ProcessBarStep();
            }

            // 隐藏进度条
            //this.CloseProcessBar();
        }

        private void CheckNgcCnGameTitle()
        {
            List<string> cnTitles = new List<string>();
            string[] titles = File.ReadAllLines(@"E:\Study\Emu\emuSrc\WiiEmuHanhua\Nintendont\nintendont\titles_Old.txt", Encoding.UTF8);
            string[] titles2 = File.ReadAllLines(@"E:\Study\Emu\emuSrc\WiiEmuHanhua\Nintendont\nintendont\titles2.txt", Encoding.UTF8);

            foreach (string title in titles2)
            {
                if (string.IsNullOrEmpty(title))
                {
                    continue;
                }

                string curTitle = title.Substring(0, 3);
                if (string.IsNullOrEmpty(cnTitles.FirstOrDefault(p => p.StartsWith(curTitle, StringComparison.Ordinal))))
                {
                    cnTitles.Add(title);
                }
            }

            foreach (string title in titles)
            {
                if (string.IsNullOrEmpty(title))
                {
                    continue;
                }

                string curTitle = title.Substring(0, 3);
                if (string.IsNullOrEmpty(cnTitles.FirstOrDefault(p => p.StartsWith(curTitle, StringComparison.Ordinal))))
                {
                    cnTitles.Add(title);
                }
            }

            

            cnTitles.Sort();

            string[] cnEnTitles = File.ReadAllLines(@"E:\Study\Emu\emuSrc\WiiEmuHanhua\Nintendont\nintendont\CnEnGameName.txt", Encoding.UTF8);
            for (int i = 0; i < cnTitles.Count; i++)
            {
                string enTitle = cnTitles[i];
                string cnEnTitle = cnEnTitles.FirstOrDefault(p => p.IndexOf(enTitle.Substring(4)) > 0);
                if (!string.IsNullOrEmpty(cnEnTitle))
                {
                    cnTitles[i] = (enTitle.Substring(0, 4) + cnEnTitle).Replace("(美)", "").Replace("(日)", "").Replace("(欧)", "")
                        .Replace("A.STG", "").Replace("ACT", "").Replace("SPG", "").Replace("STG", "").Replace("RAC", "").Replace("RPG", "")
                        .Replace("FTG", "").Replace("PUZ", "").Replace("SIM", "");
                }

            }

            File.WriteAllLines(@"E:\Study\Emu\emuSrc\WiiEmuHanhua\Nintendont\nintendont\titles.txt", cnTitles.ToArray(), Encoding.UTF8);
        }

        /// <summary>
        /// 检查Retroarch字库的白、绿字库的颜色映射
        /// </summary>
        private void CheckColorMap()
        {
            byte[] whiteColor = File.ReadAllBytes(@"H:\游戏汉化\fontTest\ZhBufFont14X14NoBlock_RGB5A3.dat");
            byte[] greenColor = File.ReadAllBytes(@"H:\游戏汉化\fontTest\ZhBufFont14X14NoBlock_RGB5A3_R.dat");
            int charImgSize = 14 * 14 * 2; // 13 * 13;
            StringBuilder sb = new StringBuilder();
            Dictionary<int, int> colorMap = new Dictionary<int, int>();
            for (int i = 4; i < whiteColor.Length; )
            {
                for (int j = i; j < i + charImgSize; j += 2)
                {
                    int key = whiteColor[j] << 8 | whiteColor[j + 1];
                    if (!colorMap.ContainsKey(key))
                    {
                        int val = greenColor[j] << 8 | greenColor[j + 1];
                        colorMap.Add(key, val);
                        sb.Append("colorMap.insert(std::pair<uint16_t, uint16_t>(");
                        sb.Append(key);
                        sb.Append(", ");
                        sb.Append(val);
                        sb.Append("));\r\n");
                    }
                }

                i += charImgSize + 4;
            }
        }

        private void AutoBuildRetroarch()
        {
            //string basePath = @"E:\Study\Emu\emuSrc\RetroArch\libretro-super-master\retroarch\";
            string basePath = @"H:\down\game\emuSrc\RetroArch\RetroArch-1.9.4\";
            
            System.Diagnostics.Process exep = new System.Diagnostics.Process();
            exep.StartInfo.FileName = @"make";
            exep.StartInfo.CreateNoWindow = true;
            exep.StartInfo.UseShellExecute = false;
            exep.StartInfo.WorkingDirectory = basePath;
            exep.StartInfo.Arguments = @"-f Makefile.griffin platform=wii";

            List<FilePosInfo> allWiiLib = Util.GetAllFiles(basePath + @"dist-scripts\").Where(p => p.File.EndsWith("_wii.a", StringComparison.OrdinalIgnoreCase)).ToList();
            // 显示进度条
            this.ResetProcessBar(allWiiLib.Count);

            foreach (FilePosInfo fileInfo in allWiiLib)
            {
                // Delete file
                File.Delete(basePath + @"libretro_wii.a");
                if (File.Exists(basePath + @"retroarch_wii.dol"))
                {
                    File.Delete(basePath + @"retroarch_wii.dol");
                }
                if (File.Exists(basePath + @"retroarch_wii.elf"))
                {
                    File.Delete(basePath + @"retroarch_wii.elf");
                }
                if (File.Exists(basePath + @"retroarch_wii.elf.map"))
                {
                    File.Delete(basePath + @"retroarch_wii.elf.map");
                }

                // copy File
                File.Copy(fileInfo.File, basePath + @"libretro_wii.a", true);

                // build file
                exep.Start();
                exep.WaitForExit();

                // move File
                if (File.Exists(basePath + @"retroarch_wii.dol"))
                {
                    File.Move(basePath + @"retroarch_wii.dol", fileInfo.File.Replace(".a", ".dol").Replace("dist-scripts", @"pkg\wii"));
                }

                // 更新进度条
                this.ProcessBarStep();
            }

            // 隐藏进度条
            this.CloseProcessBar();
        }

        private void DecompressN64()
        {
            string baseFol = @"H:\down\game\emu\Roms\N64\5";
            List<FilePosInfo> allN64 = Util.GetAllFiles(baseFol).Where(p => !p.IsFolder && p.File.EndsWith(".zip", StringComparison.OrdinalIgnoreCase)).ToList();
            foreach (FilePosInfo zipFile in allN64)
            {
                string targetName = baseFol + @"\Unzip\" + Util.GetShortNameWithoutType(zipFile.File) + ".z64";
                string unZipPath = baseFol + @"\Unzip\" + Util.GetShortNameWithoutType(zipFile.File);
                List<FilePosInfo> unZipFiles = Util.GetAllFiles(unZipPath);
                foreach (FilePosInfo tmp in unZipFiles)
                {
                    if (tmp.IsFolder)
                    {
                        continue;
                    }

                    File.Move(tmp.File, targetName);
                }
            }
        }

        List<string> GetN64Name()
        {
            List<FilePosInfo> allN64 = Util.GetAllFiles(@"E:\Study\Emu\Roms\N64Roms");
            List<string> allN64Char = new List<string>();
            foreach(FilePosInfo fileInfo in allN64)
            {
                string fileName = Util.GetShortNameWithoutType(fileInfo.File);
                for (int i = 0; i < fileName.Length; i++)
                {
                    string curChar = fileName.Substring(i, 1);
                    if (!allN64Char.Contains(curChar))
                    {
                        allN64Char.Add(curChar);
                    }
                }
            }

            string allCnTxt = File.ReadAllText(@"E:\Study\Emu\emuSrc\Not64\Wii64-beta1.2(fix94)_20171121\lang\zh.lang");
            for (int i = 0; i < allCnTxt.Length; i++)
            {
                string curChar = allCnTxt.Substring(i, 1);
                if (!allN64Char.Contains(curChar))
                {
                    allN64Char.Add(curChar);
                }
            }

            return allN64Char;
        }

        /// <summary>
        /// 生成所有BigEndian的中文字符
        /// </summary>
        /// <returns></returns>
        private List<int> GetBigEndianAllCnChars()
        {
            List<int> allZhTxt = new List<int>();

            // 生成Ascii码文字
            StringBuilder sb = new StringBuilder();
            List<string> lstBuf = new List<string>();
            for (int i = 0x20; i <= 0x7e; i++)
            {
                string tmpStr = Encoding.GetEncoding(932).GetString(new byte[] { (byte)i });
                sb.Append(tmpStr);
                lstBuf.Add(tmpStr);
            }

            //char[] chTxt = sb.Append(Util.CreateOneLevelHanzi()).Append(Util.CreateTwoLevelHanzi()).ToString().ToCharArray();
            //char[] chTxt = sb.Append(File.ReadAllText(@"H:\游戏汉化\fontTest\ComnCnChar.txt", Encoding.UTF8)).ToString().ToCharArray();

            //this.ReadChChar(@"H:\游戏汉化\fontTest\ComnCnChar.txt", lstBuf);
            this.ReadChChar(@"H:\down\game\emuSrc\RetroArch\libretro-super-master\RetroArch-1.8.4\intl\msg_hash_chs.c", lstBuf);
            this.ReadChChar(@"H:\down\game\emuSrc\RetroArch\libretro-super-master\RetroArch-1.8.4\intl\msg_hash_chs.h", lstBuf);
            this.ReadChChar(@"G:\GitHub\WiiEmuHanhua\Retroarch_CnSrc\hbc\zh.lang", lstBuf);
            this.ReadChChar(@"H:\down\game\emu\Roms\retroarch\playlists\nintendo_fc.lpl", lstBuf);
            this.ReadChChar(@"H:\down\game\emu\Roms\retroarch\playlists\nintendo_sfc.lpl", lstBuf);
            this.ReadChChar(@"H:\down\game\emu\Roms\retroarch\playlists\nintendo_gba.lpl", lstBuf);
            this.ReadChChar(@"H:\down\game\emu\Roms\retroarch\playlists\sega_md.lpl", lstBuf);
            this.ReadChChar(@"H:\down\game\emu\Roms\retroarch\playlists\fba_Pgm_PSIKYO.lpl", lstBuf);
            this.ReadChChar(@"H:\down\game\emu\Roms\retroarch\playlists\mame2003_coreA.lpl", lstBuf);
            this.ReadChChar(@"H:\down\game\emu\Roms\retroarch\playlists\mame2003_coreB.lpl", lstBuf);
            this.ReadChChar(@"H:\down\game\emu\Roms\retroarch\playlists\mame2003_coreC.lpl", lstBuf);
            this.ReadChChar(@"H:\down\game\emu\Roms\retroarch\playlists\mame2003_coreD.lpl", lstBuf);
            this.ReadChChar(@"H:\down\game\emu\Roms\retroarch\playlists\mame2003_coreE.lpl", lstBuf);
            this.ReadChChar(@"H:\down\game\emu\Roms\retroarch\playlists\mame2003_coreF.lpl", lstBuf);
            this.ReadChChar(@"H:\down\game\emu\Roms\retroarch\playlists\mame2003_coreG.lpl", lstBuf);
            char[] chTxt = string.Join("", lstBuf.ToArray()).ToCharArray();

            foreach (char chChar in chTxt)
            {
                byte[] byChar = Encoding.BigEndianUnicode.GetBytes(new char[] { chChar });
                int temp = byChar[0] << 8 | byChar[1];
                if (temp < 0x20)
                {
                    continue;
                }

                allZhTxt.Add(temp);
            }

            return allZhTxt;
        }

        private void ReadChChar(string file, List<string> lstBuf)
        {
            string tmp = File.ReadAllText(file, Encoding.UTF8);
            for (int i = 0; i < tmp.Length; i++)
            {
                string tmpChar = tmp.Substring(i, 1);
                if (!lstBuf.Contains(tmpChar))
                {
                    lstBuf.Add(tmpChar);
                }
            }
        }

        /// <summary>
        /// Mgba字库做成
        /// </summary>
        private void WriteMgbaFont()
        {
            // 生成所有BigEndian的中文字符
            List<int> allZhTxt = this.GetBigEndianAllCnChars();

            allZhTxt.Sort();

            List<byte> charIndexMap = new List<byte>();

            ImgInfo imgInfo = new ImgInfo(32, 32);
            imgInfo.BlockImgH = 32;
            imgInfo.BlockImgW = 32;
            imgInfo.NeedBorder = true;
            imgInfo.FontStyle = FontStyle.Bold;
            imgInfo.FontSize = 18;
            imgInfo.Brush = Brushes.White;
            imgInfo.Pen = new Pen(Color.Black, 0.1F);

            // 显示进度条
            this.ResetProcessBar(allZhTxt.Count);

            int charIndex = 0;
            foreach (int unicodeChar in allZhTxt)
            {
                imgInfo.NewImg();
                imgInfo.CharTxt = Encoding.BigEndianUnicode.GetString(new byte[] { (byte)(unicodeChar >> 8 & 0xFF), (byte)(unicodeChar & 0xFF) });
                imgInfo.XPadding = 0;
                imgInfo.YPadding = 0;
                imgInfo.Grp.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                ImgUtil.WriteTextBlockImg(imgInfo);

                // 保存字符映射表信息
                byte[] byChar = Encoding.BigEndianUnicode.GetBytes(imgInfo.CharTxt);
                byte[] byCurChar = new byte[32];
                byCurChar[0] = byChar[0];
                byCurChar[1] = byChar[1];

                // 保存图片宽度信息，以及重新生成图片（紧靠左边）
                imgInfo.Bmp = this.SetCharPadding(byCurChar, imgInfo.Bmp);

                charIndexMap.AddRange(byCurChar);

                if (charIndex++ < 500)
                {
                    imgInfo.Bmp.Save(@"E:\Study\MySelfProject\Hanhua\fontTest\CharPng\" + unicodeChar + ".png");
                }

                // 保存文字图片信息
                byte[] byCharFont = Util.ImageEncode(imgInfo.Bmp, "IA4");
                charIndexMap.AddRange(byCharFont);

                // 更新进度条
                this.ProcessBarStep();
            }

            // 隐藏进度条
            this.CloseProcessBar();

            File.WriteAllBytes(@"E:\Study\MySelfProject\Hanhua\fontTest\mGba_CnFont_I4.dat", charIndexMap.ToArray());
        }

        private void CheckPsZhTxt()
        {
            // 生成所有BigEndian的中文字符
            List<int> allZhTxt = this.GetBigEndianAllCnChars();
            
            string[] allLine = File.ReadAllLines(@"H:\游戏汉化\fontTest\zhChCount.xlsx.txt", Encoding.UTF8);
            //string[] allLine = File.ReadAllLines(@"E:\Study\MySelfProject\Hanhua\fontTest\zhChCount.xlsx.txt", Encoding.UTF8);
            foreach (string zhTxt in allLine)
            {
                string curChar = zhTxt.Substring(7, 1);
                byte[] byChar = Encoding.BigEndianUnicode.GetBytes(curChar);
                int curUnicode = byChar[0] << 8 | byChar[1];
                if (!allZhTxt.Contains(curUnicode))
                {
                    allZhTxt.Add(curUnicode);
                }
            }

            //List<string> allN64Char = this.GetN64Name();
            //foreach (string zhTxt in allN64Char)
            //{
            //    byte[] byChar = Encoding.BigEndianUnicode.GetBytes(zhTxt);
            //    int curUnicode = byChar[0] << 8 | byChar[1];
            //    if (!allZhTxt.Contains(curUnicode))
            //    {
            //        allZhTxt.Add(curUnicode);
            //    }
            //}

            allZhTxt.Sort();

            WriteMyPsFont(allZhTxt);
        }

        private void WriteMyPsFont(List<int> allZhTxt)
        {
            List<byte> charIndexMap = new List<byte>();
            //List<byte> charInfoMap = new List<byte>();

            //ImgInfo imgInfo = new ImgInfo(24, 24);
            //imgInfo.BlockImgH = 24;
            //imgInfo.BlockImgW = 24;
            //imgInfo.NeedBorder = false;
            //imgInfo.FontStyle = FontStyle.Regular;
            //imgInfo.FontSize = 22;
            //imgInfo.Brush = Brushes.White;

            // Retroarch font
            ImgInfo imgInfo = new ImgInfo(14, 14);
            imgInfo.BlockImgH = 14;
            imgInfo.BlockImgW = 14;
            imgInfo.NeedBorder = false;
            imgInfo.FontName = "微软雅黑";
            imgInfo.FontStyle = FontStyle.Regular;
            imgInfo.FontSize = 7f;
            imgInfo.Brush = Brushes.White;
            imgInfo.Pen = new Pen(Color.White, 0.1F);

            // 显示进度条
            this.ResetProcessBar(allZhTxt.Count);

            //Bitmap cnFontData = new Bitmap(24, 24 * allZhTxt.Count);
            int charIndex = 0;
            foreach (int unicodeChar in allZhTxt)
            {
                imgInfo.NewImg();
                imgInfo.CharTxt = Encoding.BigEndianUnicode.GetString(new byte[] { (byte)(unicodeChar >> 8 & 0xFF), (byte)(unicodeChar & 0xFF) });
                imgInfo.XPadding = 0;
                imgInfo.YPadding = -2;
                imgInfo.Grp.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                ImgUtil.WriteTextBlockImg(imgInfo);

                // 保存字符映射表信息
                byte[] byChar = Encoding.BigEndianUnicode.GetBytes(imgInfo.CharTxt);
                byte[] byCurChar = new byte[4];
                Array.Copy(byChar, 0, byCurChar, 0, byChar.Length);
                //this.SetCharPadding(byCurChar, imgInfo.Bmp);
                imgInfo.Bmp = this.SetCharPadding(byCurChar, imgInfo.Bmp);
                charIndexMap.AddRange(byCurChar);
                //charInfoMap.AddRange(byCurChar);


                //for (int y = 0; y < 24; y++)
                //{
                //    for (int x = 0; x < 24; x++)
                //    {
                //        cnFontData.SetPixel(x, charIndex * 24 + y, imgInfo.Bmp.GetPixel(x, y));
                //    }
                //}


                if (charIndex++ < 500)
                {
                    imgInfo.Bmp.Save(@"H:\游戏汉化\fontTest\CharPng\" + unicodeChar + ".png");
                }

                //charIndex = charPngData.Count;
                //charIndexMap.Add((byte)(charIndex >> 24 & 0xFF));
                //charIndexMap.Add((byte)(charIndex >> 16 & 0xFF));
                //charIndexMap.Add((byte)(charIndex >> 8 & 0xFF));
                //charIndexMap.Add((byte)(charIndex & 0xFF));

                // 保存文字图片信息
                //byte[] byCharFont = Util.ImageEncode(imgInfo.Bmp, "IA8");
                byte[] byCharFont = Util.ImageEncodeNoBlock(imgInfo.Bmp, "RGB5A3");
                //charPngData.AddRange(byCharFont);

                charIndexMap.AddRange(byCharFont);

                // 更新进度条
                this.ProcessBarStep();
            }

            // 隐藏进度条
            this.CloseProcessBar();

            //File.WriteAllBytes(@"E:\Study\MySelfProject\Hanhua\fontTest\ZhBufFont13X13NoBlock_RGB5A3_R.dat", charIndexMap.ToArray());
            //File.WriteAllBytes(@"H:\游戏汉化\fontTest\ZhBufFont13X13NoBlock_RGB5A3_R.dat", charIndexMap.ToArray());
            File.WriteAllBytes(@"H:\游戏汉化\fontTest\ZhBufFont14X14NoBlock_RGB5A3.dat", charIndexMap.ToArray());
            //File.WriteAllBytes(@"E:\Study\MySelfProject\Hanhua\fontTest\FontCn_IA8(N64).dat", charIndexMap.ToArray());
            //File.WriteAllBytes(@"E:\Study\MySelfProject\Hanhua\fontTest\FontCn_IA8.dat", Util.ImageEncode(cnFontData, "IA8").ToArray());
            //File.WriteAllBytes(@"E:\Study\MySelfProject\Hanhua\fontTest\FontCnCharInfo.dat", charInfoMap.ToArray());
            //File.WriteAllBytes(@"H:\游戏汉化\fontTest\ZhBufFont_IA8.dat", charIndexMap.ToArray());
        }

        /// <summary>
        /// 写字库小图片
        /// </summary>
        private void WriteTtfFontPics()
        {
            List<byte> charIndexMap = new List<byte>();
            //List<byte> charPngData = new List<byte>();

            // 生成Ascii码文字
            StringBuilder sb = new StringBuilder();
            for (int i = 0x20; i <= 0x7e; i++)
            {
                sb.Append(Encoding.GetEncoding(932).GetString(new byte[] { (byte)i } ));
            }

            char[] yiJiChTxt = sb.Append(Util.CreateOneLevelHanzi()).ToString().ToCharArray();
            //char[] yiJiChTxt = sb.Append("手柄存保映射卡了到游戏的设槽动插忆失记有没败上自载加档定读按右从置取状开态镜像备指不键启关经在发闭现始前左时音制目频体具典型重个支录功持打是要选当一择帧数默认信空找入显输新示未大玩初钮类较误错息通化成常各限跳编译续继请确先出作种组退返执过行释解心吗回需核视比准量正最标震网只这能而须小必着另试系统络西放东无强缩幕屏模抖式接生连被还已控总盘器换").ToString().ToCharArray();
            ImgInfo imgInfo = new ImgInfo(24, 24);
            imgInfo.BlockImgH = 24;
            imgInfo.BlockImgW = 24;
            imgInfo.NeedBorder = false;
            imgInfo.FontStyle = FontStyle.Regular;

            // 显示进度条
            this.ResetProcessBar(yiJiChTxt.Length);

            int charIndex = 0;
            foreach (char chChar in yiJiChTxt)
            {
                imgInfo.NewImg();
                imgInfo.CharTxt = chChar.ToString();
                imgInfo.XPadding = 0;
                imgInfo.YPadding = 0;
                ImgUtil.WriteBlockImg(imgInfo);
               

                // 保存字符映射表信息
                //byte[] byChar = Encoding.UTF8.GetBytes(imgInfo.CharTxt);
                byte[] byChar = Encoding.BigEndianUnicode.GetBytes(imgInfo.CharTxt);
                byte[] byCurChar = new byte[4];
                Array.Copy(byChar, 0, byCurChar, 0, byChar.Length);
                this.SetCharPadding(byCurChar, imgInfo.Bmp);
                charIndexMap.AddRange(byCurChar);

                //charIndex = charPngData.Count;
                //charIndexMap.Add((byte)(charIndex >> 24 & 0xFF));
                //charIndexMap.Add((byte)(charIndex >> 16 & 0xFF));
                //charIndexMap.Add((byte)(charIndex >> 8 & 0xFF));
                //charIndexMap.Add((byte)(charIndex & 0xFF));

                // 保存文字图片信息
                byte[] byCharFont = Util.ImageEncode(imgInfo.Bmp, "I4");
                //charPngData.AddRange(byCharFont);

                charIndexMap.AddRange(byCharFont);

                // 更新进度条
                this.ProcessBarStep();
            }

            // 隐藏进度条
            this.CloseProcessBar();

            File.WriteAllBytes(@"E:\Study\MySelfProject\Hanhua\fontTest\ZhBufFont.dat", charIndexMap.ToArray());
            //File.WriteAllBytes(@"E:\Study\MySelfProject\Hanhua\fontTest\CharPosMap.dat", charIndexMap.ToArray());
            //File.WriteAllBytes(@"E:\Study\MySelfProject\Hanhua\fontTest\CharPng.dat", charPngData.ToArray());
        }

        private Bitmap SetCharPadding(byte[] byCurChar, Bitmap img)
        {
            int leftPos = 0;
            int rightPos = 0;

            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    if (img.GetPixel(x, y).ToArgb() != 0)
                    {
                        leftPos = x > 0 ? x - 1 : 0;
                        break;
                    }
                }
                if (leftPos > 0)
                {
                    break;
                }
            }

            for (int x = img.Width - 1; x >= 0; x--)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    if (img.GetPixel(x, y).ToArgb() != 0)
                    {
                        rightPos = x + 1;
                        break;
                    }
                }
                if (rightPos > 0)
                {
                    break;
                }
            }

            if (rightPos == 0)
            {
                rightPos = img.Width / 2 + 1;
            }
            else if (rightPos >= img.Width)
            {
                rightPos = img.Width - 1;
            }

            //byCurChar[2] = (byte)leftPos;
            //byCurChar[3] = (byte)rightPos;

            Bitmap newImg = new Bitmap(img.Width, img.Height);
            for (int y = 0; y < img.Height; y++)
            {
                int newX = 0;
                for (int x = leftPos; x <= rightPos; x++)
                {
                    newImg.SetPixel(newX++, y, img.GetPixel(x, y));
                }
            }

            //byCurChar[2] = 0;
            byCurChar[3] = (byte)(rightPos - leftPos);

            return newImg;
        }

        /// <summary>
        /// 测试字符图片映射
        /// </summary>
        private void TestCharPngDat()
        {
            byte[] byCharPosMap = File.ReadAllBytes(@"E:\Study\MySelfProject\Hanhua\fontTest\CharPosMap.dat");
            byte[] byCharPng = File.ReadAllBytes(@"E:\Study\MySelfProject\Hanhua\fontTest\CharPng.dat");

            List<string> tstList = new List<string>() { "B", "A", "S", "饿" };
            foreach (string chChar in tstList)
            {
                byte[] byChar = Encoding.BigEndianUnicode.GetBytes(chChar);
                byte[] byCurChar = new byte[4];
                Array.Copy(byChar, 0, byCurChar, 4 - byChar.Length, byChar.Length);
                int charPngPos = this.GetCharPngPos(byCurChar, byCharPosMap);
                byte[] byPng = new byte[24 * 24];
                Array.Copy(byCharPng, charPngPos, byPng, 0, byPng.Length);

                Bitmap bmp = Util.ImageDecode(new Bitmap(24, 24), byPng, "I8");
                bmp.Save(@"E:\Study\MySelfProject\Hanhua\fontTest\" + chChar + ".png");
            }
        }

        private int GetCharPngPos(byte[] byChar, byte[] byCharPosMap)
        {
            int maxLen = byCharPosMap.Length - 4;
            for (int i = 0; i < maxLen; i += 8)
            {
                if (byCharPosMap[i] == byChar[0]
                    && byCharPosMap[i + 1] == byChar[1]
                    && byCharPosMap[i + 2] == byChar[2]
                    && byCharPosMap[i + 3] == byChar[3])
                {
                    return Util.GetOffset(byCharPosMap, i + 4, i + 7);
                }
            }

            return -1;
        }

        /// <summary>
        /// 取得当前文字的编码
        /// </summary>
        /// <param name="charTxt"></param>
        /// <returns></returns>
        private string GetCharNo(string charTxt)
        {
            char[] txtList = charTxt.ToCharArray();
            return txtList[0].ToString();
        }

        /// <summary>
        /// 检查翻译文本中字符个数
        /// </summary>
        private void CheckCnCharCount(params object[] param)
        {
            string oldTitle = (string)param[0];
            this.Invoke((MethodInvoker)delegate()
            {
                this.Text = oldTitle + "  处理中，请稍等...";
            });

            this.CheckCnFile(this.baseFile);

            this.Invoke((MethodInvoker)delegate()
            {
                this.Text = oldTitle;
            });
        }

        /// <summary>
        /// 设置弹出菜单
        /// </summary>
        private void SetContextMenu(ContextMenuStrip editorMenu, string[,] menuInfo)
        {
            editorMenu.Items.Clear();
            for (int i = 0; i < menuInfo.GetLength(0); i++)
            {
                if (string.IsNullOrEmpty(menuInfo[i, 0]))
                {
                    editorMenu.Items.Add(new ToolStripSeparator());
                }
                else
                {
                    ToolStripMenuItem item = new ToolStripMenuItem();
                    item.Name = menuInfo[i, 0];
                    item.Text = menuInfo[i, 1];
                    item.Image = this.GetMenuIco(item.Name);
                    editorMenu.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// 取得菜单的图片
        /// </summary>
        /// <param name="menuText"></param>
        /// <returns></returns>
        private Image GetMenuIco(string menuText)
        { 
            if (menuText.IndexOf("bio2", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return Image.FromFile(@".\img\bio2.png");
            }
            else if (menuText.IndexOf("bio3", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return Image.FromFile(@".\img\bio3.png");
            }

            return null;
        }

        /// <summary>
        /// 设置弹出菜单
        /// </summary>
        private void SetContextMenu()
        {
            // 绑定弹出菜单事件
            this.txtEditorMenu.ItemClicked -= new ToolStripItemClickedEventHandler(this.txtEditorMenu_ItemClicked);
            this.txtEditorMenu.ItemClicked += new ToolStripItemClickedEventHandler(this.txtEditorMenu_ItemClicked);
            this.imgEditorMenu.ItemClicked -= new ToolStripItemClickedEventHandler(this.imgEditorMenu_ItemClicked);
            this.imgEditorMenu.ItemClicked += new ToolStripItemClickedEventHandler(this.imgEditorMenu_ItemClicked);
            this.fntEditorMenu.ItemClicked -= new ToolStripItemClickedEventHandler(this.fntEditorMenu_ItemClicked);
            this.fntEditorMenu.ItemClicked += new ToolStripItemClickedEventHandler(this.fntEditorMenu_ItemClicked);
            this.fileEditorMenu.ItemClicked -= new ToolStripItemClickedEventHandler(this.fileEditorMenu_ItemClicked);
            this.fileEditorMenu.ItemClicked += new ToolStripItemClickedEventHandler(this.fileEditorMenu_ItemClicked);

            // 添加文本处理的菜单
            string[,] txtEditorInfo = new string[,] {
                {"btnTxtSearch", "文本查找工具"},
                {"btnTxtView", "文本查看工具"},
                {"", ""},
                {"btnChkCnChar", "中文翻译的文字个数"},
                {"", ""},
                {"btnBio0Tool", "生化0文本工具"},
                {"btnBio1Tool", "生化1文本工具"},
                {"btnBio2Tool", "生化2文本工具"},
                {"btnBio3Tool", "生化3文本工具"},
                {"btnBioCvTool", "生化维罗妮卡文本工具"},
                {"", ""},
                {"btnViewtifulTool", "红侠乔伊文本工具"},
                {"btnTos", "仙乐传说文本工具"}
            };
            this.SetContextMenu(this.txtEditorMenu, txtEditorInfo);

            // 添加图片处理的菜单
            string[,] imgEditorInfo = new string[,] {
                {"btnImgSearch", "图片查找工具"},
                {"btnTplView", "Tpl图片查看"},
                {"", ""},
                {"btnPicEdit", "图片编辑"},
                {"btnImgCreate", "简单图片生成"},
                {"", ""},
                {"btnBio2Adt", "生化2 Adt图片工具"},
                {"", ""},
                {"btnViewtifulTool", "红侠乔伊 图片文本工具"}
            };
            this.SetContextMenu(this.imgEditorMenu, imgEditorInfo);

            // 添加字库处理的菜单
            string[,] fntEditorInfo = new string[,] {
                {"btnWiiFntView", "Wii字库查看"},
                {"btnWiiFntCreate", "Wii字库做成"},
                {"", ""},    
                {"btnBio0Fnt", "生化0字库工具"},
                {"btnBio1Fnt", "生化1字库工具"}
            };
            this.SetContextMenu(this.fntEditorMenu, fntEditorInfo);

            // 添加文件处理的菜单
            string[,] fileEditorInfo = new string[,] {
                {"btnTresEdit", "Txtres类型文件处理"},
                {"btnSzsEdit", "SZS类型文件处理"},
                {"btnArcEdit", "ARC类型文件处理"},
                {"btnYayEdit", "Yay0类型文件处理"},
                {"", ""}, 
                {"btnBio0LzEdit", "生化0 Lz类型文件处理"},
                {"btnBioCvRdxEdit", "生化维罗妮卡 Rdx类型文件处理"},
                {"btnBioCvAfsEdit", "生化维罗妮卡 Afs类型文件处理"},
                {"btnRleEdit", "红侠乔伊 Rle类型文件处理"}
                
            };
            this.SetContextMenu(this.fileEditorMenu, fileEditorInfo);
        }

        /// <summary>
        /// 打开Wii字库编辑窗口
        /// </summary>
        private void OpenWiiFontView()
        {
            // 将文件中的数据，一次性读取到byData中
            byte[] byData = File.ReadAllBytes(this.baseFile);

            // 开始分析字库文件
            List<ImageHeader> imageInfo = new List<ImageHeader>();
            Image[] images;
            if (baseFile.ToLower().EndsWith("brfnt"))
            {
                WiiFontEditer wiiFontEditer = new WiiFontEditer();
                wiiFontEditer.Owner = this;
                images = wiiFontEditer.GetWiiFontInfo(byData, imageInfo);
            }
            else
            {
                NgcFontEditer ngcFontEditer = new NgcFontEditer();
                images = ngcFontEditer.GetNgcFontInfo(byData, imageInfo, null);
            }

            ImageViewer frmImageViewer = new ImageViewer(images, imageInfo, byData);
            frmImageViewer.Show();
        }

        /// <summary>
        /// 打开字库做成的窗口
        /// </summary>
        private void ShowCreateFontView()
        {
            // 开始分析字库文件
            WiiFontEditer wiiFontEditer = new WiiFontEditer();
            wiiFontEditer.Show();

            this.Do(wiiFontEditer.ViewFontInfo, File.ReadAllBytes(this.baseFile));
        }

        /// <summary>
        /// 打开RarcView
        /// </summary>
        private void ShowRarcView()
        {
            // 将文件中的数据，循环读取到byData中
            byte[] byData = File.ReadAllBytes(this.baseFile);

            string strMagic = Util.GetHeaderString(byData, 0, 3);
            if (byData[0] == 0
                && byData[1] == 0
                && byData[2] == 0
                && byData[3] == 0)
            {
                // 生化危机0Message特殊处理
                TreeNode bio0Tree = Util.Bio0ArcDecode(byData);
                SzsViewer szsViewForm = new SzsViewer(bio0Tree, byData, this.baseFile);
                szsViewForm.Show();
            }
            else if ("RARC".Equals(strMagic))
            {
                TreeNode szsFileInfoTree = Util.RarcDecode(byData);
                SzsViewer szsViewForm = new SzsViewer(szsFileInfoTree, byData, this.baseFile);
                szsViewForm.Show();
            }
            else if ("Uｪ8-".Equals(strMagic))
            {
                TreeNode szsFileInfoTree = Util.U8Decode(byData);
                SzsViewer szsViewForm = new SzsViewer(szsFileInfoTree, byData, this.baseFile);
                szsViewForm.Show();
            }
            else
            {
                MessageBox.Show("不是正常的arc文件 ： " + strMagic);
            }
        }

        /// <summary>
        /// 打开TresEditer
        /// </summary>
        private void ShowTresEditerView()
        {
            // 将文件中的数据，读取到byData中
            byte[] byData = File.ReadAllBytes(this.baseFile);

            string strFileMagic = Util.GetHeaderString(byData, 0, 3);
            if (!"TRES".Equals(strFileMagic))
            {
                MessageBox.Show("不是正常的Txtres文件 ： " + strFileMagic);
                return;
            }

            TresEditer tresEditer = new TresEditer(this.baseFile);
            tresEditer.Show();
        }

        /// <summary>
        /// 打开PicEdit
        /// </summary>
        private void ShowPicEditView()
        {
            Image img = Image.FromFile(this.baseFile);


            List<ImageHeader> tplImageInfo = new List<ImageHeader>();
            ImageHeader imageHeader = new ImageHeader();
            tplImageInfo.Add(imageHeader);
            imageHeader.Width = img.Width;
            imageHeader.Height = img.Height;
            imageHeader.Format = "png";

            ImageViewer frmTplImage = new ImageViewer(new Image[] { img }, tplImageInfo, null);
            frmTplImage.SetImgPath(this.baseFile);
            frmTplImage.Show();
        }

        /// <summary>
        /// 打开NgcIsoPatchView
        /// </summary>
        private void ShowNgcIsoPatchView()
        {
            System.Diagnostics.Process exep = new System.Diagnostics.Process();
            exep.StartInfo.FileName = @".\IsoTools.exe";
            exep.Start();
            exep.WaitForExit();
        }

        /// <summary>
        /// 检查翻译文本中字符个数
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private List<KeyValuePair<string, int>> CheckCnFile(string fileName)
        {
            List<KeyValuePair<string, int>> chChars = null;
            if (string.IsNullOrEmpty(fileName))
            {
                return chChars;
            }

            Microsoft.Office.Interop.Excel.Application xApp = null;
            Microsoft.Office.Interop.Excel.Workbook xBook = null;
            Microsoft.Office.Interop.Excel.Worksheet xSheet = null;
            string[] pageChars = null;

            try
            {
                chChars = new List<KeyValuePair<string, int>>();

                // 创建Application对象 
                xApp = new Microsoft.Office.Interop.Excel.ApplicationClass();

                // 得到WorkBook对象, 打开已有的文件 
                xBook = xApp.Workbooks._Open(
                    fileName,
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                // 显示进度条
                this.ResetProcessBar(xBook.Sheets.Count);

                for (int i = xBook.Sheets.Count; i >= 1; i--)
                {
                    // 取得相应的Sheet
                    xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets[i];

                    // 取得当前Sheet的中文文本
                    int lineNum = 1;
                    int blankNum = 0;
                    List<string> cnTxtList = new List<string>();
                    while (blankNum < 4)
                    {
                        string cellValue = xSheet.get_Range("G" + lineNum, Missing.Value).Value2 as string;

                        if (string.IsNullOrEmpty(cellValue))
                        {
                            blankNum++;
                        }
                        else
                        {
                            cnTxtList.Add(cellValue);
                            blankNum = 0;
                        }

                        lineNum++;
                    }

                    foreach (string cnTxt in cnTxtList)
                    {
                        for (int j = 0; j < cnTxt.Length - 1; j++)
                        {
                            string currentChar = cnTxt.Substring(j, 1);
                            if ("^" == currentChar)
                            {
                                // 关键字的解码
                                while (cnTxt.Substring(++j, 1) != "^")
                                {
                                }

                                continue;
                            }
                            else
                            {
                                KeyValuePair<string, int> charInfo = chChars.FirstOrDefault(p => p.Key.Equals(currentChar));
                                if (string.IsNullOrEmpty(charInfo.Key))
                                {
                                    chChars.Add(new KeyValuePair<string, int>(currentChar, 1));
                                }
                                else
                                {
                                    int charCount = charInfo.Value + 1;
                                    chChars.Remove(charInfo);
                                    chChars.Add(new KeyValuePair<string, int>(currentChar, charCount));
                                }
                            }
                        }
                    }

                    // 更新进度条
                    this.ProcessBarStep();
                }

                // 排序
                chChars.Sort(this.CharCountCompare);

                // 写结果信息
                pageChars = new string[chChars.Count];
                for (int i = 0; i < chChars.Count; i++)
                {
                    KeyValuePair<string, int> item = chChars[i];
                    //pageChars[i] = (i + 1).ToString().PadLeft(4, '0') + " : " + item.Key + "  " + item.Value;
                    pageChars[i] = item.Key;
                }
            }
            catch (Exception me)
            {
                MessageBox.Show(fileName + "\n" + me.Message);
            }
            finally
            {
                // 隐藏进度条
                this.CloseProcessBar();

                // 清空各种对象
                xSheet = null;
                xBook = null;
                if (xApp != null)
                {
                    xApp.Quit();
                    xApp = null;
                }

                if (pageChars != null)
                {
                    File.WriteAllLines(fileName + @".txt", pageChars, Encoding.UTF8);

                    MessageBox.Show("结果信息已经写到了下面的文件中：\n" + fileName + @".txt");
                }
            }

            return chChars;
        }

        /// <summary>
        /// 对象比较
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private int CharCountCompare(KeyValuePair<string, int> a, KeyValuePair<string, int> b)
        {
            return b.Value - a.Value;
        }

        #endregion
    }
}
