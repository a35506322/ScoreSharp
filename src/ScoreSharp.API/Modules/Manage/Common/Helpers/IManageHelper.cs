using ScoreSharp.API.Modules.Manage.Common.Helpers.Models;

namespace ScoreSharp.API.Modules.Manage.Common.Helpers;

public interface IManageHelper
{
    /// <summary>
    /// 取得未分派案件基礎資料
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>未分派案件基礎資料列表</returns>
    Task<List<UnassignedCaseBaseData>> GetUnassignedCaseBaseDataAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 與推廣人員是否相同
    /// </summary>
    /// <param name="promotionUserId">推廣人員員編</param>
    /// <param name="assignedUserId"></param>
    /// <param name="employeeNo"></param>
    /// <returns></returns>
    bool 與推廣人員是否相同(string promotionUserId, string assignedUserId, string? employeeNo);

    /// <summary>
    /// 與月收入確認人員是否相同
    /// </summary>
    /// <param name="monthlyIncomeCheckUserId">月收入確認人員</param>
    /// <param name="assignedUserId"></param>
    /// <returns></returns>
    bool 與月收入確認人員是否相同(HashSet<string> monthlyIncomeCheckUserId, string assignedUserId);

    /// <summary>
    /// 檢核是否為利害關係人
    /// </summary>
    /// <param name="stakeholderLookup">利害關係人</param>
    /// <param name="idSet">身分證集合 (使用 HashSet 以提升查詢效能)</param>
    /// <param name="assignedUserId"></param>
    /// <param name="employeeNo"></param>
    /// <returns></returns>
    bool 檢核利害關係人(ILookup<string, Reviewer_Stakeholder> stakeholderLookup, HashSet<string> idSet, string assignedUserId, string? employeeNo);
}
