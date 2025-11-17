using ScoreSharp.API.Modules.Auth.RouterCategory.InsertRouterCategory;

namespace ScoreSharp.API.Modules.Auth.RouterCategory
{
    public partial class RouterCategoryController
    {
        /// <summary>
        /// 新增路由類別
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <response code="200">新增成功返回 Key 值</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string?>))]
        [EndpointSpecificExample(typeof(新增路由類別_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增路由類別_2000_ResEx),
            typeof(新增路由類別資料已存在_4002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertRouterCategory")]
        public async Task<IResult> InsertRouterCategory([FromBody] InsertRouterCategoryRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Auth.RouterCategory.InsertRouterCategory
{
    public record Command(InsertRouterCategoryRequest InsertRouterCategoryRequest) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;

        public Handler(ScoreSharpContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var dto = request.InsertRouterCategoryRequest;

            var single = await _context.Auth_RouterCategory.SingleOrDefaultAsync(x => x.RouterCategoryId == dto.RouterCategoryId);

            if (single != null)
                return ApiResponseHelper.DataAlreadyExists<string>(null, dto.RouterCategoryId);

            Auth_RouterCategory auth_RouterCategory = new Auth_RouterCategory()
            {
                RouterCategoryId = dto.RouterCategoryId,
                RouterCategoryName = dto.RouterCategoryName,
                IsActive = dto.IsActive,
                Icon = dto.Icon,
                Sort = dto.Sort,
            };

            await _context.AddAsync(auth_RouterCategory);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess(auth_RouterCategory.RouterCategoryId, auth_RouterCategory.RouterCategoryId);
        }
    }
}
