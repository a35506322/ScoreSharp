using ScoreSharp.Batch.Jobs.PaperCheckNewCase.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ScoreSharp.Batch.Jobs.PaperCheckNewCase;

public class PaperCheckNewCaseRepository(ScoreSharpContext _context, IScoreSharpDapperContext _dapperContext) : IPaperCheckNewCaseRepository
{
    public async Task<bool> 檢查_是否為工作日(DateTime date)
    {
        var workDays = await _context
            .SetUp_WorkDay.Where(x => x.Date == date.ToString("yyyyMMdd") && x.IsHoliday == "Y")
            .AsNoTracking()
            .ToListAsync();
        return !workDays.Any();
    }

    public async Task<List<ReviewerPedding_PaperApplyCardCheckJob>> 查詢_待檢核案件(int limit = 100) =>
        await _context
            .ReviewerPedding_PaperApplyCardCheckJob.Where(x => x.IsChecked == CaseCheckedStatus.未完成 && x.ErrorCount < 2)
            .AsNoTracking()
            .OrderBy(x => x.ErrorCount)
            .ThenBy(x => x.AddTime)
            .Take(limit)
            .ToListAsync();

    public async Task<List<ApplyCaseDetail>> 查詢_待檢核案件詳細資料(List<string> applyNos)
    {
        const string sql =
            @"
            SELECT
                mi.ApplyNo,
                mi.ID as MainID,
                mi.CHName as MainName,
                mi.EMail as MainEmail,
                mi.Mobile as MainMobile,
                mi.CardOwner,
                mi.IsOriginalCardholder as MainIsOriginalCardholder,
                si.ID as SupplementaryID,
                si.CHName as SupplementaryName,
                si.IsOriginalCardholder as SupplementaryIsOriginalCardholder
            FROM Reviewer_ApplyCreditCardInfoMain mi
            LEFT JOIN Reviewer_ApplyCreditCardInfoSupplementary si ON mi.ApplyNo = si.ApplyNo
            WHERE mi.ApplyNo IN @ApplyNos";

        using var connection = _dapperContext.CreateScoreSharpConnection();
        var results = await connection.QueryAsync<ApplyCaseDetail>(sql, new { ApplyNos = applyNos });
        return results.ToList();
    }

    public async Task<string> GetMaxAMLProfessionVersionAsync() => await _context.SetUp_AMLProfession.AsNoTracking().MaxAsync(x => x.Version);

    public async Task<List<SetUp_AddressInfo>> GetAddressInfoAsync() => await _context.SetUp_AddressInfo.AsNoTracking().ToListAsync();

    public async Task<UspCheckShortTimeIDResult> 查詢_頻繁ID(string applyNo)
    {
        UspCheckShortTimeIDResult result = new();
        using (var conn = _dapperContext.CreateScoreSharpConnection())
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

    public async Task<SysParamManage_BatchSet> 查詢_排程設定() => await _context.SysParamManage_BatchSet.AsNoTracking().SingleOrDefaultAsync();
}
