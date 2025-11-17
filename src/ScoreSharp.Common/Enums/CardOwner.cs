namespace ScoreSharp.Common.Enums;

public enum CardOwner
{
    [EnumIsActive(true)]
    正卡 = 1,

    [EnumIsActive(true)]
    附卡 = 2,

    [EnumIsActive(true)]
    正卡_附卡 = 3,
}
