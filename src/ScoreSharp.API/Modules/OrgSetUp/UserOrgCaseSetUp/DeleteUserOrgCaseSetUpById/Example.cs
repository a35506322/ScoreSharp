namespace ScoreSharp.API.Modules.OrgSetUp.UserOrgCaseSetUp.DeleteUserOrgCaseSetUpById;

[ExampleAnnotation(Name = "[4001]刪除單筆人員組織分案群組設定-查無此資料", ExampleType = ExampleType.Response)]
public class 刪除單筆人員組織分案群組設定查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "kevin");
    }
}

[ExampleAnnotation(Name = "[2000]刪除單筆人員組織分案群組設定", ExampleType = ExampleType.Response)]
public class 刪除單筆人員組織分案群組設定_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("raya00");
    }
}
