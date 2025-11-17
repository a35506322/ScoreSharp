namespace ScoreSharp.API.Modules.SetUp.BillDay.GetBillDayByQueryString;

[ExampleAnnotation(Name = "[2000]取得全部帳單日", ExampleType = ExampleType.Response)]
public class 取得多筆帳單日_2000_ResEx : IExampleProvider<ResultResponse<List<GetBillDayByQueryStringResponse>>>
{
    public ResultResponse<List<GetBillDayByQueryStringResponse>> GetExample()
    {
        // Example data
        var data = new List<GetBillDayByQueryStringResponse>
        {
            new GetBillDayByQueryStringResponse
            {
                BillDay = "01",
                IsActive = "Y",
                AddUserId = "admin",
                AddTime = DateTime.Now,
                UpdateUserId = "admin",
                UpdateTime = DateTime.Now,
            },
            new GetBillDayByQueryStringResponse
            {
                BillDay = "27",
                IsActive = "Y",
                AddUserId = "admin",
                AddTime = DateTime.Now,
                UpdateUserId = "admin",
                UpdateTime = DateTime.Now,
            },
        };

        return ApiResponseHelper.Success(data);
    }
}
