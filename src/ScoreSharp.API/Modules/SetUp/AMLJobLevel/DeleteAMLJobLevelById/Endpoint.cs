using ScoreSharp.API.Modules.SetUp.AMLJobLevel.DeleteAMLJobLevelById;

namespace ScoreSharp.API.Modules.SetUp.AMLJobLevel
{
    public partial class AMLJobLevelController
    {
        /// <summary>
        /// 刪除單筆AML職級別
        /// </summary>
        /// <remarks>
        ///
        /// Sample Router:
        ///
        ///     /AMLJobLevel/DeleteAMLJobLevelById/2
        ///
        /// </remarks>
        /// <param name="code">AML職級別代碼</param>
        /// <returns></returns>
        [HttpDelete("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除AML職級別_2000_ResEx),
            typeof(刪除AML職級別查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteAMLJobLevelById")]
        public async Task<IResult> DeleteAMLJobLevelById([FromRoute] string code)
        {
            var result = await _mediator.Send(new Command(code));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.AMLJobLevel.DeleteAMLJobLevelById
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
            var entity = await _context.SetUp_AMLJobLevel.SingleOrDefaultAsync(x => x.AMLJobLevelCode == request.code);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(request.code, request.code.ToString());

            _context.SetUp_AMLJobLevel.Remove(entity);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess<string>(request.code);
        }
    }
}
