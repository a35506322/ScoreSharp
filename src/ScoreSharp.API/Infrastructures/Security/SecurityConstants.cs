namespace ScoreSharp.API.Infrastructures.Security;

public static class SecurityConstants
{
    public static class Policy
    {
        public const string RBAC = "Policy-RBAC";
    }

    public static class PolicyRedisKey
    {
        public const string RoleAction = "POLICY:ROLEAUTH";
        public const string Action = "POLICY:ACTION";
    }

    public static class PolicyRedisTag
    {
        public const string RoleAction = "POLICY_ROLEAUTH";
        public const string Action = "POLICY_ACTION";
    }
}
