namespace ScoreSharp.Common.Attributes;

public class ValidEnumValueAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        Type enumType = value.GetType();
        bool valid = Enum.IsDefined(enumType, value);
        if (!valid)
        {
            return new ValidationResult(String.Format("{0} 不存在於 {1}", value, enumType.Name));
        }
        return ValidationResult.Success;
    }
}
