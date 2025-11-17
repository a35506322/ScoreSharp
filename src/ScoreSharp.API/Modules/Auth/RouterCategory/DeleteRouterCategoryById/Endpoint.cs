using ScoreSharp.API.Modules.Auth.RouterCategory.DeleteRouterCategoryById;

namespace ScoreSharp.API.Modules.Auth.RouterCategory
{
    public partial class RouterCategoryController
    {
        /// <summary>
        /// 刪除單筆路由類別 ById
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /RouterCategory/DeleteRouterCategoryById/SetUp
        ///
        /// </remarks>
        /// <params name="routerCategoryId">PK</params>
        /// <returns></returns>
        [HttpDelete("{routerCategoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除路由類別_2000_ResEx),
            typeof(刪除路由類別查無此資料_4001_ResEx),
            typeof(刪除路由類別此資源已被使用_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteRouterCategoryById")]
        public async Task<IResult> DeleteRouterCategoryById([FromRoute] string routerCategoryId)
        {
            var result = await _mediator.Send(new Command(routerCategoryId));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Auth.RouterCategory.DeleteRouterCategoryById
{
    public record Command(string RouterCategoryId) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;

        public Handler(ScoreSharpContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var single = await _context.Auth_RouterCategory.SingleOrDefaultAsync(x => x.RouterCategoryId == request.RouterCategoryId);

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, request.RouterCategoryId);

            var isExist = await _context.Auth_Router.AnyAsync(x => x.RouterCategoryId == request.RouterCategoryId);

            if (isExist)
                return ApiResponseHelper.此資源已被使用<string>(null, request.RouterCategoryId);

            _context.Auth_RouterCategory.Remove(single);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess(request.RouterCategoryId);
        }
    }
}
