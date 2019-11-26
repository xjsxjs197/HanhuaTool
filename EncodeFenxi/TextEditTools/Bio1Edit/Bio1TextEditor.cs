using System;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Collections.Generic;
using System.Reflection;
using Hanhua.ImgEditTools;
using System.Data;
using System.Data.OleDb;
using Hanhua.Common;

namespace Hanhua.TextEditTools.Bio1Edit
{
    /// <summary>
    /// 生化危机1文本编辑器
    /// </summary>
    public partial class Bio1TextEditor : BaseForm
    {
        #region " 私有变量 "

        /// <summary>
        /// 设置中文输入的开始位置
        /// </summary>
        private int inputCnStartPos = 0;

        /// <summary>
        /// 设置中文输入的结束位置
        /// </summary>
        private int inputCnEndPos = 0;

        /// <summary>
        /// 是共通的文本还是文件的文本
        /// </summary>
        private bool isComText = true;

        /// <summary>
        /// 中文翻译文件的标识
        /// </summary>
        private const string cnFileBiaoshi = "_cn.dat";

        /// <summary>
        /// 共通文本的字库
        /// </summary>
        private const string cnTextFont = "Bio1CnTextFont.txt";

        /// <summary>
        /// 文件的字库
        /// </summary>
        private const string cnFileFont = "Bio1CnFileFont.txt";

        /// <summary>
        /// 中文字符和索引的映射
        /// </summary>
        private List<FontCharInfo> cnFontMap = new List<FontCharInfo>();

        /// <summary>
        /// 保存的中文的字符
        /// </summary>
        private string[] bio1CnTextChars;

        /// <summary>
        /// 保存的中文的字符
        /// </summary>
        private string[] bio1CnFileChars;

        /// <summary>
        /// 中文翻译文件
        /// </summary>
        private string cnFile = string.Empty;

        /// <summary>
        /// 日文原来文件
        /// </summary>
        private string jpFile = string.Empty;

        /// <summary>
        /// 文本中的关键字，不能被删除
        /// </summary>
        private string[] keyWords = {
            "^1 0^", "^2 0^", "^3 0^", "^3 1^", "^3 2^", "^3 3^", "^5 7^", "^6^", "^7^",
            "^b 0 20^", "^b 0 21^", "^b 0 22^", "^b 1 25^", "^b 1 26^", "^b 1 29^",
            "^11 b8^", "^11 1a8^", "^12^", "^16^", "^1b^", "^0^"
        };

        /// <summary>
        /// 保存旧的文本的长度，为了保存时验证中文的文本长度是否变化
        /// </summary>
        private int oldTextLen = 0;

        /// <summary>
        /// 保存所有的文本、地址信息
        /// </summary>
        private List<FilePosInfo> lstFilePos = new List<FilePosInfo>();

        /// <summary>
        /// 记录有Entry的Message的开始地址
        /// </summary>
        private int textEntryStart;

        /// <summary>
        /// 记录Message的Entry
        /// </summary>
        private List<int> textEntrys = new List<int>();
        private List<int> oldTextEntrys = new List<int>();

        /// <summary>
        /// 保存Entry排序前、排序后的映射
        /// </summary>
        private Dictionary<int, int> entrysMap = new Dictionary<int, int>();

        /// <summary>
        /// 记录Message的Entry
        /// </summary>
        private byte[] byMsgEntry;

        /// <summary>
        /// 是否存在文本映射表
        /// </summary>
        private bool hasEntry;

        /// <summary>
        /// 是否是Wii汉化
        /// </summary>
        private bool isWii;

        /// <summary>
        /// 子目录
        /// </summary>
        private string subFolder;

        #region " 字库 "

        #region " 共通文本字库 "

        /// <summary>
        /// 生化危机1原字库
        /// </summary>
        private static string[] bio1FontChars = { 
"　", "▷", "▽", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "：", "%", "B", "I", "F", "E", "e", "n", "t", "r", "h", "s", "u", "v", "i", "a", "l", "o", ".",
"f", "c", "'", "k", "y", "d", "g", "ス", "テ",  "ー", "タ", "画", "面", "を", "抜", "け", "ま", "す", "フ", "ァ", "イ", "ル", "见", "マ", "ッ", "プ", "ア", "ム", "使", "用", "し", "武",
"器", "装", "备", "は", "ず", "讲", "身", "调", "べ", "组", "み", "合", "わ", "せ", "ハ", "ブ", "弹", "填", "る", "事", "で", "き", "ん", "今", "う", "必", "要", "が", "な", "い", "こ", "の",
"え", "灯", "油", "ボ", "ト", "に", "移", "か", "？", "た", "滴", "も", "残", "っ", "て", "…", "れ", "以", "上", "补", "给", "火", "种", "あ", "ば", "だ", "り", "取", "持", "置", "入", "换",
"特", "变", "所", "选", "く", "さ", "洋", "馆", "手", "～", "そ", "中", "庭", "゜", "寄", "宿", "舍", "研", "究", "レ", "ン", "グ", "ニ", "ュ", "ケ", "ネ", "ィ", "呪", "书", "ヴ", "记", "捨",
"ら", "メ", "モ", "植", "物", "学", "死", "体", "处", "理", "关", "诸", "注", "意", "饲", "育", "係", "日", "志", "员", "遗", "保", "安", "部", "长", "自", "杀", "者", "ラ", "有", "機", "化",
"実", "験", "家", "族", "写", "真", "バ", "リ", "纸", "V", "-", "A", "C", "T", "つ", "ク", "警", "シ", "资", "料", "观", "察", "录", "Ｆ", "Ｂ", "大", "水", "槽", "祭", "壇", "へ", "ポ",
"サ", "ナ", "型", "非", "常", "時", "と", "ジ", "私", "空", "军", "代", "爱", "品", "ド", "ガ", "m", "パ", "ベ", "动", "拳", "銃", "S", "．", "R", "制", "式", "既", "发", "よ", "W", "ョ",
"番", "ェ", "广", "范", "围", "攻", "击", "チ", "ャ", "榴", "硫", "酸", "烧", "夷", "ロ", "ち", "出", "強", "力", "兵", "扱", "难", "炎", "放", "射", "喷", "燃", "限", "续", "切", "殊", "队",
"オ", "连", "極", "め", "威", "薬", "头", "炸", "セ", "扩", "散", "含", "目", "标", "当", "黄", "金", "矢", "じ", "分", "本", "「", "」", "裏", "側", "カ", "ギ", "铠", "纹", "章", "コ", "形",
"首", "輪", "押", "隐", "携", "帯", "一", "绪", "数", "回", "运", "ぶ", "ぺ", "ミ", "度", "足", "“", "G", "L", "Ｅ", "Y", "”", "供", "走", "爆", "楽", "谱", "曲", "最", "初", "后", "救",
"急", "受", "伤", "完", "全", "治", "地", "方", "生", "キ", "ピ", "开", "ツ", "单", "纯", "构", "造", "古", "び", "简", "剑", "刻", "盾", "兜", "号", "室", "鷲", "ダ", "（", "）", "別", "ろ",
"狼", "下", "铁", "石", "製", "八", "角", "エ", "电", "气", "示", "態", "定", "场", "扉", "星", "象", "绘", "十", "字", "突", "起", "ウ", "风", "太", "阳", "三", "月", "二", "四", "先", "端",
"六", "ゼ", "饰", "施", "女", "性", "横", "顔", "浮", "彫", "市", "名", "作", "外", "周", "込", "む", "際", "无", "擦", "ゴ", "光", "成", "赤", "宝", "美", "輝", "青", "色", "恶", "除", "草",
"剂", "枯", "偽", "文", "遊", "！", "鍮", "血", "清", "量", "瓶", "何", "U", "M", "N", "ｏ", "J", "O", "P", "混", "ｅ", "ｌ", "w", "少", "茶", "褐", "液", "失", "败", "毒", "ぜ", "危",
"险", "味", "ゆ", "ふ", "ぁ", "白", "表", "デ", "犬", "笛", "人", "间", "闻", "音", "域", "呼", "吹", "ヒ", "ズ", "ユ", "巨", "集", "通", "箱", "虫", "直", "径", "㎝", "程", "ぼ", "択", "决",
"や", "位", "戻", "转", "収", "微", "整", "ど", "法", "反", "应", "模", "样", "描", "我", "觉", "仮", "信", "图", "送", "木", "台", "粘", "付", "映", "御", "紧", "対", "相", "刺", "ひ", "閃",
"来", "的", "万", "与", "感", "针", "鈞", "×", "线", "前", "K", "D", "Ｉ", "ゲ", "H", "贩", "予", "源", "心", "穴", "円", "筒", "属", "鼻", "口", "封", "々", "多", "錠", "飲", "丈", "夫",
"眠", "お", "扫", "異", "臭", "雷", "痕", "飞", "经", "具", "半", "充", "同", "寮", "喰", "破", "不", "燭", "明", "冷", "照", "报", "告", "打", "骑", "士", "短", "胴", "贯", "違", "命", "汚",
"壶", "摇", "床", "仲", "探", "索", "计", "内", "歯", "车", "小", "左", "右", "国", "重", "差", "額", "干", "ビ", "割", "存", "状", "暗", "闇", "雾", "子", "ホ", "早", "奥", "監", "視", "術",
"陶", "并", "皿", "栓", "夕", "马", "棚", "焦", "げ", "ノ", "行", "修", "天", "墓", "碑", "像", "棺", "朽", "狂", "踊", "盛", "迫", "锁", "绿", "操", "効", "引", "酒", "服", "乱", "杂", "樫",
"ワ", "香", "高", "价", "念", "练", "习", "配", "脏", "判", "削", "凝", "球", "儀", "望", "远", "镜", "年", "悲", "鳥", "ご", "骸", "崩", "湿", "络", "立", "铜", "跡", "届", "银", "魂", "腕",
"勇", "贤", "聖", "紫", "元", "守", "脚", "材", "暖", "谁", "踏", "荒", "专", "门", "ほ", "厚", "传", "帝", "王", "幅", "黑", "话", "断", "暴", "钵", "工", "退", "路", "确", "嫌", "豪", "华",
"着", "ソ", "替", "期", "衣", "央", "姿", "魔", "近", "づ", "熱", "皮", "肤", "役", "骨", "董", "妨", "裁", "格", "奇", "妙", "ぱ", "帐", "民", "牙", "盤", "蓄", "堵", "言", "葉", "屋", "丸",
"脱", "搜", "塗", "群", "眾", "祈", "悔", "脅", "侵", "止", "壁", "腐", "奴", "仕", "業", "容", "掛", "染", "昆", "炉", "渡", "ぎ", "森", "边", "建", "築", "途", "似", "階", "降", "母", "ね",
"苦", "休", "新", "樽", "積", "助", "ォ", "尽", "椅", "查", "任", "损", "神", "軸", "固", "鹿", "剝", "牛", "哲", "類", "閱", "覽", "落", "窓", "夺", "喜", "始", "快", "道", "殻", "復", "得",
"知", "肉", "乘", "食", "滝", "流", "透", "势", "笑", "邪", "岩", "掘", "点", "插", "彼", "系", "械", "進", "各", "北", "減", "ﾍ", "南", "憎", "惡", "東", "西", "讐", "欲", "末", "雄", "叫",
"纵", "、", "薪", "粗", "包", "吊", "縛", "解", "塞", "段", "廃", "棄", "土", "搬", "正", "消", "机", "思", "娘", "良", "花", "害", "駆", "腰", "磨", "ヤ", "影", "ぽ", "貯", "蔵", "革", "会",
"优", "胜", "署", "蜂", "巢", "板", "吐", "赏", "授", "试", "管", "向", "=", "+", "，", "识", "蛇", "療", "蛾", "茂", "況", "背", "綺", "麗", "洗", "粧", "根", "詰", "響", "浸", "潜", "網",
"压", "耐", "排", "故", "再", "了", "低", "昇", "現", "維", "原", "因", "界", "達", "至", "衝", "採", "兼", "溶", "编", "卵", "ザ", "顯", "情", "晶", "袋", "易", "精", "密", "医", "共", "互",
"揭", "薄", "社", "証", "離", "漢", "粉", "触", "孔", "巖", "頑", "輕", "可", "能", "庫", "爪", "便", "醜", "ぞ", "哀", "ぐ", "赖", "追", "俺", "他", "抵", "产", "堂", "ゃ", "怪", "ょ", "怖",
"逃", "恐", "声", "普", "迷", "―", "待", "棒", "步", "借", "釈", "順", "听", "掴", "君", "静", "ぇ", "援", "喋", "噛", "絶", "痛", "倒", "陰", "裂", "答", "願", "終", "贷", "返", "ざ", "ぉ",
"忙", "独", "恩", "欠", "考", "片", "避", "牢", "狱", "然", "堪", "野", "郎", "嬢", "防", "男", "演", "溃", "都", "隷", "责", "令", "妻", "世", "好", "许", "加", "减", "昔", "‘", "’", "ｓ",
"。", "越", "/", "憩", "设", "说", "指", "徒", "b", "ｉ", "ｈ", "ａ", "z", "ｒ", "ｄ", "闭", "读", "详", "在", "ｕ", "ｃ", "ｋ", "满", "林", "战", "息", "件", "频", "袭", "坠", "梦", "疲",
"房", "帰"};

