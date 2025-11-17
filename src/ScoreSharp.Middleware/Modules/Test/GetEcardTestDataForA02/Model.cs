using System.Text.Json.Serialization;

namespace ScoreSharp.Middleware.Modules.Test.GetEcardTestDataForA02;

public class EcardNewCaseRequest
{
    [JsonPropertyName("APPLY_NO")]
    public string APPLY_NO { get; set; }

    [JsonPropertyName("ID_TYPE")]
    public string ID_TYPE { get; set; }

    [JsonPropertyName("CREDIT_ID")]
    public string CREDIT_ID { get; set; }

    [JsonPropertyName("CARD_OWNER")]
    public string CARD_OWNER { get; set; }

    [JsonPropertyName("MCARD_SER")]
    public string MCARD_SER { get; set; }

    [JsonPropertyName("FORM_ID")]
    public string FORM_ID { get; set; }

    [JsonPropertyName("CH_NAME")]
    public string CH_NAME { get; set; }

    [JsonPropertyName("SEX")]
    public string SEX { get; set; }

    [JsonPropertyName("BIRTHDAY")]
    public string BIRTHDAY { get; set; }

    [JsonPropertyName("ENG_NAME")]
    public string ENG_NAME { get; set; }

    [JsonPropertyName("BIRTH_PLACE")]
    public string BIRTH_PLACE { get; set; }

    [JsonPropertyName("BIRTH_PLACE_OTHER")]
    public string BIRTH_PLACE_OTHER { get; set; }

    [JsonPropertyName("NATIONALITY")]
    public string NATIONALITY { get; set; }

    [JsonPropertyName("P_ID")]
    public string P_ID { get; set; }

    [JsonPropertyName("P_ID_GETDATE")]
    public string P_ID_GETDATE { get; set; }

    [JsonPropertyName("P_ID_GETADDRNAME")]
    public string P_ID_GETADDRNAME { get; set; }

    [JsonPropertyName("P_ID_STATUS")]
    public string P_ID_STATUS { get; set; }

    [JsonPropertyName("REG_ZIP")]
    public string REG_ZIP { get; set; }

    [JsonPropertyName("REG_ADDR_CITY")]
    public string REG_ADDR_CITY { get; set; }

    [JsonPropertyName("REG_ADDR_DIST")]
    public string REG_ADDR_DIST { get; set; }

    [JsonPropertyName("REG_ADDR_RD")]
    public string REG_ADDR_RD { get; set; }

    [JsonPropertyName("REG_ADDR_LN")]
    public string REG_ADDR_LN { get; set; }

    [JsonPropertyName("REG_ADDR_ALY")]
    public string REG_ADDR_ALY { get; set; }

    [JsonPropertyName("REG_ADDR_NO")]
    public string REG_ADDR_NO { get; set; }

    [JsonPropertyName("REG_ADDR_SUBNO")]
    public string REG_ADDR_SUBNO { get; set; }

    [JsonPropertyName("REG_ADDR_F")]
    public string REG_ADDR_F { get; set; }

    [JsonPropertyName("REG_ADDR_OTHER")]
    public string REG_ADDR_OTHER { get; set; }

    [JsonPropertyName("HOME_ZIP")]
    public string HOME_ZIP { get; set; }

    [JsonPropertyName("HOME_ADDR_CITY")]
    public string HOME_ADDR_CITY { get; set; }

    [JsonPropertyName("HOME_ADDR_DIST")]
    public string HOME_ADDR_DIST { get; set; }

    [JsonPropertyName("HOME_ADDR_RD")]
    public string HOME_ADDR_RD { get; set; }

    [JsonPropertyName("HOME_ADDR_LN")]
    public string HOME_ADDR_LN { get; set; }

    [JsonPropertyName("HOME_ADDR_ALY")]
    public string HOME_ADDR_ALY { get; set; }

    [JsonPropertyName("HOME_ADDR_NO")]
    public string HOME_ADDR_NO { get; set; }

    [JsonPropertyName("HOME_ADDR_SUBNO")]
    public string HOME_ADDR_SUBNO { get; set; }

    [JsonPropertyName("HOME_ADDR_F")]
    public string HOME_ADDR_F { get; set; }

    [JsonPropertyName("HOME_ADDR_OTHER")]
    public string HOME_ADDR_OTHER { get; set; }

    [JsonPropertyName("BILL_TO_ADDR")]
    public string BILL_TO_ADDR { get; set; }

    [JsonPropertyName("CARD_TO_ADDR")]
    public string CARD_TO_ADDR { get; set; }

    [JsonPropertyName("CELL_PHONE_NO")]
    public string CELL_PHONE_NO { get; set; }

