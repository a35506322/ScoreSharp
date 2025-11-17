namespace ScoreSharp.API.Modules.SetUp.ApplicationCategory.InsertApplicationCategory;

[ExampleAnnotation(Name = "[2000]新增申請書類別", ExampleType = ExampleType.Request)]
public class 新增申請書類別_2000_ReqEx : IExampleProvider<InsertApplicationCategoryRequest>
{
    public InsertApplicationCategoryRequest GetExample()
    {
        InsertApplicationCategoryRequest request = new()
        {
            ApplicationCategoryCode = "AP0002",
            ApplicationCategoryName = "北部綜合版Ａ+代償信用卡",
            IsActive = "Y",
            IsOCRForm = "N",
            BINCodes = new List<string>() { "35605602", "35605624", "51570900", "35665659" },
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]新增申請書類別", ExampleType = ExampleType.Response)]
public class 新增申請書類別_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess("AP0002", "AP0002");
    }
}

[ExampleAnnotation(Name = "[4002]新增申請書類別-資料已存在", ExampleType = ExampleType.Response)]
public class 新增申請書類別資料已存在_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DataAlreadyExists<string>(null, "AP0001");
    }
}

[ExampleAnnotation(Name = "[4003]新增申請書類別-查無信用卡卡片種類資料", ExampleType = ExampleType.Response)]
public class 新增申請書類別查無信用卡卡片種類資料_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>(null, "查無信用卡卡片種類資料，請檢查");
    }
}

[ExampleAnnotation(Name = "[4003]新增申請書類別-查出未啟用之信用卡卡片種類", ExampleType = ExampleType.Response)]
public class 新增申請書類別查出未啟用之信用卡卡片種類_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>(
            null,
            "以下為未啟用之信用卡卡片種類，請檢查 :　（MO）基隆市Awesome萬 Inc、（UITC99）UITC測試卡片、（ZA）金門市Incredible薛 - 葉"
        );
    }
}
