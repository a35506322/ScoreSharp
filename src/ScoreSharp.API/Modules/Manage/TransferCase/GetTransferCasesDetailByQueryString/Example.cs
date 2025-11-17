namespace ScoreSharp.API.Modules.Manage.TransferCase.GetTransferCasesDetailByQueryString;

[ExampleAnnotation(Name = "[2000]取得調撥案件詳細資料", ExampleType = ExampleType.Request)]
public class 取得調撥案件詳細資料_2000_ReqEx : IExampleProvider<GetTransferCasesDetailByQueryStringRequest>
{
    public GetTransferCasesDetailByQueryStringRequest GetExample()
    {
        return new GetTransferCasesDetailByQueryStringRequest
        {
            TransferredUserId = "arthurlin",
            TransferCaseType = TransferCaseType.網路件月收入預審,
        };
    }
}

[ExampleAnnotation(Name = "[2000]取得待派案案件清單", ExampleType = ExampleType.Response)]
public class 取得待派案案件清單_2000_ReqEx : IExampleProvider<ResultResponse<List<GetTransferCasesDetailByQueryStringResponse>>>
{
    public ResultResponse<List<GetTransferCasesDetailByQueryStringResponse>> GetExample()
    {
        string jsonString =
            "{\"returnCodeStatus\":2000,\"returnMessage\":\"\",\"returnData\":[{\"applyNo\":\"2025100208886\",\"chName\":\"周杰倫\",\"id\":\"C262089173\",\"applyCardTypeList\":[{\"cardStatus\":3,\"cardStatusName\":\"紙本件_待月收入預審\",\"applyCardType\":\"JA00\",\"applyCardTypeName\":\"(JA00)JCB 商務晶緻卡\"},{\"cardStatus\":3,\"cardStatusName\":\"紙本件_待月收入預審\",\"applyCardType\":\"JC02\",\"applyCardTypeName\":\"(JC02)大統JCB晶緻卡C\"},{\"cardStatus\":3,\"cardStatusName\":\"紙本件_待月收入預審\",\"applyCardType\":\"JA00\",\"applyCardTypeName\":\"(JA00)JCB 商務晶緻卡\"},{\"cardStatus\":3,\"cardStatusName\":\"紙本件_待月收入預審\",\"applyCardType\":\"JC02\",\"applyCardTypeName\":\"(JC02)大統JCB晶緻卡C\"}],\"caseType\":1,\"caseTypeName\":\"一般件\",\"isUrgent\":\"N\",\"applyDate\":\"2025-10-02T17:56:23.15\",\"lastUpdateTime\":\"2025-10-17T16:51:46.767\",\"notes\":\"\"}]}";

        var result = JsonHelper.反序列化物件不分大小寫<ResultResponse<List<GetTransferCasesDetailByQueryStringResponse>>>(jsonString);

        return result;
    }
}
