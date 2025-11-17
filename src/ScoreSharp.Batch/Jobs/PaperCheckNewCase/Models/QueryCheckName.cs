namespace ScoreSharp.Batch.Jobs.PaperCheckNewCase.Models;

public class QueryCheckName : BaseResDto
{
    /// <summary>
    /// 姓名檢核查詢結果
    /// </summary>
    public Reviewer3rd_NameCheckLog Reviewer3rd_NameCheckLog { get; set; } = new();
}
