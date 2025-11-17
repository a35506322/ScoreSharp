namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateApplicationInfoById;

[ExampleAnnotation(Name = "[2000]修改申請書資料", ExampleType = ExampleType.Request)]
public class 修改申請書資料_2000_ReqEx : IExampleProvider<UpdateApplicationInfoByIdRequest>
{
    public UpdateApplicationInfoByIdRequest GetExample()
    {
        var jsonstring = """
                        {
              "applyNo": "20241009B1976",
              "chName": "姚聰健",
              "id": "R191498916",
              "citizenshipCode": "TW",
              "birthCitizenshipCode": 1,
              "mobile": "0906147533",
              "sex": 1,
              "birthDay": "0780930",
              "enName": "CrooksJean",
              "idCardRenewalLocationCode": "65000000",
              "idTakeStatus": 3,
              "birthCitizenshipCodeOther": null,
              "marriageState": 3,
              "education": 3,
              "graduatedElementarySchool": "聯邦國小",
              "eMail": ".98@gmail.com",
              "houseRegPhone": "02-89620479",
              "livePhone": "02-40791838",
              "liveOwner": 6,
              "liveYear": 11,
              "promotionUnit": "911T",
              "idIssueDate": "0960930",
              "promotionUser": "1001234",
              "anliNo": null,
              "residencePermitIssueDate": null,
              "passportNo": null,
              "passportDate": null,
              "expatValidityPeriod": null,
              "isApplyDigtalCard": null,
              "isFATCAIdentity": "N",
              "socialSecurityCode": null,
              "isDunyangBlackList": null,
              "nameCheckedReasonCodes": null,
              "isrcaForCurrentPEP": null,
              "resignPEPKind": null,
              "pepRange": null,
              "isCurrentPositionRelatedPEPPosition": null,
              "billType": 1,
              "acceptType": null,
              "isConvertCard": "N",
              "residencePermitBackendNum": null,
              "isForeverResidencePermit": null,
              "residencePermitDeadline": null,
              "oldCertificateVerified": null,
              "childrenCount": null,
              "reg_ZipCode": "99065-3179",
              "reg_City": "彰化縣",
              "reg_District": "芬園鄉",
              "reg_Road": "嘉中街",
              "reg_Lane": "73",
              "reg_Alley": "41",
              "reg_Number": "547",
              "reg_SubNumber": "78",
              "reg_Floor": "24",
              "reg_FullAddr": null,
              "reg_Other": null,
              "live_ZipCode": "86661-4157",
              "live_City": "桃園市",
              "live_District": "新屋區",
              "live_Road": "東福路",
              "live_Lane": "19",
              "live_Alley": "21",
              "live_Number": "870",
              "live_SubNumber": "98",
              "live_Floor": "57",
              "live_FullAddr": null,
              "live_Other": null,
              "liveAddressType": null,
              "sendCard_ZipCode": "86661-4157",
              "sendCard_City": "桃園市",
              "sendCard_District": "新屋區",
              "sendCard_Road": "東福路",
              "sendCard_Lane": "19",
              "sendCard_Alley": "21",
              "sendCard_Number": "870",
              "sendCard_SubNumber": "98",
              "sendCard_Floor": "57",
              "sendCard_FullAddr": null,
              "sendCard_Other": null,
              "sendCardAddressType": 2,
              "bill_ZipCode": "86661-4157",
              "bill_City": "桃園市",
              "bill_District": "新屋區",
              "bill_Road": "東福路",
              "bill_Lane": "19",
              "bill_Alley": "21",
              "bill_Number": "870",
              "bill_SubNumber": "98",
              "bill_Floor": "57",
              "bill_FullAddr": null,
              "bill_Other": null,
              "billAddressType": 2,
              "compName": "聯邦公司",
              "compID": "98520795",
              "compJobTitle": "行員",
              "compSeniority": 12,
              "compPhone": "09461083537",
              "amlProfessionCode": "1",
              "amlProfessionOther": null,
              "amlJobLevelCode": "1",
              "compTrade": 3,
              "compJobLevel": 8,
              "currentMonthIncome": 109471,
              "mainIncomeAndFundCodes": "1,3",
              "mainIncomeAndFundOther": null,
              "departmentName": null,
              "employmentDate": null,
              "comp_ZipCode": "05438",
              "comp_City": "澎湖縣",
              "comp_District": "湖西鄉",
              "comp_Road": "中寮",
              "comp_Lane": "90",
              "comp_Alley": "99",
              "comp_Number": "620",
              "comp_SubNumber": "56",
              "comp_Floor": "49",
              "comp_FullAddr": null,
              "comp_Other": null,
              "isStudent": "N",
              "parentName": null,
              "studSchool": null,
              "parentPhone": null,
              "studScheduledGraduationDate": null,
              "studentApplicantRelationship": null,
              "parentLive_ZipCode": null,
              "parentLive_City": null,
              "parentLive_District": null,
              "parentLive_Road": null,
              "parentLive_Lane": null,
              "parentLive_Alley": null,
              "parentLive_Number": null,
              "parentLive_SubNumber": null,
              "parentLive_Floor": null,
              "parentLive_FullAddr": null,
              "parentLive_Other": null,
              "parentLiveAddressType": null,
              "firstBrushingGiftCode": "11",
              "projectCode": "ETU",
              "isAgreeDataOpen": "Y",
              "isAgreeMarketing": "Y",
              "isPayNoticeBind": "Y",
              "isAcceptEasyCardDefaultBonus": null,
              "elecCodeId": null,
              "annualFeePaymentType": null,
              "creditCheckCode": null
            }
            """;
        var request = JsonHelper.反序列化物件不分大小寫<UpdateApplicationInfoByIdRequest>(jsonstring);

        return request;
    }
}

[ExampleAnnotation(Name = "[2000]修改申請書資料", ExampleType = ExampleType.Response)]
public class 修改申請書資料_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess("20241008B8045", "20241008B8045");
    }
}

[ExampleAnnotation(Name = "[4001]修改申請書資料-查無此資料", ExampleType = ExampleType.Response)]
public class 修改申請書資料查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound("20240906B4222", "20240906B4222");
    }
}

[ExampleAnnotation(Name = "[4003]修改申請書資料-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改申請書資料路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}

[ExampleAnnotation(Name = "[4003]修改申請書資料-查無郵遞區號", ExampleType = ExampleType.Response)]
public class 修改申請書資料查無郵遞區號_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>(
            null,
            "郵遞區號查詢錯誤，請自行填寫正卡人公司地址郵遞區號\r\n郵遞區號查詢錯誤，請自行填寫正卡人寄卡地址郵遞區號\r\n郵遞區號查詢錯誤，請自行填寫正卡人帳單地址郵遞區號"
        );
    }
}
