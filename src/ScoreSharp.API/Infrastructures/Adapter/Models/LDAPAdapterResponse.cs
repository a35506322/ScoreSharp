namespace ScoreSharp.API.Infrastructures.Adapter.Models;

public class LDAPAdapterResponse<T>
{
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public T? Data { get; set; } = default!;
}
