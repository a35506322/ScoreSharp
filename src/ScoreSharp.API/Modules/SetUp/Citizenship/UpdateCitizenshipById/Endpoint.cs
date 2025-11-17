using ScoreSharp.API.Modules.SetUp.Citizenship.UpdateCitizenshipById;

namespace ScoreSharp.API.Modules.SetUp.Citizenship
{
    public partial class CitizenshipController
    {
        /// <summary>
        /// 單筆修改國籍
        /// </summary>
        /// <param name="code">國籍代碼
        /// </param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [EndpointSpecificExample(typeof(修改國籍_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(修改國籍_2000_ResEx),
            typeof(修改國籍查無資料_4001_ResEx),
            typeof(修改國籍路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateCitizenshipById")]
        public async Task<IResult> UpdateCitizenshipById([FromRoute] string code, [FromBody] UpdateCitizenshipByIdRequest request)
        {
            var result = await _mediator.Send(new Command(code, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.Citizenship.UpdateCitizenshipById
{
    public record Command(string code, UpdateCitizenshipByIdRequest updateCitizenshipByIdRequest) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;

        public Handler(ScoreSharpContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var updateCitizenshipByIdRequest = request.updateCitizenshipByIdRequest;

            if (updateCitizenshipByIdRequest.CitizenshipCode != request.code)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var entity = await _context.SetUp_Citizenship.SingleOrDefaultAsync(x => x.CitizenshipCode == request.code);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, request.code);

            entity.CitizenshipName = updateCitizenshipByIdRequest.CitizenshipName;
            entity.CitizenshipCode = updateCitizenshipByIdRequest.CitizenshipCode;
            entity.IsActive = updateCitizenshipByIdRequest.IsActive;

            await _context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess<string>(request.code, request.code);
        }
    }
}
