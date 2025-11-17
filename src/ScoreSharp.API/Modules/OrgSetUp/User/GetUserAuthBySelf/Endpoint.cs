using ScoreSharp.API.Modules.OrgSetUp.User.GetUserAuthBySelf;

namespace ScoreSharp.API.Modules.OrgSetUp.User
{
    public partial class UserController
    {
        ///<summary>
        ///  取得單筆使用者權限
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /User/GetUserAuthBySelf
        ///
        /// 注意此權限並無新增至 Auth_Action 而是掛 [AllowAnonymous]
        /// 主因這是登入完呼叫的所以有點跳脫頁面框架，
        /// 且綁 Token 內部的 UserId 才能查詢如果直接呼叫此API也無意義
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetUserAuthByIdResponse>>))]
        [EndpointSpecificExample(
            typeof(取得使用者權限_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [AllowAnonymous]
        [OpenApiOperation("GetUserAuthBySelf")]
        public async Task<IResult> GetUserAuthBySelf()
        {
            var result = await _mediator.Send(new Query());
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.OrgSetUp.User.GetUserAuthBySelf
{
    public record Query() : IRequest<ResultResponse<List<GetUserAuthByIdResponse>>>;

    public class Handler : IRequestHandler<Query, ResultResponse<List<GetUserAuthByIdResponse>>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IScoreSharpDapperContext _dapperContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Handler(ScoreSharpContext context, IScoreSharpDapperContext dapperContext, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _dapperContext = dapperContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResultResponse<List<GetUserAuthByIdResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext.User;
            string userId = user.Identity.Name;
            var entities = await GetUserAuthById(userId);

            var groupedEntities = entities.GroupBy(e => new
            {
                e.RouterCategoryId,
                e.RouterCategoryName,
                e.CategoryIcon,
            });

            List<GetUserAuthByIdResponse> results = new();

            foreach (var categoryGroup in groupedEntities)
            {
                var categoryResponse = new GetUserAuthByIdResponse
                {
                    routerCategoryId = categoryGroup.Key.RouterCategoryId,
                    routerCategoryName = categoryGroup.Key.RouterCategoryName,
                    icon = categoryGroup.Key.CategoryIcon,
                    routers = new List<Router>(),
                };

                var routerGroups = categoryGroup.GroupBy(e => new
                {
                    e.RouterId,
                    e.RouterName,
                    e.RouterIcon,
                });

                foreach (var routerGroup in routerGroups)
                {
                    var router = new Router
                    {
                        routerId = routerGroup.Key.RouterId,
                        routerName = routerGroup.Key.RouterName,
                        icon = routerGroup.Key.RouterIcon,
                        actions = routerGroup.Select(a => new Action { actionId = a.ActionId, actionName = a.ActionName }).ToList(),
                    };

                    categoryResponse.routers.Add(router);
                }

                results.Add(categoryResponse);
            }

            return ApiResponseHelper.Success(results);
        }

        private async Task<IEnumerable<UserAuthDto>> GetUserAuthById(string userId)
        {
            string sql =
                @"SELECT DISTINCT F.RouterCategoryId,
                                           RouterCategoryName,
                                           F.Icon AS CategoryIcon,
                                           E.RouterId,
                                           RouterName,
                                           E.Icon AS RouterIcon,
                                           D.ActionId,
                                           ActionName,
                                           F.Sort,
                                           E.Sort 
                           FROM [ScoreSharp].[dbo].[Auth_User_Role] A
                           JOIN [ScoreSharp].[dbo].[Auth_Role] B
                           ON A.RoleId = B.RoleId
                           JOIN [ScoreSharp].[dbo].[Auth_Role_Router_Action] C
                           ON B.RoleId = C.RoleId
                           JOIN [ScoreSharp].[dbo].[Auth_Action] D
                           ON C.ActionId = D.ActionId
                           JOIN [ScoreSharp].[dbo].[Auth_Router] E
                           ON D.RouterId = E.RouterId
                           JOIN [ScoreSharp].[dbo].[Auth_RouterCategory] F
                           ON E.RouterCategoryId = F.RouterCategoryId
                           WHERE D.IsActive = 'Y' 
                           AND E.IsActive = 'Y' 
                           AND F.IsActive = 'Y'
                           AND B.IsActive = 'Y'
                           AND UserId = @UserId
                           ORDER BY F.Sort,E.Sort";

            using var conn = _dapperContext.CreateScoreSharpConnection();
            var result = await conn.QueryAsync<UserAuthDto>(sql, new { UserId = userId });
            return result;
        }
    }
}
