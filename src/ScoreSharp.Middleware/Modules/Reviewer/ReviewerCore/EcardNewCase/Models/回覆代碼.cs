namespace ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore.EcardNewCase.Models;

public static class 回覆代碼
{
    public static readonly string 匯入成功 = "0000";
    public static readonly string 申請書編號長度不符 = "0001";
    public static readonly string 申請書編號重複進件或申請書編號不對 = "0003";
    public static readonly string 無法對應卡片代碼 = "0004";
    public static readonly string 資料異常非定義值 = "0005";
    public static readonly string 資料異常資料長度過長 = "0006";
    public static readonly string 必要欄位不能為空值 = "0007";
    public static readonly string 申請書異常 = "0008";
    public static readonly string 附件異常 = "0009";
    public static readonly string UUID重複 = "0010";
    public static readonly string 其它異常訊息 = "0012";

    // 新增
    public static readonly string ECARD_FILE_DB_連線錯誤 = "0013";
    public static readonly string 查無申請書附件檔案 = "0014";
}
