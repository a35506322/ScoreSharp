namespace ScoreSharp.API.Modules.Manage.UnassignedCasesList.GetUnassignedCasesByQueryString;

[ExampleAnnotation(Name = "[2000]取得待派案案件清單", ExampleType = ExampleType.Request)]
public class 取得待派案案件清單_2000_ReqEx : IExampleProvider<GetUnassignedCasesByQueryStringRequest>
{
    public GetUnassignedCasesByQueryStringRequest GetExample()
    {
        return new GetUnassignedCasesByQueryStringRequest()
        {
            CaseAssignmentType = CaseAssignmentType.網路件月收入預審_姓名檢核Y清單,
            AssignedUserId = "arthurlin",
        };
    }
}

[ExampleAnnotation(Name = "[2000]取得待派案案件清單", ExampleType = ExampleType.Response)]
public class 取得待派案案件清單_2000_ResEx : IExampleProvider<ResultResponse<List<GetUnassignedCasesByQueryStringResponse>>>
{
    public ResultResponse<List<GetUnassignedCasesByQueryStringResponse>> GetExample()
    {
        string jsonString =
            "{\"returnCodeStatus\": 2000,\"returnMessage\": \"\",\"returnData\": [ {  \"applyNo\": \"20250711B5983\",  \"chName\": \"鄒健柏\",  \"nameCheckResultList\": [{ \"userType\": 1, \"nameCheckResult\": \"Y\"}  ],  \"nameCheckResultName\": \"Y\",  \"id\": \"I249491113\",  \"applyCardTypeList\": [{ \"userType\": 1, \"applyCardType\": \"VI00\", \"applyCardTypeName\": \"BANK CARD無限卡\"}  ],  \"applyCardTypeName\": \"(VI00)BANK CARD無限卡\",  \"cardStatusList\": [{ \"userType\": 1, \"cardStatus\": 30010, \"cardStatusName\": \"網路件_待月收入預審\"}  ],  \"cardStatusName\": \"網路件_待月收入預審\",  \"isDuplicateSubmissionList\": [{ \"isDuplicateSubmission\": \"N\", \"userType\": 1}  ],  \"isDuplicateSubmissionName\": \"否\",  \"isNewAccountList\": [{ \"userType\": 1, \"isNewAccount\": \"N\"}  ],  \"isNewAccountName\": \"否\",  \"isOriginalCardholderList\": [{ \"userType\": 1, \"isOriginalCardholder\": \"N\"}  ],  \"isOriginalCardholderName\": \"否\",  \"applyDate\": \"2025-07-11T10:28:09.757\" }, {  \"applyNo\": \"20250711B7982\",  \"chName\": \"蔣孝嚴\",  \"nameCheckResultList\": [{ \"userType\": 1, \"nameCheckResult\": \"Y\"}  ],  \"nameCheckResultName\": \"Y\",  \"id\": \"C231138406\",  \"applyCardTypeList\": [{ \"userType\": 1, \"applyCardType\": \"JC03\", \"applyCardTypeName\": \"大立JCB晶緻卡C\"}  ],  \"applyCardTypeName\": \"(JC03)大立JCB晶緻卡C\",  \"cardStatusList\": [{ \"userType\": 1, \"cardStatus\": 30010, \"cardStatusName\": \"網路件_待月收入預審\"}  ],  \"cardStatusName\": \"網路件_待月收入預審\",  \"isDuplicateSubmissionList\": [{ \"isDuplicateSubmission\": \"N\", \"userType\": 1}  ],  \"isDuplicateSubmissionName\": \"否\",  \"isNewAccountList\": [{ \"userType\": 1, \"isNewAccount\": \"N\"}  ],  \"isNewAccountName\": \"否\",  \"isOriginalCardholderList\": [{ \"userType\": 1, \"isOriginalCardholder\": \"N\"}  ],  \"isOriginalCardholderName\": \"否\",  \"applyDate\": \"2025-07-11T10:29:11.12\" }]\r\n}";
        var result = JsonHelper.反序列化物件不分大小寫<ResultResponse<List<GetUnassignedCasesByQueryStringResponse>>>(jsonString);

        return result;
    }
}
