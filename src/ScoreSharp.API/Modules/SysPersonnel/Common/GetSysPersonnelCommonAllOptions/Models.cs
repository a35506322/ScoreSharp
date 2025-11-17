using ScoreSharp.API.Modules.Reviewer.Helpers.Models;

namespace ScoreSharp.API.Modules.SysPersonnel.Common.GetSysPersonnelCommonAllOptions;

public class GetSysPersonnelCommonAllOptionsResponse
{
    /// <summary>
    /// 檢核狀態
    /// </summary>
    public List<OptionsDtoTypeInt> CaseCheckStatus { get; set; }

    /// <summary>
    /// 檢驗完畢狀態
    /// </summary>
    public List<OptionsDtoTypeInt> CaseCheckedStatus { get; set; }
}
