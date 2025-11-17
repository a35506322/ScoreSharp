namespace ScoreSharp.Common.Enums;

public enum ExecutionAction
{
    [EnumIsActive(true)]
    權限內 = 1,

    [EnumIsActive(true)]
    權限外_轉交 = 2,

    [EnumIsActive(true)]
    退回重審 = 3,
}
