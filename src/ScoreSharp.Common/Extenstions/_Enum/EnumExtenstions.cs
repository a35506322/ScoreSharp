using System.Reflection;

namespace ScoreSharp.Common.Extenstions._Enum;

public static class EnumExtenstions
{
    public static string ToDescription(this Enum value)
    {
        return value
                .GetType()
                .GetRuntimeField(value.ToString())
                .GetCustomAttributes<System.ComponentModel.DescriptionAttribute>()
                .FirstOrDefault()
                ?.Description ?? string.Empty;
    }

    public static string ToNameAttr(this Enum value)
    {
        return value.GetType().GetRuntimeField(value.ToString()).GetCustomAttributes<EnumNameAttribute>().FirstOrDefault()?.Name
            ?? string.Empty;
    }

    public static string ToValueAttr(this Enum value)
    {
        return value.GetType().GetRuntimeField(value.ToString()).GetCustomAttributes<EnumValueAttribute>().FirstOrDefault()?.Value
            ?? string.Empty;
    }

    public static string ToName(this Enum value) => value.ToString();

    public static List<EnumInfo> GetEnumInfo<T>()
        where T : Enum
    {
        var enumList = new List<EnumInfo>();
        foreach (var enumValue in Enum.GetValues(typeof(T)))
        {
            var fi = typeof(T).GetField(enumValue.ToString());
            var enumValueAttr = (EnumValueAttribute)Attribute.GetCustomAttribute(fi, typeof(EnumValueAttribute));
            var enumNameAttr = (EnumNameAttribute)Attribute.GetCustomAttribute(fi, typeof(EnumNameAttribute));
            var enumIsActiveAttr = (EnumIsActiveAttribute)Attribute.GetCustomAttribute(fi, typeof(EnumIsActiveAttribute));

            var item = new EnumInfo
            {
                NameAttr = enumNameAttr is not null ? enumNameAttr.Name : String.Empty,
                ValueAttr = enumValueAttr is not null ? enumValueAttr.Value : String.Empty,
                IsActiveAttr = enumIsActiveAttr is not null ? enumIsActiveAttr.IsActive : false,
                Name = fi.Name,
                Value = (int)enumValue,
            };

            enumList.Add(item);
        }
        return enumList;
    }

    /// <summary>
    /// 取得列舉值的選項
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="isActive"></param>
    /// <returns></returns>
    public static List<OptionsDtoTypeInt> GetEnumOptions<T>(string? isActive)
        where T : Enum
    {
        return GetEnumInfo<T>()
            .Select(x => new OptionsDtoTypeInt()
            {
                Value = x.Value,
                Name = x.Name,
                IsActive = x.IsActiveAttr ? "Y" : "N",
            })
            .Where(x => String.IsNullOrEmpty(isActive) || x.IsActive == isActive)
            .ToList();
    }

    /// <summary>
    /// 取得列舉值的選項，並使用 NameAttr 作為 Name
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="isActive"></param>
    /// <returns></returns>
    public static List<OptionsDtoTypeInt> GetEnumOptionsWithNameAttr<T>(string? isActive)
        where T : Enum
    {
        return GetEnumInfo<T>()
            .Select(x => new OptionsDtoTypeInt()
            {
                Value = x.Value,
                Name = x.NameAttr,
                IsActive = x.IsActiveAttr ? "Y" : "N",
            })
            .Where(x => String.IsNullOrEmpty(isActive) || x.IsActive == isActive)
            .ToList();
    }

    public static T? ConvertEnumByStringWithNull<T>(string value)
        where T : struct, Enum
    {
        if (Enum.TryParse(typeof(T), value, out var result))
        {
            return (T)result;
        }
        return null;
    }

    public static T ConvertEnumByStringNotNull<T>(string value)
        where T : struct, Enum
    {
        if (Enum.TryParse(typeof(T), value, out var result))
        {
            return (T)result;
        }
        return default;
    }

    public static T? ConvertEnumByIntWithNull<T>(int value)
        where T : struct, Enum
    {
        try
        {
            if (Enum.IsDefined(typeof(T), value))
            {
                return (T)Enum.ToObject(typeof(T), value);
            }
            return null;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public static T ConvertEnumByIntNotNull<T>(int value)
        where T : struct, Enum
    {
        if (Enum.IsDefined(typeof(T), value))
        {
            return (T)Enum.ToObject(typeof(T), value);
        }
        return default;
    }
}
