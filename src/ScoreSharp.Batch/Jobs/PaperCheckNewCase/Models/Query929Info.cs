namespace ScoreSharp.Batch.Jobs.PaperCheckNewCase.Models;

public class Query929Info : BaseResDto
{
    /// <summary>
    /// 929業務狀況查詢結果
    /// </summary>
    public List<Reviewer3rd_929Log> Reviewer3rd_929Logs { get; set; } = [];
}
