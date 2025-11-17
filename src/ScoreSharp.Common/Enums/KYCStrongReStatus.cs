namespace ScoreSharp.Common.Enums;

public enum KYCStrongReStatus
{
    [EnumIsActive(true)]
    未送審 = 1,

    [EnumIsActive(true)]
    送審中 = 2,

    [EnumIsActive(true)]
    核准 = 3,

    [EnumIsActive(true)]
    駁回 = 4,

    [EnumIsActive(true)]
    不需檢核 = 5,
}
