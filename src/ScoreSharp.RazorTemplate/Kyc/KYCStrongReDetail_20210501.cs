using System.Text.Json.Serialization;

namespace ScoreSharp.RazorTemplate.Kyc;

public class KYCStrongReDetail_20210501
{
    public 基本資料 基本資料 { get; set; } = new();

    [JsonPropertyName("壹、確認客戶身分加強審查事項(中、高風險客戶填寫)")]
    public 壹確認客戶身分加強審查事項中高風險客戶填寫 壹確認客戶身分加強審查事項中高風險客戶填寫 { get; set; } = new();

    [JsonPropertyName("貳、確認客戶身分加強審查事項(高風險客戶填寫及資料驗證)")]
    public 貳確認客戶身分加強審查事項高風險客戶填寫及資料驗證 貳確認客戶身分加強審查事項高風險客戶填寫及資料驗證 { get; set; } = new();

    [JsonPropertyName("參、簽核")]
    public 參簽核 參簽核 { get; set; } = new();
}

public class 基本資料
{
    public 所建立業務關係 所建立業務關係 { get; set; } = new();
    public string ID { get; set; } = string.Empty;
    public string 姓名 { get; set; } = string.Empty;
    public string 客戶風險等級 { get; set; } = string.Empty;
}

public class 所建立業務關係
{
    public bool 台幣存款 { get; set; } = false;
    public bool 保管箱 { get; set; } = false;
    public bool 外匯存款 { get; set; } = false;
    public bool 企業金融 { get; set; } = false;
    public bool 消費金融 { get; set; } = false;
    public bool 車輛貸款 { get; set; } = false;
    public bool 理財貸款 { get; set; } = false;
    public bool 票債券 { get; set; } = false;
    public bool 財富管理 { get; set; } = false;
    public bool 信託 { get; set; } = false;
    public bool 保險 { get; set; } = false;
    public bool 證券 { get; set; } = false;
    public bool 期貨輔助 { get; set; } = false;
    public bool 衍生性商品業務 { get; set; } = false;
    public bool 外匯保證金 { get; set; } = false;
    public bool 數位存款 { get; set; } = false;
    public bool 信用卡業務 { get; set; } = false;
}

public class 壹確認客戶身分加強審查事項中高風險客戶填寫
{
    [JsonPropertyName("A 客戶曾使用之姓名或別名")]
    public A客戶曾使用之姓名或別名 A客戶曾使用之姓名或別名 { get; set; } = new();

    [JsonPropertyName("B 電話照會")]
    public B電話照會 B電話照會 { get; set; } = new();

    [JsonPropertyName("C 郵件驗證")]
    public C郵件驗證 C郵件驗證 { get; set; } = new();

    [JsonPropertyName("D 任職機構驗證")]
    public D任職機構驗證 D任職機構驗證 { get; set; } = new();

    [JsonPropertyName("E 實地訪查")]
    public E實地訪查 E實地訪查 { get; set; } = new();

    [JsonPropertyName("F 聯徵資料")]
    public F聯徵資料 F聯徵資料 { get; set; } = new();
}

public class A客戶曾使用之姓名或別名
{
    public bool 是否選擇 { get; set; } = false;
    public string 客戶舊名稱 { get; set; } = string.Empty;
    public string 參考號碼 { get; set; } = string.Empty;
}

public class B電話照會
{
    public bool 是否選擇 { get; set; } = false;
    public string 連絡電話 { get; set; } = string.Empty;
    public string 驗證結果 { get; set; } = string.Empty;
}

public class C郵件驗證
{
    public bool 是否選擇 { get; set; } = false;
    public string 郵件寄送地址 { get; set; } = string.Empty;

    [JsonPropertyName("驗證結果")]
    public string 驗證結果 { get; set; } = string.Empty;
}

public class D任職機構驗證
{
    public bool 是否選擇 { get; set; } = false;

    [JsonPropertyName("驗證結果")]
    public string 驗證結果 { get; set; } = string.Empty;
}

public class E實地訪查
{
    public bool 是否選擇 { get; set; } = false;
    public string 實地訪查地址 { get; set; } = string.Empty;
    public string 實地訪查日期 { get; set; } = string.Empty;

    [JsonPropertyName("驗證結果")]
    public 驗證結果3 驗證結果 { get; set; } = new();
}

public class 驗證結果3
{
    public string 與客戶填寫資料一致 { get; set; } = string.Empty;
    public string 與客戶填寫資料不一致原因 { get; set; } = string.Empty;
    public string 驗證結果是否合理 { get; set; } = string.Empty;
}

public class F聯徵資料
{
    public bool 是否選擇 { get; set; } = false;

