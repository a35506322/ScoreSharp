using ScoreSharp.API.Modules.SetUp.AMLProfession.DeleteAMLProfessionById;

namespace ScoreSharp.API.Modules.SetUp.AMLProfession
{
    public partial class AMLProfessionController
    {
        /// <summary>
        /// 刪除單筆AML職業別
        /// </summary>
        /// <remarks>
        ///
        /// Sample Router:
        ///
        ///     /AMLProfession/DeleteAMLProfessionById/01J57D2F05Q0N06K78X0Z5XKHT
        ///
        /// </remarks>
        /// <param name="code">AML職業別代碼</param>
        /// <returns></returns>
        [HttpDelete("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除AML職業別_2000_ResEx),
            typeof(刪除AML職業別查無此資料_4001_ResEx),
            typeof(刪除AML職業別資源已被使用_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteAMLProfessionById")]
        public async Task<IResult> DeleteAMLProfessionById([FromRoute] string code)
        {
            var result = await _mediator.Send(new Command(code));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.AMLProfession.DeleteAMLProfessionById
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
            var entity = await _context.SetUp_AMLProfession.SingleOrDefaultAsync(x => x.SeqNo == request.code);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(request.code, request.code);

            var allUsingVersion = await _context
                .Reviewer_ApplyCreditCardInfoMain.AsNoTracking()
                .Select(x => x.AMLProfessionCode_Version)
                .Distinct()
                .ToListAsync();
            if (allUsingVersion.Any(x => x == entity.Version))
                return ApiResponseHelper.此資源已被使用<string>("該版本已被使用。", request.code);

            _context.SetUp_AMLProfession.Remove(entity);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess<string>(request.code);
        }
    }
}
