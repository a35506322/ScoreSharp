namespace ScoreSharp.LDAP.Services;

public interface ILDAPService
{
    /// <summary>
    /// 驗證是否通過有效的AD帳號
    /// </summary>
    Task<bool> ValidateLDAPAuth(string username, string password);

    /// <summary>
    /// 搜尋全部User
    /// </summary>
    Task<List<Models.LDAPUserInfo>> SearchUsersAll();

    /// <summary>
    /// 搜尋UserBy帳號
    /// </summary>
    Task<Models.LDAPUserInfo?> SearchBySAMAccountName(string samAccountName);
}
