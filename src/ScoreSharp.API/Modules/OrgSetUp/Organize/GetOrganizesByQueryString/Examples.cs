namespace ScoreSharp.API.Modules.OrgSetUp.Organize.GetOrganizesByQueryString;

[ExampleAnnotation(Name = "[2000]取得組織", ExampleType = ExampleType.Response)]
public class 取得組織_2000_ResEx : IExampleProvider<ResultResponse<List<GetOrganizesByQueryStringResponse>>>
{
    public ResultResponse<List<GetOrganizesByQueryStringResponse>> GetExample()
    {
        var data = new List<GetOrganizesByQueryStringResponse>
        {
            new GetOrganizesByQueryStringResponse
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
            },
            new GetOrganizesByQueryStringResponse
            {
                OrganizeCode = "ORG002",
                OrganizeName = "測試組織2",
                LegalRepresentative = "李四",
                ZIPCode = "101",
                FullAddress = "台北市中正區",
                AddUserId = "ADMIN",
                AddTime = DateTime.Now,
                UpdateUserId = "ADMIN",
                UpdateTime = DateTime.Now,
            },
        };

        return ApiResponseHelper.Success(data);
    }
}