    [JsonPropertyName("驗證結果")]
    public string 驗證結果 { get; set; } = string.Empty;
    public string 聯徵資料遭通報列為警示帳戶原因 { get; set; } = string.Empty;
    public string 驗證結果是否合理 { get; set; } = string.Empty;
}

public class 貳確認客戶身分加強審查事項高風險客戶填寫及資料驗證
{
    [JsonPropertyName("一、了解客戶財富及資金來源")]
    public 一了解客戶財富及資金來源 一了解客戶財富及資金來源 { get; set; } = new();

    [JsonPropertyName("二、客戶預期帳戶使用狀況及資金流向")]
    public 二客戶預期帳戶使用狀況及資金流向 二客戶預期帳戶使用狀況及資金流向 { get; set; } = new();
}

public class 一了解客戶財富及資金來源
{
    public 所得來源 所得來源 { get; set; } = new();
    public 文件驗證 文件驗證 { get; set; } = new();
    public 非文件驗證 非文件驗證 { get; set; } = new();
    public 判斷財富及資金來源是否合理 判斷財富及資金來源是否合理 { get; set; } = new();
}

public class 所得來源
{
    public 就業所得 就業所得 { get; set; } = new();

    [JsonPropertyName("理財投資-投資")]
    public 理財投資投資 理財投資投資 { get; set; } = new();

    [JsonPropertyName("租金收入-出租")]
    public 租金收入出租 租金收入出租 { get; set; } = new();
    public 專案執業收入 專案執業收入 { get; set; } = new();

    [JsonPropertyName("繼承/贈與")]
    public 繼承贈與 繼承贈與 { get; set; } = new();
    public 退休金 退休金 { get; set; } = new();
    public 閒置資金 閒置資金 { get; set; } = new();
    public 其他 其他 { get; set; } = new();
}

public class 就業所得
{
    public bool 是否選擇 { get; set; } = false;
    public 選項 選項 { get; set; } = new();
    public string 其他 { get; set; } = string.Empty;
}

public class 選項
{
    public bool 經營事業收入 { get; set; } = false;
    public bool 薪資所得 { get; set; } = false;
    public bool 其他 { get; set; } = false;
}

public class 理財投資投資
{
    public bool 是否選擇 { get; set; } = false;

    [JsonPropertyName("選項")]
    public 選項1 選項 { get; set; } = new();
    public string 其他 { get; set; } = string.Empty;
}

public class 選項1
{
    [JsonPropertyName("股票/有價證券")]
    public bool 股票有價證券 { get; set; } = false;
    public bool 房地產 { get; set; } = false;
    public bool 貴金屬 { get; set; } = false;
    public bool 基金 { get; set; } = false;
    public bool 期貨 { get; set; } = false;
    public bool 保險 { get; set; } = false;
    public bool 其他 { get; set; } = false;
}

public class 租金收入出租
{
    public bool 是否選擇 { get; set; } = false;

    [JsonPropertyName("選項")]
    public 選項2 選項 { get; set; } = new();
    public string 其他 { get; set; } = string.Empty;
}

public class 選項2
{
    public bool 房屋 { get; set; } = false;
    public bool 店鋪 { get; set; } = false;
    public bool 招牌 { get; set; } = false;
    public bool 車輛 { get; set; } = false;
    public bool 其他 { get; set; } = false;
}

public class 專案執業收入
{
    public bool 是否選擇 { get; set; } = false;

    [JsonPropertyName("選項")]
    public 選項3 選項 { get; set; } = new();
    public string 其他 { get; set; } = string.Empty;
}

public class 選項3
{
    public bool 法律 { get; set; } = false;
    public bool 會計 { get; set; } = false;
    public bool 企劃 { get; set; } = false;
    public bool 其他 { get; set; } = false;
}

public class 繼承贈與
{
    public bool 是否選擇 { get; set; } = false;

    [JsonPropertyName("選項")]
    public 選項4 選項 { get; set; } = new();
    public string 其他 { get; set; } = string.Empty;
}

public class 選項4
{
    [JsonPropertyName("(內/外)祖父母")]
    public bool 內外祖父母 { get; set; } = false;
    public bool 父母 { get; set; } = false;
    public bool 兄弟姊妹 { get; set; } = false;
    public bool 其他 { get; set; } = false;
}

public class 退休金
{
    public bool 是否選擇 { get; set; } = false;

    [JsonPropertyName("選項")]
    public 選項5 選項 { get; set; } = new();
}

public class 選項5
{
    [JsonPropertyName("軍公教/國營事業退休")]
    public bool 軍公教國營事業退休 { get; set; } = false;
    public bool 民營機構 { get; set; } = false;
}

public class 閒置資金
{
    public bool 是否選擇 { get; set; } = false;

