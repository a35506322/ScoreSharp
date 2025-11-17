using ScoreSharp.API.Modules.OrgSetUp.Organize.GetOrganizeById;

namespace ScoreSharp.API.Modules.OrgSetUp.Organize
{
    public partial class OrganizeController
    {
        ///<summary>
        /// 查詢單筆組織 By organizeCode
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /Organize/GetOrganizeById/ORG001
        ///
        /// </remarks>
        /// <param name="organizeCode">PK</param>
        /// <returns></returns>
        [HttpGet("{organizeCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetOrganizeByIdResponse>))]
        [EndpointSpecificExample(
            typeof(取得組織查無此資料_4001_ResEx),
            typeof(取得組織_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetOrganizeById")]
        public async Task<IResult> GetOrganizeById([FromRoute] string organizeCode) =>
            Results.Ok(await _mediator.Send(new Query(organizeCode)));
    }
}

namespace ScoreSharp.API.Modules.OrgSetUp.Organize.GetOrganizeById
{
    public record Query(string organizeCode) : IRequest<ResultResponse<GetOrganizeByIdResponse>>;

    public class Handler(ScoreSharpContext context, IMapper mapper) : IRequestHandler<Query, ResultResponse<GetOrganizeByIdResponse>>
    {
        public async Task<ResultResponse<GetOrganizeByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            string organizeCode = request.organizeCode;

            var single = await context.OrgSetUp_Organize.AsNoTracking().SingleOrDefaultAsync(x => x.OrganizeCode == organizeCode);

            if (single is null)
                return ApiResponseHelper.NotFound<GetOrganizeByIdResponse>(null, organizeCode);

            var response = mapper.Map<GetOrganizeByIdResponse>(single);

            return ApiResponseHelper.Success(response);
        }
    }
}
