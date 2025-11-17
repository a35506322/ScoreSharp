namespace ScoreSharp.API.Modules.Reviewer.Helpers.Models;

public class ApplyCreditCardInfoParamsDto
{
    /// <summary>
    /// AML職級別
    /// </summary>
    public List<OptionsDtoTypeString> AMLJobLevel { get; set; }

    /// <summary>
    /// AML職業別
    /// </summary>
    public List<OptionsDtoTypeString> AMLProfession { get; set; }

    /// <summary>
    /// 卡片種類
    /// </summary>
    public List<OptionsDtoTypeString> Card { get; set; }

    /// <summary>
    /// 國籍
    /// </summary>
    public List<OptionsDtoTypeString> Citizenship { get; set; }

    /// <summary>
    /// 徵信代碼
    /// </summary>
    public List<OptionsDtoTypeString> CreditCheckCode { get; set; }

    /// <summary>
    /// 身分證發證地點
    /// </summary>
    public List<OptionsDtoTypeString> IDCardRenewalLocation { get; set; }

    /// <summary>
    /// 主要所得來源
    /// </summary>
    public List<OptionsDtoTypeString> MainIncomeAndFund { get; set; }

    /// <summary>
    /// 專案代碼
    /// </summary>
    public List<OptionsDtoTypeString> ProjectCode { get; set; }

    /// <summary>
    /// 補件原因
    /// </summary>
    public List<OptionsDtoTypeString> SupplementReason { get; set; }

    /// <summary>
    /// 退件原因
    /// </summary>
    public List<OptionsDtoTypeString> RejectionReason { get; set; }

    /// <summary>
    /// 卡片代碼
    /// </summary>
    public List<OptionsDtoTypeString> Card_BinCode { get; set; }

    /// <summary>
    /// 年費收取方式
    /// </summary>
    public List<OptionsDtoTypeString> AnnualFeeCollectionMethod { get; set; }
}
