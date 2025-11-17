using ScoreSharp.API.Modules.SetUp.PromotionUnit.InsertPromotionUnit;

namespace ScoreSharp.API.Modules.SetUp.PromotionUnit
{
    public partial class PromotionUnitController
    {
        ///<summary>
        /// 新增單筆推廣單位
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string?>))]
        [EndpointSpecificExample(typeof(新增推廣單位_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增推廣單位_2000_ResEx),
            typeof(新增推廣單位資料已存在_4002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertPromotionUnit")]
        public async Task<IResult> InsertPromotionUnit([FromBody] InsertPromotionUnitRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.PromotionUnit.InsertPromotionUnit
{
    public record Command(InsertPromotionUnitRequest insertPromotionUnitRequest) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IJWTProfilerHelper _jwthelper;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IJWTProfilerHelper jwthelper, IMapper mapper)
        {
            _context = context;
            _jwthelper = jwthelper;
            _mapper = mapper;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var insertPromotionUnitRequest = request.insertPromotionUnitRequest;

            var single = await _context
                .SetUp_PromotionUnit.AsNoTracking()
                .SingleOrDefaultAsync(x => x.PromotionUnitCode == insertPromotionUnitRequest.PromotionUnitCode);

            if (single != null)
                return ApiResponseHelper.DataAlreadyExists<string>(null, insertPromotionUnitRequest.PromotionUnitCode);

            var entity = _mapper.Map<SetUp_PromotionUnit>(insertPromotionUnitRequest);

            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess(
                insertPromotionUnitRequest.PromotionUnitCode,
                insertPromotionUnitRequest.PromotionUnitCode
            );
        }
    }
}
