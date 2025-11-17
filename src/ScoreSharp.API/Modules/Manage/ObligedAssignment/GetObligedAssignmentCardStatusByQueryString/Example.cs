namespace ScoreSharp.API.Modules.Manage.ObligedAssignment.GetObligedAssignmentCardStatusByQueryString;

[ExampleAnnotation(Name = "[2000]取得強制派案案件狀態", ExampleType = ExampleType.Response)]
public class 取得強制派案案件狀態_2000_ResEx : IExampleProvider<ResultResponse<List<GetObligedAssignmentCardStatusByQueryStringResponse>>>
{
    public ResultResponse<List<GetObligedAssignmentCardStatusByQueryStringResponse>> GetExample()
    {
        var response = new List<GetObligedAssignmentCardStatusByQueryStringResponse>()
        {
            new GetObligedAssignmentCardStatusByQueryStringResponse
            {
                CaseQueryCardStatus = CardStatus.製卡失敗,
                RoleIds = new List<string> { "admin", "reviewer" },
            },
            new GetObligedAssignmentCardStatusByQueryStringResponse
            {
                CaseQueryCardStatus = CardStatus.拒撤退重審,
                RoleIds = new List<string> { "admin" },
            },
        };

        return ApiResponseHelper.Success(response);
    }
}
