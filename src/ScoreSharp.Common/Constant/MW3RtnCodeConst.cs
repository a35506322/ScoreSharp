namespace ScoreSharp.Common.Constant;

public static class MW3RtnCodeConst
{
    public const string 成功 = "0000";

    // 929
    public const string 查詢929_查無資料 = "8888";
    public const string 查詢929_交易有誤 = "9999";
    public const string 查詢929_聯絡系統管理員 = "Er01";
    public const string 查詢929_傳入規格不符合 = "Er02";
    public const string 查詢929_此服務已失效 = "Er03";

    // 告誡
    public const string 告誡_查無資料 = "9998";
    public const string 告誡_撤銷 = "9901";
    public const string 告誡_日期已過 = "9902";

    // 分行資訊
    public const string 查詢分行資訊_查無資料 = "8888";
    public const string 查詢分行資訊_聯絡系統管理員 = "Er01";
    public const string 查詢分行資訊_傳入規格不符合 = "Er02";
    public const string 查詢分行資訊_此服務已失效 = "Er03";

    // 信用卡資訊
    public const string 查詢信用卡資訊_查無資料 = "F103";

    // 關注名單
    public const string 查詢關注名單_查詢A關注名單失敗 = "9901";
    public const string 查詢關注名單_查詢BCDEFHI關注名單失敗 = "9902";
    public const string 查詢關注名單_查詢G關注名單失敗 = "9903";
    public const string 查詢關注名單_系統錯誤 = "9999";

    // eCardService
    public const string 查詢eCardService_作業成功 = "0000";
    public const string 查詢eCardService_無效的申請書編號 = "9100";
    public const string 查詢eCardService_eCardService系統錯誤 = "9099";
    public const string 查詢eCardService_eCardService資料庫錯誤 = "9098";

    // 姓名檢核
    public const string 查詢姓名檢核_命中 = "Y";
    public const string 查詢姓名檢核_未命中 = "N";

    // 國旅卡客戶資訊
    public const string 查詢國旅卡客戶資訊_正確資料 = "Y";
    public const string 查詢國旅卡客戶資訊_非簽約機關 = "D";
    public const string 查詢國旅卡客戶資訊_查無資料 = "N";
    public const string 查詢國旅卡客戶資訊_請聯絡系統管理員 = "Er01";
    public const string 查詢國旅卡客戶資訊_傳入規格不符合 = "Er02";
    public const string 查詢國旅卡客戶資訊_此服務已失效 = "Er03";

    // 原持卡人資料
    public const string 查詢原持卡人資料_查無資料 = "9001";

    // 入檔KYC
    public const string 入檔KYC_成功 = "K0000";
    public const string 入檔KYC_JSON格式錯誤_KD001 = "KD001";
    public const string 入檔KYC_JSON欄位有缺 = "KD002";
    public const string 入檔KYC_資料驗證失敗 = "KD003";
    public const string 入檔KYC_客戶資料時效驗證失敗 = "KD004";
    public const string 入檔KYC_KYC資料編輯中簽核中 = "KD005";
    public const string 入檔KYC_RMD錯誤 = "KD006";
    public const string 入檔KYC_KYC客戶資料寫入失敗 = "KD007";
    public const string 入檔KYC_主機TimeOut = "KC001";
    public const string 入檔KYC_傳送主機時錯誤_KM001 = "KM001";
    public const string 入檔KYC_傳送主機時錯誤_KP001 = "KM001";
    public const string 入檔KYC_傳送主機時錯誤_KE001 = "KE001";
    public const string 入檔KYC_傳送主機時錯誤_K0001 = "K0001";
    public const string 入檔KYC_JSON格式錯誤_KD008 = "KD008";
    public const string 入檔KYC_出生日期不符不得強押覆蓋AMLKYC資料 = "KD009";
    public const string 入檔KYC_PEP欄位規則錯誤 = "KD010";

    // 建議核准KYC
    public const string 建議核准KYC_成功 = "K0000";

    // 簡單查詢風險等級
    public const string 簡單查詢風險等級_成功 = "K0000";

    // 電子帳單
    public const string 查詢電子帳單_該信箱已存在 = "0000";
    public const string 查詢電子帳單_無相同信箱會員 = "0001";
}
