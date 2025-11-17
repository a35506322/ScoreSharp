using ScoreSharp.Batch.Jobs.EcardA02CheckNewCase.Models;

namespace ScoreSharp.Batch.Jobs.EcardA02CheckNewCase;

public class EcardA02CheckNewCaseRepository(ScoreSharpContext context, IScoreSharpDapperContext dapperContext) : IEcardA02CheckNewCaseRepository
{
    public async Task<SysParamManage_SysParam> 查詢_系統參數() => await context.SysParamManage_SysParam.AsNoTracking().FirstOrDefaultAsync();

    public async Task<List<SetUp_AddressInfo>> 查詢_地址資訊() => await context.SetUp_AddressInfo.AsNoTracking().ToListAsync();

    public async Task<List<string>> 查詢_行內IP() =>
        await context.SetUp_InternalIP.AsNoTracking().Where(x => x.IsActive == "Y").Select(x => x.IP).ToListAsync();

    public async Task<IEnumerable<CheckA02JobContext>> 查詢_須檢核卡友案件(int limit = 100)
    {
        string sql =
            @"
                    SELECT TOP (@Limit)
                    A.[ApplyNo],
                    A.[IsQueryOriginalCardholderData],
                    A.[QueryOriginalCardholderDataLastTime],
                    A.[IsCheck929],
                    A.[Check929LastTime],
                A.[IsCheckInternalEmail],
                A.[CheckInternalEmailLastTime],
                A.[IsCheckInternalMobile],
                A.[CheckInternalMobileLastTime],
                A.[IsCheckSameIP],
                A.[CheckSameIPLastTime],
                A.[IsCheckEqualInternalIP],
                A.[CheckEqualInternalIPLastTime],
                A.[IsCheckSameWebCaseEmail],
                A.[CheckSameWebCaseEmailLastTime],
                A.[IsCheckSameWebCaseMobile],
                A.[CheckSameWebCaseMobileLastTime],
                A.[IsCheckFocus],
                A.[CheckFocusLastTime],
                A.[IsCheckShortTimeID],
                A.[CheckShortTimeIDLastTime],
                A.[IsBlackList],
                A.[BlackListLastTime],
                A.[IsChecked],
                A.[ErrorCount],
                A.[AddTime],
                A.[IsCheckRepeatApply],
                A.[CheckRepeatApplyLastTime],
                    M.[UserSourceIP],
                    M.[ApplyDate],
                    M.[ID],
                    M.[CHName],
                    M.[EMail],
                    M.[Mobile],
                    H.[CardStatus],
                    H.[UserType],
                    H.[SeqNo] as 'HandleSeqNo'
            FROM [ScoreSharp].[dbo].[ReviewerPedding_WebApplyCardCheckJobForA02] A
            JOIN  [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoMain] M ON A.ApplyNo = M.ApplyNo
            JOIN [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoHandle] H ON M.ApplyNo = H.ApplyNo AND M.ID = H.ID
            WHERE H.CardStatus in (30009,30115)
            AND A.IsChecked = 2
            AND A.ErrorCount < 2
            Order By A.AddTime, A.ErrorCount
        ";
        using var connection = dapperContext.CreateScoreSharpConnection();
        var result = await connection.QueryAsync<CheckA02JobContext>(sql, new { Limit = limit });
        return result;
    }

    public async Task<UspCheckSameIPResult> 查詢_IP比對相同(string applyNo)
    {
        UspCheckSameIPResult result = new();
        using (var conn = dapperContext.CreateScoreSharpConnection())
        {
            SqlMapper.GridReader results = await conn.QueryMultipleAsync(
                sql: "Usp_CheckSameIP",
                param: new { ApplyNo = applyNo },
                commandType: CommandType.StoredProcedure
            );

            result.IsHit = results.Read<string>().FirstOrDefault();
            result.HitApplyNoInfos = results.Read<HitCheckSameIPApplyNoInfo>().ToList();
        }

        return result;
    }

    public async Task<UspCheckSameEmailResult> 查詢_網路電子郵件相同(string applyNo)
    {
        UspCheckSameEmailResult result = new();
        using (var conn = dapperContext.CreateScoreSharpConnection())
        {
            SqlMapper.GridReader results = await conn.QueryMultipleAsync(
                sql: "Usp_CheckSameEmail",
                param: new { ApplyNo = applyNo },
                commandType: CommandType.StoredProcedure
            );

            result.IsHit = results.Read<string>().FirstOrDefault();
            result.HitApplyNoInfos = results.Read<HitCheckSameEmailApplyNoInfo>().ToList();
        }

        return result;
    }

    public async Task<UspCheckSameMobileResult> 查詢_網路手機相同(string applyNo)
    {
        UspCheckSameMobileResult result = new();
        using (var conn = dapperContext.CreateScoreSharpConnection())
        {
            SqlMapper.GridReader results = await conn.QueryMultipleAsync(
                sql: "Usp_CheckSameMobile",
                param: new { ApplyNo = applyNo },
                commandType: CommandType.StoredProcedure
            );

            result.IsHit = results.Read<string>().FirstOrDefault();
            result.HitApplyNoInfos = results.Read<HitCheckSameMobileApplyNoInfo>().ToList();
        }

        return result;
    }

    public async Task<UspCheckShortTimeIDResult> 查詢_頻繁ID(string applyNo)
    {
        UspCheckShortTimeIDResult result = new();
        using (var conn = dapperContext.CreateScoreSharpConnection())
        {
            SqlMapper.GridReader results = await conn.QueryMultipleAsync(
                sql: "Usp_CheckShortTimeID",
                param: new { ApplyNo = applyNo },
                commandType: CommandType.StoredProcedure
            );

            result.IsHit = results.Read<string>().FirstOrDefault();
            result.HitApplyNoInfos = results.Read<HitCheckShortTimeIDApplyNoInfo>().ToList();
        }

        return result;
    }
}
