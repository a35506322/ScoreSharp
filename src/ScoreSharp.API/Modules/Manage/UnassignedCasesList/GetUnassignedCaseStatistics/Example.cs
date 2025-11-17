namespace ScoreSharp.API.Modules.Manage.UnassignedCasesList.GetUnassignedCaseStatistics;

[ExampleAnnotation(Name = "[2000]取得未分派案件統計", ExampleType = ExampleType.Response)]
public class 取得未分派案件統計_2000_ResEx : IExampleProvider<ResultResponse<GetUnassignedCaseStatisticsResponse>>
{
    public ResultResponse<GetUnassignedCaseStatisticsResponse> GetExample()
    {
        return ApiResponseHelper.Success(
            new GetUnassignedCaseStatisticsResponse
            {
                new()
                {
                    Id = (int)CaseAssignmentType.網路件月收入預審_姓名檢核Y清單,
                    Name = CaseAssignmentType.網路件月收入預審_姓名檢核Y清單.ToName(),
                    Value = 13,
                },
                new()
                {
                    Id = (int)CaseAssignmentType.網路件月收入預審_姓名檢核N清單,
                    Name = CaseAssignmentType.網路件月收入預審_姓名檢核N清單.ToName(),
                    Value = 20,
                },
                new()
                {
                    Id = (int)CaseAssignmentType.網路件人工徵信中,
                    Name = CaseAssignmentType.網路件人工徵信中.ToName(),
                    Value = 25,
                },
                new()
                {
                    Id = (int)CaseAssignmentType.紙本件月收入預審_姓名檢核Y清單,
                    Name = CaseAssignmentType.紙本件月收入預審_姓名檢核Y清單.ToName(),
                    Value = 58,
                },
                new()
                {
                    Id = (int)CaseAssignmentType.紙本件月收入預審_姓名檢核N清單,
                    Name = CaseAssignmentType.紙本件月收入預審_姓名檢核N清單.ToName(),
                    Value = 30,
                },
                new()
                {
                    Id = (int)CaseAssignmentType.紙本件人工徵信中,
                    Name = CaseAssignmentType.紙本件人工徵信中.ToName(),
                    Value = 20,
                },
                new()
                {
                    Id = 7,
                    Name = "總件數",
                    Value = 166,
                },
            }
        );
    }
}
