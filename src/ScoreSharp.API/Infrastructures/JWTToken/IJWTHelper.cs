namespace ScoreSharp.API.Infrastructures.JWTToken;

public interface IJWTHelper
{
    string GenerateToken(
        string userId,
        string name,
        List<string> roles,
        int expireMinutes = 480,
        string? organize = null,
        List<CaseDispatchGroup>? caseDispatchGroup = null
    );
    bool VaildToken(string token, string secretKey);
}
