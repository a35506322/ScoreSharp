namespace ScoreSharp.Common.Enums;

public enum IDTakeStatus
{
    [EnumIsActive(true)]
    初發 = 1,

    [EnumIsActive(true)]
    補發 = 2,

    [EnumIsActive(true)]
    換發 = 3,
}
