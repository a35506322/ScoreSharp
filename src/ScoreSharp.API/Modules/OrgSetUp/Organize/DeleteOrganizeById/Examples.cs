namespace ScoreSharp.API.Modules.OrgSetUp.Organize.DeleteOrganizeById;

[ExampleAnnotation(Name = "[4001]刪除組織-查無此資料", ExampleType = ExampleType.Response)]
public class 刪除組織查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "DP");
    }
}

[ExampleAnnotation(Name = "[2000]刪除組織", ExampleType = ExampleType.Response)]
public class 刪除組織_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("DP");
    }
}
