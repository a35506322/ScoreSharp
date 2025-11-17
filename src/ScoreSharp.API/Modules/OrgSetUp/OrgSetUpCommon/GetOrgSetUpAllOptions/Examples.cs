namespace ScoreSharp.API.Modules.OrgSetUp.OrgSetUpCommon.GetOrgSetUpAllOptions;

[ExampleAnnotation(Name = "[2000]取得全部組織設定相關下拉選單", ExampleType = ExampleType.Response)]
public class 取得全部組織設定相關下拉選單_2000_ResEx : IExampleProvider<ResultResponse<GetOrgSetUpAllOptionsResponse>>
{
    public ResultResponse<GetOrgSetUpAllOptionsResponse> GetExample()
    {
        var response = new GetOrgSetUpAllOptionsResponse { CaseDispatchGroup = EnumExtenstions.GetEnumOptions<CaseDispatchGroup>("Y") };

        return ApiResponseHelper.Success(response);
    }
}
