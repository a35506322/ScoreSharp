using ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase.Helpers;
using ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase.Models;

namespace ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase;

public interface IEcardNotA02CheckNewCaseRepository
{
    public Task<List<string>> 查詢_行內IP();
    public Task<List<CheckJobContext>> 查詢_須檢核非卡友案件(int limit);
    public Task<List<SetUp_AddressInfo>> 查詢_地址資訊();
    public Task<SysParamManage_SysParam> 查詢_系統參數();
    public Task<List<GetHistoryApplyCreditInfoForCheck>> 查詢_歷史申請資料();
}
