namespace ScoreSharp.Common.Enums;

public enum CaseChangeAction
{
    [EnumIsActive(true)]
    權限內_補件作業 = 1,

    [EnumIsActive(true)]
    權限內_撤件作業 = 2,

    [EnumIsActive(true)]
    權限內_退件作業 = 3,

    [EnumIsActive(true)]
    權限內_核卡作業 = 4,

    [EnumIsActive(true)]
    權限外_排入補件 = 5,

    [EnumIsActive(true)]
    權限外_排入撤件 = 6,

    [EnumIsActive(true)]
    權限外_排入退件 = 7,

    [EnumIsActive(true)]
    權限外_排入核卡 = 8,
}
