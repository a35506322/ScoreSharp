namespace ScoreSharp.API.Modules.OrgSetUp.Organize.UpdateOrganizeById;

[ExampleAnnotation(Name = "[2000]更新組織", ExampleType = ExampleType.Request)]
public class 修改組織_2000_ReqEx : IExampleProvider<UpdateOrganizeByIdRequest>
{
    public UpdateOrganizeByIdRequest GetExample()
    {
        UpdateOrganizeByIdRequest request = new()
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

[ExampleAnnotation(Name = "[2000]更新組織", ExampleType = ExampleType.Response)]
public class 修改組織_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess("ORG001", "ORG001");
    }
}

[ExampleAnnotation(Name = "[4001]更新組織-查無此資料", ExampleType = ExampleType.Response)]
public class 修改組織查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "ORG001");
    }
}

[ExampleAnnotation(Name = "[4003]更新組織-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改組織路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
