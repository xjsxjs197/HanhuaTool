using System;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Reflection;
using Hanhua.Common;

namespace Hanhua.TextEditTools.Bio3Edit
{
    /// <summary>
    /// 生化危机3文本编辑器
    /// </summary>
    public partial class Bio3TextEditor : BaseTextEditor
    {
        #region " 本地变量 "

        #region " 日文字库 "

        /// <summary>
        /// 生化危机3字库0x00--0xEA
        /// </summary>
        private string[] jpFontChars00Ea = { 
 "　", ".", "▷", "「", "」", "（", "）", "『", "』", "“", "”", "▿", "0", "1", "2", "3", "4", "5"
,"6", "7", "8", "9", "：", "、", "，", "▵", "！", "？", "$", "A", "B", "C", "D", "E", "F", "G"
,"H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y"
,"Z", "+", "╱", "-", "'", "一", "・", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k"
,"l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "あ", "い", "う"
,"え", "お", "か", "き", "く", "け", "こ", "さ", "し", "す", "せ", "そ", "た", "ち", "つ", "て", "と", "な"
,"に", "ぬ", "ね", "の", "は", "ひ", "ふ", "へ", "ほ", "ま", "み", "む", "め", "も", "や", "ゆ", "よ", "ら"
,"り", "る", "れ", "ろ", "わ", "を", "ん", "が", "ぎ", "ぐ", "げ", "ご", "ざ", "じ", "ず", "ぜ", "ぞ", "だ"
,"ぢ", "づ", "で", "ど", "ば", "び", "ぶ", "べ", "ぼ", "ぱ", "ぴ", "ぷ", "ぺ", "ぽ", "ぁ", "ぃ", "ぅ", "ぇ"
,"ぉ", "ゃ", "ゅ", "ょ", "っ", "ア", "イ", "ウ", "エ", "オ", "カ", "キ", "ク", "ケ", "コ", "サ", "シ", "ス"
,"セ", "ソ", "タ", "チ", "ツ", "テ", "ト", "ナ", "ニ", "ヌ", "ネ", "ノ", "ハ", "ヒ", "フ", "ヘ", "ホ", "マ"
,"ミ", "ム", "メ", "モ", "ヤ", "ユ", "ヨ", "ラ", "リ", "ル", "レ", "ロ", "ワ", "ヲ", "ン", "ガ", "ギ", "グ"
,"ゲ", "ゴ", "ザ", "ジ", "ズ", "ゼ", "ゾ", "ダ", "ヂ", "ヅ", "デ", "ド", "バ", "ビ", "ブ", "ベ", "ボ", "パ"
,"ピ", "プ", "ペ", "ポ", "ァ", "ィ", "ゥ", "ェ", "ォ", "ャ", "ュ", "ョ", "ッ", "ヴ", "-", "―", "L2", "R2"
,"＆", "…", "[", "]", "L1", "R1", "△", "○", "☓", "☐", "■", "☓", "上", "右", "下", "左", "使", "用"
,"通", "常", "火", "炎", "硫", "酸", "冷", "冻", "弹", "救", "急", "调", "查", "状", "况", "必", "要", "⁉"
        };

        /// <summary>
        /// 生化危机3字库0xEB
        /// </summary>
        private string[] jpFontCharsEb = { 
 "集", "地", "域", "图", "纳", "传", "票", "散", "荷", "社", "箱", "造", "積", "置", "窓", "靜", "務", "良"
,"区", "鸣", "谁", "助", "月", "昼", "逃", "惑", "人", "悲", "私", "駄", "目", "方", "気", "俺", "娘", "失"
,"毒", "嫌", "野", "郎", "食", "飢", "変", "関", "係", "立", "禁", "頑", "丈", "故", "板", "隙", "间", "固"
,"饮", "并", "腹", "绝", "午", "後", "半", "演", "街", "评", "判", "闭", "去", "拾", "求", "广", "表", "歳"
,"若", "返", "扉", "封", "锁", "路", "完", "阶", "段", "纸", "乱", "只", "期", "台", "衆", "话", "流", "洗"
,"太", "刀", "打", "訳", "前", "殺", "意", "味", "跡", "油", "刊", "情", "服", "着", "侵", "决", "内", "天"
,"女", "制", "御", "危", "险", "受", "待", "室", "家", "族", "引", "巡", "袭", "伤", "负", "見", "悪", "奥"
,"暗", "放", "风", "骚", "现", "队", "棚", "役", "格", "件", "信", "障", "络", "囲", "存", "至", "男", "始"
,"脅", "威", "少", "济", "邪", "魔", "别", "亲", "友", "類", "壳", "推", "移", "会", "案", "激", "退", "高"
,"项", "紧", "态", "收", "证", "番", "号", "替", "九", "勇", "‥", "逆", "滅", "選", "択", "愚", "払", "許"
,"遅", "運", "‥", "过", "级", "确", "认", "除", "异", "降", "讨", "缶", "他", "当", "顔", "贯", "暖", "炉"
,"末", "血", "坏", "瞔"
        };

        /// <summary>
        /// 生化危机3字库0xED--0xEE
        /// </summary>
        private string[] jpFontCharsEdEe = { 
 "古", "机", "何", "生", "外", "簡", "単", "傭", "兵", "強", "化", "版", "改", "合", "四", "角", "没", "S"
,"T", "A", "R", "添", "加", "剂", "混", "鉄", "有", "緑", "宝", "石", "青", "琥", "珀", "玉", "黒", "曜"
,"水", "晶", "乾", "電", "池", "市", "長", "本", "羅", "針", "盤", "六", "無", "限", "処", "理", "付", "紋"
,"章", "入", "公", "園", "鍵", "南", "京", "錠", "廃", "工", "場", "残", "遺", "言", "日", "記", "営", "業"
,"所", "報", "告", "書", "新", "聞", "者", "手", "帳", "死", "士", "指", "示", "美", "術", "品", "絵", "監"
,"視", "員", "辞", "令", "病", "院", "医", "管", "警", "備", "熱", "練", "道", "整", "志", "察", "署", "裏"
,"口", "切", "文", "字", "一", "般", "墓", "小", "屋", "取", "以", "組", "事", "出", "来", "今", "違", "込"
,"不", "発", "消", "倉", "預", "十", "分", "数", "作", "成", "愛", "軽", "量", "多", "最", "型", "銃", "扱"
,"易", "製", "大", "反", "動", "押", "開", "試", "様", "々", "旧", "式", "力", "武", "器", "装", "部", "門"
,"特", "殊", "効", "竸", "技", "精", "度", "軍", "安", "定", "射", "撃", "連", "毎", "秒", "続", "機", "構"
,"持", "近", "接", "戦", "填", "自", "追", "尾", "能", "炸", "裂", "命", "中", "時", "破", "片", "標", "準"
,"的", "頭", "対", "物", "濃", "極", "低", "温", "液", "室", "素", "体", "薬", "全", "回", "復", "先", "端"
,"形", "写", "真", "起", "重", "容", "源", "性", "械", "挟", "具", "幅", "節", "配", "栓", "携", "帯", "患"
,"診", "結", "果", "解", "説", "師", "声", "録", "音", "側", "保", "库", "。", "主", "離", "操", "神", "歯"
,"車", "像", "知", "惠", "識", "金", "属", "培", "養", "材", "料", "感", "染", "種", "木", "詰", "首", "飾"
,"柄", "施", "設", "応", "基", "礎", "二", "三", "排", "質", "検", "値", "計", "民", "正", "輪", "刻", "札"
        };

        /// <summary>
        /// 生化危机3字库0xEF--0xF0
        /// </summary>
        private string[] jpFontCharsEfF0 = { 
 "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
,"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
,"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
,"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
,"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
,"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
,"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
,"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
,"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
,"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
,"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
,"", "", "", "", "", "", "", "", "編", "山", "之", "内", "秘", "関", "明", "方", "法", "去"
,"九", "月", "人", "立", "勇", "気", "‥", "逆", "減", "愚", "払", "許", "遅", "運", "流", "誰", "‥", "過"
,"私", "同", "毒", "治", "与", "未", "号", "彫", "護", "身", "除", "可", "居", "駐", "療", "赤", "買", "賞"
,"？", "選", "択", "後", "向", "既", "", "", "", "", "高", "比", "引", "適", "断", "色", "輝", "専", "間", "銀", "希", "望"
,"室", "資", "薬", "米", "隊", "現", "在", "欠", "番", "掛", "棒", "台", "坑", "横", "穴", "制", "丸", "仕"
,"涂", "光", "行", "空", "莢", "失", "敗", "務", "室", "路", "地", "置", "暗", "商", "店", "街", "塔", "礼"
,"拝", "斎", "休", "憩", "塗", "光", "行", "空", "莢", "砲", "店", "責", "任", "焼", "却", "官", "社", "階"
        };

