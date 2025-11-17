using ScoreSharp.API.Modules.Reviewer.Helpers.Models;

namespace ScoreSharp.API.Modules.Reviewer.Helpers;

public interface IReviewerHelper
{
    /// <summary>
    /// 取得徵審申請書資料相關參數
    /// </summary>
    /// <param name="isActive">非必填，Y或N</param>
    /// <returns></returns>
    public Task<ApplyCreditCardInfoParamsDto> GetCreditCardInfoParams(string? isActive = null);

    /// <summary>
    /// 取得徵審地址資料相關參數
    /// </summary>
    /// <param name="isActive">非必填，Y或N</param>
    /// <returns></returns>
    public Task<AddressInfoParamsDto> GetAddressInfoParams(string? isActive = null);

    public (bool, List<string>) 檢查正卡人必填地址(Reviewer_ApplyCreditCardInfoMain main);
    public (bool, List<string>) 檢查附卡人必填地址(Reviewer_ApplyCreditCardInfoSupplementary supplementary);
    public bool 檢查對應地址(Reviewer_ApplyCreditCardInfoMain main, MailingAddressType mailingAddressType);

    public (bool, List<string>) 檢查銀行追蹤回覆是否必輸(Reviewer_BankTrace bankTrace, BillType? billType = null);

    public Task<int> UpdateMainLastModified(string applyNo, string userId, DateTime? updateTime = null);

    /// <summary>
    /// 取得徵審案件資料
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public Task<IEnumerable<GetApplyCreditCardBaseDataResult>> GetApplyCreditCardBaseData(GetApplyCreditCardBaseDataDto dto);
}
