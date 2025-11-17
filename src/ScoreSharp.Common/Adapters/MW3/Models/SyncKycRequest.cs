using ScoreSharp.Common.Constant;

namespace ScoreSharp.Common.Adapters.MW3.Models;

public class SyncKycRequest
{
    /// <summary>
    /// API 名稱
    /// </summary>
    /// <value>
    /// 固定值：KYC00CREDIT
    /// </value>
    [JsonPropertyName("apiName")]
    public string ApiName { get; private set; } = "KYC00CREDIT";

    /// <summary>
    /// 夾帶 header
    /// </summary>
    [JsonPropertyName("headers")]
    public SyncKycMW3Headers Headers { get; set; } = new();

    /// <summary>
    /// EAIHUB Request
    /// </summary>
    [JsonPropertyName("info")]
    public SyncKycMW3Info Info { get; set; } = new();
}

public class SyncKycMW3Headers
{
    /// <summary>
    /// 授權
    /// </summary>
    /// <value>
    /// TEST：Basic Y3JkU1M6Y3JkU1M= <br/>
    /// PROD：
    /// </value>
    [JsonPropertyName("Authorization")]
    public string Authorization { get; set; } = string.Empty;
}

public class SyncKycMW3Info
{
    /// <summary>
    /// 交易類型
    /// </summary>
    /// <value>
    /// 固定值：KYC00CREDIT
    /// </value>
    [JsonPropertyName("_RestType")]
    public string RestType { get; private set; } = "KYC00CREDIT";

    /// <summary>
    /// 已建立業務關係
    /// </summary>
    /// <value>
    /// 固定值：16
    /// </value>
    [JsonPropertyName("bRFlag")]
    public string BRFlag { get; private set; } = KYCRequestConst.已建立業務關係_信用卡業務;

    /// <summary>
    /// 身分類別
    /// </summary>
    /// <remarks>
    /// 可使用強制修改註記*
    /// </remarks>
    /// <value>
    /// 固定值：01
    /// </value>
    [JsonPropertyName("idType")]
    public string IDType { get; private set; } = KYCRequestConst.身分類別_自然人;

    /// <summary>
    /// 信用卡業務風險因子
    /// </summary>
    /// <remarks>
    /// 定義值
    /// P01：同時或累積申辦五張以上
    /// P03：額度200萬以上
    /// P05：透過線上方式申請信用卡業務
    /// </remarks>
    /// <value>P01,P03</value>
    [JsonPropertyName("bRRFlag")]
    public string BRRFlag { get; set; } = string.Empty;

    /// <summary>
    /// 身份證字號
    /// </summary>
    /// <remarks>
    /// 本國自然人：
    /// 1.身分證字號 + A
    ///
    /// 外國自然人：
    /// 1.舊稅籍編號(西元出生年月日8碼數字＋護照英文名前2碼) + C
    /// 2.統一證號/居留證號(二碼英文＋8碼數字) + A
    /// 3.新式居留證號(1碼英文＋數字8或9＋8碼數字) + A
    /// </remarks>
    [JsonPropertyName("uninumber")]
    public string ID { get; set; } = string.Empty;

    /// <summary>
    /// 中文姓名
    /// </summary>
    /// <remarks>
    /// 1.中文姓名會有可能是英文姓名：現在可以強押
    /// 2.全形
    /// 可使用強制修改註記*
    /// </remarks>
    [JsonPropertyName("cName")]
    public string CHName { get; set; } = string.Empty;

    /// <summary>
    /// 英文姓名
    /// </summary>
    /// <remarks>
    /// 1.可使用強制修改註記*
    /// 2.全形
    /// </remarks>
    [JsonPropertyName("cEName")]
    public string ENName { get; set; } = string.Empty;

    /// <summary>
    /// 出生日期
    /// </summary>
    /// <remarks>
    /// 格式：yyyy-MM-dd
    /// 可使用強制修改註記*
    /// </remarks>
    /// <value>
    /// 2019-01-01
    /// </value>
    [JsonPropertyName("cBirthD")]
    public string BirthDay { get; set; } = string.Empty;

