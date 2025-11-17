namespace ScoreSharp.API.Modules.SetUp.Card.GetCardAllParams;

public class GetCardAllParamsResponse(
    List<SampleRejectionLetterDto> sampleRejectionLetterOptions,
    List<CardCategoryDto> cardCategoryOptions,
    List<SaleLoanCategoryDto> saleLoanCategoryOptions
)
{
    public List<SampleRejectionLetterDto> SampleRejectionLetterOptions { get; } = sampleRejectionLetterOptions;
    public List<CardCategoryDto> CardCategoryOptions { get; } = cardCategoryOptions;
    public List<SaleLoanCategoryDto> SaleLoanCategoryOptions { get; } = saleLoanCategoryOptions;
}

public class CardCategoryDto
{
    public string Name { get; set; }
    public int Value { get; set; }
}

public class SampleRejectionLetterDto
{
    public string Name { get; set; }
    public int Value { get; set; }
}

public class SaleLoanCategoryDto
{
    public string Name { get; set; }
    public int Value { get; set; }
}
