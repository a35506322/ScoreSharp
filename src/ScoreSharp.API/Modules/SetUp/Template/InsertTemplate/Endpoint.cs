using ScoreSharp.API.Modules.SetUp.Template.InsertTemplate;

namespace ScoreSharp.API.Modules.SetUp.Template
{
    public partial class TemplateController
    {
        ///<summary>
        /// 新增單筆樣板
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string?>))]
        [EndpointSpecificExample(typeof(新增樣板_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增樣板_2000_ResEx),
            typeof(新增樣板資料已存在_4002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertTemplate")]
        public async Task<IResult> InsertTemplate([FromBody] InsertTemplateRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.Template.InsertTemplate
{
    public record Command(InsertTemplateRequest insertTemplateRequest) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, IMapper mapper) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var dto = request.insertTemplateRequest;

            var single = await context.SetUp_Template.AsNoTracking().SingleOrDefaultAsync(x => x.TemplateId == dto.TemplateId);

            if (single != null)
                return ApiResponseHelper.DataAlreadyExists<string>(null, dto.TemplateId);

            var entity = mapper.Map<SetUp_Template>(dto);

            await context.AddAsync(entity);
            await context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess(dto.TemplateId, dto.TemplateId);
        }
    }
}
