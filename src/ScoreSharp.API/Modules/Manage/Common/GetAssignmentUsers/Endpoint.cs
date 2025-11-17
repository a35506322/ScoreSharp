using ScoreSharp.API.Modules.Manage.Common.GetAssignmentUsers;

namespace ScoreSharp.API.Modules.Manage.Common
{
    public partial class ManageCommonController
    {
        /// <summary>
        /// 取得可派案的徵審人員
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ManageCommon/GetAssignmentUsers
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetAssignmentUsersResponse>>))]
        [EndpointSpecificExample(
            typeof(取得可派案的徵審人員_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetAssignmentUsers")]
        public async Task<IResult> GetAssignmentUsers()
        {
            var result = await _mediator.Send(new Query());
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Manage.Common.GetAssignmentUsers
{
    public record Query() : IRequest<ResultResponse<List<GetAssignmentUsersResponse>>>;

    public class Handler(ScoreSharpContext context, IJWTProfilerHelper jwtProfilerHelper)
        : IRequestHandler<Query, ResultResponse<List<GetAssignmentUsersResponse>>>
    {
        public async Task<ResultResponse<List<GetAssignmentUsersResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var sql = @"EXEC [dbo].[Usp_GetAssignmentUsers] @CaseDispatchGroups = @CaseDispatchGroups";

            var caseDispatchGroups = string.Join(",", jwtProfilerHelper.CaseDispatchGroups.Select(x => (int)x));
            var queryAssignmentUsersResponse = await context
                .Database.GetDbConnection()
                .QueryAsync<QueryAssignmentUsersResponse>(sql, new { CaseDispatchGroups = caseDispatchGroups });

            var now = DateTime.Now;

            var vacationUsers = await context
                .OrgSetUp_UserTakeVacation.AsNoTracking()
                .Where(x => x.StartTime <= now && now <= x.EndTime)
                .Select(x => x.UserId)
                .Distinct()
                .ToListAsync();

            var response = queryAssignmentUsersResponse
                .Select(x => new GetAssignmentUsersResponse
                {
                    UserId = x.UserId,
                    UserName = x.UserName,
                    CaseDispatchGroups = x
                        .CaseDispatchGroup.Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(y => new CaseDispatchGroupInfo { CaseDispatchGroup = Enum.Parse<CaseDispatchGroup>(y) })
                        .ToList(),
                    IsVacation = vacationUsers.Contains(x.UserId) ? "Y" : "N",
                    IsPaperCase = x.IsPaperCase,
                    PaperCaseSort = x.PaperCaseSort,
                    IsWebCase = x.IsWebCase,
                    WebCaseSort = x.WebCaseSort,
                    IsManualCase = x.IsManualCase,
                    ManualCaseSort = x.ManualCaseSort,
                })
                .ToList();

            return ApiResponseHelper.Success(response);
        }
    }
}
