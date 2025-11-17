using ScoreSharp.API.Modules.SetUp.AnnualFeeCollectionMethod.InsertAnnualFeeCollectionMethod;

namespace ScoreSharp.API.Modules.SetUp.AnnualFeeCollectionMethod
{
    public partial class AnnualFeeCollectionMethodController
    {
        /// <summary>
        /// 新增單筆年費收取方式
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(新增年費收取方式_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增年費收取方式_2000_ResEx),
            typeof(新增年費收取方式資料已存在_4002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertAnnualFeeCollectionMethod")]
        public async Task<IResult> InsertAnnualFeeCollectionMethod([FromBody] InsertAnnualFeeCollectionMethodRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.AnnualFeeCollectionMethod.InsertAnnualFeeCollectionMethod
{
    public record class Command(InsertAnnualFeeCollectionMethodRequest insertAnnualFeeCollectionMethodRequest) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext _context, IMapper _mapper, ILogger<Handler> logger) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var dto = request.insertAnnualFeeCollectionMethodRequest;

            var single = await _context
                .SetUp_AnnualFeeCollectionMethod.AsNoTracking()
                .SingleOrDefaultAsync(x => x.AnnualFeeCollectionCode == dto.AnnualFeeCollectionCode);

            if (single is not null)
                return ApiResponseHelper.DataAlreadyExists<string>(dto.AnnualFeeCollectionCode, dto.AnnualFeeCollectionCode);

            var entity = _mapper.Map<SetUp_AnnualFeeCollectionMethod>(dto);

            await _context.SetUp_AnnualFeeCollectionMethod.AddAsync(entity);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess(entity.AnnualFeeCollectionCode, entity.AnnualFeeCollectionCode);
        }
    }
}