    /// <summary>
    /// 客戶姓名檢核
    /// </summary>
    /// <remarks>
    /// 定義值
    /// 1.PEP
    /// 3.黑名單
    /// 4.負面新聞
    /// 5.無
    /// 6.RCA
    /// 7.國內PEP
    /// 8.國外PEP
    /// 9.國際組織PEP
    /// 10.卸任PEP
    /// <br/>
    /// 邏輯：卡處判斷後再提供結果(可複選)
    /// (1)若選項6.RCA有勾選，則必選1.PEP（例：1,6)
    /// (2)選項6.RCA除選項1外不可跟其他選項並存
    /// (3)若選7,8,9,10其一則必選1(例：1,7)
    /// (4)若同時勾選 1.PEP + 10.卸任PEP時，需再確認「cRca、cPepType、cPepType、cPepType」問項。
    /// (5)若勾選選項5，則不得選其他選項。
    /// <br/>
    /// 如果都沒有就填5
    /// </remarks>
    [JsonPropertyName("cNCRFlag")]
    public string NameCheckedReasonCodes { get; set; } = string.Empty;

    /// <summary>
    /// 客戶姓名檢核-客戶是否有RCA為現任PEP
    /// </summary>
    /// <remarks>
    /// 定義值
    /// Y：是
    /// N：否
    /// <br/>
    /// 邏輯
    /// 1.若「cNCRFlag」為"1,10"，「cRca」應帶固定值"N"，其它由徵審人工判斷
    /// 2.若「cNCRFlag」為"1,10"則為必填，非1,10"則下空值
    /// </remarks>
    [JsonPropertyName("cRca")]
    public string ISRCAForCurrentPEP { get; set; } = string.Empty;

    /// <summary>
    /// 客戶姓名檢核-客戶卸任前為何種PEP
    /// </summary>
    /// <remarks>
    /// 定義值
    /// 1.國內
    /// 2.國外
    /// 3.國際組織
    /// <br/>
    /// 邏輯
    /// 若「cNCRFlag」為"1,10"則為必填，
    /// 非1,10"則下空值
    /// </remarks>
    [JsonPropertyName("cPepType")]
    public string ResignPEPKind { get; set; } = string.Empty;

    /// <summary>
    /// 客戶姓名檢核-客戶擔任PEP期間
    /// </summary>
    /// <remarks>
    /// 定義值
    /// 1.不滿2年
    /// 2.滿2年但不滿4年
    /// 3.滿4年
    /// <br/>
    /// 邏輯
    /// 若「cNCRFlag」為"1,10"則為必填，
    /// 非1,10"則下空值
    /// </remarks>
    [JsonPropertyName("cPepTime")]
    public string PEPRange { get; set; } = string.Empty;

    /// <summary>
    /// 客戶姓名檢核-客戶現職與PEP職務是否具關聯性
    /// </summary>
    /// <remarks>
    /// 定義值
    /// Y：是 N：否
    /// <br/>
    /// 邏輯
    /// 若「cNCRFlag」為"1,10"則為必填，
    /// 非1,10"則下空值
    /// </remarks>
    [JsonPropertyName("cPepJ")]
    public string IsCurrentPositionRelatedPEPPosition { get; set; } = string.Empty;

    /// <summary>
    /// 姓名檢核_查詢日期
    /// </summary>
    /// <remarks>
    /// 格式：yyyy-MM-dd HH:mm:ss
    /// </remarks>
    [JsonPropertyName("cNcTime")]
    public string NameCheckStartTime { get; set; } = string.Empty;

    /// <summary>
    /// 姓名檢核_命中有無
    /// </summary>
    /// <remarks>
    /// 定義值
    /// 1：有 0：無
    /// </remarks>
    [JsonPropertyName("cNcHit")]
    public string NameCheckResponseResult { get; set; } = string.Empty;

    /// <summary>
    /// 姓名檢核_分數
    /// </summary>
    [JsonPropertyName("cNcScore")]
    public string NameCheckRcPoint { get; set; } = string.Empty;

    /// <summary>
    /// 姓名檢核_回傳查詢代碼
    /// </summary>
    [JsonPropertyName("cNcRef")]
    public string NameCheckAMLId { get; set; } = string.Empty;

    /// <summary>
    /// 國籍
    /// </summary>
    /// <remarks>
    /// 定義值
    /// 1：中華民國
    /// 2：其他
    /// <br/>
    /// 徵審系統如果是CitizenshipCode = TW => cNation = 1，其餘則 2
    /// 可使用強制修改註記*
    /// </remarks>
    [JsonPropertyName("cNation")]
    public string Nation { get; set; } = string.Empty;

