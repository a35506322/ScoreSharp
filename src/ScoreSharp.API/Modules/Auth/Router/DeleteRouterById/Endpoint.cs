using System.Linq;
using ScoreSharp.API.Modules.Auth.Router.DeleteRouterById;

namespace ScoreSharp.API.Modules.Auth.Router
{
    public partial class RouterController
    {
        /// <summary>
        /// 刪除單筆路由 ById
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /Router/DeleteRouterById/SetUpBillDay
        ///
        /// </remarks>
        /// <param name="routerId">PK</param>
        /// <returns></returns>
        [HttpDelete("{routerId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除路由_2000_ResEx),
            typeof(刪除路由查無此資料_4001_ResEx),
            typeof(刪除路由此資源已被使用_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteRouterById")]
        public async Task<IResult> DeleteRouterById([FromRoute] string routerId)
        {
            var result = await _mediator.Send(new Command(routerId));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Auth.Router.DeleteRouterById
{
    public record Command(string routerId) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;

        public Handler(ScoreSharpContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var single = await _context.Auth_Router.SingleOrDefaultAsync(x => x.RouterId == request.routerId);

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, request.routerId);

            var isExist = await _context.Auth_Action.AnyAsync(x => x.RouterId == request.routerId);

            if (isExist)
                return ApiResponseHelper.此資源已被使用<string>(null, request.routerId);

            _context.Auth_Router.Remove(single);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess(request.routerId);
        }
    }
}
