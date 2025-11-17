namespace ScoreSharp.API.Modules.Manage.Stakeholder.GetStakeholderByQueryString;

[ExampleAnnotation(Name = "[2000]查詢多筆利害關係人", ExampleType = ExampleType.Response)]
public class 查詢多筆利害關係人_2000_ResEx : IExampleProvider<ResultResponse<List<GetStakeholderByQueryStringResponse>>>
{
    public ResultResponse<List<GetStakeholderByQueryStringResponse>> GetExample()
    {
        var data = new List<GetStakeholderByQueryStringResponse>
        {
            new GetStakeholderByQueryStringResponse
            {
                SeqNo = 1,
                IsActive = "Y",
                AddTime = DateTime.Now,
                AddUserId = "alexislee",
                UpdateTime = null,
                UpdateUserId = null,
            },
            new GetStakeholderByQueryStringResponse
            {
                SeqNo = 3,
                ID = "W259630351",
                UserId = "alexislee",
                AddUserId = "SYSTEM",
                AddTime = DateTime.Now,
                UpdateUserId = null,
                UpdateTime = null,
                IsActive = "Y",
            },
        };
        return ApiResponseHelper.Success(data);
    }
}
