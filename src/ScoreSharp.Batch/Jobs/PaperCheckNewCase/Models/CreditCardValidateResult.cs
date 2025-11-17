namespace ScoreSharp.Batch.Jobs.PaperCheckNewCase.Models;

public class CreditCardValidateResult
{
    public bool 是否成功 { get; set; }
    public bool 是否命中 { get; set; }
    public string 訊息 { get; set; } = string.Empty;
    public List<System_ErrorLog> 錯誤清單 { get; set; } = new();
    public Dictionary<string, object> 額外資料 { get; set; } = new();
}
