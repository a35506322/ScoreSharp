using ScoreSharp.API.Modules.Auth.Router.GetRoutersByQueryString;

namespace ScoreSharp.API.Modules.Auth.Router
{
    public partial class RouterController
    {
        /// <summary>
        /// 取得路由 ByQueryString
        /// </summary>
        /// <remarks>
        /// Sample QueryString:
        ///
        ///     ?IsActive=Y&amp;RouterCategoryId=setup
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetRoutersByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得路由_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetRoutersByQueryString")]
        public async Task<IResult> GetRoutersByQueryString([FromQuery] GetRoutersByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Auth.Router.GetRoutersByQueryString
{
    public record Query(GetRoutersByQueryStringRequest getRoutersByQueryStringRequest)
        : IRequest<ResultResponse<List<GetRoutersByQueryStringResponse>>>;

    public class Handler : IRequestHandler<Query, ResultResponse<List<GetRoutersByQueryStringResponse>>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<List<GetRoutersByQueryStringResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var getRouterRequest = request.getRoutersByQueryStringRequest;

            var query = await _context
                .Auth_Router.AsNoTracking()
                .Where(x => String.IsNullOrEmpty(getRouterRequest.IsActive) || x.IsActive == getRouterRequest.IsActive)
                .Where(x =>
                    String.IsNullOrEmpty(getRouterRequest.RouterCategoryId) || x.RouterCategoryId == getRouterRequest.RouterCategoryId
                )
                .OrderBy(x => x.Sort)
                .ToListAsync();

            var result = _mapper.Map<List<GetRoutersByQueryStringResponse>>(query);

            return ApiResponseHelper.Success(result);
        }
    }
}
