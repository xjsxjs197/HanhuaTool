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
using Hanhua.Common.TextEditTools.RfoEdit;

using System.Threading;
using System.Runtime.InteropServices;
using System.Net;
using Hanhua.Common.TextEditTools.Dino;

namespace Hanhua.Common
{
    /// <summary>
    /// 汉化入口
    /// </summary>
    public partial class StartMenu : BaseForm
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        //SendMessage参数
        private const int WM_KEYDOWN = 0X100;
        private const int WM_KEYUP = 0X101;
        private const int WM_SYSCHAR = 0X106;
        private const int WM_SYSKEYUP = 0X105;
        private const int WM_SYSKEYDOWN = 0X104;
        private const int WM_CHAR = 0X102;

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

                case "btnRfo":
                    RfoEdit rfoTool = new RfoEdit();
                    rfoTool.Show();
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

                    this.ShowRarcView();
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

                case "btnSkAscEdit":
                    compTool = new BaseCompTool(new EternalDarknessSkArcComp());
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
        /// 纹理测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTexTest_Click(object sender, EventArgs e)
        {
            this.CheckTexPng();
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
            //this.CheckBof4Text();
            //this.TestEternalDarkness();
            //this.Check3DSFont();
            //this.GetBigEndianAllCnChars();
            //this.CheckDino2Tex();
            //this.CheckRpgTxt();
            //this.DecodeTex();
            //this.ChkCopyBuf();
            //this.ChkDomainIp();
            //this.ChkSvnHtpasswd();
            //this.CheckCopyTexPng();
            DinoEdit dinoEdit = new DinoEdit();
            dinoEdit.Show();
        }

        #endregion

        #region " 私有方法 "

