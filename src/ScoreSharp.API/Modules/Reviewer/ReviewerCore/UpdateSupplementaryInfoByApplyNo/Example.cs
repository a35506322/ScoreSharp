namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateSupplementaryInfoByApplyNo;

[ExampleAnnotation(Name = "[2000]修改申請書附卡人資料", ExampleType = ExampleType.Request)]
public class 修改申請書附卡人資料_2000_ReqEx : IExampleProvider<UpdateSupplementaryInfoByApplyNoRequest>
{
    public UpdateSupplementaryInfoByApplyNoRequest GetExample()
    {
        var jsonstring =
            "{\"applyNo\": \"20250701990001\",\"chName\": \"佐藤杏\",\"id\": \"F122345678\",\"sex\": 2,\"birthDay\": \"0800930\",\"enName\": \"SATO XIN\",\"marriageState\": 3,\"applicantRelationship\": 1,\"amlRiskLevel\": null,\"citizenshipCode\": \"TW\",\"idIssueDate\": \"0900930\",\"idCardRenewalLocationCode\": \"65000000\",\"idTakeStatus\": 3,\"birthCitizenshipCode\": 1,\"birthCitizenshipCodeOther\": null,\"isFATCAIdentity\": \"N\",\"socialSecurityCode\": null,\"isForeverResidencePermit\": null,\"residencePermitIssueDate\": null,\"residencePermitDeadline\": null,\"residencePermitBackendNum\": null,\"passportNo\": null,\"passportDate\": null,\"expatValidityPeriod\": null,\"oldCertificateVerified\": null,\"compName\": \"佐藤工作室\",\"compJobTitle\": \"負責人\",\"amlProfessionCode\": \"1\",\"amlProfessionOther\": null,\"compPhone\": \"09461083537\",\"livePhone\": \"02-40791838\",\"mobile\": \"0906147533\",\"live_ZipCode\": \"25005\",\"live_City\": \"臺東縣\",\"live_District\": \"卑南鄉\",\"live_Road\": \"稻葉路\",\"live_Lane\": \"16\",\"live_Alley\": \"2\",\"live_Number\": \"56\",\"live_SubNumber\": \"16\",\"live_Floor\": \"10\",\"live_Other\": \"昊強大樓A棟\",\"sendCard_ZipCode\": \"14931\",\"sendCard_City\": \"臺北市\",\"sendCard_District\": \"萬華區\",\"sendCard_Road\": \"忠孝西路２段\",\"sendCard_Lane\": \"18\",\"sendCard_Alley\": \"10\",\"sendCard_Number\": \"179\",\"sendCard_SubNumber\": \"19\",\"sendCard_Floor\": \"12\",\"sendCard_Other\": \"B2\",\"isDunyangBlackList\": null,\"nameCheckedReasonCodes\": null,\"mainIncomeAndFundCodes\": \"1,3\",\"mainIncomeAndFundOther\": null,\"amlJobLevelCode\": \"1\",\"isrcaForCurrentPEP\": \"N\",\"resignPEPKind\": 1,\"pepRange\": 1,\"isCurrentPositionRelatedPEPPosition\": \"N\"\r\n}";
        var request = JsonHelper.反序列化物件不分大小寫<UpdateSupplementaryInfoByApplyNoRequest>(jsonstring);

        return request;
    }
}

[ExampleAnnotation(Name = "[2000]修改申請書附卡人資料", ExampleType = ExampleType.Response)]
public class 修改申請書附卡人資料_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess("20250701990001", "20250701990001");
    }
}

