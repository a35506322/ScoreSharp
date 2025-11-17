namespace ScoreSharp.API.Modules.SetUp.ApplicationCategory.GetApplicationCategoriesByQueryString;

public class GetApplicationCategoriesByQueryStringRequest
{
    /// <summary>
    /// 申請書類別名稱
    /// </summary>
    [Display(Name = "申請書類別名稱")]
    public string? ApplicationCategoryName { get; set; } = null!;

    /// <summary>
    /// 是否為OCR表單，Y | N
    /// </summary>
    [Display(Name = "是否為OCR表單")]
    [RegularExpression("[YN]")]
    public string? IsOCRForm { get; set; } = null!;

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    public string? IsActive { get; set; } = null!;

    /// <summary>
    /// 申請書類別代碼
    /// </summary>
    [Display(Name = "申請書類別代碼")]
    public string? ApplicationCategoryCode { get; set; } = null!;
}

public class GetApplicationCategoriesByQueryStringResponse
{
    /// <summary>
    /// 申請書類別代碼，範例: AF00002
    /// </summary>
    public string ApplicationCategoryCode { get; set; } = null!;

    /// <summary>
    /// 申請書類別名稱
    /// </summary>
    public string ApplicationCategoryName { get; set; } = null!;

    /// <summary>
    /// 是否為OCR表單，Y | N
    /// </summary>
    public string IsOCRForm { get; set; } = null!;

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    public string IsActive { get; set; } = null!;

    /// <summary>
    /// 新增員工
    /// </summary>
    public string AddUserId { get; set; } = null!;

    /// <summary>
    /// 新增時間
    /// </summary>
    public DateTime AddTime { get; set; }

    /// <summary>
    /// 修正員工
    /// </summary>
    public string? UpdateUserId { get; set; }

    /// <summary>
    /// 修正時間
    /// </summary>
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// 修正時間
    /// </summary>
    public List<CardInfoDto> CardInfo { get; set; } = new List<CardInfoDto>();
}

public class CardInfoDto
{
    public string BINCode { get; set; }
    public string CardCode { get; set; }
    public string CardName { get; set; }
}