        /// <summary>
        /// 表示哪些文字使用旧字库的图片
        /// </summary>
        private List<string> useOldFont = new List<string>() {
            "　", "▷", "▽", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "：", "%", "B", "I", "F", "E", "e", "n", "t", "r", "h", "s", "u", "v", "i", "a", "l", "o", ".",
"f", "c", "'", "k", "y", "d", "g", "？", "…", "゜", 
"V", "-", "A", "C", "T", "S", "．", "R", "「", "」",
"G", "L", "Y", "（", "）",
"！", "U", "M", "N", "J", "O", "P", "w", 
"㎝", "×", "K", "D", 
"、", "+", "。", "/", "b", "z"

        };
//        private List<string> useOldFont = new List<string>() {
//            "　", "▷", "▽", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "：", "%", "B", "I", "F", "E", "e", "n", "t", "r", "h", "s", "u", "v", "i", "a", "l", "o", ".",
//"f", "c", "'", "k", "y", "d", "g", 
//"゜", 
//"V", "-", "A", "C", "T", "Ｆ", "Ｂ", 
//"m", "S", "．", "R", "W", 
//"「", "」", 
//"“", "G", "L", "Ｅ", "Y", "”", 
//"（", "）", 
//"！", "U", "M", "N", "ｏ", "J", "O", "P", "ｅ", "ｌ", "w", 
//"㎝", 
//"K", "D", "Ｉ", "H", 
//"、", 
//"=", "+", "，", 
//"‘", "’", "ｓ",
//"。", "/", "b", "ｉ", "ｈ", "ａ", "z", "ｒ", "ｄ", "ｕ", "ｃ", "ｋ" 
//        };

        #endregion

        #region " 文件字库 "

