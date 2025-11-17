namespace ScoreSharp.API.Modules.SetUp.ApplicationCategory.GetApplicationCategoriesByQueryString;

[ExampleAnnotation(Name = "[2000]取得申請書類別", ExampleType = ExampleType.Response)]
public class 取得申請書類別_2000_ResEx : IExampleProvider<ResultResponse<List<GetApplicationCategoriesByQueryStringResponse>>>
{
    public ResultResponse<List<GetApplicationCategoriesByQueryStringResponse>> GetExample()
    {
        List<GetApplicationCategoriesByQueryStringResponse> response = new()
        {
            new GetApplicationCategoriesByQueryStringResponse
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
            },
        };

        return ApiResponseHelper.Success(response);
    }
}
