namespace ScoreSharp.Common.Helpers;

public static class JsonHelper
{
    public static T? 反序列化物件不分大小寫<T>(string json)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
            };

            var data = JsonSerializer.Deserialize<T>(json, options);
            return data;
        }
        catch (Exception ex)
        {
            Console.WriteLine("反序列化物件不分大小寫 input: " + json);
            Console.WriteLine("反序列化物件不分大小寫 error: " + ex);
            return default;
        }
    }

    public static string 序列化物件(object obj)
    {
        try
        {
            // 序列化物件時，將中文進行編碼
            var serializerOptions = new JsonSerializerOptions { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
            return JsonSerializer.Serialize(obj, serializerOptions);
        }
        catch (Exception ex)
        {
            throw new Exception("序列化物件失敗", ex);
        }
    }

    public static string 格式化Json(string json)
    {
        var jsonDoc = JsonDocument.Parse(json);
        var options = new JsonSerializerOptions { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
        return JsonSerializer.Serialize(jsonDoc, options);
    }
}
