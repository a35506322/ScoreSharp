namespace ScoreSharp.API.Modules.Reviewer.ReviewManual.ReturnInManualReviewByApplyNo;

[ExampleAnnotation(Name = "[2000]人工徵信_退回重審", ExampleType = ExampleType.Request)]
public class 人工徵信_退回重審_2000_ReqEx : IExampleProvider<ReturnInManualReviewByApplyNoRequest>
{
    public ReturnInManualReviewByApplyNoRequest GetExample()
    {
        return new ReturnInManualReviewByApplyNoRequest() { ApplyNo = "20240601A0001", Note = "退回重審註記" };
    }
}

[ExampleAnnotation(Name = "[4003]人工徵信_退回重審_卡片狀態不符合", ExampleType = ExampleType.Response)]
public class 人工徵信_退回重審_卡片狀態不符合_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>(
            "20240601A0001",
            "執行退回重審，狀態只能為申請核卡中、申請退件中、申請補件中、申請撤件中、退回重審"
        );
    }
}

[ExampleAnnotation(Name = "[4003]人工徵信_退回重審_查無篩選最新指派卡片記錄", ExampleType = ExampleType.Response)]
public class 人工徵信_退回重審_查無篩選最新指派卡片記錄_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>("20240601A0001", "執行退回重審，查無篩選最新指派卡片記錄");
    }
}

[ExampleAnnotation(Name = "[2000]人工徵信_退回重審", ExampleType = ExampleType.Response)]
public class 人工徵信_退回重審_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.Success<string>("20240601A0001", "案件編號: 20240601A0001，退回重審成功");
    }
}
