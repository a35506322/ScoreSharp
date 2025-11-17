using System.Collections;
using System.Reflection;

namespace ScoreSharp.Common.Extenstions;

public static class DataTableExtensions
{
    /// <summary>
    /// 將物件轉換為 DataTable
    /// </summary>
    /// <typeparam name="T">物件類型</typeparam>
    /// <param name="source">來源物件</param>
    /// <param name="tableName">資料表名稱，預設為類型名稱</param>
    /// <returns>DataTable</returns>
    public static DataTable ObjectToDataTable<T>(this T source, string? tableName = null)
        where T : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        var dataTable = new DataTable(tableName ?? typeof(T).Name);
        var properties = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && IsValidDataTableType(p.PropertyType))
            .ToArray();

        // 建立欄位
        foreach (var property in properties)
        {
            var columnType = GetDataTableColumnType(property.PropertyType);
            dataTable.Columns.Add(property.Name, columnType);
        }

        // 新增資料列
        var row = dataTable.NewRow();
        foreach (var property in properties)
        {
            var value = property.GetValue(source);
            row[property.Name] = value ?? DBNull.Value;
        }
        dataTable.Rows.Add(row);

        return dataTable;
    }

    /// <summary>
    /// 將物件集合轉換為 DataTable
    /// </summary>
    /// <typeparam name="T">物件類型</typeparam>
    /// <param name="source">來源物件集合</param>
    /// <param name="tableName">資料表名稱，預設為類型名稱</param>
    /// <returns>DataTable</returns>
    public static DataTable ListToDataTable<T>(this IEnumerable<T> source, string? tableName = null)
        where T : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        var list = source.ToList();
        if (list.Count == 0)
        {
            // 空集合時仍建立包含欄位結構的 DataTable
            return CreateEmptyDataTable<T>(tableName);
        }

        var dataTable = new DataTable(tableName ?? typeof(T).Name);
        var properties = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && IsValidDataTableType(p.PropertyType))
            .ToArray();

        // 建立欄位
        foreach (var property in properties)
        {
            var columnType = GetDataTableColumnType(property.PropertyType);
            dataTable.Columns.Add(property.Name, columnType);
        }

        // 新增資料列
        foreach (var item in list)
        {
            var row = dataTable.NewRow();
            foreach (var property in properties)
            {
                var value = property.GetValue(item);
                row[property.Name] = value ?? DBNull.Value;
            }
            dataTable.Rows.Add(row);
        }

        return dataTable;
    }

    /// <summary>
    /// 建立空的 DataTable，僅包含欄位結構
    /// </summary>
    /// <typeparam name="T">物件類型</typeparam>
    /// <param name="tableName">資料表名稱</param>
    /// <returns>空的 DataTable</returns>
    private static DataTable CreateEmptyDataTable<T>(string? tableName)
        where T : class
    {
        var dataTable = new DataTable(tableName ?? typeof(T).Name);
        var properties = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && IsValidDataTableType(p.PropertyType))
            .ToArray();

        foreach (var property in properties)
        {
            var columnType = GetDataTableColumnType(property.PropertyType);
            dataTable.Columns.Add(property.Name, columnType);
        }

        return dataTable;
    }

    /// <summary>
    /// 檢查類型是否適合作為 DataTable 的欄位類型
    /// </summary>
    /// <param name="type">要檢查的類型</param>
    /// <returns>是否為有效的 DataTable 欄位類型</returns>
    private static bool IsValidDataTableType(Type type)
    {
        // 處理 Nullable 類型
        var underlyingType = Nullable.GetUnderlyingType(type) ?? type;

        // DataTable 支援的基本類型
        var validTypes = new[]
        {
            typeof(bool),
            typeof(byte),
            typeof(sbyte),
            typeof(char),
            typeof(decimal),
            typeof(double),
            typeof(float),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(short),
            typeof(ushort),
            typeof(string),
            typeof(DateTime),
            typeof(TimeSpan),
            typeof(Guid),
            typeof(byte[]),
        };

        return validTypes.Contains(underlyingType) || underlyingType.IsEnum;
    }

    /// <summary>
    /// 取得適合 DataTable 欄位的類型
    /// </summary>
    /// <param name="type">原始類型</param>
    /// <returns>DataTable 欄位類型</returns>
    private static Type GetDataTableColumnType(Type type)
    {
        var underlyingType = Nullable.GetUnderlyingType(type) ?? type;

        // 列舉類型轉換為其底層類型
        if (underlyingType.IsEnum)
        {
            underlyingType = Enum.GetUnderlyingType(underlyingType);
        }

        return underlyingType;
    }
}
