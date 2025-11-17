namespace ScoreSharp.API.Modules.OrgSetUp.User.GetADUserById;

public class GetADUserByIdResponse
{
    /// <summary>
    /// 範例: 陳曉明
    /// 一定要有值
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// memberOf = CN=資訊服務部,OU=資訊服務部,OU=UITC,DC=uitctech,DC=com,DC=tw
    /// 通常這個要大於0，不然他可能是電腦帳號而已
    /// </summary>
    public List<string> MemberOf { get; set; } = new();

    /// <summary>
    /// chenming
    /// </summary>
    public string? SAMAccountName { get; set; }

    /// <summary>
    /// chenming@uitc.com.tw
    /// </summary>
    public string? UserPrincipalName { get; set; }
}
