namespace ScoreSharp.API.Modules.Manage.UnassignedCasesList.BatchUpdateCurrentHandleUserId;

[ExampleAnnotation(Name = "[2000]批次修改當前經辦", ExampleType = ExampleType.Request)]
public class 批次修改當前經辦_2000_ReqEx : IExampleProvider<BatchUpdateCurrentHandleUserIdRequest>
{
    public BatchUpdateCurrentHandleUserIdRequest GetExample() =>
        new BatchUpdateCurrentHandleUserIdRequest()
        {
            ApplyNo = new List<string> { "20250102X7534", "20250102F6911", "20250102Z7050" },
            CurrentHandleUserId = "rahul",
            CaseAssignmentType = CaseAssignmentType.網路件月收入預審_姓名檢核Y清單,
        };
}

[ExampleAnnotation(Name = "[2000]批次修改當前經辦", ExampleType = ExampleType.Response)]
public class 批次修改當前經辦_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.Success<string>(null, "更新成功");
}
