namespace ScoreSharp.Common.Adapters.MW3.Models;

public class BaseMW3Response<T>
{
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public T? Data { get; set; } = default;
}