        /// <summary>
        /// 生化危机3字库3
        /// </summary>
        private string[] jpFontCharsEb2 = { 
 "駐", "利", "規", "則", "頑", "丈", "故", "変", "停", "郵", "便", "集", "閉", "供", "給", "足", "建", "築"
,"程", "抜", "身", "明", "航", "海", "旅", "人", "越", "再", "始", "路", "固", "話", "隊", "投", "由", "助"
,"態", "俺", "饲", "犬", "達", "雇", "叩", "和", "間", "任", "務", "当", "信", "気", "流", "難", "立", "風"
,"孔", "止", "置", "独", "占", "山", "地", "猟", "奇", "件", "謎", "送", "目", "考", "台", "丸", "税", "支"
,"払", "街", "貴", "方", "政", "周", "年", "念", "完", "碑", "非", "昇", "降", "茶", "犠", "牲", "寝", "彼"
,"八", "勘", "腹", "企", "関", "私", "協", "障", "運", "転", "席", "注", "意", "駅", "案", "内", "断", "線"
,"損", "底", "仲", "会", "娘", "延", "普", "奥", "夫", "落", "着", "寄", "女", "危", "険", "曹", "我", "点"
,"塔", "移", "歩", "番", "問", "題", "災", "隔", "各", "帰", "楽", "辺", "良", "確", "認", "図", "収", "乗"
,"友", "仕", "後", "甲", "斐", "負", "傷", "勢", "眠", "群", "遠", "吠", "嗅", "少", "存", "子", "洗", "浄"
,"並", "雑", "然", "油", "穴", "折", "殺", "耳", "鳴", "拾", "資", "散", "乱", "窓", "細", "見", "失", "酔"
,"嬢", "派", "遣", "策", "奴", "飛", "隠", "尻", "慢", "野", "郎", "腫", "食", "味", "防", "可", "燃", "引"
,"面", "伍", "君", "逃", "堂", "焼", "早", "胸", "元", "脱", "圧", "老", "朽", "漏", "格", "壊", "影", "響"
,"前", "高", "域", "室", "読", "段", "階", "慟", "両", "赤", "苦", "痛", "顔", "等", "学", "与", "未", "充"
        };

        /// <summary>
        /// 生化危机3字库4
        /// </summary>
        private string[] jpFontCharsEb3 = { 
 "祭", "壇", "間", "気", "丈", "夫", "服", "怪", "彼", "帰", "男", "目", "野", "郎", "俺", "達", "観", "敵"
,"役", "立", "悪", "人", "心", "後", "流", "信", "待", "伏", "私", "身", "恐", "奴", "詳", "許", "街", "還"
,"予", "休", "収", "画", "決", "選", "暖", "炉", "尽", "変", "散", "互", "仕", "方", "苦", "表", "情", "毒"
,"冒", "受", "皿", "巫", "女", "乗", "過", "去", "現", "在", "未", "家", "積", "雑", "然", "塔", "鐘", "扉"
,"前", "置", "侵", "防", "娘", "仲", "任", "務", "落", "床", "照", "明", "銅", "板", "穴", "次", "演", "奏"
,"年", "寄", "贈", "頼", "覚", "眠", "月", "夜", "雨", "響", "逆", "礼", "拝", "堂", "痛", "怖", "薄", "弱"
,"白", "旗", "念", "訴", "戻", "旋", "律", "狂", "荒", "段", "崩", "柱", "壊", "早", "治"
        };

        /// <summary>
        /// 生化危机3字库5
        /// </summary>
        private string[] jpFontCharsEb4 = { 
 "完", "暗", "見", "私", "立", "間", "随", "受", "壊", "頑", "丈", "降", "話", "健", "康", "険", "案", "内"
,"守", "変", "非", "灯", "閉", "荒", "跡", "飲", "催", "認", "移", "止", "点", "滴", "注", "意", "閲", "覧"
,"資", "山", "地", "目", "横", "紙", "詳", "得", "達", "漬", "関", "掲", "学", "籍", "並", "台", "療", "置"
,"階", "掛", "埋", "穏", "傷", "負", "奴", "前", "仏", "頂", "面", "銀", "髪", "野", "郎", "同", "俺", "細"
,"菌", "系", "複", "雑", "怪", "供", "給", "替", "槽", "Ⅰ", "Ⅱ", "Ⅲ", "戻", "制", "高", "顕", "微", "鏡"
,"胞", "協", "殺", "禁", "奥", "子", "眠", "家", "族", "暖", "炉", "明", "掃", "除", "酒", "転", "助", "気"
,"差", "支", "我", "雇", "存", "在", "群", "隊", "収", "集", "減", "信", "図", "印", "脱", "突", "抜", "飛"
,"橋", "危", "柵", "登", "爆", "早", "噴", "途", "御", "薪", "終", "了", "件", "撤", "繰", "返", "崩", "月"
,"参", "照", "棚", "燃", "風", "吹", "総"
        };

        /// <summary>
        /// 生化危机3字库6
        /// </summary>
        private string[] jpFontCharsEb5 = { 
 "週", "誌", "簿", "雑", "然", "積", "夜", "明", "街", "跡", "吹", "飛", "当", "情", "間", "置", "制", "御"
,"建", "供", "給", "替", "読", "頑", "丈", "扉", "閉", "働", "除", "確", "認", "異", "臭", "野", "郎", "気"
,"満", "寄", "方", "良", "止", "駆", "際", "退", "去", "徹", "底", "人", "逃", "他", "達", "交", "渉", "次"
,"第", "私", "関", "係", "君", "雇", "嫌", "亡", "酬", "額", "懐", "暖", "爪", "壁", "線", "途", "熱", "子"
,"剖", "放", "奇", "妙", "植", "散", "乱", "身", "薄", "汚", "白", "衣", "内", "始", "危", "険", "待", "避"
,"類", "話", "受", "信", "隊", "静", "横", "巨", "焼", "見", "済", "探", "誰", "非", "激", "息", "洋", "館"
,"実", "験", "走", "波", "注", "意", "経", "表", "紫", "照", "濁", "棄", "遮", "維", "忘", "距", "役", "立"
,"絶", "延", "攻", "超", "速", "脱", "悪", "名", "借", "別", "暵", "終", "覚", "悟", "泱", "由", "俺", "瞬"
,"収", "返", "前", "呼", "位", "絡", "法", "怪", "骸", "爆", "壊", "刺", "食", "荒", "完", "投", "輸", "送"
,"足", "荷", "過", "剰", "厳", "態", "再", "突", "抜", "降", "橋", "変", "差", "固", "悲", "伝", "致", "存"
,"統", "領", "邦", "会", "議", "減", "菌", "移", "件", "犠", "牲", "万", "予", "想", "落", "年", "代"
        };

        /// <summary>
        /// 生化危机3字库7
        /// </summary>
        private string[] jpFontCharsEb6 = { 
 "楽", "酬", "受", "君", "由"
        };

        /// <summary>
        /// 生化危机3字库8
        /// </summary>
        private string[] jpFontCharsEb7 = { 
 "諸", "君", "延", "目", "前", "敵", "減", "埋", "爆", "達", "次", "第", "我", "楽", "倒"
        };

        #endregion

        #region " 中文字库 "

        /// <summary>
        /// 生化危机3字库0x00--0xEA
        /// </summary>
        private string[] cnFontChars00Ea = { 
  "　", ".", "▷", "「", "」", "（", "）", "『", "』", "“", "”", "▿", "0", "1", "2", "3", "4", "5",
"6", "7", "8", "9", "：", "、", "，", "▵", "！", "？", "$", "A", "B", "C", "D", "E", "F", "G",
"H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y",
"Z", "+", "╱", "-", "'", "―", "・", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k",
"l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "一", "上", "下",
"个", "中", "为", "么", "义", "之", "九", "也", "买", "亡", "人", "什", "从", "他", "们", "价", "会", "但",
"作", "出", "切", "制", "动", "卡", "原", "反", "取", "古", "吉", "后", "哈", "回", "天", "奏", "始", "存",
"它", "安", "定", "导", "将", "尔", "尼", "己", "市", "布", "开", "弱", "必", "我", "所", "抗", "拉", "按",
"换", "控", "操", "敢", "数", "文", "斯", "无", "且", "时", "是", "普", "最", "月", "有", "本", "机", "束",
"档", "次", "止", "正", "每", "没", "法", "洛", "浣", "消", "演", "灭", "熊", "的", "确", "符", "第", "米",
"红", "终", "结", "置", "翻", "而", "胆", "能", "自", "致", "蓝", "行", "被", "评", "谅", "负", "责", "购",
"转", "轮", "软", "返", "这", "退", "逃", "通", "都", "重", "量", "钮", "间", "阻", "除", "雷", "非", "页",
"须", "记", "秒", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "…",
"ピ", "プ", "ペ", "ポ", "ァ", "ィ", "ゥ", "ェ", "ォ", "ャ", "ュ", "ョ", "ッ", "ヴ", "-", "―", "L2", "R2",
"＆", "…", "[", "]", "L1", "R1", "△", "○", "☓", "☐", "■", "☓", "Ｓ", "Ｔ", "Ａ", "Ｒ", "使", "用",
"通", "常", "火", "炎", "硫", "酸", "冷", "冻", "弹", "救", "急", "调", "查", "状", "况", "必", "要", "⁉"
        };

        /// <summary>
        /// 生化危机3字库0xEB
        /// </summary>
        private string[] cnFontCharsEb = { 
"丢", "丧", "临", "丹", "乎", "乐", "乱", "产", "任", "何", "侧", "储", "儿", "克", "冠", "决", "几", "刊",
"则", "刚", "别", "到", "勒", "半", "占", "印", "又", "叉", "受", "变", "另", "吃", "各", "吧", "听", "呢",
"味", "咬", "售", "啊", "噬", "围", "固", "圣", "圾", "坏", "坚", "垃", "域", "堆", "墨", "壁", "太", "奖",
"女", "她", "妙", "季", "宁", "实", "宣", "尸", "屏", "岁", "巡", "巴", "幕", "干", "年", "并", "广", "待",
"忙", "快", "怪", "情", "惜", "想", "憾", "户", "找", "招", "拥", "持", "挡", "援", "摆", "撕", "攻", "故",
"整", "早", "昼", "晚", "曾", "杀", "杯", "杰", "板", "框", "桌", "欢", "此", "死", "汽", "泡", "活", "侵",
"涌", "清", "滚", "漆", "灵", "炉", "热", "牢", "狗", "玩", "珠", "瑞", "瓶", "痕", "登", "盒", "省", "看",
"碎", "禁", "离", "移", "穿", "窗", "等", "箱", "纸", "练", "绑", "绳", "美", "聘", "肠", "胁", "脏", "脑",
"节", "菲", "落", "蒸", "虐", "衣", "觉", "训", "议", "货", "贯", "贴", "资", "赛", "赶", "起", "路", "迎",
"远", "迟", "迦", "迹", "递", "速", "遥", "那", "邮", "酒", "钉", "销", "闭", "闸", "队", "随", "障", "集",
"零", "静", "颗", "风", "食", "饮", "饿", "馆", "魂", "齐", "龙", "慢", "", "", "", "", "", ""
        };

