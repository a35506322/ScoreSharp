using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ScoreSharp.API.Modules.SysPersonnel.WebRetryCase.UpdateWebRetryCaseBySeqNo;

public class UpdateWebRetryCaseBySeqNoRequest
{
    /// <summary>
    /// PK
    /// </summary>
    [DisplayName("PK")]
    [Required]
    public long SeqNo { get; set; }

    /// <summary>
    /// Request
    /// </summary>
    [DisplayName("Request")]
    [Required]
    public string Request { get; set; }
}

[ValidateNever]
public class EcardNewCaseRequest
{
    /// <summary>
    /// 申請書編號
    /// 必填
    /// 由網通（E-CARD）端編碼
    /// </summary>
    [JsonPropertyName("APPLY_NO")]
    public string ApplyNo { get; set; } = "";

    /// <summary>
    /// 身份別
    /// 卡友
    /// 存戶
    /// 持他行卡
    /// 自然人憑證

    /// </summary>
    [JsonPropertyName("ID_TYPE")]
    public string? IDType { get; set; }

    /// <summary>
    /// 徵信代碼
    /// 卡友 = A02
    /// 非卡友 = 空值
    /// </summary>
    [JsonPropertyName("CREDIT_ID")]
    public string? CreditCheckCode { get; set; } = "";

    /// <summary>
    /// 主附卡別
    /// 1. 正卡
    /// 2. 附卡
    /// 3. 正卡+附卡
    /// 4. 附卡2
    /// 5. 正卡+附卡2
    /// </summary>
    [JsonPropertyName("CARD_OWNER")]
    public string? CardOwner { get; set; } = "";

    /// <summary>
    /// 卡片種類
    /// 必填
    /// 可多選欄位，如多選資料以/相隔，例如：JA00/JC00
    /// </summary>
    [JsonPropertyName("MCARD_SER")]
    public string? ApplyCardType { get; set; } = "";

    /// <summary>
    /// 表單代碼
    /// 必填
    /// 表單學習專用，由資處定義代碼。
    /// </summary>
    [JsonPropertyName("FORM_ID")]
    public string? FormCode { get; set; } = "";

    /// <summary>
    /// 正卡_中文姓名
    /// 必填
    /// </summary>
    [JsonPropertyName("CH_NAME")]
    public string? CHName { get; set; } = "";

    /// <summary>
    /// 正卡_性別
    /// 1. 男
    /// 2. 女
    /// </summary>
    [JsonPropertyName("SEX")]
    public string? Sex { get; set; }

    /// <summary>
    /// 正卡_出生日期
    /// 必填
    /// YYYMMDD(民國年)
    /// </summary>
    [JsonPropertyName("BIRTHDAY")]
    public string? Birthday { get; set; } = "";

    /// <summary>
    /// 正卡_英文姓名
    /// </summary>
    [JsonPropertyName("ENG_NAME")]
    public string? ENName { get; set; }

    /// <summary>
    /// 正卡_出生地
    /// ECARD 值如果是台灣要對應台灣縣市，如果是外國人則是其他
    /// </summary>
    [JsonPropertyName("BIRTH_PLACE")]
    public string? BirthPlace { get; set; }

    /// <summary>
    /// 正卡_出生地_其他
    /// 當正卡_出生地 = 其他時，必填
    /// </summary>
    [JsonPropertyName("BIRTH_PLACE_OTHER")]
    public string? BirthPlaceOther { get; set; }

    /// <summary>
    /// 正卡_國籍別
    /// 需符合Paperless「國籍設定」
    /// </summary>
    [JsonPropertyName("NATIONALITY")]
    public string? CitizenshipCode { get; set; }

    /// <summary>
    /// 正卡_身份證字號
    /// 必填
    /// </summary>
    [JsonPropertyName("P_ID")]
    public string? ID { get; set; } = "";

    private string? _idIssueDate;