        /// <summary>
        /// 生化危机1各种文件的字库
        /// </summary>
        private static string[] bio1FileFontChars = { 
"　", "▷", "▽", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "：", "%", "B", "I", "F", "E", "e", "n", "t", "r", "h", "s", "u", "v", "i", "a", "l", "o", ".",
"f", "c", "'", "k", "y", "d", "g", "プ", "レ", "イ", "ン", "グ", "マ", "ニ", "ュ", "ア", "ル", "※", "コ", "ト", "ロ", "ー", "ラ", "タ", "を", "変", "更", "し", "て", "い", "る", "場",
"合", "は", "、", "ボ", "名", "読", "み", "替", "え", "ご", "覧", "く", "だ", "さ", "。", "＜", "ス", "テ", "画", "面", "呼", "び", "出", "す", "＞", "ゲ", "ム", "中", "に", "Y", "押", "（",
"ベ", "や", "ダ", "メ", "ジ", "受", "け", "時", "せ", "ま", "ん", "）", "こ", "の", "で", "以", "下", "機", "能", "が", "使", "゜", "「", "武", "器", "装", "備", "」", "用", "M", "A", "P",
"表", "示", "フ", "ァ", "見", "…", "ｅ", "ｔ", "ｃ", "上", "部", "か", "ら", "選", "択", "Z", "も", "可", "未", "入", "手", "今", "行", "っ", "た", "事", "あ", "屋", "れ", "載", "情", "報",
"無", "色", "探", "索", "な", "状", "態", "橙", "残", "緑", "全", "取", "赤", "扉", "鍵", "閉", "白", "開", "青", "通", "R", "Ｅ", "L", "S", "U", "V", "Ｉ", "追", "加", "～", "Ｂ", "O",
"×", "身", "確", "認", "内", "リ", "移", "動", "方", "向", "き", "ィ", "ッ", "ク", "十", "字", "そ", "対", "力", "続", "段", "差", "昇", "り", "降", "ヤ", "腰", "ぐ", "高", "前", "欄", "ド",
"実", "ば", "敵", "攻", "撃", "勢", "つ", "ガ", "一", "番", "近", "銃", "構", "法", "現", "在", "度", "倒", "C", "ケ", "ネ", "持", "映", "像", "と", "呪", "書", "誰", "宛", "セ", "記", "四",
"仮", "わ", "ち", "口", "鼻", "目", "三", "揃", "う", "災", "再", "蘇", "捨", "モ", "日", "ペ", "サ", "卿", "所", "隠", "々", "考", "結", "果", "僕", "デ", "思", "狂", "爆", "饲", "犬", "守",
"ど", "ろ", "？", "お", "気", "大", "食", "堂", "階", "西", "君", "笛", "じ", "ゃ", "願", "首", "輪", "渡", "信", "奴", "頼", "礼", "必", "ず", "欲", "例", "ツ", "よ", "む", "ョ", "植", "物",
"学", "薬", "効", "人", "類", "太", "古", "様", "傷", "病", "癒", "本", "山", "地", "周", "辺", "自", "生", "ハ", "ブ", "げ", "概", "要", "述", "べ", "種", "存", "ぞ", "異", "草", "体", "回",
"復", "等", "毒", "消", "具", "但", "外", "的", "単", "独", "何", "調", "発", "揮", "同", "士", "知", "死", "処", "理", "関", "諸", "注", "意", "化", "真", "判", "活", "①", "燃", "②", "頭",
"破", "壊", "志", "者", "為", "洋", "館", "Ｆ", "オ", "分", "道", "伸", "着", "火", "達", "！", "育", "係", "誌", "ａ", "ｙ", "．", "夜", "警", "員", "エ", "研", "究", "ポ", "カ", "ね", "ェ",
"俺", "新", "世", "話", "皮", "ひ", "ゴ", "豚", "投", "足", "ぎ", "臓", "引", "遊", "朝", "頃", "宇", "宙", "服", "防", "護", "衣", "突", "然", "起", "故", "連", "寝", "験", "昨", "背", "妙",
"ゆ", "腹", "飯", "抜", "味", "医", "務", "室", "バ", "ソ", "ウ", "貼", "眠", "ぜ", "腫", "静", "数", "め", "逃", "射", "胸", "肉", "落", "へ", "ぬ", "－", "紙", "愛", "J", "ｕ", "ｎ", "届",
"喜", "悲", "野", "郎", "電", "来", "ほ", "製", "会", "社", "先", "月", "漏", "感", "染", "寮", "正", "彼", "歩", "叩", "瞳", "性", "光", "間", "脳", "恐", "永", "久", "去", "過", "尽", "症",
"進", "遅", "失", "耐", "屍", "安", "後", "二", "覚", "決", "断", "解", "切", "遠", "チ", "ホ", "保", "長", "T", "I", "-", "D", "づ", "週", "作", "戦", "速", "順", "次", "誘", "込", "W",
"兵", "得", "含", "胚", "個", "収", "廃", "棄", "殺", "遺", "，", "緒", "兆", "候", "仕", "浴", "多", "私", "最", "参", "悔", "準", "終", "勇", "幕", "裏", "文", "許", "命", "成", "張", "他",
"比", "強", "影", "響", "宿", "主", "想", "困", "難", "形", "球", "栄", "養", "源", "根", "直", "水", "槽", "侵", "流", "品", "急", "促", "天", "井", "広", "ぶ", "獲", "丁", "触", "巻", "吸",
"官", "血", "睡", "侵", "既", "职", "牺", "牲", "戻", "総", "花", "弁", "際", "露", "增", "告", "言", "秘", "密", "へ", "有", "常", "细", "胞", "共", "質", "繰", "返", "興", "深", "明", "系",
"N", "o", "減", "付", "計", "算", "接", "秒", "则", "徒", "混", "良", "特", "徽", "扱", "簡", "W", "黄", "茶", "褐", "写", "『", "好", "父", "＆", "』", "親", "検", "査", "反", "応", "陽",
"姿", "爆", "置", "資", "料", "脱", "ミ", "公", "キ", "シ", "除", "小", "端", "末", "パ", "ワ", "念", "线", "暗", "号", "聡", "楽", "h", "ノ", "識", "休", "期", "组", "织", "築", "改", "造",
"我", "筆", "筋", "ピ", "幅", "[", "]", "与", "亡", "瞬", "海", "虐", "“", "ゾ", "”", "放", "訳", "贵", "重", "避", "義", "危", "险", "冷", "冻", "墓", "衛", "管", "門", "统", "元", "害",
"策", "委", "阅", "禁", "容", "了", "第", "件", "被", "当", "初", "推", "定", "问", "题", "半", "队", "ぼ", "况", "属", "项", "点", "刻", "猶", "予", "讲", "制", "御", "走", "少", "惨", "ふ",
"奇", "功", "憂", "慮", "材", "提", "供", "望", "早", "州", "察", "介", "迅", "打", "政", "符", "及", "经", "营", "干", "限", "尚", "搭", "乘", "路", "顾", "紧", "停", "止", "轄", "设", "利",
"载", "量", "权", "ヴ", "任", "监", "须", "各", "专", "炉", "派", "遣", "督", "别", "指", "不", "观", "绿", "G", "至", "始", "祖", "年", "熟", "寄", "易", "钠", "贪", "葉", "相", "ヒ", "超",
"系", "導", "歴", "史", "塗", "忌", "嬢", "違", "詳", "ナ", "捕", "ぃ", "家", "族", "V", "裂", "败", "继", "抹", "济", "母", "约", "束", "偽", "颜", "鸣", "聞", "石", "箱", "匂", "ヤ", "邪",
"魔", "幾", "剂", "男", "愕", "忘", "袭", "格", "闇", "覆", "抱", "怖", "隙", "ょ", "粗", "運", "暇", "試", "待", "痛", "妻", "誕", "赠", "叔", "舞", "娘", "浮", "憶", "鮮", "益", "立", "女",
"敷", "尋", "双", "眸", "虎", "金", "标", "並", "滴", "陰", "洞", "窟", "片", "廊", "转", "辿", "幹", "心", "奪", "悪", "震", "息", "駄", "添", "换", "否", "依", "役", "盗", "绘", "伝", "美",
"術", "奥", "ぱ", "ヌ", "居"};

        /// <summary>
        /// 表示哪些文字使用旧字库的图片
        /// </summary>
        private List<string> useOldFileFont = new List<string>() {
"　", "▷", "▽", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "：", "%", "B", "I", "F", "E", "e", "n", "t", "r", "h", "s", "u", "v", "i", "a", "l", "o", ".",
"f", "c", "'", "k", "y", "d", "g",  
"、", "。", "Y", "（", "～",
"）", "゜", "「", "」", "M", "A", "P",
"Z", "R", "L", "S", "U", "V", "C", 
"？", "！", "．", "J", "T", "-", "D",
"N", "o", "G"
        };

        #endregion

        /// <summary>
        /// 临时的指针，指向上面的两个字库
        /// </summary>
        private string[] tempFontChars;

        #endregion

        #endregion

        #region " 构造方法 "

        /// <summary>
        /// 生化危机1 文本编辑工具
        /// </summary>
        public Bio1TextEditor()
        {
            InitializeComponent();

            this.ResetHeight();

            //this.gameName = "Bio1";
            this.baseFolder = @"G:\GitHub\HanhuaProject\Bio1";
            //this.baseFolder = @"E:\Study\MySelfProject\Hanhua\TodoCn\HanhuaProject\Bio1";
            this.subFolder = @"\WiiJp";
            this.txtCnEdit.OtherRichTextBox = this.txtJpEdit;

            this.isWii = true;
            if (this.baseFolder.IndexOf("Ngc") != -1)
            {
                this.isWii = false;
            }

            this.ReadCnFont();

            this.LoadAllText();
        }

        #endregion

        #region " 共有方法 "

        /// <summary>
        /// 根据输入的字符串，得到对应的位置
        /// </summary>
        /// <param name="text">字符串文本</param>
        /// <param name="isComText">是共通的文本还是文件的文本</param>
        /// <returns>文本对应的字符的位置</returns>
        public static string GetDiffData(string text, bool isComText)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            string[] byFontChar = isComText ? Bio1TextEditor.bio1FontChars : Bio1TextEditor.bio1FileFontChars;
            for (int i = 0; i < text.Length; i++)
            {
                string strChar = text.Substring(i, 1);
                for (int j = 0; j < byFontChar.Length; j++)
                {
                    if (byFontChar[j] == strChar)
                    {
                        sb.Append(" " + j);
                        break;
                    }
                }
            }

            return sb.ToString().Substring(1);
        }

        /// <summary>
        /// 取得当前字符集合
        /// </summary>
        /// <param name="isComText"></param>
        /// <returns></returns>
        public static string[] GetFontChars(bool isComText)
        {
            if (isComText)
            {
                return Bio1TextEditor.bio1FontChars;
            }
            else
            {
                return Bio1TextEditor.bio1FileFontChars;
            }
        }

        #endregion

        #region " 页面事件 "

