namespace ScoreSharp.API.Infrastructures.JWTToken;

public interface IJWTProfilerHelper
{
    string UserId { get; }

    string[] RoleIds { get; }

    string UserName { get; }

    List<CaseDispatchGroup> CaseDispatchGroups { get; }
}
