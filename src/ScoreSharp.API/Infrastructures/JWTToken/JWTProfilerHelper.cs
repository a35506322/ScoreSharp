namespace ScoreSharp.API.Infrastructures.JWTToken;

public class JWTProfilerHelper : IJWTProfilerHelper
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public JWTProfilerHelper(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

    public string[] RoleIds =>
        _httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role).Select(x => x.Value).ToArray() ?? Array.Empty<string>();

    public string UserName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Name) ?? string.Empty;

    public List<CaseDispatchGroup> CaseDispatchGroups =>
        _httpContextAccessor.HttpContext?.User?.FindAll("casedispatchgroup").Select(x => Enum.Parse<CaseDispatchGroup>(x.Value)).ToList() ?? [];
}
