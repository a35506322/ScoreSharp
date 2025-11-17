namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetAllApplyCreditCardListByQueryString;

public class GetAllApplyCreditCardListByQueryStringRequest : IValidatableObject
{
    /// <summary>
    /// 申請書編號：範例20180625A0001
    /// </summary>
    [Display(Name = "申請書編號")]
    public string? ApplyNo { get; set; }

    /// <summary>
    /// 中文姓名
    /// </summary>
    [Display(Name = "中文姓名")]
    public string? CHName { get; set; }

    /// <summary>
    /// 身份證字號
    /// </summary>
    [Display(Name = "身份證字號")]
    public string? ID { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var errors = new List<ValidationResult>();

        // 檢查是否所有欄位都為空
        if (string.IsNullOrWhiteSpace(ApplyNo) && string.IsNullOrWhiteSpace(CHName) && string.IsNullOrWhiteSpace(ID))
        {
            errors.Add(new ValidationResult("申請書編號、中文姓名、身份證字號至少需要一個", new[] { nameof(ApplyNo), nameof(CHName), nameof(ID) }));
        }
        else
        {
            // 計算已填寫的欄位數量
            var filledFields = new[]
            {
                !string.IsNullOrWhiteSpace(ApplyNo),
                !string.IsNullOrWhiteSpace(CHName),
                !string.IsNullOrWhiteSpace(ID),
            }.Count(x => x);

            if (filledFields > 1)
            {
                errors.Add(
                    new ValidationResult("申請書編號、中文姓名、身份證字號只能輸入一個條件", new[] { nameof(ApplyNo), nameof(CHName), nameof(ID) })
                );
            }
        }

        return errors;
    }
}

public class GetAllApplyCreditCardListByQueryStringResponse
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; }

    /// <summary>
    /// 中文姓名
    /// </summary>
    public string CHName { get; set; }

    /// <summary>
    /// 身份證字號
    /// </summary>
    public string ID { get; set; }

    /// <summary>
    /// 案件類型
    /// </summary>
    public CaseType? CaseType { get; set; }

    /// <summary>
    /// 案件類型名稱
    /// </summary>
    public string CaseTypeName => this.CaseType.ToString() ?? string.Empty;

    /// <summary>
    /// 急件備註
    /// </summary>
    public string UrgentNotes => this.CaseType.HasValue && this.CaseType.Value == ScoreSharp.Common.Enums.CaseType.急件 ? "是" : "否";

    /// <summary>
    /// 申請日期
    /// </summary>
    public DateTime ApplyDate { get; set; }

    /// <summary>
    /// 目前處理人員
    /// </summary>
    public string? CurrentHandleUserId { get; set; }

    /// <summary>
    /// 目前處理人員名稱
    /// </summary>
    public string? CurrentHandleUserName { get; set; }

    /// <summary>
    /// 申請卡片類型
    /// </summary>
    public List<ApplyCardTypeDto> ApplyCardTypeList { get; set; }

    /// <summary>
    /// 申請卡片類型
    /// </summary>
    public string ApplyCardTypeName { get; set; }

    /// <summary>
    /// 卡片狀態
    /// </summary>
    public List<CardStatusDto> CardStatusList { get; set; }

    /// <summary>
    /// 卡片狀態
    /// </summary>
    public string CardStatusName { get; set; }

    /// <summary>
    /// 是否查詢詳細資料，當為N時，不能點選詳細資料
    /// </summary>
    public string IsQuery { get; set; }
}

public class ApplyCardTypeDto
{
    /// <summary>
    /// 申請卡片類型
    /// </summary>
    public string ApplyCardType { get; set; }

    /// <summary>
    /// 申請卡片類型名稱
    /// </summary>
    public string ApplyCardTypeName { get; set; }
}

public class CardStatusDto
{
    public CardStatus CardStatus { get; set; }
    public string CardStatusName { get; set; }
}

public class CardDto
{
    public string CardCode { get; set; }
    public string CardName { get; set; }
}

public class UserDto
{
    public string UserId { get; set; }
    public string UserName { get; set; }
}

public class MainDto
{
    public string ApplyNo { get; set; }
    public string CHName { get; set; }
    public string ID { get; set; }
    public string CurrentHandleUserId { get; set; }
    public CaseType CaseType { get; set; }
    public DateTime ApplyDate { get; set; }
}

public class HandleDto
{
    public string ApplyNo { get; set; }
    public string ID { get; set; }
    public string ApplyCardType { get; set; }
    public CardStatus CardStatus { get; set; }
    public UserType UserType { get; set; }
}
