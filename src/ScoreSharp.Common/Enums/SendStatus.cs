namespace ScoreSharp.Common.Enums;

public enum SendStatus
{
    [EnumIsActive(true)]
    等待 = 1,

    [EnumIsActive(true)]
    成功 = 2,

    [EnumIsActive(true)]
    失敗 = 3
}
