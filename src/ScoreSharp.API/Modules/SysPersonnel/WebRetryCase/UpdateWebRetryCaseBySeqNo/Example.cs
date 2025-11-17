namespace ScoreSharp.API.Modules.SysPersonnel.WebRetryCase.UpdateWebRetryCaseBySeqNo;

[ExampleAnnotation(Name = "[2000]修改單筆網路件", ExampleType = ExampleType.Request)]
public class 修改單筆網路件_2000_ReqEx : IExampleProvider<UpdateWebRetryCaseBySeqNoRequest>
{
    public UpdateWebRetryCaseBySeqNoRequest GetExample()
    {
        UpdateWebRetryCaseBySeqNoRequest request = new UpdateWebRetryCaseBySeqNoRequest()
        {
            SeqNo = 73,
            Request =
                "{\"APPLY_NO\":\"20250124B0006\",\"ID_TYPE\":\"存戶\",\"CREDIT_ID\":\"\",\"CARD_OWNER\":\"1\",\"MCARD_SER\":\"JST59\",\"FORM_ID\":\"AP00A171\",\"CH_NAME\":\"陳婉婷\",\"SEX\":\"1\",\"BIRTHDAY\":\"0710817\",\"ENG_NAME\":\"CHEN WAN TING\",\"BIRTH_PLACE\":\"彰化縣\",\"BIRTH_PLACE_OTHER\":\"\",\"NATIONALITY\":\"TW\",\"P_ID\":\"R120667319\",\"P_ID_GETDATE\":\"0920505\",\"P_ID_GETADDRNAME\":\"新北市\",\"P_ID_STATUS\":\"1\",\"REG_ZIP\":\"\",\"REG_ADDR_CITY\":\"新北市\",\"REG_ADDR_DIST\":\"土城區\",\"REG_ADDR_RD\":\"福安街\",\"REG_ADDR_LN\":\"258\",\"REG_ADDR_ALY\":\"91\",\"REG_ADDR_NO\":\"123\",\"REG_ADDR_SUBNO\":\"56\",\"REG_ADDR_F\":\"78\",\"REG_ADDR_OTHER\":\"90\",\"HOME_ZIP\":\"\",\"HOME_ADDR_CITY\":\"新北市\",\"HOME_ADDR_DIST\":\"土城區\",\"HOME_ADDR_RD\":\"福安街\",\"HOME_ADDR_LN\":\"258\",\"HOME_ADDR_ALY\":\"91\",\"HOME_ADDR_NO\":\"123\",\"HOME_ADDR_SUBNO\":\"56\",\"HOME_ADDR_F\":\"78\",\"HOME_ADDR_OTHER\":\"90\",\"BILL_TO_ADDR\":\"3\",\"CARD_TO_ADDR\":\"2\",\"CELL_PHONE_NO\":\"0958445587\",\"EMAIL\":\"\",\"JOB_TYPE\":\"14\",\"JOB_TYPE_OTHER\":\"\",\"JOB_LV\":\"1\",\"COMP_NAME\":\"聯邦網通\",\"COMP_TEL_NO\":\"02-55845347#197\",\"COMP_ZIP\":\"\",\"COMP_ADDR_CITY\":\"嘉義市\",\"COMP_ADDR_DIST\":\"西區\",\"COMP_ADDR_RD\":\"八德路\",\"COMP_ADDR_LN\":\"\",\"COMP_ADDR_ALY\":\"\",\"COMP_ADDR_NO\":\"123\",\"COMP_ADDR_SUBNO\":\"\",\"COMP_ADDR_F\":\"\",\"COMP_ADDR_OTHER\":\"\",\"SALARY\":\"50000\",\"MAIN_INCOME\":\"2,4\",\"MAIN_INCOME_OTHER\":\"\",\"ACCEPT_DM_FLG\":\"1\",\"M_PROJECT_CODE_ID\":\"ECU\",\"PROM_UNIT_SER\":\"DEP\",\"PROM_USER_NAME\":\"1001234\",\"AGREE_MARKETING_FLG\":\"1\",\"NOT_ACCEPT_EPAPAER_FLG\":\"1\",\"FAMILY6_AGE\":\"1\",\"CHG_CARD_ADDR\":\"\",\"BILL_TYPE\":\"5\",\"AUTO_FLG\":\"\",\"ACCT_NO\":\"\",\"PAY_WAY\":\"\",\"SOURCE_TYPE\":\"ECARD\",\"SOURCE_IP\":\"122.99.27.3\",\"OTP_MOBILE_PHONE\":\"0958445587\",\"OTP_TIME\":\"2025/01/24 13:56:24\",\"DIGI_CARD_FLG\":\"N\",\"CARD_KIND\":\"1\",\"APPLICATION_FILE_NAME\":\"12413564265502335\",\"APPENDIX_FLG\":\"\",\"MYDATA_NO\":\"\",\"CARD_ADDR\":\"\",\"KYC_CHG_FLG\":\"\",\"CONSUM_NOTICE_FLG\":\"Y\",\"UUID\":\"\",\"DEMAND_CURR_BAL\":\"\",\"TIME_CURR_BAL\":\"\",\"DEMAND_90DAY_BAL\":\"\",\"TIME_90DAY_BAL\":\"\",\"BAL_UPD_DATE\":\"\",\"STUDENT_FLG\":\"N\",\"PARENT_NAME\":\"\",\"PARENT_HOME_TEL_NO\":\"\",\"PARENT_HOME_ZIP\":\"\",\"PARENT_HOME_ADDR_CITY\":\"\",\"PARENT_HOME_ADDR_DIST\":\"\",\"PARENT_HOME_ADDR_RD\":\"\",\"PARENT_HOME_ADDR_LN\":\"\",\"PARENT_HOME_ADDR_ALY\":\"\",\"PARENT_HOME_ADDR_NO\":\"\",\"PARENT_HOME_ADDR_SUBNO\":\"\",\"PARENT_HOME_ADDR_F\":\"\",\"PARENT_HOME_ADDR_OTHER\":\"\",\"EDUCATION\":\"\",\"REG_TEL_NO\":\"02-99398343\",\"HOME_TEL_NO\":\"02-22466249\",\"HOME_ADDR_COND\":\"5\",\"PRIMARY_SCHOOL\":\"中山國小\",\"COMP_ID\":\"40853948\",\"JOB_TITLE\":\"業務經理\",\"JOB_YEAR\":\"\",\"AL_NO\":\"\",\"APPENDIX_FILE_NAME_01\":\"\",\"APPENDIX_FILE_NAME_02\":\"\",\"APPENDIX_FILE_NAME_03\":\"\",\"APPENDIX_FILE_NAME_04\":\"\",\"APPENDIX_FILE_NAME_05\":\"\",\"APPENDIX_FILE_NAME_06\":\"\",\"APPENDIX_FILE_NAME_07\":\"\",\"APPENDIX_FILE_NAME_08\":\"\"}",
        };

        return request;
    }
}

