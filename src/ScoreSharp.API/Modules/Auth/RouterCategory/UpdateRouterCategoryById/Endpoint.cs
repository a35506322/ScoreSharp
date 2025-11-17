using ScoreSharp.API.Modules.Auth.RouterCategory.UpdateRouterCategoryById;

namespace ScoreSharp.API.Modules.Auth.RouterCategory
{
    public partial class RouterCategoryController
    {
        /// <summary>
        /// 更新路由類別 By Id
        /// </summary>
        /// <param name="routerCategoryId">PK</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{routerCategoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [EndpointSpecificExample(typeof(修改路由類別_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(修改路由類別_2000_ResEx),
            typeof(修改路由類別查無此資料_4001_ResEx),
            typeof(修改路由類別路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateRouterCategoryById")]
        public async Task<IResult> UpdateRouterCategoryById([FromRoute] string routerCategoryId, [FromBody] UpdateRouterCategoryByIdRequest request)
        {
            var result = await _mediator.Send(new Command(routerCategoryId, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Auth.RouterCategory.UpdateRouterCategoryById
{
    public record Command(string RouterCategoryId, UpdateRouterCategoryByIdRequest UpdateRouterCategoryByIdRequest)
        : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IScoreSharpDapperContext _dapperContext;
        private readonly IJWTProfilerHelper _jwtProfilerHelper;

        public Handler(ScoreSharpContext context, IScoreSharpDapperContext dapperContext, IJWTProfilerHelper jwtProfilerHelper)
        {
            _context = context;
            _dapperContext = dapperContext;
            _jwtProfilerHelper = jwtProfilerHelper;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.RouterCategoryId != request.UpdateRouterCategoryByIdRequest.RouterCategoryId)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var single = await _context.Auth_RouterCategory.SingleOrDefaultAsync(x => x.RouterCategoryId == request.RouterCategoryId);

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, request.RouterCategoryId);

            var dto = request.UpdateRouterCategoryByIdRequest;

            Auth_RouterCategory entity = new()
            {
                RouterCategoryId = dto.RouterCategoryId,
                RouterCategoryName = dto.RouterCategoryName,
                IsActive = dto.IsActive,
                Icon = dto.Icon,
                UpdateTime = DateTime.Now,
                UpdateUserId = _jwtProfilerHelper.UserId,
                Sort = dto.Sort,
            };

            var result = await this.UpdateRouterCategoryById(entity);

            return ApiResponseHelper.UpdateByIdSuccess(request.RouterCategoryId, request.RouterCategoryId);
        }

        private async Task<bool> UpdateRouterCategoryById(Auth_RouterCategory entity)
        {
            var sql =
                @"UPDATE [dbo].[Auth_RouterCategory]
                       SET [RouterCategoryName] = @RouterCategoryName
                          ,[IsActive] = @IsActive
                          ,[UpdateUserId] = @UpdateUserId
                          ,[UpdateTime] = @UpdateTime
                          ,[Icon] = @Icon
                          ,[Sort] = @Sort
                        WHERE RouterCategoryId = @RouterCategoryId";

            using var conn = _dapperContext.CreateScoreSharpConnection();
            var result = await conn.ExecuteAsync(sql, entity);
            return result > 0;
        }
    }
}
