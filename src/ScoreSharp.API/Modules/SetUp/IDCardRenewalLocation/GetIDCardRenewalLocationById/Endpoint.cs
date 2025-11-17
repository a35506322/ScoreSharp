using ScoreSharp.API.Modules.SetUp.IDCardRenewalLocation.GetIDCardRenewalLocationById;

namespace ScoreSharp.API.Modules.SetUp.IDCardRenewalLocation
{
    public partial class IDCardRenewalLocationController
    {
        /// <summary>
        /// 查詢單筆身分證換發地點
        /// </summary>
        /// <remarks>
        ///
        /// Sample Router:
        ///
        ///       /IDCardRenewalLocation/GetIDCardRenewalLocationById/09007000
        ///
        /// </remarks>
        /// <param name="idCardRenewalLocationCode">PK</param>
        /// <returns></returns>
        [HttpGet("{idCardRenewalLocationCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetIDCardRenewalLocationByIdResponse>))]
        [EndpointSpecificExample(
            typeof(取得身分證換發地點_2000_ResEx),
            typeof(取得身分證換發地點查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetIDCardRenewalLocationById")]
        public async Task<IResult> GetIDCardRenewalLocationById([FromRoute] string idCardRenewalLocationCode)
        {
            var result = await _mediator.Send(new Query(idCardRenewalLocationCode));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.IDCardRenewalLocation.GetIDCardRenewalLocationById
{
    public record Query(string idCardRenewalLocationCode) : IRequest<ResultResponse<GetIDCardRenewalLocationByIdResponse>>;

    public class Handler : IRequestHandler<Query, ResultResponse<GetIDCardRenewalLocationByIdResponse>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<GetIDCardRenewalLocationByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var single = await _context
                .SetUp_IDCardRenewalLocation.AsNoTracking()
                .SingleOrDefaultAsync(x => x.IDCardRenewalLocationCode == request.idCardRenewalLocationCode);

            if (single is null)
                return ApiResponseHelper.NotFound<GetIDCardRenewalLocationByIdResponse>(null, request.idCardRenewalLocationCode);

            var result = _mapper.Map<GetIDCardRenewalLocationByIdResponse>(single);

            return ApiResponseHelper.Success(result);
        }
    }
}
