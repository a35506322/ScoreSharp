using ScoreSharp.API.Modules.SetUp.ApplicationCategory.InsertApplicationCategory;

namespace ScoreSharp.API.Modules.SetUp.ApplicationCategory
{
    public partial class ApplicationCategoryController
    {
        /// <summary>
        /// 新增單筆申請書類別
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(新增申請書類別_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增申請書類別_2000_ResEx),
            typeof(新增申請書類別資料已存在_4002_ResEx),
            typeof(新增申請書類別查無信用卡卡片種類資料_4003_ResEx),
            typeof(新增申請書類別查出未啟用之信用卡卡片種類_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertApplicationCategory")]
        public async Task<IResult> InsertApplicationCategory([FromBody] InsertApplicationCategoryRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.ApplicationCategory.InsertApplicationCategory
{
    public record Command(InsertApplicationCategoryRequest insertApplicationCategoryRequest) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var insertApplicationCategoryRequest = request.insertApplicationCategoryRequest;

            var single = await _context
                .SetUp_ApplicationCategory.AsNoTracking()
                .SingleOrDefaultAsync(x =>
                    x.ApplicationCategoryCode == insertApplicationCategoryRequest.ApplicationCategoryCode
                    || x.ApplicationCategoryCode == insertApplicationCategoryRequest.ApplicationCategoryCode
                );

            if (single != null)
                return ApiResponseHelper.DataAlreadyExists<string>(null, insertApplicationCategoryRequest.ApplicationCategoryCode);

            var allCard = _context.SetUp_Card.AsNoTracking().ToList();

            var isCard = allCard.Select(x => x.BINCode);

            if (!insertApplicationCategoryRequest.BINCodes.All(cardInfo => isCard.Contains(cardInfo)))
            {
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "查無信用卡卡片種類資料，請檢查");
            }

            var inactiveBINCodes = allCard.Where(x => x.IsActive == "N").Select(x => x.BINCode);

            var isNotActiveCard = insertApplicationCategoryRequest.BINCodes.Where(cardInfo => inactiveBINCodes.Contains(cardInfo));

            if (isNotActiveCard.Any())
            {
                var notActiveBINCodes = allCard
                    .Where(x => isNotActiveCard.Contains(x.BINCode))
                    .Select(x => $"（{x.CardCode}）{x.CardName}")
                    .ToList();
                string notActiveBINCode = string.Join("、", notActiveBINCodes);
                return ApiResponseHelper.BusinessLogicFailed<string>(null, $"以下為未啟用之信用卡卡片種類，請檢查 :　{notActiveBINCode}");
            }

            var entities = insertApplicationCategoryRequest
                .BINCodes.Select(cardInfo => new SetUp_ApplicationCategory_Card
                {
                    BINCode = cardInfo,
                    ApplicationCategoryCode = insertApplicationCategoryRequest.ApplicationCategoryCode,
                })
                .ToList();

            var entity = _mapper.Map<SetUp_ApplicationCategory>(insertApplicationCategoryRequest);

            await _context.SetUp_ApplicationCategory.AddAsync(entity);
            await _context.AddRangeAsync(entities);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess<string>(
                insertApplicationCategoryRequest.ApplicationCategoryCode,
                insertApplicationCategoryRequest.ApplicationCategoryCode
            );
        }
    }
}