[ExampleAnnotation(Name = "[2000]修改單筆網路件", ExampleType = ExampleType.Response)]
public class 修改單筆網路件_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess<string>("73", "73");
    }
}

[ExampleAnnotation(Name = "[4003]修改單筆網路件-Request格式有誤", ExampleType = ExampleType.Request)]
public class 修改單筆網路件格式有誤_4003_ReqEx : IExampleProvider<UpdateWebRetryCaseBySeqNoRequest>
{
    public UpdateWebRetryCaseBySeqNoRequest GetExample()
    {
        UpdateWebRetryCaseBySeqNoRequest request = new UpdateWebRetryCaseBySeqNoRequest()
        {
            SeqNo = 73,
            Request =
                "{1\"APPLY_NO\":\"20250124B0006\",\"ID_TYPE\":\"存戶\",\"CREDIT_ID\":\"\",\"CARD_OWNER\":\"1\",\"MCARD_SER\":\"JST59\",\"FORM_ID\":\"AP00A171\",\"CH_NAME\":\"陳婉婷\",\"SEX\":\"1\",\"BIRTHDAY\":\"0710817\",\"ENG_NAME\":\"CHEN WAN TING\",\"BIRTH_PLACE\":\"彰化縣\",\"BIRTH_PLACE_OTHER\":\"\",\"NATIONALITY\":\"TW\",\"P_ID\":\"R120667319\",\"P_ID_GETDATE\":\"0920505\",\"P_ID_GETADDRNAME\":\"新北市\",\"P_ID_STATUS\":\"1\",\"REG_ZIP\":\"\",\"REG_ADDR_CITY\":\"新北市\",\"REG_ADDR_DIST\":\"土城區\",\"REG_ADDR_RD\":\"福安街\",\"REG_ADDR_LN\":\"258\",\"REG_ADDR_ALY\":\"91\",\"REG_ADDR_NO\":\"123\",\"REG_ADDR_SUBNO\":\"56\",\"REG_ADDR_F\":\"78\",\"REG_ADDR_OTHER\":\"90\",\"HOME_ZIP\":\"\",\"HOME_ADDR_CITY\":\"新北市\",\"HOME_ADDR_DIST\":\"土城區\",\"HOME_ADDR_RD\":\"福安街\",\"HOME_ADDR_LN\":\"258\",\"HOME_ADDR_ALY\":\"91\",\"HOME_ADDR_NO\":\"123\",\"HOME_ADDR_SUBNO\":\"56\",\"HOME_ADDR_F\":\"78\",\"HOME_ADDR_OTHER\":\"90\",\"BILL_TO_ADDR\":\"3\",\"CARD_TO_ADDR\":\"2\",\"CELL_PHONE_NO\":\"0958445587\",\"EMAIL\":\"\",\"JOB_TYPE\":\"14\",\"JOB_TYPE_OTHER\":\"\",\"JOB_LV\":\"1\",\"COMP_NAME\":\"聯邦網通\",\"COMP_TEL_NO\":\"02-55845347#197\",\"COMP_ZIP\":\"\",\"COMP_ADDR_CITY\":\"嘉義市\",\"COMP_ADDR_DIST\":\"西區\",\"COMP_ADDR_RD\":\"八德路\",\"COMP_ADDR_LN\":\"\",\"COMP_ADDR_ALY\":\"\",\"COMP_ADDR_NO\":\"123\",\"COMP_ADDR_SUBNO\":\"\",\"COMP_ADDR_F\":\"\",\"COMP_ADDR_OTHER\":\"\",\"SALARY\":\"50000\",\"MAIN_INCOME\":\"2,4\",\"MAIN_INCOME_OTHER\":\"\",\"ACCEPT_DM_FLG\":\"1\",\"M_PROJECT_CODE_ID\":\"ECU\",\"PROM_UNIT_SER\":\"DEP\",\"PROM_USER_NAME\":\"1001234\",\"AGREE_MARKETING_FLG\":\"1\",\"NOT_ACCEPT_EPAPAER_FLG\":\"1\",\"FAMILY6_AGE\":\"1\",\"CHG_CARD_ADDR\":\"\",\"BILL_TYPE\":\"3\",\"AUTO_FLG\":\"\",\"ACCT_NO\":\"\",\"PAY_WAY\":\"\",\"SOURCE_TYPE\":\"ECARD\",\"SOURCE_IP\":\"122.99.27.3\",\"OTP_MOBILE_PHONE\":\"0958445587\",\"OTP_TIME\":\"2025/01/24 13:56:24\",\"DIGI_CARD_FLG\":\"N\",\"CARD_KIND\":\"1\",\"APPLICATION_FILE_NAME\":\"12413564265502335\",\"APPENDIX_FLG\":\"\",\"MYDATA_NO\":\"\",\"CARD_ADDR\":\"\",\"KYC_CHG_FLG\":\"\",\"CONSUM_NOTICE_FLG\":\"Y\",\"UUID\":\"\",\"DEMAND_CURR_BAL\":\"\",\"TIME_CURR_BAL\":\"\",\"DEMAND_90DAY_BAL\":\"\",\"TIME_90DAY_BAL\":\"\",\"BAL_UPD_DATE\":\"\",\"STUDENT_FLG\":\"N\",\"PARENT_NAME\":\"\",\"PARENT_HOME_TEL_NO\":\"\",\"PARENT_HOME_ZIP\":\"\",\"PARENT_HOME_ADDR_CITY\":\"\",\"PARENT_HOME_ADDR_DIST\":\"\",\"PARENT_HOME_ADDR_RD\":\"\",\"PARENT_HOME_ADDR_LN\":\"\",\"PARENT_HOME_ADDR_ALY\":\"\",\"PARENT_HOME_ADDR_NO\":\"\",\"PARENT_HOME_ADDR_SUBNO\":\"\",\"PARENT_HOME_ADDR_F\":\"\",\"PARENT_HOME_ADDR_OTHER\":\"\",\"EDUCATION\":\"\",\"REG_TEL_NO\":\"02-99398343\",\"HOME_TEL_NO\":\"02-22466249\",\"HOME_ADDR_COND\":\"5\",\"PRIMARY_SCHOOL\":\"中山國小\",\"COMP_ID\":\"40853948\",\"JOB_TITLE\":\"業務經理\",\"JOB_YEAR\":\"\",\"AL_NO\":\"\",\"APPENDIX_FILE_NAME_01\":\"\",\"APPENDIX_FILE_NAME_02\":\"\",\"APPENDIX_FILE_NAME_03\":\"\",\"APPENDIX_FILE_NAME_04\":\"\",\"APPENDIX_FILE_NAME_05\":\"\",\"APPENDIX_FILE_NAME_06\":\"\",\"APPENDIX_FILE_NAME_07\":\"\",\"APPENDIX_FILE_NAME_08\":\"\"}",
        };

        return request;
    }
}

[ExampleAnnotation(Name = "[4003]修改單筆網路件-Request格式有誤", ExampleType = ExampleType.Response)]
public class 修改單筆網路件格式有誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>(null, "錯誤訊息");
    }
}

[ExampleAnnotation(Name = "[4001]修改單筆網路件-查無資料", ExampleType = ExampleType.Response)]
public class 修改單筆網路件查無資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "2");
    }
}

[ExampleAnnotation(Name = "[4003]修改單筆網路件-呼叫有誤", ExampleType = ExampleType.Response)]
public class 修改單筆網路件呼叫有誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
