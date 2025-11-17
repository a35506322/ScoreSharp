using System.Collections;

namespace ScoreSharp.Common.Attributes;

public class ValidEnumListAttribute : ValidationAttribute
{
    private readonly Type _enumType;

    public ValidEnumListAttribute(Type enumType)
    {
        if (enumType == null)
        {
            throw new ArgumentNullException(nameof(enumType));
        }

        if (!enumType.IsEnum)
        {
            throw new ArgumentException("Type must be an enum", nameof(enumType));
        }
        _enumType = enumType;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        if (value is not IEnumerable enumList)
        {
            return new ValidationResult("值必須是列舉陣列");
        }

        foreach (var item in enumList)
        {
            if (item == null)
                continue;

            if (!Enum.IsDefined(_enumType, item))
            {
                return new ValidationResult($"值 {item} 不存在於 {_enumType.Name}");
            }
        }

        return ValidationResult.Success;
    }
}
