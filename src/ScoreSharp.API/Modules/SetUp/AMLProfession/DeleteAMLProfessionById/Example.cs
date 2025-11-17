namespace ScoreSharp.API.Modules.SetUp.AMLProfession.DeleteAMLProfessionById;

[ExampleAnnotation(Name = "[2000]刪除AML職業別", ExampleType = ExampleType.Response)]
public class 刪除AML職業別_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("01J57D2F05Q0N06K78X0Z5XKHT");
    }
}

[ExampleAnnotation(Name = "[4001]刪除AML職業別-查無此資料", ExampleType = ExampleType.Response)]
public class 刪除AML職業別查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>("01J57D7NVXFHHD4QSZQDABTH3B", "01J57D7NVXFHHD4QSZQDABTH3B");
    }
}

[ExampleAnnotation(Name = "[4003]刪除AML職業別-資源已被使用", ExampleType = ExampleType.Response)]
public class 刪除AML職業別資源已被使用_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.此資源已被使用<string>("該版本已被使用。", "01J57D7NVXFHHD4QSZQDABTH3B");
    }
}
