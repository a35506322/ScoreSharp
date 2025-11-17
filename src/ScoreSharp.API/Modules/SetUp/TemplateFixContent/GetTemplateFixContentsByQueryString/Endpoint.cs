using ScoreSharp.API.Modules.SetUp.TemplateFixContent.GetTemplateFixContentsByQueryString;

namespace ScoreSharp.API.Modules.SetUp.TemplateFixContent
{
    public partial class TemplateFixContentController
    {
        /// <summary>
        ///  查詢樣板固定值
        /// </summary>
        /// <remarks>
        ///
        ///  Sample QueryString :
        ///
        ///     ?IsActive=Y&amp;TemplateId=E30
        ///
        /// </remarks>
        /// <params name="request"></params>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetTemplateFixContentsByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(查詢樣板固定值_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetTemplateFixContentsByQueryString")]
        public async Task<IResult> GetTemplateFixContentsByQueryString([FromQuery] GetTemplateFixContentsByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.TemplateFixContent.GetTemplateFixContentsByQueryString
{
    public record Query(GetTemplateFixContentsByQueryStringRequest getTemplateFixContentsByQueryStringRequest)
        : IRequest<ResultResponse<List<GetTemplateFixContentsByQueryStringResponse>>>;

    public class Handler(IScoreSharpDapperContext context)
        : IRequestHandler<Query, ResultResponse<List<GetTemplateFixContentsByQueryStringResponse>>>
    {
        public async Task<ResultResponse<List<GetTemplateFixContentsByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var dto = request.getTemplateFixContentsByQueryStringRequest;

            var response = await GetAllTemplateFixContents(request);

            return ApiResponseHelper.Success(response.ToList());
        }

        private async Task<IEnumerable<GetTemplateFixContentsByQueryStringResponse>> GetAllTemplateFixContents(Query request)
        {
            var dto = request.getTemplateFixContentsByQueryStringRequest;

            string sql =
                @"SELECT A.[SeqNo]
                                 ,A.[TemplateId]
                                 ,A.[TemplateKey]
                                 ,A.[TemplateValue]
                                 ,A.[AddUserId]
                                 ,A.[AddTime]
                                 ,A.[UpdateUserId]
                                 ,A.[UpdateTime]
                                 ,A.[IsActive]
                                 ,B.[TemplateName]
                           FROM SetUp_TemplateFixContent AS A
                           JOIN SetUp_Template AS B
                           ON A.TemplateId = B.TemplateId
                           WHERE (@IsActive IS NULL OR A.IsActive = @IsActive)
                             AND (@TemplateId IS NULL OR A.TemplateId = @TemplateId);";

            using var conn = context.CreateScoreSharpConnection();
            var result = await conn.QueryAsync<GetTemplateFixContentsByQueryStringResponse>(
                sql,
                new
                {
                    IsActive = string.IsNullOrEmpty(dto.IsActive) ? null : dto.IsActive,
                    TemplateId = string.IsNullOrEmpty(dto.TemplateId) ? null : dto.TemplateId,
                }
            );

            return result;
        }
    }
}