    /// <summary>
    /// 正卡_身分證發證日期
    /// 民國 YYYMMDD
    /// 20250120 經測試 ECARD 會傳 6碼過來,例如880101
    /// </summary>
    [JsonPropertyName("P_ID_GETDATE")]
    public string? IDIssueDate
    {
        get => _idIssueDate;
        set
        {
            if (!string.IsNullOrEmpty(value) && value.Length != 7)
            {
                _idIssueDate = value.PadLeft(7, '0');
            }
            else if (string.IsNullOrEmpty(value))
            {
                _idIssueDate = string.Empty;
            }
            else
            {
                _idIssueDate = value;
            }
        }
    }

    /// <summary>
    /// 正卡_身分證發證地點
    /// 關聯 SetUp_IDCardRenewalLocation
    /// Ecard給中文 要自己對應 SetUp_IDCardRenewalLocation 的 Name
    /// </summary>
    [JsonPropertyName("P_ID_GETADDRNAME")]
    public string? IDCardRenewalLocationName { get; set; }

    /// <summary>
    /// 正卡_身分證請領狀態
    /// 1. 初發
    /// 2. 補發
    /// 3. 換發
    /// </summary>
    [JsonPropertyName("P_ID_STATUS")]
    public string? IDTakeStatus { get; set; }

    /// <summary>
    /// 正卡戶藉地址_郵區
    /// </summary>
    [JsonPropertyName("REG_ZIP")]
    public string? Reg_Zip { get; set; }

    /// <summary>
    /// 正卡戶藉地址_縣市
    /// </summary>
    [JsonPropertyName("REG_ADDR_CITY")]
    public string? Reg_City { get; set; }

    /// <summary>
    /// 正卡戶藉地址_鄉鎮市區
    /// </summary>
    [JsonPropertyName("REG_ADDR_DIST")]
    public string? Reg_District { get; set; }

    /// <summary>
    /// 正卡戶藉地址_路
    /// </summary>
    [JsonPropertyName("REG_ADDR_RD")]
    public string? Reg_Road { get; set; }

    /// <summary>
    /// 正卡戶藉地址_巷
    /// </summary>
    [JsonPropertyName("REG_ADDR_LN")]
    public string? Reg_Lane { get; set; }

    /// <summary>
    /// 正卡戶藉地址_弄
    /// </summary>
    [JsonPropertyName("REG_ADDR_ALY")]
    public string? Reg_Alley { get; set; }

    /// <summary>
    /// 正卡戶藉地址_號
    /// </summary>
    [JsonPropertyName("REG_ADDR_NO")]
    public string? Reg_Number { get; set; }

    /// <summary>
    /// 正卡戶藉地址_之號
    /// </summary>
    [JsonPropertyName("REG_ADDR_SUBNO")]
    public string? Reg_SubNumber { get; set; }

    /// <summary>
    /// 正卡戶藉地址_樓
    /// </summary>
    [JsonPropertyName("REG_ADDR_F")]
    public string? Reg_Floor { get; set; }

    /// <summary>
    /// 正卡戶藉地址_室
    /// </summary>
    [JsonPropertyName("REG_ADDR_OTHER")]
    public string? Reg_Other { get; set; }

    /// <summary>
    /// 正卡居住地址_郵區
    /// </summary>
    [JsonPropertyName("HOME_ZIP")]
    public string? Home_Zip { get; set; }

    /// <summary>
    /// 正卡居住地址_縣市
    /// </summary>
    [JsonPropertyName("HOME_ADDR_CITY")]
    public string? Home_City { get; set; }

    /// <summary>
    /// 正卡居住地址_鄉鎮市區
    /// </summary>
    [JsonPropertyName("HOME_ADDR_DIST")]
    public string? Home_District { get; set; }

    /// <summary>
    /// 正卡居住地址_路
    /// </summary>
    [JsonPropertyName("HOME_ADDR_RD")]
    public string? Home_Road { get; set; }

    /// <summary>
    /// 正卡居住地址_巷
    /// </summary>
    [JsonPropertyName("HOME_ADDR_LN")]
    public string? Home_Lane { get; set; }

