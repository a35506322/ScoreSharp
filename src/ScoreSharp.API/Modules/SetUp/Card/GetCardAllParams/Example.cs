namespace ScoreSharp.API.Modules.SetUp.Card.GetCardAllParams;

[ExampleAnnotation(Name = "[2000]取得全部信用卡卡片種類參數", ExampleType = ExampleType.Response)]
public class 取得全部信用卡卡片種類參數_2000_ResEx : IExampleProvider<ResultResponse<GetCardAllParamsResponse>>
{
    public ResultResponse<GetCardAllParamsResponse> GetExample()
    {
        List<SampleRejectionLetterDto> sampleRejectionLetterOptions = new()
        {
            new SampleRejectionLetterDto { Name = "拒件函_信用卡", Value = 1 },
            new SampleRejectionLetterDto { Name = "拒件函_銷貸", Value = 2 },
            new SampleRejectionLetterDto { Name = "拒件函_代償", Value = 3 },
        };
        List<CardCategoryDto> cardCategoryOptions = new()
        {
            new CardCategoryDto { Name = "一般發卡", Value = 1 },
            new CardCategoryDto { Name = "國民現金卡", Value = 2 },
            new CardCategoryDto { Name = "銷貸", Value = 3 },
            new CardCategoryDto { Name = "現金卡代償", Value = 4 },
        };
        List<SaleLoanCategoryDto> saleLoanCategoryOptions = new()
        {
            new SaleLoanCategoryDto { Name = "代償", Value = 0 },
            new SaleLoanCategoryDto { Name = "銷貸", Value = 1 },
            new SaleLoanCategoryDto { Name = "其他", Value = 2 },
        };
        GetCardAllParamsResponse response = new(sampleRejectionLetterOptions, cardCategoryOptions, saleLoanCategoryOptions);
        return ApiResponseHelper.Success(response);
    }
}
