namespace ScoreSharp.API.Modules.Manage.Stakeholder.GetStakeholderById;

[ExampleAnnotation(Name = "[2000]查詢單筆利害關係人", ExampleType = ExampleType.Response)]
public class 查詢單筆利害關係人_2000_ResEx : IExampleProvider<ResultResponse<GetStakeholderByIdResponse>>
{
    public ResultResponse<GetStakeholderByIdResponse> GetExample()
    {
        GetStakeholderByIdResponse response = new()
        {
            IsActive = "Y",
            AddTime = DateTime.Now,
            AddUserId = "SYSTEM",
            UpdateTime = null,
            UpdateUserId = null,
        };
        return ApiResponseHelper.Success(response);
    }
}

[ExampleAnnotation(Name = "[4001]查詢單筆利害關係人-查無此資料", ExampleType = ExampleType.Response)]
public class 查詢單筆利害關係人查無此資料_4001_ResEx : IExampleProvider<ResultResponse<GetStakeholderByIdResponse>>
{
    public ResultResponse<GetStakeholderByIdResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetStakeholderByIdResponse>(null, "502");
    }
}
