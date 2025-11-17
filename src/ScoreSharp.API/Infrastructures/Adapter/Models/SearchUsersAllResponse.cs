namespace ScoreSharp.API.Infrastructures.Adapter.Models;

public class SearchUsersAllResponse
{
    public bool IsSuccess { get; set; }
    public List<LDAPUserInfo> UserInfo { get; set; } = new();
    public string? ErrorMessage { get; set; }
}
