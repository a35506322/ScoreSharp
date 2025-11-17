namespace ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase.Models;

public class CheckCaseRes<T>
{
    /// <summary>
    /// 是否成功
    /// </summary>
    public bool IsSuccess { get; private set; } = false;

    /// <summary>
    /// 當 IsSuccess 為 true 時，會有資料
    /// </summary>
    public SuccessData<T>? SuccessData { get; private set; } = default;

    /// <summary>
    /// 當 IsSuccess 為 false 時，會有錯誤紀錄
    /// </summary>
    public System_ErrorLog? ErrorData { get; private set; } = default;

    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }

    public void SetStartTime() => StartTime = DateTime.Now;

    public void SetEndTime() => EndTime = DateTime.Now;

    public void SetSuccess(T data)
    {
        IsSuccess = true;

        SuccessData = new SuccessData<T>() { Data = data };

        SetEndTime();
    }

    public void SetError(SystemErrorLog dto)
    {
        ErrorData = new System_ErrorLog()
        {
            ApplyNo = dto.ApplyNo,
            ErrorMessage = dto.ErrorMessage,
            AddTime = StartTime,
            Project = dto.Project,
            Source = dto.Source,
            ErrorDetail = dto.ErrorDetail,
            Request = dto.Request,
            Response = dto.Response,
            Type = dto.Type,
            SendStatus = SendStatus.等待,
        };

        IsSuccess = false;

        SetEndTime();
    }
}

public class SuccessData<T>
{
    /// <summary>
    /// 當 IsSuccess 為 true 時，會有資料
    /// </summary>
    public T? Data { get; set; } = default;
}

public class SystemErrorLog
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; } = null!;

    /// <summary>
    /// 專案
    /// API
    /// BATCH
    /// Middlewave
    /// </summary>
    public string Project { get; set; } = null!;

    /// <summary>
    /// 來源
    /// 範例:如非卡友檢核排程
    /// </summary>
    public string Source { get; set; } = null!;

    /// <summary>
    /// 類型
    ///
    /// 第三方API呼叫
    /// 系統錯誤
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// 錯誤訊息
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Log標題，方便辨識
    /// </summary>
    public string? ErrorDetail { get; set; }

    /// <summary>
    /// 可用於放置參數，例如呼叫第三方API
    /// </summary>
    public string? Request { get; set; }

    /// <summary>
    /// 可用於放置回應，例如呼叫第三方API
    /// </summary>
    public string? Response { get; set; }
}
