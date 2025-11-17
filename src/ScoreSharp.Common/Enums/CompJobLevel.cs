namespace ScoreSharp.Common.Enums;

public enum CompJobLevel
{
    [EnumIsActive(true)]
    [EnumName("駕駛人員")]
    駕駛人員 = 1,

    [EnumIsActive(true)]
    [EnumName("服務生/門市人員")]
    服務生_門市人員 = 2,

    [EnumIsActive(true)]
    [EnumName("專業人員")]
    專業人員 = 3,

    [EnumIsActive(true)]
    [EnumName("專業技工")]
    專業技工 = 4,

    [EnumIsActive(true)]
    [EnumName("業務人員")]
    業務人員 = 5,

    [EnumIsActive(true)]
    [EnumName("一般職員")]
    一般職員 = 6,

    [EnumIsActive(true)]
    [EnumName("主管階層")]
    主管階層 = 7,

    [EnumIsActive(true)]
    [EnumName("股東/董事/負責人")]
    股東_董事_負責人 = 8,

    [EnumIsActive(true)]
    [EnumName("家管/其他")]
    家管_其他 = 9,
}
