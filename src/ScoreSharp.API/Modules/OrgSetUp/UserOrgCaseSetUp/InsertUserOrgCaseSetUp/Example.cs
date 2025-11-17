namespace ScoreSharp.API.Modules.OrgSetUp.UserOrgCaseSetUp.InsertUserOrgCaseSetUp;

[ExampleAnnotation(Name = "[2000]新增單筆人員組織分案群設定", ExampleType = ExampleType.Request)]
public class 新增單筆人員組織分案群設定_2000_ReqEx : IExampleProvider<InsertUserOrgCaseSetUpRequest>
{
    public InsertUserOrgCaseSetUpRequest GetExample()
    {
        InsertUserOrgCaseSetUpRequest request = new()
        {
            UserId = "徵審人員的使用者帳號",
            CardLimit = 5000000,
            DesignatedSupervisor1 = null,
            DesignatedSupervisor2 = null,
            IsPaperCase = "N",
            IsWebCase = "Y",
            CheckWeight = 75,
            PaperCaseSort = 3,
            WebCaseSort = 3,
            IsManualCase = "N",
            ManualCaseSort = 1,
        };

        return request;
    }
}

[ExampleAnnotation(Name = "[2000]新增單筆人員組織分案群設定", ExampleType = ExampleType.Response)]
public class 新增單筆人員組織分案群設定_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess("linachen", "linachen");
    }
}

[ExampleAnnotation(Name = "[2000]新增單筆人員組織分案群設定-使用者帳號不在使用者名單中", ExampleType = ExampleType.Request)]
public class 新增單筆人員組織分案群設定使用者帳號不在使用者名單中_4003_ReqEx : IExampleProvider<InsertUserOrgCaseSetUpRequest>
{
    public InsertUserOrgCaseSetUpRequest GetExample()
    {
        InsertUserOrgCaseSetUpRequest request = new()
        {
            UserId = "不在名單內的使用者帳號",
            CardLimit = 5000000,
            DesignatedSupervisor1 = null,
            DesignatedSupervisor2 = null,
            IsPaperCase = "N",
            IsWebCase = "Y",
            CheckWeight = 75,
            PaperCaseSort = 3,
            WebCaseSort = 3,
            IsManualCase = "N",
            ManualCaseSort = 1,
        };

        return request;
    }
}

[ExampleAnnotation(Name = "[4003]新增單筆人員組織分案群設定-使用者帳號不在使用者名單中", ExampleType = ExampleType.Response)]
public class 新增單筆人員組織分案群設定使用者帳號不在使用者名單中_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>("不在名單內的使用者帳號", "該使用者帳號 不在使用者名單中");
    }
}

[ExampleAnnotation(Name = "[2000]新增單筆人員組織分案群設定-指定主管不在使用者名單中", ExampleType = ExampleType.Request)]
public class 新增單筆人員組織分案群設定指定主管不在使用者名單中_4003_ReqEx : IExampleProvider<InsertUserOrgCaseSetUpRequest>
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
            IsManualCase = "N",
            ManualCaseSort = 1,
        };

        return request;
    }
}

[ExampleAnnotation(Name = "[4003]新增單筆人員組織分案群設定-指定主管不在使用者名單中", ExampleType = ExampleType.Response)]
public class 新增單筆人員組織分案群設定指定主管不在使用者名單中_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>("不在使用者名單內的帳號", "指定主管一 不在使用者名單中");
    }
}

[ExampleAnnotation(Name = "[4002]新增單筆人員組織分案群設定-資料已存在", ExampleType = ExampleType.Response)]
public class 新增單筆人員組織分案群設定資料已存在_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DataAlreadyExists(string.Empty, "linachen");
    }
}
