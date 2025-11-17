using System.Data;
using ScoreSharp.API.Modules.Report.SameMobileReport.ExportSameMobileReportByQueryString;

namespace ScoreSharp.API.Modules.Report.SameMobileReport
{
    public partial class SameMobileReportController
    {
        ///<summary>
        /// 匯出線上辦卡手機號碼比對相同報表 ByQueryString
        /// </summary>
        /// <remarks>
        ///
        /// Sample QueryString :
        ///
        ///     ?StartDate=2025/03/06&amp;EndDate=2025/03/07&amp;ComparisonResult=ALL
        ///
        /// </remarks>
        ///<returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<ExportSameMobileReportByQueryStringResponse>))]
        [EndpointSpecificExample(
            typeof(匯出線上辦卡手機號碼比對相同報表Excel_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("ExportSameMobileReportByQueryString")]
        public async Task<IResult> ExportSameMobileReportByQueryString([FromQuery] ExportSameMobileReportByQueryStringRequest request) =>
            Results.Ok(await _mediator.Send(new Query(request)));
    }
}

namespace ScoreSharp.API.Modules.Report.SameMobileReport.ExportSameMobileReportByQueryString
{
    public record Query(ExportSameMobileReportByQueryStringRequest exportSameMobileReportByQueryStringRequest)
        : IRequest<ResultResponse<ExportSameMobileReportByQueryStringResponse>>;

    public class Handler(IScoreSharpDapperContext scoreSharpDapperContext, IMiniExcelHelper miniExcelHelper)
        : IRequestHandler<Query, ResultResponse<ExportSameMobileReportByQueryStringResponse>>
    {
        public async Task<ResultResponse<ExportSameMobileReportByQueryStringResponse>> Handle(
            Query request,
            CancellationToken cancellationToken
        ) => ApiResponseHelper.Success(await ExportSameMobileReportByQueryString(request.exportSameMobileReportByQueryStringRequest));

        private async Task<ExportSameMobileReportByQueryStringResponse> ExportSameMobileReportByQueryString(
            ExportSameMobileReportByQueryStringRequest request
        )
        {
            List<ExportSameMobileReportByQueryStringExportDto> exportDto = new();

            using (var conn = scoreSharpDapperContext.CreateScoreSharpConnection())
            {
                SqlMapper.GridReader results = await conn.QueryMultipleAsync(
                    sql: "Usp_GetSameMobileReport",
                    param: new
                    {
                        startDate = request.StartDate,
                        endDate = request.EndDate,
                        comparisonResult = request.ComparisonResult,
                    },
                    commandType: CommandType.StoredProcedure
                );

                exportDto = results.Read<ExportSameMobileReportByQueryStringExportDto>().ToList();
            }

            var exportData = exportDto
                .Select(x => new ExportToExcelDto
                {
                    ApplyNo = x.ApplyNo,
                    ID = x.ID,
                    CHName = x.CHName,
                    CardStatusName = x.CardStatus.ToString(),
                    CompName = x.CompName,
                    Mobile = x.Mobile,
                    OTPMobile = x.OTPMobile,
                    PromotionUnit = x.PromotionUnit,
                    PromotionUser = x.PromotionUser,
                    SameWebCaseMobileChecked = x.SameWebCaseMobileChecked,
                    SameApplyNo = x.SameApplyNo,
                    SameID = x.SameID,
                    SameName = x.SameName,
                    SameCardStatusName = x.SameCardStatus.ToString(),
                    SameCompName = x.SameCompName,
                    SameOTPMobile = x.SameOTPMobile,
                    IsError = x.IsError,
                    CheckRecord = x.CheckRecord,
                    UpdateUserId = x.UpdateUserId,
                    UpdateTime = x.UpdateTime?.ToString("yyyy/MM/dd HH:mm:ss") ?? null,
                })
                .ToList();

            MemoryStream memoryStream = await miniExcelHelper.基本匯出ExcelToStream(exportData);

            var result = new ExportSameMobileReportByQueryStringResponse
            {
                FileContent = memoryStream.ToArray(),
                FileName = $"線上辦卡手機號碼比對相同報表_{DateTime.Now.ToString("yyyyMMddHHmm")}.xlsx",
            };

            return result;
        }
    }
}
