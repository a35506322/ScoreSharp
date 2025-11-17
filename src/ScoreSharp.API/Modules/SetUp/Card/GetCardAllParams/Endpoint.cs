using ScoreSharp.API.Modules.SetUp.Card.GetCardAllParams;
using ScoreSharp.Common.Enums;
using ScoreSharp.Common.Extenstions._Enum;

namespace ScoreSharp.API.Modules.SetUp.Card
{
    public partial class CardController
    {
        /// <summary>
        /// 查詢全部信用卡卡片種類參數
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetCardAllParamsResponse>))]
        [EndpointSpecificExample(
            typeof(取得全部信用卡卡片種類參數_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetCardAllParams")]
        public async Task<IResult> GetCardAllParams()
        {
            var result = await _mediator.Send(new Query());
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.Card.GetCardAllParams
{
    public record Query() : IRequest<ResultResponse<GetCardAllParamsResponse>>;

    public class Handler : IRequestHandler<Query, ResultResponse<GetCardAllParamsResponse>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<GetCardAllParamsResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var sampleRejectionLetterList = EnumExtenstions.GetEnumInfo<SampleRejectionLetter>();
            var cardCategoryList = EnumExtenstions.GetEnumInfo<CardCategory>();
            var saleLoanCategoryList = EnumExtenstions.GetEnumInfo<SaleLoanCategory>();

            var sampleRejectionLetterDto = sampleRejectionLetterList
                .Where(x => x.IsActiveAttr == true)
                .Select(x => new SampleRejectionLetterDto { Name = x.Name, Value = x.Value })
                .ToList();

            var cardCategoryDto = cardCategoryList
                .Where(x => x.IsActiveAttr == true)
                .Select(x => new CardCategoryDto { Name = x.Name, Value = x.Value })
                .ToList();

            var saleLoanCategoryDto = saleLoanCategoryList
                .Where(x => x.IsActiveAttr == true)
                .Select(x => new SaleLoanCategoryDto { Name = x.Name, Value = x.Value })
                .ToList();
            ;

            GetCardAllParamsResponse getCardAllParams = new GetCardAllParamsResponse(
                sampleRejectionLetterDto,
                cardCategoryDto,
                saleLoanCategoryDto
            );

            return ApiResponseHelper.Success(getCardAllParams);
        }
    }
}
