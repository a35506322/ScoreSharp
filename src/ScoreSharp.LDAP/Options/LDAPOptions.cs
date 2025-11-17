namespace ScoreSharp.LDAP.Options;

public class LDAPOptions
{
    [Required]
    public string LDAPServer { get; set; } = string.Empty;

    [Required]
    public int Port { get; set; }

    [Required]
    public string AdminUser { get; set; } = string.Empty;

    [Required]
    public string AdminMima { get; set; } = string.Empty;

    [Required]
    public string BaseDN { get; set; } = string.Empty;

    [Required]
    public string Domain { get; set; } = string.Empty;
}
