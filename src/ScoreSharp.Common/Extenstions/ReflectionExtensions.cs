namespace ScoreSharp.Common.Extenstions;

public static class ReflectionExtensions
{
    public static string GetDisplayName<T>(this T Object, string propertyName)
    {
        var property = typeof(T).GetProperty(propertyName);
        if (property == null)
            throw new ArgumentException($"屬性 '{propertyName}' 查無此類型 '{typeof(T).Name}'.");

        var displayAttribute = property.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;

        return displayAttribute is not null ? displayAttribute.Name : property.Name;
    }
}
