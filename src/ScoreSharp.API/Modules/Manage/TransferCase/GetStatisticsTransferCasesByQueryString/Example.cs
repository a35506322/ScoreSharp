namespace ScoreSharp.API.Modules.Manage.TransferCase.GetStatisticsTransferCasesByQueryString;

[ExampleAnnotation(Name = "[2000]取得統計調撥案件", ExampleType = ExampleType.Response)]
public class 取得統計調撥案件_2000_ResEx : IExampleProvider<ResultResponse<List<GetStatisticsTransferCasesByQueryStringResponse>>>
{
    public ResultResponse<List<GetStatisticsTransferCasesByQueryStringResponse>> GetExample()
    {
        string jsonString =
            "{\"returnCodeStatus\":2000,\"returnMessage\":\"\",\"returnData\":[{\"userId\":\"arthurlin\",\"userName\":\"林芃均\",\"transferCases\":[{\"transferCaseType\":1,\"transferCaseTypeName\":\"網路件月收入預審\",\"caseCount\":12},{\"transferCaseType\":2,\"transferCaseTypeName\":\"網路件人工徵信中\",\"caseCount\":0},{\"transferCaseType\":3,\"transferCaseTypeName\":\"紙本件月收入預審\",\"caseCount\":2},{\"transferCaseType\":4,\"transferCaseTypeName\":\"紙本件人工徵信中\",\"caseCount\":0}],\"transferCasesTotalCount\":14},{\"userId\":\"janehuang\",\"userName\":\"黃亭蓁\",\"transferCases\":[{\"transferCaseType\":1,\"transferCaseTypeName\":\"網路件月收入預審\",\"caseCount\":4},{\"transferCaseType\":2,\"transferCaseTypeName\":\"網路件人工徵信中\",\"caseCount\":0},{\"transferCaseType\":3,\"transferCaseTypeName\":\"紙本件月收入預審\",\"caseCount\":0},{\"transferCaseType\":4,\"transferCaseTypeName\":\"紙本件人工徵信中\",\"caseCount\":0}],\"transferCasesTotalCount\":4}]}";
        var result = JsonHelper.反序列化物件不分大小寫<ResultResponse<List<GetStatisticsTransferCasesByQueryStringResponse>>>(jsonString);

        return result;
    }
}