        /// <summary>
        /// 生化危机3字库0xED--0xEE
        /// </summary>
        private string[] cnFontCharsEdEe = { 
 "不", "与", "专", "东", "丝", "严", "主", "书", "了", "予", "事", "于", "些", "今", "介", "仓", "令", "以",
"件", "休", "传", "似", "低", "住", "体", "余", "你", "佣", "使", "便", "保", "信", "候", "值", "做", "停",
"像", "光", "入", "全", "公", "六", "关", "兵", "其", "具", "养", "内", "册", "再", "写", "军", "冲", "况",
"冷", "冻", "准", "减", "击", "刀", "分", "利", "刻", "剂", "前", "剩", "力", "办", "功", "加", "包", "化",
"匙", "区", "医", "单", "卖", "南", "卫", "危", "即", "卸", "厂", "厅", "压", "去", "发", "口", "只", "可",
"台", "号", "司", "合", "同", "吗", "否", "启", "呈", "告", "员", "周", "命", "和", "品", "哪", "商", "喷",
"器", "噩", "四", "园", "图", "圆", "圈", "在", "地", "场", "型", "城", "培", "堂", "塑", "填", "墓", "士",
"处", "备", "复", "外", "多", "大", "失", "头", "夹", "夺", "好", "如", "姆", "威", "子", "孔", "字", "宅",
"完", "宝", "客", "室", "家", "容", "密", "察", "对", "封", "射", "小", "少", "就", "局", "属", "山", "嵌",
"工", "左", "已", "巷", "帕", "带", "常", "库", "应", "店", "废", "度", "座", "庭", "异", "弃", "式", "张",
"弹", "强", "当", "录", "形", "径", "很", "得", "德", "徽", "忆", "忘", "态", "急", "性", "恐", "恢", "息",
"患", "惧", "意", "感", "愿", "戏", "成", "或", "战", "截", "房", "手", "才", "打", "扔", "托", "扣", "扳",
"技", "把", "报", "抵", "押", "拆", "拔", "拜", "择", "拿", "挂", "指", "据", "掉", "掌", "排", "接", "提",
"插", "握", "携", "支", "收", "改", "放", "效", "救", "散", "料", "断", "新", "方", "旁", "旋", "日", "旧",
"明", "晶", "曜", "曲", "更", "服", "木", "未", "术", "材", "条", "来", "极", "构", "林", "果", "枪", "架",
"柄", "某", "染", "柜", "查", "标", "栓", "样", "格", "案", "梦", "梯", "械", "检", "棒", "植", "楼", "榴"
        };

        /// <summary>
        /// 生化危机3字库0xEF--0xF0
        /// </summary>
        private string[] cnFontCharsEfF0 = { 
 "模", "撬", "欧", "步", "武", "殊", "毒", "比", "毫", "氮", "水", "池", "油", "治", "洗", "浓", "涂", "液",
"混", "添", "温", "游", "源", "满", "火", "炎", "炮", "炸", "点", "烧", "然", "照", "燃", "爆", "爬", "片",
"版", "牌", "物", "特", "状", "现", "珀", "珍", "班", "理", "琥", "瓦", "生", "用", "由", "电", "疗", "疫",
"病", "白", "皮", "监", "盖", "盘", "目", "相", "真", "着", "瞄", "知", "短", "石", "码", "硫", "硬", "碟",
"磁", "示", "礼", "神", "种", "秘", "空", "突", "竞", "章", "端", "签", "简", "管", "箭", "类", "粉", "系",
"素", "紧", "级", "线", "组", "经", "络", "绝", "统", "继", "续", "绿", "缆", "老", "者", "联", "肯", "胶",
"膛", "色", "艺", "芒", "苗", "草", "药", "获", "藏", "虽", "螺", "街", "裁", "装", "西", "要", "覆", "见",
"视", "角", "解", "警", "计", "认", "让", "记", "许", "设", "识", "诊", "试", "话", "误", "说", "请", "读",
"调", "贝", "败", "质", "贵", "赐", "走", "足", "踪", "身", "车", "轴", "轻", "载", "输", "边", "达", "过",
"近", "还", "进", "连", "追", "适", "选", "逊", "造", "道", "遗", "部", "配", "酸", "采", "里", "金", "针",
"钟", "钢", "钥", "钩", "钱", "铁", "铜", "银", "链", "锁", "锈", "错", "键", "镜", "长", "门", "闻", "防",
"附", "限", "院", "险", "隐", "雕", "雾", "需", "青", "面", "音", "项", "饰", "首", "马", "高", "麦", "麻",
"黄", "黑", "齿", "表", "笔", "", "", "", "", "", "", "", "", "", "", "", "", "",
"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "，", "。"
        };

        /// <summary>
        /// 生化危机3字库3
        /// </summary>
        private string[] cnFontCharsEb2 = { 
 "且", "世", "丧", "举", "乎", "乱", "亚", "任", "伍", "伙", "伟", "伤", "伦", "位", "何", "供", "偷", "充",
"先", "克", "兴", "决", "几", "凹", "划", "列", "刚", "别", "到", "刹", "务", "助", "势", "升", "却", "卷",
"厚", "友", "受", "变", "叼", "各", "向", "吧", "听", "呀", "呢", "味", "哇", "响", "哎", "哼", "唉", "唔",
"啃", "啊", "喂", "嗯", "嘿", "噢", "因", "围", "固", "坏", "坚", "域", "堵", "境", "墨", "声", "太", "奉",
"女", "她", "姐", "学", "孩", "官", "害", "寻", "尉", "尝", "尸", "巨", "差", "帮", "干", "幸", "建", "弄",
"往", "心", "快", "怎", "怪", "恩", "悬", "情", "想", "慧", "憾", "懂", "找", "抱", "拾", "持", "挖", "挡",
"损", "揭", "摊", "故", "整", "施", "映", "智", "朵", "杀", "杂", "松", "根", "棍", "槽", "歉", "此", "死",
"民", "气", "汽", "沃", "泄", "注", "活", "派", "漏", "灯", "灵", "灾", "烂", "狂", "狗", "独", "狭", "猪",
"献", "玩", "环", "疑", "疯", "疼", "痛", "直", "盾", "看", "破", "离", "移", "程", "税", "穿", "窄", "窖",
"窗", "等", "筑", "答", "策", "糕", "糟", "约", "纳", "给", "美", "考", "耳", "聋", "胸", "良", "苟", "苦",
"英", "菲", "营", "落", "虑", "表", "观", "规", "觉", "证", "诉", "该", "谁", "起", "跑", "跟", "路", "跳",
"躲", "辆", "迈", "运", "远", "迷", "送", "遣", "那", "闭", "问", "闸", "队", "阿", "降", "随", "隔", "障",
"雄", "雇", "露", "静", "靠", "颈", "颗", "题", "风", "食", "餐", "驶", "驾", "骗", "板", "", "", ""
        };

        /// <summary>
        /// 生化危机3字库4
        /// </summary>
        private string[] cnFontCharsEb3 = { 
 "两", "丧", "乎", "乐", "乱", "交", "代", "任", "伙", "伴", "何", "侵", "倒", "儿", "决", "划", "刚", "初",
"别", "到", "副", "厢", "又", "变", "吧", "呃", "呢", "味", "呵", "呼", "啊", "嗯", "嘿", "团", "坏", "坚",
"坛", "堵", "塌", "壁", "声", "夜", "太", "女", "尸", "尽", "屉", "巫", "干", "年", "弄", "律", "心", "忧",
"快", "怀", "怎", "怪", "恶", "情", "惊", "想", "找", "护", "抱", "抽", "担", "持", "挡", "振", "捉", "推",
"敌", "斗", "晕", "晚", "杀", "板", "柱", "根", "楚", "歉", "死", "气", "活", "清", "渗", "灯", "炉", "犹",
"玩", "画", "略", "疑", "疼", "痛", "盒", "直", "看", "祭", "离", "移", "等", "答", "算", "精", "糕", "糟",
"纸", "给", "美", "苦", "荣", "营", "落", "表", "觉", "论", "证", "该", "豫", "赶", "起", "路", "运", "迷",
"透", "那", "醒", "镇", "问", "难", "雨", "顾", "颗", "题", "", "", "", "", "", "", "", ""
        };

