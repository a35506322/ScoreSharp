using System.Text.RegularExpressions;

namespace ScoreSharp.Common.Attributes;

public class TWIDAttribute : ValidationAttribute
{
    private static readonly Regex TaiwanIdRegex = new(@"^[A-Z][12]\d{8}$", RegexOptions.Compiled); // 台灣國民身分證
    private static readonly Regex OldForeignIdRegex = new(@"^[A-Z]{2}\d{8}$", RegexOptions.Compiled); // 舊制外籍證號
    private static readonly Regex NewForeignIdRegex = new(@"^[A-Z]{1}[89]{1}\d{8}$", RegexOptions.Compiled); // 新制外籍證號（2021）

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
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
        if (value == null || !(value is string idNumber))
        {
            return ValidationResult.Success;
        }

        if (string.IsNullOrWhiteSpace(idNumber))
        {
            return ValidationResult.Success;
        }

        if (IsValidTaiwanId(idNumber) || IsValidOldForeignId(idNumber) || IsValidNewForeignId(idNumber))
        {
            return ValidationResult.Success;
        }

        return new ValidationResult("無效的台灣身分證或統一證號格式");
    }

    private bool IsValidTaiwanId(string id)
    {
        return TaiwanIdRegex.IsMatch(id);
    }

    private bool IsValidOldForeignId(string id)
    {
        return OldForeignIdRegex.IsMatch(id);
    }

    private bool IsValidNewForeignId(string id)
    {
        return NewForeignIdRegex.IsMatch(id);
    }
}
