namespace ScoreSharp.API.Common.Helpers.MiniExcel;

public interface IMiniExcelHelper
{
    public Task<MemoryStream> 基本匯出ExcelToStream<T>(List<T> data)
        where T : class;

    public Task<MemoryStream> 匯出多個工作表ExcelToStream<T>(Dictionary<string, T> data)
        where T : class;
}