        /// <summary>
        /// 生化危机3字库5
        /// </summary>
        private string[] cnFontCharsEb4 = { 
 "Ⅰ", "Ⅱ", "Ⅲ", "丢", "丧", "乎", "亚", "仔", "仪", "任", "伍", "伙", "何", "供", "修", "储", "充", "克",
"兹", "净", "创", "别", "到", "务", "助", "午", "历", "变", "叛", "吃", "各", "名", "吧", "听", "吹", "咖",
"啃", "啊", "啡", "因", "圾", "坏", "垃", "墙", "壁", "声", "太", "奇", "奥", "学", "实", "尸", "尽", "层",
"居", "屠", "帘", "帮", "干", "平", "底", "建", "弗", "彻", "徒", "徽", "快", "怎", "怪", "总", "恩", "您",
"情", "惊", "想", "慎", "持", "损", "推", "搜", "摆", "故", "施", "显", "暗", "杀", "杂", "杯", "杰", "板",
"析", "枯", "柴", "栅", "栏", "核", "桶", "槽", "此", "死", "毁", "毕", "永", "泉", "泡", "注", "洁", "活",
"派", "测", "侵", "清", "灯", "炉", "爱", "牲", "牺", "爪", "瓶", "疑", "疯", "看", "眠", "禁", "福", "离",
"科", "窗", "立", "筑", "筒", "算", "箱", "精", "索", "纸", "细", "给", "维", "聚", "胞", "菌", "观", "詹",
"该", "谨", "资", "赶", "起", "远", "遇", "那", "酒", "闭", "闸", "队", "阵", "阿", "随", "隔", "集", "顾",
"颈", "风", "食", "丑", "八", "嗯", "易", "灰", "背", "验", "", "", "", "", "", "", "", ""
        };

        /// <summary>
        /// 生化危机3字库6
        /// </summary>
        private string[] cnFontCharsEb5 = { 
 "乎", "仔", "代", "位", "供", "侧", "充", "决", "凿", "划", "别", "到", "升", "印", "却", "又", "变", "叛",
"另", "叫", "右", "名", "向", "吧", "听", "呢", "味", "呵", "啊", "喂", "嗯", "固", "国", "域", "堵", "墙",
"墟", "壁", "太", "奖", "实", "尊", "尸", "巨", "干", "年", "序", "底", "引", "弥", "待", "徒", "心", "快",
"念", "怎", "怪", "恶", "情", "想", "憾", "扇", "执", "找", "抢", "护", "抱", "推", "掩", "摧", "撤", "撬",
"擎", "攻", "整", "显", "杀", "权", "板", "核", "槽", "欠", "歉", "死", "毁", "气", "污", "波", "注", "活",
"测", "渗", "滤", "漫", "焦", "爪", "牢", "犯", "梳", "直", "看", "离", "秒", "移", "程", "立", "笔", "等",
"答", "算", "紫", "细", "罪", "美", "脱", "臭", "舱", "落", "讨", "论", "证", "该", "谁", "谢", "账", "资",
"路", "跳", "运", "远", "送", "透", "逛", "速", "那", "闲", "阀", "降", "靠", "验", "黎", "", "", ""
        };

        /// <summary>
        /// 生化危机3字库7
        /// </summary>
        private string[] cnFontCharsEb6 = { 
 "太", "奖", "实", "彩", "晚", "精", "", "", "", "", "", "", "", "", "", ""
        };

        /// <summary>
        /// 生化危机3字库8
        /// </summary>
        private string[] cnFontCharsEb7 = { 
 "临", "则", "到", "因", "域", "持", "晚", "欢", "此", "段", "清", "祝", "程", "脑", "规", "迎", "运", "醒"
        };

        #endregion

        /// <summary>
        /// 记录当前使用字库
        /// </summary>
        private Dictionary<int, string[]> jpFontCharPage = new Dictionary<int,string[]>();

        /// <summary>
        /// 记录当前使用的中文字库
        /// </summary>
        private Dictionary<int, string[]> cnFontCharPage = new Dictionary<int,string[]>();

        /// <summary>
        /// Option字库
        /// </summary>
        private List<KeyValuePair<int, string>> jpOptionChars = new List<KeyValuePair<int, string>>();

        /// <summary>
        /// Option字库
        /// </summary>
        private List<KeyValuePair<int, string>> cnOptionChars = new List<KeyValuePair<int, string>>();

        /// <summary>
        /// 全局变量
        /// </summary>
        private int maxFindLen = 0;

        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        public Bio3TextEditor()
        {
            InitializeComponent();

            this.ResetHeight();

            this.gameName = "Bio3";
            //this.baseFolder = @"E:\Study\MySelfProject\Hanhua\TodoCn\Bio3";
            this.baseFolder = @"F:\game\iso\wii\生化危机3汉化";
            
            this.SetPsLoadStatus(false);

            // 初始化
            this.EditorInit();
        }

        #region " 事件 "

        /// <summary>
        /// 打包
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPatch_Click(object sender, EventArgs e)
        {
            try
            {
                //string temp = "以件休传以软";
                //List<byte> byTest = new List<byte>();
                //for (int i = 0; i < temp.Length; i++)
                //{
                //    byTest.AddRange(this.EncodeChar(temp.Substring(i, 1)));
                //}

                //StringBuilder sb = new StringBuilder();
                //for (int i = 0; i < byTest.Count; i++)
                //{
                //    sb.Append(byTest[i].ToString("x") + " ");
                //}
                //sb.ToString();

                string bio3Ngc = this.baseFolder + @"\";

                // 复制start.dol
                Directory.CreateDirectory(bio3Ngc + @"NgcBio3Patch\root\&&systemdata\");
                //byte[] startDol = File.ReadAllBytes(bio3Ngc + @"NgcBio3Cn\root\&&systemdata\Start.dol");
                //byte[] targetDol = new byte[2298368];
                //Array.Copy(startDol, 0, targetDol, 0, targetDol.Length);
                //File.WriteAllBytes(bio3Ngc + @"NgcBio3Patch\root\&&systemdata\Start.dol", targetDol);
                File.Copy(bio3Ngc + @"NgcBio3Cn\root\&&systemdata\Start.dol_Copy", bio3Ngc + @"NgcBio3Patch\root\&&systemdata\Start.dol", true);

                // 复制其他文件
                string[] copyFiles = File.ReadAllLines(bio3Ngc + @"NgcOtherFile.txt");
                for (int i = 0; i < copyFiles.Length; i++)
                {
                    string targetFile = bio3Ngc + @"NgcBio3Patch\" + copyFiles[i];
                    if (!Directory.Exists(Path.GetDirectoryName(targetFile)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(targetFile));
                    }
                    File.Copy(bio3Ngc + @"NgcBio3Cn\" + copyFiles[i], targetFile, true);
                }

                //// 复制data_aj文件
                //copyFiles = File.ReadAllLines(bio3Ngc + @"NgcBio3Jp\NgcTextAddrAj.txt");
                //Directory.CreateDirectory(bio3Ngc + @"NgcBio3Patch\root\bio19\data_aj\rdt\");
                //for (int i = 0; i < copyFiles.Length; i += 2)
                //{
                //    File.Copy(bio3Ngc + @"NgcBio3Cn\root\bio19\data_aj\rdt\" + copyFiles[i] + ".rdt", bio3Ngc + @"NgcBio3Patch\root\bio19\data_aj\rdt\" + copyFiles[i] + ".rdt", true);
                //}

                // 复制data_j文件
                copyFiles = File.ReadAllLines(bio3Ngc + @"NgcTextAddrj.txt");
                Directory.CreateDirectory(bio3Ngc + @"NgcBio3Patch\root\bio19\data_j\rdt\");
                for (int i = 0; i < copyFiles.Length; i += 2)
                {
                    File.Copy(bio3Ngc + @"NgcBio3Cn\root\bio19\data_j\rdt\" + copyFiles[i] + ".rdt", bio3Ngc + @"NgcBio3Patch\root\bio19\data_j\rdt\" + copyFiles[i] + ".rdt", true);
                }

                MessageBox.Show("打包完成！");
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message + "\n" + exp.StackTrace);
            }
        }

        /// <summary>
        /// 重新加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReLoad_Click(object sender, EventArgs e)
        {
            this.EditorInit();
        }

        /// <summary>
        /// 切换类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoNgc_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdoNgc.Checked)
            {
                this.SetNgcLoadStatus(true);

                this.SetPsLoadStatus(false);
            }
            else
            {
                this.SetNgcLoadStatus(false);

                this.SetPsLoadStatus(true);
            }
        }

        /// <summary>
        /// 切换Ngc取得Option
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkNgcOption_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkNgcOption.Checked)
            {
                this.chkNgcDol.Checked = false;
                this.chkNgcRdt.Checked = false;
            }
        }