    [JsonPropertyName("EMAIL")]
    public string EMAIL { get; set; }

    [JsonPropertyName("JOB_TYPE")]
    public string JOB_TYPE { get; set; }

    [JsonPropertyName("JOB_TYPE_OTHER")]
    public string JOB_TYPE_OTHER { get; set; }

    [JsonPropertyName("JOB_LV")]
    public string JOB_LV { get; set; }

    [JsonPropertyName("COMP_NAME")]
    public string COMP_NAME { get; set; }

    [JsonPropertyName("COMP_TEL_NO")]
    public string COMP_TEL_NO { get; set; }

    [JsonPropertyName("COMP_ZIP")]
    public string COMP_ZIP { get; set; }

    [JsonPropertyName("COMP_ADDR_CITY")]
    public string COMP_ADDR_CITY { get; set; }

    [JsonPropertyName("COMP_ADDR_DIST")]
    public string COMP_ADDR_DIST { get; set; }

    [JsonPropertyName("COMP_ADDR_RD")]
    public string COMP_ADDR_RD { get; set; }

    [JsonPropertyName("COMP_ADDR_LN")]
    public string COMP_ADDR_LN { get; set; }

    [JsonPropertyName("COMP_ADDR_ALY")]
    public string COMP_ADDR_ALY { get; set; }

    [JsonPropertyName("COMP_ADDR_NO")]
    public string COMP_ADDR_NO { get; set; }

    [JsonPropertyName("COMP_ADDR_SUBNO")]
    public string COMP_ADDR_SUBNO { get; set; }

    [JsonPropertyName("COMP_ADDR_F")]
    public string COMP_ADDR_F { get; set; }

    [JsonPropertyName("COMP_ADDR_OTHER")]
    public string COMP_ADDR_OTHER { get; set; }

    [JsonPropertyName("SALARY")]
    public string SALARY { get; set; }

    [JsonPropertyName("MAIN_INCOME")]
    public string MAIN_INCOME { get; set; }

    [JsonPropertyName("MAIN_INCOME_OTHER")]
    public string MAIN_INCOME_OTHER { get; set; }

    [JsonPropertyName("ACCEPT_DM_FLG")]
    public string ACCEPT_DM_FLG { get; set; }

    [JsonPropertyName("M_PROJECT_CODE_ID")]
    public string M_PROJECT_CODE_ID { get; set; }

    [JsonPropertyName("PROM_UNIT_SER")]
    public string PROM_UNIT_SER { get; set; }

    [JsonPropertyName("PROM_USER_NAME")]
    public string PROM_USER_NAME { get; set; }

    [JsonPropertyName("AGREE_MARKETING_FLG")]
    public string AGREE_MARKETING_FLG { get; set; }

    [JsonPropertyName("NOT_ACCEPT_EPAPAER_FLG")]
    public string NOT_ACCEPT_EPAPAER_FLG { get; set; }

    [JsonPropertyName("FAMILY6_AGE")]
    public string FAMILY6_AGE { get; set; }

    [JsonPropertyName("CHG_CARD_ADDR")]
    public string CHG_CARD_ADDR { get; set; }

    [JsonPropertyName("BILL_TYPE")]
    public string BILL_TYPE { get; set; }

    [JsonPropertyName("AUTO_FLG")]
    public string AUTO_FLG { get; set; }

    [JsonPropertyName("ACCT_NO")]
    public string ACCT_NO { get; set; }

    [JsonPropertyName("PAY_WAY")]
    public string PAY_WAY { get; set; }

    [JsonPropertyName("SOURCE_TYPE")]
    public string SOURCE_TYPE { get; set; }

    [JsonPropertyName("SOURCE_IP")]
    public string SOURCE_IP { get; set; }

    [JsonPropertyName("OTP_MOBILE_PHONE")]
    public string OTP_MOBILE_PHONE { get; set; }

    [JsonPropertyName("OTP_TIME")]
    public string OTP_TIME { get; set; }

    [JsonPropertyName("DIGI_CARD_FLG")]
    public string DIGI_CARD_FLG { get; set; }

    [JsonPropertyName("CARD_KIND")]
    public string CARD_KIND { get; set; }

    [JsonPropertyName("APPLICATION_FILE_NAME")]
    public string APPLICATION_FILE_NAME { get; set; }

    [JsonPropertyName("APPENDIX_FLG")]
    public string APPENDIX_FLG { get; set; }

    [JsonPropertyName("MYDATA_NO")]
    public string MYDATA_NO { get; set; }

    [JsonPropertyName("CARD_ADDR")]
    public string CARD_ADDR { get; set; }

    [JsonPropertyName("KYC_CHG_FLG")]
    public string KYC_CHG_FLG { get; set; }

