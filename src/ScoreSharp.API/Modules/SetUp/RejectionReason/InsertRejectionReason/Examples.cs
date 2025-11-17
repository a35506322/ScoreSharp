namespace ScoreSharp.API.Modules.SetUp.RejectionReason.InsertRejectionReason;

[ExampleAnnotation(Name = "[2000]新增退件原因", ExampleType = ExampleType.Request)]
public class 新增退件原因_2000_ReqEx : IExampleProvider<InsertRejectionReasonRequest>
{
    public InsertRejectionReasonRequest GetExample()
    {
        InsertRejectionReasonRequest request = new()
        {
            RejectionReasonCode = "04",
            RejectionReasonName = "申請資料不完整或不正確",
            IsActive = "Y",
        };

        return request;
    }
}

[ExampleAnnotation(Name = "[2000]新增退件原因", ExampleType = ExampleType.Response)]
public class 新增退件原因_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess<string>("04", "04");
    }
}

[ExampleAnnotation(Name = "[4002]新增退件原因-資料已存在", ExampleType = ExampleType.Response)]
public class 新增退件原因資料已存在_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DataAlreadyExists(string.Empty, "01");
    }
}
