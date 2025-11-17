namespace ScoreSharp.API.Modules.Manage.ObligedAssignment.GetObligedAssignmentByQueryString;

public class GetObligedAssignmentByQueryStringRequest : IValidatableObject
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    [Display(Name = "申請書編號")]
    public string? ApplyNo { get; set; }

    /// <summary>
    /// 身分證字號
    /// </summary>
    [Display(Name = "身分證字號")]
    public string? ID { get; set; }

    /// <summary>
    /// 案件狀態
    /// </summary>
    /// <remarks>
    /// 目前開放的卡片狀態碼如下：
    /// - 製卡失敗 (10302)
    /// </remarks>
    [Display(Name = "案件狀態")]
    [ValidEnumValue]
    public CardStatus? CardStatus { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        int count = 0;
        if (!string.IsNullOrWhiteSpace(ApplyNo))
            count += 1;
        if (!string.IsNullOrWhiteSpace(ID))
            count += 1;
        if (CardStatus.HasValue)
            count += 1;

        if (count == 0)
        {
            yield return new ValidationResult(
                "必須指定其中一個條件：申請書編號、身分證字號 或 案件狀態。",
                new[] { nameof(ApplyNo), nameof(ID), nameof(CardStatus) }
            );
        }

        if (count > 1)
        {
            yield return new ValidationResult(
                "申請書編號、身分證字號 或 案件狀態 只能擇一指定。",
                new[] { nameof(ApplyNo), nameof(ID), nameof(CardStatus) }
            );
        }
    }
}

public class GetObligedAssignmentByQueryStringResponse
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    [Display(Name = "申請書編號")]
    public string ApplyNo { get; set; }

    /// <summary>
    /// 身分證字號
    /// </summary>
    [Display(Name = "身分證字號")]
    public string ID { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    [Display(Name = "姓名")]
    public string CHName { get; set; }

    /// <summary>
    /// 正附卡
    /// </summary>
    [Display(Name = "正附卡")]
    public CardOwner CardOwner { get; set; }

    /// <summary>
    /// 正附卡名稱
    /// </summary>
    [Display(Name = "正附卡名稱")]
    public string CardOwnerName => this.CardOwner.ToString();

    /// <summary>
    /// 申請卡別資料
    /// </summary>
    /// <returns></returns>
    [Display(Name = "申請卡別資料")]
    public List<ApplyCardListDto> ApplyCardList { get; set; } = new();

    /// <summary>
    /// 核准人員
    /// </summary>
    [Display(Name = "申請書編號")]
    public List<ApproveUserDto> ApproveUserList { get; set; } = new();

}

public class ApplyCardTypeDto
{
    /// <summary>
    /// 申請卡別：以 " / "串接，如JA00/JC00
    /// </summary>
    [Display(Name = "申請卡別")]
    public string ApplyCardType { get; set; }
    public string ApplyCardTypeName { get; set; }
}

public class CardStatusDto
{
    /// <summary>
    /// 卡片狀態，查看附件-卡片狀態碼
    /// </summary>
    [Display(Name = "卡片狀態")]
    public CardStatus CardStatus { get; set; }

    public string CardStatusName => CardStatus.ToString();
}

public class ApproveUserDto
{
    /// <summary>
    /// 核准人員ID
    /// </summary>
    [Display(Name = "核准人員ID")]
    public string? ApproveUserId { get; set; }

    /// <summary>
    /// 核准人員姓名
    /// </summary>
    [Display(Name = "核准人員姓名")]
    public string? ApproveUserName { get; set; }

    /// <summary>
    /// 核准時間
    /// </summary>
    [Display(Name = "核准時間")]
    public DateTime? ApproveTime { get; set; }
}

public class ApplyCardListDto
{
    /// <summary>
    /// 卡片處理序號
    /// </summary>
    public string HandleSeqNo { get; set; }

    /// <summary>
    /// 卡片階段
    /// </summary>
    public CardStep? CardStep { get; set; }

    /// <summary>
    /// 卡片階段名稱
    /// </summary>
    public string CardStepName => CardStep is not null ? CardStep.ToString() : string.Empty;

    /// <summary>
    /// 正附卡
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 正附卡名稱
    /// </summary>
    public string UserTypeName => UserType.ToString();

    /// <summary>
    /// 申請卡別代碼
    /// </summary>
    public string ApplyCardType { get; set; }

    /// <summary>
    /// 申請卡別名稱
    /// </summary>
    public string ApplyCardName { get; set; }

    /// <summary>
    /// 該卡片的狀態
    /// </summary>
    public CardStatus CardStatus { get; set; }

    /// <summary>
    /// 卡片狀態名稱
    /// </summary>
    public string CardStatusName => CardStatus.ToString();

    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; }
}
