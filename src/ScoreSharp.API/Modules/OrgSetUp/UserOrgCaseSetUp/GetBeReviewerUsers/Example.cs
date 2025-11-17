namespace ScoreSharp.API.Modules.OrgSetUp.UserOrgCaseSetUp.GetBeReviewerUsers;

[ExampleAnnotation(Name = "[2000]查詢可新增人員", ExampleType = ExampleType.Response)]
public class 查詢可新增人員_2000_ResEx : IExampleProvider<ResultResponse<List<GetBeReviewerUsersResponse>>>
{
    public ResultResponse<List<GetBeReviewerUsersResponse>> GetExample()
    {
        List<GetBeReviewerUsersResponse> response = new()
        {
            new GetBeReviewerUsersResponse { UserId = "janehuang", UserName = "黃亭蓁" },
            new GetBeReviewerUsersResponse { UserId = "linachen", UserName = "陳書齊" },
            new GetBeReviewerUsersResponse { UserId = "arthurlin", UserName = "林芃均" },
        };
        return ApiResponseHelper.Success(response);
    }
}
