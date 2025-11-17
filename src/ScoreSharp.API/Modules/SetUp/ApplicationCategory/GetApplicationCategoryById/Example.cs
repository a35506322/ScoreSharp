namespace ScoreSharp.API.Modules.SetUp.ApplicationCategory.GetApplicationCategoryById;

[ExampleAnnotation(Name = "[2000]取得申請書類別", ExampleType = ExampleType.Response)]
public class 取得申請書類別_2000_ResEx : IExampleProvider<ResultResponse<GetApplicationCategoryByIdResponse>>
{
    public ResultResponse<GetApplicationCategoryByIdResponse> GetExample()
    {
        GetApplicationCategoryByIdResponse response = new()
        {
            ApplicationCategoryCode = "AP0001",
            ApplicationCategoryName = "北部綜合版Ａ",
            IsOCRForm = "N",
            IsActive = "Y",
            AddTime = DateTime.Now,
            AddUserId = "ADMIN",
            UpdateTime = DateTime.Now,
            UpdateUserId = "ADMIN",
            CardInfo = new List<CardInfoDto>()
            {
                new CardInfoDto()
                {
                    BINCode = "35605602",
                    CardCode = "JC02",
                    CardName = "大統JCB晶緻卡C",
                },
                new CardInfoDto()
                {
                    BINCode = "35605624",
                    CardCode = "JC24",
                    CardName = "微風JCB晶緻卡C",
                },
            },
        };
        return ApiResponseHelper.Success(response);
    }
}

[ExampleAnnotation(Name = "[4001]取得申請書類別-查無此資料", ExampleType = ExampleType.Response)]
public class 取得申請書類別查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "AP0005");
    }
}
