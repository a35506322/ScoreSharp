using ScoreSharp.Batch.Jobs.EcardA02CheckNewCase.Models;

namespace ScoreSharp.Batch.Jobs.EcardA02CheckNewCase;

public interface IEcardA02CheckNewCaseRepository
{
    public Task<IEnumerable<CheckA02JobContext>> 查詢_須檢核卡友案件(int limit);
    public Task<List<string>> 查詢_行內IP();
    public Task<List<SetUp_AddressInfo>> 查詢_地址資訊();
    public Task<SysParamManage_SysParam> 查詢_系統參數();
    public Task<UspCheckSameIPResult> 查詢_IP比對相同(string applyNo);
    public Task<UspCheckSameEmailResult> 查詢_網路電子郵件相同(string applyNo);
    public Task<UspCheckSameMobileResult> 查詢_網路手機相同(string applyNo);
    public Task<UspCheckShortTimeIDResult> 查詢_頻繁ID(string applyNo);
}