    /// <summary>
    /// 國家名
    /// </summary>
    /// <remarks>
    /// 若國籍選其他則必填，ISO國家代碼(半形英文)
    /// </remarks>
    [JsonPropertyName("cNationO")]
    public string NationO { get; set; } = string.Empty;

    /// <summary>
    /// 出生地
    /// </summary>
    /// <remarks>
    /// 定義值
    /// 1：中華民國
    /// 2：其他
    /// <br/>
    /// 新北市、臺中市、高雄市、
    /// 臺北市、桃園市、臺南市、
    /// 彰化縣、屏東縣、雲林縣、
    /// 新竹縣、苗栗縣、嘉義縣、
    /// 南投縣、宜蘭縣、新竹市、
    /// 基隆市、花蓮縣、嘉義市、
    /// 臺東縣、金門縣、澎湖縣、
    /// 連江縣 → 1 中華民國
    /// <br/>
    /// 非以上縣市→2其它
    /// 可使用強制修改註記*
    /// </remarks>
    [JsonPropertyName("cBirthP")]
    public string BirthCitizenshipCode { get; set; } = string.Empty;

    /// <summary>
    /// 出生地_其他
    /// </summary>
    /// <remarks>
    /// 若出生地選其他則必填，ISO國家代碼(半形英文)
    /// </remarks>
    [JsonPropertyName("cBirthPO")]
    public string BirthCitizenshipCodeOther { get; set; } = string.Empty;

    /// <summary>
    /// 職業別
    /// </summary>
    /// <remarks>
    /// 定義值
    /// SetUp_AMLProfession
    /// <br/>
    /// 「職業別」請以二碼代碼拋送kyc，依卡處代碼右靠左補0
    /// 可使用強制修改註記*
    /// </remarks>
    [JsonPropertyName("cJob")]
    public string AMLProfessionCode { get; set; } = string.Empty;

    /// <summary>
    /// 職業別其他
    /// </summary>
    /// <remarks>
    /// 職業別為其他時必填
    /// </remarks>
    [JsonPropertyName("cJobO")]
    public string AMLProfessionOther { get; set; } = string.Empty;

    /// <summary>
    /// 職級
    /// </summary>
    /// <remarks>
    /// 定義值
    /// SetUp_AMLJobLevel
    /// 0.其他
    /// 1.職員
    /// 2.財務主管或其他主管職
    /// 3.總經理或相當職位
    /// 4.董事長或相當職位
    /// <br/>
    /// 「職級別」不須補0
    /// 可使用強制修改註記*
    /// </remarks>
    [JsonPropertyName("cPosFlag")]
    public string AMLJobLevelCode { get; set; } = string.Empty;

    /// <summary>
    /// 職級其他
    /// </summary>
    /// <remarks>
    /// 邏輯： <br/>
    /// 1. 當職業別(cJob)為A～K、N (01～11、14)， 且職務(cPosFlag)勾選其他，則本欄必填 <br/>
    /// 2. 當職業別(cJob)為L、M、O、P(12、13、15、00) ，且職務(cPosFlag)勾選其他，則本欄可為空白(非必填)
    /// </remarks>
    [JsonPropertyName("cPosO")]
    public string PosO { get; set; } = string.Empty;

    /// <summary>
    /// 戶籍地址FLG
    /// </summary>
    /// <remarks>
    /// 定義值
    /// 1：中華民國
    /// 2：其他
    /// <br/>
    /// 本國地址若因特殊因素無法切欄之地址需選 2
    /// 地址資訊需入至以下欄位
    /// 國家名cHomeO：TW
    /// 郵遞區號(外國)cHomeAddZip：非必填（若可判斷系統帶入）
    /// 地址名cHomeAdd：完整地址寫入本欄
    /// 可使用強制修改註記*
    /// <br/>
    /// 1、本國正常切欄地址寫入1
    /// 2、本國無法切欄地址寫入2
    /// 3、外國地址寫入2
    /// </remarks>
    [JsonPropertyName("cHomeAddFlag")]
    public string HomeAddFlag { get; set; } = string.Empty;

    /// <summary>
    /// 戶籍_郵遞區號
    /// </summary>
    /// <remarks>
    /// 拋送時半形數字
    /// </remarks>
    [JsonPropertyName("cHomezip")]
    public string Reg_ZipCode { get; set; } = string.Empty;

