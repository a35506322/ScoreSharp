using ScoreSharp.API.Modules.SetUp.TemplateFixContent.DeleteTemplateFixContentById;

namespace ScoreSharp.API.Modules.SetUp.TemplateFixContent
{
    public partial class TemplateFixContentController
    {
        /// <summary>
        ///  刪除單筆樣板固定值
        /// </summary>
        /// <remarks>
        ///  Sample Router:
        ///
        ///         /TemplateFixContent/DeleteTemplateFixContentById/6
        ///
        /// </remarks>
        /// <param name="seqNo">PK</param>
        /// <returns></returns>
        [HttpDelete("{seqNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除單筆樣板固定值_2000_ResEx),
            typeof(刪除單筆樣板固定值查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteTemplateFixContentById")]
        public async Task<IResult> DeleteTemplateFixContentById([FromRoute] long seqNo)
        {
            var result = await _mediator.Send(new Command(seqNo));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.TemplateFixContent.DeleteTemplateFixContentById
{
    public record Command(long seqNo) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var single = await context.SetUp_TemplateFixContent.SingleOrDefaultAsync(x => x.SeqNo == request.seqNo);

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, request.seqNo.ToString());

            context.Remove(single);
            await context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess(request.seqNo.ToString());
        }
    }
}
