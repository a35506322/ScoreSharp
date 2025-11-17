using ScoreSharp.API.Modules.Manage.Common.Helpers.Models;

namespace ScoreSharp.API.Modules.Manage.Common.Helpers;

public class ManageHelper(ScoreSharpContext context) : IManageHelper
{
    /// <summary>
    /// 取得未分派案件基礎資料
    /// </summary>
    /// <remarks>
    /// 查詢邏輯：
    /// 1. 找出所有「有待派案卡片」的案件（CardStatus 為 30010、3、10201）
    /// 2. 且案件的 CurrentHandleUserId 為 NULL（未分派）
    /// 3. 返回案件編號、來源、姓名檢核（正卡+附卡）、卡片狀態
    /// </remarks>
    /// <param name="cancellationToken"></param>
    /// <returns>未分派案件基礎資料列表</returns>
    public async Task<List<UnassignedCaseBaseData>> GetUnassignedCaseBaseDataAsync(CancellationToken cancellationToken = default)
    {
        string sql = """
            -- 步驟1: 找出所有「有待派案卡片」的案件編號
            ;WITH UnassignedCaseApplyNos AS (
                SELECT DISTINCT H.ApplyNo
                FROM [dbo].[Reviewer_ApplyCreditCardInfoHandle] H
                JOIN [dbo].[Reviewer_ApplyCreditCardInfoMain] M ON H.ApplyNo = M.ApplyNo
                WHERE M.CurrentHandleUserId IS NULL
                AND H.CardStatus IN (30010, 3, 10201)
            ),
            -- 步驟2: 用案件編號查詢完整資料（包含卡片狀態和姓名檢核）
            UnassignedCaseData AS (
                SELECT
                    H.ApplyNo,
                    M.Source,
                    M.NameChecked AS MainNameChecked,
                    S.NameChecked AS SuppNameChecked,
                    H.CardStatus,
                    H.ApplyCardType
                FROM UnassignedCaseApplyNos U
                JOIN [dbo].[Reviewer_ApplyCreditCardInfoMain] M ON U.ApplyNo = M.ApplyNo
                JOIN [dbo].[Reviewer_ApplyCreditCardInfoHandle] H ON M.ApplyNo = H.ApplyNo
                LEFT JOIN [dbo].[Reviewer_ApplyCreditCardInfoSupplementary] S ON M.ApplyNo = S.ApplyNo
                WHERE H.CardStatus IN (30010, 3, 10201)
            )
            SELECT
                ApplyNo,
                Source,
                MainNameChecked,
                SuppNameChecked,
                CardStatus,
                ApplyCardType
            FROM UnassignedCaseData;
            """;

        var result = await context.Database.SqlQueryRaw<UnassignedCaseBaseData>(sql).ToListAsync(cancellationToken);

        return result;
    }

    public bool 與推廣人員是否相同(string promotionUserId, string assignedUserId, string? employeeNo) =>
        assignedUserId.Equals(promotionUserId, StringComparison.OrdinalIgnoreCase)
        || (!string.IsNullOrEmpty(employeeNo) && employeeNo.Equals(promotionUserId, StringComparison.OrdinalIgnoreCase));

    public bool 與月收入確認人員是否相同(HashSet<string> monthlyIncomeCheckUserId, string assignedUserId) =>
        monthlyIncomeCheckUserId.Contains(assignedUserId, StringComparer.OrdinalIgnoreCase);

    public bool 檢核利害關係人(
        ILookup<string, Reviewer_Stakeholder> stakeholderLookup,
        HashSet<string> idSet,
        string assignedUserId,
        string? employeeNo
    )
    {
        foreach (var id in idSet)
        {
            var stakeholder = stakeholderLookup[id];
            if (
                stakeholder.Any(s =>
                    s.UserId.Equals(assignedUserId, StringComparison.OrdinalIgnoreCase)
                    || (!string.IsNullOrEmpty(employeeNo) && s.UserId.Equals(employeeNo, StringComparison.OrdinalIgnoreCase))
                )
            )
            {
                return true;
            }
        }
        return false;
    }
}
