namespace ScoreSharp.API.Modules.Manage.TransferCase.ExecTransferCaseManual;

[ExampleAnnotation(Name = "[2000]手動調撥案件", ExampleType = ExampleType.Request)]
public class 手動調撥案件_2000_ReqEx : IExampleProvider<ExecTransferCaseManualRequest>
{
    public ExecTransferCaseManualRequest GetExample()
    {
        return new ExecTransferCaseManualRequest
        {
            HandleUserId = "arthurlin",
            TransferredUserId = "janehuang",
            ApplyNos = ["20251015B8596", "20251015B8391"],
        };
    }
}

[ExampleAnnotation(Name = "[2000]成功", ExampleType = ExampleType.Response)]
public class 手動調撥案件_2000_ResEx : IExampleProvider<ResultResponse<ExecTransferCaseManualResponse>>
{
    public ResultResponse<ExecTransferCaseManualResponse> GetExample()
    {
        return ApiResponseHelper.Success(new ExecTransferCaseManualResponse { FileName = "手動調撥案件結果.xlsx", ResultFile = null });
    }
}

[ExampleAnnotation(Name = "[4003]商業邏輯驗證失敗-調撥指定案件數量與系統查找的筆數不符", ExampleType = ExampleType.Response)]
public class 商業邏輯驗證失敗_調撥指定案件數量與系統查找的筆數不符_4003_ResEx : IExampleProvider<ResultResponse<ExecTransferCaseManualResponse>>
{
    public ResultResponse<ExecTransferCaseManualResponse> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<ExecTransferCaseManualResponse>(null, "調撥指定案件數量與系統查找的筆數不符");
    }
}

[ExampleAnnotation(Name = "[4003]商業邏輯驗證失敗-與指派人員的派案組織不同", ExampleType = ExampleType.Response)]
public class 商業邏輯驗證失敗_與指派人員的派案組織不同_4003_ResEx : IExampleProvider<ResultResponse<ExecTransferCaseManualResponse>>
{
    public ResultResponse<ExecTransferCaseManualResponse> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<ExecTransferCaseManualResponse>(null, $"與指派人員的派案組織不同");
    }
}
