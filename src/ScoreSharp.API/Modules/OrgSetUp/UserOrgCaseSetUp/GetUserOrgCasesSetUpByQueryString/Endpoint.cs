using ScoreSharp.API.Modules.OrgSetUp.UserOrgCaseSetUp.GetUserOrgCasesSetUpByQueryString;

namespace ScoreSharp.API.Modules.OrgSetUp.UserOrgCaseSetUp
{
    public partial class UserOrgCaseSetUpController
    {
        ///<summary>
        /// 查詢多筆人員組織分案群組設定
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetUserOrgCasesSetUpByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(查詢多筆人員組織分案群組設定_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetUserOrgCasesSetUpByQueryString")]
        public async Task<IResult> GetUserOrgCasesSetUpByQueryString([FromQuery] GetUserOrgCasesSetUpByQueryStringRequest request) =>
            Results.Ok(await _mediator.Send(new Query(request)));
    }
}

namespace ScoreSharp.API.Modules.OrgSetUp.UserOrgCaseSetUp.GetUserOrgCasesSetUpByQueryString
{
    public record Query(GetUserOrgCasesSetUpByQueryStringRequest GetUserOrgCasesSetUpByQueryStringRequest)
        : IRequest<ResultResponse<List<GetUserOrgCasesSetUpByQueryStringResponse>>>;

    public class Handler(ScoreSharpContext context, IMapper mapper, IJWTProfilerHelper jWTProfilerHelper)
        : IRequestHandler<Query, ResultResponse<List<GetUserOrgCasesSetUpByQueryStringResponse>>>
    {
        public async Task<ResultResponse<List<GetUserOrgCasesSetUpByQueryStringResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            string 派案群組 = string.Join(",", jWTProfilerHelper.CaseDispatchGroups?.Cast<int>());
            var dto = await context
                .Database.SqlQueryRaw<GetBeReviewerUsersDto>("EXEC Usp_GetBeReviewerUsers @Groups", new SqlParameter("@Groups", 派案群組))
                .ToListAsync();

            var userIds = dto.Select(x => x.UserId).ToHashSet();
            var entities = await context.OrgSetUp_UserOrgCaseSetUp.AsNoTracking().Where(x => userIds.Contains(x.UserId)).ToListAsync();

            var userDic = await context
                .OrgSetUp_User.AsNoTracking()
                .ToDictionaryAsync(x => x.UserId, x => new { Name = x.UserName, CaseDispatchGroup = x.CaseDispatchGroup });

            var result = mapper
                .Map<List<GetUserOrgCasesSetUpByQueryStringResponse>>(entities)
                .Select(x =>
                {
                    x.UserName = userDic[x.UserId].Name;
                    x.DesignatedSupervisor1Name = string.IsNullOrEmpty(x.DesignatedSupervisor1)
                        ? string.Empty
                        : userDic[x.DesignatedSupervisor1].Name;
                    x.DesignatedSupervisor2Name = string.IsNullOrEmpty(x.DesignatedSupervisor2)
                        ? string.Empty
                        : userDic[x.DesignatedSupervisor2].Name;
                    x.CaseDispatchGroups = string.IsNullOrWhiteSpace(userDic[x.UserId].CaseDispatchGroup)
                        ? []
                        : userDic[x.UserId]
                            .CaseDispatchGroup.Split(",")
                            .Select(y => new CaseDispatchGroupModel { CaseDispatchGroup = Enum.Parse<CaseDispatchGroup>(y) })
                            .ToList();
                    return x;
                })
                .ToList();

            return ApiResponseHelper.Success(result);
        }
    }
}
