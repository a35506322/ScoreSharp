using ScoreSharp.API.Modules.SetUp.ProjectCode.DeleteProjectCodeById;

namespace ScoreSharp.API.Modules.SetUp.ProjectCode
{
    public partial class ProjectCodeController
    {
        /// <summary>
        /// 刪除單筆專案代號
        /// </summary>
        /// <remarks>
        ///
        /// Sample Router:
        ///
        ///     /ProjectCode/DeleteProjectCodeById/202
        ///
        /// </remarks>
        /// <param name="code">專案代號代碼</param>
        /// <returns></returns>
        [HttpDelete("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除專案代號_2000_ResEx),
            typeof(刪除專案代號查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteProjectCodeById")]
        public async Task<IResult> DeleteProjectCodeById([FromRoute] string code)
        {
            var result = await _mediator.Send(new Command(code));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.ProjectCode.DeleteProjectCodeById
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
            var single = await _context.SetUp_ProjectCode.SingleOrDefaultAsync(x => x.ProjectCode == request.code);

            if (single is null)
                return ApiResponseHelper.NotFound<string>(request.code, request.code.ToString());

            _context.SetUp_ProjectCode.Remove(single);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess<string>(request.code);
        }
    }
}
