namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateCommunicationNotesByApplyNo;

[ExampleAnnotation(Name = "[2000]修改溝通備註-成功", ExampleType = ExampleType.Request)]
public class 修改溝通備註成功_2000_ReqEx : IExampleProvider<UpdateCommunicationNotesByApplyNoRequest>
{
    public UpdateCommunicationNotesByApplyNoRequest GetExample() => new() { ApplyNo = "20250321G7943", CommunicationNotes = "備註" };
}

[ExampleAnnotation(Name = "[4001]修改溝通備註-查無此申請書編號", ExampleType = ExampleType.Response)]
public class 修改溝通備註查無此申請書編號_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.NotFound<string>(null, "20250101G3234");
}

[ExampleAnnotation(Name = "[4003]修改溝通備註-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改溝通備註路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.路由與Req比對錯誤<string>("2");
}

[ExampleAnnotation(Name = "[2000]修改溝通備註-成功", ExampleType = ExampleType.Response)]
public class 修改溝通備註成功_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.UpdateByIdSuccess<string>("20250321G7943", "20250321G7943");
}
