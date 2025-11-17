namespace ScoreSharp.API.Modules.OrgSetUp.Organize.InsertOrganize;

[ExampleAnnotation(Name = "[2000]新增組織", ExampleType = ExampleType.Request)]
public class 新增組織_2000_ReqEx : IExampleProvider<InsertOrganizeRequest>
{
    public InsertOrganizeRequest GetExample()
    {
        InsertOrganizeRequest request = new()
        {
            OrganizeCode = "ORG001",
            OrganizeName = "測試組織",
            LegalRepresentative = "張三",
            ZIPCode = "100",
            FullAddress = "台北市中正區",
        };

        return request;
    }
}

[ExampleAnnotation(Name = "[2000]新增組織", ExampleType = ExampleType.Response)]
public class 新增組織_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess("ORG001", "ORG001");
    }
}

[ExampleAnnotation(Name = "[4002]新增組織-資料已存在", ExampleType = ExampleType.Response)]
public class 新增組織資料已存在_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DataAlreadyExists(string.Empty, "ORG001");
    }
}
