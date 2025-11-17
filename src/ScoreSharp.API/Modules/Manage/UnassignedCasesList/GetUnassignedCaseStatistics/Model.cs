namespace ScoreSharp.API.Modules.Manage.UnassignedCasesList.GetUnassignedCaseStatistics;

public class GetUnassignedCaseStatisticsRequest
{
    // 這個 API 不需要請求參數，統計所有未分派案件
}

public class CaseStatisticItem
{
    /// <summary>
    /// 統計項目 ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 統計項目名稱
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 統計數量
    /// </summary>
    public int Value { get; set; }
}

public class GetUnassignedCaseStatisticsResponse : List<CaseStatisticItem> { }
