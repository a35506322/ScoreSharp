namespace ScoreSharp.API.Modules.Manage.UnassignedCasesList.BatchAssignCasesAutomatically;

public class BatchAssignCasesAutomaticallyRequest
{
    [Required]
    [Display(Name = "分案類型")]
    [ValidEnumValue]
    public CaseAssignmentType CaseAssignmentType { get; set; }

    [Required]
    [Display(Name = "分案列表")]
    [MinLength(1)]
    public List<AssignUserInfo> AssignCaseUserInfos { get; set; } = [];
}

public class AssignUserInfo
{
    [Required]
    [Display(Name = "分案人員")]
    public string AssignedUserId { get; set; }

    [Required]
    [Display(Name = "分案數量")]
    public int CaseCount { get; set; }
}

public class BatchAssignCasesAutomaticallyResponse
{
    /// <summary>
    /// 結果檔案
    /// </summary>
    [Display(Name = "結果檔案")]
    public byte[] ResultFile { get; set; } = [];

    /// <summary>
    /// 檔案名稱
    /// </summary>
    [Display(Name = "檔案名稱")]
    public string FileName { get; set; }

    /// <summary>
    /// 檔案格式
    /// </summary>
    [Display(Name = "檔案格式")]
    public string ContentType { get; set; }
}

public class QueryAssignmentUsersResult
{
    /// <summary>
    /// 使用者
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// 使用者姓名
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 派案組織
    /// </summary>
    public string CaseDispatchGroup { get; set; } = string.Empty;

    /// <summary>
    /// 一般件預審分案
    /// </summary>
    public string IsPaperCase { get; set; } = string.Empty;

    /// <summary>
    /// 一般件預審比重排序
    /// </summary>
    public int PaperCaseSort { get; set; }

    /// <summary>
    /// 快辦件預審分案
    /// </summary>
    public string IsWebCase { get; set; } = string.Empty;

    /// <summary>
    /// 快辦件預審比重排序
    /// </summary>
    public int WebCaseSort { get; set; }

    /// <summary>
    /// 人工徵信件預審分案
    /// </summary>
    public string IsManualCase { get; set; } = string.Empty;

    /// <summary>
    /// 人工徵信件預審比重排序
    /// </summary>
    public int ManualCaseSort { get; set; }
}

public class AssignCaseResult
{
    public string ApplyNo { get; set; }

    public string UserId { get; set; }

    public string UserName { get; set; }

    /// <summary>
    /// 篩選原因
    /// </summary>
    public string FilterReason { get; set; }

    /// <summary>
    /// 例外訊息
    /// </summary>
    public string ExceptionMessage { get; set; }

    /// <summary>
    /// 是否派案成功
    /// </summary>
    public bool IsAssigned => string.IsNullOrEmpty(FilterReason) && string.IsNullOrEmpty(ExceptionMessage);
}

public class AssignCasesInfo
{
    public string ApplyNo { get; set; }
    public string PromotionUserId { get; set; }
    public string MonthlyIncomeCheckUserId { get; set; }
    public string M_ID { get; set; }
    public string S1_ID { get; set; }
    public string AssignedUserId { get; set; }
    public string M_NameChecked { get; set; }
    public string S1_NameChecked { get; set; }
    public bool HasNameCheckedY => M_NameChecked == "Y" || S1_NameChecked == "Y";
}

public class AssignedUserDto
{
    /// <summary>
    /// 使用者
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// 使用者姓名
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 派案組織
    /// </summary>
    public string CaseDispatchGroup { get; set; } = string.Empty;

    /// <summary>
    /// 一般件預審分案
    /// </summary>
    public string IsPaperCase { get; set; } = string.Empty;

    /// <summary>
    /// 一般件預審比重排序
    /// </summary>
    public int PaperCaseSort { get; set; }

    /// <summary>
    /// 快辦件預審分案
    /// </summary>
    public string IsWebCase { get; set; } = string.Empty;

    /// <summary>
    /// 快辦件預審比重排序
    /// </summary>
    public int WebCaseSort { get; set; }

    /// <summary>
    /// 人工徵信件預審分案
    /// </summary>
    public string IsManualCase { get; set; } = string.Empty;

    /// <summary>
    /// 人工徵信件預審比重排序
    /// </summary>
    public int ManualCaseSort { get; set; }

    /// <summary>
    /// 員工編號
    /// </summary>
    public string? EmployeeNo { get; set; }

    /// <summary>
    /// 分案數量
    /// </summary>
    public int CaseCount { get; set; }
}

public class BatchAssignCasesAutomaticallyExcelDto
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    [ExcelColumnName("申請書編號")]
    public string ApplyNo { get; set; }

    /// <summary>
    /// 是否派案
    /// </summary>
    [ExcelColumnName("是否派案")]
    public string IsAssigned { get; set; }

    /// <summary>
    /// 篩選原因
    /// </summary>
    [ExcelColumnName("篩選原因")]
    public string FilterReason { get; set; }

    /// <summary>
    /// 例外訊息
    /// </summary>
    [ExcelColumnName("例外訊息")]
    public string ExceptionMessage { get; set; }
}