    /// <summary>
    /// 正卡居住地址_弄
    /// </summary>
    [JsonPropertyName("HOME_ADDR_ALY")]
    public string? Home_Alley { get; set; }

    /// <summary>
    /// 正卡居住地址_號
    /// </summary>
    [JsonPropertyName("HOME_ADDR_NO")]
    public string? Home_Number { get; set; }

    /// <summary>
    /// 正卡居住地址_之號
    /// </summary>
    [JsonPropertyName("HOME_ADDR_SUBNO")]
    public string? Home_SubNumber { get; set; }

    /// <summary>
    /// 正卡居住地址_樓
    /// </summary>
    [JsonPropertyName("HOME_ADDR_F")]
    public string? Home_Floor { get; set; }

    /// <summary>
    /// 正卡居住地址_室
    /// </summary>
    [JsonPropertyName("HOME_ADDR_OTHER")]
    public string? Home_Other { get; set; }

    /// <summary>
    /// 正卡帳單地址
    /// 1. 同戶籍
    /// 2. 同居住
    /// 3. 同公司
    /// </summary>
    [JsonPropertyName("BILL_TO_ADDR")]
    public string? BillAddress { get; set; }

    /// <summary>
    /// 正卡寄卡地址
    /// 1. 同戶籍
    /// 2. 同居住
    /// 3. 同公司
    /// </summary>
    [JsonPropertyName("CARD_TO_ADDR")]
    public string? SendCardAddress { get; set; }

    /// <summary>
    /// 正卡行動電話
    /// </summary>
    [JsonPropertyName("CELL_PHONE_NO")]
    public string? Mobile { get; set; }

    /// <summary>
    /// 正卡電子信箱
    /// </summary>
    [JsonPropertyName("EMAIL")]
    public string? EMail { get; set; }

    /// <summary>
    /// 正卡_ AML職業類別
    /// </summary>
    [JsonPropertyName("JOB_TYPE")]
    public string? AMLProfessionCode { get; set; }

    /// <summary>
    /// 正卡_ AML職業類別_其他
    /// </summary>
    [JsonPropertyName("JOB_TYPE_OTHER")]
    public string? AMLProfessionOther { get; set; }

    /// <summary>
    /// 正卡_ AML職級別
    /// </summary>
    [JsonPropertyName("JOB_LV")]
    public string? AMLJobLevelCode { get; set; }

    /// <summary>
    /// 正卡_公司名稱
    /// </summary>
    [JsonPropertyName("COMP_NAME")]
    public string? CompName { get; set; }

    /// <summary>
    /// 正卡_公司電話
    /// </summary>
    [JsonPropertyName("COMP_TEL_NO")]
    public string? CompPhone { get; set; }

    /// <summary>
    /// 正卡_公司地址_郵區
    /// </summary>
    [JsonPropertyName("COMP_ZIP")]
    public string? Comp_Zip { get; set; }

    /// <summary>
    /// 正卡_公司地址_縣市
    /// </summary>
    [JsonPropertyName("COMP_ADDR_CITY")]
    public string? Comp_City { get; set; }

    /// <summary>
    /// 正卡_公司地址_鄉鎮市區
    /// </summary>
    [JsonPropertyName("COMP_ADDR_DIST")]
    public string? Comp_District { get; set; }

    /// <summary>
    /// 正卡_公司地址_路
    /// </summary>
    [JsonPropertyName("COMP_ADDR_RD")]
    public string? Comp_Road { get; set; }

    /// <summary>
    /// 正卡_公司地址_巷
    /// </summary>
    [JsonPropertyName("COMP_ADDR_LN")]
    public string? Comp_Lane { get; set; }

    /// <summary>
    /// 正卡_公司地址_弄
    /// </summary>
    [JsonPropertyName("COMP_ADDR_ALY")]
    public string? Comp_Alley { get; set; }