        /// <summary>
        /// 保存翻译
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.Save())
            {
                MessageBox.Show("保存成功");
            }
        }

        /// <summary>
        /// 生成中文的字库文件
        /// (一次性的)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCnFontSave_Click(object sender, EventArgs e)
        {
            // 重新初始化字库字符
            this.ResetFontChar();

            MessageBox.Show("字库文字被重置，需要再次导入所有文本！");
        }

        /// <summary>
        /// 生成字库图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateFont_Click(object sender, EventArgs e)
        {
            Bio1FontEditor tplFontEditor = new Bio1FontEditor(this.baseFolder);
            tplFontEditor.Show();
        }

        /// <summary>
        /// 整理字库，重新保存文本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCnFont_Click(object sender, EventArgs e)
        {
            // 先重新写空白字库
            //this.btnCnFontSave_Click(this, new EventArgs());

            // 将所有的中文翻译文件，按照新字库的编码，重新保存

            //// 取得所有中文翻译文件
            //List<FilePosInfo> allFiles = this.GetAllCnText();

            //// 取得中文翻译文件中的所有字符
            //Dictionary<string, int> fontCharInfo = new Dictionary<string, int>();
            //string currentChar;
            //string nextChar;

            //foreach (FilePosInfo fileInfo in allFiles)
            //{
            //    string fileText = File.ReadAllText(fileInfo.File + "_info.txt", Encoding.UTF8).Replace("\n", string.Empty).Replace(@"------", string.Empty);
            //    for (int i = 0; i < fileText.Length - 1; i++)
            //    {
            //        currentChar = fileText.Substring(i, 1);
            //        if ("^" == currentChar)
            //        {
            //            // 关键字的解码
            //            while ((nextChar = fileText.Substring(++i, 1)) != "^")
            //            {
            //            }

            //            continue;
            //        }
            //        else
            //        {
            //            if (fontCharInfo.ContainsKey(currentChar))
            //            {
            //                fontCharInfo[currentChar] = fontCharInfo[currentChar] + 1;
            //            }
            //            else
            //            {
            //                fontCharInfo[currentChar] = 1;
            //            }
            //        }
            //    }
            //}

            //Dictionary<string, int> fontCharInfo1 = (from d in fontCharInfo
            //                                         orderby d.Value
            //                                         select d).ToDictionary(k => k.Key, v => v.Value);
        }

        /// <summary>
        /// 检查翻译的长度的正确性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCheck_Click(object sender, EventArgs e)
        {
            // 检查输入的中文长度是否正确
            this.CheckCnText();
        }

        /// <summary>
        /// 变更文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lstFile.SelectedIndex < 0)
            {
                return;
            }

            FilePosInfo filePosInfo = this.lstFilePos[this.lstFile.SelectedIndex];
            try
            {
                string fileName = this.TrimFileName(filePosInfo.File);
                string jpFile = this.GetFileName(fileName, string.Empty);

                // 判断文件类型
                if (filePosInfo.File.IndexOf("subscr_2") >= 0 || filePosInfo.File.IndexOf("main_3") >= 0)
                {
                    this.isComText = false;
                }
                else
                {
                    this.isComText = true;
                }

                // 判断是否存在文本映射表
                this.hasEntry = filePosInfo.PosInfo.Length == 3;

                // 设置地址输入框
                filePosInfo.TextStart = Convert.ToInt32(filePosInfo.PosInfo[0], 16);
                this.SetInputPos(jpFile, filePosInfo.TextStart,
                    Convert.ToInt32(filePosInfo.PosInfo[filePosInfo.PosInfo.Length - 1], 16), this.hasEntry);

                // 开始读取文件
                this.txtCnEdit.Text = string.Empty;
                this.cnFile = jpFile.Replace("Jp", "Cn");
                //if (!File.Exists(this.cnFile))
                //{
                //    File.Copy(jpFile, this.cnFile);
                //}

                this.txtCnEdit.Text = this.ReadText(false, this.cnFile, filePosInfo.TextStart, this.hasEntry);
                //this.txtCnEdit.Text = File.ReadAllText(filePosInfo.File + "_info.txt", Encoding.UTF8);
                this.txtJpEdit.Text = this.ReadText(true, jpFile, filePosInfo.TextStart, this.hasEntry); 
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
            }
        }

        /// <summary>
        /// 将汉化文件Copy到相应目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPatch_Click(object sender, EventArgs e)
        {
            // 将汉化的文件Copy相应的地方
            string targetPath = this.baseFolder + @"\生化危机1文件";
            this.CopyFiles(this.baseFolder + @"\etc\", targetPath, true);
            this.CopyFiles(this.baseFolder + @"\subscr\", targetPath, true);
            this.CopyFiles(this.baseFolder + @"\st1\", targetPath, true);
            this.CopyFiles(this.baseFolder + @"\st2\", targetPath, true);
            this.CopyFiles(this.baseFolder + @"\st3\", targetPath, true);
            this.CopyFiles(this.baseFolder + @"\st4\", targetPath, true);
            this.CopyFiles(this.baseFolder + @"\st5\", targetPath, true);
            if (this.isWii)
            {
                this.CopyFiles(this.baseFolder + @"\sys\", targetPath, true);
            }
            else
            {
                this.CopyFiles(this.baseFolder + @"\&&systemdata\", targetPath, true);
            }

            // Copy各种图片
            Util.NeedCheckTpl = true;
            this.CopyFiles(this.baseFolder + @"\fixGraphic\", targetPath, false);
            this.CopyFiles(this.baseFolder + @"\strap\", targetPath, false);
            this.CopyFiles(this.baseFolder + @"\tpl\", targetPath, false);
            Util.NeedCheckTpl = false;

            MessageBox.Show("Copy完成！");
        }

        /// <summary>
        /// 将所有翻译文本导出到Excel中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application xApp = null;
            Microsoft.Office.Interop.Excel.Workbook xBook = null;
            Microsoft.Office.Interop.Excel.Worksheet xSheet = null;

            // 设定保存的文件名
            string fileName = this.baseFolder + @"\ComText.xls";
            string[] items = File.ReadAllLines(this.baseFolder + @"\ComText.txt");
            if (!this.isComText)
            {
                fileName = this.baseFolder + @"\FileText.xls";
                items = File.ReadAllLines(this.baseFolder + @"\FileText.txt");
            }

            // 显示进度条
            this.ResetProcessBar(items.Length / 2);

            try
            {
                // 创建Application对象 
                xApp = new Microsoft.Office.Interop.Excel.ApplicationClass();
                //xApp.Visible = true;

                // 追加一个WorkBook
                xBook = xApp.Workbooks.Add(Missing.Value);

                for (int j = 0; j < items.Length; j += 2)
                {
                    int sheetIndex = -1;
                    string sheetName = string.Empty;
                    for (int i = 0; i < this.lstFile.Items.Count; j++)
                    {
                        sheetName = Util.GetShortFileName(this.lstFilePos[i].File);
                        if (items[j].IndexOf(sheetName) >= 0)
                        {
                            sheetIndex = i;
                            break;
                        }
                    }

                    if (sheetIndex > -1)
                    {
                        // 更新当前文本
                        this.lstFile.SelectedIndex = sheetIndex;

                        // 取得日文、中文文本
                        string jpText = this.txtJpEdit.Text;
                        string cnText = this.txtCnEdit.Text;

                        // 追加一个Sheet
                        xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                        xSheet.Name = Util.GetShortFileName(sheetName);

                        // 将每行文本保存到Sheet中
                        string[] jpTexts = jpText.Split('\n');
                        string[] cnTexts = cnText.Split('\n');
                        for (int i = 0; i < jpTexts.Length; i++)
                        {
                            // 写入日文文本
                            Microsoft.Office.Interop.Excel.Range rngJp = xSheet.get_Range("A" + (i + 1), Missing.Value);
                            rngJp.Value2 = jpTexts[i];
                        }
                        for (int i = 0; i < cnTexts.Length; i++)
                        {
                            // 写入中文文本
                            Microsoft.Office.Interop.Excel.Range rngCn = xSheet.get_Range("G" + (i + 1), Missing.Value);
                            rngCn.Value2 = cnTexts[i];
                        }
                    }

                    // 更新进度条
                    this.ProcessBarStep();
                }

                // 保存
                //xBook.Save();
                xSheet.SaveAs(
                    fileName,
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                // 隐藏进度条
                this.CloseProcessBar();

                // 显示保存完成信息
                MessageBox.Show((this.isComText ? "共通的翻译文本" : "文件的翻译文本") + "导出完成！");

            }
            catch (Exception me)
            {
                MessageBox.Show(this.baseFile + "\n" + me.Message);
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

        /// <summary>
        /// 将Excel中所有翻译文本导入到各个子文件中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImport_Click(object sender, EventArgs e)
        {
            // 打开要导入的文件
            this.baseFile = Util.SetOpenDailog("生化危机1 翻译文件（*.xls）|*.xls", string.Empty);
            if (string.IsNullOrEmpty(this.baseFile))
            {
                return;
            }

            // 设定保存的文件名
            //string fileName = @"E:\My\Hanhua\testFile\bio1Text\ComText.xls";
            //string fileName = @"D:\game\iso\wii\生化危机1汉化\ComText.xls";
            //if (!this.isComText)
            //{
            //    //fileName = @"E:\My\Hanhua\testFile\bio1Text\FileText.xls";
            //    fileName = @"D:\game\iso\wii\生化危机1汉化\FileText.xls";
            //}
            string fileName = this.baseFile;

            Microsoft.Office.Interop.Excel.Application xApp = null;
            Microsoft.Office.Interop.Excel.Workbook xBook = null;
            Microsoft.Office.Interop.Excel.Worksheet xSheet = null;

            try
            {
                // 创建Application对象 
                xApp = new Microsoft.Office.Interop.Excel.ApplicationClass();
                //xApp.Visible = true;

                // 得到WorkBook对象, 打开已有的文件 
                xBook = xApp.Workbooks._Open(
                    fileName,
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                // 显示进度条
                this.ResetProcessBar(xBook.Sheets.Count);

                for (int j = 1; j <= xBook.Sheets.Count; j++)
                {
                    // 查找当前sheet对应的文本
                    xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets[j];
                    int sheetIndex = -1;
                    string sheetName = string.Empty;
                    for (int i = 0; i < this.lstFile.Items.Count; i++)
                    {
                        sheetName = Util.GetShortFileName(this.lstFilePos[i].File);
                        if (sheetName.IndexOf(xSheet.Name) >= 0)
                        {
                            sheetIndex = i;
                            break;
                        }
                    }

                    if (sheetIndex > -1)
                    {
                        // 更新当前文本
                        this.lstFile.SelectedIndex = sheetIndex;

                        // 取得当前Sheet的中文文本
                        int lineNum = 1;
                        int blankNum = 0;
                        StringBuilder sb = new StringBuilder();
                        while (blankNum < 4)
                        {
                            string cellValue = xSheet.get_Range("G" + lineNum, Missing.Value).Value2 as string;
                            sb.Append(cellValue).Append("\n");

                            if (string.IsNullOrEmpty(cellValue))
                            {
                                blankNum++;
                            }
                            else
                            {
                                blankNum = 0;
                            }

                            lineNum++;
                        }

                        sb = sb.Replace("\n\n\n\n\n", "\n");

                        this.txtCnEdit.Text = sb.ToString();

                        // 保存
                        if (!this.Save())
                        {
                            throw new Exception("有文件检查失败");
                        }
                    }

                    // 更新进度条
                    this.ProcessBarStep();
                }

                // 隐藏进度条
                this.CloseProcessBar();

                // 显示保存完成信息
                MessageBox.Show("导入完成！");

            }
            catch (Exception me)
            {
                MessageBox.Show(this.baseFile + "\n" + me.Message);
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

        #endregion

        #region " 私有方法 "

        /// <summary>
        /// 将源目录下的文件Copy到目标目录
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        private void CopyFiles(string source, string target, bool withCn)
        {
            List<FilePosInfo> fileNameInfo = Util.GetAllFiles(source).Where(p => !p.IsFolder && (!withCn || (withCn && p.File.IndexOf("_cn.") >= 0))).ToList();
            if (fileNameInfo.Count == 0)
            {
                return;
            }

            foreach (FilePosInfo fileInfo in fileNameInfo)
            {
                string targetFile = fileInfo.File.Replace("_cn", string.Empty).Replace("_文本字库", string.Empty);

                if (fileInfo.File.IndexOf("main") != -1 || !this.isWii)
                {
                    targetFile = targetFile.Replace(this.baseFolder, target);
                }
                else
                {
                    targetFile = targetFile.Replace(this.baseFolder, target + @"\files");
                }

                if (!File.Exists(targetFile))
                {
                    string shortName = Util.GetShortName(targetFile);
                    System.IO.Directory.CreateDirectory(targetFile.Replace(shortName, string.Empty));
                }

                File.Copy(fileInfo.File, targetFile, true);
            }
        }

        /// <summary>
        /// 重新初始化字库字符
        /// </summary>
        private void ResetFontChar()
        {
            if (this.isComText)
            {
                string[] bio1FontChars = new string[Bio1TextEditor.bio1FontChars.Length];

                for (int i = 0; i < bio1FontChars.Length; i++)
                {
                    if (this.useOldFont.Contains(Bio1TextEditor.bio1FontChars[i]))
                    {
                        bio1FontChars[i] = Bio1TextEditor.bio1FontChars[i];
                    }
                    else
                    {
                        bio1FontChars[i] = "*";
                    }
                }

                File.WriteAllLines(cnTextFont, bio1FontChars, Encoding.UTF8);
            }
            else
            {
                string[] bio1FileFontChars = new string[Bio1TextEditor.bio1FileFontChars.Length];

                for (int i = 0; i < bio1FileFontChars.Length; i++)
                {
                    if (this.useOldFileFont.Contains(Bio1TextEditor.bio1FileFontChars[i]))
                    {
                        bio1FileFontChars[i] = Bio1TextEditor.bio1FileFontChars[i];
                    }
                    else
                    {
                        bio1FileFontChars[i] = "*";
                    }
                }

                File.WriteAllLines(cnFileFont, bio1FileFontChars, Encoding.UTF8);
            }
        }

        /// <summary>
        /// 读取中文字库
        /// </summary>
        private void ReadCnFont()
        {
            this.bio1CnTextChars = File.ReadAllLines(cnTextFont, Encoding.UTF8);
            this.bio1CnFileChars = File.ReadAllLines(cnFileFont, Encoding.UTF8);

            string[] cnFont = this.isComText ? this.bio1CnTextChars : this.bio1CnFileChars;
            this.cnFontMap.Clear();
            for (int i = 0; i < cnFont.Length; i++)
            {
                this.cnFontMap.Add(new FontCharInfo(cnFont[i], i));
            }
        }

        /// <summary>
        /// 取得所有翻译的文件
        /// </summary>
        /// <returns></returns>
        private List<FilePosInfo> GetAllCnText()
        {
            List<FilePosInfo> allFiles;
            if (this.isComText)
            {
                allFiles = this.lstFilePos.Where(p => p.File.IndexOf("subscr_2") == -1 && p.File.IndexOf("main_3") == -1).ToList();
            }
            else
            {
                allFiles = this.lstFilePos.Where(p => p.File.IndexOf("subscr_2") != -1 || p.File.IndexOf("main_3") != -1).ToList();
            }

            return allFiles;
        }

        /// <summary>
        /// 取得文件名称
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="cnInfo"></param>
        /// <returns></returns>
        private string GetFileName(string fileName, string cnInfo)
        {
            return fileName + cnInfo + ((fileName.IndexOf("main") == -1) ? ".dat" : ".dol");
        }

        /// <summary>
        /// 去掉文件名称的关联后缀
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string TrimFileName(string name)
        {
            return name.Replace("_1", string.Empty).Replace("_2", string.Empty).Replace("_3", string.Empty);
        }

        /// <summary>
        /// 取得所有保存的文本
        /// </summary>
        private void LoadAllText()
        {
            // 初始化
            this.ClearInputPos();
            this.lstFilePos.Clear();
            this.lstFile.Items.Clear();

            // 读取文本配置信息
            string[] comTextInfo = File.ReadAllLines(this.baseFolder + @"\WiiComText.txt");
            string[] fileTextInfo = File.ReadAllLines(this.baseFolder + @"\WiiFileText.txt");
            string[] textInfo = new string[comTextInfo.Length + fileTextInfo.Length];
            Array.Copy(comTextInfo, 0, textInfo, 0, comTextInfo.Length);
            Array.Copy(fileTextInfo, 0, textInfo, comTextInfo.Length, fileTextInfo.Length);

            // 根据配置的信息，读取各个文件文本
            for (int i = 0; i < textInfo.Length; i += 2)
            {
                string fileName = textInfo[i].StartsWith("sys") ? textInfo[i] : @"files\" + textInfo[i];
                string fullName = this.baseFolder + this.subFolder + @"\" + fileName;
                if (File.Exists(this.GetFileName(this.TrimFileName(fullName), string.Empty)))
                {
                    string[] posInfo = textInfo[i + 1].Split(' ');

                    this.lstFile.Items.Add(this.TrimFileName(fileName)
                        + "（" + textInfo[i + 1] + "）");
                    this.lstFilePos.Add(new FilePosInfo(fullName, posInfo));
                }
            }

            // 选中第一个
            this.lstFile.SelectedIndex = 0;
        }

        /// <summary>
        /// 检查输入的中文长度是否正确
        /// </summary>
        /// <returns>输入的中文长度是否正确</returns>
        private bool CheckCnText()
        {
            if (this.hasEntry)
            {
                //int jpLen = this.txtJpEdit.Text.Replace("^", string.Empty).Length;
                //int cnLen = this.txtCnEdit.Text.Replace("^", string.Empty).Length;

                //if (jpLen < cnLen)
                //{
                //    this.DisplayTitle(jpLen, cnLen);
                //    return false;
                //}
                //else
                //{
                //    this.DisplayTitle(0, 0);
                //    this.txtCnEdit.BackColor = SystemColors.Window;
                //    return true;
                //}
                return true;
            }
            else
            {
                string[] jpTexts = Regex.Split(this.txtJpEdit.Text.Replace("^1^^6^", "^1 0^"), @"1 0");
                string[] cnTexts = Regex.Split(this.txtCnEdit.Text.Replace("^1^^6^", "^1 0^"), @"1 0");
                string jpText = string.Empty;
                string cnText = string.Empty;
                int selectionStart = 0;
                for (int i = 0; i < cnTexts.Length; i++)
                {
                    jpText = jpTexts[i];
                    cnText = cnTexts[i];

                    //if (cnText.Length > jpText.Length || cnText.Length < jpText.Length)
                    //{
                    //    this.DisplayTitle(jpText.Length, cnText.Length);
                    //    this.txtCnEdit.Focus();
                    //    this.txtCnEdit.SelectionStart = selectionStart;
                    //    this.txtCnEdit.SelectionLength = cnText.Length;
                    //    this.txtCnEdit.SelectionColor = Color.SeaGreen;

                    //    return false;
                    //}
                    
                    // 关键字是否被删除的判断
                    foreach (string keyWord in this.keyWords)
                    {
                        if (jpText.IndexOf(keyWord) != -1
                            && cnText.IndexOf(keyWord) == -1)
                        {
                            this.txtCnEdit.SelectionStart = selectionStart - cnText.Length - 1;
                            this.txtCnEdit.SelectionLength = cnText.Length;
                            this.txtCnEdit.SelectionColor = Color.Red;
                            this.txtCnEdit.ScrollToCaret();
                            MessageBox.Show("关键字：" + keyWord + "不能被删除！");
                            return false;
                        }
                    }

                    selectionStart += cnText.Length + 3;
                }

                this.DisplayTitle(0, 0);
                this.txtCnEdit.SelectionColor = Color.Black;
                return true;
            }
        }

        /// <summary>
        /// 清空输入的地址
        /// </summary>
        private void ClearInputPos()
        {
            this.inputCnStartPos = 0;
            this.inputCnEndPos = 0;
        }

        /// <summary>
        /// 设置地址输入框
        /// </summary>
        private void SetInputPos(string fileName, int textStart, int textEnd, bool hasEntry)
        {
            try
            {
                int subTextStart = textStart;
                int subTextEnd = textEnd;

                if (hasEntry)
                {
                    // 将文件中的数据，循环读取到byData中
                    byte[] byData = File.ReadAllBytes(fileName);

                    int len = Util.GetOffset(byData, textStart + 6, textStart + 7);
                    int subEntryPosStart = textStart + 8;
                    subTextStart = textStart + Util.GetOffset(byData, subEntryPosStart, subEntryPosStart + 3);

                    if (byData[subTextEnd - 2] == 0 && byData[subTextEnd - 1] == 0)
                    {
                        subTextEnd -= 2;
                    }
                }

                this.inputCnStartPos = subTextStart;
                this.inputCnEndPos = subTextEnd;

                // 显示Title信息
                this.DisplayTitle(this.inputCnEndPos - this.inputCnStartPos, this.inputCnEndPos - this.inputCnStartPos);

                // 保存原始的长度
                this.oldTextLen = this.inputCnEndPos - this.inputCnStartPos;
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
            }
        }

        /// <summary>
        /// 显示Title信息
        /// </summary>
        /// <param name="jpBytes">日语的字节数</param>
        /// <param name="cnBytes">中文的字节数</param>
        private void DisplayTitle(int jpBytes, int cnBytes)
        {
            if (jpBytes < cnBytes)
            {
                this.Text = "Bio1文本编辑 翻译的文本数增加了：" + (cnBytes - jpBytes) + "文字！游戏可能无法运行！";
            }
            else if (jpBytes > cnBytes)
            {
                this.Text = "Bio1文本编辑 翻译的文本数减少了：" + (cnBytes - jpBytes) + "文字！游戏可能无法运行！";
            }
            else
            {
                this.Text = "Bio1文本编辑 翻译的文本数刚好。";
            }
        }

        /// <summary>
        /// 开始读取文本
        /// </summary>
        /// <param name="isJpText">读取日文还是中文文档</param>
        /// <param name="cnTextLoaded">中文文档是否已经被Load了</param>
        private string ReadText(bool isJpText, string fileName, int textStart, bool hasEntry)
        {
            this.textEntryStart = textStart;

            if (isJpText)
            {
                this.tempFontChars = this.isComText ? Bio1TextEditor.bio1FontChars : Bio1TextEditor.bio1FileFontChars;
            }
            else
            {
                // 读入中文字库
                this.tempFontChars = this.isComText ? this.bio1CnTextChars : this.bio1CnFileChars;
            }

            try
            {
                // 将文件中的数据，循环读取到byData中
                byte[] byData = File.ReadAllBytes(fileName);

                if (!hasEntry)
                {
                    int startPos = this.inputCnStartPos;
                    int maxLen = this.inputCnEndPos;

                    // 开始分析文档
                    byte[] byTextData = new byte[maxLen - startPos + 5];
                    Array.Copy(byData, startPos, byTextData, 0, byTextData.Length);

                    return this.DecodeTextByData(byTextData);
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    int len = Util.GetOffset(byData, textStart + 6, textStart + 7);
                    int subEntryPosStart = textStart + 8;

                    int subTextStart = textStart + Util.GetOffset(byData, subEntryPosStart, subEntryPosStart + 3);

                    //if (isJpText)
                    {
                        // 保存日文的原始数据
                        this.textEntrys.Clear();
                        this.oldTextEntrys.Clear();
                        this.entrysMap.Clear();
                        this.byMsgEntry = new byte[8 + len * 4];
                        Array.Copy(byData, textStart, this.byMsgEntry, 0, this.byMsgEntry.Length);
                        this.textEntrys.Add(subTextStart - textStart);
                        this.oldTextEntrys.Add(subTextStart - textStart);
                        this.entrysMap.Add(subTextStart - textStart, 0);

                        for (int i = 1; i < len; i++)
                        {
                            int subEntryPosEnd = textStart + 8 + i * 4;
                            int subTextStartEnd = textStart + Util.GetOffset(byData, subEntryPosEnd, subEntryPosEnd + 3);

                            this.textEntrys.Add(subTextStartEnd - textStart);
                            this.oldTextEntrys.Add(subTextStartEnd - textStart);
                            this.entrysMap.Add(subTextStartEnd - textStart, i);
                        }

                        this.textEntrys.Sort();
                        this.oldTextEntrys.Sort();
                    }

                    for (int i = 1; i < len; i++)
                    {
                        //int subEntryPosEnd = textStart + 8 + i * 4;
                        //int subTextStartEnd = textStart + Util.GetOffset(byData, subEntryPosEnd, subEntryPosEnd + 3);
                        int subTextStartEnd = this.textEntrys[i] + textStart;

                        //sb.Append((subTextStart - textStart).ToString() + ":").Append(this.GetSubLine(byData, subTextStart, subTextStartEnd));
                        sb.Append(this.GetSubLine(byData, subTextStart, subTextStartEnd));
                        sb.Append("\n------\n");

                        //subEntryPosStart = subEntryPosEnd;
                        subTextStart = subTextStartEnd;
                        
                        //if (isJpText)
                        //{
                        //    // 保存日文的原始数据
                        //    int entryI = subTextStart - textStart;
                        //    if (entryI < this.textEntrys[this.textEntrys.Count - 1])
                        //    {
                        //        this.noTextEntrys.Add(entryI);
                        //    }
                        //    this.textEntrys.Add(entryI);
                        //}
                    }

                    sb.Append(this.GetSubLine(byData, subTextStart, this.inputCnEndPos));

                    return sb.ToString().Replace("^9 2^^ffff^代", "^9 2^^ffff^^16b^").Replace("^8^り^1^", "^8^^fa^^1^").Replace("^8^イ^1^", "^8^^b4^^1^");
                }
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
                return string.Empty;
            }
        }

        /// <summary>
        /// 将字节数据翻译成文本字符串
        /// </summary>
        /// <param name="byData">字节数据</param>
        /// <returns>文本字符串</returns>
        private string DecodeTextByData(byte[] byData)
        {
            int step = 0;
            StringBuilder sb = new StringBuilder();
            int maxLen = byData.Length - 6;
            for (int i = 0; i < maxLen; i += step)
            {
                sb.Append(this.GetCharText(byData, i, ref step, maxLen));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 取得当前行文本
        /// </summary>
        /// <param name="byData">文本的字节数据</param>
        /// <param name="startPos">开始地址</param>
        /// <param name="endPos">结束地址</param>
        /// <returns>当前行文本</returns>
        private string GetSubLine(byte[] byData, int startPos, int endPos)
        {
            StringBuilder sb = new StringBuilder();
            int step = 0;

            for (int j = startPos; j < endPos; j += step)
            {
                sb.Append(this.GetCharText(byData, j, ref step, endPos));
            }

            //return startPos.ToString("x") + " : " + sb.ToString();
            return sb.ToString();
        }

        /// <summary>
        /// 查找文本开始的位置
        /// </summary>
        /// <returns>文本开始的位置</returns>
        private int SearchStartPos(int maxLen, byte[] byData, int startIndex)
        {
            for (int i = startIndex; i < maxLen; i += 2)
            {
                int index = byData[i] << 8 | byData[i + 1];
                int nextIndex = byData[i + 2] << 8 | byData[i + 3];

                if (index == 1 && nextIndex == 0)
                {
                    int before1 = byData[i - 2] << 8 | byData[i - 1];
                    int before2 = byData[i - 4] << 8 | byData[i - 3];
                    int before3 = byData[i - 6] << 8 | byData[i - 5];
                    int before4 = byData[i - 8] << 8 | byData[i - 7];
                    if (before1 >= 0x80 && before1 < 0x80 + this.tempFontChars.Length
                        && before2 >= 0x80 && before2 < 0x80 + this.tempFontChars.Length
                        && before3 >= 0x80 && before3 < 0x80 + this.tempFontChars.Length
                        && before4 >= 0x80 && before4 < 0x80 + this.tempFontChars.Length)
                    {
                        return i - 6;
                    }
                }
            }

            return 0;
        }

        /// <summary>
        /// 分析文本的当前值和下一个值
        /// </summary>
        /// <param name="byData">文本的字节数据</param>
        /// <param name="i">处理到第几个字节</param>
        /// <param name="step">需要往下跳多少个字节</param>
        /// <returns>当前文本</returns>
        private string GetCharText(byte[] byData, int i, ref int step, int endPos)
        {
            int value = byData[i] << 8 | byData[i + 1];
            int nextValue = byData[i + 2] << 8 | byData[i + 3];
            int nextNextValue = byData[i + 4] << 8 | byData[i + 5];

            string text = string.Empty;
            step = 2;
            switch (value)
            {
                // ^1 0^代表一行的结束
                case 1:
                    if (nextValue == 0)
                    {
                        if (i < endPos - 2)
                        {
                            text = "^1 0^\n";
                            step = 4;
                        }
                        else
                        {
                            text = "^1^\n";
                        }
                        //text = "^1 0^\n";
                        //step = 4;
                    }
                    else
                    {
                        text = "^1^";
                    }
                    break;

                // ^2 0^代表一行的开始？
                case 2:
                    if (nextValue == 0)
                    {
                        text = "^2 0^";
                        step = 4;
                    }
                    else
                    {
                        text = "^2^";
                    }
                    break;

                // ^3 1^代表特殊颜色（红色）文字开始
                // ^3 2^代表特殊颜色（绿色）文字开始
                // ^3 3^代表特殊颜色（蓝色）文字开始
                // ^3 0^代表特殊颜色文字结束
                case 3:
                    if (nextValue == 1 || nextValue == 2 || nextValue == 3)
                    {
                        text = "^3 " + nextValue + "^";
                        step = 4;
                    }
                    else if (nextValue == 0)
                    {
                        text = "^3 0^";
                        step = 4;
                    }
                    else
                    {
                        text = "^3^";
                    }
                    break;


                // ^5 7^代表换行，换行后文字有三角的闪动提示
                case 5:
                    if (nextValue == 7)
                    {
                        text = "^5 7^\n";
                        step = 4;
                    }
                    else
                    {
                        text = "^5^";
                    }
                    break;

                // ^6^代表直接换行
                case 6:
                    text = "^6^\n";
                    break;

                // ^7^代表文档中左右的两个箭头
                case 7:
                    text = "^7^\n";
                    break;

                // 控制字符
                case 8:
                    if (nextValue == 0xa9)
                    {
                        text = "^8 a9^";
                        step = 4;
                    }
                    else if (nextValue == 0x82)
                    {
                        text = "^8 82^";
                        step = 4;
                    }
                    else if (nextValue == 0x8e)
                    {
                        text = "^8 8e^";
                        step = 4;
                    }
                    else if (nextValue == 0x91)
                    {
                        text = "^8 91^";
                        step = 4;
                    }
                    else if (nextValue == 0x92)
                    {
                        text = "^8 92^";
                        step = 4;
                    }
                    else if (nextValue == 0x9b)
                    {
                        text = "^8 9b^";
                        step = 4;
                    }
                    else
                    {
                        text = "^8^";
                    }
                    break;

                // ^9 2^代表捡到某个物品？
                case 9:
                    if (nextValue == 1)
                    {
                        text = "^9 1 " + nextNextValue.ToString("x") + "^";
                        step = 6;
                        return text;
                    }
                    else if (nextValue == 2)
                    {
                        if (nextNextValue == 0xffff)
                        {
                            text = "^9 2^";
                            step = 4;
                        }
                        else
                        {
                            text = "^9 2 " + nextNextValue.ToString("x") + "^";
                            step = 6;
                            return text;
                        }
                    }
                    else
                    {
                        text = "^9^";
                    }
                    break;

                // ^b 1 25^代表锁门的地方的开始
                // ^b 1 26^代表锁门的地方的开始
                // ^b 1 29^代表锁门的地方，使用了钥匙
                // ^b 0 20^代表存档相关
                // ^b 0 21^代表存档相关
                case 0xB:
                    if (nextValue == 0)
                    {
                        if (nextNextValue == 0x20 || nextNextValue == 0x21 || nextNextValue == 0x22)
                        {
                            text = "^b 0 " + nextNextValue.ToString("x") + "^";
                            step = 6;
                            return text;
                        }
                    }
                    else if (nextValue == 1)
                    {
                        if (nextNextValue == 0x25 || nextNextValue == 0x26 || nextNextValue == 0x29)
                        {
                            text = "^b 1 " + nextNextValue.ToString("x") + "^";
                            step = 6;
                            return text;
                        }
                    }

                    text = "^b^";
                    break;

                // 控制字符
                case 0xC:
                    if (nextValue == 9 || nextValue == 0xffff)
                    {
                        text = "^c^";
                    }
                    else
                    {
                        text = "^c " + nextValue.ToString("x") + "^";
                        step = 4;
                    }
                    break;

                // 控制字符
                case 0xD:
                    if (nextValue == 4)
                    {
                        text = "^d 4 " + nextNextValue.ToString("x") + "^";
                        step = 6;
                    }
                    else
                    {
                        text = "^d^";
                    }
                    break;

                // 控制字符
                case 0xE:
                    if (nextValue == 5)
                    {
                        text = "^e 5 " + nextNextValue.ToString("x") + "^";
                        step = 6;
                    }
                    else
                    {
                        text = "^e^";
                    }
                    break;

                // ^11 B8^代表存档相关
                // ^11 1A8^代表存档相关
                case 0x11:
                    if (nextValue == 0xB8 || nextValue == 0x1A8 || nextValue == 0x12C || nextValue == 0x15E)
                    {
                        text = "^11 " + nextValue.ToString("x") + "^";
                        step = 4;
                    }
                    else
                    {
                        text = "^11^";
                    }
                    break;

                // 0x12代表疑问句结束，需要提示是还是否
                case 0x12:
                    text = "^12^";
                    break;

                // 0x16代表一个文档
                case 0x16:
                    text = "^16^";
                    break;

                // 0x18代表存档相关
                case 0x18:
                    if (nextValue == 0)
                    {
                        text = "^18 0^";
                        step = 4;
                    }
                    else
                    {
                        text = "^18^";
                    }
                    break;

                // 0x1b代表空格
                case 0x1B:
                    text = " ";
                    break;

                // 0xffff代表???
                case 0xffff:
                    if (nextValue == 367 || nextValue == 368 || nextValue == 370)
                    {
                        text = "^ffff " + nextValue.ToString("x") + "^";
                        step = 4;
                    }
                    else
                    {
                        text = "^ffff^";
                    }
                    break;

                default:
                    if (value >= 0x80 && value < 0x80 + this.tempFontChars.Length)
                    {
                        if (value == 0xca && nextValue == 0xbe && nextNextValue == 0x17d)
                        {
                            text = "^ca be 17d d8^";
                            step = 8;
                        }
                        else if (value == 0x80 && nextValue == 0x9 && nextNextValue == 0x2)
                        {
                            text = "^80 9 2 176^";
                            step = 8;
                        }
                        else if (value == 0x81 && nextValue == 0x9 && nextNextValue == 0x2)
                        {
                            text = "^81 9 2 177^";
                            step = 8;
                        }
                        else if (value == 0xcc && nextValue == 0xc6 && nextNextValue == 0x3cb)
                        {
                            text = "^cc c6 3cb^";
                            step = 6;
                        }
                        else
                        {
                            text = this.tempFontChars[value - 0x80];
                        }
                    }
                    else
                    {
                        text = "^" + value.ToString("x") + "^";
                    }
                    break;
            }

            return text;
        }

        /// <summary>
        /// 将中文文本转换成二进制数据
        /// </summary>
        /// <param name="cnBytes">中文的字节数据</param>
        /// <returns></returns>
        private bool EncodeCnText(List<byte> cnBytes, string jpAllText, string cnAllText, bool hasEntry)
        {
            string[] jpTexts;
            string[] cnTexts;
            if (hasEntry)
            {
                jpTexts = Regex.Split(jpAllText.Replace("\n", string.Empty), @"------");
                cnTexts = Regex.Split(cnAllText.Replace("\n", string.Empty), @"------");
            }
            else
            {
                jpTexts = jpAllText.Split('\n');
                cnTexts = cnAllText.Split('\n');
            }
            int maxLen = cnTexts.Length;
            int diffLen = 0;
            if (cnTexts[maxLen - 1] == "")
            {
                maxLen--;
            }

            for (int i = 0; i < maxLen; i++)
            {
                string jpText = jpTexts[i];
                string cnText = cnTexts[i];

                // 将当前行文本编码
                cnBytes.AddRange(this.EncodeLineText(cnText));

                if (hasEntry)
                {
                    // 如果当前行字节变化，修改后面的所有的Entry的偏移
                    diffLen = cnText.Length - jpText.Length;
                    if (diffLen != 0)
                    {
                        //int oldPos = this.oldTextEntrys[i];
                        //for (int j = i; j < maxLen; j++)
                        //{
                        //    if (this.oldTextEntrys[j] > oldPos)
                        //    {
                        //        this.textEntrys[j] += diffLen * 2;
                        //    }
                        //}
                        //int oldPos = this.oldTextEntrys[i];
                        for (int j = i + 1; j < maxLen; j++)
                        {
                            this.textEntrys[j] += diffLen * 2;
                        }
                    }
                }
            }

            // 修正TextEntry的数据
            if (hasEntry)
            {
                for (int j = 0; j < this.textEntrys.Count; j++)
                {
                    int entryIndex = 8 + this.entrysMap[this.oldTextEntrys[j]] * 4;

                    this.byMsgEntry[entryIndex] = (byte)((this.textEntrys[j] >> 24) & 0xFF);
                    this.byMsgEntry[entryIndex + 1] = (byte)((this.textEntrys[j] >> 16) & 0xFF);
                    this.byMsgEntry[entryIndex + 2] = (byte)((this.textEntrys[j] >> 8) & 0xFF);
                    this.byMsgEntry[entryIndex + 3] = (byte)(this.textEntrys[j] & 0xFF);
                }
            }

            return true;
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

                    string[] keyWords = keyWordsSb.ToString().Split(' ');
                    foreach (string keyWord in keyWords)
                    {
                        charIndex = Convert.ToInt32(keyWord, 16);
                        byData.Add((byte)((charIndex & 0xFF00) >> 8));
                        byData.Add((byte)(charIndex & 0xFF));
                    }

                    continue;
                }
                else if (" " == currentChar)
                {
                    byData.Add(0);
                    byData.Add(0x1B);
                }
                else
                {
                    charIndex = this.GetCharIndex(currentChar);
                    charIndex += 0x80;
                    byData.Add((byte)((charIndex & 0xFF00) >> 8));
                    byData.Add((byte)(charIndex & 0xFF));
                }
            }

            return byData.ToArray();
        }

        /// <summary>
        /// 取得当前文字的索引
        /// </summary>
        /// <param name="currenChar"></param>
        /// <returns></returns>
        private int GetCharIndex(string currentChar)
        {
            //bool getedInsertIndex = false;
            //int insertIndex = -1;

            //for (int i = 0; i < this.cnFontMap.Count; i++)
            //{
            //    FontCharInfo fontChar = this.cnFontMap[i];
            //    if (fontChar.Char == currentChar)
            //    {
            //        return this.CheckMaxIndex(fontChar.Index);
            //    }
            //    else if (fontChar.Char == "*" && getedInsertIndex == false)
            //    {
            //        insertIndex = i;
            //        getedInsertIndex = true;
            //    }
            //}

            //if (insertIndex == -1)
            //{
            //    insertIndex = this.cnFontMap.Count;
            //    this.cnFontMap.Add(new FontCharInfo(currentChar, this.cnFontMap.Count));
            //    return this.CheckMaxIndex(insertIndex);
            //}
            //else
            //{
            //    this.cnFontMap[insertIndex].Char = currentChar;
            //    return insertIndex;
            //}
            for (int i = 0; i < this.cnFontMap.Count; i++)
            {
                FontCharInfo fontChar = this.cnFontMap[i];
                if (fontChar.Char == currentChar)
                {
                    return this.CheckMaxIndex(fontChar.Index);
                }
            }

            throw new Exception("没有找到相应文字：" + currentChar);
        }

        /// <summary>
        /// 查看使用的字符是否超过了字库容量
        /// </summary>
        /// <param name="index">原始的字符位置</param>
        /// <returns>翻译后使用的位置</returns>
        private int CheckMaxIndex(int index)
        {
            if (!this.isComText && index >= this.tempFontChars.Length)
            {
                return 0;
            }
            else
            {
                return index;
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        private bool Save()
        {
            // 读入中文字库
            this.ReadCnFont();

            // 检查输入的中文长度、关键字是否正确
            if (!this.CheckCnText())
            {
                return false;
            }

            // 将中文文本转换成二进制数据
            List<byte> cnBytes = new List<byte>();
            if (!this.EncodeCnText(cnBytes, this.txtJpEdit.Text, this.txtCnEdit.Text, this.hasEntry))
            {
                return false;
            }

            // 修正最大长度不足的字节
            if (this.oldTextLen != cnBytes.Count)
            {
                int diffLen = this.oldTextLen - cnBytes.Count;
                if (diffLen < 0)
                {
                    this.DisplayTitle(this.oldTextLen, cnBytes.Count);
                    return false;
                }
                while (diffLen > 0)
                {
                    cnBytes.Add(0);
                    diffLen--;
                }
            }

            // 保存二进制数据
            if (!this.Save(cnBytes, this.hasEntry))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 保存翻译
        /// </summary>
        /// <param name="cnBytes">翻译的字节数据</param>
        private bool Save(List<byte> cnBytes, bool hasEntry)
        {
            try
            {
                // 将文件中的数据，循环读取到byData中
                byte[] byData = File.ReadAllBytes(this.cnFile);

                int startPos = this.inputCnStartPos;
                int maxLen = this.inputCnEndPos;

                // 定义整个文件的字节数组
                int totalSize = startPos + cnBytes.Count + byData.Length - maxLen;
                byte[] allData = new byte[totalSize];

                // 复制前面未修改的部分
                Array.Copy(byData, 0, allData, 0, startPos);

                // 复制修改的部分
                Array.Copy(cnBytes.ToArray(), 0, allData, startPos, cnBytes.Count);

                // 复制后面未修改的部分
                Array.Copy(byData, maxLen, allData, startPos + cnBytes.Count, byData.Length - maxLen);

                // 如果是带Entry的Message，保存修改后的各个Entry
                if (hasEntry)
                {
                    Array.Copy(this.byMsgEntry, 0, allData, this.textEntryStart, this.byMsgEntry.Length);
                }

                // 翻译后的字节数组写入文件
                File.WriteAllBytes(this.cnFile, allData);

                // 显示Title信息
                this.DisplayTitle(this.inputCnEndPos - this.inputCnStartPos, cnBytes.Count);

                // 写字库文件
                //string[] newFont = new string[this.cnFontMap.Count];
                //int charIndex = 0;
                //foreach (FontCharInfo fontChar in this.cnFontMap)
                //{
                //    newFont[charIndex++] = fontChar.Char;
                //}
                //File.WriteAllLines(this.isComText ? cnTextFont : cnFileFont, newFont, Encoding.UTF8);

                return true;
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
                return false;
            }
        }

        /// <summary>
        /// 取得后面不带0的中文文本
        /// </summary>
        /// <returns></returns>
        private string GetCnTextWithoutZero()
        {
            string cnText = this.txtCnEdit.Text;

            if (cnText.EndsWith("^0^"))
            {
                int checkIndex = cnText.Length - 4;
                while (cnText.Substring(checkIndex, 3) == "^0^")
                {
                    checkIndex -= 3;
                }

                checkIndex += 3;

                return cnText.Substring(0, checkIndex);
            }
            else
            {
                return cnText;
            }
        }

        #endregion
    }
}