using ScoreSharp.API.Modules.Auth.Action.DeleteActionById;

namespace ScoreSharp.API.Modules.Auth.Action
{
    public partial class ActionController
    {
        ///<summary>
        /// 刪除單筆操作
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /Action/DeleteActionById/GetBillDayByQueryString
        ///
        /// </remarks>
        /// <param name="actionId">PK</param>
        ///<returns></returns>
        [HttpDelete("{actionId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除操作_2000_ResEx),
            typeof(刪除操作查無此資料_4001_ResEx),
            typeof(刪除操作此資源已被使用_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteActionById")]
        public async Task<IResult> DeleteActionById([FromRoute] string actionId)
        {
            var result = await _mediator.Send(new Command(actionId));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Auth.Action.DeleteActionById
{
    public record Command(string actionId) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IFusionCache _fusionCache;

        public Handler(ScoreSharpContext context, IFusionCache fusionCache)
        {
            _context = context;
            _fusionCache = fusionCache;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var single = await _context.Auth_Action.SingleOrDefaultAsync(x => x.ActionId == request.actionId);

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, request.actionId);

            var allActionWithRouterAndRole = await _context.Auth_Role_Router_Action.Select(x => x.ActionId).Distinct().ToListAsync();

            var isExist = allActionWithRouterAndRole.Contains(request.actionId);

            if (isExist)
                return ApiResponseHelper.此資源已被使用<string>(null, request.actionId);

            _context.Auth_Action.Remove(single);
            await _context.SaveChangesAsync();

            await _fusionCache.RemoveAsync($"{SecurityConstants.PolicyRedisKey.Action}:{request.actionId}");

            return ApiResponseHelper.DeleteByIdSuccess(request.actionId);
        }
    }
}
