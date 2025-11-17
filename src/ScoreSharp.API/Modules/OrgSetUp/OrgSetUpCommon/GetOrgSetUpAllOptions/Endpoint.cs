using ScoreSharp.API.Modules.OrgSetUp.OrgSetUpCommon.GetOrgSetUpAllOptions;

namespace ScoreSharp.API.Modules.OrgSetUp.OrgSetUpCommon
{
    public partial class OrgSetUpCommonController
    {
        /// <summary>
        /// 取得組織設定相關所有的下拉選單
        /// </summary>
        /// <remarks>
        ///
        ///     Sample Router: /OrgSetUpCommon/GetOrgSetUpAllOptions?IsActive=Y
        ///
        ///     對照表
        ///     派案群組 = CaseDispatchGroup
        /// </remarks>
        ///<returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetOrgSetUpAllOptionsResponse>))]
        [EndpointSpecificExample(
            typeof(取得全部組織設定相關下拉選單_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetOrgSetUpAllOptions")]
        public async Task<IResult> GetOrgSetUpAllOptions([FromQuery] string? isActive)
        {
            var result = await _mediator.Send(new Query(isActive));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.OrgSetUp.OrgSetUpCommon.GetOrgSetUpAllOptions
{
    public record Query(string? isActive) : IRequest<ResultResponse<GetOrgSetUpAllOptionsResponse>>;

    public class Handler() : IRequestHandler<Query, ResultResponse<GetOrgSetUpAllOptionsResponse>>
    {
        public async Task<ResultResponse<GetOrgSetUpAllOptionsResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var caseDispatchGroupOptions = EnumExtenstions.GetEnumOptions<CaseDispatchGroup>(request.isActive);

            var response = new GetOrgSetUpAllOptionsResponse { CaseDispatchGroup = caseDispatchGroupOptions };

            return ApiResponseHelper.Success(response);
        }
    }
}
