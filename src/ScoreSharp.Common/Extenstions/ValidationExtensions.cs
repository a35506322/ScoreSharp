using System.Collections;

namespace ScoreSharp.Common.Extenstions;

public static class ValidationExtensions
{
    public static (bool IsValid, List<ValidationResult>? Errors) ValidateCompletely(this object model)
    {
        if (model == null)
            return (true, null);

        var allResults = new List<ValidationResult>();

        // 檢查是否為集合（但排除字串）
        if (model is IEnumerable enumerable and not string)
        {
            int index = 0;
            foreach (var item in enumerable)
            {
                if (item != null)
                {
                    var itemResults = ValidateSingleItem(item);
                    // 為集合中的項目加上索引前綴
                    foreach (var result in itemResults)
                    {
                        // 修正：確保 MemberNames 有值
                        var memberNames =
                            result.MemberNames?.Any() == true ? result.MemberNames.Select(m => $"[{index}].{m}").ToArray() : new[] { $"[{index}]" };

                        allResults.Add(new ValidationResult($"[{index}].{result.ErrorMessage}", memberNames));
                    }
                }
                index++;
            }
        }
        else
        {
            // 單一物件驗證
            allResults.AddRange(ValidateSingleItem(model));
        }

        return allResults.Any() ? (false, allResults) : (true, null);
    }

    private static List<ValidationResult> ValidateSingleItem(object item)
    {
        var context = new ValidationContext(item);
        var results = new List<ValidationResult>();

        // 驗證 DataAnnotations
        Validator.TryValidateObject(item, context, results, true);

        // 驗證 IValidatableObject
        /*
            Tips: 只會先驗證前置條件，後置條件不會驗證，如需驗證後置條件，請打開
            但如果 Model 都剩後置條件，則會執行兩次 1. TryValidateObject 2. IValidatableObject Validate
            就會發生兩條一樣錯誤訊息
        */
        // if (item is IValidatableObject validatable)
        // {
        //     var customResults = validatable.Validate(context);
        //     if (customResults != null)
        //     {
        //         results.AddRange(customResults);
        //     }
        // }

        return results;
    }
}
