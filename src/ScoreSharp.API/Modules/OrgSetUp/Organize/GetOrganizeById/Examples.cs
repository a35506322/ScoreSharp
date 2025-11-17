namespace ScoreSharp.API.Modules.OrgSetUp.Organize.GetOrganizeById;

[ExampleAnnotation(Name = "[4001]取得組織-查無此資料", ExampleType = ExampleType.Response)]
public class 取得組織查無此資料_4001_ResEx : IExampleProvider<ResultResponse<GetOrganizeByIdResponse>>
{
    public ResultResponse<GetOrganizeByIdResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetOrganizeByIdResponse>(null, "DP");
    }
}

[ExampleAnnotation(Name = "[2000]取得組織", ExampleType = ExampleType.Response)]
public class 取得組織_2000_ResEx : IExampleProvider<ResultResponse<GetOrganizeByIdResponse>>
{
    public ResultResponse<GetOrganizeByIdResponse> GetExample()
    {
        GetOrganizeByIdResponse response = new GetOrganizeByIdResponse
        {
            OrganizeCode = "ORG001",
            OrganizeName = "測試組織",
            LegalRepresentative = "張三",
            ZIPCode = "100",
            FullAddress = "台北市中正區",
            AddUserId = "ADMIN",
            AddTime = DateTime.Now,
            UpdateUserId = "ADMIN",
            UpdateTime = DateTime.Now,
        };

        return ApiResponseHelper.Success(response);
    }
}
