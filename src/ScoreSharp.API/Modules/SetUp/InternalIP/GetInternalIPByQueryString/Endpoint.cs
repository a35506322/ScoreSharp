using ScoreSharp.API.Modules.SetUp.InternalIP.GetInternalIPByQueryString;

namespace ScoreSharp.API.Modules.SetUp.InternalIP
{
    public partial class InternalIPController
    {
        /// <summary>
        /// 取得多筆行內IP ByQueryString
        /// </summary>
        /// <remarks>
        /// Sample QueryString:
        ///
        ///     ?IsActive=Y
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetInternalIPByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得多筆行內IP_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetInternalIPByQueryString")]
        public async Task<IResult> GetInternalIPByQueryString([FromQuery] GetInternalIPByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.InternalIP.GetInternalIPByQueryString
{
    public record Query(GetInternalIPByQueryStringRequest getInternalIPByQueryStringRequest)
        : IRequest<ResultResponse<List<GetInternalIPByQueryStringResponse>>>;

    public class Handler : IRequestHandler<Query, ResultResponse<List<GetInternalIPByQueryStringResponse>>>
    {
        private readonly ScoreSharpContext _context;

        public Handler(ScoreSharpContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<List<GetInternalIPByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var getInternalIPByQueryStringRequest = request.getInternalIPByQueryStringRequest;

            var entity = await _context
                .SetUp_InternalIP.AsNoTracking()
                .Where(x =>
                    String.IsNullOrEmpty(getInternalIPByQueryStringRequest.IsActive)
                    || x.IsActive == getInternalIPByQueryStringRequest.IsActive
                )
                .Select(x => new GetInternalIPByQueryStringResponse()
                {
                    IP = x.IP,
                    AddUserId = x.AddUserId,
                    AddTime = x.AddTime,
                    IsActive = x.IsActive,
                    UpdateTime = x.UpdateTime,
                    UpdateUserId = x.UpdateUserId,
                })
                .ToListAsync();

            return ApiResponseHelper.Success(entity);
        }
    }
}
