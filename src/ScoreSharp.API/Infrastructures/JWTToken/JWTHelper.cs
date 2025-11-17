namespace ScoreSharp.API.Infrastructures.JWTToken;

public class JWTHelper : IJWTHelper
{
    private readonly IConfiguration _configuration;

    public JWTHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(
        string userId,
        string name,
        List<string> roles,
        int expireMinutes = 480,
        string? organize = null,
        List<CaseDispatchGroup>? caseDispatchGroup = null
    )
    {
        var issuer = _configuration.GetValue<string>("JwtSettings:Issuer");
        var signKey = _configuration.GetValue<string>("JwtSettings:SignKey");

        // Configuring "Claims" to your JWT Token
        var claims = new List<Claim>();

        // In RFC 7519 (Section#4), there are defined 7 built-in Claims, but we mostly use 2 of them.
        //claims.Add(new Claim(JwtRegisteredClaimNames.Iss, issuer));
        //claims.Add(new Claim(JwtRegisteredClaimNames.Aud, "The Audience"));
        //claims.Add(new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddMinutes(30).ToUnixTimeSeconds().ToString()));
        //claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())); // 必須為數字
        //claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())); // 必須為數字
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())); // JWT ID

        // The "NameId" claim is usually unnecessary.
        claims.Add(new Claim(JwtRegisteredClaimNames.NameId, userId));

        // 新增姓名
        claims.Add(new Claim(JwtRegisteredClaimNames.Name, name));

        // 新增系統時間
        claims.Add(new Claim("systemdate", DateTimeOffset.UtcNow.ToString("yyyy/MM/dd")));

        // 新增 組織
        claims.Add(new Claim("organize", organize ?? string.Empty));

        // 新增 派案組織
        if (caseDispatchGroup.Count != 0)
        {
            foreach (var item in caseDispatchGroup)
            {
                claims.Add(new Claim("casedispatchgroup", $"{(int)item}"));
            }
        }
        // This Claim can be replaced by JwtRegisteredClaimNames.Sub, so it's redundant.

        // You can define your "roles" to your Claims.
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var userClaimsIdentity = new ClaimsIdentity(claims);

        // Create a SymmetricSecurityKey for JWT Token signatures
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signKey));

        // HmacSha256 MUST be larger than 128 bits, so the key can't be too short. At least 16 and more characters.
        // https://stackoverflow.com/questions/47279947/idx10603-the-algorithm-hs256-requires-the-securitykey-keysize-to-be-greater
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        // Create SecurityTokenDescriptor
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = issuer,
            //Audience = issuer, // Sometimes you don't have to define Audience.
            //NotBefore = DateTime.Now, // Default is DateTime.Now
            //IssuedAt = DateTime.Now, // Default is DateTime.Now
            Subject = userClaimsIdentity,
            Expires = DateTime.Now.AddMinutes(expireMinutes),
            SigningCredentials = signingCredentials,
        };

        // Generate a JWT securityToken, than get the serialized Token result (string)
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var serializeToken = tokenHandler.WriteToken(securityToken);

        return serializeToken;
    }

    public bool VaildToken(string token, string secretKey)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
            if (jwtToken == null)
            {
                return false;
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var validationParameters = new TokenValidationParameters()
            {
                RequireExpirationTime = true,
                ValidateIssuer = true,
                ValidateAudience = false,
                IssuerSigningKey = key,
            };

            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);
            if (principal == null)
            {
                return false;
            }

            return true;
        }
        catch
        {
            return false;
        }
    }
}
