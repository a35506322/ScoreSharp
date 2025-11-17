using ScoreSharp.API.Modules.SetUp.AMLJobLevel.UpdateAMLJobLevelById;

namespace ScoreSharp.API.Modules.SetUp.AMLJobLevel
{
    public partial class AMLJobLevelController
    {
        /// <summary>
        /// 單筆修改AML職級別
        /// </summary>
        /// <param name="code">PK</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(修改AML職級別_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(修改AML職級別_2000_ResEx),
            typeof(修改AML職級別查無此資料_4001_ResEx),
            typeof(修改AML職級別路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateAMLJobLevelById")]
        public async Task<IResult> UpdateAMLJobLevelById([FromRoute] string code, [FromBody] UpdateAMLJobLevelByIdRequest request)
        {
            var result = await _mediator.Send(new Command(code, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.AMLJobLevel.UpdateAMLJobLevelById
{
    public record Command(string code, UpdateAMLJobLevelByIdRequest updateAMLJobLevelByIdRequest) : IRequest<ResultResponse<string>>;

    public class Hanlder : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;

        public Hanlder(ScoreSharpContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var updateAMLJobLevelByIdRequest = request.updateAMLJobLevelByIdRequest;

            if (request.code != updateAMLJobLevelByIdRequest.AMLJobLevelCode)
                return ApiResponseHelper.路由與Req比對錯誤<string>(request.code);

            var entity = await _context.SetUp_AMLJobLevel.SingleOrDefaultAsync(x => x.AMLJobLevelCode == request.code);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(request.code, request.code);

            entity.IsActive = updateAMLJobLevelByIdRequest.IsActive;
            entity.IsSeniorManagers = updateAMLJobLevelByIdRequest.IsSeniorManagers;
            entity.AMLJobLevelName = updateAMLJobLevelByIdRequest.AMLJobLevelName;

            await _context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess(request.code, request.code);
        }
    }
}
