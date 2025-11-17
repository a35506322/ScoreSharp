using System.Linq;
using ScoreSharp.Common.Extenstions;

namespace ScoreSharp.API.Modules.Manage.UnassignedCasesList.GetUnassignedCasesByQueryString;

public class GetUnassignedCasesByQueryStringRequest
{
    /// <summary>
    /// 分案類型
    /// </summary>
    [Display(Name = "分案類型")]
    [Required]
    [ValidEnumValue]
    public CaseAssignmentType CaseAssignmentType { get; set; }

    /// <summary>
    /// 被派案人員
    /// </summary>
    [Display(Name = "被派案人員")]
    public string? AssignedUserId { get; set; }
}

public class GetUnassignedCasesByQueryStringResponse
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    [Display(Name = "申請書編號")]
    public string ApplyNo { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    [Display(Name = "姓名")]
    public string CHName { get; set; }

    /// <summary>
    /// 姓名檢核-正/附卡
    /// </summary>
    [Display(Name = "姓名檢核-正/附卡")]
    public List<NameCheckResultDto> NameCheckResultList { get; set; }

    /// <summary>
    /// 姓名檢核-正/附卡_字串
    /// </summary>
    [Display(Name = "姓名檢核-正/附卡_字串")]
    public string NameCheckResultName => string.Join("/", NameCheckResultList.OrderBy(x => x.UserType).Select(x => x.NameCheckResult));

    /// <summary>
    /// 身分證編號
    /// </summary>
    [Display(Name = "身分證編號")]
    public string ID { get; set; }

    /// <summary>
    /// 申請卡別
    /// </summary>
    [Display(Name = "申請卡別")]
    public List<ApplyCardTypeDto> ApplyCardTypeList { get; set; } = new();

    /// <summary>
    /// 申請卡別_字串
    /// </summary>
    [Display(Name = "申請卡別_字串")]
    public string ApplyCardTypeName => string.Join("/", ApplyCardTypeList.Select(x => $"({x.ApplyCardType}){x.ApplyCardTypeName}"));

    /// <summary>
    /// 案件狀態
    /// </summary>
    [Display(Name = "案件狀態")]
    public List<CardStatusDto> CardStatusList { get; set; } = new();

    /// <summary>
    /// 案件狀態_字串
    /// </summary>
    [Display(Name = "案件狀態_字串")]
    public string CardStatusName => string.Join("/", CardStatusList.Select(x => $"{x.CardStatusName}"));

    /// <summary>
    /// 重複進件
    /// </summary>
    [Display(Name = "重複進件")]
    public List<IsDuplicateSubmissionDto> IsDuplicateSubmissionList { get; set; }

    /// <summary>
    /// 重複進件_字串
    /// </summary>
    [Display(Name = "重複進件_字串")]
    public string IsDuplicateSubmissionName => string.Join("/", IsDuplicateSubmissionList.Select(x => x.IsDuplicateSubmission == "Y" ? "是" : "否"));

    /// <summary>
    /// 新戶
    /// </summary>
    [Display(Name = "新戶")]
    public List<IsNewAccountDto> IsNewAccountList { get; set; } = new();

    /// <summary>
    /// 新戶_字串
    /// </summary>
    [Display(Name = "新戶_字串")]
    public string IsNewAccountName => string.Join("/", IsNewAccountList.Select(x => $"{(x.IsNewAccount == "Y" ? "是" : "否")}"));

    /// <summary>
    /// 原持卡人
    /// </summary>
    [Display(Name = "原持卡人")]
    public List<IsOriginalCardholderDto> IsOriginalCardholderList { get; set; } = new();

    /// <summary>
    /// 原持卡人_字串
    /// </summary>
    [Display(Name = "原持卡人_字串")]
    public string IsOriginalCardholderName =>
        string.Join("/", IsOriginalCardholderList.Select(x => $"{(x.IsOriginalCardholder == "Y" ? "是" : "否")}"));

    /// <summary>
    /// 進件時間
    /// </summary>
    [Display(Name = "進件時間")]
    public DateTime ApplyDate { get; set; }

    /// <summary>
    /// 篩選原因
    /// </summary>
    [Display(Name = "篩選原因")]
    public string? FilterReason { get; set; }

    /// <summary>
    /// 案件種類
    /// </summary>
    public CaseType? CaseType { get; set; }

    /// <summary>
    /// 案件種類名稱
    /// </summary>
    public string CaseTypeName => CaseType is not null ? CaseType.ToString() : string.Empty;
}

public class IsDuplicateSubmissionDto
{
    public string IsDuplicateSubmission { get; set; }
    public UserType UserType { get; set; }
}

public class CardStatusDto
{
    public UserType UserType { get; set; }
    public CardStatus CardStatus { get; set; }

    public string CardStatusName => this.CardStatus.ToString();
}

public class ApplyCardTypeDto
{
    public UserType UserType { get; set; }
    public string ApplyCardType { get; set; }
    public string ApplyCardTypeName { get; set; }
}

public class IsOriginalCardholderDto
{
    /// <summary>
    /// 正/附卡
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 是否為原持卡人
    /// </summary>
    public string IsOriginalCardholder { get; set; }
}

public class IsNewAccountDto
{
    /// <summary>
    /// 正/附卡
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 是否為新戶
    /// </summary>
    public string IsNewAccount { get; set; }
}

public class NameCheckResultDto
{
    /// <summary>
    /// 正/附卡
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 是否為新戶
    /// </summary>
    public string NameCheckResult { get; set; }
}

public class ApplyInfoDto
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string CHName { get; set; }

    /// <summary>
    /// 身分證編號
    /// </summary>
    public string ID { get; set; }
    public string IsRepeatApply { get; set; }
    public string IsOriginalCardholder { get; set; }

    /// <summary>
    /// 進件時間
    /// </summary>
    public DateTime ApplyDate { get; set; }
    public string CurrentHandleUserId { get; set; }
    public UserType UserType { get; set; }
    public string NameChecked { get; set; }
}

public class HandleDto
{
    public string ApplyNo { get; set; }

    public string ID { get; set; }

    public UserType UserType { get; set; }

    public CardStatus CardStatus { get; set; }

    public string ApplyCardType { get; set; }
    public string MonthlyIncomeCheckUserId { get; set; }
}
