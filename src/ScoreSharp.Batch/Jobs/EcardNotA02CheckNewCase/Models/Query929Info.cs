namespace ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase.Models;

public class Query929Info : BaseResDto
{
    /// <summary>
    /// 929業務狀況查詢結果
    /// </summary>
    public List<Reviewer3rd_929Log> Reviewer3rd_929Logs { get; set; } = [];
    public string 是否命中 => Reviewer3rd_929Logs.Any() ? "Y" : "N";
}
