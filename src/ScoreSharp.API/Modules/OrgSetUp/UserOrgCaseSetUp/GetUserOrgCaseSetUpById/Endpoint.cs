using ScoreSharp.API.Modules.OrgSetUp.UserOrgCaseSetUp.GetUserOrgCaseSetUpById;

namespace ScoreSharp.API.Modules.OrgSetUp.UserOrgCaseSetUp
{
    public partial class UserOrgCaseSetUpController
    {
        ///<summary>
        /// 查詢單筆人員組織分案群組設定 By UserId
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /UserOrgCaseSetUp/GetUserOrgCaseSetUpById/raya00
        ///
        /// </remarks>
        /// <param name="userId">PK</param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetUserOrgCaseSetUpByIdResponse>))]
        [EndpointSpecificExample(
            typeof(查詢單筆人員組織分案群組設定查無此資料_4001_ResEx),
            typeof(查詢單筆人員組織分案群組設定_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetUserOrgCaseSetUpById")]
        public async Task<IResult> GetUserOrgCaseSetUpById([FromRoute] string userId) => Results.Ok(await _mediator.Send(new Query(userId)));
    }
}

namespace ScoreSharp.API.Modules.OrgSetUp.UserOrgCaseSetUp.GetUserOrgCaseSetUpById
{
    public record Query(string userId) : IRequest<ResultResponse<GetUserOrgCaseSetUpByIdResponse>>;

    public class Handler(ScoreSharpContext context, IMapper mapper) : IRequestHandler<Query, ResultResponse<GetUserOrgCaseSetUpByIdResponse>>
    {
        public async Task<ResultResponse<GetUserOrgCaseSetUpByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var userId = request.userId;

            var users = await context.OrgSetUp_User.AsNoTracking().ToListAsync();
            var userDic = users.ToDictionary(x => x.UserId, x => new { Name = x.UserName, CaseDispatchGroup = x.CaseDispatchGroup });

            var single = await context.OrgSetUp_UserOrgCaseSetUp.AsNoTracking().SingleOrDefaultAsync(x => x.UserId == userId);

            if (single is null)
                return ApiResponseHelper.NotFound<GetUserOrgCaseSetUpByIdResponse>(null, userId);

            var response = mapper.Map<GetUserOrgCaseSetUpByIdResponse>(single);
            response.UserName = userDic[userId].Name;
            response.DesignatedSupervisor1Name = string.IsNullOrEmpty(response.DesignatedSupervisor1)
                ? string.Empty
                : userDic[response.DesignatedSupervisor1].Name;
            response.DesignatedSupervisor2Name = string.IsNullOrEmpty(response.DesignatedSupervisor2)
                ? string.Empty
                : userDic[response.DesignatedSupervisor2].Name;

            var 分派組織 = userDic[userId].CaseDispatchGroup;
            response.CaseDispatchGroups = string.IsNullOrWhiteSpace(分派組織)
                ? []
                : 分派組織.Split(",").Select(x => new CaseDispatchGroupModel { CaseDispatchGroup = Enum.Parse<CaseDispatchGroup>(x) }).ToList();

            return ApiResponseHelper.Success(response);
        }
    }
}
