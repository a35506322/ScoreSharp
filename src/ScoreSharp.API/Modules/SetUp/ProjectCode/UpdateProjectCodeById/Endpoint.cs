using ScoreSharp.API.Modules.SetUp.ProjectCode.UpdateProjectCodeById;

namespace ScoreSharp.API.Modules.SetUp.ProjectCode
{
    public partial class ProjectCodeController
    {
        /// <summary>
        /// 修改單筆專案代號
        /// </summary>
        /// <param name="code">專案代碼</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(修改專案代號_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(修改專案代號_2000_ResEx),
            typeof(修改專案代號查無此資料_4001_ResEx),
            typeof(修改專案代號路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateProjectCodeById")]
        public async Task<IResult> UpdateProjectCodeById([FromRoute] string code, [FromBody] UpdateProjectCodeByIdRequest request)
        {
            var result = await _mediator.Send(new Command(code, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.ProjectCode.UpdateProjectCodeById
{
    public record Command(string code, UpdateProjectCodeByIdRequest updateProjectCodeByIdRequest) : IRequest<ResultResponse<string>>;

    public class Hanlder : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;

        public Hanlder(ScoreSharpContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var updateProjectCodeByIdRequest = request.updateProjectCodeByIdRequest;

            if (request.code != updateProjectCodeByIdRequest.ProjectCode)
                return ApiResponseHelper.路由與Req比對錯誤<string>(request.code);

            var entity = await _context.SetUp_ProjectCode.SingleOrDefaultAsync(x => x.ProjectCode == request.code);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(request.code, request.code.ToString());

            entity.IsActive = updateProjectCodeByIdRequest.IsActive;
            entity.ProjectName = updateProjectCodeByIdRequest.ProjectName;

            await _context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess<string>(request.code, request.code.ToString());
        }
    }
}
