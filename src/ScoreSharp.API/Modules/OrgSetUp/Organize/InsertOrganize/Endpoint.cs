using ScoreSharp.API.Modules.OrgSetUp.Organize.InsertOrganize;

namespace ScoreSharp.API.Modules.OrgSetUp.Organize
{
    public partial class OrganizeController
    {
        ///<summary>
        /// 新增單筆組織
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <response code="200">新增成功返回 Key 值</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string?>))]
        [EndpointSpecificExample(typeof(新增組織_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增組織_2000_ResEx),
            typeof(新增組織資料已存在_4002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertOrganize")]
        public async Task<IResult> InsertOrganize([FromBody] InsertOrganizeRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.OrgSetUp.Organize.InsertOrganize
{
    public record Command(InsertOrganizeRequest insertOrganizeRequest) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, IMapper mapper) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var dto = request.insertOrganizeRequest;

            var single = await context.OrgSetUp_Organize.SingleOrDefaultAsync(x => x.OrganizeCode == dto.OrganizeCode);

            if (single != null)
                return ApiResponseHelper.DataAlreadyExists<string>(null, dto.OrganizeCode);

            var entity = mapper.Map<OrgSetUp_Organize>(dto);

            await context.AddAsync(entity);
            await context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess(entity.OrganizeCode, entity.OrganizeCode);
        }
    }
}
