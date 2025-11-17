using ScoreSharp.Common.Helpers;
using ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore.EcardNewCase.Models;

namespace ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore.EcardNewCase;

[ExampleAnnotation(Name = "[0000]ECARD進件-非卡友非存戶", ExampleType = ExampleType.Request)]
public class ECARD進件非卡友非存戶_0000_ReqEx : IExampleProvider<EcardNewCaseRequest>
{
    public EcardNewCaseRequest GetExample()
    {
        string jsonString =
            "{\"APPLY_NO\":\"20240821X0001\",\"UITC_ENCRYPT\":\"Y\",\"ID_TYPE\":\"\",\"CREDIT_ID\":\"\",\"CARD_OWNER\":\"1\",\"MCARD_SER\":\"JST59\",\"FORM_ID\":\"AP00A169\",\"CH_NAME\":\"痾\",\"SEX\":\"1\",\"BIRTHDAY\":\"0950201\",\"ENG_NAME\":\"L\",\"BIRTH_PLACE\":\"屏東縣\",\"BIRTH_PLACE_OTHER\":\"\",\"NATIONALITY\":\"TW\",\"P_ID\":\"A182345678\",\"P_ID_GETDATE\":\"1130101\",\"P_ID_GETADDRNAME\":\"北市\",\"P_ID_STATUS\":\"1\",\"REG_ZIP\":\"\",\"REG_ADDR_CITY\":\"臺北市\",\"REG_ADDR_DIST\":\"內湖區\",\"REG_ADDR_RD\":\"民權東路六段\",\"REG_ADDR_LN\":\"283\",\"REG_ADDR_ALY\":\"165\",\"REG_ADDR_NO\":\"218\",\"REG_ADDR_SUBNO\":\"\",\"REG_ADDR_F\":\"\",\"REG_ADDR_OTHER\":\"\",\"HOME_ZIP\":\"\",\"HOME_ADDR_CITY\":\"臺北市\",\"HOME_ADDR_DIST\":\"內湖區\",\"HOME_ADDR_RD\":\"民權東路六段\",\"HOME_ADDR_LN\":\"283\",\"HOME_ADDR_ALY\":\"165\",\"HOME_ADDR_NO\":\"218\",\"HOME_ADDR_SUBNO\":\"\",\"HOME_ADDR_F\":\"\",\"HOME_ADDR_OTHER\":\"\",\"BILL_TO_ADDR\":\"3\",\"CARD_TO_ADDR\":\"3\",\"CELL_PHONE_NO\":\"0955555555\",\"EMAIL\":\"\",\"JOB_TYPE\":\"17\",\"JOB_TYPE_OTHER\":\"\",\"JOB_LV\":\"1\",\"COMP_NAME\":\"l\",\"COMP_TEL_NO\":\"\",\"COMP_ZIP\":\"\",\"COMP_ADDR_CITY\":\"臺北市\",\"COMP_ADDR_DIST\":\"士林區\",\"COMP_ADDR_RD\":\"下樹林街\",\"COMP_ADDR_LN\":\"5\",\"COMP_ADDR_ALY\":\"5\",\"COMP_ADDR_NO\":\"5\",\"COMP_ADDR_SUBNO\":\"\",\"COMP_ADDR_F\":\"\",\"COMP_ADDR_OTHER\":\"\",\"SALARY\":\"555\",\"MAIN_INCOME\":\"2\",\"MAIN_INCOME_OTHER\":\"\",\"ACCEPT_DM_FLG\":\"1\",\"M_PROJECT_CODE_ID\":\"ENP\",\"PROM_UNIT_SER\":\"\",\"PROM_USER_NAME\":\"\",\"AGREE_MARKETING_FLG\":\"1\",\"NOT_ACCEPT_EPAPAER_FLG\":\"1\",\"FAMILY6_AGE\":\"11\",\"CHG_CARD_ADDR\":\"\",\"BILL_TYPE\":\"2\",\"AUTO_FLG\":\"\",\"ACCT_NO\":\"\",\"PAY_WAY\":\"\",\"SOURCE_TYPE\":\"ECARD\",\"SOURCE_IP\":\"122.99.27.3\",\"OTP_MOBILE_PHONE\":\"0955555555\",\"OTP_TIME\":\"2024/08/21 15:55:24\",\"DIGI_CARD_FLG\":\"N\",\"CARD_KIND\":\"1\",\"APPLICATION_FILE_NAME\":\"82115564675906792\",\"APPENDIX_FLG\":\"\",\"MYDATA_NO\":\"\",\"CARD_ADDR\":\"\",\"KYC_CHG_FLG\":\"\",\"CONSUM_NOTICE_FLG\":\"Y\",\"UUID\":\"\",\"DEMAND_CURR_BAL\":\"\",\"TIME_CURR_BAL\":\"\",\"DEMAND_90DAY_BAL\":\"\",\"TIME_90DAY_BAL\":\"\",\"BAL_UPD_DATE\":\"\",\"STUDENT_FLG\":\"N\",\"PARENT_NAME\":\"\",\"PARENT_HOME_TEL_NO\":\"\",\"PARENT_HOME_ZIP\":\"\",\"PARENT_HOME_ADDR_CITY\":\"\",\"PARENT_HOME_ADDR_DIST\":\"\",\"PARENT_HOME_ADDR_RD\":\"\",\"PARENT_HOME_ADDR_LN\":\"\",\"PARENT_HOME_ADDR_ALY\":\"\",\"PARENT_HOME_ADDR_NO\":\"\",\"PARENT_HOME_ADDR_SUBNO\":\"\",\"PARENT_HOME_ADDR_F\":\"\",\"PARENT_HOME_ADDR_OTHER\":\"\",\"EDUCATION\":\"\",\"REG_TEL_NO\":\"\",\"HOME_TEL_NO\":\"\",\"HOME_ADDR_COND\":\"\",\"PRIMARY_SCHOOL\":\"l\",\"COMP_ID\":\"\",\"JOB_TITLE\":\"\",\"JOB_YEAR\":\"\",\"AL_NO\":\"\",\"APPENDIX_FILE_NAME_01\":\"\",\"APPENDIX_FILE_NAME_02\":\"\",\"APPENDIX_FILE_NAME_03\":\"\",\"APPENDIX_FILE_NAME_04\":\"\",\"APPENDIX_FILE_NAME_05\":\"\",\"APPENDIX_FILE_NAME_06\":\"\",\"APPENDIX_FILE_NAME_07\":\"\",\"APPENDIX_FILE_NAME_08\":\"\"}";
        var data = JsonHelper.反序列化物件不分大小寫<EcardNewCaseRequest>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[0000]ECARD進件-非卡友存戶", ExampleType = ExampleType.Request)]
public class ECARD進件非卡友存戶_0000_ReqEx : IExampleProvider<EcardNewCaseRequest>
{
    public EcardNewCaseRequest GetExample()
    {
        string jsonString =
            "{\"APPLY_NO\":\"20240820B0003\",\"UITC_ENCRYPT\":\"Y\",\"ID_TYPE\":\"存戶\",\"CREDIT_ID\":\"\",\"CARD_OWNER\":\"1\",\"MCARD_SER\":\"JS59\",\"FORM_ID\":\"AP00A171\",\"CH_NAME\":\"李測\",\"SEX\":\"1\",\"BIRTHDAY\":\"0800101\",\"ENG_NAME\":\"UNION BANK\",\"BIRTH_PLACE\":\"臺北市\",\"BIRTH_PLACE_OTHER\":\"\",\"NATIONALITY\":\"TW\",\"P_ID\":\"K121223932\",\"P_ID_GETDATE\":\"1110203\",\"P_ID_GETADDRNAME\":\"嘉市\",\"P_ID_STATUS\":\"3\",\"REG_ZIP\":\"\",\"REG_ADDR_CITY\":\"臺北市\",\"REG_ADDR_DIST\":\"松山區\",\"REG_ADDR_RD\":\"市民大道六段\",\"REG_ADDR_LN\":\"\",\"REG_ADDR_ALY\":\"\",\"REG_ADDR_NO\":\"200\",\"REG_ADDR_SUBNO\":\"\",\"REG_ADDR_F\":\"5\",\"REG_ADDR_OTHER\":\"\",\"HOME_ZIP\":\"\",\"HOME_ADDR_CITY\":\"臺北市\",\"HOME_ADDR_DIST\":\"松山區\",\"HOME_ADDR_RD\":\"市民大道六段\",\"HOME_ADDR_LN\":\"\",\"HOME_ADDR_ALY\":\"\",\"HOME_ADDR_NO\":\"200\",\"HOME_ADDR_SUBNO\":\"\",\"HOME_ADDR_F\":\"5\",\"HOME_ADDR_OTHER\":\"\",\"BILL_TO_ADDR\":\"1\",\"CARD_TO_ADDR\":\"1\",\"CELL_PHONE_NO\":\"0911123122\",\"EMAIL\":\"1234-CHUN_KAO@UBOT.COM.TW\",\"JOB_TYPE\":\"16\",\"JOB_TYPE_OTHER\":\"\",\"JOB_LV\":\"1\",\"COMP_NAME\":\"您好公司\",\"COMP_TEL_NO\":\"02-25151110#123\",\"COMP_ZIP\":\"\",\"COMP_ADDR_CITY\":\"臺北市\",\"COMP_ADDR_DIST\":\"內湖區\",\"COMP_ADDR_RD\":\"民權東路六段\",\"COMP_ADDR_LN\":\"\",\"COMP_ADDR_ALY\":\"\",\"COMP_ADDR_NO\":\"50\",\"COMP_ADDR_SUBNO\":\"\",\"COMP_ADDR_F\":\"10\",\"COMP_ADDR_OTHER\":\"\",\"SALARY\":\"50000\",\"MAIN_INCOME\":\"2,1\",\"MAIN_INCOME_OTHER\":\"\",\"ACCEPT_DM_FLG\":\"1\",\"M_PROJECT_CODE_ID\":\"ETU\",\"PROM_UNIT_SER\":\"911T\",\"PROM_USER_NAME\":\"1131111\",\"AGREE_MARKETING_FLG\":\"1\",\"NOT_ACCEPT_EPAPAER_FLG\":\"1\",\"FAMILY6_AGE\":\"11\",\"CHG_CARD_ADDR\":\"\",\"BILL_TYPE\":\"3\",\"AUTO_FLG\":\"\",\"ACCT_NO\":\"\",\"PAY_WAY\":\"\",\"SOURCE_TYPE\":\"ECARD\",\"SOURCE_IP\":\"61.66.79.187\",\"OTP_MOBILE_PHONE\":\"0911123122\",\"OTP_TIME\":\"2024/08/20 10:27:28\",\"DIGI_CARD_FLG\":\"N\",\"CARD_KIND\":\"1\",\"APPLICATION_FILE_NAME\":\"82010291532655855\",\"APPENDIX_FLG\":\"\",\"MYDATA_NO\":\"\",\"CARD_ADDR\":\"\",\"KYC_CHG_FLG\":\"\",\"CONSUM_NOTICE_FLG\":\"Y\",\"UUID\":\"\",\"DEMAND_CURR_BAL\":\"\",\"TIME_CURR_BAL\":\"\",\"DEMAND_90DAY_BAL\":\"\",\"TIME_90DAY_BAL\":\"\",\"BAL_UPD_DATE\":\"\",\"STUDENT_FLG\":\"N\",\"PARENT_NAME\":\"\",\"PARENT_HOME_TEL_NO\":\"\",\"PARENT_HOME_ZIP\":\"\",\"PARENT_HOME_ADDR_CITY\":\"\",\"PARENT_HOME_ADDR_DIST\":\"\",\"PARENT_HOME_ADDR_RD\":\"\",\"PARENT_HOME_ADDR_LN\":\"\",\"PARENT_HOME_ADDR_ALY\":\"\",\"PARENT_HOME_ADDR_NO\":\"\",\"PARENT_HOME_ADDR_SUBNO\":\"\",\"PARENT_HOME_ADDR_F\":\"\",\"PARENT_HOME_ADDR_OTHER\":\"\",\"EDUCATION\":\"3\",\"REG_TEL_NO\":\"02-27192233\",\"HOME_TEL_NO\":\"02-27192211\",\"HOME_ADDR_COND\":\"1\",\"PRIMARY_SCHOOL\":\"中山國小\",\"COMP_ID\":\"\",\"JOB_TITLE\":\"業務\",\"JOB_YEAR\":\"10\",\"AL_NO\":\"\",\"APPENDIX_FILE_NAME_01\":\"\",\"APPENDIX_FILE_NAME_02\":\"\",\"APPENDIX_FILE_NAME_03\":\"\",\"APPENDIX_FILE_NAME_04\":\"\",\"APPENDIX_FILE_NAME_05\":\"\",\"APPENDIX_FILE_NAME_06\":\"\",\"APPENDIX_FILE_NAME_07\":\"\",\"APPENDIX_FILE_NAME_08\":\"\"}";
        var data = JsonHelper.反序列化物件不分大小寫<EcardNewCaseRequest>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[0000]ECARD進件-卡友", ExampleType = ExampleType.Request)]
public class ECARD進件卡友_0000_ReqEx : IExampleProvider<EcardNewCaseRequest>
{
    public EcardNewCaseRequest GetExample()
    {
        string jsonString =
            "{\"applY_NO\": \"20241011B7532\",\"iD_TYPE\": \"卡友\",\"crediT_ID\": \"A02\",\"carD_OWNER\": \"1\",\"mcarD_SER\": \"JST59\",\"forM_ID\": \"AP00A169\",\"cH_NAME\": \"覃梓晨\",\"sex\": \"1\",\"birthday\": \"0721205\",\"enG_NAME\": \"CassinTyler\",\"birtH_PLACE\": \"南投縣\",\"birtH_PLACE_OTHER\": \"\",\"nationality\": \"TW\",\"p_ID\": \"U193254471\",\"p_ID_GETDATE\": \"\",\"p_ID_GETADDRNAME\": \"\",\"p_ID_STATUS\": \"\",\"reG_ZIP\": \"26646\",\"reG_ADDR_CITY\": \"\",\"reG_ADDR_DIST\": \"\",\"reG_ADDR_RD\": \"\",\"reG_ADDR_LN\": \"\",\"reG_ADDR_ALY\": \"\",\"reG_ADDR_NO\": \"\",\"reG_ADDR_SUBNO\": \"\",\"reG_ADDR_F\": \"\",\"reG_ADDR_OTHER\": \"宜蘭縣三星鄉尚健五路二段249號3樓\",\"homE_ZIP\": \"\",\"homE_ADDR_CITY\": \"\",\"homE_ADDR_DIST\": \"\",\"homE_ADDR_RD\": \"\",\"homE_ADDR_LN\": \"\",\"homE_ADDR_ALY\": \"\",\"homE_ADDR_NO\": \"\",\"homE_ADDR_SUBNO\": \"\",\"homE_ADDR_F\": \"\",\"homE_ADDR_OTHER\": \"\",\"bilL_TO_ADDR\": \"\",\"carD_TO_ADDR\": \"\",\"celL_PHONE_NO\": \"0911239456\",\"email\": \"16@hotmail.com\",\"joB_TYPE\": \"8\",\"joB_TYPE_OTHER\": \"\",\"joB_LV\": \"5\",\"comP_NAME\": \"\",\"comP_TEL_NO\": \"\",\"comP_ZIP\": \"\",\"comP_ADDR_CITY\": \"\",\"comP_ADDR_DIST\": \"\",\"comP_ADDR_RD\": \"\",\"comP_ADDR_LN\": \"\",\"comP_ADDR_ALY\": \"\",\"comP_ADDR_NO\": \"\",\"comP_ADDR_SUBNO\": \"\",\"comP_ADDR_F\": \"\",\"comP_ADDR_OTHER\": \"\",\"salary\": \"\",\"maiN_INCOME\": \"4,5\",\"maiN_INCOME_OTHER\": \"\",\"accepT_DM_FLG\": \"1\",\"m_PROJECT_CODE_ID\": \"OTN\",\"proM_UNIT_SER\": \"911T\",\"proM_USER_NAME\": \"1001234\",\"agreE_MARKETING_FLG\": \"1\",\"noT_ACCEPT_EPAPAER_FLG\": \"1\",\"familY6_AGE\": \"\",\"chG_CARD_ADDR\": \"\",\"bilL_TYPE\": \"4\",\"autO_FLG\": \"\",\"accT_NO\": \"\",\"paY_WAY\": \"\",\"sourcE_TYPE\": \"ECARD\",\"sourcE_IP\": \"76.109.59.57\",\"otP_MOBILE_PHONE\": \"0971344939\",\"otP_TIME\": \"2024/10/10 23:14:27\",\"digI_CARD_FLG\": \"Y\",\"carD_KIND\": \"1\",\"applicatioN_FILE_NAME\": \"\",\"appendiX_FLG\": \"\",\"mydatA_NO\": \"\",\"carD_ADDR\": \"桃園市楊梅區蘋果路7弄7號之7號7樓\",\"kyC_CHG_FLG\": \"\",\"consuM_NOTICE_FLG\": \"Y\",\"uuid\": \"\",\"demanD_CURR_BAL\": \"\",\"timE_CURR_BAL\": \"\",\"demanD_90DAY_BAL\": \"\",\"timE_90DAY_BAL\": \"\",\"baL_UPD_DATE\": \"\",\"studenT_FLG\": \"N\",\"parenT_NAME\": \"\",\"parenT_HOME_TEL_NO\": \"\",\"parenT_HOME_ZIP\": \"\",\"parenT_HOME_ADDR_CITY\": \"\",\"parenT_HOME_ADDR_DIST\": \"\",\"parenT_HOME_ADDR_RD\": \"\",\"parenT_HOME_ADDR_LN\": \"\",\"parenT_HOME_ADDR_ALY\": \"\",\"parenT_HOME_ADDR_NO\": \"\",\"parenT_HOME_ADDR_SUBNO\": \"\",\"parenT_HOME_ADDR_F\": \"\",\"parenT_HOME_ADDR_OTHER\": \"\",\"education\": \"\",\"reG_TEL_NO\": \"\",\"homE_TEL_NO\": \"\",\"homE_ADDR_COND\": \"\",\"primarY_SCHOOL\": \"\",\"comP_ID\": \"\",\"joB_TITLE\": \"\",\"joB_YEAR\": \"\",\"aL_NO\": \"\",\"appendiX_FILE_NAME_01\": \"\",\"appendiX_FILE_NAME_02\": \"\",\"appendiX_FILE_NAME_03\": \"\",\"appendiX_FILE_NAME_04\": \"\",\"appendiX_FILE_NAME_05\": \"\",\"appendiX_FILE_NAME_06\": \"\",\"appendiX_FILE_NAME_07\": \"\",\"appendiX_FILE_NAME_08\": \"\"}";
        var data = JsonHelper.反序列化物件不分大小寫<EcardNewCaseRequest>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[0001]ECARD進件-申請書編號長度不符", ExampleType = ExampleType.Request)]
public class ECARD進件申請書編號長度不符_0001_ReqEx : IExampleProvider<EcardNewCaseRequest>
{
    public EcardNewCaseRequest GetExample()
    {
        string jsonString =
            "{\"APPLY_NO\":\"20240820B000312\",\"UITC_ENCRYPT\":\"Y\",\"ID_TYPE\":\"存戶\",\"CREDIT_ID\":\"\",\"CARD_OWNER\":\"1\",\"MCARD_SER\":\"JS59\",\"FORM_ID\":\"AP00A171\",\"CH_NAME\":\"李測\",\"SEX\":\"1\",\"BIRTHDAY\":\"0800101\",\"ENG_NAME\":\"UNION BANK\",\"BIRTH_PLACE\":\"臺北市\",\"BIRTH_PLACE_OTHER\":\"\",\"NATIONALITY\":\"TW\",\"P_ID\":\"K121223932\",\"P_ID_GETDATE\":\"1110203\",\"P_ID_GETADDRNAME\":\"嘉市\",\"P_ID_STATUS\":\"3\",\"REG_ZIP\":\"\",\"REG_ADDR_CITY\":\"臺北市\",\"REG_ADDR_DIST\":\"松山區\",\"REG_ADDR_RD\":\"市民大道六段\",\"REG_ADDR_LN\":\"\",\"REG_ADDR_ALY\":\"\",\"REG_ADDR_NO\":\"200\",\"REG_ADDR_SUBNO\":\"\",\"REG_ADDR_F\":\"5\",\"REG_ADDR_OTHER\":\"\",\"HOME_ZIP\":\"\",\"HOME_ADDR_CITY\":\"臺北市\",\"HOME_ADDR_DIST\":\"松山區\",\"HOME_ADDR_RD\":\"市民大道六段\",\"HOME_ADDR_LN\":\"\",\"HOME_ADDR_ALY\":\"\",\"HOME_ADDR_NO\":\"200\",\"HOME_ADDR_SUBNO\":\"\",\"HOME_ADDR_F\":\"5\",\"HOME_ADDR_OTHER\":\"\",\"BILL_TO_ADDR\":\"1\",\"CARD_TO_ADDR\":\"1\",\"CELL_PHONE_NO\":\"0911123122\",\"EMAIL\":\"1234-CHUN_KAO@UBOT.COM.TW\",\"JOB_TYPE\":\"16\",\"JOB_TYPE_OTHER\":\"\",\"JOB_LV\":\"1\",\"COMP_NAME\":\"您好公司\",\"COMP_TEL_NO\":\"02-25151110#123\",\"COMP_ZIP\":\"\",\"COMP_ADDR_CITY\":\"臺北市\",\"COMP_ADDR_DIST\":\"內湖區\",\"COMP_ADDR_RD\":\"民權東路六段\",\"COMP_ADDR_LN\":\"\",\"COMP_ADDR_ALY\":\"\",\"COMP_ADDR_NO\":\"50\",\"COMP_ADDR_SUBNO\":\"\",\"COMP_ADDR_F\":\"10\",\"COMP_ADDR_OTHER\":\"\",\"SALARY\":\"50000\",\"MAIN_INCOME\":\"2,1\",\"MAIN_INCOME_OTHER\":\"\",\"ACCEPT_DM_FLG\":\"1\",\"M_PROJECT_CODE_ID\":\"ETU\",\"PROM_UNIT_SER\":\"911T\",\"PROM_USER_NAME\":\"1131111\",\"AGREE_MARKETING_FLG\":\"1\",\"NOT_ACCEPT_EPAPAER_FLG\":\"1\",\"FAMILY6_AGE\":\"11\",\"CHG_CARD_ADDR\":\"\",\"BILL_TYPE\":\"3\",\"AUTO_FLG\":\"\",\"ACCT_NO\":\"\",\"PAY_WAY\":\"\",\"SOURCE_TYPE\":\"ECARD\",\"SOURCE_IP\":\"61.66.79.187\",\"OTP_MOBILE_PHONE\":\"0911123122\",\"OTP_TIME\":\"2024/08/20 10:27:28\",\"DIGI_CARD_FLG\":\"N\",\"CARD_KIND\":\"1\",\"APPLICATION_FILE_NAME\":\"82010291532655855\",\"APPENDIX_FLG\":\"\",\"MYDATA_NO\":\"\",\"CARD_ADDR\":\"\",\"KYC_CHG_FLG\":\"\",\"CONSUM_NOTICE_FLG\":\"Y\",\"UUID\":\"\",\"DEMAND_CURR_BAL\":\"\",\"TIME_CURR_BAL\":\"\",\"DEMAND_90DAY_BAL\":\"\",\"TIME_90DAY_BAL\":\"\",\"BAL_UPD_DATE\":\"\",\"STUDENT_FLG\":\"N\",\"PARENT_NAME\":\"\",\"PARENT_HOME_TEL_NO\":\"\",\"PARENT_HOME_ZIP\":\"\",\"PARENT_HOME_ADDR_CITY\":\"\",\"PARENT_HOME_ADDR_DIST\":\"\",\"PARENT_HOME_ADDR_RD\":\"\",\"PARENT_HOME_ADDR_LN\":\"\",\"PARENT_HOME_ADDR_ALY\":\"\",\"PARENT_HOME_ADDR_NO\":\"\",\"PARENT_HOME_ADDR_SUBNO\":\"\",\"PARENT_HOME_ADDR_F\":\"\",\"PARENT_HOME_ADDR_OTHER\":\"\",\"EDUCATION\":\"3\",\"REG_TEL_NO\":\"02-27192233\",\"HOME_TEL_NO\":\"02-27192211\",\"HOME_ADDR_COND\":\"1\",\"PRIMARY_SCHOOL\":\"中山國小\",\"COMP_ID\":\"\",\"JOB_TITLE\":\"業務\",\"JOB_YEAR\":\"10\",\"AL_NO\":\"\",\"APPENDIX_FILE_NAME_01\":\"\",\"APPENDIX_FILE_NAME_02\":\"\",\"APPENDIX_FILE_NAME_03\":\"\",\"APPENDIX_FILE_NAME_04\":\"\",\"APPENDIX_FILE_NAME_05\":\"\",\"APPENDIX_FILE_NAME_06\":\"\",\"APPENDIX_FILE_NAME_07\":\"\",\"APPENDIX_FILE_NAME_08\":\"\"}";
        var data = JsonHelper.反序列化物件不分大小寫<EcardNewCaseRequest>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[0003]ECARD進件-申請書編號重複進件或申請書編號不對", ExampleType = ExampleType.Request)]
public class ECARD進件申請書編號重複進件或申請書編號不對_0003_ReqEx : IExampleProvider<EcardNewCaseRequest>
{
    public EcardNewCaseRequest GetExample()
    {
        string jsonString =
            "{\"APPLY_NO\":\"20240820B0003\",\"UITC_ENCRYPT\":\"Y\",\"ID_TYPE\":\"存戶\",\"CREDIT_ID\":\"\",\"CARD_OWNER\":\"1\",\"MCARD_SER\":\"JS59\",\"FORM_ID\":\"AP00A171\",\"CH_NAME\":\"李測\",\"SEX\":\"1\",\"BIRTHDAY\":\"0800101\",\"ENG_NAME\":\"UNION BANK\",\"BIRTH_PLACE\":\"臺北市\",\"BIRTH_PLACE_OTHER\":\"\",\"NATIONALITY\":\"TW\",\"P_ID\":\"K121223932\",\"P_ID_GETDATE\":\"1110203\",\"P_ID_GETADDRNAME\":\"嘉市\",\"P_ID_STATUS\":\"3\",\"REG_ZIP\":\"\",\"REG_ADDR_CITY\":\"臺北市\",\"REG_ADDR_DIST\":\"松山區\",\"REG_ADDR_RD\":\"市民大道六段\",\"REG_ADDR_LN\":\"\",\"REG_ADDR_ALY\":\"\",\"REG_ADDR_NO\":\"200\",\"REG_ADDR_SUBNO\":\"\",\"REG_ADDR_F\":\"5\",\"REG_ADDR_OTHER\":\"\",\"HOME_ZIP\":\"\",\"HOME_ADDR_CITY\":\"臺北市\",\"HOME_ADDR_DIST\":\"松山區\",\"HOME_ADDR_RD\":\"市民大道六段\",\"HOME_ADDR_LN\":\"\",\"HOME_ADDR_ALY\":\"\",\"HOME_ADDR_NO\":\"200\",\"HOME_ADDR_SUBNO\":\"\",\"HOME_ADDR_F\":\"5\",\"HOME_ADDR_OTHER\":\"\",\"BILL_TO_ADDR\":\"1\",\"CARD_TO_ADDR\":\"1\",\"CELL_PHONE_NO\":\"0911123122\",\"EMAIL\":\"1234-CHUN_KAO@UBOT.COM.TW\",\"JOB_TYPE\":\"16\",\"JOB_TYPE_OTHER\":\"\",\"JOB_LV\":\"1\",\"COMP_NAME\":\"您好公司\",\"COMP_TEL_NO\":\"02-25151110#123\",\"COMP_ZIP\":\"\",\"COMP_ADDR_CITY\":\"臺北市\",\"COMP_ADDR_DIST\":\"內湖區\",\"COMP_ADDR_RD\":\"民權東路六段\",\"COMP_ADDR_LN\":\"\",\"COMP_ADDR_ALY\":\"\",\"COMP_ADDR_NO\":\"50\",\"COMP_ADDR_SUBNO\":\"\",\"COMP_ADDR_F\":\"10\",\"COMP_ADDR_OTHER\":\"\",\"SALARY\":\"50000\",\"MAIN_INCOME\":\"2,1\",\"MAIN_INCOME_OTHER\":\"\",\"ACCEPT_DM_FLG\":\"1\",\"M_PROJECT_CODE_ID\":\"ETU\",\"PROM_UNIT_SER\":\"911T\",\"PROM_USER_NAME\":\"1131111\",\"AGREE_MARKETING_FLG\":\"1\",\"NOT_ACCEPT_EPAPAER_FLG\":\"1\",\"FAMILY6_AGE\":\"11\",\"CHG_CARD_ADDR\":\"\",\"BILL_TYPE\":\"3\",\"AUTO_FLG\":\"\",\"ACCT_NO\":\"\",\"PAY_WAY\":\"\",\"SOURCE_TYPE\":\"ECARD\",\"SOURCE_IP\":\"61.66.79.187\",\"OTP_MOBILE_PHONE\":\"0911123122\",\"OTP_TIME\":\"2024/08/20 10:27:28\",\"DIGI_CARD_FLG\":\"N\",\"CARD_KIND\":\"1\",\"APPLICATION_FILE_NAME\":\"82010291532655855\",\"APPENDIX_FLG\":\"\",\"MYDATA_NO\":\"\",\"CARD_ADDR\":\"\",\"KYC_CHG_FLG\":\"\",\"CONSUM_NOTICE_FLG\":\"Y\",\"UUID\":\"\",\"DEMAND_CURR_BAL\":\"\",\"TIME_CURR_BAL\":\"\",\"DEMAND_90DAY_BAL\":\"\",\"TIME_90DAY_BAL\":\"\",\"BAL_UPD_DATE\":\"\",\"STUDENT_FLG\":\"N\",\"PARENT_NAME\":\"\",\"PARENT_HOME_TEL_NO\":\"\",\"PARENT_HOME_ZIP\":\"\",\"PARENT_HOME_ADDR_CITY\":\"\",\"PARENT_HOME_ADDR_DIST\":\"\",\"PARENT_HOME_ADDR_RD\":\"\",\"PARENT_HOME_ADDR_LN\":\"\",\"PARENT_HOME_ADDR_ALY\":\"\",\"PARENT_HOME_ADDR_NO\":\"\",\"PARENT_HOME_ADDR_SUBNO\":\"\",\"PARENT_HOME_ADDR_F\":\"\",\"PARENT_HOME_ADDR_OTHER\":\"\",\"EDUCATION\":\"3\",\"REG_TEL_NO\":\"02-27192233\",\"HOME_TEL_NO\":\"02-27192211\",\"HOME_ADDR_COND\":\"1\",\"PRIMARY_SCHOOL\":\"中山國小\",\"COMP_ID\":\"\",\"JOB_TITLE\":\"業務\",\"JOB_YEAR\":\"10\",\"AL_NO\":\"\",\"APPENDIX_FILE_NAME_01\":\"\",\"APPENDIX_FILE_NAME_02\":\"\",\"APPENDIX_FILE_NAME_03\":\"\",\"APPENDIX_FILE_NAME_04\":\"\",\"APPENDIX_FILE_NAME_05\":\"\",\"APPENDIX_FILE_NAME_06\":\"\",\"APPENDIX_FILE_NAME_07\":\"\",\"APPENDIX_FILE_NAME_08\":\"\"}";
        var data = JsonHelper.反序列化物件不分大小寫<EcardNewCaseRequest>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[0005]ECARD進件-資料異常非定義值", ExampleType = ExampleType.Request)]
public class ECARD進件資料異常非定義值_0005_ReqEx : IExampleProvider<EcardNewCaseRequest>
{
    public EcardNewCaseRequest GetExample()
    {
        string jsonString =
            "{\"APPLY_NO\":\"20240820B0012\",\"UITC_ENCRYPT\":\"Y\",\"ID_TYPE\":\"存戶\",\"CREDIT_ID\":\"\",\"CARD_OWNER\":\"1\",\"MCARD_SER\":\"JS59\",\"FORM_ID\":\"AP00A171\",\"CH_NAME\":\"李園伊\",\"SEX\":\"1\",\"BIRTHDAY\":\"0800101\",\"ENG_NAME\":\"UNION BANK\",\"BIRTH_PLACE\":\"臺北市\",\"BIRTH_PLACE_OTHER\":\"\",\"NATIONALITY\":\"TW\",\"P_ID\":\"K121225932\",\"P_ID_GETDATE\":\"1110203\",\"P_ID_GETADDRNAME\":\"嘉市\",\"P_ID_STATUS\":\"3\",\"REG_ZIP\":\"\",\"REG_ADDR_CITY\":\"臺北市\",\"REG_ADDR_DIST\":\"松山區\",\"REG_ADDR_RD\":\"市民大道六段\",\"REG_ADDR_LN\":\"\",\"REG_ADDR_ALY\":\"\",\"REG_ADDR_NO\":\"200\",\"REG_ADDR_SUBNO\":\"\",\"REG_ADDR_F\":\"5\",\"REG_ADDR_OTHER\":\"\",\"HOME_ZIP\":\"\",\"HOME_ADDR_CITY\":\"臺北市\",\"HOME_ADDR_DIST\":\"松山區\",\"HOME_ADDR_RD\":\"市民大道六段\",\"HOME_ADDR_LN\":\"\",\"HOME_ADDR_ALY\":\"\",\"HOME_ADDR_NO\":\"200\",\"HOME_ADDR_SUBNO\":\"\",\"HOME_ADDR_F\":\"8\",\"HOME_ADDR_OTHER\":\"\",\"BILL_TO_ADDR\":\"5\",\"CARD_TO_ADDR\":\"4\",\"CELL_PHONE_NO\":\"0911123122\",\"EMAIL\":\"1234-CHUN_KAO@UBOT.COM.TW\",\"JOB_TYPE\":\"16\",\"JOB_TYPE_OTHER\":\"\",\"JOB_LV\":\"1\",\"COMP_NAME\":\"您好公司\",\"COMP_TEL_NO\":\"02-25151110#123\",\"COMP_ZIP\":\"\",\"COMP_ADDR_CITY\":\"臺北市\",\"COMP_ADDR_DIST\":\"內湖區\",\"COMP_ADDR_RD\":\"民權東路六段\",\"COMP_ADDR_LN\":\"\",\"COMP_ADDR_ALY\":\"\",\"COMP_ADDR_NO\":\"50\",\"COMP_ADDR_SUBNO\":\"\",\"COMP_ADDR_F\":\"10\",\"COMP_ADDR_OTHER\":\"\",\"SALARY\":\"50000\",\"MAIN_INCOME\":\"2,1\",\"MAIN_INCOME_OTHER\":\"\",\"ACCEPT_DM_FLG\":\"1\",\"M_PROJECT_CODE_ID\":\"ETU\",\"PROM_UNIT_SER\":\"911T\",\"PROM_USER_NAME\":\"1131111\",\"AGREE_MARKETING_FLG\":\"1\",\"NOT_ACCEPT_EPAPAER_FLG\":\"1\",\"FAMILY6_AGE\":\"11\",\"CHG_CARD_ADDR\":\"\",\"BILL_TYPE\":\"3\",\"AUTO_FLG\":\"\",\"ACCT_NO\":\"\",\"PAY_WAY\":\"\",\"SOURCE_TYPE\":\"ECARD\",\"SOURCE_IP\":\"61.66.79.187\",\"OTP_MOBILE_PHONE\":\"0911123122\",\"OTP_TIME\":\"2024/08/20 10:27:28\",\"DIGI_CARD_FLG\":\"N\",\"CARD_KIND\":\"1\",\"APPLICATION_FILE_NAME\":\"82010291532655855\",\"APPENDIX_FLG\":\"\",\"MYDATA_NO\":\"\",\"CARD_ADDR\":\"\",\"KYC_CHG_FLG\":\"\",\"CONSUM_NOTICE_FLG\":\"Y\",\"UUID\":\"\",\"DEMAND_CURR_BAL\":\"\",\"TIME_CURR_BAL\":\"\",\"DEMAND_90DAY_BAL\":\"\",\"TIME_90DAY_BAL\":\"\",\"BAL_UPD_DATE\":\"\",\"STUDENT_FLG\":\"N\",\"PARENT_NAME\":\"\",\"PARENT_HOME_TEL_NO\":\"\",\"PARENT_HOME_ZIP\":\"\",\"PARENT_HOME_ADDR_CITY\":\"\",\"PARENT_HOME_ADDR_DIST\":\"\",\"PARENT_HOME_ADDR_RD\":\"\",\"PARENT_HOME_ADDR_LN\":\"\",\"PARENT_HOME_ADDR_ALY\":\"\",\"PARENT_HOME_ADDR_NO\":\"\",\"PARENT_HOME_ADDR_SUBNO\":\"\",\"PARENT_HOME_ADDR_F\":\"\",\"PARENT_HOME_ADDR_OTHER\":\"\",\"EDUCATION\":\"3\",\"REG_TEL_NO\":\"02-27192233\",\"HOME_TEL_NO\":\"02-27192211\",\"HOME_ADDR_COND\":\"1\",\"PRIMARY_SCHOOL\":\"中山國小\",\"COMP_ID\":\"\",\"JOB_TITLE\":\"業務\",\"JOB_YEAR\":\"10\",\"AL_NO\":\"\",\"APPENDIX_FILE_NAME_01\":\"\",\"APPENDIX_FILE_NAME_02\":\"\",\"APPENDIX_FILE_NAME_03\":\"\",\"APPENDIX_FILE_NAME_04\":\"\",\"APPENDIX_FILE_NAME_05\":\"\",\"APPENDIX_FILE_NAME_06\":\"\",\"APPENDIX_FILE_NAME_07\":\"\",\"APPENDIX_FILE_NAME_08\":\"\"}";
        var data = JsonHelper.反序列化物件不分大小寫<EcardNewCaseRequest>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[0007]ECARD進件-必要欄位不能為空值", ExampleType = ExampleType.Request)]
public class ECARD進件必要欄位不能為空值_0007_ReqEx : IExampleProvider<EcardNewCaseRequest>
{
    public EcardNewCaseRequest GetExample()
    {
        string jsonString =
            "{\"APPLY_NO\":\"20240820B0012\",\"UITC_ENCRYPT\":\"Y\",\"ID_TYPE\":\"存戶\",\"CREDIT_ID\":\"\",\"CARD_OWNER\":\"1\",\"MCARD_SER\":\"JS59\",\"FORM_ID\":\"AP00A171\",\"CH_NAME\":\"李園伊\",\"SEX\":\"1\",\"BIRTHDAY\":\"\",\"ENG_NAME\":\"UNION BANK\",\"BIRTH_PLACE\":\"臺北市\",\"BIRTH_PLACE_OTHER\":\"\",\"NATIONALITY\":\"TW\",\"P_ID\":\"K121225932\",\"P_ID_GETDATE\":\"1110203\",\"P_ID_GETADDRNAME\":\"嘉市\",\"P_ID_STATUS\":\"3\",\"REG_ZIP\":\"\",\"REG_ADDR_CITY\":\"臺北市\",\"REG_ADDR_DIST\":\"松山區\",\"REG_ADDR_RD\":\"市民大道六段\",\"REG_ADDR_LN\":\"\",\"REG_ADDR_ALY\":\"\",\"REG_ADDR_NO\":\"200\",\"REG_ADDR_SUBNO\":\"\",\"REG_ADDR_F\":\"5\",\"REG_ADDR_OTHER\":\"\",\"HOME_ZIP\":\"\",\"HOME_ADDR_CITY\":\"臺北市\",\"HOME_ADDR_DIST\":\"松山區\",\"HOME_ADDR_RD\":\"市民大道六段\",\"HOME_ADDR_LN\":\"\",\"HOME_ADDR_ALY\":\"\",\"HOME_ADDR_NO\":\"200\",\"HOME_ADDR_SUBNO\":\"\",\"HOME_ADDR_F\":\"8\",\"HOME_ADDR_OTHER\":\"\",\"BILL_TO_ADDR\":\"1\",\"CARD_TO_ADDR\":\"1\",\"CELL_PHONE_NO\":\"0911123122\",\"EMAIL\":\"1234-CHUN_KAO@UBOT.COM.TW\",\"JOB_TYPE\":\"16\",\"JOB_TYPE_OTHER\":\"\",\"JOB_LV\":\"1\",\"COMP_NAME\":\"您好公司\",\"COMP_TEL_NO\":\"02-25151110#123\",\"COMP_ZIP\":\"\",\"COMP_ADDR_CITY\":\"臺北市\",\"COMP_ADDR_DIST\":\"內湖區\",\"COMP_ADDR_RD\":\"民權東路六段\",\"COMP_ADDR_LN\":\"\",\"COMP_ADDR_ALY\":\"\",\"COMP_ADDR_NO\":\"50\",\"COMP_ADDR_SUBNO\":\"\",\"COMP_ADDR_F\":\"10\",\"COMP_ADDR_OTHER\":\"\",\"SALARY\":\"50000\",\"MAIN_INCOME\":\"2,1\",\"MAIN_INCOME_OTHER\":\"\",\"ACCEPT_DM_FLG\":\"1\",\"M_PROJECT_CODE_ID\":\"ETU\",\"PROM_UNIT_SER\":\"911T\",\"PROM_USER_NAME\":\"1131111\",\"AGREE_MARKETING_FLG\":\"1\",\"NOT_ACCEPT_EPAPAER_FLG\":\"1\",\"FAMILY6_AGE\":\"11\",\"CHG_CARD_ADDR\":\"\",\"BILL_TYPE\":\"3\",\"AUTO_FLG\":\"\",\"ACCT_NO\":\"\",\"PAY_WAY\":\"\",\"SOURCE_TYPE\":\"\",\"SOURCE_IP\":\"61.66.79.187\",\"OTP_MOBILE_PHONE\":\"0911123122\",\"OTP_TIME\":\"2024/08/20 10:27:28\",\"DIGI_CARD_FLG\":\"N\",\"CARD_KIND\":\"1\",\"APPLICATION_FILE_NAME\":\"82010291532655855\",\"APPENDIX_FLG\":\"\",\"MYDATA_NO\":\"\",\"CARD_ADDR\":\"\",\"KYC_CHG_FLG\":\"\",\"CONSUM_NOTICE_FLG\":\"Y\",\"UUID\":\"\",\"DEMAND_CURR_BAL\":\"\",\"TIME_CURR_BAL\":\"\",\"DEMAND_90DAY_BAL\":\"\",\"TIME_90DAY_BAL\":\"\",\"BAL_UPD_DATE\":\"\",\"STUDENT_FLG\":\"N\",\"PARENT_NAME\":\"\",\"PARENT_HOME_TEL_NO\":\"\",\"PARENT_HOME_ZIP\":\"\",\"PARENT_HOME_ADDR_CITY\":\"\",\"PARENT_HOME_ADDR_DIST\":\"\",\"PARENT_HOME_ADDR_RD\":\"\",\"PARENT_HOME_ADDR_LN\":\"\",\"PARENT_HOME_ADDR_ALY\":\"\",\"PARENT_HOME_ADDR_NO\":\"\",\"PARENT_HOME_ADDR_SUBNO\":\"\",\"PARENT_HOME_ADDR_F\":\"\",\"PARENT_HOME_ADDR_OTHER\":\"\",\"EDUCATION\":\"3\",\"REG_TEL_NO\":\"02-27192233\",\"HOME_TEL_NO\":\"02-27192211\",\"HOME_ADDR_COND\":\"1\",\"PRIMARY_SCHOOL\":\"中山國小\",\"COMP_ID\":\"\",\"JOB_TITLE\":\"業務\",\"JOB_YEAR\":\"10\",\"AL_NO\":\"\",\"APPENDIX_FILE_NAME_01\":\"\",\"APPENDIX_FILE_NAME_02\":\"\",\"APPENDIX_FILE_NAME_03\":\"\",\"APPENDIX_FILE_NAME_04\":\"\",\"APPENDIX_FILE_NAME_05\":\"\",\"APPENDIX_FILE_NAME_06\":\"\",\"APPENDIX_FILE_NAME_07\":\"\",\"APPENDIX_FILE_NAME_08\":\"\"}";
        var data = JsonHelper.反序列化物件不分大小寫<EcardNewCaseRequest>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[0000]ECARD進件-匯入成功", ExampleType = ExampleType.Response)]
public class ECARD進件匯入成功_0000_ResEx : IExampleProvider<EcardNewCaseResponse>
{
    public EcardNewCaseResponse GetExample()
    {
        string jsonString = "{\"ID\": \"0000\",\"RESULT\": \"匯入成功\"}";
        var data = JsonHelper.反序列化物件不分大小寫<EcardNewCaseResponse>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[0001]ECARD進件-申請書編號長度不符", ExampleType = ExampleType.Response)]
public class ECARD進件申請書編號長度不符_0001_ResEx : IExampleProvider<EcardNewCaseResponse>
{
    public EcardNewCaseResponse GetExample()
    {
        string jsonString = "{\"ID\": \"0001\",\"RESULT\": \"申請書編號長度不符\"}";
        var data = JsonHelper.反序列化物件不分大小寫<EcardNewCaseResponse>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[0003]ECARD進件-申請書編號重複進件或申請書編號不對", ExampleType = ExampleType.Response)]
public class ECARD進件申請書編號重複進件或申請書編號不對_0003_ResEx : IExampleProvider<EcardNewCaseResponse>
{
    public EcardNewCaseResponse GetExample()
    {
        string jsonString = "{\"ID\": \"0003\",\"RESULT\": \"申請書編號重複進件或申請書編號不對\"}";
        var data = JsonHelper.反序列化物件不分大小寫<EcardNewCaseResponse>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[0005]ECARD進件-資料異常非定義值", ExampleType = ExampleType.Response)]
public class ECARD進件資料異常非定義值_0005_ResEx : IExampleProvider<EcardNewCaseResponse>
{
    public EcardNewCaseResponse GetExample()
    {
        string jsonString = "{\"ID\": \"0005\",\"RESULT\": \"資料異常非定義值\"}";
        var data = JsonHelper.反序列化物件不分大小寫<EcardNewCaseResponse>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[0007]ECARD進件-必要欄位不能為空值", ExampleType = ExampleType.Response)]
public class ECARD進件必要欄位不能為空值_0007_ResEx : IExampleProvider<EcardNewCaseResponse>
{
    public EcardNewCaseResponse GetExample()
    {
        string jsonString = "{\"ID\": \"0007\",\"RESULT\": \"必要欄位不能為空值\"}";
        var data = JsonHelper.反序列化物件不分大小寫<EcardNewCaseResponse>(jsonString);
        return data;
    }
}
