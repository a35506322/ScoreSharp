namespace ScoreSharp.API.Infrastructures.Security;

public class PermissionAuthorizationRequirement : IAuthorizationRequirement
{
    public string PolicyName { get; init; }
}
