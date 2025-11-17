using ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase.Helpers;
using ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase.Models;

namespace ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase;

public interface IEcardNotA02CheckNewCaseService
{
    public Task<CheckCaseRes<bool>> 檢核_行內IP相同(CheckJobContext context, CommonDBDataDto commonDBDataDto);
    public Task<CheckCaseRes<CheckSameIP>> 檢核_相同IP比對(CheckJobContext context, CommonDBDataDto commonDBDataDto);
    public Task<CheckCaseRes<CheckSameWebCaseEmail>> 檢核_相同電子郵件比對(CheckJobContext context, CommonDBDataDto commonDBDataDto);
    public Task<CheckCaseRes<CheckSameWebCaseMobile>> 檢核_相同手機號碼比對(CheckJobContext context, CommonDBDataDto commonDBDataDto);
    public Task<CheckCaseRes<QueryBranchInfo>> 檢核_查詢分行資訊(CheckJobContext context);
    public Task<CheckCaseRes<Query929Info>> 檢核_發查929(CheckJobContext context);
    public Task<CheckCaseRes<ConcernDetailInfo>> 檢核_查詢關注名單(CheckJobContext context);
    public Task<CheckCaseRes<CheckShortTimeID>> 檢核_短時間ID相同比對(CheckJobContext context, CommonDBDataDto commonDBDataDto);
    public Task 通知_檢核異常信件(string applyNo, string type, string errorMessage, string errorDetail = "");
    public string 計算郵遞區號(AddressContext address, List<SetUp_AddressInfo> addressInfos);
    public Task<CheckCaseRes<QueryCheckName>> 檢核_查詢姓名檢核(CheckJobContext context);
    public Task<int> 案件異常處理(string applyNo, string type, string errorTitle, Exception ex);
    public Task 寄信給達錯誤2次案件(List<string> applyNo);
    public Task<CheckCaseRes<CheckInternalEmailSameResult>> 檢核_行內Email資料(CheckJobContext context);
    public Task<CheckCaseRes<CheckInternalMobileSameResult>> 檢核_行內Mobile資料(CheckJobContext context);
    public Task<CheckCaseRes<bool>> 檢查_是否為重覆進件(CheckJobContext context);
}