    /// <summary>
    /// 正卡_公司地址_號
    /// </summary>
    [JsonPropertyName("COMP_ADDR_NO")]
    public string? Comp_Number { get; set; }

    /// <summary>
    /// 正卡_公司地址_之號
    /// </summary>
    [JsonPropertyName("COMP_ADDR_SUBNO")]
    public string? Comp_SubNumber { get; set; }

    /// <summary>
    /// 正卡_公司地址_樓
    /// </summary>
    [JsonPropertyName("COMP_ADDR_F")]
    public string? Comp_Floor { get; set; }

    /// <summary>
    /// 正卡_公司地址_室
    /// </summary>
    [JsonPropertyName("COMP_ADDR_OTHER")]
    public string? Comp_Other { get; set; }

    /// <summary>
    /// 正卡_現職月收入
    /// 檢驗數字
    /// </summary>
    [JsonPropertyName("SALARY")]
    public string? CurrentMonthIncome { get; set; }

    /// <summary>
    /// 正卡_主要所得及資金來源
    /// 需符合Paperless「主要所得及資金來源設定」。多選時中間以逗號(,)區隔。
    /// </summary>
    [JsonPropertyName("MAIN_INCOME")]
    public string? MainIncomeAndFundCodes { get; set; }

    /// <summary>
    /// 正卡_主要所得其他
    /// </summary>
    [JsonPropertyName("MAIN_INCOME_OTHER")]
    public string? MainIncomeAndFundOther { get; set; }

    /// <summary>
    /// 正卡_本人同意提供資料予聯名(認同)集團
    /// 0: 否
    /// 1: 是
    /// </summary>
    [JsonPropertyName("ACCEPT_DM_FLG")]
    public string? IsAgreeDataOpen { get; set; }

    /// <summary>
    /// 正卡_專案代號
    /// </summary>
    [JsonPropertyName("M_PROJECT_CODE_ID")]
    public string? ProjectCode { get; set; }

    /// <summary>
    /// 推廣單位
    /// </summary>
    [JsonPropertyName("PROM_UNIT_SER")]
    public string? PromotionUnit { get; set; }

    /// <summary>
    /// 推廣人員代號
    /// </summary>
    [JsonPropertyName("PROM_USER_NAME")]
    public string? PromotionUser { get; set; }

    /// <summary>
    /// 是否同意提供資料於第三人行銷
    /// 0: 不同意
    /// 1: 同意
    /// </summary>
    [JsonPropertyName("AGREE_MARKETING_FLG")]
    public string? IsAgreeMarketing { get; set; }

    /// <summary>
    /// 正卡_是否同意悠遊卡自動加值預設開啟
    /// 0: 否
    /// 1: 是
    /// </summary>
    [JsonPropertyName("NOT_ACCEPT_EPAPAER_FLG")]
    public string? IsAcceptEasyCardDefaultBonus { get; set; }

    /// <summary>
    /// 家庭成員6年齡(首刷禮代碼使用)
    /// </summary>
    [JsonPropertyName("FAMILY6_AGE")]
    public string? FirstBrushingGiftCode { get; set; }

    /// <summary>
    /// 戶籍遷出轉卡（桃園市民卡用）
    /// Y：同意
    /// N：不同意
    /// </summary>
    [JsonPropertyName("CHG_CARD_ADDR")]
    public string? HouseholdRegTransferCardForTaoyuanCitizenCard { get; set; }

    /// <summary>
    /// 帳單形式
    /// 1. 電子帳單
    /// 2. 簡訊帳單
    /// 3. 紙本帳單
    /// 4. LINE帳單
    /// </summary>
    [JsonPropertyName("BILL_TYPE")]
    public string? BillType { get; set; }

    /// <summary>
    /// 是否申辦自動扣繳
    /// Y：是
    /// N：否
    /// </summary>
    [JsonPropertyName("AUTO_FLG")]
    public string? IsApplyAutoDeduction { get; set; }

