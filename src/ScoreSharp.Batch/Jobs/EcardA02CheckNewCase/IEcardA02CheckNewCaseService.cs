using ScoreSharp.Batch.Jobs.EcardA02CheckNewCase.Models;

namespace ScoreSharp.Batch.Jobs.EcardA02CheckNewCase;

public interface IEcardA02CheckNewCaseService
{
    public Task 通知_檢核異常信件(string applyNo, string type, string errorMessage, string errorDetail);
    public Task<CheckCaseRes<QueryOriginalCardholderData>> 檢核_原持卡人資料(CheckA02JobContext context);
    public Task<CheckCaseRes<CheckInternalEmailSameResult>> 檢核_行內Email資料(CheckA02JobContext context);
    public Task<CheckCaseRes<CheckInternalMobileSameResult>> 檢核_行內Mobile資料(CheckA02JobContext context);
    public Task<CheckCaseRes<Check929Info>> 檢核_發查929(CheckA02JobContext context);
    public Task<CheckCaseRes<bool>> 檢核_行內IP相同(CheckA02JobContext context, CommonDBDataDto commonDBDataDto);
    public Task<CheckCaseRes<CheckSameIP>> 檢核_IP比對相同(CheckA02JobContext context);
    public Task<CheckCaseRes<CheckSameWebCaseEmail>> 檢核_網路電子郵件相同(CheckA02JobContext context);
    public Task<CheckCaseRes<CheckSameWebMobile>> 檢核_網路手機相同(CheckA02JobContext context);
    public Task<CheckCaseRes<CheckShortTimeID>> 檢核_頻繁ID(CheckA02JobContext context);
    public Task<CheckCaseRes<ConcernDetailInfo>> 檢核_查詢關注名單(CheckA02JobContext context);
    public Task<int> 案件異常處理(string applyNo, string type, string errorTitle, Exception ex);
    public Task 寄信給達錯誤2次案件(List<string> applyNo);
    public Task<CheckCaseRes<bool>> 檢查_是否為重覆進件(CheckA02JobContext context);
    public CheckCaseRes<string> 計算_郵遞區號(string fullAddress, CommonDBDataDto commonDBDataDto, string applyNo);
}
