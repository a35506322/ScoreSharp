namespace ScoreSharp.LDAP.Options;

public class IPWhitelistOptions
{
    [Required]
    public List<string> AllowedIPs { get; set; } = new List<string>();
}
