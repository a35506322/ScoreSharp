namespace ScoreSharp.API.Modules.SetUp.TemplateFixContent.GetTemplateFixContentsByQueryString;

[ExampleAnnotation(Name = "[2000]查詢樣板固定值", ExampleType = ExampleType.Response)]
public class 查詢樣板固定值_2000_ResEx : IExampleProvider<ResultResponse<List<GetTemplateFixContentsByQueryStringResponse>>>
{
    public ResultResponse<List<GetTemplateFixContentsByQueryStringResponse>> GetExample()
    {
        // Example data
        var data = new List<GetTemplateFixContentsByQueryStringResponse>
        {
            new GetTemplateFixContentsByQueryStringResponse
            {
                SeqNo = 7,
                TemplateKey = "phone",
                TemplateValue = "02-24567891",
                TemplateId = "E30",
                TemplateName = "Test通知函_範本123",
                IsActive = "Y",
                AddUserId = "admin",
                AddTime = DateTime.Now,
            },
            new GetTemplateFixContentsByQueryStringResponse
            {
                SeqNo = 8,
                TemplateKey = "title",
                TemplateValue = "聯邦商業銀行 信用卡暨支付金融處 敬上",
                TemplateId = "E30",
                TemplateName = "Test通知函_範本123",
                IsActive = "N",
                AddUserId = "admin",
                AddTime = DateTime.Now,
            },
        };

        return ApiResponseHelper.Success(data);
    }
}
