using ScoreSharp.API.Modules.OrgSetUp.Organize.GetOrganizesByQueryString;

namespace ScoreSharp.API.Modules.OrgSetUp.Organize
{
    public partial class OrganizeController
    {
        ///<summary>
        /// 查詢多筆組織
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetOrganizesByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得組織_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetOrganizesByQueryString")]
        public async Task<IResult> GetOrganizesByQueryString([FromQuery] GetOrganizeByQueryStringRequest request) =>
            Results.Ok(await _mediator.Send(new Query(request)));
    }
}

namespace ScoreSharp.API.Modules.OrgSetUp.Organize.GetOrganizesByQueryString
{
    public record Query(GetOrganizeByQueryStringRequest getOrganizeByQueryStringRequest)
        : IRequest<ResultResponse<List<GetOrganizesByQueryStringResponse>>>;

    public class Handler(ScoreSharpContext context, IMapper mapper)
        : IRequestHandler<Query, ResultResponse<List<GetOrganizesByQueryStringResponse>>>
    {
        public async Task<ResultResponse<List<GetOrganizesByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var entities = await context.OrgSetUp_Organize.AsNoTracking().ToListAsync();

            var result = mapper.Map<List<GetOrganizesByQueryStringResponse>>(entities);

            return ApiResponseHelper.Success(result);
        }
    }
}