    [JsonPropertyName("選項")]
    public 選項6 選項 { get; set; } = new();
    public string 其他 { get; set; } = string.Empty;
}

public class 選項6
{
    public bool 出售不動產 { get; set; } = false;
    public bool 動產 { get; set; } = false;
    public bool 存款 { get; set; } = false;
    public bool 其他 { get; set; } = false;
}

public class 其他
{
    public bool 是否選擇 { get; set; } = false;
    public string 說明 { get; set; } = string.Empty;
}

public class 文件驗證
{
    public string 來源 { get; set; } = string.Empty;

    [JsonPropertyName("選項")]
    public 選項7 選項 { get; set; } = new();
    public string 其他可證明財富及資金來源之文件 { get; set; } = string.Empty;
}

public class 選項7
{
    public bool 各類所得扣繳稅額報繳證明 { get; set; } = false;
    public bool 薪資單 { get; set; } = false;
    public bool 年報或財報 { get; set; } = false;
    public bool 合約 { get; set; } = false;
    public bool 發票 { get; set; } = false;
    public bool 收據 { get; set; } = false;
    public bool 存摺 { get; set; } = false;
    public bool 任職證明 { get; set; } = false;
    public bool 交易證明文件 { get; set; } = false;
    public bool 不動產權狀 { get; set; } = false;
    public bool 名片 { get; set; } = false;
    public bool 保單 { get; set; } = false;

    [JsonPropertyName("取得寄送客戶所提供地址之水/電/電話費等資料")]
    public bool 取得寄送客戶所提供地址之水電電話費等資料 { get; set; } = false;
    public bool 勞保局投保明細 { get; set; } = false;
    public bool 員工證 { get; set; } = false;
    public bool 聘書 { get; set; } = false;
    public bool 其他可證明財富及資金來源之文件 { get; set; } = false;
}

public class 非文件驗證
{
    public string 方式 { get; set; } = string.Empty;

    [JsonPropertyName("連絡電話/訪查地址/內容")]
    public string 連絡電話訪查地址內容 { get; set; } = string.Empty;
}

public class 判斷財富及資金來源是否合理
{
    public string 是否合理 { get; set; } = string.Empty;
    public string 否_原因 { get; set; } = string.Empty;
}

public class 二客戶預期帳戶使用狀況及資金流向
{
    [JsonPropertyName("1. 預期每月入帳金額與次數(買入)")]
    public _1預期每月入帳金額與次數買入 _1預期每月入帳金額與次數買入 { get; set; } = new();

    [JsonPropertyName("2. 預期每月出帳金額與次數(賣出)")]
    public _2預期每月出帳金額與次數賣出 _2預期每月出帳金額與次數賣出 { get; set; } = new();

    [JsonPropertyName("3. 預期資金流向")]
    public _3預期資金流向 _3預期資金流向 { get; set; } = new();
}

public class _1預期每月入帳金額與次數買入
{
    public string 金額選擇 { get; set; } = string.Empty;
    public string 次數選擇 { get; set; } = string.Empty;

    [JsonPropertyName("依其性質無法提供(如信用卡、證券、票債券、保管箱、保險、財管及授信業務等業務)")]
    public bool 依其性質無法提供如信用卡證券票債券保管箱保險財管及授信業務等業務 { get; set; } = false;
}

public class _2預期每月出帳金額與次數賣出
{
    public string 金額選擇 { get; set; } = string.Empty;
    public string 次數選擇 { get; set; } = string.Empty;

    [JsonPropertyName("依其性質無法提供(如信用卡、證券、票債券、保管箱、保險、財管及授信業務等業務)")]
    public bool 依其性質無法提供如信用卡證券票債券保管箱保險財管及授信業務等業務 { get; set; } = false;
}

public class _3預期資金流向
{
    public string 流向選擇 { get; set; } = string.Empty;
    public string 境外國家 { get; set; } = string.Empty;
    public string 是否為高風險地域國家 { get; set; } = string.Empty;

    [JsonPropertyName("依其性質無法提供(如信用卡、證券、票債券、保管箱、保險、財管及授信業務等業務)")]
    public bool 依其性質無法提供如信用卡證券票債券保管箱保險及財管業務等業務 { get; set; } = false;
}

public class 參簽核
{
    public string 審查意見 { get; set; } = string.Empty;
    public string 備註 { get; set; } = string.Empty;
    public string 經辦簽核 { get; set; } = string.Empty;
    public string 日期 { get; set; } = string.Empty;
    public string 覆核 { get; set; } = string.Empty;
    public string 防制洗錢及打擊資恐督導主管簽核 { get; set; } = string.Empty;
    public string 主管簽核 { get; set; } = string.Empty;
}
