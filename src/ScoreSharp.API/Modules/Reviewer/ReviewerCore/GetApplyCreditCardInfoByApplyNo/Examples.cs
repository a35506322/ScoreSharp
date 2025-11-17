namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyCreditCardInfoByApplyNo;

[ExampleAnnotation(Name = "[2000]取得單筆申請案件", ExampleType = ExampleType.Response)]
public class 取得單筆申請案件_2000_ResEx : IExampleProvider<ResultResponse<GetApplyCreditCardInfoByApplyNoResponse>>
{
    public ResultResponse<GetApplyCreditCardInfoByApplyNoResponse> GetExample()
    {
        string jsonString = """
                        {
                "returnCodeStatus": 2000,
                "returnMessage": "",
                "returnData": {
                    "mainInfo": {
                        "chName": "馬紹輝",
                        "id": "A189245019",
                        "applyDate": "2025-08-25T16:54:01.05",
                        "applyNo": "20250825B8321",
                        "cardStatusList": [
                            {
                                "cardStatus": 10201,
                                "cardStatusName": "人工徵信中",
                                "id": "A189245019",
                                "userType": 1
                            }
                        ],
                        "familyMessageCheckedList": [
                            {
                                "id": "A189245019",
                                "userType": 1,
                                "familyMessageChecked": ""
                            }
                        ],
                        "isRepeatApply": "N",
                        "isOriginalCardholderList": [
                            {
                                "id": "A189245019",
                                "userType": 1,
                                "isOriginalCardholder": "N"
                            }
                        ],
                        "applyCardTypeList": [
                            {
                                "applyCardType": "VI91",
                                "applyCardTypeName": "微風黑鑽無限卡",
                                "id": "A189245019",
                                "userType": 1
                            }
                        ],
                        "checked929List": [
                            {
                                "id": "A189245019",
                                "userType": 1,
                                "checked929": "N"
                            }
                        ],
                        "customerSpecialNotes": null,
                        "idCheckResultCheckedList": [
                            {
                                "id": "A189245019",
                                "userType": 1,
                                "idCheckResultChecked": ""
                            }
                        ],
                        "focus1CheckedList": [
                            {
                                "id": "A189245019",
                                "userType": 1,
                                "focus1Hit": "",
                                "focus1Checked": "N"
                            }
                        ],
                        "blackListNote": null,
                        "caseType": 1,
                        "caseTypeName": "一般件",
                        "originCardholderJCICNotesList": [
                            {
                                "id": "A189245019",
                                "userType": 1,
                                "originCardholderJCICNotes": ""
                            }
                        ],
                        "focus2CheckedList": [
                            {
                                "id": "A189245019",
                                "userType": 1,
                                "focus2Hit": "",
                                "focus2Checked": "N"
                            }
                        ],
                        "monthlyIncomeCheckUserId": "arthurlin",
                        "monthlyIncomeCheckUserName": "林芃均",
                        "cardOwner": 1,
                        "cardOwnerName": "正卡",
                        "longTerm": null,
                        "nameCheckedList": [
                            {
                                "id": "A189245019",
                                "userType": 1,
                                "nameChecked": "N"
                            }
                        ],
                        "reviewerUserList": [
                            {
                                "reviewerUserId": "janehuang",
                                "reviewerUserName": "黃亭蓁",
                                "id": "A189245019",
                                "userType": 1,
                                "userTypeName": "正卡人",
                                "applyCardType": "VI91"
                            }
                        ],
                        "creditLimit_RatingAdviceList": [
                            {
                                "id": "A189245019",
                                "userType": 1,
                                "creditLimit_RatingAdvice": null
                            }
                        ],
                        "isBranchCustomer": "N",
                        "amlRiskLevel": "M",
                        "approveUserList": [
                            {
                                "approveUserId": null,
                                "approveUserName": "",
                                "id": "A189245019",
                                "userType": 1,
                                "userTypeName": "正卡人",
                                "applyCardType": "VI91"
                            }
                        ],
                        "customerServiceNotes": null,
                        "creditCheckCodeList": [
                            {
                                "seqNo": "01K3G6YD2EZ55T408HGVYHY0B6",
                                "creditCheckCode": "A24",
                                "creditCheckName": "收入證明申請"
                            }
                        ],
                        "currentHandleUserId": "janehuang",
                        "currentHandleUserName": "黃亭蓁",
                        "isSelf": "N",
                        "previousHandleUserId": "janehuang",
                        "previousHandleUserName": "黃亭蓁",
                        "source": 1,
                        "sourceName": "ECARD",
                        "cardStep": 2,
                        "cardStepName": "人工徵審"
                    },
                    "primary_BasicInfo": {
                        "chName": "馬紹輝",
                        "id": "A189245019",
                        "sex": 2,
                        "sexName": "女",
                        "birthDay": "0580904",
                        "isStudent": "N",
                        "enName": "MA SHAO HUI",
                        "idIssueDate": "0760904",
                        "idCardRenewalLocationCode": "01235456",
                        "idCardRenewalLocationName": "嘉市",
                        "idTakeStatus": 2,
                        "idTakeStatusName": "補發",
                        "citizenshipCode": "TW",
                        "citizenshipName": "台灣",
                        "birthCitizenshipCode": 1,
                        "birthCitizenshipName": "中華民國",
                        "birthCitizenshipCodeOther": null,
                        "marriageState": null,
                        "marriageStateName": "",
                        "education": 3,
                        "educationName": "大學",
                        "graduatedElementarySchool": "聯邦國小",
                        "isApplyDigtalCard": "N",
                        "isConvertCard": null,
                        "mobile": "0961574677",
                        "eMail": "18@yahoo.com",
                        "billType": 3,
                        "billTypeName": "紙本帳單",
                        "reg_ZipCode": "96541",
                        "reg_City": "臺東縣",
                        "reg_District": "大武鄉",
                        "reg_Road": "民族街",
                        "reg_Lane": "261",
                        "reg_Alley": "63",
                        "reg_Number": "57",
                        "reg_SubNumber": "74",
                        "reg_Floor": "48",
                        "reg_FullAddr": null,
                        "reg_Other": "",
                        "live_ZipCode": "31345",
                        "live_City": "新竹縣",
                        "live_District": "尖石鄉",
                        "live_Road": "新光",
                        "live_Lane": "18",
                        "live_Alley": "86",
                        "live_Number": "31",
                        "live_SubNumber": "95",
                        "live_Floor": "82",
                        "live_FullAddr": null,
                        "live_Other": "",
                        "liveAddressType": null,
                        "liveAddressTypeName": "",
                        "bill_ZipCode": "71146",
                        "bill_City": "臺南市",
                        "bill_District": "歸仁區",
                        "bill_Road": "大成路",
                        "bill_Lane": "3",
                        "bill_Alley": "43",
                        "bill_Number": "38",
                        "bill_SubNumber": "83",
                        "bill_Floor": "38",
                        "bill_FullAddr": null,
                        "bill_Other": "",
                        "billAddressType": 4,
                        "billAddressTypeName": "同公司地址",
                        "sendCard_ZipCode": "71146",
                        "sendCard_City": "臺南市",
                        "sendCard_District": "歸仁區",
                        "sendCard_Road": "大成路",
                        "sendCard_Lane": "3",
                        "sendCard_Alley": "43",
                        "sendCard_Number": "38",
                        "sendCard_SubNumber": "83",
                        "sendCard_Floor": "38",
                        "sendCard_FullAddr": null,
                        "sendCard_Other": "",
                        "sendCardAddressType": 4,
                        "sendCardAddressTypeName": "同公司地址",
                        "houseRegPhone": "08-93661542",
                        "livePhone": "06-67486131",
                        "liveOwner": 5,
                        "liveOwnerName": "宿舍",
                        "liveYear": null,
                        "promotionUnit": "100",
                        "promotionUser": "1001234",
                        "acceptType": null,
                        "acceptTypeName": "",
                        "anliNo": "",
                        "residencePermitIssueDate": null,
                        "passportNo": null,
                        "passportDate": null,
                        "expatValidityPeriod": null,
                        "isDunyangBlackList": "N",
                        "isFATCAIdentity": "N",
                        "socialSecurityCode": null,
                        "nameCheckedReasonCodeList": [],
                        "isrcaForCurrentPEP": null,
                        "resignPEPKind": null,
                        "resignPEPKindName": "",
                        "pepRange": null,
                        "pepRangeName": "",
                        "isCurrentPositionRelatedPEPPosition": null,
                        "residencePermitBackendNum": null,
                        "isForeverResidencePermit": null,
                        "residencePermitDeadline": null,
                        "oldCertificateVerified": null,
                        "childrenCount": 1
                    },
                    "primary_JobInfo": {
                        "compName": "聯邦測試公司",
                        "compID": "72022433",
                        "compJobTitle": "Global Quality Speci",
                        "compSeniority": 34,
                        "comp_ZipCode": "71146",
                        "comp_City": "臺南市",
                        "comp_District": "歸仁區",
                        "comp_Road": "大成路",
                        "comp_Lane": "3",
                        "comp_Alley": "43",
                        "comp_Number": "38",
                        "comp_SubNumber": "83",
                        "comp_Floor": "38",
                        "comp_FullAddr": null,
                        "comp_Other": "",
                        "compPhone": "00-01958113#4622",
                        "amlProfessionCode_Version": "20250121",
                        "amlProfessionCode": "13",
                        "amlProfessionName": "政府官員/民意代表",
                        "amlProfessionOther": "",
                        "amlJobLevelCode": "3",
                        "amlJobLevelName": "總經理或相當職位",
                        "compTrade": null,
                        "compTradeName": "",
                        "compJobLevel": null,
                        "compJobLevelName": "",
                        "mainIncomeAndFundCodes": "2,7",
                        "mainIncomeAndFundNames": "薪資所得,專案執業收入",
                        "mainIncomeAndFundOther": "",
                        "currentMonthIncome": 100000,
                        "creditCheckCode": "A24",
                        "creditCheckName": "收入證明申請",
                        "departmentName": null,
                        "employmentDate": null
                    },
                    "primary_StudentInfo": {
                        "isStudent": "N",
                        "studSchool": null,
                        "studScheduledGraduationDate": null,
                        "parentName": "",
                        "parentPhone": "",
                        "studentApplicantRelationship": null,
                        "studentApplicantRelationshipName": "",
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
                        "parentLiveAddressTypeName": ""
                    },
                    "primary_WebCardInfo": {
                        "userSourceIP": "220.150.207.192",
                        "otpTime": "2025-08-25T16:51:45",
                        "otpMobile": "0978507057",
                        "sameIPChecked": "N",
                        "sameWebCaseMobileChecked": "N",
                        "sameWebCaseEmailChecked": "N",
                        "isEqualInternalIP": "N"
                    },
                    "primary_ActivityInfo": {
                        "firstBrushingGiftCode": "1",
                        "isAgreeDataOpen": "Y",
                        "isPayNoticeBind": "Y",
                        "projectCode": "ETU",
                        "isAgreeMarketing": "Y",
                        "isAcceptEasyCardDefaultBonus": "Y",
                        "elecCodeId": null,
                        "annualFeePaymentType":"01",
                        "annualFeePaymentTypeName":"一次繳清"
                    },
                    "primary_BankTraceInfo": {
                        "shortTimeIDChecked": "N",
                        "internalEmailSame_Flag": "N",
                        "internalMobileSame_Flag": "N"
                    },
                    "supplementary": null,
                    "kycInfo": {
                        "amlRiskLevel": "M",
                        "kyC_RtnCode": "K0000",
                        "kyC_QueryTime": "2025-08-25T17:00:40.873",
                        "kyC_Message": "",
                        "kyC_StrongReVersion": "20210501",
                        "kyC_StrongReStatus": 3,
                        "kyC_Handler": "janehuang",
                        "kyC_Handler_SignTime": "2025-08-26T10:42:45.873",
                        "kyC_Reviewer": "arthurlin",
                        "kyC_Reviewer_SignTime": "2025-08-26T10:48:09.37",
                        "kyC_KYCManager": "arthurlin",
                        "kyC_KYCManager_SignTime": "2025-08-26T10:48:09.37",
                        "kyC_StrongReDetailJson": "{\"基本資料\":{\"所建立業務關係\":{\"台幣存款\":false,\"保管箱\":false,\"外匯存款\":false,\"企業金融\":false,\"消費金融\":false,\"車輛貸款\":false,\"理財貸款\":false,\"票債券\":false,\"財富管理\":false,\"信託\":false,\"保險\":false,\"證券\":false,\"期貨輔助\":false,\"衍生性商品業務\":false,\"外匯保證金\":false,\"數位存款\":false,\"信用卡業務\":false},\"ID\":\"A189245019\",\"姓名\":\"馬紹輝\",\"客戶風險等級\":{\"高風險(H)\":false,\"中風險(M)\":true}},\"壹、確認客戶身分加強審查事項(中、高風險客戶填寫)\":{\"A 客戶曾使用之姓名或別名\":{\"是否選擇\":false,\"客戶舊名稱\":\"\",\"參考號碼\":\"\"},\"B 電話照會\":{\"是否選擇\":false,\"連絡電話\":\"\",\"驗證結果\":{\"已照會\":false,\"聯絡未果\":false}},\"C 郵件驗證\":{\"是否選擇\":false,\"郵件寄送地址\":\"\",\"驗證結果\":{\"已回函\":false,\"未回函\":false}},\"D 任職機構驗證\":{\"是否選擇\":false,\"驗證結果\":{\"是\":false,\"否\":false}},\"E 實地訪查\":{\"是否選擇\":false,\"實地訪查地址\":\"\",\"實地訪查日期\":\"\",\"驗證結果\":{\"與客戶填寫資料一致\":false,\"與客戶填寫資料不一致\":false,\"與客戶填寫資料不一致原因\":\"\",\"與客戶填寫資料不一致是否合理\":{\"是\":false,\"否\":false}}},\"F 聯徵資料\":{\"是否選擇\":true,\"驗證結果\":{\"否\":true,\"是\":false,\"聯徵資料遭通報列為警示帳戶原因\":\"\",\"聯徵資料遭通報列為警示帳戶是否合理\":{\"是\":false,\"否\":false}}}},\"貳、確認客戶身分加強審查事項(高風險客戶填寫及資料驗證)\":{\"一、了解客戶財富及資金來源\":{\"所得來源\":{\"就業所得\":null,\"理財投資-投資\":{\"是否選擇\":false,\"選項\":[],\"其他\":\"\"},\"租金收入-出租\":{\"是否選擇\":false,\"選項\":[],\"其他\":\"\"},\"專案執業收入\":{\"是否選擇\":false,\"選項\":[],\"其他\":\"\"},\"繼承/贈與\":{\"是否選擇\":false,\"選項\":[],\"其他\":\"\"},\"退休金\":{\"是否選擇\":false,\"選項\":[]},\"閒置資金\":{\"是否選擇\":false,\"選項\":[],\"其他\":\"\"},\"其他\":{\"是否選擇\":false,\"說明\":\"\"}},\"文件驗證\":{\"來源\":{\"客戶提供\":false,\"本行由內/外部機構\":false},\"選項\":{\"各類所得扣繳稅額報繳證明\":false,\"薪資單\":false,\"年報或財報\":false,\"合約\":false,\"發票\":false,\"收據\":false,\"存摺\":false,\"任職證明\":false,\"交易證明文件\":false,\"不動產權狀\":false,\"名片\":false,\"保單\":false,\"取得寄送客戶所提供地址之水/電/電話費等資料\":false,\"勞保局投保明細\":false,\"員工證\":false,\"聘書\":false,\"其他可證明財富及資金來源之文件\":false},\"其他可證明財富及資金來源之文件原因\":\"\"},\"非文件驗證\":{\"選項\":{\"客戶面對面訪談\":false,\"辦理電話訪查\":false,\"辦理實地訪查\":false},\"連絡電話訪查地址內容\":\"\"},\"判斷財富及資金來源是否合理\":{\"選項\":{\"是\":false,\"否\":false},\"否_原因\":\"\"}},\"二、客戶預期帳戶使用狀況及資金流向\":{\"1. 預期每月入帳金額與次數(買入)\":{\"金額選項\":{\"10萬元(含)以下\":false,\"10萬元至50萬元(含)\":false,\"50萬元至200萬元(含)\":false,\"200萬元以上\":false},\"次數選項\":{\"10次以下\":false,\"11次至50次\":false,\"51次至100次\":false,\"101次以上\":false},\"依其性質無法提供如信用卡證券票債券保管箱保險財管及授信業務等業務\":false},\"2. 預期每月出帳金額與次數(賣出)\":{\"金額選項\":{\"10萬元(含)以下\":false,\"10萬元至50萬元(含)\":false,\"50萬元至200萬元(含)\":false,\"200萬元以上\":false},\"次數選項\":{\"10次以下\":false,\"11次至50次\":false,\"51次至100次\":false,\"101次以上\":false},\"依其性質無法提供如信用卡證券票債券保管箱保險財管及授信業務等業務\":false},\"3. 預期資金流向\":{\"選項\":{\"境內\":false,\"境外\":false,\"依其性質無法提供如信用卡證券票債券保管箱保險及財管業務等業務\":false},\"境外國家\":\"\",\"是否為高風險地域國家\":{\"是\":false,\"否\":false}}}},\"參、簽核\":{\"審查意見\":{\"建議核准\":true,\"建議婉拒\":false},\"備註\":\"\",\"經辦簽核\":\"\",\"日期\":\"\",\"覆核\":\"\",\"防制洗錢及打擊資恐督導主管簽核\":\"\",\"主管簽核\":\"\"}}",
                        "kyC_Suggestion": "Y",
                        "isShowKYCStrongReview": "Y"
                    }
                }
            }
            """;
        var result = JsonHelper.反序列化物件不分大小寫<ResultResponse<GetApplyCreditCardInfoByApplyNoResponse>>(jsonString);
        result.ReturnData.MainInfo.PreviousHandleUserId = "arthurlin";
        result.ReturnData.MainInfo.PreviousHandleUserName = "林芃均";
        return result;
    }
}