    /// <summary>
    /// 自動扣繳用戶＿銀行存款帳號
    /// </summary>
    [JsonPropertyName("ACCT_NO")]
    public string? AutoDeductionBankAccount { get; set; }

    /// <summary>
    /// 自動扣繳用戶＿繳款方式
    /// 10 = 最低
    /// 20 = 全額
    /// </summary>
    [JsonPropertyName("PAY_WAY")]
    public string? AutoDeductionPayType { get; set; }

    /// <summary>
    /// 進件方式
    /// Ecard
    /// APP
    /// </summary>
    [JsonPropertyName("SOURCE_TYPE")]
    public string Source { get; set; } = null!;

    /// <summary>
    /// 來源IP
    /// </summary>
    [JsonPropertyName("SOURCE_IP")]
    public string? UserSourceIP { get; set; }

    /// <summary>
    /// OTP手機
    /// </summary>
    [JsonPropertyName("OTP_MOBILE_PHONE")]
    public string? OTPMobile { get; set; }

    /// <summary>
    /// OTP時間
    /// YYYY/MM/DD HH:MM:SS
    /// </summary>
    [JsonPropertyName("OTP_TIME")]
    public string? OTPTime { get; set; }

    /// <summary>
    /// 是否申請數位卡
    /// Y：是
    /// N：否
    /// </summary>
    [JsonPropertyName("DIGI_CARD_FLG")]
    public string? IsApplyDigtalCard { get; set; }

    /// <summary>
    /// 申請卡種
    /// 1. 實體
    /// 2. 數位
    /// 3. 實體+數位
    /// </summary>
    [JsonPropertyName("CARD_KIND")]
    public string? ApplyCardKind { get; set; }

    /// <summary>
    /// 申請書檔名
    /// ECard流水序號，Paperless以此欄位對應ApplyFile資料表的cCard_AppId欄位，取得ApplyFile.uploadPDF二進位資料即為申請書檔案
    /// </summary>
    [JsonPropertyName("APPLICATION_FILE_NAME")]
    public string CardAppId { get; set; }

    /// <summary>
    /// 附件註記
    /// 1. 附件異常
    /// 2. MYDATA後補
    /// </summary>
    [JsonPropertyName("APPENDIX_FLG")]
    public string? EcardAttachmentNotes { get; set; }

    /// <summary>
    /// MyData案件編號
    /// 當附件註記為2：MYDATA後補時，本欄位必定有值
    /// </summary>
    [JsonPropertyName("MYDATA_NO")]
    public string? MyDataCaseNo { get; set; }

    /// <summary>
    /// 正卡寄卡地址
    /// 對應 E-CARD = CARD_ADDR 為正卡人寄卡地址
    /// 客戶為原卡友時ECARD傳入完整地址，分別可能為同帳單的地址、同公司的地址、其他(客戶自填)
    /// </summary>
    [JsonPropertyName("CARD_ADDR")]
    public string? CardAddr { get; set; }

    /// <summary>
    /// 是否變更KYC
    /// Y：是
    /// N：否
    /// </summary>
    [JsonPropertyName("KYC_CHG_FLG")]
    public string? IsKYCChange { get; set; }

    /// <summary>
    /// 消費通知綁定
    /// Y：同意
    /// N：不同意
    /// </summary>
    [JsonPropertyName("CONSUM_NOTICE_FLG")]
    public string? IsPayNoticeBind { get; set; }

    /// <summary>
    /// UUID
    /// </summary>
    [JsonPropertyName("UUID")]
    public string? LineBankUUID { get; set; }

    /// <summary>
    /// 活存目前餘額
    /// 檢驗數字
    /// </summary>
    [JsonPropertyName("DEMAND_CURR_BAL")]
    public string? HUOCUN_Balance { get; set; }

    /// <summary>
    /// 定存目前餘額
    /// 檢驗數字
    /// </summary>
    [JsonPropertyName("TIME_CURR_BAL")]
    public string? DINGCUN_Balance { get; set; }

