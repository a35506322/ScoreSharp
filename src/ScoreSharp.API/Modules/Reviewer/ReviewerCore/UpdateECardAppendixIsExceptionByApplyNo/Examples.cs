namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateECardAppendixIsExceptionByApplyNo;

[ExampleAnnotation(Name = "[4001]修改ECard附件異常註記-查無此申請書編號", ExampleType = ExampleType.Response)]
public class 修改ECard附件異常註記查無此申請書編號_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.NotFound<string>(null, "20240803B0001");
}

[ExampleAnnotation(Name = "[2000]修改ECard附件異常註記-成功", ExampleType = ExampleType.Response)]
public class 修改ECard附件異常註記成功_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.UpdateByIdSuccess<string>("20240803B2222", "20240803B2222");
}
