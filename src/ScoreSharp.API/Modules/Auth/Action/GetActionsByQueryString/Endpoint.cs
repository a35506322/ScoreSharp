using ScoreSharp.API.Modules.Auth.Action.GetActionsByQueryString;

namespace ScoreSharp.API.Modules.Auth.Action
{
    public partial class ActionController
    {
        ///<summary>
        /// 查詢多筆操作
        /// </summary>
        /// <remarks>
        /// Sample QueryString:
        ///
        ///     ?IsActive=Y&amp;RouterId=SetUpBillDay
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetActionsByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得操作_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetActionByQueryString")]
        public async Task<IResult> GetActionsByQueryString([FromQuery] GetActionByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Auth.Action.GetActionsByQueryString
{
    public record Query(GetActionByQueryStringRequest getActionByQueryStringRequest)
        : IRequest<ResultResponse<List<GetActionsByQueryStringResponse>>>;

    public class Handler : IRequestHandler<Query, ResultResponse<List<GetActionsByQueryStringResponse>>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<List<GetActionsByQueryStringResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var getActionByQueryStringRequest = request.getActionByQueryStringRequest;

            var entities = await _context
                .Auth_Action.AsNoTracking()
                .Where(x =>
                    string.IsNullOrEmpty(getActionByQueryStringRequest.IsCommon) || x.IsCommon == getActionByQueryStringRequest.IsCommon
                )
                .Where(x =>
                    string.IsNullOrEmpty(getActionByQueryStringRequest.IsActive) || x.IsActive == getActionByQueryStringRequest.IsActive
                )
                .Where(x =>
                    string.IsNullOrEmpty(getActionByQueryStringRequest.RouterId) || x.RouterId == getActionByQueryStringRequest.RouterId
                )
                .ToListAsync();

            var result = _mapper.Map<List<GetActionsByQueryStringResponse>>(entities);

            return ApiResponseHelper.Success(result);
        }
    }
}
