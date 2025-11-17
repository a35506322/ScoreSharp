namespace ScoreSharp.API.Modules.SetUp.TemplateFixContent.GetTemplateFixContentById;

[ExampleAnnotation(Name = "[4001]取得單筆樣板固定值-查無此資料", ExampleType = ExampleType.Response)]
public class 取得單筆樣板固定值查無此資料_4001_ResEx : IExampleProvider<ResultResponse<GetTemplateFixContentByIdResponse>>
{
    public ResultResponse<GetTemplateFixContentByIdResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetTemplateFixContentByIdResponse>(null, "顯示找不到的ID");
    }
}

[ExampleAnnotation(Name = "[2000]取得單筆樣板固定值", ExampleType = ExampleType.Response)]
public class 取得單筆樣板固定值_2000_ResEx : IExampleProvider<ResultResponse<GetTemplateFixContentByIdResponse>>
{
    public ResultResponse<GetTemplateFixContentByIdResponse> GetExample()
    {
        GetTemplateFixContentByIdResponse response = new GetTemplateFixContentByIdResponse
        {
            SeqNo = 2,
            TemplateKey = "FaxNumber",
            TemplateValue = "02-87526169　07-3302783",
            TemplateId = "A01",
            TemplateName = "補件函",
            IsActive = "N",
            AddUserId = "ADMIN",
            AddTime = DateTime.Now,
        };

        return ApiResponseHelper.Success(response);
    }
}
