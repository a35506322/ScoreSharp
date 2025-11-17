using ScoreSharp.API.Modules.Auth.Action.InsertAction;

namespace ScoreSharp.API.Modules.Auth.Action
{
    public partial class ActionController
    {
        ///<summary>
        /// 新增單筆操作
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <response code="200">新增成功返回 Key 值</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string?>))]
        [EndpointSpecificExample(typeof(新增操作_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增操作_2000_ResEx),
            typeof(新增操作資料已存在_4002_ResEx),
            typeof(新增操作查無路由_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertAction")]
        public async Task<IResult> InsertAction([FromBody] InsertActionRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Auth.Action.InsertAction
{
    public record Command(InsertActionRequest insertActionRequest) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IJWTProfilerHelper _jwthelper;
        private readonly IMapper _mapper;
        private readonly IFusionCache _fusionCache;

        public Handler(ScoreSharpContext context, IJWTProfilerHelper jwthelper, IMapper mapper, IFusionCache fusionCache)
        {
            _context = context;
            _jwthelper = jwthelper;
            _mapper = mapper;
            _fusionCache = fusionCache;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var dto = request.insertActionRequest;

            var single = await _context.Auth_Action.SingleOrDefaultAsync(x => x.ActionId == dto.ActionId);

            if (single != null)
                return ApiResponseHelper.DataAlreadyExists<string>(null, dto.ActionId);

            var router = await _context.Auth_Router.SingleOrDefaultAsync(x => x.RouterId == dto.RouterId && x.IsActive == "Y");

            if (router is null)
                return ApiResponseHelper.前端傳入關聯資料有誤<string>(null, "路由Id", dto.RouterId);

            var entity = _mapper.Map<Auth_Action>(dto);
            entity.AddTime = DateTime.Now;
            entity.AddUserId = _jwthelper.UserId;
            entity.RouterId = router.RouterId;

            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            var cacheKey = $"{SecurityConstants.PolicyRedisKey.Action}:{dto.ActionId}";
            await _fusionCache.RemoveAsync(cacheKey);

            return ApiResponseHelper.InsertSuccess(dto.ActionId, dto.ActionId);
        }
    }
}
