namespace ScoreSharp.API.Modules.Reviewer.Common.GetReviewerCaseStatusCount;

[ExampleAnnotation(Name = "[2000]取得徵審案件狀態數量", ExampleType = ExampleType.Response)]
public class 取得徵審案件狀態彙總_2000_ResEx : IExampleProvider<ResultResponse<List<GetReviewerCaseStatusCountResponse>>>
{
    public ResultResponse<List<GetReviewerCaseStatusCountResponse>> GetExample()
    {
        string jsonstring =
            "{\"returnCodeStatus\":2000,\"returnMessage\":\"\",\"returnData\":[{\"statusName\":\"網路件月收入確認\",\"statusId\":\"1\",\"count\":0},{\"statusName\":\"網路件人工審查\",\"statusId\":\"2\",\"count\":0},{\"statusName\":\"緊急製卡\",\"statusId\":\"3\",\"count\":0},{\"statusName\":\"補回件\",\"statusId\":\"4\",\"count\":0},{\"statusName\":\"拒件/撤件重審\",\"statusId\":\"5\",\"count\":0},{\"statusName\":\"網路件製卡失敗\",\"statusId\":\"6\",\"count\":0},{\"statusName\":\"紙本件月收入確認\",\"statusId\":\"7\",\"count\":0},{\"statusName\":\"紙本件人工審查\",\"statusId\":\"8\",\"count\":0},{\"statusName\":\"急件\",\"statusId\":\"9\",\"count\":0},{\"statusName\":\"退回重審\",\"statusId\":\"10\",\"count\":0},{\"statusName\":\"未補回\",\"statusId\":\"11\",\"count\":0},{\"statusName\":\"紙本件製卡失敗\",\"statusId\":\"12\",\"count\":0}]}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<List<GetReviewerCaseStatusCountResponse>>>(jsonstring);

        return data;
    }
}
