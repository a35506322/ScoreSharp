namespace ScoreSharp.API.Modules.Manage.Common.GetManageCommonAllOptions;

[ExampleAnnotation(Name = "[2000]取得全部管理作業相關下拉選單", ExampleType = ExampleType.Response)]
public class 取得全部管理作業相關下拉選單_2000_ResEx : IExampleProvider<ResultResponse<GetManageCommonAllOptionsResponse>>
{
    public ResultResponse<GetManageCommonAllOptionsResponse> GetExample()
    {
        var response = new GetManageCommonAllOptionsResponse
        {
            CaseAssignmentType = EnumExtenstions.GetEnumOptions<CaseAssignmentType>("Y"),
            AssignmentChangeStatus = EnumExtenstions.GetEnumOptions<AssignmentChangeStatus>("Y"),
            TransferCaseType = EnumExtenstions.GetEnumOptions<TransferCaseType>("Y"),
            CaseStatisticType = EnumExtenstions.GetEnumOptions<CaseStatisticType>("Y"),
        };

        return ApiResponseHelper.Success(response);
    }
}