    /// <summary>
    /// 活存90天平均餘額
    /// 檢驗數字
    /// </summary>
    [JsonPropertyName("DEMAND_90DAY_BAL")]
    public string? HUOCUN_Balance_90 { get; set; }

    /// <summary>
    /// 定存90天平均餘額
    /// 檢驗數字
    /// </summary>
    [JsonPropertyName("TIME_90DAY_BAL")]
    public string? DINGCUN_Balance_90 { get; set; }

    /// <summary>
    /// 餘額更新日期
    /// </summary>
    [JsonPropertyName("BAL_UPD_DATE")]
    public string? BalanceUpdateDate { get; set; }

    /// <summary>
    /// 是否學生身分
    /// Y：是
    /// N：否
    /// </summary>
    [JsonPropertyName("STUDENT_FLG")]
    public string? IsStudent { get; set; }

    /// <summary>
    /// 家長姓名
    /// 如為中文，最長13碼
    /// 如為英文，最長26碼
    /// </summary>
    [JsonPropertyName("PARENT_NAME")]
    public string? ParentName { get; set; }

    /// <summary>
    /// 家長連絡電話
    /// Paperless判斷當資料是09開頭，此欄位放【家長行動電話】。非09開頭，此欄位放【家長居住電話】。
    /// </summary>
    [JsonPropertyName("PARENT_HOME_TEL_NO")]
    public string? ParentPhone { get; set; }

    /// <summary>
    /// 家長居住地址_郵區
    /// </summary>
    [JsonPropertyName("PARENT_HOME_ZIP")]
    public string? ParentLive_ZipCode { get; set; }

    /// <summary>
    /// 家長居住地址_縣市
    /// </summary>
    [JsonPropertyName("PARENT_HOME_ADDR_CITY")]
    public string? ParentLive_City { get; set; }

    /// <summary>
    /// 家長居住地址_鄉鎮市區
    /// </summary>
    [JsonPropertyName("PARENT_HOME_ADDR_DIST")]
    public string? ParentLive_District { get; set; }

    /// <summary>
    /// 家長居住地址_路
    /// </summary>
    [JsonPropertyName("PARENT_HOME_ADDR_RD")]
    public string? ParentLive_Road { get; set; }

    /// <summary>
    /// 家長居住地址_巷
    /// </summary>
    [JsonPropertyName("PARENT_HOME_ADDR_LN")]
    public string? ParentLive_Lane { get; set; }

    /// <summary>
    /// 家長居住地址_弄
    /// </summary>
    [JsonPropertyName("PARENT_HOME_ADDR_ALY")]
    public string? ParentLive_Alley { get; set; }

    /// <summary>
    /// 家長居住地址_號
    /// </summary>
    [JsonPropertyName("PARENT_HOME_ADDR_NO")]
    public string? ParentLive_Number { get; set; }

    /// <summary>
    /// 家長居住地址_之號
    /// </summary>
    [JsonPropertyName("PARENT_HOME_ADDR_SUBNO")]
    public string? ParentLive_SubNumber { get; set; }

    /// <summary>
    /// 家長居住地址_樓
    /// </summary>
    [JsonPropertyName("PARENT_HOME_ADDR_F")]
    public string? ParentLive_Floor { get; set; }

    /// <summary>
    /// 家長居住地址_室
    /// </summary>
    [JsonPropertyName("PARENT_HOME_ADDR_OTHER")]
    public string? ParentLive_Other { get; set; }

    /// <summary>
    /// 學歷
    /// 1. 博士
    /// 2. 碩士
    /// 3. 大學
    /// 4. 專科
    /// 5. 高中高職
    /// 6. 其他
    /// </summary>
    [JsonPropertyName("EDUCATION")]
    public string? Education { get; set; }

    /// <summary>
    /// 戶籍電話
    /// </summary>
    [JsonPropertyName("REG_TEL_NO")]
    public string? HouseRegPhone { get; set; }

