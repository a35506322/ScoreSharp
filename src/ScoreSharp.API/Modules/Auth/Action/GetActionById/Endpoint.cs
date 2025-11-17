using ScoreSharp.API.Modules.Auth.Action.GetActionById;

namespace ScoreSharp.API.Modules.Auth.Action
{
    public partial class ActionController
    {
        ///<summary>
        /// 查詢單筆操作 ById
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /Action/GetActionById/GetBillDayByQueryString
        ///
        /// </remarks>
        /// <param name="actionId">PK</param>
        /// <returns></returns>
        [HttpGet("{actionId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetActionByIdResponse>))]
        [EndpointSpecificExample(
            typeof(取得操作查無此資料_4001_ResEx),
            typeof(取得操作_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetActionById")]
        public async Task<IResult> GetActionById([FromRoute] string actionId)
        {
            var result = await _mediator.Send(new Query(actionId));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Auth.Action.GetActionById
{
    public record Query(string actionId) : IRequest<ResultResponse<GetActionByIdResponse>>;

    public class Handler : IRequestHandler<Query, ResultResponse<GetActionByIdResponse>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<GetActionByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var single = await _context.Auth_Action.AsNoTracking().SingleOrDefaultAsync(x => x.ActionId == request.actionId);

            if (single is null)
                return ApiResponseHelper.NotFound<GetActionByIdResponse>(null, request.actionId);

            var response = _mapper.Map<GetActionByIdResponse>(single);

            return ApiResponseHelper.Success(response);
        }
    }
}
