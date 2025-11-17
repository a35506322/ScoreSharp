using ScoreSharp.Batch.Jobs.PaperCheckNewCase.Models;

namespace ScoreSharp.Batch.Jobs.PaperCheckNewCase;

public interface IPaperCheckNewCaseService
{
    /// <summary>
    /// 檢核單筆案件
    /// </summary>
    /// <param name="context">檢核上下文</param>
    Task 檢核單筆案件(PaperCheckJobContext context);

    /// <summary>
    /// 寄信給達錯誤2次案件
    /// </summary>
    /// <param name="applyNo">申請號碼</param>
    Task 寄信給達錯誤2次案件(List<string> applyNo);

    /// <summary>
    /// 案件處理異常
    /// </summary>
    /// <param name="applyNo">申請號碼</param>
    /// <param name="type">錯誤類型</param>
    /// <param name="errorTitle">錯誤標題</param>
    /// <param name="ex">例外</param>
    Task<int> 案件處理異常(string applyNo, string type, string errorTitle, Exception ex);

    /// <summary>
    /// 更新案件資料
    /// </summary>
    /// <param name="context">檢核上下文</param>
    /// <param name="addressInfos">地址資訊</param>
    Task<(bool isSuccess, int errorCount)> 更新案件資料(PaperCheckJobContext context, List<SetUp_AddressInfo> addressInfos);

    /// <summary>
    /// 新增系統錯誤紀錄
    /// </summary>
    /// <param name="systemErrorLogs">系統錯誤紀錄</param>
    Task 新增系統錯誤紀錄(List<System_ErrorLog> systemErrorLogs);
}