    /// <summary>
    /// 居住電話
    /// </summary>
    [JsonPropertyName("HOME_TEL_NO")]
    public string? LivePhone { get; set; }

    /// <summary>
    /// 居住地所有權人(住所狀態)
    /// 1. 本人
    /// 2. 配偶
    /// 3. 父母親
    /// 4. 親屬
    /// 5. 宿舍
    /// 6. 租貸
    /// 7. 其他
    /// </summary>
    [JsonPropertyName("HOME_ADDR_COND")]
    public string? LiveOwner { get; set; }

    /// <summary>
    /// 畢業國小名稱
    /// </summary>
    [JsonPropertyName("PRIMARY_SCHOOL")]
    public string? GraduatedElementarySchool { get; set; }

    /// <summary>
    /// 統一編號
    /// </summary>
    [JsonPropertyName("COMP_ID")]
    public string? CompID { get; set; }

    /// <summary>
    /// 職稱
    /// 如為中文，最長15碼
    /// 如為英文，最長30碼
    /// </summary>
    [JsonPropertyName("JOB_TITLE")]
    public string? CompJobTitle { get; set; }

    /// <summary>
    /// 年資
    /// </summary>
    [JsonPropertyName("JOB_YEAR")]
    public string? CompSeniority { get; set; }

    /// <summary>
    /// 安麗直銷商/會員編號(限安麗卡)
    /// </summary>
    [JsonPropertyName("AL_NO")]
    public string? AnliNo { get; set; }

    /// <summary>
    /// 附件檔名1
    /// 空值，上述申請書檔名對應到後，ApplyFile.idPic1二進位資料即為附件1檔案。
    /// </summary>
    [JsonPropertyName("APPENDIX_FILE_NAME_01")]
    public string? AppendixFileName_01 { get; set; }

    /// <summary>
    /// 附件檔名2
    /// 空值，上述申請書檔名對應到後，ApplyFile.idPic2二進位資料即為附件2檔案。
    /// </summary>
    [JsonPropertyName("APPENDIX_FILE_NAME_02")]
    public string? AppendixFileName_02 { get; set; }

    /// <summary>
    /// 附件檔名3
    /// 空值，上述申請書檔名對應到後，ApplyFile.upload1二進位資料即為附件3檔案。
    /// </summary>
    [JsonPropertyName("APPENDIX_FILE_NAME_03")]
    public string? AppendixFileName_03 { get; set; }

    /// <summary>
    /// 附件檔名4
    /// 空值，上述申請書檔名對應到後，ApplyFile.upload2二進位資料即為附件4檔案。
    /// </summary>
    [JsonPropertyName("APPENDIX_FILE_NAME_04")]
    public string? AppendixFileName_04 { get; set; }

    /// <summary>
    /// 附件檔名5
    /// 空值，上述申請書檔名對應到後，ApplyFile.upload3二進位資料即為附件5檔案。
    /// </summary>
    [JsonPropertyName("APPENDIX_FILE_NAME_05")]
    public string? AppendixFileName_05 { get; set; }

    /// <summary>
    /// 附件檔名6
    /// 空值，上述申請書檔名對應到後，ApplyFile.upload4二進位資料即為附件6檔案。
    /// </summary>
    [JsonPropertyName("APPENDIX_FILE_NAME_06")]
    public string? AppendixFileName_06 { get; set; }

    /// <summary>
    /// 附件檔名7
    /// 空值，上述申請書檔名對應到後，ApplyFile.upload5二進位資料即為附件7檔案。
    /// </summary>
    [JsonPropertyName("APPENDIX_FILE_NAME_07")]
    public string? AppendixFileName_07 { get; set; }

    /// <summary>
    /// 附件檔名8
    /// 空值，上述申請書檔名對應到後，ApplyFile.upload6二進位資料即為附件8檔案。
    /// </summary>
    [JsonPropertyName("APPENDIX_FILE_NAME_08")]
    public string? AppendixFileName_08 { get; set; }
}
