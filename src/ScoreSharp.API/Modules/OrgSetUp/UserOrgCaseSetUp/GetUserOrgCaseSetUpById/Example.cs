namespace ScoreSharp.API.Modules.OrgSetUp.UserOrgCaseSetUp.GetUserOrgCaseSetUpById;

[ExampleAnnotation(Name = "[2000]查詢單筆人員組織分案群組設定", ExampleType = ExampleType.Response)]
public class 查詢單筆人員組織分案群組設定_2000_ResEx : IExampleProvider<ResultResponse<GetUserOrgCaseSetUpByIdResponse>>
{
    public ResultResponse<GetUserOrgCaseSetUpByIdResponse> GetExample()
    {
        GetUserOrgCaseSetUpByIdResponse response = new()
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
            UpdateUserId = null,
            UpdateTime = null,
            PaperCaseSort = 6,
            WebCaseSort = 4,
            IsManualCase = "N",
            ManualCaseSort = 1,
            CaseDispatchGroups =
            [
                new CaseDispatchGroupModel { CaseDispatchGroup = CaseDispatchGroup.台北徵審科 },
                new CaseDispatchGroupModel { CaseDispatchGroup = CaseDispatchGroup.高雄徵審科 },
            ],
        };
        return ApiResponseHelper.Success(response);
    }
}

[ExampleAnnotation(Name = "[2000]查詢單筆人員組織分案群組設定-查無此資料", ExampleType = ExampleType.Response)]
public class 查詢單筆人員組織分案群組設定查無此資料_4001_ResEx : IExampleProvider<ResultResponse<GetUserOrgCaseSetUpByIdResponse>>
{
    public ResultResponse<GetUserOrgCaseSetUpByIdResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetUserOrgCaseSetUpByIdResponse>(null, "kevin");
    }
}
