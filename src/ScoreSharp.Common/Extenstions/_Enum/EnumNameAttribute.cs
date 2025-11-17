namespace ScoreSharp.Common.Extenstions._Enum;

[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
public class EnumNameAttribute : Attribute
{
    public string Name { get; }

    public EnumNameAttribute(string name)
    {
        this.Name = name;
    }
}