    /// <summary>
    /// 縣市
    /// </summary>
    /// <remarks>
    /// 若戶籍地址選1則必填
    /// </remarks>
    /// <value>臺北市、新北市</value>
    [JsonPropertyName("cHomeCity")]
    public string Reg_City { get; set; } = string.Empty;

    /// <summary>
    /// 鄉鎮區
    /// </summary>
    /// <remarks>
    /// 若戶籍地址選1則必填
    /// </remarks>
    [JsonPropertyName("cHomeArea")]
    public string Reg_District { get; set; } = string.Empty;

    /// <summary>
    /// 里
    /// </summary>
    /// <remarks>
    /// 格式：固定空白,因徵審無此欄位
    /// </remarks>
    [JsonPropertyName("cHomeVil")]
    public string HomeVil { get; private set; } = string.Empty;

    /// <summary>
    /// 路/街
    /// </summary>
    /// <remarks>
    /// 若戶籍地址選1則必填
    /// </remarks>
    [JsonPropertyName("cHomeStreet")]
    public string Reg_Road { get; set; } = string.Empty;

    /// <summary>
    /// 巷
    /// </summary>
    /// <remarks>
    /// 格式：全形數字
    /// </remarks>
    [JsonPropertyName("cHomeLane")]
    public string Reg_Lane { get; set; } = string.Empty;

    /// <summary>
    /// 弄
    /// </summary>
    /// <remarks>
    /// 格式：全形數字
    /// </remarks>
    [JsonPropertyName("cHomeAly")]
    public string Reg_Alley { get; set; } = string.Empty;

    /// <summary>
    /// 號
    /// </summary>
    /// <remarks>
    /// 若戶籍地址選1則必填
    /// 格式：全形數字
    /// </remarks>
    [JsonPropertyName("cHomeNum")]
    public string Reg_Number { get; set; } = string.Empty;

    /// <summary>
    /// 之
    /// </summary>
    /// <remarks>
    /// 格式：全形數字
    /// </remarks>
    [JsonPropertyName("cHomeOf")]
    public string Reg_SubNumber { get; set; } = string.Empty;

    /// <summary>
    /// 樓
    /// </summary>
    /// <remarks>
    /// 格式：全形數字
    /// </remarks>
    [JsonPropertyName("cHomeFlr")]
    public string Reg_Floor { get; set; } = string.Empty;

    /// <summary>
    /// 其他
    /// </summary>
    [JsonPropertyName("cHomeEtc")]
    public string Reg_Other { get; set; } = string.Empty;

    /// <summary>
    /// 國家名
    /// </summary>
    /// <remarks>
    /// 1.外國人依客戶申辦勾選值帶入，若無則填空
    /// 2.本國人且戶籍地址cHomeAddFlag為2其他，本欄固定帶入TW
    /// </remarks>
    [JsonPropertyName("cHomeO")]
    public string HomeO { get; set; } = string.Empty;

    /// <summary>
    /// 郵遞區號(外國)
    /// </summary>
    /// <remarks>
    /// 3+2比對提供（若無法比對下空值)
    /// </remarks>
    [JsonPropertyName("cHomeAddZip")]
    public string HomeAddZip { get; set; } = string.Empty;

    /// <summary>
    /// 完整地址
    /// </summary>
    /// <remarks>
    /// 1.cHomeO = TW + cHomeAddFlag = 2 完整地址寫入本欄
    /// 2.通常為原卡友使用
    /// </remarks>
    [JsonPropertyName("cHomeAdd")]
    public string HomeAdd { get; set; } = string.Empty;

    /// <summary>
    /// 主要所得及資金來源
    /// </summary>
    /// <remarks>
    /// 定義值 <br/>
    /// 01：經營事業收入
    /// 02：薪資所得
    /// 03：繼承／贈與
    /// 04：理財投資
    /// 05：退休金
    /// 06：租金收入
    /// 07：專案執業收入
    /// 08：閒置資金
    /// 09：其他
    /// <br/>
    /// 來源：SetUp_MainIncomeAndFund <br/>
    /// 備註：拋送時我們要幫忙補0
    /// 可使用強制修改註記*
    /// </remarks>
    /// <value>
    /// 01,08
    /// </value>
    [JsonPropertyName("cIAMFlag")]
    public string MainIncomeAndFundCodes { get; set; } = string.Empty;

