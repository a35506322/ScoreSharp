using ScoreSharp.API.Modules.SetUp.IDCardRenewalLocation.UpdateIDCardRenewalLocationById;

namespace ScoreSharp.API.Modules.SetUp.IDCardRenewalLocation
{
    public partial class IDCardRenewalLocationController
    {
        /// <summary>
        /// 單筆修改身分證換發地點
        /// </summary>
        /// <param name="idCardRenewalLocationCode">PK</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{idCardRenewalLocationCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(修改身分證換發地點_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(修改身分證換發地點_2000_ResEx),
            typeof(修改身分證換發地點查無資料_4001_ResEx),
            typeof(修改身分證換發地點路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateIDCardRenewalLocationById")]
        public async Task<IResult> UpdateIDCardRenewalLocationById(
            [FromRoute] string idCardRenewalLocationCode,
            [FromBody] UpdateIDCardRenewalLocationByIdRequest request
        )
        {
            var result = await _mediator.Send(new Command(idCardRenewalLocationCode, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.IDCardRenewalLocation.UpdateIDCardRenewalLocationById
{
    public record Command(string idCardRenewalLocationCode, UpdateIDCardRenewalLocationByIdRequest updateIDCardRenewalLocationByIdRequest)
        : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;

        public Handler(ScoreSharpContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var updateIDCardRenewalLocationByIdRequest = request.updateIDCardRenewalLocationByIdRequest;

            if (request.idCardRenewalLocationCode != updateIDCardRenewalLocationByIdRequest.IDCardRenewalLocationCode)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var entity = await _context.SetUp_IDCardRenewalLocation.SingleOrDefaultAsync(x =>
                x.IDCardRenewalLocationCode == request.idCardRenewalLocationCode
            );

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, request.idCardRenewalLocationCode);

            entity.IsActive = updateIDCardRenewalLocationByIdRequest.IsActive;
            entity.IDCardRenewalLocationName = updateIDCardRenewalLocationByIdRequest.IDCardRenewalLocationName;

            await _context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess<string>(request.idCardRenewalLocationCode, request.idCardRenewalLocationCode);
        }
    }
}
