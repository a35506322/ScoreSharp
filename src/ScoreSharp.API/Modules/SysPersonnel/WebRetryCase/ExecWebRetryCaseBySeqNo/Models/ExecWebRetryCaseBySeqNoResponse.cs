namespace ScoreSharp.API.Modules.SysPersonnel.WebRetryCase.ExecWebRetryCaseBySeqNo.Models;

public class ExecWebRetryCaseBySeqNoResponse
{
    /// <summary>
    /// PK
    /// </summary>
    [DisplayName("PK")]
    public long SeqNo { get; set; }

    public string? ReturnCode { get; set; }

    public string? ReturnCodeMessage { get; set; }
}
