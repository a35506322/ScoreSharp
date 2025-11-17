using ScoreSharp.API.Modules.SetUp.TemplateFixContent.GetTemplateFixContentById;

namespace ScoreSharp.API.Modules.SetUp.TemplateFixContent
{
    public partial class TemplateFixContentController
    {
        /// <summary>
        ///  查詢單筆樣板固定值
        /// </summary>
        /// <remarks>
        ///  Sample Router :
        ///
        ///         /TemplateFixContent/GetTemplateFixContentById/A01
        ///
        /// </remarks>
        /// <params name="seqNo">PK</params>
        /// <returns></returns>
        [HttpGet("{seqNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetTemplateFixContentByIdResponse>))]
        [EndpointSpecificExample(
            typeof(取得單筆樣板固定值_2000_ResEx),
            typeof(取得單筆樣板固定值查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetTemplateFixContentById")]
        public async Task<IResult> GetTemplateFixContentById([FromRoute] long seqNo)
        {
            var result = await _mediator.Send(new Query(seqNo));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.TemplateFixContent.GetTemplateFixContentById
{
    public record Query(long seqNo) : IRequest<ResultResponse<GetTemplateFixContentByIdResponse>>;

    public class Handler(IScoreSharpDapperContext context) : IRequestHandler<Query, ResultResponse<GetTemplateFixContentByIdResponse>>
    {
        public async Task<ResultResponse<GetTemplateFixContentByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await GetTemplateFixContent(request.seqNo);

            if (result is null)
                return ApiResponseHelper.NotFound<GetTemplateFixContentByIdResponse>(null, request.seqNo.ToString());

            return ApiResponseHelper.Success(result);
        }

        private async Task<GetTemplateFixContentByIdResponse> GetTemplateFixContent(long seqNo)
        {
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
                           WHERE A.SeqNo = @SeqNo;";

            using var conn = context.CreateScoreSharpConnection();
            var result = await conn.QueryFirstOrDefaultAsync<GetTemplateFixContentByIdResponse>(sql, new { SeqNo = seqNo });

            return result;
        }
    }
}
