namespace ScoreSharp.API.Modules.Manage.Common.GetAssignmentUsers;

[ExampleAnnotation(Name = "[2000]取得能分配的徵審人員", ExampleType = ExampleType.Response)]
public class 取得可派案的徵審人員_2000_ResEx : IExampleProvider<ResultResponse<List<GetAssignmentUsersResponse>>>
{
    public ResultResponse<List<GetAssignmentUsersResponse>> GetExample()
    {
        var result = new List<GetAssignmentUsersResponse>
        {
            new GetAssignmentUsersResponse
            {
                UserId = "arthurlin",
                UserName = "林芃均",
                CaseDispatchGroups =
                [
                    new CaseDispatchGroupInfo { CaseDispatchGroup = CaseDispatchGroup.台北徵審科 },
                    new CaseDispatchGroupInfo { CaseDispatchGroup = CaseDispatchGroup.外派徵審人員 },
                ],
                IsVacation = "N",
                IsPaperCase = "Y",
                PaperCaseSort = 1,
                IsWebCase = "Y",
                WebCaseSort = 1,
                IsManualCase = "N",
                ManualCaseSort = 1,
            },
            new GetAssignmentUsersResponse
            {
                UserId = "yunxilin",
                UserName = "林昀希",
                CaseDispatchGroups =
                [
                    new CaseDispatchGroupInfo { CaseDispatchGroup = CaseDispatchGroup.台北徵審科 },
                    new CaseDispatchGroupInfo { CaseDispatchGroup = CaseDispatchGroup.高雄徵審科 },
                ],
                IsVacation = "N",
                IsPaperCase = "Y",
                PaperCaseSort = 2,
                IsWebCase = "Y",
                WebCaseSort = 2,
                IsManualCase = "N",
                ManualCaseSort = 2,
            },
        };

        return ApiResponseHelper.Success(result);
    }
}
