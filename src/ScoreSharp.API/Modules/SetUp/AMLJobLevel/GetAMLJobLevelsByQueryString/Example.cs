namespace ScoreSharp.API.Modules.SetUp.AMLJobLevel.GetAMLJobLevelsByQueryString;

[ExampleAnnotation(Name = "[2000]取得AML職級別", ExampleType = ExampleType.Response)]
public class 取得AML職級別_2000_ResEx : IExampleProvider<ResultResponse<List<GetAMLJobLevelsByQueryStringResponse>>>
{
    public ResultResponse<List<GetAMLJobLevelsByQueryStringResponse>> GetExample()
    {
        var data = new List<GetAMLJobLevelsByQueryStringResponse>
        {
            new GetAMLJobLevelsByQueryStringResponse
            {
                AMLJobLevelCode = "1",
                AMLJobLevelName = "職員",
                IsSeniorManagers = "N",
                IsActive = "Y",
                AddTime = DateTime.Now,
                AddUserId = "ADMIN",
                UpdateTime = DateTime.Now,
                UpdateUserId = "ADMIN",
            },
        };
        return ApiResponseHelper.Success(data);
    }
}
