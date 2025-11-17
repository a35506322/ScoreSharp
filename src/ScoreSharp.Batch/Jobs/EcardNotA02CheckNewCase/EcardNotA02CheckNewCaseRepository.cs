using ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase.Helpers;
using ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase.Models;

namespace ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase;

public class EcardNotA02CheckNewCaseRepository(ScoreSharpContext context, IScoreSharpDapperContext dapperContext) : IEcardNotA02CheckNewCaseRepository
{
    public async Task<List<GetHistoryApplyCreditInfoForCheck>> 查詢_歷史申請資料()
    {
        var historyApplyCreditInfo = new List<GetHistoryApplyCreditInfoForCheck>();
        using (var conn = dapperContext.CreateScoreSharpConnection())
        {
            SqlMapper.GridReader results = await conn.QueryMultipleAsync(
                sql: "Usp_GetHistoryApplyCreditInfoForCheck",
                commandType: CommandType.StoredProcedure
            );
            historyApplyCreditInfo = results.Read<GetHistoryApplyCreditInfoForCheck>().ToList();
        }

        return historyApplyCreditInfo;
    }

    public async Task<SysParamManage_SysParam> 查詢_系統參數() => await context.SysParamManage_SysParam.AsNoTracking().FirstOrDefaultAsync();

    public async Task<List<SetUp_AddressInfo>> 查詢_地址資訊() => await context.SetUp_AddressInfo.AsNoTracking().ToListAsync();

    public async Task<List<string>> 查詢_行內IP() =>
        await context.SetUp_InternalIP.AsNoTracking().Where(x => x.IsActive == "Y").Select(x => x.IP).ToListAsync();

    public async Task<List<CheckJobContext>> 查詢_須檢核非卡友案件(int limit)
    {
        string sql =
            @"
                        SELECT TOP (@Limit)
                              J.[ApplyNo],
                              J.[IsCheckName],
                              J.[CheckNameLastTime],
                              J.[IsQueryBranchInfo],
                              J.[QueryBranchInfoLastTime],
                              J.[IsCheck929],
                              J.[Check929LastTime],
                              J.[IsCheckSameIP],
                              J.[CheckSameIPLastTime],
                              J.[IsCheckEqualInternalIP],
                              J.[CheckEqualInternalIPLastTime],
                              J.[IsChecked],
                              J.[ErrorCount],
                              J.[AddTime],
                              J.[IsCheckSameWebCaseEmail],
                              J.[CheckSameWebCaseEmailLastTime],
                              J.[IsCheckSameWebCaseMobile],
                              J.[CheckSameWebCaseMobileLastTime],
                              J.[IsCheckFocus],
                              J.[CheckFocusLastTime],
                              J.[IsCheckShortTimeID],
                              J.[CheckShortTimeIDLastTime],

                              J.[IsCheckInternalEmail],
                              J.[CheckInternalEmailLastTime],
                              J.[IsCheckInternalMobile],
                              J.[CheckInternalMobileLastTime],
                              J.[IsCheckRepeatApply],
                              J.[CheckRepeatApplyLastTime],
                              M.[UserSourceIP],
                              M.[ApplyDate],
                              M.[ID],
                              M.[CHName],
                              M.[EMail],
                              M.[Mobile],
                              H.[CardStatus],
                              H.[UserType],
                              H.[SeqNo] as 'HandleSeqNo'
                        FROM [ScoreSharp].[dbo].[ReviewerPedding_WebApplyCardCheckJobForNotA02] J
                        JOIN  [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoMain] M ON J.ApplyNo = M.ApplyNo
                        JOIN [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoHandle] H ON M.ApplyNo = H.ApplyNo AND M.ID = H.ID
                        WHERE H.CardStatus in ('30106','30100')
                        AND J.IsChecked = 2
                        AND J.ErrorCount < 2
                        Order By J.AddTime, J.ErrorCount
            ";

        using var conn = dapperContext.CreateScoreSharpConnection();
        var result = await conn.QueryAsync<CheckJobContext>(sql, new { Limit = limit });
        return result.ToList();
    }
}
