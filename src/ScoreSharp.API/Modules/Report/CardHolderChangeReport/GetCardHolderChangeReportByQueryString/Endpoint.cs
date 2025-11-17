using ScoreSharp.API.Modules.Report.CardHolderChangeReport.GetCardHolderChangeReportByQueryString;
using ReportType = ScoreSharp.API.Modules.Report.CardHolderChangeReport.GetCardHolderChangeReportByQueryString.ReportType;

namespace ScoreSharp.API.Modules.Report.CardHolderChangeReport
{
    public partial class CardHolderChangeReportController
    {
        /// <summary>
        /// 取得/匯出正卡人附卡人資料變更報表 By QueryString
        /// </summary>
        /// <remarks>
        /// Sample QueryString (查詢):
        ///
        ///     ?Type=1&amp;ApplyNo=20241104B0001&amp;StartDate=2024/11/01&amp;EndDate=2024/11/30
        ///
        /// Sample QueryString (匯出):
        ///
        ///     ?Type=2&amp;StartDate=2024/11/01&amp;EndDate=2024/11/30
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetCardHolderChangeReportByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得正卡人附卡人資料變更報表_2000_ResEx),
            typeof(匯出正卡人附卡人資料變更報表Excel_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetCardHolderChangeReportByQueryString")]
        public async Task<IResult> GetCardHolderChangeReportByQueryString([FromQuery] GetCardHolderChangeReportByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));

            // 根據 Type 決定回傳格式
            if (request.Type == ReportType.Export)
            {
                var exportData = result.ReturnData as ExportCardHolderChangeReportResponse;
                if (exportData != null)
                {
                    return Results.File(
                        exportData.FileContent,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        exportData.FileName
                    );
                }
            }

            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Report.CardHolderChangeReport.GetCardHolderChangeReportByQueryString
{
    public record Query(GetCardHolderChangeReportByQueryStringRequest request) : IRequest<ResultResponse<object>>;

    public class Handler(IScoreSharpDapperContext scoreSharpDapperContext, IMiniExcelHelper miniExcelHelper)
        : IRequestHandler<Query, ResultResponse<object>>
    {
        public async Task<ResultResponse<object>> Handle(Query request, CancellationToken cancellationToken)
        {
            var req = request.request;

            // 執行查詢
            var data = await QueryData(req);

            // 根據 Type 決定回傳格式
            if (req.Type == ReportType.Export)
            {
                return ApiResponseHelper.Success<object>(await ExportToExcel(data));
            }
            else
            {
                return ApiResponseHelper.Success<object>(data);
            }
        }

        private async Task<List<CardHolderChangeReportDto>> QueryData(GetCardHolderChangeReportByQueryStringRequest request)
        {
            using var conn = scoreSharpDapperContext.CreateScoreSharpConnection();

            var sql =
                @"
                SELECT
                    SeqNo,
                    ApplyNo,
                    UserType,
                    SupplementaryID,
                    ChangeDateTime,
                    ChangeUserId,
                    ChangeUserName,
                    ChangeSource,
                    ChangeAPIEndpoint,
                    BeforeMobile,
                    AfterMobile,
                    BeforeEmail,
                    AfterEmail,
                    BeforeBillAddress,
                    AfterBillAddress,
                    BeforeSendCardAddress,
                    AfterSendCardAddress
                FROM
                    [dbo].[Reviewer_CardHolderChangeLog]
                WHERE
                    1 = 1
                    AND (@ApplyNo IS NULL OR ApplyNo = @ApplyNo)
                    AND (@ChangeUserId IS NULL OR ChangeUserId = @ChangeUserId)
                    AND (@StartDate IS NULL OR ChangeDateTime >= @StartDate)
                    AND (@EndDate IS NULL OR ChangeDateTime <= @EndDate)
                    AND (@UserType IS NULL OR UserType = @UserType)
                    AND (
                        BeforeMobile IS NOT NULL
                        OR BeforeEmail IS NOT NULL
                        OR BeforeBillAddress IS NOT NULL
                        OR BeforeSendCardAddress IS NOT NULL
                    )
                ORDER BY
                    ChangeDateTime DESC
                ";

            var result = await conn.QueryAsync<CardHolderChangeReportDto>(
                sql,
                new
                {
                    ApplyNo = string.IsNullOrEmpty(request.ApplyNo) ? null : request.ApplyNo,
                    ChangeUserId = string.IsNullOrEmpty(request.ChangeUserId) ? null : request.ChangeUserId,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    UserType = request.UserType,
                }
            );

            return result.ToList();
        }

        private async Task<ExportCardHolderChangeReportResponse> ExportToExcel(List<CardHolderChangeReportDto> data)
        {
            // 轉換為 Excel 匯出格式
            var exportData = data.Select(x => new ExportToExcelDto
                {
                    申請書編號 = x.ApplyNo,
                    卡別類型 = x.UserType == UserType.正卡人 ? "正卡" : "附卡",
                    附卡人身分證 = x.SupplementaryID ?? "",
                    更改帳號 = x.ChangeUserId,
                    更改人員 = x.ChangeUserName ?? "",
                    更改時間 = x.ChangeDateTime.ToString("yyyy/MM/dd HH:mm:ss"),
                    更改來源 = x.ChangeSource ?? "",
                    更改API端點 = x.ChangeAPIEndpoint ?? "",
                    更改前行動電話 = x.BeforeMobile ?? "",
                    更改後行動電話 = x.AfterMobile ?? "",
                    更改前Email = x.BeforeEmail ?? "",
                    更改後Email = x.AfterEmail ?? "",
                    更改前帳單地址 = x.BeforeBillAddress ?? "",
                    更改後帳單地址 = x.AfterBillAddress ?? "",
                    更改前寄卡地址 = x.BeforeSendCardAddress ?? "",
                    更改後寄卡地址 = x.AfterSendCardAddress ?? "",
                })
                .ToList();

            MemoryStream memoryStream = await miniExcelHelper.基本匯出ExcelToStream(exportData);

            return new ExportCardHolderChangeReportResponse
            {
                FileContent = memoryStream.ToArray(),
                FileName = $"正卡人附卡人資料變更報表_{DateTime.Now:yyyyMMddHHmm}.xlsx",
            };
        }
    }
}
