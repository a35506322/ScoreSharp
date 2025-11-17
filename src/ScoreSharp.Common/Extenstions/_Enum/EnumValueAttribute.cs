namespace ScoreSharp.Common.Extenstions._Enum;

/*
AttributeTargets.Field：這表示這個屬性只能應用在欄位（fields）上。在你的例子中，這是列舉值。
Inherited = false：這表示這個屬性不會被繼承。如果你有一個繼承自帶有這個屬性的類別的子類別，該屬性不會自動應用到子類別上。
AllowMultiple = false：這表示這個屬性在同一個項目上只能應用一次。如果設定為 true，則可以在同一個項目上應用多次這個屬性。
*/

[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
public class EnumValueAttribute : Attribute
{
    public string Value { get; }

    public EnumValueAttribute(string value)
    {
        this.Value = value;
    }
}
