namespace ScoreSharp.API.Modules.SetUp.TemplateFixContent.DeleteTemplateFixContentById;

[ExampleAnnotation(Name = "[4001]刪除單筆樣板固定值-查無此資料", ExampleType = ExampleType.Response)]
public class 刪除單筆樣板固定值查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "刪除的ID");
    }
}

[ExampleAnnotation(Name = "[2000]刪除單筆樣板固定值", ExampleType = ExampleType.Response)]
public class 刪除單筆樣板固定值_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("刪除的ID");
    }
}
