namespace ScoreSharp.Common.Enums;

public enum UserType
{
    [EnumIsActive(true)]
    [EnumName("正卡人")]
    正卡人 = 1,

    [EnumIsActive(true)]
    [EnumName("附卡人")]
    附卡人 = 2,
}
