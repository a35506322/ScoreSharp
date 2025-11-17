namespace ScoreSharp.API.Modules.Reviewer.Helpers.Models;

public static class ReviewerValidationRule
{
    public const string KYC = "KYC";
    public const string _929 = "929";
    public const string NameCheck = "姓名檢核";
    public const string Focus1 = "關注名單1";
    public const string Focus2 = "關注名單2";
    public const string DataFormat = "資料格式";
    public const string Addresses = "地址欄位";
    public const string BankTraceReply = "銀行追蹤回復";
    public const string BankTraceRequired = "銀行追蹤必填";
    public const string InternalEmailSame = "行內 Email 相同";
    public const string InternalMobileSame = "行內手機相同";
    public const string BranchCustomer = "分行客戶檢核";
}
