using ScoreSharp.API.Modules.SetUp.Template.GetTemplatesByQueryString;

namespace ScoreSharp.API.Modules.SetUp.Template
{
    public partial class TemplateController
    {
        /// <summary>
        ///  查詢多筆樣板
        /// </summary>
        /// <remarks>
        ///
        ///  Sample QueryString :
        ///
        ///     ?IsActive=Y&amp;TemplateId=&amp;TemplateName
        ///
        /// </remarks>
        /// <params name="request"></params>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetTemplatesByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得多筆樣板_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetTemplatesByQueryString")]
        public async Task<IResult> GetTemplatesByQueryString([FromQuery] GetTemplatesByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.Template.GetTemplatesByQueryString
{
    public record Query(GetTemplatesByQueryStringRequest getTemplatesByQueryStringRequest)
        : IRequest<ResultResponse<List<GetTemplatesByQueryStringResponse>>>;

    public class Handler(ScoreSharpContext context, IMapper mapper)
        : IRequestHandler<Query, ResultResponse<List<GetTemplatesByQueryStringResponse>>>
    {
        public async Task<ResultResponse<List<GetTemplatesByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var dto = request.getTemplatesByQueryStringRequest;

            var entity = await context
                .SetUp_Template.Where(x => string.IsNullOrEmpty(dto.IsActive) || x.IsActive == dto.IsActive)
                .Where(x => string.IsNullOrEmpty(dto.TemplateName) || x.TemplateName.Contains(dto.TemplateName))
                .Where(x => string.IsNullOrEmpty(dto.TemplateId) || x.TemplateId == dto.TemplateId)
                .ToListAsync();

            var result = mapper.Map<List<GetTemplatesByQueryStringResponse>>(entity);

            return ApiResponseHelper.Success(result);
        }
    }
}
