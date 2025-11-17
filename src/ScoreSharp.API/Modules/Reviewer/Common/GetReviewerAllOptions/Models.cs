using ScoreSharp.API.Modules.Reviewer.Helpers.Models;

namespace ScoreSharp.API.Modules.Reviewer.Common.GetReviewerAllOptions;

public class GetReviewerAllOptionsResponse
{
    /// <summary>
    /// AML職級別
    /// </summary>
    public List<OptionsDtoTypeString> AMLJobLevel { get; set; }

    /// <summary>
    /// AML職業別
    /// </summary>
    public List<AmlProfessionVersionDto> AMLProfession { get; set; }

    /// <summary>
    /// 卡片種類
    /// </summary>
    public List<OptionsDtoTypeString> Card { get; set; }

    /// <summary>
    /// 國籍
    /// </summary>
    public List<OptionsDtoTypeString> Citizenship { get; init; }

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
    /// 性別
    /// </summary>
    public List<OptionsDtoTypeInt> Sex { get; set; }

    /// <summary>
    /// 婚姻狀況
    /// </summary>
    public List<OptionsDtoTypeInt> MarriageState { get; init; }

    /// <summary>
    /// 學歷
    /// </summary>
    public List<OptionsDtoTypeInt> Education { get; set; }

    /// <summary>
    /// 居住地所有權人
    /// </summary>
    public List<OptionsDtoTypeInt> LiveOwner { get; set; }

    /// <summary>
    /// 身分證換發補領狀態
    /// </summary>
    public List<OptionsDtoTypeInt> IDTakeStatus { get; set; }

    /// <summary>
    /// 出生地國籍
    /// </summary>
    public List<OptionsDtoTypeInt> BirthCitizenshipCode { get; set; }

    /// <summary>
    /// 學生與申請人關係
    /// </summary>
    public List<OptionsDtoTypeInt> StudentApplicantRelationship { get; set; }

    /// <summary>
    /// 擔任PEP期間
    /// </summary>
    public List<OptionsDtoTypeInt> PEPRange { get; set; }

    /// <summary>
    /// 卸任前為何種PEP
    /// </summary>
    public List<OptionsDtoTypeInt> ResignPEPKind { get; set; }

    /// <summary>
    /// 公司職級別
    /// </summary>
    public List<OptionsDtoTypeInt> CompJobLevel { get; set; }

    /// <summary>
    /// 公司行業別
    /// </summary>
    public List<OptionsDtoTypeInt> CompTrade { get; set; }

    /// <summary>
    /// 姓名檢核理由碼
    /// </summary>
    public List<OptionsDtoTypeInt> NameCheckedReasonCode { get; set; }

    /// <summary>
    /// 帳單形式
    /// </summary>
    public List<OptionsDtoTypeInt> BillType { get; set; }

    /// <summary>
    /// 進件方式
    /// </summary>
    public List<OptionsDtoTypeInt> AcceptType { get; set; }

    /// <summary>
    /// 寄卡地址類型
    /// </summary>
    public List<OptionsDtoTypeInt> SendCardAddressType { get; set; }

    /// <summary>
    /// 帳單地址類型
    /// </summary>
    public List<OptionsDtoTypeInt> BillAddressType { get; set; }

    /// <summary>
    /// 居住地址類型
    /// </summary>
    public List<OptionsDtoTypeInt> LiveAddressType { get; set; }

    /// <summary>
    /// 家長居住地址類型
    /// </summary>
    public List<OptionsDtoTypeInt> ParentLiveAddressType { get; set; }

    /// <summary>
    /// 補件原因
    /// </summary>
    public List<OptionsDtoTypeString> SupplementReason { get; set; }

    /// <summary>
    /// 退件原因
    /// </summary>
    public List<OptionsDtoTypeString> RejectionReason { get; set; }

    /// <summary>
    /// 月收入確認動作
    /// </summary>
    public List<OptionsDtoTypeInt> IncomeConfirmationAction { get; set; }

    /// <summary>
    /// 案件狀態
    /// </summary>
    public List<OptionsDtoTypeInt> CardStatus { get; set; }

    /// <summary>
    /// 補聯繫紀錄_補件類別
    /// </summary>
    public List<OptionsDtoTypeInt> SupplementContactRecordsType { get; set; }

    /// <summary>
    /// 補聯繫紀錄_聯繫結果
    /// </summary>
    public List<OptionsDtoTypeInt> SupplementContactRecordsResult { get; set; }

    /// <summary>
    /// 人工徵審權限內_徵審動作
    /// </summary>
    public List<OptionsDtoTypeInt> ManualReviewAction_AuthIn { get; set; }

    /// <summary>
    /// 人工徵審權限外_徵審動作
    /// </summary>
    public List<OptionsDtoTypeInt> ManualReviewAction_AuthOut { get; set; }

    /// <summary>
    /// 與正卡人關係
    /// </summary>
    public List<OptionsDtoTypeInt> ApplicantRelationship { get; set; }

    /// <summary>
    /// 附卡人寄卡地址類型
    /// </summary>
    public List<OptionsDtoTypeInt> ShippingCardAddressType { get; set; }

    /// <summary>
    /// 附卡人居住地址類型
    /// </summary>
    public List<OptionsDtoTypeInt> ResidenceType { get; set; }

    /// <summary>
    /// 卡片代碼
    /// </summary>
    public List<OptionsDtoTypeString> Card_BinCode { get; set; }

    /// <summary>
    /// 年費收取方式
    /// </summary>
    public List<OptionsDtoTypeString> AnnualFeeCollectionMethod { get; set; }

    /// <summary>
    /// 確認關係
    /// </summary>
    public List<OptionsDtoTypeInt> SameDataRelation { get; set; }
}

public class AmlProfessionVersionDto
{
    /// <summary>
    /// 版本日期
    /// 例如：20220101
    /// </summary>
    public string Version { get; set; }

    /// <summary>
    /// 此版本對應的職業選項
    /// </summary>
    public List<OptionsDtoTypeString> ProfessionOptions { get; set; }
}
