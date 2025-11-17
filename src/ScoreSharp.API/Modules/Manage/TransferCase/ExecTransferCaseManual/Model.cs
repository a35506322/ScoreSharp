namespace ScoreSharp.API.Modules.Manage.TransferCase.ExecTransferCaseManual;

public class ExecTransferCaseManualRequest
{
    [Required]
    [Display(Name = "處理人員")]
    public string HandleUserId { get; set; }

    [Required]
    [Display(Name = "調撥人員")]
    public string TransferredUserId { get; set; }

    [Required]
    [Display(Name = "申請書編號")]
    public List<string> ApplyNos { get; set; }
}

public class TransferCaseInfo
{
    public string ApplyNo { get; set; }
    public string PromotionUserId { get; set; }
    public string MonthlyIncomeCheckUserId { get; set; }
    public string M_ID { get; set; }
    public string S1_ID { get; set; }
    public Source Source { get; set; }
    public List<ApplyCardDto> ApplyCardList { get; set; } = [];
}

public class TransferUserDto
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string EmployeeNo { get; set; }
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

public class TransferCaseResult
{
    public string ApplyNo { get; set; }

    public string UserId { get; set; }

    public string UserName { get; set; }

    public List<ApplyCardDto> ApplyCardList { get; set; } = [];

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
    public bool IsTransferred => string.IsNullOrEmpty(FilterReason) && string.IsNullOrEmpty(ExceptionMessage);
}

public class TransferCaseExcelDto
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    [ExcelColumnName("申請書編號")]
    public string ApplyNo { get; set; }

    /// <summary>
    /// 是否調撥
    /// </summary>
    [ExcelColumnName("是否調撥")]
    public string IsTransferred { get; set; }

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

public class ExecTransferCaseManualResponse
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

public class ApplyCardDto
{
    public string ApplyCardType { get; set; }
    public CardStatus CardStatus { get; set; }
}
