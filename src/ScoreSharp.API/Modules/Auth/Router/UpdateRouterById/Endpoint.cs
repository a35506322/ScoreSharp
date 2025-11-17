using ScoreSharp.API.Modules.Auth.Router.UpdateRouterById;

namespace ScoreSharp.API.Modules.Auth.Router
{
    public partial class RouterController
    {
        /// <summary>
        /// 更新路由 By Id
        /// </summary>
        /// <param name="routerId">PK</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{routerId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [EndpointSpecificExample(typeof(修改路由_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(修改路由_2000_ResEx),
            typeof(修改路由路由與Req比對錯誤4003_ResEx),
            typeof(修改路由查無此資料_4001_ResEx),
            typeof(修改路由查無路由類別_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateRouterById")]
        public async Task<IResult> UpdateRouterById([FromRoute] string routerId, [FromBody] UpdateRouterByIdRequest request)
        {
            var result = await _mediator.Send(new Command(routerId, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Auth.Router.UpdateRouterById
{
    public record Command(string routerId, UpdateRouterByIdRequest updateRouterByIdRequest) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IJWTProfilerHelper _jwthelper;
        private readonly IScoreSharpDapperContext _dapperContext;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IJWTProfilerHelper jwthelper, IScoreSharpDapperContext dapperContext, IMapper mapper)
        {
            _context = context;
            _jwthelper = jwthelper;
            _dapperContext = dapperContext;
            _mapper = mapper;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var single = await _context.Auth_Router.AsNoTracking().SingleOrDefaultAsync(x => x.RouterId == request.routerId);

            if (request.routerId != request.updateRouterByIdRequest.RouterId)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, request.routerId);

            var dto = request.updateRouterByIdRequest;

            var routerCategory = await _context
                .Auth_RouterCategory.AsNoTracking()
                .SingleOrDefaultAsync(x => x.RouterCategoryId == dto.RouterCategoryId && x.IsActive == "Y");
            if (routerCategory is null)
                return ApiResponseHelper.前端傳入關聯資料有誤<string>(null, "路由類別Id", dto.RouterCategoryId);

            var entity = _mapper.Map<Auth_Router>(dto);
            entity.UpdateTime = DateTime.Now;
            entity.UpdateUserId = _jwthelper.UserId;

            var result = await this.UpdateRouterById(entity);

            return ApiResponseHelper.UpdateByIdSuccess(request.routerId, request.routerId);
        }

        public async Task<bool> UpdateRouterById(Auth_Router entity)
        {
            var sql =
                @"UPDATE [dbo].[Auth_Router]
                                SET   [DynamicParams] = @DynamicParams
                                        ,[Icon] = @Icon
                                        ,[UpdateTime] = @UpdateTime
                                        ,[UpdateUserId] = @UpdateUserId
                                        ,[RouterName] = @RouterName
                                        ,[RouterCategoryId] = @RouterCategoryId
                                        ,[IsActive] = @IsActive
                                        ,[Sort] = @Sort
                                WHERE [RouterId] = @RouterId ";

            using var conn = _dapperContext.CreateScoreSharpConnection();
            var result = await conn.ExecuteAsync(sql, entity);
            return result > 0;
        }
    }
}
