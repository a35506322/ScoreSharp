using ScoreSharp.API.Modules.Auth.Router.InsertRouter;

namespace ScoreSharp.API.Modules.Auth.Router
{
    public partial class RouterController
    {
        /// <summary>
        /// 新增單筆路由
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <response code="200">新增成功返回 Key 值</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string?>))]
        [EndpointSpecificExample(typeof(新增路由_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增路由_2000_ResEx),
            typeof(新增路由資料已存在_4002_ResEx),
            typeof(新增路由查無路由類別_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertRouter")]
        public async Task<IResult> InsertRouter([FromBody] InsertRouterRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Auth.Router.InsertRouter
{
    public record Command(InsertRouterRequest insertRouterRequest) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IJWTProfilerHelper _jwthelper;

        public Handler(ScoreSharpContext context, IJWTProfilerHelper jwthelper)
        {
            _context = context;
            _jwthelper = jwthelper;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var dto = request.insertRouterRequest;

            var single = await _context.Auth_Router.SingleOrDefaultAsync(x => x.RouterId == dto.RouterId);

            if (single != null)
                return ApiResponseHelper.DataAlreadyExists<string>(null, dto.RouterId);

            var routerCategory = await _context.Auth_RouterCategory.SingleOrDefaultAsync(x =>
                x.RouterCategoryId == dto.RouterCategoryId && x.IsActive == "Y"
            );
            if (routerCategory is null)
                return ApiResponseHelper.前端傳入關聯資料有誤<string>(null, "路由類別Id", dto.RouterCategoryId);

            Auth_Router auth_Router = new Auth_Router()
            {
                RouterId = dto.RouterId,
                RouterName = dto.RouterName,
                DynamicParams = dto.DynamicParams,
                IsActive = dto.IsActive,
                AddUserId = _jwthelper.UserId,
                AddTime = DateTime.Now,
                RouterCategoryId = routerCategory.RouterCategoryId,
                Icon = dto.Icon,
                Sort = dto.Sort,
            };

            await _context.AddAsync(auth_Router);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess(dto.RouterId, dto.RouterId);
        }
    }
}
