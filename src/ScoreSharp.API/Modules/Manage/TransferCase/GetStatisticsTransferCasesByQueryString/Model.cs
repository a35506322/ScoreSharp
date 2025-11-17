namespace ScoreSharp.API.Modules.Manage.TransferCase.GetStatisticsTransferCasesByQueryString;

public class GetStatisticsTransferCasesByQueryStringResponse
{
    /// <summary>
    /// 使用者帳號
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// 使用者姓名
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 案件總計
    /// </summary>
    public int TransferCasesTotalCount { get; set; }
    public List<TransferCasesDto> TransferCases { get; set; }
}

public class TransferCasesDto
{
    /// <summary>
    /// 調撥案件類型
    /// </summary>
    public TransferCaseType TransferCaseType { get; set; }

    /// <summary>
    /// 調撥案件類型名稱
    /// </summary>
    public string TransferCaseTypeName => TransferCaseType.ToString();

    /// <summary>
    /// 案件數量
    /// </summary>
    /// <value></value>
    public int CaseCount { get; set; }
}
