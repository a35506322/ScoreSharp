using ScoreSharp.API.Modules.Manage.ObligedAssignment.GetObligedAssignmentCardStatusByQueryString;

namespace ScoreSharp.API.Modules.Manage.ObligedAssignment
{
    public partial class ObligedAssignmentController
    {
        /// <summary>
        /// 取得強制派案案件狀態
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ObligedAssignment/GetObligedAssignmentCardStatusByQueryString?RoleId=
        ///
        ///     可用在前端
        ///     1. 根據當前登入者是否可使用案件狀態做查詢
        ///     2. 用於強制派案狀態設定查詢
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetObligedAssignmentCardStatusByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得強制派案案件狀態_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetObligedAssignmentCardStatusByQueryString")]
        public async Task<IResult> GetObligedAssignmentCardStatusByQueryString([FromQuery] GetObligedAssignmentCardStatusByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Manage.ObligedAssignment.GetObligedAssignmentCardStatusByQueryString
{
    public record Query(GetObligedAssignmentCardStatusByQueryStringRequest GetObligedAssignmentCardStatusByQueryStringRequest)
        : IRequest<ResultResponse<List<GetObligedAssignmentCardStatusByQueryStringResponse>>>;

    public class Handler(ScoreSharpContext _context)
        : IRequestHandler<Query, ResultResponse<List<GetObligedAssignmentCardStatusByQueryStringResponse>>>
    {
        public async Task<ResultResponse<List<GetObligedAssignmentCardStatusByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var req = request.GetObligedAssignmentCardStatusByQueryStringRequest;

            var cardStatusRoles = await _context.Auth_ObligedAssignmentCardStatus_Role.AsNoTracking().ToListAsync();
            var response = cardStatusRoles
                .Select(x => new GetObligedAssignmentCardStatusByQueryStringResponse
                {
                    RoleIds = string.IsNullOrWhiteSpace(x.RoleId)
                        ? new List<string>()
                        : x.RoleId.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList(),
                    CaseQueryCardStatus = x.CardStatus,
                })
                .ToList();

            if (string.IsNullOrWhiteSpace(req.RoleId))
            {
                return ApiResponseHelper.Success(response);
            }

            var filteredResponse = response.Where(x => x.RoleIds.Contains(req.RoleId, StringComparer.OrdinalIgnoreCase)).ToList();
            return ApiResponseHelper.Success(filteredResponse);
        }
    }
}