    [JsonPropertyName("CONSUM_NOTICE_FLG")]
    public string CONSUM_NOTICE_FLG { get; set; }

    [JsonPropertyName("UUID")]
    public string UUID { get; set; }

    [JsonPropertyName("DEMAND_CURR_BAL")]
    public string DEMAND_CURR_BAL { get; set; }

    [JsonPropertyName("TIME_CURR_BAL")]
    public string TIME_CURR_BAL { get; set; }

    [JsonPropertyName("DEMAND_90DAY_BAL")]
    public string DEMAND_90DAY_BAL { get; set; }

    [JsonPropertyName("TIME_90DAY_BAL")]
    public string TIME_90DAY_BAL { get; set; }

    [JsonPropertyName("BAL_UPD_DATE")]
    public string BAL_UPD_DATE { get; set; }

    [JsonPropertyName("STUDENT_FLG")]
    public string STUDENT_FLG { get; set; }

    [JsonPropertyName("PARENT_NAME")]
    public string PARENT_NAME { get; set; }

    [JsonPropertyName("PARENT_HOME_TEL_NO")]
    public string PARENT_HOME_TEL_NO { get; set; }

    [JsonPropertyName("PARENT_HOME_ZIP")]
    public string PARENT_HOME_ZIP { get; set; }

    [JsonPropertyName("PARENT_HOME_ADDR_CITY")]
    public string PARENT_HOME_ADDR_CITY { get; set; }

    [JsonPropertyName("PARENT_HOME_ADDR_DIST")]
    public string PARENT_HOME_ADDR_DIST { get; set; }

    [JsonPropertyName("PARENT_HOME_ADDR_RD")]
    public string PARENT_HOME_ADDR_RD { get; set; }

    [JsonPropertyName("PARENT_HOME_ADDR_LN")]
    public string PARENT_HOME_ADDR_LN { get; set; }

    [JsonPropertyName("PARENT_HOME_ADDR_ALY")]
    public string PARENT_HOME_ADDR_ALY { get; set; }

    [JsonPropertyName("PARENT_HOME_ADDR_NO")]
    public string PARENT_HOME_ADDR_NO { get; set; }

    [JsonPropertyName("PARENT_HOME_ADDR_SUBNO")]
    public string PARENT_HOME_ADDR_SUBNO { get; set; }

    [JsonPropertyName("PARENT_HOME_ADDR_F")]
    public string PARENT_HOME_ADDR_F { get; set; }

    [JsonPropertyName("PARENT_HOME_ADDR_OTHER")]
    public string PARENT_HOME_ADDR_OTHER { get; set; }

    [JsonPropertyName("EDUCATION")]
    public string EDUCATION { get; set; }

    [JsonPropertyName("REG_TEL_NO")]
    public string REG_TEL_NO { get; set; }

    [JsonPropertyName("HOME_TEL_NO")]
    public string HOME_TEL_NO { get; set; }

    [JsonPropertyName("HOME_ADDR_COND")]
    public string HOME_ADDR_COND { get; set; }

    [JsonPropertyName("PRIMARY_SCHOOL")]
    public string PRIMARY_SCHOOL { get; set; }

    [JsonPropertyName("COMP_ID")]
    public string COMP_ID { get; set; }

    [JsonPropertyName("JOB_TITLE")]
    public string JOB_TITLE { get; set; }

    [JsonPropertyName("JOB_YEAR")]
    public string JOB_YEAR { get; set; }

    [JsonPropertyName("AL_NO")]
    public string AL_NO { get; set; }

    [JsonPropertyName("APPENDIX_FILE_NAME_01")]
    public string APPENDIX_FILE_NAME_01 { get; set; }

    [JsonPropertyName("APPENDIX_FILE_NAME_02")]
    public string APPENDIX_FILE_NAME_02 { get; set; }

    [JsonPropertyName("APPENDIX_FILE_NAME_03")]
    public string APPENDIX_FILE_NAME_03 { get; set; }

    [JsonPropertyName("APPENDIX_FILE_NAME_04")]
    public string APPENDIX_FILE_NAME_04 { get; set; }

    [JsonPropertyName("APPENDIX_FILE_NAME_05")]
    public string APPENDIX_FILE_NAME_05 { get; set; }

    [JsonPropertyName("APPENDIX_FILE_NAME_06")]
    public string APPENDIX_FILE_NAME_06 { get; set; }

    [JsonPropertyName("APPENDIX_FILE_NAME_07")]
    public string APPENDIX_FILE_NAME_07 { get; set; }

    [JsonPropertyName("APPENDIX_FILE_NAME_08")]
    public string APPENDIX_FILE_NAME_08 { get; set; }
}

public enum 測試種類
{
    原卡友 = 10,
}
