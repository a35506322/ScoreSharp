namespace ScoreSharp.Common.Enums;

public enum CaseOfAction
{
    [EnumIsActive(true)]
    [EnumName("案件種類_一般件")]
    案件種類_一般件 = 1,

    [EnumIsActive(true)]
    [EnumName("案件種類_急件")]
    案件種類_急件 = 2,

    [EnumIsActive(true)]
    [EnumName("案件種類_緊急製卡")]
    案件種類_緊急製卡 = 3,

    [EnumIsActive(true)]
    [EnumName("狀態_補回件")]
    狀態_補回件 = 4,
}