        private void CheckCopyTexPng()
        {
            //string path = @"D:\WiiStationDebug\CopyTex_640_480.bin";
            //byte[] byCnFont = File.ReadAllBytes(path);
            //int w = Convert.ToInt32("640");
            //int h = Convert.ToInt32("480");
            //Bitmap chkBmp = new Bitmap(w, h);
            //chkBmp = Util.ImageDecode(chkBmp, byCnFont, "RGB565");
            //chkBmp.Save(path + ".png");

            string path = @"D:\WiiStationDebug\OldVramTex_320_240.bin";
            byte[] byCnFont = File.ReadAllBytes(path);
            int w = Convert.ToInt32("320");
            int h = Convert.ToInt32("240");
            Bitmap chkBmp = new Bitmap(w, h);
            int idx = 0;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int tmpPixel = (((int)(byCnFont[idx + 1]) << 8) + (int)(byCnFont[idx])) & 0x7fff;
                    chkBmp.SetPixel(x, y, Color.FromArgb(Util.Convert5To8((byte)(tmpPixel & 0x1f)), Util.Convert5To8((byte)((tmpPixel >> 5) & 0x1f)), Util.Convert5To8((byte)(tmpPixel >> 10))));
                    idx += 2;
                }
            }
            chkBmp.Save(path + ".png");
        }

        private void ChkSvnHtpasswd()
        {
            string[] passwd = File.ReadAllLines(@"G:\NewWebSvn\htpasswd145Svn20250729");
            StringBuilder sb = new StringBuilder();
            foreach (string userInfo in passwd)
            {
                if (string.IsNullOrEmpty(userInfo))
                {
                    break;
                }

                string[] userPw = userInfo.Split(':');
                //string pw = userPw[1].Replace(".", "").Replace("/", "");
                //sb.Append("Xayr!234").Append(pw.Substring(pw.Length - 5)).Append(",");
                //sb.Append(userPw[0]).Append(",");
                sb.Append(userPw[1]).Append(",");
            }
        }

        private void ChkDomainIp()
        {
            string[] allDomain = File.ReadAllLines(@"G:\会社関連\网_委_会\Juniper\DMZ_Url_Black_White_list\blacklist");
            StringBuilder sb = new StringBuilder();
            StringBuilder errSb = new StringBuilder();
            foreach (string domain in allDomain)
            {
                if (string.IsNullOrEmpty(domain))
                {
                    break;
                }

                try
                {
                    IPAddress[] addresses = Dns.GetHostAddresses(domain);

                    foreach (IPAddress ip in addresses)
                    {
                        sb.Append("address ").Append(ip.ToString().PadRight(15, ' ')).Append(" ").Append(ip.ToString()).Append("/32").Append("; ## ").Append(domain).Append("\r\n");
                    }
                }
                catch (Exception ex)
                {
                    errSb.Append(domain).Append("\r\n");
                }
            }
        }

        private void ChkCopyBuf()
        {
            Bitmap tmpImg = new Bitmap(640, 480);
            byte[] byImg = File.ReadAllBytes(@"D:\WiiStationDebug\tmpCpy.bin");
            int idx = 0;
            for (int y = 0; y < 480; y++)
            {
                for (int x = 0; x < 640; x++)
                {
                    int tmpPix = ((int)(byImg[idx]) << 8) + byImg[idx + 1];
                    idx += 2;
                    tmpImg.SetPixel(x, y, Color.FromArgb((tmpPix & 0xf800) >> 8, (tmpPix & 0x7e0) >> 3, (tmpPix & 0x1f) << 3));
                }
            }
            tmpImg.Save(@"D:\WiiStationDebug\tmpCpyImg.png");
        }

        private void CheckRpgTxt()
        {
            string[] allChkLines = File.ReadAllLines(@"D:\RPG\workspace\10.RPG-SQL化\02.E_営業\第５弾(D2510U_D540U1_D610U1_DJ010U1_DJ010U3_DJ010U4_DJ010U2)\【営業】DJ010UCL_04_DJ010U2_営業初受注ファイル作成\test1.txt", Encoding.UTF8);
            string[] allRpgLines = File.ReadAllLines(@"D:\RPG\workspace\10.RPG-SQL化\02.E_営業\第５弾(D2510U_D540U1_D610U1_DJ010U1_DJ010U3_DJ010U4_DJ010U2)\【営業】DJ010UCL_04_DJ010U2_営業初受注ファイル作成\DJ010U2S_tool生成.sql", Encoding.UTF8);
            StringBuilder sb = new StringBuilder();
            foreach (string chkLine in allChkLines)
            {
                if (string.IsNullOrEmpty(chkLine))
                {
                    break;
                }

                int lineCnt = 0;
                foreach (string rpgkLine in allRpgLines)
                {
                    if (rpgkLine.IndexOf(chkLine.Substring(4, 8)) > 0)
                    {
                        lineCnt++;
                    }
                }

                if (lineCnt <= 1)
                {
                    sb.Append("--");
                }
                sb.Append(chkLine).Append("\r\n");
            }

            int b = 0;
        }

        private void CheckDino2Tex()
        {
            //byte[] byData = File.ReadAllBytes(@"G:\Study\MySelfProject\Hanhua\TodoCn\Dino2\PSX\DATA\CAPLOGO.PXL");
            //// 初始化图片
            //Bitmap img = new Bitmap(320, 240, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            //// 生成图片
            //int byIndex = 0;
            //for (int y = 0; y < img.Height; y++)
            //{
            //    for (int x = 0; x < img.Width; x++)
            //    {
            //        int pixelColor = byData[byIndex + 1] << 8 | byData[byIndex];
            //        int colorR = Util.Convert5To8((byte)(pixelColor & 0x1F));
            //        int colorG = Util.Convert5To8((byte)((pixelColor >> 5) & 0x1F));
            //        int colorB = Util.Convert5To8((byte)((pixelColor >> 10) & 0x1F));
            //        img.SetPixel(x, y, Color.FromArgb(colorR, colorG, colorB));

            //        byIndex += 2;
            //    }
            //}
            //img.Save(@"G:\Study\MySelfProject\Hanhua\TodoCn\Dino2\PSX\DATA\CAPLOGO.bmp");

            List<FilePosInfo> allTexFiles = Util.GetAllFiles(@"G:\Study\MySelfProject\Hanhua\TodoCn\Dino2\PSX\DATA\");
            foreach (FilePosInfo fi in allTexFiles)
            {
                if (!fi.File.EndsWith(".PXL", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                // 初始化图片
                byte[] byData = File.ReadAllBytes(fi.File);
                Bitmap img = new Bitmap(320, 240, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                // 生成图片
                int byIndex = 0;
                for (int y = 0; y < img.Height; y++)
                {
                    for (int x = 0; x < img.Width; x++)
                    {
                        int pixelColor = byData[byIndex + 1] << 8 | byData[byIndex];
                        int colorR = Util.Convert5To8((byte)(pixelColor & 0x1F));
                        int colorG = Util.Convert5To8((byte)((pixelColor >> 5) & 0x1F));
                        int colorB = Util.Convert5To8((byte)((pixelColor >> 10) & 0x1F));
                        img.SetPixel(x, y, Color.FromArgb(colorR, colorG, colorB));

                        byIndex += 2;
                    }
                }
                img.Save(fi.File.Replace(".PXL", ".bmp"));
            }
        }

        private void DecodeTex()
        {
            // 打开要分析的文件
            this.baseFile = Util.SetOpenDailog("Txt Log文件（*.txt）|*.txt|所有文件|*.*", string.Empty);
            if (string.IsNullOrEmpty(this.baseFile))
            {
                return;
            }

            string[] allLine = File.ReadAllLines(this.baseFile);
            TexTestView texView = new TexTestView(allLine);
            texView.Show();
        }

        private void CheckTexPng()
        {
            string openFolder = Util.OpenFolder(@"D:\WiiStationDebug\TexTest");
            if (string.Empty.Equals(openFolder))
            {
                return;
            }

            List<FilePosInfo> allTexFiles = Util.GetAllFiles(openFolder);
            foreach (FilePosInfo fi in allTexFiles)
            {
                if (fi.IsFolder)
                {
                    continue;
                }

                byte[] byCnFont = File.ReadAllBytes(fi.File);
                string[] names = fi.File.Split('_');
                int w = Convert.ToInt32(names[1]);
                int h = Convert.ToInt32(names[2]);
                Bitmap chkBmp = new Bitmap(w, h);
                int pixelIdx = 0;
                chkBmp = Util.ImageDecode(chkBmp, byCnFont, "RGB5A3");
                //for (int y = 0; y < chkBmp.Height; y += 1)
                //{
                //    // 开始循环Image的宽(每次递增一个Block的宽)
                //    for (int x = 0; x < chkBmp.Width; x += 1)
                //    {
                //        chkBmp.SetPixel(x, y, Color.FromArgb(byCnFont[pixelIdx], byCnFont[pixelIdx + 1], byCnFont[pixelIdx + 2], byCnFont[pixelIdx + 3]));
                //        pixelIdx += 4;
                //    }
                //}
                chkBmp.Save(fi.File + ".png");
            }
        }
        
        private void Check3DSFont()
        {
            //Dictionary<int, int> testMap = new Dictionary<int, int>();
            //for (int y = 0; y < 418; y++)
            //{
            //    for (int x = 0; x < 440; x++)
            //    {
            //        testMap.Add(ctrgu_swizzle_coords(x, y, 440), x + y * 440);
            //    }
            //}

            byte[] byCnFont = File.ReadAllBytes(@"C:\Users\xiao.jiansheng\AppData\Roaming\Citra\sdmc\retroarch\cnFontStb");
            int wh = 42 * 14;
            Bitmap chkBmp = new Bitmap(wh, wh);
            for (int y = 0; y < wh; y++)
            {
                for (int x = 0; x < wh; x++)
                {
                    //if ((byCnFont[y * 440 + x]) == 0xFF)
                    //{
                    //    chkBmp.SetPixel(x, y, Color.White);
                    //}
                    //else
                    //{
                    //    chkBmp.SetPixel(x, y, Color.Black);
                    //}
                    chkBmp.SetPixel(x, y, Color.FromArgb(byCnFont[y * wh + x], byCnFont[y * wh + x], byCnFont[y * wh + x]));

                }
            }
            chkBmp.Save(@"C:\Users\xiao.jiansheng\AppData\Roaming\Citra\sdmc\retroarch\cnFontStb.bmp");

            //int tmp = this.next_pow2(1023);
        }

        private int ctrgu_swizzle_coords(int x, int y, int width)
        {
           int pos = (x & 0x1) << 0 | ((x & 0x2) << 1) | ((x & 0x4) << 2) |
                     (y & 0x1) << 1 | ((y & 0x2) << 2) | ((y & 0x4) << 3);

           return ((x >> 3) << 6) + ((y >> 3) * ((width >> 3) << 6)) + pos;

        }

        private int next_pow2(int x)
        {
            x--;
            x = (x >> 1) | x;
            x = (x >> 2) | x;
            x = (x >> 4) | x;
            x = (x >> 8) | x;
            x = (x >> 16) | x;
            return ++x;
        }

        private void TestEternalDarkness()
        {
            List<FilePosInfo> allFiles = Util.GetAllFiles(@"G:\Study\MySelfProject\Hanhua\TodoCn\HanhuaProject\EternalDarkness\cn\root");
            foreach (FilePosInfo fi in allFiles)
            {
                if (fi.IsFolder)
                {
                    continue;
                }

                if (fi.File.EndsWith(".decompressed"))
                {
                    //File.Copy(fi.File, fi.File.Replace(@"\cn\", @"\cnBak\"), true);
                    File.Copy(fi.File, fi.File.Replace(@".decompressed", ""), true);
                    File.Delete(fi.File);
                }
            }
        }

        private void CheckPcBio2()
        {
            byte[] byPcBio2 = File.ReadAllBytes(@"G:\Study\MySelfProject\Hanhua\TodoCn\HanhuaProject\Bio2\helpPc\bio2.exe");
            byte[] byJpBio2 = File.ReadAllBytes(@"G:\Study\MySelfProject\Hanhua\TodoCn\HanhuaProject\Bio2\helpPc\jp\leon.rel");

            byte[] byJpBio2Search = new byte[0x1a89b8 - 0x1a8990];
            Array.Copy(byJpBio2, 0x1a8990, byJpBio2Search, 0, byJpBio2Search.Length);
            bool searchOk = false;
            int chkIdx = 1;
            int maxIdx = 0;
            for (int i = 0; i < byPcBio2.Length; i++)
            {
                if (byPcBio2[i] == byJpBio2Search[0])
                {
                    chkIdx = 1;
                    for (; chkIdx < byJpBio2Search.Length && i + chkIdx < byPcBio2.Length; chkIdx++)
                    {
                        if (byPcBio2[i + chkIdx] != byJpBio2Search[chkIdx])
                        {
                            break;
                        }
                    }
                    maxIdx = Math.Max(maxIdx, chkIdx);
                    if (chkIdx >= byJpBio2Search.Length)
                    {
                        searchOk = true;
                        break;
                    }
                }
            }

            if (searchOk)
            {
                MessageBox.Show("找到日文文本了！");
            }
            else
            {
                MessageBox.Show("没有找到日文文本 " + maxIdx + " " + byJpBio2Search.Length);
            }
        }
		
		private void InitADSR()                                    // INIT ADSR
        {
            int[] RateTableSub = new int[128];
            int[] RateTableAdd = new int[128];
            int lcv;
            int denom;

            // Optimize table - Dr. Hell ADSR math
            for (lcv = 0; lcv < 48; lcv++)
            {
                RateTableAdd[lcv] = (int)((7 - (lcv & 3)) << (11 - (lcv >> 2)));
                RateTableSub[lcv] = (int)((-8 + (lcv & 3)) << (11 - (lcv >> 2)));
            }

            for (; lcv < 128; lcv++)
            {
                denom = (int)(1 << ((lcv >> 2) - 11));

                RateTableAdd[lcv] = (int)(((int)(7) - (int)(lcv & 7)) << 16) / denom;
                RateTableSub[lcv] = (int)(((int)(-8) + (int)(lcv & 7)) << 16) / denom;

            }

        }
		
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
            int charImgSize = 13 * 13 * 2; // 13 * 13;
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
            string basePath = @"G:\Study\Emu\emuSrc\RetroArch\RetroArch-master20240426Old\RetroArch-master\";

            //string targetPath = @"G:\Study\Emu\emuSrc\WiiEmuHanhua\Release\apps\RetroArch_1.18.0Cn\Src\";
            //string[] allLines = File.ReadAllLines(basePath + "updFiles.txt", Encoding.UTF8);
            //for (int i = 0; i < allLines.Length; i++)
            //{
            //    if (string.IsNullOrEmpty(allLines[i]))
            //    {
            //        break;
            //    }

            //    string targetFile = allLines[i].Replace(basePath, targetPath);
            //    string fileName = Util.GetShortFileName(targetFile);
            //    if (!Directory.Exists(targetFile.Replace(fileName, "")))
            //    {
            //        Directory.CreateDirectory(targetFile.Replace(fileName, ""));
            //    }
            //    File.Copy(allLines[i], targetFile, true);
            //}

            System.Diagnostics.Process exep = new System.Diagnostics.Process();
            exep.StartInfo.FileName = @"make";
            exep.StartInfo.CreateNoWindow = true;
            exep.StartInfo.UseShellExecute = false;
            exep.StartInfo.WorkingDirectory = basePath;
            exep.StartInfo.Arguments = @"-f Makefile.ctr";

            List<FilePosInfo> allWiiLib = Util.GetAllFiles(basePath + @"dist-scripts\").Where(p => p.File.EndsWith("_ctr.a", StringComparison.OrdinalIgnoreCase)).ToList();
            // 显示进度条
            this.ResetProcessBar(allWiiLib.Count);

            foreach (FilePosInfo fileInfo in allWiiLib)
            {
                // Delete file
                File.Delete(basePath + @"libretro_ctr.a");
                if (File.Exists(basePath + @"retroarch_3ds.cia"))
                {
                    File.Delete(basePath + @"retroarch_3ds.cia");
                }
                if (File.Exists(basePath + @"retroarch_3ds.elf"))
                {
                    File.Delete(basePath + @"retroarch_3ds.elf");
                }
                if (File.Exists(basePath + @"retroarch_3ds.smdh"))
                {
                    File.Delete(basePath + @"retroarch_3ds.smdh");
                }

                // copy File
                File.Copy(fileInfo.File, basePath + @"libretro_ctr.a", true);

                // build file
                exep.Start();
                exep.WaitForExit();

                // move File
                if (File.Exists(basePath + @"retroarch_3ds.cia"))
                {
                    File.Move(basePath + @"retroarch_3ds.cia", fileInfo.File.Replace(".a", ".cia").Replace("dist-scripts", @"pkg\ctr").Replace("_ctr", ""));
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
            this.ReadChChar(@"G:\Study\Emu\emuSrc\RetroArch\RetroArch-1.18.0_3DS\intl\googleplay_chs.json", lstBuf);
            this.ReadChChar(@"G:\Study\Emu\emuSrc\RetroArch\RetroArch-1.18.0_3DS\intl\steam_chs.json", lstBuf);
            this.ReadChChar(@"G:\Study\Emu\emuSrc\RetroArch\RetroArch-1.18.0_3DS\intl\msg_hash_chs.h", lstBuf);
            //this.ReadChChar(@"G:\Study\Emu\emuSrc\RetroArch\RetroArch-1.9.6\intl\msg_hash_it_pt.c", lstBuf);
            //this.ReadChChar(@"G:\Study\Emu\emuSrc\RetroArch\RetroArch-1.9.6\intl\msg_hash_it_pt.h", lstBuf);
            //this.ReadChChar(@"G:\Study\de.lang", lstBuf);
            this.ReadChChar(@"G:\Study\Emu\emuSrc\WiiEmuHanhua\Retroarch_CnSrc\hbc\playlists\fba_cps1.lpl", lstBuf);
            this.ReadChChar(@"G:\Study\Emu\emuSrc\WiiEmuHanhua\Retroarch_CnSrc\hbc\playlists\fba_cps2.lpl", lstBuf);
            this.ReadChChar(@"G:\Study\Emu\emuSrc\WiiEmuHanhua\Retroarch_CnSrc\hbc\playlists\fba_cps3.lpl", lstBuf);
            this.ReadChChar(@"G:\Study\Emu\emuSrc\WiiEmuHanhua\Retroarch_CnSrc\hbc\playlists\fba_neogeo.lpl", lstBuf);
            this.ReadChChar(@"G:\Study\Emu\emuSrc\WiiEmuHanhua\Retroarch_CnSrc\hbc\playlists\nintendo_fc.lpl", lstBuf);
            this.ReadChChar(@"G:\Study\Emu\emuSrc\WiiEmuHanhua\Retroarch_CnSrc\hbc\playlists\nintendo_sfc.lpl", lstBuf);
            this.ReadChChar(@"G:\Study\Emu\emuSrc\WiiEmuHanhua\Retroarch_CnSrc\hbc\playlists\nintendo_gba.lpl", lstBuf);
            this.ReadChChar(@"G:\Study\Emu\emuSrc\WiiEmuHanhua\Retroarch_CnSrc\hbc\playlists\sega_md.lpl", lstBuf);
            this.ReadChChar(@"G:\Study\Emu\emuSrc\WiiEmuHanhua\Retroarch_CnSrc\hbc\playlists\fba_Pgm_PSIKYO.lpl", lstBuf);
            this.ReadChChar(@"G:\Study\Emu\emuSrc\WiiEmuHanhua\Retroarch_CnSrc\hbc\playlists\mame2003_coreA.lpl", lstBuf);
            this.ReadChChar(@"G:\Study\Emu\emuSrc\WiiEmuHanhua\Retroarch_CnSrc\hbc\playlists\mame2003_coreB.lpl", lstBuf);
            this.ReadChChar(@"G:\Study\Emu\emuSrc\WiiEmuHanhua\Retroarch_CnSrc\hbc\playlists\mame2003_coreC.lpl", lstBuf);
            this.ReadChChar(@"G:\Study\Emu\emuSrc\WiiEmuHanhua\Retroarch_CnSrc\hbc\playlists\mame2003_coreD.lpl", lstBuf);
            this.ReadChChar(@"G:\Study\Emu\emuSrc\WiiEmuHanhua\Retroarch_CnSrc\hbc\playlists\mame2003_coreE.lpl", lstBuf);
            this.ReadChChar(@"G:\Study\Emu\emuSrc\WiiEmuHanhua\Retroarch_CnSrc\hbc\playlists\mame2003_coreF.lpl", lstBuf);
            this.ReadChChar(@"G:\Study\Emu\emuSrc\WiiEmuHanhua\Retroarch_CnSrc\hbc\playlists\mame2003_coreG.lpl", lstBuf);

            string[] allLine = File.ReadAllLines(@"G:\Study\MySelfProject\Hanhua\fontTest\zhChCount.xlsx.txt", Encoding.UTF8);
            foreach (string zhTxt in allLine)
            {
                string curChar = zhTxt.Substring(7, 1);
                if (!lstBuf.Contains(curChar))
                {
                    lstBuf.Add(curChar);
                }
            }

            char[] chTxt = string.Join("", lstBuf.ToArray()).ToCharArray();

            foreach (char chChar in chTxt)
            {
                //byte[] byChar = Encoding.BigEndianUnicode.GetBytes(new char[] { chChar });
                //int temp = byChar[0] << 8 | byChar[1];
                //if (temp < 0x20)
                //{
                //    continue;
                //}
                //byte[] byChar = Encoding.UTF8.GetBytes(new char[] { chChar });
                //int shift = 0;
                //int temp = 0;
                //for (int i = byChar.Length - 1; i >= 0; i--)
                //{
                //    temp += byChar[i] << shift;
                //    shift += 8;
                //}
                int temp = chChar;
                if (temp < 0x20)
                {
                    continue;
                }

                allZhTxt.Add(temp);
            }
            allZhTxt.Sort();
            //string.Join("", allZhTxt.ToArray());
            sb.Length = 0;
            int tmpLine = 20;
            sb.Append("int minCnChar[] = {");
            foreach (int tmp in allZhTxt)
            {
                sb.Append(tmp).Append(",");
                if (tmpLine-- == 0)
                {
                    tmpLine = 20;
                    sb.Append("\r\n");
                }
            }
            sb.Append("};");

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

            ImgInfo imgInfo = new ImgInfo(24, 24);
            imgInfo.BlockImgH = 24;
            imgInfo.BlockImgW = 22;
            imgInfo.NeedBorder = false;
            //imgInfo.FontStyle = FontStyle.Bold;
            imgInfo.FontSize = 19;
            //imgInfo.FontName = "gulim";
            imgInfo.FontName = "Times New Roman";
            imgInfo.Brush = Brushes.White;
            //imgInfo.Sf.LineAlignment = StringAlignment.Far;

            // Retroarch font
            //ImgInfo imgInfo = new ImgInfo(13, 13);
            //imgInfo.BlockImgH = 13;
            //imgInfo.BlockImgW = 13;
            //imgInfo.NeedBorder = false;
            //imgInfo.FontName = "微软雅黑";
            //imgInfo.FontName = "Futura";
            //imgInfo.FontStyle = FontStyle.Regular;
            //imgInfo.FontSize = 8f;
            //imgInfo.Brush = Brushes.White;
            //imgInfo.Pen = new Pen(Color.White, 0.1F);

            // 显示进度条
            this.ResetProcessBar(allZhTxt.Count);

            //Bitmap cnFontData = new Bitmap(24, 24 * allZhTxt.Count);
            int charIndex = 0;
            foreach (int unicodeChar in allZhTxt)
            {
                imgInfo.NewImg();
                imgInfo.CharTxt = Encoding.BigEndianUnicode.GetString(new byte[] { (byte)(unicodeChar >> 8 & 0xFF), (byte)(unicodeChar & 0xFF) });
                //if ("G".Equals(imgInfo.CharTxt) || "g".Equals(imgInfo.CharTxt))
                //{
                //    //imgInfo.YPadding = -3;
                //}
                //else
                {
                    imgInfo.YPadding = 0;
                }
                imgInfo.PosX = 0;
                imgInfo.PosY = 1;
                imgInfo.XPadding = 0;
                
                imgInfo.Sf.Alignment = StringAlignment.Center;
                imgInfo.Sf.LineAlignment = StringAlignment.Center;

                imgInfo.Grp.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                ImgUtil.WriteBlockImg(imgInfo);

                // 保存字符映射表信息
                byte[] byChar = Encoding.BigEndianUnicode.GetBytes(imgInfo.CharTxt);
                byte[] byCurChar = new byte[4];
                Array.Copy(byChar, 0, byCurChar, 0, byChar.Length);
                //this.SetCharPadding(byCurChar, imgInfo.Bmp);
                imgInfo.Bmp = this.SetCharPadding(byCurChar, imgInfo.Bmp);
                //this.SetCharPadding(byCurChar, imgInfo.Bmp);
                charIndexMap.AddRange(byCurChar);
                //charInfoMap.AddRange(byCurChar);


                //for (int y = 0; y < 24; y++)
                //{
                //    for (int x = 0; x < 24; x++)
                //    {
                //        cnFontData.SetPixel(x, charIndex * 24 + y, imgInfo.Bmp.GetPixel(x, y));
                //    }
                //}


                //if (charIndex++ < 500)
                {
                    //imgInfo.Bmp.Save(@"H:\游戏汉化\fontTest\CharPng\" + unicodeChar + ".png");
                    imgInfo.Bmp.Save(@"G:\Study\MySelfProject\Hanhua\fontTest\CharPng\" + unicodeChar + ".png");
                }

                //charIndex = charPngData.Count;
                //charIndexMap.Add((byte)(charIndex >> 24 & 0xFF));
                //charIndexMap.Add((byte)(charIndex >> 16 & 0xFF));
                //charIndexMap.Add((byte)(charIndex >> 8 & 0xFF));
                //charIndexMap.Add((byte)(charIndex & 0xFF));

                // 保存文字图片信息
                byte[] byCharFont = Util.ImageEncode(imgInfo.Bmp, "IA8");
                //byte[] byCharFont = Util.ImageEncodeNoBlock(imgInfo.Bmp, "RGB5A3");
                //charPngData.AddRange(byCharFont);

                charIndexMap.AddRange(byCharFont);

                // 更新进度条
                this.ProcessBarStep();
            }

            // 隐藏进度条
            this.CloseProcessBar();

            //File.WriteAllBytes(@"E:\Study\MySelfProject\Hanhua\fontTest\ZhBufFont13X13NoBlock_RGB5A3.dat", charIndexMap.ToArray());
            //File.WriteAllBytes(@"H:\游戏汉化\fontTest\ZhBufFont13X13NoBlock_RGB5A3_R.dat", charIndexMap.ToArray());
            //File.WriteAllBytes(@"H:\游戏汉化\fontTest\ZhBufFont14X14NoBlock_RGB5A3.dat", charIndexMap.ToArray());

            //File.WriteAllBytes(@"E:\Study\MySelfProject\Hanhua\fontTest\FontCn_IA8(N64).dat", charIndexMap.ToArray());
            //File.WriteAllBytes(@"E:\Study\MySelfProject\Hanhua\fontTest\FontCn_IA8.dat", Util.ImageEncode(cnFontData, "IA8").ToArray());
            File.WriteAllBytes(@"G:\Study\MySelfProject\Hanhua\fontTest\De.dat", charIndexMap.ToArray());
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
            else
            {
                rightPos++;
                if (rightPos >= img.Width)
                {
                    rightPos = img.Width - 1;
                }
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
                {"btnTos", "仙乐传说文本工具"},
                {"", ""},
                {"btnRfo", "符文工房边境"}
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
                {"btnSkAscEdit", "SkAsc类型文件处理"},
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
