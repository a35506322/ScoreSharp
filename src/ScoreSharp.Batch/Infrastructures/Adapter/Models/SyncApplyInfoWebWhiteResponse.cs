namespace ScoreSharp.Batch.Infrastructures.Adapter.Models;

public record SyncApplyInfoWebWhiteResponse(
    PaperMiddlewareReturnCodeStatus ReturnCodeStatus = PaperMiddlewareReturnCodeStatus.成功,
    string ReturnMessage = "",
    object? SuccessData = default,
    object? ErrorDetail = null,
    Dictionary<string, string[]> ValidationErrors = null,
    string ErrorMessage = "",
    string? TraceId = null
);

/// <summary>
/// 狀態碼枚舉
/// </summary>
public enum PaperMiddlewareReturnCodeStatus
{
    成功 = 2000,
    格式驗證失敗 = 4000,
    資料庫定義值錯誤 = 4001,
    查無此資料 = 4002,
    商業邏輯有誤 = 4003,
    標頭驗證失敗 = 4004,
    內部程式失敗 = 5000,
    外部服務錯誤 = 5001,
    資料庫執行失敗 = 5002,
}
