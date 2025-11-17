namespace ScoreSharp.API.Modules.SetUp.ApplicationCategory.UpdateApplicationCategoryById;

[ExampleAnnotation(Name = "[2000]修改申請書類別", ExampleType = ExampleType.Request)]
public class 修改申請書類別_2000_ReqEx : IExampleProvider<UpdateApplicationCategoryByIdRequest>
{
    public UpdateApplicationCategoryByIdRequest GetExample()
    {
        UpdateApplicationCategoryByIdRequest request = new()
        {
            ApplicationCategoryCode = "AP0002",
            ApplicationCategoryName = "北部綜合版Ａ+代償信用卡",
            IsActive = "N",
            IsOCRForm = "N",
            BINCodes = new List<string>() { "35605602", "35605624", "51570900", "35665659" },
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]修改申請書類別", ExampleType = ExampleType.Response)]
public class 修改申請書類別_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess<string>("0003", "0003");
    }
}

[ExampleAnnotation(Name = "[4001]修改申請書類別-查無資料", ExampleType = ExampleType.Response)]
public class 修改申請書類別查無資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "0043");
    }
}

[ExampleAnnotation(Name = "[4003]修改申請書類別-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改申請書類別路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}

[ExampleAnnotation(Name = "[4003]修改申請書類別-查無信用卡卡片種類資料", ExampleType = ExampleType.Response)]
public class 修改申請書類別查無信用卡卡片種類資料_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>(null, "查無信用卡卡片種類資料，請檢查");
    }
}

[ExampleAnnotation(Name = "[4003]修改申請書類別-查出未啟用之信用卡卡片種類", ExampleType = ExampleType.Response)]
public class 修改申請書類別查出未啟用之信用卡卡片種類_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>(
            null,
            "以下為未啟用之信用卡卡片種類，請檢查 :　（MO）基隆市Awesome萬 Inc、（UITC99）UITC測試卡片、（ZA）金門市Incredible薛 - 葉"
        );
    }
}
