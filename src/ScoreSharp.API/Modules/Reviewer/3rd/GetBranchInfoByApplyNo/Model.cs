namespace ScoreSharp.API.Modules.Reviewer3rd.GetBranchInfoByApplyNo;

public class QueryBranchInfoDto
{
    /// <summary>
    /// 客戶資訊
    /// </summary>
    /// <value></value>
    public List<Reviewer3rd_BranchCusCusInfo> BranchCusCusInfo { get; set; } = [];

    /// <summary>
    /// 財富管理戶
    /// </summary>
    public List<Reviewer3rd_BranchCusWMCust> BranchCusWMCust { get; set; } = [];

    /// <summary>
    /// 定存戶
    /// </summary>
    public List<Reviewer3rd_BranchCusCD> BranchCusCD { get; set; } = [];

    /// <summary>
    /// 活存戶
    /// </summary>
    public List<Reviewer3rd_BranchCusDD> BranchCusDD { get; set; } = [];

    /// <summary>
    /// 支票存款
    /// </summary>
    public List<Reviewer3rd_BranchCusCAD> BranchCusCAD { get; set; } = [];

    /// <summary>
    /// 授信逾期
    /// </summary>
    public List<Reviewer3rd_BranchCusCreditOver> BranchCusCreditOver { get; set; } = [];
}
