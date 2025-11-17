namespace ScoreSharp.API.Modules.SetUp.Card.DeleteCardById;

[ExampleAnnotation(Name = "[2000]刪除信用卡卡片種類", ExampleType = ExampleType.Response)]
public class 刪除信用卡卡片種類_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("XYZ456");
    }
}

[ExampleAnnotation(Name = "[4001]刪除信用卡卡片種類-查無此資料", ExampleType = ExampleType.Response)]
public class 刪除信用卡卡片種類查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "XYZ443");
    }
}