    /// <summary>
    /// 主要所得及資金來源其他
    /// </summary>
    /// <remarks>
    /// 當「主要所得及資金來源」填選09時必填
    /// </remarks>
    [JsonPropertyName("cIAMO")]
    public string MainIncomeAndFundOther { get; set; } = string.Empty;

    /// <summary>
    /// 建立業務關係目的
    /// </summary>
    /// <value>
    /// 固定值：12
    /// </value>
    [JsonPropertyName("cBRPFlag")]
    public string BRPFlag { get; private set; } = KYCRequestConst.建立業務關係目的_發卡業務;

    /// <summary>
    /// 是否為OBU客戶
    /// </summary>
    /// <value>
    /// 固定值：N
    /// </value>
    [JsonPropertyName("cOBU")]
    public string OBU { get; private set; } = "N";

    /// <summary>
    /// 客戶風險評估
    /// </summary>
    /// <remarks>
    /// 定義值 <br/>
    /// 01：新（加）開帳戶／新增業務往來關係
    /// 02：客戶身分背景有重大變動
    /// 03：發生導致客戶風險狀況變化之事件
    /// 04：執行客戶定期審查
    /// 05：其他【介面隱藏】
    /// 06：原卡友加辦信用卡業務
    /// </remarks>
    /// <value>
    /// 非卡友固定值：01
    /// 卡友固定值：06
    /// </value>
    [JsonPropertyName("cRAreaFlag")]
    public string RAreaFlag { get; set; } = string.Empty;

    /// <summary>
    /// 經辦審查意見
    /// </summary>
    /// <remarks>
    /// 此為重要欄位攸關是變更還是撤退件
    /// <br/>
    /// 定義值
    /// Y：建議核准
    /// N：建議拒絕
    /// <br/>
    /// 邏輯
    /// 1. 新建立時固定帶Y
    /// 2. 若退件或撤件時加N
    /// 3. 若取消退撤件再報Y
    /// 4. 必要欄住異動重拋時下Y
    /// </remarks>
    [JsonPropertyName("uOpnion")]
    public string UOpnion { get; set; } = string.Empty;

    /// <summary>
    /// 系統客戶資料最後編輯時間(動作時間)
    /// </summary>
    /// <remarks>
    /// 送檔時間
    /// 格式：yyyy-MM-dd HH:mm:ss
    /// </remarks>
    [JsonPropertyName("cEditTime")]
    public string EditTime { get; set; } = string.Empty;

    /// <summary>
    /// 經辦帳號
    /// </summary>
    /// <value>
    /// 固定值：卡處經辦
    /// </value>
    [JsonPropertyName("uId")]
    public string UId { get; private set; } = "卡處經辦";

    /// <summary>
    /// 督導主管帳號
    /// </summary>
    /// <value>
    /// 固定值：卡處督導主管
    /// </value>
    [JsonPropertyName("sId")]
    public string SId { get; private set; } = "卡處督導主管";

    /// <summary>
    /// 主管帳號
    /// </summary>
    /// <remarks>
    /// 中低風險案件帶空白
    /// 高風險案件固定帶「卡處主管」
    /// </remarks>
    [JsonPropertyName("mId")]
    public string MId { get; set; } = string.Empty;

    /// <summary>
    /// KYC用分行代號
    /// </summary>
    /// <value>
    /// 固定值：911
    /// </value>
    [JsonPropertyName("branch")]
    public string Branch { get; private set; } = KYCRequestConst.卡處分行;

    /// <summary>
    /// RMD用歸屬分行分行代號
    /// </summary>
    /// <value>
    /// 固定值：911
    /// </value>
    [JsonPropertyName("BranchRMD")]
    public string BranchRMD { get; private set; } = KYCRequestConst.卡處分行;

    /// <summary>
    /// 手機
    /// </summary>
    [JsonPropertyName("cCell")]
    public string Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 資料來源
    /// </summary>
    /// <remarks>
    /// 定義值
    /// 1：新卡友
    /// 2：原卡友加辦卡
    /// <br/>
    /// 邏輯：原卡友資料入檔KYC處理方式 <br/>
    /// 1.入檔資料不需踢退，故無需人工強壓
    /// 2.仍回拋風險等級
    /// 3.不需執行加強審查
    /// </remarks>
    [JsonPropertyName("source")]
    public string Source { get; set; } = string.Empty;
}
