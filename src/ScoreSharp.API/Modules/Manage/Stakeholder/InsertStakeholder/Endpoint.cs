using ScoreSharp.API.Modules.Manage.Stakeholder.InsertStakeholder;

namespace ScoreSharp.API.Modules.Manage.Stakeholder
{
    public partial class StakeholderController
    {
        /// <summary>
        /// 新增單筆利害關係人
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <response code="200">新增成功返回 Key 值</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(新增利害關係人_2000_ReqEx), ExampleType = ExampleType.Request, ResponseStatusCode = StatusCodes.Status200OK)]
        [EndpointSpecificExample(
            typeof(新增利害關係人_2000_ResEx),
            typeof(新增利害關係人資料已存在_4002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertStakeholder")]
        public async Task<IResult> InsertStakeholder([FromBody] InsertStakeholderRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Manage.Stakeholder.InsertStakeholder
{
    public record Command(InsertStakeholderRequest insertStakeholderRequest) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, IMapper _mapper, ILogger<Handler> logger) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var dto = request.insertStakeholderRequest;

            var single = await context.Reviewer_Stakeholder.SingleOrDefaultAsync(x => x.ID == dto.ID && x.UserId == dto.UserId);

            if (single != null)
                return ApiResponseHelper.DataAlreadyExists<string>(null, $"{dto.UserId}已存在ID：{dto.ID}之利害關係人資料。");

            var entity = _mapper.Map<Reviewer_Stakeholder>(dto);

            await context.AddAsync(entity);
            await context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess(entity.SeqNo.ToString(), entity.SeqNo.ToString());
        }
    }
}
