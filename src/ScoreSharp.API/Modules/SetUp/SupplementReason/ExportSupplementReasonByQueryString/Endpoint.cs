using Microsoft.IdentityModel.Tokens;
using MiniExcelLibs;
using ScoreSharp.API.Modules.SetUp.SupplementReason.ExportSupplementReasonByQueryString;

namespace ScoreSharp.API.Modules.SetUp.SupplementReason
{
    public partial class SupplementReasonController
    {
        /// <summary>
        ///  匯出多筆補件原因
        /// </summary>
        /// <remarks>
        ///
        /// Sample QueryString :
        ///
        ///     ?IsActive=Y&amp;supplementReasonName=證明&amp;supplementReasonCode
        ///
        /// </remarks>
        /// <params name="request"></params>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<ExportSupplementReasonByQueryStringResponse>))]
        [EndpointSpecificExample(
            typeof(匯出補件原因Excel_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("ExportSupplementReasonByQueryString")]
        public async Task<IResult> ExportSupplementReasonByQueryString([FromQuery] SupplementReasonByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.SupplementReason.ExportSupplementReasonByQueryString
{
    public record Query(SupplementReasonByQueryStringRequest supplementReasonByQueryStringRequest)
        : IRequest<ResultResponse<ExportSupplementReasonByQueryStringResponse>>;

    public class Handler : IRequestHandler<Query, ResultResponse<ExportSupplementReasonByQueryStringResponse>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMiniExcelHelper _miniExcelHelper;

        public Handler(ScoreSharpContext context, IMiniExcelHelper miniExcelHelper)
        {
            _context = context;
            _miniExcelHelper = miniExcelHelper;
        }

        public async Task<ResultResponse<ExportSupplementReasonByQueryStringResponse>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var filter = request.supplementReasonByQueryStringRequest;

            var exportDto = await _context
                .SetUp_SupplementReason.Where(x => string.IsNullOrEmpty(filter.IsActive) || x.IsActive == filter.IsActive)
                .Where(x => string.IsNullOrEmpty(filter.SupplementReasonCode) || x.SupplementReasonCode == filter.SupplementReasonCode)
                .Where(x =>
                    string.IsNullOrEmpty(filter.SupplementReasonName) || x.SupplementReasonName.Contains(filter.SupplementReasonName)
                )
                .Select(x => new SupplementReasonByQueryStringExportDto()
                {
                    IsActive = x.IsActive,
                    SupplementReasonCode = x.SupplementReasonCode,
                    SupplementReasonName = x.SupplementReasonName,
                })
                .ToListAsync();

            MemoryStream memoryStream = await _miniExcelHelper.基本匯出ExcelToStream(exportDto);

            var result = new ExportSupplementReasonByQueryStringResponse
            {
                FileName = "補件原因.xlsx",
                FileContent = memoryStream.ToArray(),
            };

            return ApiResponseHelper.Success(result);
        }
    }
}
