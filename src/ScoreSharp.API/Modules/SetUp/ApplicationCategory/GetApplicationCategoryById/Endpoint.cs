using ScoreSharp.API.Modules.SetUp.ApplicationCategory.GetApplicationCategoryById;

namespace ScoreSharp.API.Modules.SetUp.ApplicationCategory
{
    public partial class ApplicationCategoryController
    {
        /// <summary>
        /// 查詢單筆申請書類別
        /// </summary>
        /// <remarks>
        ///
        /// Sample Routers:
        ///
        ///     /ApplicationCategory/GetApplicationCategoryById/AP0001
        ///
        /// </remarks>
        /// <param name="code">申請書類別代碼</param>
        /// <returns></returns>
        [HttpGet("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetApplicationCategoryByIdResponse>))]
        [EndpointSpecificExample(
            typeof(取得申請書類別_2000_ResEx),
            typeof(取得申請書類別查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetApplicationCategoryById")]
        public async Task<IResult> GetApplicationCategoryById([FromRoute] string code)
        {
            var result = await _mediator.Send(new Query(code));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.ApplicationCategory.GetApplicationCategoryById
{
    public record Query(string code) : IRequest<ResultResponse<GetApplicationCategoryByIdResponse>>;

    public class Handler : IRequestHandler<Query, ResultResponse<GetApplicationCategoryByIdResponse>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<GetApplicationCategoryByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var single = await _context
                .SetUp_ApplicationCategory.AsNoTracking()
                .SingleOrDefaultAsync(x => x.ApplicationCategoryCode == request.code);

            if (single is null)
                return ApiResponseHelper.NotFound<GetApplicationCategoryByIdResponse>(null, request.code.ToString());

            var entities = _context
                .View_ApplicationCategoryJoinCard.Where(x => x.ApplicationCategoryCode == request.code)
                .AsNoTracking()
                .ToList();

            var result = entities
                .GroupBy(x => new
                {
                    x.ApplicationCategoryCode,
                    x.ApplicationCategoryName,
                    x.IsOCRForm,
                    x.IsActive,
                    x.AddTime,
                    x.UpdateTime,
                    x.AddUserId,
                    x.UpdateUserId,
                })
                .Select(x => new GetApplicationCategoryByIdResponse
                {
                    ApplicationCategoryCode = x.Key.ApplicationCategoryCode,
                    ApplicationCategoryName = x.Key.ApplicationCategoryName,
                    IsOCRForm = x.Key.IsOCRForm,
                    IsActive = x.Key.IsActive,
                    AddTime = x.Key.AddTime,
                    UpdateTime = x.Key.UpdateTime,
                    AddUserId = x.Key.AddUserId,
                    UpdateUserId = x.Key.UpdateUserId,
                    CardInfo = x.Select(y => new CardInfoDto
                        {
                            BINCode = y.BINCode,
                            CardCode = y.CardCode,
                            CardName = y.CardName,
                        })
                        .ToList(),
                })
                .Single();

            return ApiResponseHelper.Success(result);
        }
    }
}
