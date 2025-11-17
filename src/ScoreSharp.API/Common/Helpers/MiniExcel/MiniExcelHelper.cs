using MiniExcelLibs;
using MiniExcelLibs.OpenXml;

namespace ScoreSharp.API.Common.Helpers.MiniExcel;

public class MiniExcelHelper : IMiniExcelHelper
{
    /// <summary>
    /// 最基本的匯出Excel
    /// </summary>
    /// <typeparam name="T">class 類別</typeparam>
    /// <param name="data">資料</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<MemoryStream> 基本匯出ExcelToStream<T>(List<T> data)
        where T : class
    {
        var memoryStream = new MemoryStream();
        var config = new OpenXmlConfiguration { FastMode = true, EnableAutoWidth = true };
        await memoryStream.SaveAsAsync(value: data, configuration: config);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return memoryStream;
    }

    public async Task<MemoryStream> 匯出多個工作表ExcelToStream<T>(Dictionary<string, T> data)
        where T : class
    {
        var memoryStream = new MemoryStream();
        var config = new OpenXmlConfiguration { FastMode = true, EnableAutoWidth = true };

        Dictionary<string, object> sheets = [];

        foreach (var item in data)
            sheets[item.Key] = item.Value;

        await memoryStream.SaveAsAsync(value: sheets, configuration: config);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return memoryStream;
    }
}
