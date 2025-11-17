using ScoreSharp.API.Modules.Auth.Action.UpdateActionById;

namespace ScoreSharp.API.Modules.Auth.Action
{
    public partial class ActionController
    {
        ///<summary>
        /// 更新單筆操作 By Id
        /// </summary>
        /// <param name="actionId">PK</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{actionId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [EndpointSpecificExample(typeof(修改操作_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(修改操作_2000_ResEx),
            typeof(修改操作查無此資料_4001_ResEx),
            typeof(修改操作查無路由_4003_ResEx),
            typeof(修改操作路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateActionById")]
        public async Task<IResult> UpdateActionById([FromRoute] string actionId, [FromBody] UpdateActionByIdRequest request)
        {
            var result = await _mediator.Send(new Command(actionId, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Auth.Action.UpdateActionById
{
    public record Command(string actionId, UpdateActionByIdRequest updateActionByIdRequest) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IJWTProfilerHelper _jwthelper;
        private readonly IMapper _mapper;
        private readonly IScoreSharpDapperContext _dapperContext;
        private readonly IFusionCache _fusionCache;

        public Handler(
            ScoreSharpContext context,
            IJWTProfilerHelper jwthelper,
            IMapper mapper,
            IScoreSharpDapperContext dapperContext,
            IFusionCache fusionCache
        )
        {
            _context = context;
            _jwthelper = jwthelper;
            _mapper = mapper;
            _dapperContext = dapperContext;
            _fusionCache = fusionCache;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.actionId != request.updateActionByIdRequest.ActionId)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var single = await _context.Auth_Action.SingleOrDefaultAsync(x => x.ActionId == request.actionId);

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, request.actionId);

            var dto = request.updateActionByIdRequest;

            var router = await _context.Auth_Router.AsNoTracking().SingleOrDefaultAsync(x => x.RouterId == dto.RouterId && x.IsActive == "Y");
            if (router is null)
                return ApiResponseHelper.前端傳入關聯資料有誤<string>(null, "路由Id", dto.RouterId);

            var entity = _mapper.Map<Auth_Action>(dto);
            entity.UpdateTime = DateTime.Now;
            entity.UpdateUserId = _jwthelper.UserId;

            var result = await this.UpdateActionById(entity);

            await _fusionCache.RemoveAsync($"{SecurityConstants.PolicyRedisKey.Action}:{request.actionId}");

            return ApiResponseHelper.UpdateByIdSuccess(request.actionId, request.actionId);
        }

        public async Task<bool> UpdateActionById(Auth_Action entity)
        {
            string sql =
                @"UPDATE [dbo].[Auth_Action]
                                    SET  [ActionName] = @ActionName
                                        ,[IsCommon] = @IsCommon
                                        ,[IsActive] = @IsActive
                                        ,[UpdateUserId] = @UpdateUserId
                                        ,[UpdateTime] = @UpdateTime
                                        ,[RouterId] = @RouterId
                                    WHERE [ActionId] = @ActionId";

            using var conn = _dapperContext.CreateScoreSharpConnection();
            var result = await conn.ExecuteAsync(sql, entity);
            return result > 0;
        }
    }
}