[ExampleAnnotation(Name = "[4000]修改申請書附卡人資料-查無定義值", ExampleType = ExampleType.Request)]
public class 修改申請書附卡人資料查無定義值_4000_ReqEx : IExampleProvider<UpdateSupplementaryInfoByApplyNoRequest>
{
    public UpdateSupplementaryInfoByApplyNoRequest GetExample()
    {
        var jsonstring =
            "{\"applyNo\":\"20241009B1976\",\"chName\":\"姚聰健\",\"id\":\"R191498916\",\"citizenshipCode\":\"TW\",\"birthCitizenshipCode\":1,\"mobile\":\"0906147533\",\"sex\":1,\"birthDay\":\"0780930\",\"enName\":\"CrooksJean\",\"idCardRenewalLocationCode\":\"65000000\",\"idTakeStatus\":3,\"birthCitizenshipCodeOther\":null,\"marriageState\":3,\"education\":99,\"graduatedElementarySchool\":\"聯邦國小\",\"eMail\":\".98@gmail.com\",\"houseRegPhone\":\"02-89620479\",\"livePhone\":\"02-40791838\",\"liveOwner\":6,\"liveYear\":11,\"promotionUnit\":\"911T\",\"idIssueDate\":\"0960930\",\"promotionUser\":\"1001234\",\"aliNo\":\"100123456789\",\"residencePermitIssueDate\":null,\"passportNo\":null,\"passportDate\":null,\"expatValidityPeriod\":null,\"isFATCAIdentity\":\"N\",\"socialSecurityCode\":null,\"nameCheckedReasonCodes\":null,\"isrcaForCurrentPEP\":null,\"resignPEPKind\":null,\"pepRange\":null,\"isCurrentPositionRelatedPEPPosition\":null,\"billType\":1,\"acceptType\":null,\"isConvertCard\":\"N\",\"residencePermitBackendNum\":null,\"isForeverResidencePermit\":null,\"residencePermitDeadline\":null,\"oldCertificateVerified\":null,\"reg_ZipCode\":\"99065-3179\",\"reg_City\":\"彰化縣\",\"reg_District\":\"芬園鄉\",\"reg_Road\":\"嘉中街\",\"reg_Lane\":\"73\",\"reg_Alley\":\"41\",\"reg_Number\":\"547\",\"reg_SubNumber\":\"78\",\"reg_Floor\":\"24\",\"reg_FullAddr\":null,\"live_ZipCode\":\"86661-4157\",\"live_City\":\"桃園市\",\"live_District\":\"新屋區\",\"live_Road\":\"東福路\",\"live_Lane\":\"19\",\"live_Alley\":\"21\",\"live_Number\":\"870\",\"live_SubNumber\":\"98\",\"live_Floor\":\"57\",\"live_FullAddr\":null,\"liveAddressType\":null,\"sendCard_ZipCode\":\"86661-4157\",\"sendCard_City\":\"桃園市\",\"sendCard_District\":\"新屋區\",\"sendCard_Road\":\"東福路\",\"sendCard_Lane\":\"19\",\"sendCard_Alley\":\"21\",\"sendCard_Number\":\"870\",\"sendCard_SubNumber\":\"98\",\"sendCard_Floor\":\"57\",\"sendCard_FullAddr\":null,\"sendCardAddressType\":2,\"bill_ZipCode\":\"86661-4157\",\"bill_City\":\"桃園市\",\"bill_District\":\"新屋區\",\"bill_Road\":\"東福路\",\"bill_Lane\":\"199\",\"bill_Alley\":\"212\",\"bill_Number\":\"870\",\"bill_SubNumber\":\"98\",\"bill_Floor\":\"57\",\"bill_FullAddr\":null,\"billAddressType\":5,\"compName\":\"聯邦公司\",\"compID\":\"98520795\",\"compJobTitle\":\"行員\",\"compSeniority\":12,\"compPhone\":\"09461083537\",\"amlProfessionCode\":\"1\",\"amlProfessionOther\":null,\"amlJobLevelCode\":\"1\",\"compTrade\":3,\"compJobLevel\":8,\"currentMonthIncome\":109471,\"mainIncomeAndFundCodes\":\"1,3\",\"mainIncomeAndFundOther\":null,\"comp_ZipCode\":\"05438\",\"comp_City\":\"澎湖縣\",\"comp_District\":\"湖西鄉\",\"comp_Road\":\"中寮\",\"comp_Lane\":\"90\",\"comp_Alley\":\"99\",\"comp_Number\":\"620\",\"comp_SubNumber\":\"56\",\"comp_Floor\":\"49\",\"comp_FullAddr\":null,\"isStudent\":\"N\",\"parentName\":null,\"studSchool\":null,\"parentPhone\":null,\"studScheduledGraduationDate\":null,\"studentApplicantRelationship\":null,\"parentLive_ZipCode\":null,\"parentLive_City\":null,\"parentLive_District\":null,\"parentLive_Road\":null,\"parentLive_Lane\":null,\"parentLive_Alley\":null,\"parentLive_Number\":null,\"parentLive_SubNumber\":null,\"parentLive_Floor\":null,\"parentLive_FullAddr\":null,\"parentLiveAddressType\":null,\"firstBrushingGiftCode\":\"11\",\"projectCode\":\"ETU\",\"isAgreeDataOpen\":\"Y\",\"isAgreeMarketing\":\"Y\",\"isPayNoticeBind\":\"Y\",\"b68UnsecuredCredit\":198091}";
        var request = JsonHelper.反序列化物件不分大小寫<UpdateSupplementaryInfoByApplyNoRequest>(jsonstring);

        return request;
    }
}

[ExampleAnnotation(Name = "[4000]修改申請書附卡人資料-查無定義值", ExampleType = ExampleType.Response)]
public class 修改申請書附卡人資料查無定義值_4000_ResEx : IExampleProvider<ResultResponse<Dictionary<string, IEnumerable<string>>>>
{
    public ResultResponse<Dictionary<string, IEnumerable<string>>> GetExample()
    {
        var validationResults = new List<ValidationResult> { new ValidationResult("查無國籍資料，請檢查", new[] { "國籍" }) };

        var errors = validationResults.ToDictionary(
            result => result.MemberNames.FirstOrDefault() ?? string.Empty,
            result => (IEnumerable<string>)new List<string> { result.ErrorMessage }
        );

        return ApiResponseHelper.BadRequest(errors, "");
    }
}

[ExampleAnnotation(Name = "[4003]修改申請書附卡人資料-查無郵遞區號", ExampleType = ExampleType.Response)]
public class 修改申請書附卡人資料查無郵遞區號_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>(null, "郵遞區號查詢錯誤，請自行填寫附卡人寄卡地址郵遞區號\r\n郵遞區號查詢錯誤");
    }
}

[ExampleAnnotation(Name = "[4003]修改申請書附卡人資料-國籍與FATCA身分不符", ExampleType = ExampleType.Response)]
public class 修改申請書附卡人資料國籍與FATCA身分不符_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>(
            null,
            "當國籍 = 美國時，是否為FATCA身份需為Y，請檢查 / 當國籍不為美國時，是否為FATCA身份需為N，請檢查 / 因屬於 FATCA身份 社會安全號碼不為空值，請檢查"
        );
    }
}

[ExampleAnnotation(Name = "[4003]修改申請書附卡人資料-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改申請書附卡人資料路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}

[ExampleAnnotation(Name = "[4001]修改申請書附卡人資料-查無此資料", ExampleType = ExampleType.Response)]
public class 修改申請書附卡人資料查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound("20240906B4222", "20240906B4222");
    }
}
