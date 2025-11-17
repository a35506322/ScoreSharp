namespace ScoreSharp.API.Modules.Manage.UnassignedCasesList.BatchAssignCasesAutomatically;

[ExampleAnnotation(Name = "[2000]整批派案", ExampleType = ExampleType.Request)]
public class 整批派案_2000_ReqEx : IExampleProvider<BatchAssignCasesAutomaticallyRequest>
{
    public BatchAssignCasesAutomaticallyRequest GetExample()
    {
        return new BatchAssignCasesAutomaticallyRequest
        {
            CaseAssignmentType = CaseAssignmentType.網路件月收入預審_姓名檢核Y清單,
            AssignCaseUserInfos =
            [
                new AssignUserInfo { AssignedUserId = "arthurlin", CaseCount = 2 },
                new AssignUserInfo { AssignedUserId = "janehuang", CaseCount = 3 },
            ],
        };
    }
}

[ExampleAnnotation(Name = "[2000]整批派案-成功", ExampleType = ExampleType.Response)]
public class 整批派案_2000_ResEx : IExampleProvider<ResultResponse<BatchAssignCasesAutomaticallyResponse>>
{
    public ResultResponse<BatchAssignCasesAutomaticallyResponse> GetExample()
    {
        BatchAssignCasesAutomaticallyResponse response = new() { FileName = "整批派案.xlsx", ResultFile = null };

        return ApiResponseHelper.Success(response);
    }
}

[ExampleAnnotation(Name = "[4003]整批派案-商業邏輯驗證失敗", ExampleType = ExampleType.Response)]
public class 商業邏輯驗證失敗_4003_ResEx : IExampleProvider<ResultResponse<BatchAssignCasesAutomaticallyResponse>>
{
    public ResultResponse<BatchAssignCasesAutomaticallyResponse> GetExample()
    {
        var caseAssignmentType = CaseAssignmentType.網路件月收入預審_姓名檢核Y清單;

        return ApiResponseHelper.BusinessLogicFailed<BatchAssignCasesAutomaticallyResponse>(null, $"{caseAssignmentType} 派案案件數量為 0");
    }
}
