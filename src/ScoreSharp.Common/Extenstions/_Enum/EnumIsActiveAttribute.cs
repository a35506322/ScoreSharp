namespace ScoreSharp.Common.Extenstions._Enum;

[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
public class EnumIsActiveAttribute : Attribute
{
    public bool IsActive { get; }

    public EnumIsActiveAttribute(bool isActive)
    {
        this.IsActive = isActive;
    }
}
