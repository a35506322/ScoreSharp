namespace ScoreSharp.Common.Enums;

public enum CardStatus
{
    // Module: 紙本件
    [EnumIsActive(true)]
    紙本件_初始 = 1,

    [EnumIsActive(true)]
    紙本件_一次件檔中 = 20002,

    [EnumIsActive(true)]
    紙本件_二次件檔中 = 20004,

    [EnumIsActive(true)]
    紙本件_建檔審核中 = 20007,

    // 此為翻新系統新增狀態
    [EnumIsActive(true)]
    紙本件_待檢核 = 20100,

    // 此為翻新系統新增狀態
    [EnumIsActive(true)]
    紙本件_待月收入預審_檢核異常 = 20101,

    [EnumIsActive(true)]
    紙本件_待月收入預審 = 3,

    // Module: 網路件
    [EnumIsActive(true)]
    網路件_卡友_待檢核 = 30009,

    // 此為翻新系統新增狀態
    [EnumIsActive(true)]
    網路件_卡友_檢核異常 = 30115,

    // 此為翻新系統新增狀態
    [EnumIsActive(true)]
    網路件_卡友_檔案連線異常 = 30114,

    // 此為翻新系統新增狀態
    [EnumIsActive(true)]
    網路件_卡友_查無申請書_附件 = 30119,

    // 此為翻新系統新增狀態
    [EnumIsActive(true)]
    網路件_卡友_申請書異常 = 30111,

    // 此為翻新系統新增狀態
    [EnumIsActive(true)]
    網路件_非卡友_待檢核 = 30100,

    // 此為翻新系統新增狀態
    [EnumIsActive(true)]
    網路件_非卡友_檔案連線異常 = 30104,

    [EnumIsActive(true)]
    網路件_非卡友_申請書異常 = 30002,

    // 此為翻新系統新增狀態
    [EnumIsActive(true)]
    網路件_非卡友_查無申請書_附件 = 30105,

    // 此為翻新系統新增狀態
    [EnumIsActive(true)]
    網路件_待月收入預審_檢核異常 = 30106,

    [EnumIsActive(true)]
    網路件_待月收入預審 = 30010,

    // 此為翻新系統新增狀態
    [EnumIsActive(true)]
    申請書異常_系統撤件 = 30112,

    [EnumIsActive(true)]
    網路件_等待MyData附件 = 30035,

    [EnumIsActive(true)]
    網路件_MyData取回成功 = 30036,

    [EnumIsActive(true)]
    網路件_MyData取回失敗 = 30037,

    [EnumIsActive(true)]
    MyData取回失敗 = 30038,

    // 網路小白件 => 紙本同步
    [EnumIsActive(true)]
    網路件_書面申請等待MyData = 20012,

    [EnumIsActive(true)]
    網路件_書面申請等待列印申請書及回郵信封 = 20014,

    [EnumIsActive(true)]
    國旅人事名冊確認 = 30013,

    // 此為翻新系統新增狀態
    [EnumIsActive(true)]
    國旅人士名冊確認_系統撤件 = 30990,

    // 此為翻新系統新增狀態
    [EnumIsActive(true)]
    網路件初始_待重新發送_非定義值 = 30101,

    // 此為翻新系統新增狀態
    [EnumIsActive(true)]
    網路件初始_待重新發送_資料長度過長 = 30102,

    // 此為翻新系統新增狀態
    [EnumIsActive(true)]
    網路件初始_待重新發送_必要欄位不能為空值 = 30103,

    // 此為翻新系統新增狀態
    [EnumIsActive(true)]
    網路件初始_待重新發送_ECARD_FILE_DB_例外錯誤 = 30109,

    // 此為翻新系統新增狀態
    [EnumIsActive(true)]
    網路件初始_待重新發送_查無申請書附件檔案 = 30110,

    // 此為翻新系統新增狀態
    [EnumIsActive(true)]
    KYC入檔作業_完成卡友檢核 = 30116,

    [EnumIsActive(true)]
    網路件_卡友_完成KYC入檔作業 = 30200,

    // 案件異動 (紙本+網路共用)
    [EnumIsActive(true)]
    完成月收入確認 = 30012,

    [EnumIsActive(true)]
    完成黑名單查詢 = 30021,

    // 此為翻新系統新增狀態
    [EnumIsActive(true)]
    KYC入檔作業_紙本件待月收入預審 = 30117,

    // 此為翻新系統新增狀態
    [EnumIsActive(true)]
    KYC入檔作業_網路件待月收入預審 = 30118,

    [EnumIsActive(true)]
    人工徵信中 = 10201,

    [EnumIsActive(true)]
    申請核卡中 = 10210,

    [EnumIsActive(true)]
    核卡作業中 = 10211,

    [EnumIsActive(true)]
    申請退件中 = 10220,

    [EnumIsActive(true)]
    申請補件中 = 10230,

    [EnumIsActive(true)]
    申請撤件中 = 10240,

    // 此為翻新系統新增狀態
    [EnumIsActive(true)]
    核卡_等待完成本案徵審 = 10901,

    // 此為翻新系統新增狀態
    [EnumIsActive(true)]
    申請核卡_等待完成本案徵審 = 10911,

    // 此為翻新系統新增狀態
    [EnumIsActive(true)]
    申請退件_等待完成本案徵審 = 10912,

    // 此為翻新系統新增狀態
    [EnumIsActive(true)]
    申請補件_等待完成本案徵審 = 10913,

    // 此為翻新系統新增狀態
    [EnumIsActive(true)]
    申請撤件_等待完成本案徵審 = 10914,

    [EnumIsActive(true)]
    退件_等待完成本案徵審 = 10224,

    [EnumIsActive(true)]
    補件_等待完成本案徵審 = 10234,

    [EnumIsActive(true)]
    撤件_等待完成本案徵審 = 10244,

    [EnumIsActive(true)]
    退件作業中_終止狀態 = 10221,

    [EnumIsActive(true)]
    補件作業中 = 10231,

    [EnumIsActive(true)]
    補回件 = 10232,

    [EnumIsActive(true)]
    製卡失敗 = 10302,

    [EnumIsActive(true)]
    撤件作業中_終止狀態 = 10241,

    [EnumIsActive(true)]
    退回重審 = 10250,

    [EnumIsActive(true)]
    拒撤退重審 = 10251,
}
