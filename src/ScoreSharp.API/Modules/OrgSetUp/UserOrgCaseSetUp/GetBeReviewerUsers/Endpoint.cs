using System.Data;
using ScoreSharp.API.Modules.OrgSetUp.UserOrgCaseSetUp.GetBeReviewerUsers;

namespace ScoreSharp.API.Modules.OrgSetUp.UserOrgCaseSetUp
{
    public partial class UserOrgCaseSetUpController
    {
        /// <summary>
        /// 查詢可新增人員
        ///
        ///     /UserOrgCaseSetUp/GetBeReviewerUsers?QueryType=SELF
        ///     /UserOrgCaseSetUp/GetBeReviewerUsers?QueryType=ALL
        ///
        /// </summary>
        ///<returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetBeReviewerUsersResponse>>))]
        [EndpointSpecificExample(typeof(查詢可新增人員_2000_ResEx), ExampleType = ExampleType.Response, ResponseStatusCode = StatusCodes.Status200OK)]
        [OpenApiOperation("GetBeReviewerUsers")]
        public async Task<IResult> GetBeReviewerUsers([FromQuery] GetBeReviewerUsersRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.OrgSetUp.UserOrgCaseSetUp.GetBeReviewerUsers
{
    public record Query(GetBeReviewerUsersRequest GetBeReviewerUsersRequest) : IRequest<ResultResponse<List<GetBeReviewerUsersResponse>>>;

    public class Handler(ScoreSharpContext context, IJWTProfilerHelper jWTProfilerHelper)
        : IRequestHandler<Query, ResultResponse<List<GetBeReviewerUsersResponse>>>
    {
        public async Task<ResultResponse<List<GetBeReviewerUsersResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var queryType = request.GetBeReviewerUsersRequest.QueryType;

            string 派案群組 = string.Empty;
            if (queryType.Equals(查詢類型.自己, StringComparison.OrdinalIgnoreCase))
            {
                派案群組 = string.Join(",", jWTProfilerHelper.CaseDispatchGroups?.Cast<int>());
            }
            else if (queryType.Equals(查詢類型.全部, StringComparison.OrdinalIgnoreCase))
            {
                var values = EnumExtenstions.GetEnumOptions<CaseDispatchGroup>("Y").Select(x => x.Value).ToList();
                派案群組 = string.Join(",", values);
            }

            var parameter = new SqlParameter("@CaseDispatchGroups", 派案群組);

            List<GetBeReviewerUsersResponse> result = await context
                .Database.SqlQueryRaw<GetBeReviewerUsersResponse>("EXEC Usp_GetBeReviewerUsers @CaseDispatchGroups", parameter)
                .ToListAsync(cancellationToken);

            return ApiResponseHelper.Success(result);
        }
    }
}
