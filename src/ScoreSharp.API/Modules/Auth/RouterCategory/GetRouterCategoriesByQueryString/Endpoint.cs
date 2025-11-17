using ScoreSharp.API.Modules.Auth.RouterCategory.GetRouterCategoriesByQueryString;

namespace ScoreSharp.API.Modules.Auth.RouterCategory
{
    public partial class RouterCategoryController
    {
        /// <summary>
        /// 取得路由類別 ByQueryString
        /// </summary>
        /// <remarks>
        /// Sample QueryString:
        ///
        ///     ?RouterCategoryName=測試
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetRouterCategoriesByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得路由類別_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetRouterCategoriesByQueryString")]
        public async Task<IResult> GetRouterCategoriesByQueryString([FromQuery] GetRouterCategoriesByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Auth.RouterCategory.GetRouterCategoriesByQueryString
{
    public record Query(GetRouterCategoriesByQueryStringRequest getRouterCategoryRequest)
        : IRequest<ResultResponse<List<GetRouterCategoriesByQueryStringResponse>>>;

    public class Handler : IRequestHandler<Query, ResultResponse<List<GetRouterCategoriesByQueryStringResponse>>>
    {
        private readonly ScoreSharpContext _context;

        public Handler(ScoreSharpContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<List<GetRouterCategoriesByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var getRouterCategoryRequest = request.getRouterCategoryRequest;

            var filterEnties = await _context
                .Auth_RouterCategory.AsNoTracking()
                .Where(x =>
                    String.IsNullOrEmpty(getRouterCategoryRequest.RouterCategoryName)
                    || x.RouterCategoryName.Contains(getRouterCategoryRequest.RouterCategoryName)
                )
                .Where(x => String.IsNullOrEmpty(getRouterCategoryRequest.IsActive) || x.IsActive == getRouterCategoryRequest.IsActive)
                .Select(x => new GetRouterCategoriesByQueryStringResponse()
                {
                    UpdateUserId = x.UpdateUserId,
                    UpdateTime = x.UpdateTime,
                    AddTime = x.AddTime,
                    AddUserId = x.AddUserId,
                    IsActive = x.IsActive,
                    RouterCategoryId = x.RouterCategoryId,
                    RouterCategoryName = x.RouterCategoryName,
                    Icon = x.Icon,
                    Sort = x.Sort,
                })
                .OrderBy(x => x.Sort)
                .ToListAsync();

            return ApiResponseHelper.Success(filterEnties);
        }
    }
}
