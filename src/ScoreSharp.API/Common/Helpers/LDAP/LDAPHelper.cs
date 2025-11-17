using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;

namespace ScoreSharp.API.Common.Helpers.LDAP;

public class LDAPHelper : ILDAPHelper
{
    private readonly LDAPOptions _ldapOptions;
    private readonly ILogger<LDAPHelper> _logger;
    private readonly IUITCSecurityHelper _uitcSecurityHelper;

    private const string 組織架構 = "memberOf";
    private const string 姓名 = "displayName";
    private const string 帳號 = "sAMAccountName";
    private const string 完整帳號 = "userPrincipalName";

    public LDAPHelper(IOptions<LDAPOptions> config, ILogger<LDAPHelper> logger, IUITCSecurityHelper uitcSecurityHelper)
    {
        _ldapOptions = config.Value;
        _logger = logger;
        _uitcSecurityHelper = uitcSecurityHelper;
    }

    private LdapConnection GetConnection(string user, string password)
    {
        LdapConnection connection = new LdapConnection();
        connection.Connect(_ldapOptions.LdapServer, _ldapOptions.Port);
        connection.Bind(user, password);
        return connection;
    }

    public LDAPUserInfo? SearchBySAMAccountName(string samAccountName)
    {
        try
        {
            string user = _uitcSecurityHelper.DecryptData(_ldapOptions.AdminUser);
            string mima = _uitcSecurityHelper.DecryptData(_ldapOptions.AdminMima);

            using (var connection = GetConnection(user, mima))
            {
                string searchFilter = "(&(objectClass=user)(sAMAccountName={0}))";
                string[] attrList = new string[] { 組織架構, 姓名, 帳號, 完整帳號 };
                var lsc = connection.Search(
                    _ldapOptions.BaseDN,
                    LdapConnection.ScopeSub,
                    String.Format(searchFilter, samAccountName),
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
            _logger.LogError(
                "LDAP取得單筆ADUser失敗，samAccountName：{@SamAccountName}e，Appsetting：{@Appsetting}，Error：{@Error}",
                samAccountName,
                _ldapOptions,
                ex
            );
            return null;
        }
    }

    public List<LDAPUserInfo> SearchUsersAll()
    {
        try
        {
            string user = _uitcSecurityHelper.DecryptData(_ldapOptions.AdminUser);
            string mima = _uitcSecurityHelper.DecryptData(_ldapOptions.AdminMima);

            using (var connection = GetConnection(user, mima))
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
            _logger.LogError("LDAP取得所有ADUser失敗，Appsetting：{@Appsetting}，Error：{@Error}", _ldapOptions, ex);
            return new List<LDAPUserInfo>();
        }
    }

    public bool VaildLDAPAuth(string username, string password)
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
