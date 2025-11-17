using ScoreSharp.API.Modules.Auth.Role.GetRoleAuthById;

namespace ScoreSharp.API.Modules.Auth.Role
{
    public partial class RoleController
    {
        ///<summary>
        ///  取得單筆角色權限
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /Role/GetRoleAuthById/Admin
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpGet("{roleId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetRoleAuthByIdResponse>>))]
        [EndpointSpecificExample(
            typeof(取得角色權限_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetRoleAuthById")]
        public async Task<IResult> GetRoleAuthById([FromRoute] string roleId)
        {
            var result = await _mediator.Send(new Query(roleId));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Auth.Role.GetRoleAuthById
{
    public record Query(string roleId) : IRequest<ResultResponse<List<GetRoleAuthByIdResponse>>>;

    public class Handler : IRequestHandler<Query, ResultResponse<List<GetRoleAuthByIdResponse>>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IScoreSharpDapperContext _dapperContext;

        public Handler(ScoreSharpContext context, IScoreSharpDapperContext dapperContext)
        {
            _context = context;
            _dapperContext = dapperContext;
        }

        public async Task<ResultResponse<List<GetRoleAuthByIdResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var entities = await GetAllAction();
            var roleRouterAction = await _context
                .Auth_Role_Router_Action.Where(x => x.RoleId == request.roleId)
                .Select(x => x.ActionId)
                .ToListAsync();

            var groupedEntities = entities.GroupBy(e => new { e.RouterCategoryId, e.RouterCategoryName });

            List<GetRoleAuthByIdResponse> results = new();

            foreach (var categoryGroup in groupedEntities)
            {
                var categoryResponse = new GetRoleAuthByIdResponse
                {
                    RouterCategoryId = categoryGroup.Key.RouterCategoryId,
                    RouterCategoryName = categoryGroup.Key.RouterCategoryName,
                    Routers = new List<Router>(),
                };

                var routerGroups = categoryGroup.GroupBy(e => new { e.RouterId, e.RouterName });

                foreach (var routerGroup in routerGroups)
                {
                    var router = new Router
                    {
                        RouterId = routerGroup.Key.RouterId,
                        RouterName = routerGroup.Key.RouterName,
                        Actions = routerGroup
                            .Select(a => new Action
                            {
                                ActionId = a.ActionId,
                                ActionName = a.ActionName,
                                HasPermission = roleRouterAction.SingleOrDefault(x => x == a.ActionId) != null ? "Y" : "N",
                            })
                            .ToList(),
                    };

                    categoryResponse.Routers.Add(router);
                }

                results.Add(categoryResponse);
            }

            return ApiResponseHelper.Success(results);
        }

        private async Task<IEnumerable<RoleAuthDto>> GetAllAction()
        {
            string sql =
                @"SELECT  C.RouterCategoryId
		                           , RouterCategoryName
		                           , B.RouterId
		                           , RouterName
		                           , A.ActionId
		                           , ActionName		
                           FROM  [ScoreSharp].[dbo].[Auth_Action] A
                           JOIN [ScoreSharp].[dbo].[Auth_Router] B
                           ON A.RouterId = B.RouterId
                           JOIN [ScoreSharp].[dbo].[Auth_RouterCategory] C
                           ON B.RouterCategoryId = C.RouterCategoryId
                           WHERE A.ActionId NOT IN ('Login','GetUserAuthBySelf')
                           ORDER BY C.Sort,B.Sort";

            using var conn = _dapperContext.CreateScoreSharpConnection();
            var result = await conn.QueryAsync<RoleAuthDto>(sql);
            return result;
        }
    }
}