        /// <summary>
        /// 切换Ngc取得Start.Dol
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkNgcDol_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkNgcDol.Checked)
            {
                this.chkNgcOption.Checked = false;
            }
        }

        /// <summary>
        /// 切换Ngc取得Rdt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkNgcRdt_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkNgcRdt.Checked)
            {
                this.chkNgcOption.Checked = false;
            }
        }

        /// <summary>
        /// CopyPs的文本到Ngc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopyFromPs_Click(object sender, EventArgs e)
        {
            this.Do(this.CopyPsToNgc);
            //this.Do(this.CheckPsCnJpDiffPos);
        }

        #endregion

        #region " 重写父类方法 "

        /// <summary>
        /// 读取字库信息
        /// </summary>
        protected override void ReadFontChar()
        {
            // 日文Option字库
            this.jpOptionChars.Clear();
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8260, "Ａ"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8261, "Ｂ"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8262, "Ｃ"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8263, "Ｄ"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8264, "Ｅ"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8265, "Ｆ"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8266, "Ｇ"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8268, "Ｉ"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x826a, "Ｋ"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x826b, "Ｌ"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x826c, "Ｍ"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x826d, "Ｎ"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x826e, "Ｏ"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x826f, "Ｐ"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8271, "Ｒ"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8272, "Ｓ"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8273, "Ｔ"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8274, "Ｕ"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8275, "Ｖ"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8276, "Ｗ"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8277, "Ｘ"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8278, "Ｙ"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x824f, "0"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8250, "1"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8251, "2"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8252, "3"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8253, "4"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8254, "5"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8255, "6"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8256, "7"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8257, "8"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8258, "9"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8144, "."));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8148, "?"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8267, "Ｈ"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8269, "Ｊ"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8144, "."));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8149, "!"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x815e, "/"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8146, ":"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x817b, "+"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x817c, "-"));
            this.jpOptionChars.Add(new KeyValuePair<int, string>(0x8197, " "));

            // 中文Option字库
            this.cnOptionChars.Clear();
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8260, "体"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8261, "关"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8262, "动"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8263, "单"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8264, "否"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8265, "启"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8266, "回"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8268, "开"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x826a, "按"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x826b, "声"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x826c, "是"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x826d, "景"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x826e, "画"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x826f, "立"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8271, "类"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8272, "置"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8273, "背"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8274, "自"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8275, "设"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8276, "返"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8277, "重"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8278, "键"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x824f, "0"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8250, "1"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8251, "2"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8252, "3"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8253, "4"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8254, "5"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8255, "6"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8256, "7"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8257, "8"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8258, "9"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8144, "面"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8148, "?"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8267, "型"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8269, "手"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8144, "."));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8149, "音"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x815e, "/"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8146, "震"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x817b, "+"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x817c, "-"));
            this.cnOptionChars.Add(new KeyValuePair<int, string>(0x8197, " "));


            // 读取日文字库
            this.ReadFontFile(this.jpFontCharPage, this.jpFontChars00Ea, this.jpFontCharsEb, this.jpFontCharsEdEe, this.jpFontCharsEfF0);

            // 读取中文字库
            this.ReadFontFile(this.cnFontCharPage, this.cnFontChars00Ea, this.cnFontCharsEb, this.cnFontCharsEdEe, this.cnFontCharsEfF0);
        }

        /// <summary>
        /// 读取需要汉化的文件
        /// </summary>
        protected override void LoadAllFiles()
        {
            base.LoadAllFiles();

            // 根据配置文件，取得所有汉化的文件
            List<FilePosInfo> allFiles = this.LoadFiles();
            if (allFiles.Count == 0)
            {
                MessageBox.Show("路径错误，没有找到需要Copy的文件！");
                return;
            }

            // 添加Ngc start.dol文件
            if (this.chkNgcDol.Checked || this.chkNgcOption.Checked)
            {
                string ngcStartDol = this.baseFolder + @"\NgcBio3Jp\root\&&systemdata\start.dol";
                allFiles.ForEach(p => this.AddFile(ngcStartDol, p));
            }

            // 添加Bin文件
            if (this.chkPsBin.Checked)
            {
                foreach (FilePosInfo fileInfo in allFiles)
                {
                    // 取得各个文件名
                    string fileName = Util.TrimFileNo(fileInfo.File);
                    string jpFile = this.baseFolder + @"\PsBio3Jp\CD_DATA\BIN\" + fileName + ".bin";
                    if (fileName.IndexOf("SLPS_023.00") >= 0)
                    {
                        jpFile = this.baseFolder + @"\PsBio3Jp\" + fileName;
                    }

                    if (File.Exists(jpFile))
                    {
                        this.AddFile(jpFile, fileInfo);
                    }
                }
            }

            // 开始循环所有的日文rdt文件
            if (this.chkPsArd.Checked || this.chkNgcRdt.Checked)
            {
                StringBuilder sb = new StringBuilder();
                string fileName = string.Empty;
                string jpFile = string.Empty;

                for (int i = 1; i <= 7; i++)
                {
                    List<FilePosInfo> copyFiles = allFiles.Where(p => p.File.IndexOf("r" + i) != -1).ToList();
                    foreach (FilePosInfo fileInfo in copyFiles)
                    {
                        // 取得各个文件名
                        fileName = Util.TrimFileNo(fileInfo.File);
                        if (this.chkPsArd.Checked)
                        {
                            jpFile = this.baseFolder + @"\PsBio3Jp\CD_DATA\STAGE" + i + @"\" + fileName + ".ard";
                        }
                        else
                        {
                            jpFile = this.baseFolder + @"\NgcBio3Jp\root\bio19\data_j\rdt\" + fileName + ".rdt";
                        }

                        if (File.Exists(jpFile))
                        {
                            this.AddFile(jpFile, fileInfo);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 选择的文件变更
        /// </summary>
        /// <param name="currentFileInfo">当前选择的文件</param>
        protected override void FileChanged(FilePosInfo currentFileInfo)
        {
            // 修改字库
            string index = currentFileInfo.File.Substring(currentFileInfo.File.Length - 7, 1);
            if ("1234567".IndexOf(index) >= 0)
            {
                this.ResetFontChar(Convert.ToInt32(index));
            }
            else
            {
                this.ResetFontChar(1);
            }

            if (currentFileInfo.File.EndsWith(".rdt", StringComparison.OrdinalIgnoreCase))
            {
                byte[] byData = File.ReadAllBytes(currentFileInfo.File);
                int startPos = ((byData[0x3f] << 24) | (byData[0x3e] << 16) | (byData[0x3d] << 8) | byData[0x3c]);
                int endPos = (byData[0x43] << 24) | (byData[0x42] << 16) | (byData[0x41] << 8) | byData[0x40];
                currentFileInfo.TextStart = startPos + (byData[startPos + 1] << 8 | byData[startPos]);
                currentFileInfo.TextEnd = endPos;
                currentFileInfo.EntryPos = startPos;
                currentFileInfo.TextCopyStart = startPos;

                // 取得最后一句
                if (endPos == 0)
                {
                    int textEndPos = startPos + (byData[currentFileInfo.TextStart - 1] << 8 | byData[currentFileInfo.TextStart - 2]);
                    endPos = byData.Length;
                    while (textEndPos < endPos)
                    {
                        if (byData[textEndPos] == 0xF7)
                        {
                            textEndPos++;
                            break;
                        }

                        if (byData[textEndPos] == 0xFE)
                        {
                            textEndPos += 2;
                            break;
                        }

                        textEndPos++;
                    }
                    currentFileInfo.TextEnd = textEndPos;
                }
            }
        }

        /// <summary>
        /// 开始解码文本
        /// </summary>
        /// <param name="currentFileInfo">当前选择的文件</param>
        /// <param name="isCnTxt">是否是中文</param>
        /// <returns>解码的文本</returns>
        protected override string DecodeText(FilePosInfo currentFileInfo, bool isCnTxt)
        {
            if (isCnTxt)
            {
                if (this.chkNgcOption.Checked)
                {
                    return this.LoadOptionText(File.ReadAllBytes(this.cnFile), this.cnOptionChars, currentFileInfo.TextStart, currentFileInfo.TextEnd);
                }
                else
                {
                    return this.DecodeText(File.ReadAllBytes(this.cnFile), currentFileInfo, this.cnFontCharPage);
                }
            }
            else
            {
                if (this.chkNgcOption.Checked)
                {
                    return this.LoadOptionText(File.ReadAllBytes(currentFileInfo.File), this.jpOptionChars, currentFileInfo.TextStart, currentFileInfo.TextEnd);
                }
                else
                {
                    return this.DecodeText(File.ReadAllBytes(currentFileInfo.File), currentFileInfo, this.jpFontCharPage);
                }
            }
        }

        /// <summary>
        /// 取得当前文字的编码
        /// </summary>
        /// <param name="currenChar">当前文字</param>
        /// <returns>当前文字的编码</returns>
        protected override byte[] EncodeChar(string currentChar)
        {
            if (this.chkNgcOption.Checked)
            {
                // 从Option字库中查找
                KeyValuePair<int, string> fontCharInfo = this.cnOptionChars.FirstOrDefault(p => p.Value.Equals(currentChar));
                if (fontCharInfo.Key != 0)
                {
                    byte highByte = (byte)((fontCharInfo.Key >> 8) & 0xFF);
                    byte lowByte = (byte)(fontCharInfo.Key & 0xFF);
                    return new byte[] { highByte, lowByte };
                }
            }
            else if (this.chkNgcDol.Checked || this.chkNgcRdt.Checked)
            {
                // 在字库中查找
                foreach (int fontPage in this.cnFontCharPage.Keys)
                {
                    string[] pageFonts = this.cnFontCharPage[fontPage];
                    for (int i = 0; i < pageFonts.Length; i++)
                    {
                        if (currentChar == pageFonts[i])
                        {
                            if (fontPage == 0)
                            {
                                return new byte[] { (byte)i };
                            }
                            else
                            {
                                return new byte[] { (byte)fontPage, (byte)i };
                            }
                        }
                    }
                }
            }
            else
            {
                throw new Exception("PsBio3不能被保存");
            }

            throw new Exception("未查询到相应的中文字符 : " + currentChar);
        }

        /// <summary>
        /// 重新设置带Entry信息的翻译后的数据
        /// </summary>
        /// <param name="currentFileInfo">当前选择的文件</param>
        /// <param name="byData">当前选择的文件的字节数据</param>
        /// <param name="cnBytes">翻译后的字节数据</param>
        /// <returns>带Entry信息的翻译后的数据</returns>
        protected override byte[] ResetCnDataWithEnrty(FilePosInfo currentFileInfo, byte[] byData, List<byte> cnBytes)
        {
            byte[] byCnData = null;
            if (currentFileInfo.File.EndsWith(".rdt", StringComparison.OrdinalIgnoreCase))
            {
                byCnData = new byte[currentFileInfo.TextEntrys.Count * 2 + cnBytes.Count];

                // 带Entry的文本，先修改后的各个Entry
                int entryStart = 0;
                for (int i = 0; i < currentFileInfo.TextEntrys.Count; i++)
                {
                    int entryPos = currentFileInfo.TextEntrys[i] + (currentFileInfo.TextEntrys.Count * 2);
                    byCnData[entryStart + i * 2] = (byte)(entryPos & 0xFF);
                    byCnData[entryStart + i * 2 + 1] = (byte)((entryPos >> 8) & 0xFF);
                }

                // 再保存文本数据
                Array.Copy(cnBytes.ToArray(), 0, byCnData, currentFileInfo.TextEntrys.Count * 2, cnBytes.Count);
            }
            else
            {
                // 带Entry的文本，先保存文本数据
                byCnData = new byte[currentFileInfo.TextEntrys.Count * 2 + cnBytes.Count];
                Array.Copy(cnBytes.ToArray(), 0, byCnData, 0, cnBytes.Count);

                // 再保存修改后的各个Entry
                int entryStart = cnBytes.Count;
                for (int i = 0; i < currentFileInfo.TextEntrys.Count; i++)
                {
                    int entryPos = currentFileInfo.TextEntrys[i];
                    byCnData[entryStart + i * 2] = (byte)((entryPos >> 8) & 0xFF);
                    byCnData[entryStart + i * 2 + 1] = (byte)(entryPos & 0xFF);
                }
            }

            return byCnData;
        }

        /// <summary>
        /// 检查输入的中文长度是否正确
        /// </summary>
        /// <param name="chkKeyWords">是否需要检查关键字</param>
        /// <returns>输入的中文长度是否正确</returns>
        protected override bool CheckCnText(bool chkKeyWords)
        {
            //return base.CheckCnText(false);
            return true;
        }

        /// <summary>
        /// 导出前的操作
        /// </summary>
        /// <returns>是否可以继续</returns>
        protected override bool BeforeExport()
        {
            string fileName = string.Empty;
            if (this.rdoNgc.Checked)
            {
                if (this.chkNgcOption.Checked)
                {
                    fileName = "Bio3NgcOption";
                }

                if (this.chkNgcDol.Checked)
                {
                    fileName = "Bio3NgcDol";
                }

                if (this.chkNgcRdt.Checked)
                {
                    if (string.IsNullOrEmpty(fileName))
                    {
                        fileName = "Bio3NgcRdt";
                    }
                    else
                    {
                        fileName += "Rdt";
                    }
                }
            }
            else
            { 
                if (this.chkPsBin.Checked)
                {
                    fileName = "Bio3PsBin";
                }

                if (this.chkPsArd.Checked)
                {
                    if (string.IsNullOrEmpty(fileName))
                    {
                        fileName = "Bio3PsArd";
                    }
                    else
                    {
                        fileName += "Ard";
                    }
                }
            }

            this.baseFile = Util.SetSaveDailog(this.gameName + "翻译后文件（*.xlsx）|*.xlsx|所有文件|*.*", this.baseFolder + @"\" + fileName + ".xlsx");
            if (string.IsNullOrEmpty(this.baseFile))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 导入前判断
        /// </summary>
        /// <returns></returns>
        protected override bool BeforeImport()
        {
            this.baseFile = Util.SetOpenDailog(this.gameName + "翻译后文件（*.xls,*.xlsx）|*.xls;*.xlsx|所有文件|*.*", this.baseFolder + @"\.xlsx");
            if (string.IsNullOrEmpty(this.baseFile))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 保存存前的处理
        /// </summary>
        /// <param name="byCnData">要保存的数据</param>
        /// <param name="fileInfo">当前文件信息</param>
        protected override void BeforeSave(byte[] byCnData, FilePosInfo fileInfo)
        {
            // 修改Opening Enrty信息
            if (fileInfo.TextEnd == 0x2051a4)
            {
                byte[] byCnDataCopy = new byte[byCnData.Length];
                Array.Copy(byCnData, 0, byCnDataCopy, 0, byCnDataCopy.Length);

                for (int i = 0; i < 8; i++)
                {
                    byCnDataCopy[0x22f480 + i] = byCnData[0x2051a4 + i];
                }

                byCnDataCopy[0x2051a4 + 0] = 0x80;
                byCnDataCopy[0x2051a4 + 1] = 0x0D;
                byCnDataCopy[0x2051a4 + 2] = 0x3F;
                byCnDataCopy[0x2051a4 + 3] = 0x8C;
                byCnDataCopy[0x2051a4 + 4] = 0x80;
                byCnDataCopy[0x2051a4 + 5] = 0x0D;
                byCnDataCopy[0x2051a4 + 6] = 0x3F;
                byCnDataCopy[0x2051a4 + 7] = 0xA0;
                byCnDataCopy[0x2051a4 + 8] = 0x80;
                byCnDataCopy[0x2051a4 + 9] = 0x0D;

                //string jpFileOld = fileInfo.File.Replace("start.dol", "start.dol_old");
                //byte[] byJpOld = File.ReadAllBytes(jpFileOld);
                //Array.Copy(byJpOld, 0x22f489, byCnDataCopy, 0x22f489, 0x50);

                string cnCopyFile = fileInfo.File.Replace("NgcBio3Jp", "NgcBio3Cn").Replace("start.dol", "start.dol_Copy");
                File.WriteAllBytes(cnCopyFile, byCnDataCopy);
            }
        }

        #endregion

        #region " 私有方法 "

        /// <summary>
        /// 检查Ps中、日文的差异位置
        /// </summary>
        private void CheckPsCnJpDiffPos()
        {
            // 取得需要Copy的文件
            List<FilePosInfo> needCopyFilesRdt = new List<FilePosInfo>();
            needCopyFilesRdt.AddRange(this.LoadFiles(this.baseFolder + @"\PsTextAddr.txt"));
            if (needCopyFilesRdt.Count == 0)
            {
                MessageBox.Show("路径错误，没有找到需要Copy的文件！");
                return;
            }

            // 显示进度条
            this.ResetProcessBar(needCopyFilesRdt.Count);

            List<string> newAddrInfo = new List<string>();

            // 开始循环所有的日文rdt文件
            for (int i = 1; i <= 7; i++)
            {
                List<FilePosInfo> copyFiles = needCopyFilesRdt.Where(p => p.File.IndexOf("r" + i) != -1).ToList();
                foreach (FilePosInfo fileInfo in copyFiles)
                {
                    newAddrInfo.Add(fileInfo.File);
                    int startPos = 0;
                    int endPos = 0;

                    // 取得各个文件名
                    string fileName = @"\" + Util.TrimFileNo(fileInfo.File);
                    string jpFile = this.baseFolder + @"\PsBio3Jp\CD_DATA\STAGE" + i + fileName + ".ard";
                    string cnFile = this.baseFolder + @"\PsBio3Cn\CD_DATA\STAGE" + i + fileName + ".ard";

                    if (File.Exists(jpFile)
                        && File.Exists(cnFile))
                    {
                        // 取得文本数据
                        byte[] byJpData = File.ReadAllBytes(jpFile);
                        byte[] byCnData = File.ReadAllBytes(cnFile);

                        for (int j = 0; j < byJpData.Length; j++)
                        {
                            if (byJpData[j] != byCnData[j])
                            {
                                startPos = j;
                                break;
                            }
                        }

                        for (int j = byJpData.Length - 1; j >= 0; j--)
                        {
                            if (byJpData[j] != byCnData[j])
                            {
                                endPos = j;
                                break;
                            }
                        }

                        newAddrInfo.Add(startPos.ToString("x") + " " + endPos.ToString("x"));
                    }

                    // 更新进度条
                    this.ProcessBarStep();
                }
            }

            // 隐藏进度条
            this.CloseProcessBar();

            File.WriteAllLines(this.baseFolder + @"\PsTextDiffAddr.txt", newAddrInfo.ToArray(), Encoding.UTF8);
        }

        /// <summary>
        /// 设置当前Ps区域状态
        /// </summary>
        /// <param name="isPs"></param>
        private void SetPsLoadStatus(bool isPs)
        {
            this.chkPsBin.Checked = isPs;
            this.chkPsBin.Enabled = isPs;

            this.chkPsArd.Checked = isPs;
            this.chkPsArd.Enabled = isPs;
        }

        /// <summary>
        /// 设置当前Ngc区域状态
        /// </summary>
        /// <param name="isNgc"></param>
        private void SetNgcLoadStatus(bool isNgc)
        {
            this.chkNgcDol.Checked = isNgc;
            this.chkNgcDol.Enabled = isNgc;

            this.chkNgcRdt.Checked = isNgc;
            this.chkNgcRdt.Enabled = isNgc;

            this.chkNgcOption.Checked = false;
            this.chkNgcOption.Enabled = isNgc;
        }

        /// <summary>
        /// 加载Option文本
        /// </summary>
        /// <param name="byData"></param>
        /// <param name="fontCharPage"></param>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <returns></returns>
        private string LoadOptionText(byte[] byData, List<KeyValuePair<int, string>> fontChars, int startPos, int endPos)
        { 
            StringBuilder sb = new StringBuilder();
            
            for (int i = startPos; i < endPos;)
            {
                if (byData[i] == 0x82 || byData[i] == 0x81)
                {
                    int key = (byData[i] << 8) | byData[i + 1];
                    if (key == 0x8140)
                    {
                        sb.Append("^81 40^");
                    }
                    else
                    {
                        KeyValuePair<int, string> fontCharInfo = fontChars.First(p => p.Key == key);
                        sb.Append(fontCharInfo.Value);
                    }
                    i += 2;
                }
                else
                {
                    sb.Append("^" + byData[i].ToString("x") + "^\n");
                    i++;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 重新设置字库
        /// </summary>
        /// <param name="index"></param>
        private void ResetFontChar(int index)
        {
            switch (index)
            {
                case 1:
                    this.jpFontCharPage[0xEB] = this.jpFontCharsEb;
                    this.cnFontCharPage[0xEB] = this.cnFontCharsEb;
                    break;

                case 2:
                    this.jpFontCharPage[0xEB] = this.jpFontCharsEb2;
                    this.cnFontCharPage[0xEB] = this.cnFontCharsEb2;
                    break;

                case 3:
                    this.jpFontCharPage[0xEB] = this.jpFontCharsEb3;
                    this.cnFontCharPage[0xEB] = this.cnFontCharsEb3;
                    break;

                case 4:
                    this.jpFontCharPage[0xEB] = this.jpFontCharsEb4;
                    this.cnFontCharPage[0xEB] = this.cnFontCharsEb4;
                    break;

                case 5:
                    this.jpFontCharPage[0xEB] = this.jpFontCharsEb5;
                    this.cnFontCharPage[0xEB] = this.cnFontCharsEb5;
                    break;

                case 6:
                    this.jpFontCharPage[0xEB] = this.jpFontCharsEb6;
                    this.cnFontCharPage[0xEB] = this.cnFontCharsEb6;
                    break;

                case 7:
                    this.jpFontCharPage[0xEB] = this.jpFontCharsEb7;
                    this.cnFontCharPage[0xEB] = this.cnFontCharsEb7;
                    break;
            }
        }

        /// <summary>
        /// 保存文本数据
        /// </summary>
        /// <param name="ngcFile"></param>
        /// <param name="byJpData"></param>
        private int getNgcTextStartPos(string ngcFile, byte[] byPsJpData)
        {
            if (File.Exists(ngcFile))
            {
                try
                {
                    // 根据Ps日文文本数据，查找Ngc中的文本数据
                    return this.GetTextStartPos(File.ReadAllBytes(ngcFile), byPsJpData);
                }
                catch
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 根据Ps文本数据，查找Ngc中的文本数据
        /// </summary>
        /// <param name="byData"></param>
        /// <param name="byJpData"></param>
        /// <returns></returns>
        private int GetTextStartPos(byte[] byData, byte[] byJpData)
        {
            // 二进制检索
            bool findedKey = true;
            int maxLen = byData.Length - byJpData.Length;
            this.maxFindLen = 0;

            for (int j = 0; j < maxLen; j++)
            {
                if (byData[j] == byJpData[0])
                {
                    findedKey = true;
                    for (int i = 1; i < byJpData.Length; i++)
                    {
                        if (byData[j + i] != byJpData[i])
                        {
                            findedKey = false;
                            this.maxFindLen = Math.Max(this.maxFindLen, i);
                            break;
                        }
                    }

                    if (findedKey)
                    {
                        return j;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// 取得文本数据
        /// </summary>
        /// <param name="jpFile"></param>
        /// <param name="cnFile"></param>
        /// <param name="addrInfo"></param>
        /// <param name="byJpData"></param>
        /// <param name="byCnData"></param>
        private bool GetTextData(string jpFile, string cnFile, string[] addrInfo, byte[] byJpData, byte[] byCnData)
        {
            FileStream fs = null;

            try
            {
                int startPos = Convert.ToInt32(addrInfo[0], 16);
                int endPos = Convert.ToInt32(addrInfo[1], 16);

                fs = new FileStream(jpFile, FileMode.Open);
                fs.Seek(startPos, SeekOrigin.Begin);
                fs.Read(byJpData, 0, byJpData.Length);
                fs.Close();

                fs = new FileStream(cnFile, FileMode.Open);
                fs.Seek(startPos, SeekOrigin.Begin);
                fs.Read(byCnData, 0, byCnData.Length);
                fs.Close();
            }
            catch
            {
                return false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }

            return true;
        }

        /// <summary>
        /// 根据配置文件，读入需要汉化的文件
        /// </summary>
        /// <returns></returns>
        private List<FilePosInfo> LoadFiles()
        {
            List<FilePosInfo> needCopyFiles = new List<FilePosInfo>();

            if (this.rdoNgc.Checked)
            {
                if (this.chkNgcOption.Checked)
                {
                    return this.LoadFiles(this.baseFolder + @"\NgcOption.txt");
                }

                if (this.chkNgcDol.Checked)
                {
                    needCopyFiles.AddRange(this.LoadFiles(this.baseFolder + @"\NgcDolAddr.txt"));
                }

                if (this.chkNgcRdt.Checked)
                {
                    needCopyFiles.AddRange(this.LoadFiles(this.baseFolder + @"\NgcTextAddrJ.txt"));
                }
            }
            else
            {
                if (this.chkPsBin.Checked)
                {
                    needCopyFiles.AddRange(this.LoadFiles(this.baseFolder + @"\PsBinAddr.txt"));
                }

                if (this.chkPsArd.Checked)
                {
                    needCopyFiles.AddRange(this.LoadFiles(this.baseFolder + @"\PsTextAddr.txt"));
                }
            }

            return needCopyFiles;
        }

        /// <summary>
        /// 开始解码
        /// </summary>
        /// <param name="byData"></param>
        /// <param name="startPos"></param>
        private string DecodeText(byte[] byData, FilePosInfo filePosInfo, Dictionary<int, string[]> fontCharPage)
        {
            if (filePosInfo.File.EndsWith(".rdt", StringComparison.OrdinalIgnoreCase))
            {
                int startPos = ((byData[0x3f] << 24) | (byData[0x3e] << 16) | (byData[0x3d] << 8) | byData[0x3c]);
                int endPos = (byData[0x43] << 24) | (byData[0x42] << 16) | (byData[0x41] << 8) | byData[0x40];
                List<int> entryList = new List<int>();
                int txtStartPos = startPos + (byData[startPos + 1] << 8 | byData[startPos]);

                for (int i = startPos; i < txtStartPos; i += 2)
                {
                    entryList.Add(startPos + (byData[i + 1] << 8 | byData[i]));
                }
                //entryList.Add(endPos);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < entryList.Count - 1; i++)
                {
                    sb.Append(this.DecodeText(byData, fontCharPage, entryList[i], entryList[i + 1])).Append("\n");
                }

                // 取得最后一句
                int textEndPos = startPos + (byData[currentFileInfo.TextStart - 1] << 8 | byData[currentFileInfo.TextStart - 2]);
                if (endPos == 0)
                {
                    endPos = byData.Length;
                }
                while (textEndPos < endPos)
                {
                    if (byData[textEndPos] == 0xFE)
                    {
                        textEndPos += 2;
                        break;
                    }

                    textEndPos++;
                }
                if (entryList.Count > 0)
                {
                    sb.Append(this.DecodeText(byData, fontCharPage, entryList[entryList.Count - 1], textEndPos)).Append("\n");
                }
                else 
                {
                    sb.Append(this.DecodeText(byData, fontCharPage, txtStartPos, textEndPos)).Append("\n");
                }

                return sb.ToString();
            }
            else if (filePosInfo.EntryPos > 0)
            {
                int entryStart = filePosInfo.TextEnd;
                int entryEnd = filePosInfo.EntryPos;
                List<int> entryList = new List<int>();
                for (int i = entryStart; i < entryEnd; i += 2)
                {
                    entryList.Add(filePosInfo.TextStart + Util.GetOffset(byData, i, i + 1));
                }

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < entryList.Count - 1; i++)
                {
                    sb.Append(this.DecodeText(byData, fontCharPage, entryList[i], entryList[i + 1])).Append("\n");
                }

                // 取得最后一句
                int textEndPos = filePosInfo.TextEnd;
                while (byData[textEndPos - 2] == 0)
                {
                    textEndPos--;
                }
                if (textEndPos > entryList[entryList.Count - 1]) 
                {
                    sb.Append(this.DecodeText(byData, fontCharPage, entryList[entryList.Count - 1], textEndPos)).Append("\n");
                }

                return sb.ToString();
            }
            else 
            {
                return this.DecodeText(byData, fontCharPage, filePosInfo.TextStart, filePosInfo.TextEnd) + "\n";
            }
        }

        /// <summary>
        /// 开始解码
        /// </summary>
        /// <param name="byData"></param>
        /// <param name="startPos"></param>
        private string DecodeText(byte[] byData, Dictionary<int, string[]> fontCharPage, int startPos, int endPos)
        {
            StringBuilder sb = new StringBuilder();
            int temp;
            for (int j = startPos; j < endPos; j++)
            {
                switch (byData[j])
                {
                    case 0xF3:
                        sb.Append("^f3 " + byData[j + 1].ToString("x") + "^");
                        j += 1;
                        continue;

                    case 0xF7:
                        sb.Append("^f7^");
                        continue;

                    case 0xF4:
                    case 0xF9: // 绿颜色
                        sb.Append("^" + byData[j].ToString("x") + " " + byData[j + 1].ToString("x") + "^");
                        j += 1;
                        continue;

                    case 0xFA:
                        if (byData[j + 2] == 0xFC)
                        {
                            sb.Append("^fa " + byData[j + 1].ToString("x") + " fc^");
                            j += 2;
                        }
                        else if (byData[j + 1] == 0x01)
                        {
                            sb.Append("^fa 01 " + byData[j + 2].ToString("x") + " " + byData[j + 3].ToString("x") + "^");
                            j += 3;
                        }
                        else if (byData[j + 1] == 0x02)
                        {
                            if (byData[j + 2] == 0xFE || byData[j + 2] == 0xFD)
                            {
                                sb.Append("^fa 02 " + byData[j + 2].ToString("x") + " " + byData[j + 3].ToString("x") + "^");
                                j += 3;
                            }
                            else
                            {
                                sb.Append("^fa 02^");
                                j += 1;
                            }
                        }
                        else
                        {
                            sb.Append("^fa^");
                        }
                        continue;

                    case 0xFB:
                        //if (byData[j + 2] == 0xFE)
                        //{
                        //    sb.Append("^fb " + byData[j + 1].ToString("x") + "^");
                        //    j += 2;
                        //}
                        //else
                        {
                            sb.Append("^fb " + byData[j + 1].ToString("x") + "^");
                            j += 1;
                        }
                        continue;

                    case 0xFD:
                        sb.Append("^fd " + byData[j + 1].ToString("x") + "^");
                        j += 1;
                        continue;

                    case 0xFE:
                        if (byData[j + 1] == 0)
                        {
                            sb.Append("^fe 00^");
                            j += 1;
                        }
                        else if ((byData[j + 1] < 90 || (byData[j + 1] > 133 && byData[j + 1] < 234))
                            || (byData[j + 2] == 0 || byData[j + 2] == 0xfa))
                        {
                            sb.Append("^fe " + byData[j + 1].ToString("x") + "^");
                            j += 1;
                        }
                        else
                        {
                            sb.Append("^fe^");
                        }
                        continue;

                    case 0:
                        sb.Append("　");
                        continue;
                }



                temp = byData[j];

                if (fontCharPage.ContainsKey(temp))
                {
                    if (!(temp == 0xEA || temp == 0xEB || temp == 0xED || temp == 0xEE || temp == 0xEF || temp == 0xF0))
                    {
                        sb.Append("^" + temp.ToString("x") + " " + byData[j + 1].ToString("x") + "^");
                        j++;
                        continue;
                    }

                    string[] pageChars = fontCharPage[temp];
                    if (byData[j + 1] < pageChars.Length)
                    {
                        sb.Append(pageChars[byData[j + 1]]);
                        j++;
                    }
                    else
                    {
                        sb.Append("^" + temp.ToString("x") + " " + byData[j + 1].ToString("x") + "^");
                        j++;
                    }
                }
                else
                {
                    string[] pageChars = fontCharPage[0];
                    if (temp < pageChars.Length)
                    {
                        sb.Append(pageChars[temp]);
                    }
                    else
                    {
                        sb.Append("^" + temp.ToString("x") + "^");
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 读取字库信息
        /// </summary>
        private void ReadFontFile(Dictionary<int, string[]> fontCharPage, string[] chars00Ea, string[] charsEb, string[] charsEdEe, string[] charsEfF0)
        {
            fontCharPage.Clear();

            try
            {
                string[] page = new string[234];
                fontCharPage.Add(0, page);
                for (int i = 0; i < 234; i++)
                {
                    page[i] = chars00Ea[i];
                }

                page = new string[54];
                fontCharPage.Add(0xEA, page);
                for (int i = 0; i < 54; i++)
                {
                    page[i] = chars00Ea[234 + i];
                }

                fontCharPage.Add(0xEB, charsEb);

                page = new string[216 + 0x24];
                fontCharPage.Add(0xED, page);
                for (int i = 0; i < 216; i++)
                {
                    page[0x24 + i] = charsEdEe[i];
                }

                page = new string[72];
                fontCharPage.Add(0xEE, page);
                for (int i = 216; i < charsEdEe.Length; i++)
                {
                    page[i - 216] = charsEdEe[i];
                }

                page = new string[256];
                fontCharPage.Add(0xEF, page);
                for (int i = 0; i < 256; i++)
                {
                    page[i] = charsEfF0[i];
                }

                page = new string[charsEfF0.Length - 252];
                fontCharPage.Add(0xF0, page);
                for (int i = 252; i < charsEfF0.Length; i++)
                {
                    page[i - 252] = charsEfF0[i];
                }
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message + "\n" + me.StackTrace);
                return;
            }
        }

        /// <summary>
        /// CopyPs的文本到Ngc
        /// </summary>
        private void CopyPsToNgc()
        {
            // 取得需要Copy的文件
            List<FilePosInfo> needCopyFilesBin = new List<FilePosInfo>();
            List<FilePosInfo> needCopyFilesRdt = new List<FilePosInfo>();
            needCopyFilesBin.AddRange(this.LoadFiles(this.baseFolder + @"\PsBinAddr.txt"));
            needCopyFilesRdt.AddRange(this.LoadFiles(this.baseFolder + @"\PsTextDiffAddr.txt"));
            if (needCopyFilesBin.Count == 0 || needCopyFilesRdt.Count == 0)
            {
                MessageBox.Show("路径错误，没有找到需要Copy的文件！");
                return;
            }

            // 显示进度条
            this.ResetProcessBar(needCopyFilesBin.Count + needCopyFilesRdt.Count);

            StringBuilder saveFaileFiles = new StringBuilder();

            // 开始循环所有的日文bin文件
            for (int i = 0; i < needCopyFilesBin.Count; i++)
            {
                FilePosInfo fileInfo = needCopyFilesBin[i];

                // 取得各个文件名
                string fileName = @"\" + Util.TrimFileNo(fileInfo.File);
                string jpFile = this.baseFolder + @"\PsBio3Jp\CD_DATA\BIN" + fileName + ".bin";
                string cnFile = this.baseFolder + @"\PsBio3Cn\CD_DATA\BIN" + fileName + ".bin";
                string ngcCnFile = this.baseFolder + @"\NgcBio3Cn\root\&&systemdata\Start.dol";
                jpFile = jpFile.Replace(@"CD_DATA\BIN\SLPS_023.00.bin", @"SLPS_023.00");
                cnFile = cnFile.Replace(@"CD_DATA\BIN\SLPS_023.00.bin", @"SLPS_023.00");

                if (File.Exists(jpFile)
                    && File.Exists(cnFile)
                    && File.Exists(ngcCnFile))
                {
                    // 取得文本数据
                    byte[] byJpData = new byte[fileInfo.TextEnd - fileInfo.TextStart];
                    byte[] byCnData = new byte[byJpData.Length];
                    this.GetTextData(jpFile, cnFile, fileInfo.PosInfo, byJpData, byCnData);

                    // 保存文本数据
                    this.SaveTextData(ngcCnFile, byJpData, byCnData, saveFaileFiles, fileInfo.File);
                }

                // 更新进度条
                this.ProcessBarStep();
            }

            // 开始循环所有的日文rdt文件
            //for (int i = 1; i <= 7; i++)
            //{
            //    List<FilePosInfo> copyFiles = needCopyFilesRdt.Where(p => p.File.IndexOf("r" + i) != -1).ToList();
            //    foreach (FilePosInfo fileInfo in copyFiles)
            //    {
            //        // 取得各个文件名
            //        string fileName = @"\" + Util.TrimFileNo(fileInfo.File);
            //        string jpFile = this.baseFolder + @"\PsBio3Jp\CD_DATA\STAGE" + i + fileName + ".ard";
            //        string cnFile = this.baseFolder + @"\PsBio3Cn\CD_DATA\STAGE" + i + fileName + ".ard";
            //        string ngcFile1 = this.baseFolder + @"\NgcBio3Cn\root\bio19\data_j\rdt" + fileName + ".rdt";
            //        string ngcFile2 = this.baseFolder + @"\NgcBio3Cn\root\bio19\data_aj\rdt" + fileName + ".rdt";

            //        if (File.Exists(jpFile)
            //            && File.Exists(cnFile))
            //        {
            //            // 取得文本数据
            //            byte[] byJpData = new byte[fileInfo.TextEnd - fileInfo.TextStart + 1];
            //            byte[] byCnData = new byte[byJpData.Length];
            //            this.GetTextData(jpFile, cnFile, fileInfo.PosInfo, byJpData, byCnData);

            //            // 保存文本数据
            //            this.SaveTextData(ngcFile1, byJpData, byCnData, saveFaileFiles, fileInfo.File);
            //            this.SaveTextData(ngcFile2, byJpData, byCnData, saveFaileFiles, fileInfo.File);
            //        }

            //        // 更新进度条
            //        this.ProcessBarStep();
            //    }
            //}

            // 隐藏进度条
            this.CloseProcessBar();
        }

        /// <summary>
        /// 保存文本数据
        /// </summary>
        /// <param name="ngcFile"></param>
        /// <param name="byJpData"></param>
        /// <param name="byCnData"></param>
        /// <param name="saveFaileFiles"></param>
        private void SaveTextData(string ngcCnFile, byte[] byJpData, byte[] byCnData, StringBuilder saveFaileFiles, string sortName)
        {
            this.baseFile = ngcCnFile;
            if (!this.SaveTextData(ngcCnFile, byJpData, byCnData))
            {
                saveFaileFiles.Append(ngcCnFile + " : " + sortName).Append("\r\n");
            }
        }

        /// <summary>
        /// 保存文本数据
        /// </summary>
        /// <param name="ngcCnFile"></param>
        /// <param name="byJpData"></param>
        /// <param name="byCnData"></param>
        /// <returns></returns>
        private bool SaveTextData(string ngcCnFile, byte[] byJpData, byte[] byCnData)
        {
            try
            {
                // 取得Ngc数据
                byte[] byNgcData = File.ReadAllBytes(ngcCnFile);

                // 根据Ps日文文本数据，查找日文Ngc中的文本数据
                int txtStartPos = this.GetTextStartPos(File.ReadAllBytes(ngcCnFile.Replace("Bio3Cn", "Bio3Jp")), byJpData);
                if (txtStartPos > -1)
                {
                    // 将中文数据写入Ngc数据
                    Array.Copy(byCnData, 0, byNgcData, txtStartPos, byCnData.Length);

                    // 保存中文数据
                    File.WriteAllBytes(ngcCnFile, byNgcData);
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
