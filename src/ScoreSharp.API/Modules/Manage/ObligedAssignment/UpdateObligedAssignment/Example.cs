namespace ScoreSharp.API.Modules.Manage.ObligedAssignment.UpdateObligedAssignment;

[ExampleAnnotation(Name = "[2000]單筆強制派案", ExampleType = ExampleType.Request)]
public class 單筆強制派案_2000_ReqEx : IExampleProvider<UpdateObligedAssignmentRequest>
{
    public UpdateObligedAssignmentRequest GetExample()
    {
        var jsonString = """
                        {
                "applyNo": "2025100205545",
                "assignCaseList": [
                    {
                        "seqNo": "01K6J5PA800N420VVX5W80RWV4",
                        "cardStatus": 10221,
                        "assignmentChangeStatus": 10251
                    },
                    {
                        "seqNo": "01K6J5PA801PFG2D0736YYATDR",
                        "cardStatus": 10221,
                        "assignmentChangeStatus": 10251
                    }
                ],
                "assignToUserId": "janehuang"
            }
            """;
        var request = JsonSerializer.Deserialize<UpdateObligedAssignmentRequest>(jsonString);
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]單筆強制派案", ExampleType = ExampleType.Response)]
public class 單筆強制派案_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess<string>("2025100205545", "2025100205545");
    }
}

[ExampleAnnotation(Name = "[4003]單筆強制派案-指派人員不符合指派規則", ExampleType = ExampleType.Response)]
public class 單筆強制派案指派人員不符合指派規則_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>(null, "被指派人員：arthurlin 與月收入確認人員相同");
    }
}
