using ScoreSharp.Batch.Jobs.PaperCheckNewCase.Models;

namespace ScoreSharp.Batch.Jobs.PaperCheckNewCase;

public interface IPaperCheckNewCaseRepository
{
    /// <summary>
    /// 檢查是否為工作日
    /// </summary>
    /// <param name="date">檢查日期</param>
    /// <returns>是否為工作日</returns>
    Task<bool> 檢查_是否為工作日(DateTime date);

    /// <summary>
    /// 查詢待檢核的紙本件案件
    /// </summary>
    /// <param name="limit">查詢筆數</param>
    /// <returns>待檢核案件清單</returns>
    Task<List<ReviewerPedding_PaperApplyCardCheckJob>> 查詢_待檢核案件(int limit = 100);

    /// <summary>
    /// 根據申請號碼查詢案件詳細資料
    /// </summary>
    /// <param name="applyNos">申請號碼清單</param>
    /// <returns>案件詳細資料</returns>
    Task<List<ApplyCaseDetail>> 查詢_待檢核案件詳細資料(List<string> applyNos);

    /// <summary>
    /// 查詢地址資訊
    /// </summary>
    /// <returns>地址資訊清單</returns>
    Task<List<SetUp_AddressInfo>> GetAddressInfoAsync();

    /// <summary>
    /// 檢查頻繁 ID 申請
    /// </summary>
    /// <param name="applyNo">申請號碼</param>
    /// <returns>檢核結果</returns>
    Task<UspCheckShortTimeIDResult> 查詢_頻繁ID(string applyNo);

    /// <summary>
    /// 查詢_排程設定
    /// </summary>
    /// <returns></returns>
    Task<SysParamManage_BatchSet> 查詢_排程設定();
}
