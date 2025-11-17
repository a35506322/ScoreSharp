namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyCreditCardInfoQueryLogByApplyNo;

[ExampleAnnotation(Name = "[2000]取得申請信用卡資料查詢紀錄", ExampleType = ExampleType.Response)]
public class 取得申請信用卡資料查詢紀錄_2000_ResEx : IExampleProvider<ResultResponse<List<GetApplyCreditCardInfoQueryLogByApplyNoResponse>>>
{
    public ResultResponse<List<GetApplyCreditCardInfoQueryLogByApplyNoResponse>> GetExample()
    {
        var response = new List<GetApplyCreditCardInfoQueryLogByApplyNoResponse>
        {
            new GetApplyCreditCardInfoQueryLogByApplyNoResponse
            {
                ApplyNo = "20241014B0701",
                UserId = "user123",
                QueryTime = DateTime.Parse("2024-10-14T10:30:00"),
                UserName = "張三",
            },
            new GetApplyCreditCardInfoQueryLogByApplyNoResponse
            {
                ApplyNo = "20241014B0701",
                UserId = "user456",
                QueryTime = DateTime.Parse("2024-10-14T14:45:00"),
                UserName = "李四",
            },
        };

        return ApiResponseHelper.Success(response);
    }
}
