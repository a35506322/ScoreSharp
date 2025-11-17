using ScoreSharp.API.Modules.Manage.UnassignedCasesList.BatchAssignCasesAutomatically;

namespace ScoreSharp.API.Modules.Manage.TransferCase.ExecTransferCaseAuto;

[ExampleAnnotation(Name = "[2000]智慧調撥案件", ExampleType = ExampleType.Request)]
public class 智慧調撥案件_2000_ReqEx : IExampleProvider<ExecTransferCaseAutoRequest>
{
    public ExecTransferCaseAutoRequest GetExample()
    {
        return new ExecTransferCaseAutoRequest
        {
            HandleUserId = "arthurlin",
            TransferredUserId = "janehuang",
            TransferredCaseCount = 3,
            TransferCaseType = TransferCaseType.網路件月收入預審,
        };
    }
}

[ExampleAnnotation(Name = "[2000]成功", ExampleType = ExampleType.Response)]
public class 智慧調撥案件_2000_ResEx : IExampleProvider<ResultResponse<ExecTransferCaseAutoResponse>>
{
    public ResultResponse<ExecTransferCaseAutoResponse> GetExample()
    {
        ExecTransferCaseAutoResponse response = new() { FileName = "智慧調撥案件結果.xlsx", ResultFile = null };
        return ApiResponseHelper.Success(response);
    }
}

[ExampleAnnotation(Name = "[4003]商業邏輯驗證失敗-調撥案件數量為0", ExampleType = ExampleType.Response)]
public class 商業邏輯驗證失敗_調撥案件數量為0_4003_ResEx : IExampleProvider<ResultResponse<ExecTransferCaseAutoResponse>>
{
    public ResultResponse<ExecTransferCaseAutoResponse> GetExample()
    {
        var transferCsaeType = TransferCaseType.網路件月收入預審;

        return ApiResponseHelper.BusinessLogicFailed<ExecTransferCaseAutoResponse>(null, $"{transferCsaeType} 調撥案件數量為 0");
    }
}

[ExampleAnnotation(Name = "[4003]商業邏輯驗證失敗-與指派人員的派案組織不同", ExampleType = ExampleType.Response)]
public class 商業邏輯驗證失敗_與指派人員的派案組織不同_4003_ResEx : IExampleProvider<ResultResponse<ExecTransferCaseAutoResponse>>
{
    public ResultResponse<ExecTransferCaseAutoResponse> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<ExecTransferCaseAutoResponse>(null, $"與指派人員的派案組織不同");
    }
}
