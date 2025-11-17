namespace ScoreSharp.API.Modules.SetUp.Template.GetTemplatesByQueryString;

[ExampleAnnotation(Name = "[2000]取得多筆樣板", ExampleType = ExampleType.Response)]
public class 取得多筆樣板_2000_ResEx : IExampleProvider<ResultResponse<List<GetTemplatesByQueryStringResponse>>>
{
    public ResultResponse<List<GetTemplatesByQueryStringResponse>> GetExample()
    {
        // Example data
        var data = new List<GetTemplatesByQueryStringResponse>
        {
            new GetTemplatesByQueryStringResponse
            {
                TemplateId = "A01",
                TemplateName = "補件函",
                IsActive = "Y",
                AddUserId = "admin",
                AddTime = DateTime.Now,
            },
        };

        return ApiResponseHelper.Success(data);
    }
}
