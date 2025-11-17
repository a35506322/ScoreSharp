namespace ScoreSharp.LDAP.Services;

public class LDAPService : ILDAPService
{
    private readonly LDAPOptions _ldapOptions;
    private readonly ILogger<LDAPService> _logger;
    private readonly IUITCSecurityHelper _uitcSecurityHelper;

    private const string 組織架構 = "memberOf";
    private const string 姓名 = "displayName";
    private const string 帳號 = "sAMAccountName";
    private const string 完整帳號 = "userPrincipalName";

    public LDAPService(IOptions<LDAPOptions> config, ILogger<LDAPService> logger, IUITCSecurityHelper uitcSecurityHelper)
    {
        _ldapOptions = config.Value;
        _logger = logger;
        _uitcSecurityHelper = uitcSecurityHelper;
    }

    private LdapConnection GetConnection(string user, string password)
    {
        string decryptedUser = _uitcSecurityHelper.DecryptData(user);
        string decryptedPassword = _uitcSecurityHelper.DecryptData(password);

        LdapConnection connection = new LdapConnection();
        connection.Connect(_ldapOptions.LDAPServer, _ldapOptions.Port);
        connection.Bind(decryptedUser, decryptedPassword);
        return connection;
    }

    public async Task<LDAPUserInfo?> SearchBySAMAccountName(string samAccountName)
    {
        try
        {
            using (var connection = GetConnection(_ldapOptions.AdminUser, _ldapOptions.AdminMima))
            {
                string searchFilter = "(&(objectClass=user)(sAMAccountName={0}))";
                string[] attrList = new string[] { 組織架構, 姓名, 帳號, 完整帳號 };
                var lsc = connection.Search(
                    _ldapOptions.BaseDN,
                    LdapConnection.ScopeSub,
                    string.Format(searchFilter, samAccountName),
                    attrList,
                    false
                );

                if (!lsc.HasMore())
                {
                    return null;
                }

                LDAPUserInfo info = new LDAPUserInfo();
                while (lsc.HasMore())
                {
                    LdapEntry nextEntry = lsc.Next();
                    var account = nextEntry.GetAttribute(帳號);
                    if (account != null && account.StringValue == samAccountName)
                    {
                        foreach (var item in nextEntry.GetAttributeSet())
                        {
                            if (item.Name == 姓名)
                            {
                                info.DisplayName = item.StringValue;
                            }
                            else if (item.Name == 組織架構)
                            {
                                info.MemberOf.AddRange(item.StringValueArray);
                            }
                            else if (item.Name == 帳號)
                            {
                                info.SAMAccountName = item.StringValue;
                            }
                            else if (item.Name == 完整帳號)
                            {
                                info.UserPrincipalName = item.StringValue;
                            }
                        }
                        break;
                    }
                }
                return info;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("LDAP取得單筆ADUser失敗，samAccountName：{@SamAccountName}，Error：{@Error}", samAccountName, ex);
            return null;
        }
    }

    public async Task<List<LDAPUserInfo>> SearchUsersAll()
    {
        try
        {
            using (var connection = GetConnection(_ldapOptions.AdminUser, _ldapOptions.AdminMima))
            {
                string searchFilter = "(objectClass=user)";
                string[] attrList = new string[] { 組織架構, 姓名, 帳號, 完整帳號 };
                var lsc = connection.Search(_ldapOptions.BaseDN, LdapConnection.ScopeSub, searchFilter, attrList, false);

                List<LDAPUserInfo> result = new List<LDAPUserInfo>();
                while (lsc.HasMore())
                {
                    LdapEntry nextEntry = lsc.Next();
                    if (nextEntry is null)
                    {
                        continue;
                    }

                    LDAPUserInfo dto = new LDAPUserInfo();
                    foreach (var item in nextEntry.GetAttributeSet())
                    {
                        if (item.Name == 姓名)
                        {
                            dto.DisplayName = item.StringValue;
                        }
                        else if (item.Name == 組織架構)
                        {
                            dto.MemberOf.AddRange(item.StringValueArray);
                        }
                        else if (item.Name == 帳號)
                        {
                            dto.SAMAccountName = item.StringValue;
                        }
                        else if (item.Name == 完整帳號)
                        {
                            dto.UserPrincipalName = item.StringValue;
                        }
                    }
                    result.Add(dto);
                }
                return result;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("LDAP取得所有ADUser失敗，Error：{@Error}", ex);
            return new List<LDAPUserInfo>();
        }
    }

    public async Task<bool> ValidateLDAPAuth(string username, string password)
    {
        try
        {
            string user = $"{username}@{_ldapOptions.Domain}";
            using (var connection = GetConnection(user, password))
            {
                return connection.Bound;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("{@Username}-LDAP驗證失敗：Error：{@Error}", username, ex);
            return false;
        }
    }
}
