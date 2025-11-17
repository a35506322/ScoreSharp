using ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore.EcardNewCase.Models;

namespace ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore.EcardNewCase.Helpers;

public static class VerifyHelper
{
    public static bool 檢查_申請書編號是否長度為13(string applyNo) => applyNo.Length == 13;

    public static bool 檢查_申請書編號格式是否正確(string applyNo) => Regex.IsMatch(applyNo, @"^\d{8}[A-Z]{1}\d{4}$");

    public static bool 檢查_徵信代碼只能為A02或者空值(string creditCheckCode) => creditCheckCode == "A02" || string.IsNullOrEmpty(creditCheckCode);

    /// <summary>
    /// 檢查必要欄位是否為空值
    /// </summary>
    /// <param name="applyNo">申請書編號</param>
    /// <param name="cardOwner">主附卡別</param>
    /// <param name="applyCardType">申請卡別</param>
    /// <param name="formCode">表單代碼</param>
    /// <param name="chName">中文姓名</param>
    /// <param name="birthday">出生日期</param>
    /// <param name="id">正卡_身份證字號</param>
    /// <param name="source">進件方式</param>
    /// <returns></returns>
    public static CheckRequiredResult 檢查_必要欄位不能為空值(
        string applyNo,
        string cardOwner,
        string applyCardType,
        string formCode,
        string chName,
        string birthday,
        string id,
        string source
    )
    {
        var errorBuilder = new StringBuilder();
        var result = new CheckRequiredResult();

        if (string.IsNullOrEmpty(applyNo))
            errorBuilder.Append("申請書編號不能為空、");
        if (string.IsNullOrEmpty(cardOwner))
            errorBuilder.Append("主附卡別不能為空、");
        if (string.IsNullOrEmpty(applyCardType))
            errorBuilder.Append("申請卡別不能為空、");
        if (string.IsNullOrEmpty(formCode))
            errorBuilder.Append("表單代碼不能為空、");
        if (string.IsNullOrEmpty(chName))
            errorBuilder.Append("中文姓名不能為空、");
        if (string.IsNullOrEmpty(birthday))
            errorBuilder.Append("出生日期不能為空、");
        if (string.IsNullOrEmpty(id))
            errorBuilder.Append("正卡_身份證字號不能為空、");
        if (string.IsNullOrEmpty(source))
            errorBuilder.Append("進件方式不能為空、");

        if (errorBuilder.Length > 0)
        {
            result.IsValid = false;
            result.ErrorMessage = errorBuilder.ToString();
        }
        else
        {
            result.IsValid = true;
            result.ErrorMessage = "";
        }
        return result;
    }

    public static bool 檢查是否民國年日期正確(string date) =>
        DateTime.TryParseExact(date, "yyyMMdd", null, System.Globalization.DateTimeStyles.None, out var result);

    private static readonly Regex TaiwanIdRegex = new(@"^[A-Z][12]\d{8}$", RegexOptions.Compiled); // 台灣國民身分證
    private static readonly Regex OldForeignIdRegex = new(@"^[A-Z]{2}\d{8}$", RegexOptions.Compiled); // 舊制外籍證號
    private static readonly Regex NewForeignIdRegex = new(@"^[A-Z]{1}[89]{1}\d{8}$", RegexOptions.Compiled); // 新制外籍證號（2021）

    public static bool 檢查是否身分證格式(string id)
    {
        /*
            台灣國民身分證號（現行）
            1 碼英文字母 + 9 碼數字
            第二碼 為 1（男） 或 2（女）

            ✔ 舊制外籍人士統一證號
            2 碼英文字母 + 8 碼數字

            ✔ 新制外籍人士統一證號（2021）
            1 碼英文字母 + 第二個為 8 或 9 + 8 碼數字

        */
        if (string.IsNullOrEmpty(id))
        {
            return false;
        }

        if (TaiwanIdRegex.IsMatch(id) || OldForeignIdRegex.IsMatch(id) || NewForeignIdRegex.IsMatch(id))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool 檢查是否存在EnumByValue<T>(string value) =>
        int.TryParse(value, out int intValue) ? Enum.IsDefined(typeof(T), intValue) : false;

    public static bool 檢查是否存在EnumByName<T>(string value) => Enum.IsDefined(typeof(T), value);

    public static bool 檢查手機號碼格式(string phoneNumber) => Regex.IsMatch(phoneNumber, @"^09\d{8}$");

    public static bool 檢查電話號碼格式(string phoneNumber) => Regex.IsMatch(phoneNumber, @"^[\d\s\+\-\(\)]+$");

    public static bool 驗證只能輸入01(string value) => Regex.IsMatch(value, @"^[01]$");

    public static bool 驗證只能輸入YN(string value) => Regex.IsMatch(value, @"^[YN]$");

    public static bool 驗證統一編號(string value) => Regex.IsMatch(value, @"^\d{8}$");

    public static bool 驗證IPV4(string value) =>
        Regex.IsMatch(value, @"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");

    public static bool 驗證Email(string value) => Regex.IsMatch(value, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$");

    public static bool 檢查地址有效性(string city, string district, string road)
    {
        bool isFailed = false;
        if (string.IsNullOrEmpty(district) || string.IsNullOrEmpty(city) || string.IsNullOrEmpty(road))
        {
            isFailed = true;
        }
        return isFailed;
    }

    public static (bool isFailed, string errorMsg) 檢查對應地址填寫(EcardSendAddress sendAddress, EcardNewCaseRequest request)
    {
        bool isFailed = false;
        string errorMsg = "";
        switch (sendAddress)
        {
            case EcardSendAddress.同公司:
                isFailed = VerifyHelper.檢查地址有效性(request.Comp_City, request.Comp_District, request.Comp_Road);
                errorMsg = string.Format(
                    "正卡人公司地址一定要有 縣市 / 區域 / 路名: {0} / {1} / {2}、",
                    request.Comp_City,
                    request.Comp_District,
                    request.Comp_Road
                );
                break;
            case EcardSendAddress.同戶籍:
                isFailed = VerifyHelper.檢查地址有效性(request.Reg_City, request.Reg_District, request.Reg_Road);
                errorMsg = string.Format(
                    "正卡人戶籍地址一定要有 縣市 / 區域 / 路名: {0} / {1} / {2}、",
                    request.Reg_City,
                    request.Reg_District,
                    request.Reg_Road
                );
                break;
            case EcardSendAddress.同居住:
                isFailed = VerifyHelper.檢查地址有效性(request.Home_City, request.Home_District, request.Home_Road);
                errorMsg = string.Format(
                    "正卡人居住地址一定要有 縣市 / 區域 / 路名: {0} / {1} / {2}、",
                    request.Home_City,
                    request.Home_District,
                    request.Home_Road
                );
                break;
        }

        return (isFailed, errorMsg);
    }
}
