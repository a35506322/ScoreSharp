using ScoreSharp.API.Modules.OrgSetUp.UserOrgCaseSetUp.InsertUserOrgCaseSetUp;

namespace ScoreSharp.API.Modules.OrgSetUp.UserOrgCaseSetUp.UpdateUserOrgCaseSetUpById;

[ExampleAnnotation(Name = "[2000]更新單筆人員組織分案群組設定", ExampleType = ExampleType.Request)]
public class 修改人員組織分案群組設定_2000_ReqEx : IExampleProvider<UpdateUserOrgCaseSetUpByIdRequest>
{
    public UpdateUserOrgCaseSetUpByIdRequest GetExample()
    {
        UpdateUserOrgCaseSetUpByIdRequest request = new()
        {
            UserId = "徵審人員的使用者帳號",
            CardLimit = 430000,
            DesignatedSupervisor1 = null,
            DesignatedSupervisor2 = null,
            IsPaperCase = "Y",
            IsWebCase = "N",
            CheckWeight = 0,
            PaperCaseSort = 7,
            WebCaseSort = 9,
            IsManualCase = "N",
            ManualCaseSort = 1,
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]更新單筆人員組織分案群組設定", ExampleType = ExampleType.Response)]
public class 修改人員組織分案群組設定_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess("raya00", "raya00");
    }
}

[ExampleAnnotation(Name = "[4001]更新單筆人員組織分案群組設定-查無此資料", ExampleType = ExampleType.Response)]
public class 修改人員組織分案群組設定查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "kevin");
    }
}

[ExampleAnnotation(Name = "[4003]更新單筆人員組織分案群組設定-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改人員組織分案群組設定路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}

[ExampleAnnotation(Name = "[2000]更新人員組織分案群設定-指定主管不在使用者名單中", ExampleType = ExampleType.Request)]
public class 修改人員組織分案群設定指定主管不在使用者名單中_4003_ReqEx : IExampleProvider<InsertUserOrgCaseSetUpRequest>
{
    public InsertUserOrgCaseSetUpRequest GetExample()
    {
        InsertUserOrgCaseSetUpRequest request = new()
        {
            UserId = "徵審人員的使用者帳號",
            CardLimit = 5000000,
            DesignatedSupervisor1 = "不存在的使用者帳號",
            DesignatedSupervisor2 = null,
            IsPaperCase = "N",
            IsWebCase = "Y",
            CheckWeight = 75,
            PaperCaseSort = 3,
            WebCaseSort = 3,
        };

        return request;
    }
}

[ExampleAnnotation(Name = "[4003]更新人員組織分案群設定-指定主管不在使用者名單中", ExampleType = ExampleType.Response)]
public class 修改人員組織分案群設定指定主管不在使用者名單中_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>("不在使用者名單內的帳號", "指定主管一 不在使用者名單中");
    }
}
