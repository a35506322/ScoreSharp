using ScoreSharp.API.Modules.Report.BatchReportDownload.GetReportFileBySeqNo;

namespace ScoreSharp.API.Modules.Report.BatchReportDownload
{
    public partial class BatchReportDownloadController
    {
        /// <summary>
        /// 下載報表 By SeqNo
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /BatchReportDownload/GetReportFileBySeqNo/01HRHK6NMVT1234567890
        ///
        /// </remarks>
        [HttpGet("{seqNo}")]
        [OpenApiOperation("GetReportFileBySeqNo")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetReportFileBySeqNoResponse>))]
        [EndpointSpecificExample(
            typeof(下載報表成功_2000_ResEx),
            typeof(下載報表查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        public async Task<IResult> GetReportFileBySeqNo([FromRoute] string seqNo)
        {
            var result = await _mediator.Send(new Command(seqNo));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Report.BatchReportDownload.GetReportFileBySeqNo
{
    public record Command(string SeqNo) : IRequest<ResultResponse<GetReportFileBySeqNoResponse>>;

    public class CommandHandler(ScoreSharpContext scoreSharpContext, IJWTProfilerHelper jwtProfilerHelper)
        : IRequestHandler<Command, ResultResponse<GetReportFileBySeqNoResponse>>
    {
        public async Task<ResultResponse<GetReportFileBySeqNoResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            var report = await scoreSharpContext.Report_BatchReportDownload.FirstOrDefaultAsync(
                x => x.SeqNo == request.SeqNo,
                cancellationToken
            );

            if (report is null)
            {
                return ApiResponseHelper.NotFound<GetReportFileBySeqNoResponse>(null, request.SeqNo);
            }

            report.LastDownloadUserId = jwtProfilerHelper.UserId;
            await scoreSharpContext.SaveChangesAsync(cancellationToken);

            var filePath = report.ReportFullAddr;
            var fileType = GetContentType(Path.GetExtension(filePath));
            var fileBytes = await File.ReadAllBytesAsync(filePath, cancellationToken);

            return ApiResponseHelper.Success(
                new GetReportFileBySeqNoResponse
                {
                    FileContent = fileBytes,
                    FileName = Path.GetFileName(filePath),
                    ContentType = fileType,
                }
            );
        }

        private string GetContentType(string extension)
        {
            return extension.ToLower() switch
            {
                ".pdf" => "application/pdf",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".txt" => "text/plain",
                _ => "application/octet-stream",
            };
        }
    }
}
