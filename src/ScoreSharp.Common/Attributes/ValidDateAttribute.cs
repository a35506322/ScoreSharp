using System.Globalization;

namespace ScoreSharp.Common.Attributes;

public class ValidDateAttribute : ValidationAttribute
{
    /// <summary>
    /// 日期格式
    /// 範例:民國 yyyMMdd
    /// </summary>
    public string Format { get; }

    /// <summary>
    /// 是否民國年
    /// </summary>
    public bool IsROC { get; }

    public ValidDateAttribute(string format, bool isROC)
    {
        Format = format;
        IsROC = isROC;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty((string?)value))
            return ValidationResult.Success;

        var dateString = value.ToString();

        try
        {
            if (IsROC) // 民國年格式 (如 1130229)
            {
                return TryParseDate(dateString, Format, validationContext, new CultureInfo("zh-TW"));
            }
            else // 西元格式 (如 20250229)
            {
                return TryParseDate(dateString, Format, validationContext, CultureInfo.InvariantCulture);
            }
        }
        catch
        {
            var memberName = validationContext.MemberName;
            if (string.IsNullOrEmpty(memberName))
            {
                memberName = validationContext.DisplayName;
            }
            return new ValidationResult($"{validationContext.DisplayName} 日期格式錯誤，請確認是否符合指定格式（{Format}）。", new[] { memberName });
        }
    }

    private ValidationResult TryParseDate(string dateStr, string format, ValidationContext validationContext, CultureInfo cultureInfo)
    {
        // 判斷是否只有年月
        var memberName = validationContext.MemberName;
        var displayName = validationContext.DisplayName;

        if (format.Equals("yyyyMM", StringComparison.OrdinalIgnoreCase))
        {
            // 補上日，讓 TryParseExact 可以驗證
            var fullDateStr = dateStr + "01"; // e.g. 202502 → 20250201
            var fullFormat = "yyyyMMdd";

            if (DateTime.TryParseExact(fullDateStr, fullFormat, cultureInfo, DateTimeStyles.None, out _))
                return ValidationResult.Success;

            return new ValidationResult($"{displayName} 為無效年月，請輸入正確的年月（格式：{format}）。", new[] { memberName });
        }

        if (DateTime.TryParseExact(dateStr, format, cultureInfo, DateTimeStyles.None, out _))
            return ValidationResult.Success;

        return new ValidationResult($"{displayName} 為無效日期，請輸入正確的日期（格式：{format}）。", new[] { memberName });
    }
}
