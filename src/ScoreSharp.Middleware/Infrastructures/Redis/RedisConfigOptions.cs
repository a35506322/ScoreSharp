namespace ScoreSharp.Middleware.Infrastructures.Redis;

public class RedisConfigOptions
{
    public string[] EndPoints { get; set; } = Array.Empty<string>();
    public int DefaultDatabase { get; set; }
    public string Mima { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
}
