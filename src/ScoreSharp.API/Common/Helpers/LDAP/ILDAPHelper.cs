namespace ScoreSharp.API.Common.Helpers.LDAP;

public interface ILDAPHelper
{
    /// <summary>
    /// 驗證是否通過有效的AD帳號
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public bool VaildLDAPAuth(string username, string password);

    /// <summary>
    /// 搜尋全部User
    /// </summary>
    /// <returns></returns>
    public List<LDAPUserInfo> SearchUsersAll();

    /// <summary>
    /// 搜尋UserBy帳號
    /// </summary>
    /// <param name="samAccountName"></param>
    /// <returns></returns>
    public LDAPUserInfo? SearchBySAMAccountName(string samAccountName);
}
