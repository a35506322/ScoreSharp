using ScoreSharp.API.Modules.SetUp.ApplicationCategory.DeleteApplicationCategoryById;

namespace ScoreSharp.API.Modules.SetUp.ApplicationCategory
{
    public partial class ApplicationCategoryController
    {
        /// <summary>
        /// 刪除單筆申請書類別
        /// </summary>
        /// <remarks>
        ///
        /// Sample Router:
        ///
        ///     /ApplicationCategory/DeleteApplicationCategoryById/AP0008
        ///
        /// </remarks>
        /// <param name="code">申請書類別代碼</param>
        /// <returns></returns>
        [HttpDelete("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除申請書類別_2000_ResEx),
            typeof(刪除申請書類別查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteApplicationCategoryById")]
        public async Task<IResult> DeleteApplicationCategoryById([FromRoute] string code)
        {
            var result = await _mediator.Send(new Command(code));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.ApplicationCategory.DeleteApplicationCategoryById
{
    public record Command(string code) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;

        public Handler(ScoreSharpContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var entity = await _context.SetUp_ApplicationCategory.SingleOrDefaultAsync(x => x.ApplicationCategoryCode == request.code);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, request.code);

            await _context
                .SetUp_ApplicationCategory_Card.Where(x => x.ApplicationCategoryCode == entity.ApplicationCategoryCode)
                .ExecuteDeleteAsync();
            _context.Remove(entity);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess(request.code);
        }
    }
}
