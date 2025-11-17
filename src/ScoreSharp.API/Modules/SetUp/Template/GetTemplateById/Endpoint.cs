using ScoreSharp.API.Modules.SetUp.Template.GetTemplateById;

namespace ScoreSharp.API.Modules.SetUp.Template
{
    public partial class TemplateController
    {
        /// <summary>
        ///  查詢單筆樣板
        /// </summary>
        /// <remarks>
        ///  Sample Router :
        ///
        ///         /Template/GetTemplateById/A01
        ///
        /// </remarks>
        /// <params name="templateId">PK</params>
        /// <returns></returns>
        [HttpGet("{templateId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetTemplateByIdResponse>))]
        [EndpointSpecificExample(
            typeof(取得單筆樣板_2000_ResEx),
            typeof(取得單筆樣板查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetTemplateById")]
        public async Task<IResult> GetTemplateById([FromRoute] string templateId)
        {
            var result = await _mediator.Send(new Query(templateId));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.Template.GetTemplateById
{
    public record Query(string templateId) : IRequest<ResultResponse<GetTemplateByIdResponse>>;

    public class Handler(ScoreSharpContext context, IMapper mapper) : IRequestHandler<Query, ResultResponse<GetTemplateByIdResponse>>
    {
        public async Task<ResultResponse<GetTemplateByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var single = await context.SetUp_Template.AsNoTracking().SingleOrDefaultAsync(x => x.TemplateId == request.templateId);

            if (single is null)
                return ApiResponseHelper.NotFound<GetTemplateByIdResponse>(null, request.templateId);

            var result = mapper.Map<GetTemplateByIdResponse>(single);

            return ApiResponseHelper.Success(result);
        }
    }
}
