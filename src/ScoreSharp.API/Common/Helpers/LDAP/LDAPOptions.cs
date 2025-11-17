namespace ScoreSharp.API.Common.Helpers.LDAP;

public class LDAPOptions
{
    /// <summary>
    /// Ldap Server 192.168.10.250
    /// </summary>
    [Required]
    public string LdapServer { get; set; }

    /// <summary>
    /// 預設 389
    /// </summary>
    [Required]
    public int Port { get; set; }

    /// <summary>
    /// Admin 帳號 => 用來搜尋
    /// </summary>
    [Required]
    public string AdminUser { get; set; }

    [Required]
    public string AdminMima { get; set; }

    /// <summary>
    /// OU=UITC,DC=uitctech,DC=com,DC=tw
    /// </summary>
    [Required]
    public string BaseDN { get; set; }

    /// <summary>
    /// uitctech.com.tw
    /// </summary>
    [Required]
    public string Domain { get; set; }
}
