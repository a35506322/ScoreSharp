namespace ScoreSharp.Common.Enums;

public enum StudentApplicantRelationship
{
    [EnumIsActive(true)]
    父子 = 1,

    [EnumIsActive(true)]
    父女 = 2,

    [EnumIsActive(true)]
    母子 = 3,

    [EnumIsActive(true)]
    母女 = 4,

    [EnumIsActive(true)]
    其他 = 5,
}
