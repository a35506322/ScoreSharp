using ScoreSharp.API.Modules.SetUp.Template.DeleteTemplateById;

namespace ScoreSharp.API.Modules.SetUp.Template
{
    public partial class TemplateController
    {
        /// <summary>
        ///  刪除單筆樣板
        /// </summary>
        /// <remarks>
        ///  Sample Router:
        ///
        ///         /Template/DeleteTemplateById/Z99
        ///
        /// </remarks>
        /// <param name="templateId">PK</param>
        /// <returns></returns>
        [HttpDelete("{templateId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除單筆樣板_2000_ResEx),
            typeof(刪除單筆樣板查無此資料_4001_ResEx),
            typeof(刪除單筆樣板由此資源已被使用_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteTemplateById")]
        public async Task<IResult> DeleteTemplateById([FromRoute] string templateId)
        {
            var result = await _mediator.Send(new Command(templateId));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.Template.DeleteTemplateById
{
    public record Command(string templateId) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var single = await context.SetUp_Template.SingleOrDefaultAsync(x => x.TemplateId == request.templateId);

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, request.templateId);

            var isExist = await context.SetUp_TemplateFixContent.AnyAsync(x => x.TemplateId == request.templateId);

            if (isExist)
                return ApiResponseHelper.此資源已被使用<string>(null, request.templateId);

            context.Remove(single);
            await context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess(request.templateId);
        }
    }
}
