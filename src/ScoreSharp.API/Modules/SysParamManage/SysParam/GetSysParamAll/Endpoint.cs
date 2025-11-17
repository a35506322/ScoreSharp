using ScoreSharp.API.Modules.SysParamManage.SysParam.GetSysParamAll;

namespace ScoreSharp.API.Modules.SysParamManage.SysParam
{
    public partial class SysParamController
    {
        /// <summary>
        /// 查詢全部系統參數設定
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetSysParamAllResponse>))]
        [EndpointSpecificExample(
            typeof(查詢全部系統參數設定_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetSysParamAll")]
        public async Task<IResult> GetSysParamAll()
        {
            var result = await _mediator.Send(new Query());
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SysParamManage.SysParam.GetSysParamAll
{
    public record Query() : IRequest<ResultResponse<GetSysParamAllResponse>>;

    public class Handler(ScoreSharpContext context, IMapper mapper) : IRequestHandler<Query, ResultResponse<GetSysParamAllResponse>>
    {
        public async Task<ResultResponse<GetSysParamAllResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await context.SysParamManage_SysParam.AsNoTracking().SingleOrDefaultAsync();

            var response = mapper.Map<GetSysParamAllResponse>(entity);

            return ApiResponseHelper.Success(response);
        }
    }
}
