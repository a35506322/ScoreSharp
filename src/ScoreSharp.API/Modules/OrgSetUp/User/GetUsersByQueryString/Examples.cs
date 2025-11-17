namespace ScoreSharp.API.Modules.OrgSetUp.User.GetUsersByQueryString;

[ExampleAnnotation(Name = "[2000]取得使用者", ExampleType = ExampleType.Response)]
public class 取得使用者_2000_ResEx : IExampleProvider<ResultResponse<List<GetUsersByQueryStringResponse>>>
{
    public ResultResponse<List<GetUsersByQueryStringResponse>> GetExample()
    {
        // Example data
        var data = new List<GetUsersByQueryStringResponse>
        {
            new GetUsersByQueryStringResponse
            {
                UserId = "SuperAdmin",
                UserName = "超級管理員",
                IsActive = "Y",
                AddTime = DateTime.Now,
                AddUserId = "ADMIN",
                UpdateTime = DateTime.Now,
                UpdateUserId = "ADMIN",
                IsAD = "N",
                LastUpdateMimaTime = DateTime.Now,
                MimaErrorCount = 1,
                OrganizeCode = "DP",
                StopReason = "",
                CaseDispatchGroups = [new CaseDispatchGroupModel { CaseDispatchGroup = CaseDispatchGroup.台北徵審科 }],
                EmployeeNo = "",
            },
            new GetUsersByQueryStringResponse
            {
                UserId = "SuperAdmin2",
                UserName = "超級管理員2",
                IsActive = "N",
                AddTime = DateTime.Now,
                AddUserId = "ADMIN",
                UpdateTime = DateTime.Now,
                UpdateUserId = "ADMIN",
                IsAD = "N",
                LastUpdateMimaTime = DateTime.Now,
                MimaErrorCount = 5,
                OrganizeCode = "DP",
                StopReason = "密碼輸入超過5次",
                CaseDispatchGroups =
                [
                    new CaseDispatchGroupModel { CaseDispatchGroup = CaseDispatchGroup.台北徵審科 },
                    new CaseDispatchGroupModel { CaseDispatchGroup = CaseDispatchGroup.高雄徵審科 },
                ],
                EmployeeNo = "",
            },
        };

        return ApiResponseHelper.Success(data);
    }
}
