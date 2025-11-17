namespace ScoreSharp.Batch.Jobs.PaperCheckNewCase.Models;

public class CheckCaseRes<T>
{
    /// <summary>
    /// 如果為不成功，不會有SuccessData，但會有ErrorData
    /// 如果為成功，會有SuccessData，但ErrorData會是空
    /// </summary>
    public bool IsSuccess => ErrorData.Count == 0;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public T? SuccessData { get; set; } = default;
    public List<System_ErrorLog> ErrorData { get; set; } = new();

    public void SetStartTime() => StartTime = DateTime.Now;

    public void SetEndTime() => EndTime = DateTime.Now;

    public void SetSuccess(T data)
    {
        SuccessData = data;
    }

    public void SetError(System_ErrorLog dto)
    {
        ErrorData.Add(
            new System_ErrorLog()
            {
                ApplyNo = dto.ApplyNo,
                ErrorMessage = dto.ErrorMessage,
                AddTime = StartTime,
                Project = SystemErrorLogProjectConst.BATCH,
                Source = "PaperCheckNewCase",
                ErrorDetail = dto.ErrorDetail,
                Request = dto.Request,
                Response = dto.Response,
                Type = dto.Type,
                SendStatus = SendStatus.等待,
            }
        );
    }
}
