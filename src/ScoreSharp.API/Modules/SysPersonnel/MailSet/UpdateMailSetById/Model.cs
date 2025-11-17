namespace ScoreSharp.API.Modules.SysPersonnel.MailSet.UpdateMailSetById;

public class UpdateMailSetByIdRequest
{
    /// <summary>
    /// PK
    /// </summary>
    [Display(Name = "PK")]
    public int SeqNo { get; set; }

    /// <summary>
    /// 徵審錯誤_郵件樣板
    ///
    /// 範例 Email/EmailTemplate
    /// </summary>
    [Display(Name = "徵審錯誤_郵件樣板")]
    public string SystemErrorLog_Template { get; set; } = null!;

    /// <summary>
    /// 徵審錯誤_郵收件人
    ///
    /// 以逗點分割
    /// </summary>
    [Display(Name = "徵審錯誤_郵收件人")]
    public string SystemErrorLog_To { get; set; } = null!;

    /// <summary>
    /// 徵審錯誤_郵收件標題
    /// </summary>
    [Display(Name = "徵審錯誤_郵收件標題")]
    public string SystemErrorLog_Title { get; set; } = null!;

    /// <summary>
    /// 國旅卡客戶檢核失敗_郵件樣板
    /// </summary>
    [Display(Name = "國旅卡客戶檢核失敗_郵件樣板")]
    public string GuoLuKaCheckFailLog_Template { get; set; } = null!;

    /// <summary>
    /// 國旅卡客戶檢核失敗_收件人
    /// </summary>
    [Display(Name = "國旅卡客戶檢核失敗_收件人")]
    public string GuoLuKaCheckFailLog_To { get; set; } = null!;

    /// <summary>
    /// 國旅卡客戶檢核失敗_收件標題
    /// </summary>
    [Display(Name = "國旅卡客戶檢核失敗_收件標題")]
    public string GuoLuKaCheckFailLog_Title { get; set; } = null!;

    /// <summary>
    /// KYC錯誤_郵件樣板
    /// </summary>
    [Display(Name = "KYC錯誤_郵件樣板")]
    public string KYCErrorLog_Template { get; set; } = null!;

    /// <summary>
    /// KYC錯誤_收件人
    /// </summary>
    [Display(Name = "KYC錯誤_收件人")]
    public string KYCErrorLog_To { get; set; } = null!;

    /// <summary>
    /// KYC錯誤_收件標題
    /// </summary>
    [Display(Name = "KYC錯誤_收件標題")]
    public string KYCErrorLog_Title { get; set; } = null!;
}
