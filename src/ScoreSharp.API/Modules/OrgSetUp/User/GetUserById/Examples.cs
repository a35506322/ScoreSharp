namespace ScoreSharp.API.Modules.OrgSetUp.User.GetUserById;

[ExampleAnnotation(Name = "[2000]取得使用者", ExampleType = ExampleType.Response)]
public class 取得使用者_2000_ResEx : IExampleProvider<ResultResponse<GetUserByIdResponse>>
{
    public ResultResponse<GetUserByIdResponse> GetExample()
    {
        GetUserByIdResponse response = new()
        {
            UserId = "SuperAdmin",
            UserName = "超級管理員",
            IsActive = "Y",
            AddTime = DateTime.Now,
            AddUserId = "ADMIN",
            UpdateTime = DateTime.Now,
            UpdateUserId = "ADMIN",
            LastUpdateMimaTime = DateTime.Now,
            MimaErrorCount = 1,
            OrganizeCode = "DP",
            StopReason = "",
            EmployeeNo = "",
        };
        return ApiResponseHelper.Success(response);
    }
}

[ExampleAnnotation(Name = "[2000]取得使用者-查無此資料", ExampleType = ExampleType.Response)]
public class 取得使用者查無此資料_4001_ResEx : IExampleProvider<ResultResponse<GetUserByIdResponse>>
{
    public ResultResponse<GetUserByIdResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetUserByIdResponse>(null, "SuperReviewer");
    }
}
