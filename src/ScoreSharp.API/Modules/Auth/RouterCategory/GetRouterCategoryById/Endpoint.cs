using ScoreSharp.API.Modules.Auth.RouterCategory.GetRouterCategoryById;

namespace ScoreSharp.API.Modules.Auth.RouterCategory
{
    public partial class RouterCategoryController
    {
        /// <summary>
        /// 取得單筆路由類別 ById
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /RouterCategory/GetRouterCatregoryById/1
        ///
        /// </remarks>
        /// <params name="routerCategoryId">PK</params>
        /// <returns></returns>
        [HttpGet("{routerCategoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetRouterCategoryByIdResponse>))]
        [EndpointSpecificExample(
            typeof(取得路由類別_2000_ResEx),
            typeof(取得路由類別查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetRouterCategoryById")]
        public async Task<IResult> GetRouterCategoryById([FromRoute] string routerCategoryId)
        {
            var result = await _mediator.Send(new Query(routerCategoryId));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Auth.RouterCategory.GetRouterCategoryById
{
    public record Query(string routerCategoryId) : IRequest<ResultResponse<GetRouterCategoryByIdResponse>>;

    public class Handler : IRequestHandler<Query, ResultResponse<GetRouterCategoryByIdResponse>>
    {
        private readonly ScoreSharpContext _context;
        private IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<GetRouterCategoryByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await _context
                .Auth_RouterCategory.AsNoTracking()
                .SingleOrDefaultAsync(x => x.RouterCategoryId == request.routerCategoryId);

            if (entity == null)
            {
                return ApiResponseHelper.NotFound<GetRouterCategoryByIdResponse>(null, request.routerCategoryId);
            }

            var response = _mapper.Map<GetRouterCategoryByIdResponse>(entity);

            return ApiResponseHelper.Success(response);
        }
    }
}
