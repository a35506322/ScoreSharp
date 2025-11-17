namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateCaseOfCaseTypeAndCardStatusByApplyNo;

public class UpdateCaseOfCaseTypeAndCardStatusByApplyNoRequest
{
    /// <summary>
    /// 案件編號
    /// </summary>
    public string ApplyNo { get; set; }

    /// <summary>
    /// 更改案件狀態動作
    /// </summary>
    public CaseOfAction CaseOfAction { get; set; }
}
