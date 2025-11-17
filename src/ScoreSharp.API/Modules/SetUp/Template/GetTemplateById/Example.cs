namespace ScoreSharp.API.Modules.SetUp.Template.GetTemplateById;

[ExampleAnnotation(Name = "[4001]取得單筆樣板-查無此資料", ExampleType = ExampleType.Response)]
public class 取得單筆樣板查無此資料_4001_ResEx : IExampleProvider<ResultResponse<GetTemplateByIdResponse>>
{
    public ResultResponse<GetTemplateByIdResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetTemplateByIdResponse>(null, "顯示找不到的ID");
    }
}

[ExampleAnnotation(Name = "[2000]取得單筆樣板", ExampleType = ExampleType.Response)]
public class 取得單筆樣板_2000_ResEx : IExampleProvider<ResultResponse<GetTemplateByIdResponse>>
{
    public ResultResponse<GetTemplateByIdResponse> GetExample()
    {
        GetTemplateByIdResponse response = new GetTemplateByIdResponse
        {
            TemplateId = "A01",
            TemplateName = "補件函",
            IsActive = "Y",
            AddUserId = "ADMIN",
            AddTime = DateTime.Now,
        };

        return ApiResponseHelper.Success(response);
    }
}
