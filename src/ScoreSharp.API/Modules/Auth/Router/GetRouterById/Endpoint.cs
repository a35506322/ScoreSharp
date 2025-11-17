using ScoreSharp.API.Modules.Auth.Router.GetRouterById;

namespace ScoreSharp.API.Modules.Auth.Router
{
    public partial class RouterController
    {
        ///<summary>
        ///  取得路由 ById
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /Router/GetRouterById/SetUp
        ///
        /// </remarks>
        /// <params name="routerId">PK</params>
        /// <returns></returns>
        [HttpGet("{routerId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetRouterByIdResponse>))]
        [EndpointSpecificExample(
            typeof(取得路由_2000_ResEx),
            typeof(取得路由查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetRouterById")]
        public async Task<IResult> GetRouterById([FromRoute] string routerId)
        {
            var result = await _mediator.Send(new Query(routerId));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Auth.Router.GetRouterById
{
    public record Query(string routerId) : IRequest<ResultResponse<GetRouterByIdResponse>>;

    public class Handler : IRequestHandler<Query, ResultResponse<GetRouterByIdResponse>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<GetRouterByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await _context.Auth_Router.AsNoTracking().SingleOrDefaultAsync(x => x.RouterId == request.routerId);

            if (entity == null)
            {
                return ApiResponseHelper.NotFound<GetRouterByIdResponse>(null, request.routerId);
            }

            var response = _mapper.Map<GetRouterByIdResponse>(entity);

            return ApiResponseHelper.Success(response);
        }
    }
}
