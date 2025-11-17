namespace ScoreSharp.API.Modules.SetUp.AMLJobLevel.GetAMLJobLevelById;

[ExampleAnnotation(Name = "[2000]取得AML職級別", ExampleType = ExampleType.Response)]
public class 取得AML職級別_2000_ResEx : IExampleProvider<ResultResponse<GetAMLJobLevelByIdResponse>>
{
    public ResultResponse<GetAMLJobLevelByIdResponse> GetExample()
    {
        GetAMLJobLevelByIdResponse response = new()
        {
            AMLJobLevelCode = "2",
            AMLJobLevelName = "財務主管或其他主管職",
            IsSeniorManagers = "N",
            IsActive = "Y",
            AddUserId = "ADMIN",
            AddTime = DateTime.Now,
            UpdateTime = DateTime.Now,
            UpdateUserId = "ADMIN",
        };
        return ApiResponseHelper.Success(response);
    }
}

[ExampleAnnotation(Name = "[4001]取得AML職級別-查無此資料", ExampleType = ExampleType.Response)]
public class 取得AML職級別查無此資料_4001_ResEx : IExampleProvider<ResultResponse<GetAMLJobLevelByIdResponse>>
{
    public ResultResponse<GetAMLJobLevelByIdResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetAMLJobLevelByIdResponse>(null, "13");
    }
}
