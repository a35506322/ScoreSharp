namespace ScoreSharp.API.Modules.OrgSetUp.UserOrgCaseSetUp.GetUserOrgCasesSetUpByQueryString;

[ExampleAnnotation(Name = "[2000]查詢多筆人員組織分案群組設定", ExampleType = ExampleType.Response)]
public class 查詢多筆人員組織分案群組設定_2000_ResEx : IExampleProvider<ResultResponse<List<GetUserOrgCasesSetUpByQueryStringResponse>>>
{
    public ResultResponse<List<GetUserOrgCasesSetUpByQueryStringResponse>> GetExample()
    {
        var data = new List<GetUserOrgCasesSetUpByQueryStringResponse>
        {
            new GetUserOrgCasesSetUpByQueryStringResponse
            {
                UserId = "raya00",
                UserName = "A00清宏",
                CardLimit = 450000,
                DesignatedSupervisor1 = "poshenghsu",
                DesignatedSupervisor1Name = "許博盛",
                DesignatedSupervisor2 = null,
                DesignatedSupervisor2Name = null,
                IsPaperCase = "Y",
                IsWebCase = "Y",
                CheckWeight = 0,
                AddUserId = "tinalien",
                AddTime = DateTime.Now,
                UpdateUserId = "tinalien",
                UpdateTime = DateTime.Now,
                PaperCaseSort = 5,
                WebCaseSort = 7,
                IsManualCase = "N",
                ManualCaseSort = 1,
                CaseDispatchGroups =
                [
                    new CaseDispatchGroupModel { CaseDispatchGroup = CaseDispatchGroup.台北徵審科 },
                    new CaseDispatchGroupModel { CaseDispatchGroup = CaseDispatchGroup.高雄徵審科 },
                ],
            },
            new GetUserOrgCasesSetUpByQueryStringResponse
            {
                UserId = "hanschen",
                UserName = "陳世翰",
                CardLimit = 450000,
                DesignatedSupervisor1 = null,
                DesignatedSupervisor1Name = null,
                DesignatedSupervisor2 = null,
                DesignatedSupervisor2Name = null,
                IsPaperCase = "Y",
                IsWebCase = "N",
                CheckWeight = 0,
                AddUserId = "tinalien",
                AddTime = DateTime.Now,
                UpdateUserId = null,
                UpdateTime = null,
                PaperCaseSort = 4,
                WebCaseSort = 6,
                IsManualCase = "N",
                ManualCaseSort = 1,
            },
        };

        return ApiResponseHelper.Success(data);
    }
}
