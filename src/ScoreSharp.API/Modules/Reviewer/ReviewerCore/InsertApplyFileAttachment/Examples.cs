namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.InsertApplyFileAttachment;

[ExampleAnnotation(Name = "[2000]新增審行員附件", ExampleType = ExampleType.Response)]
public class 新增審行員附件_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess("1", "1");
    }
}

[ExampleAnnotation(Name = "[4003]新增審行員附件-檔案格式有誤", ExampleType = ExampleType.Response)]
public class 新增審行員附件檔案格式有誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>(null, "檔案格式不符合要求，請上傳符合 JPG 或 PNG 格式的檔案");
    }
}

[ExampleAnnotation(Name = "[4003]新增審行員附件-檔名重複", ExampleType = ExampleType.Response)]
public class 新增審行員附件_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>(null, "檔名重複，請檢查");
    }
}

[ExampleAnnotation(Name = "[4001]新增審行員附件-查無申請書編號", ExampleType = ExampleType.Response)]
public class 新增審行員附件查無申請書編號_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "20241203X1105");
    }
}
