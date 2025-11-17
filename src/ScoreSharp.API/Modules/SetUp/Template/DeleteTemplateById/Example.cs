namespace ScoreSharp.API.Modules.SetUp.Template.DeleteTemplateById;

[ExampleAnnotation(Name = "[4001]刪除單筆樣板-查無此資料", ExampleType = ExampleType.Response)]
public class 刪除單筆樣板查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "刪除的ID");
    }
}

[ExampleAnnotation(Name = "[2000]刪除單筆樣板", ExampleType = ExampleType.Response)]
public class 刪除單筆樣板_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("刪除的ID");
    }
}

[ExampleAnnotation(Name = "[4003]刪除單筆樣板-此資源已被使用", ExampleType = ExampleType.Response)]
public class 刪除單筆樣板由此資源已被使用_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.此資源已被使用<string>(null, "SetUpBillDay");
    }
}
