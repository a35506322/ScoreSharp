using System.Text;
using System.Transactions;
using DocumentFormat.OpenXml.InkML;
using QRCoder;
using TemplateEngine.Docx;

namespace ScoreSharp.Batch.Jobs.SupplementTemplateReport;

[Queue("batch")]
[AutomaticRetry(Attempts = 0)]
[Tag("報表作業_補件函批次")]
[WorkdayCheck]
public class SupplementTemplateReportJob(
    ScoreSharpContext _context,
    IScoreSharpDapperContext _dapperContext,
    IOptions<ReportPath> _sharePath,
    IOptions<TemplateReportOption> _templateReport,
    ILogger<SupplementTemplateReportJob> _logger
)
{
    [DisplayName("報表作業_補件函批次 - 執行人員：{0}")]
    public async Task Execute(string createBy)
    {
        var systemBatchSet = await _context.SysParamManage_BatchSet.AsNoTracking().SingleOrDefaultAsync();
        if (systemBatchSet.SupplementTemplateReport_IsEnabled == "N")
        {
            _logger.LogInformation("系統參數設定不執行【報表作業_補件函批次】排程，執行結束");
            return;
        }

        try
        {
            _logger.LogInformation("開始執行補件函批次報表");

            _logger.LogInformation("取得補件報表作業的案件資料");
            var peddingCase = await GetPeddingSupplementCaseAsync();

            if (!peddingCase.Any())
            {
                _logger.LogInformation("無補件報表作業的案件");
                return;
            }

            var applyNos = peddingCase.Select(x => x.ApplyNo).ToList();
            _logger.LogInformation("需產生補件報表作業的案件編號：{applyNos}", string.Join(",", applyNos));

            _logger.LogInformation("取得補件代碼並轉換為補件原因");
            var supplementReasonNamesDic = await GetSupplementReasonDicAsync();
            peddingCase.ForEach(x =>
            {
                x.SupplementReasonNames = x
                    .SupplementReasonCode.Split(',')
                    .Select(code => supplementReasonNamesDic.GetValueOrDefault(code, ""))
                    .ToArray();
            });

            _logger.LogInformation("取得樣板固定值設定");
            var supplementFixContent = await GetTemplateSettingsAsync();

            _logger.LogInformation("產生補件函簽名報表");
            var supplementSignReport = GenerateSupplementSignReport(peddingCase, supplementFixContent);

            _logger.LogInformation("儲存補件函簽名報表至指定路徑");
            string reportSignName = $"補件函簽名{DateTime.Now:yyyyMMddHHmm}.docx";
            string reportSignFullPath = Path.Combine(_sharePath.Value.SupplementSignReportPath, reportSignName);
            var (isSaveSupplementSignReportSuccess, supplementSignReportErrorMsg) = SaveSupplementSignReport(
                supplementSignReport,
                reportSignFullPath
            );

            _logger.LogInformation("產生補件函不包含簽名報表");
            var supplementNoSignReport = GenerateSupplementNoSignReport(peddingCase);

            _logger.LogInformation("儲存補件函(不包含簽名函)報表至指定路徑");
            string reportNoSignName = $"補件函不含簽名{DateTime.Now:yyyyMMddHHmm}.txt";
            string reportNoSignFullPath = Path.Combine(_sharePath.Value.SupplementNoSignReportPath, reportNoSignName);
            var (isSaveSupplementNoSignReportSuccess, supplementNoSignReportErrorMsg) = SaveSupplementNoSignReport(
                supplementNoSignReport,
                reportNoSignFullPath
            );

            if (isSaveSupplementNoSignReportSuccess && isSaveSupplementSignReportSuccess)
            {
                _logger.LogInformation("儲存補件函報表成功");
            }
            else
            {
                List<string> errorMsgs = new List<string>();
                if (!isSaveSupplementNoSignReportSuccess)
                {
                    errorMsgs.Add($"儲存補件函(不包含簽名函)報表時發生錯誤，{reportNoSignFullPath}，錯誤訊息：{supplementNoSignReportErrorMsg}");
                }

                if (!isSaveSupplementSignReportSuccess)
                {
                    errorMsgs.Add($"儲存補件函(包含簽名函)報表時發生錯誤，{reportSignFullPath}，錯誤訊息：{supplementSignReportErrorMsg}");
                }

                _logger.LogError(string.Join(Environment.NewLine, errorMsgs));

                await _context.System_ErrorLog.AddAsync(
                    new System_ErrorLog
                    {
                        Project = "Batch",
                        Source = "SupplementTemplateReportJob",
                        Type = "Error",
                        ErrorMessage = string.Join(Environment.NewLine, errorMsgs),
                        AddTime = DateTime.Now,
                        SendStatus = SendStatus.等待,
                    }
                );
                await _context.SaveChangesAsync();
                return;
            }

            _logger.LogInformation("開始儲存資料庫資料");

            string signSeqNo = Ulid.NewUlid().ToString();
            string noSignSeqNo = Ulid.NewUlid().ToString();
            var now = DateTime.Now;

            List<Report_BatchReportDownload> reportBatchReportDownloads = new List<Report_BatchReportDownload>
            {
                new Report_BatchReportDownload
                {
                    SeqNo = signSeqNo,
                    ReportName = reportSignName,
                    ReportType = ReportType.補件函_包含簽名函,
                    ReportFullAddr = reportSignFullPath,
                    AddTime = now,
                },
                new Report_BatchReportDownload
                {
                    SeqNo = noSignSeqNo,
                    ReportName = reportNoSignName,
                    ReportType = ReportType.補件函_不包含簽名函,
                    ReportFullAddr = reportNoSignFullPath,
                    AddTime = now,
                },
            };

            var updateChangeStatusDtos = peddingCase
                .Select(x => new UpdateChangeStatusDto { HandleSeqNo = x.HandleSeqNo, BatchSupplementTime = now })
                .ToList();

            var isSaveDataToDBSuccess = await SaveDataToDB(reportBatchReportDownloads, updateChangeStatusDtos);
            if (isSaveDataToDBSuccess)
            {
                _logger.LogInformation("儲存資料庫成功 (Report_BatchReportDownload, Reviewer_CaseChangeDetail)");
            }
            else
            {
                _logger.LogError("儲存資料庫失敗 (Report_BatchReportDownload, Reviewer_CaseChangeDetail)");
            }

            _logger.LogInformation("執行補件批次報表完成");
        }
        catch (Exception ex)
        {
            _logger.LogError("執行補件批次報表時發生錯誤 {@ex}", ex);
            await _context.System_ErrorLog.AddAsync(
                new System_ErrorLog
                {
                    Project = "Batch",
                    Source = "SupplementTemplateReportJob",
                    Type = "Error",
                    ErrorMessage = $"執行補件批次報表時發生錯誤: {ex.Message}",
                    ErrorDetail = ex.ToString(),
                    AddTime = DateTime.Now,
                    SendStatus = SendStatus.等待,
                }
            );
            await _context.SaveChangesAsync();
        }
    }

    private async Task<List<PeddingSupplementCaseDto>> GetPeddingSupplementCaseAsync()
    {
        string sql =
            @"SELECT H.[ApplyNo],
                    H.[ID],
                    H.[UserType],
                    H.[CardStatus],
                    H.[BatchSupplementStatus],
                    H.[BatchSupplementTime],
                    H.SupplementReasonCode,
                    H.SupplementSendCardAddr,
                    M.CHName,
                    M.Mobile,
                    CASE H.SupplementSendCardAddr
                        WHEN 1 THEN M.Bill_ZipCode
                        WHEN 2 THEN M.Reg_ZipCode
                        WHEN 3 THEN M.Comp_ZipCode
                        WHEN 4 THEN M.Live_ZipCode
                    END AS 'ZipCode',
                    CASE H.SupplementSendCardAddr
                        WHEN 1 THEN M.Bill_City
                        WHEN 2 THEN M.Reg_City
                        WHEN 3 THEN M.Comp_City
                        WHEN 4 THEN M.Live_City
                    END AS 'City',
                    CASE H.SupplementSendCardAddr
                        WHEN 1 THEN M.Bill_District
                        WHEN 2 THEN M.Reg_District
                        WHEN 3 THEN M.Comp_District
                        WHEN 4 THEN M.Live_District
                    END AS 'District',
                    CASE H.SupplementSendCardAddr
                        WHEN 1 THEN M.Bill_Road
                        WHEN 2 THEN M.Reg_Road
                        WHEN 3 THEN M.Comp_Road
                        WHEN 4 THEN M.Live_Road
                    END AS 'Road',
                    CASE H.SupplementSendCardAddr
                        WHEN 1 THEN M.Bill_Lane
                        WHEN 2 THEN M.Reg_Lane
                        WHEN 3 THEN M.Comp_Lane
                        WHEN 4 THEN M.Live_Lane
                    END AS 'Lane',
                    CASE H.SupplementSendCardAddr
                        WHEN 1 THEN M.Bill_Alley
                        WHEN 2 THEN M.Reg_Alley
                        WHEN 3 THEN M.Comp_Alley
                        WHEN 4 THEN M.Live_Alley
                    END AS 'Alley',
                    CASE H.SupplementSendCardAddr
                        WHEN 1 THEN M.Bill_Number
                        WHEN 2 THEN M.Reg_Number
                        WHEN 3 THEN M.Comp_Number
                        WHEN 4 THEN M.Live_Number
                    END AS 'Number',
                    CASE H.SupplementSendCardAddr
                        WHEN 1 THEN M.Bill_SubNumber
                        WHEN 2 THEN M.Reg_SubNumber
                        WHEN 3 THEN M.Comp_SubNumber
                        WHEN 4 THEN M.Live_SubNumber
                    END AS 'SubNumber',
                    CASE H.SupplementSendCardAddr
                        WHEN 1 THEN M.Bill_Floor
                        WHEN 2 THEN M.Reg_Floor
                        WHEN 3 THEN M.Comp_Floor
                        WHEN 4 THEN M.Live_Floor
                    END AS 'Floor',
                    CASE H.SupplementSendCardAddr
                        WHEN 1 THEN M.Bill_Other
                        WHEN 2 THEN M.Reg_Other
                        WHEN 3 THEN M.Comp_Other
                        WHEN 4 THEN M.Live_Other
                    END AS 'Other',
                    H.SeqNo as 'HandleSeqNo'
                FROM [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoHandle] H
                JOIN [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoMain] M ON H.ApplyNo = M.ApplyNo
                WHERE H.CardStatus = 10231
                AND H.BatchSupplementStatus = 'N'
                AND H.IsPrintSMSAndPaper = 'Y';";

        using var connection = _dapperContext.CreateScoreSharpConnection();
        var result = await connection.QueryAsync<PeddingSupplementCaseDto>(sql);
        return result.ToList();
    }

    private async Task<Dictionary<string, string>> GetSupplementReasonDicAsync() =>
        await _context.SetUp_SupplementReason.ToDictionaryAsync(x => x.SupplementReasonCode, x => x.SupplementReasonName);

    private async Task<SupplementFixContentDto> GetTemplateSettingsAsync()
    {
        string _supplementTemplateId = _templateReport.Value.SupplementTemplateId;

        var dict = await _context
            .SetUp_TemplateFixContent.Where(t => t.TemplateId == _supplementTemplateId && t.IsActive == "Y")
            .ToDictionaryAsync(t => t.TemplateKey, t => t.TemplateValue);

        return new SupplementFixContentDto(
            cardUrl: dict.GetValueOrDefault("CardUrl", ""),
            email: dict.GetValueOrDefault("Email", ""),
            ext: dict.GetValueOrDefault("Ext", ""),
            servicePhone: dict.GetValueOrDefault("ServicePhone", ""),
            qrCodeUrl: dict.GetValueOrDefault("QrCodeUrl", "")
        );
    }

    private (bool isSuccess, string errorMsg) SaveSupplementSignReport(Content content, string reportFullPath)
    {
        try
        {
            const string TEMPLATE_FILE_NAME = "補件函含簽名Template.docx";
            string templateFile = Path.Combine(Directory.GetCurrentDirectory(), "Files", TEMPLATE_FILE_NAME);
            _logger.LogInformation("範本路徑：{templateFile}", templateFile);

            using (var sourceStream = File.OpenRead(templateFile))
            using (var targetStream = File.Create(reportFullPath))
            {
                sourceStream.CopyTo(targetStream);
                targetStream.Position = 0;

                // 處理文件
                using (var outputDocument = new TemplateProcessor(targetStream).SetRemoveContentControls(true))
                {
                    outputDocument.FillContent(content);
                    outputDocument.SaveChanges();
                }
            }

            return (true, null);
        }
        catch (Exception ex)
        {
            _logger.LogError("儲存補件函(包含簽名函)報表時發生錯誤 {@ex}", ex);
            return (false, ex.ToString());
        }
    }

    private Content GenerateSupplementSignReport(List<PeddingSupplementCaseDto> peddingCase, SupplementFixContentDto fixContent)
    {
        // 取得民國年月日
        var today = DateTime.Now;
        var taiwanCalendar = new TaiwanCalendar();
        var year = taiwanCalendar.GetYear(today);
        var month = taiwanCalendar.GetMonth(today);

        var day = taiwanCalendar.GetDayOfMonth(today);

        // 產生 QR Code
        var qrCodeImage = ConvertQrCode(fixContent.QrCodeUrl);

        // 準備重複內容
        var repeatContent = new RepeatContent("@repeat");

        foreach (var pedding in peddingCase)
        {
            var fields = new List<IContentItem>
            {
                // 上文
                new FieldContent("@zipCode", pedding.ZipCode),
                new FieldContent("@fullAddr", ConvertFullAddr(pedding)),
                new FieldContent("@chName", pedding.CHName),
                new FieldContent("@applyNo", pedding.ApplyNo),
                // 補件原因
                new FieldContent("@supplementReasonNames", string.Join(Environment.NewLine, pedding.SupplementReasonNames.Select(r => $"．{r}"))),
                // QR Code
                new ImageContent("@qrCodeUrl", qrCodeImage),
                // 產生日期
                new FieldContent("@year", year.ToString()),
                new FieldContent("@month", month.ToString("D2")),
                new FieldContent("@day", day.ToString("D2")),
                // 下文
                new FieldContent("@cardUrl", fixContent.CardUrl),
                new FieldContent("@email", fixContent.Email),
                new FieldContent("@ext", fixContent.Ext),
                new FieldContent("@servicePhone", fixContent.ServicePhone),
            };
            repeatContent.AddItem(fields.ToArray());
        }

        var fillContent = new Content(repeatContent);
        return fillContent;
    }

    private string ConvertFullAddr(PeddingSupplementCaseDto caseDetail)
    {
        StringBuilder fullAddr = new StringBuilder();
        if (!string.IsNullOrEmpty(caseDetail.City))
        {
            fullAddr.Append(caseDetail.City);
        }

        if (!string.IsNullOrEmpty(caseDetail.District))
        {
            fullAddr.Append(caseDetail.District);
        }

        if (!string.IsNullOrEmpty(caseDetail.Road))
        {
            fullAddr.Append(caseDetail.Road + "路");
        }

        if (!string.IsNullOrEmpty(caseDetail.Lane))
        {
            fullAddr.Append(caseDetail.Lane + "巷");
        }

        if (!string.IsNullOrEmpty(caseDetail.Alley))
        {
            fullAddr.Append(caseDetail.Alley + "弄");
        }

        if (!string.IsNullOrEmpty(caseDetail.Number))
        {
            fullAddr.Append(caseDetail.Number + "號");
        }

        if (!string.IsNullOrEmpty(caseDetail.SubNumber))
        {
            fullAddr.Append("之" + caseDetail.SubNumber);
        }

        if (!string.IsNullOrEmpty(caseDetail.Floor))
        {
            fullAddr.Append(caseDetail.Floor + "樓");
        }

        if (!string.IsNullOrEmpty(caseDetail.Other))
        {
            fullAddr.Append(caseDetail.Other + "號");
        }

        return fullAddr.ToString();
    }

    private byte[] ConvertQrCode(string url)
    {
        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrCodeData);
        return qrCode.GetGraphic(20);
    }

    private string GenerateSupplementNoSignReport(List<PeddingSupplementCaseDto> caseDetails)
    {
        string noticeMsg = String.Join(
            Environment.NewLine,
            caseDetails.Select(x => $"{x.CHName}|{(!String.IsNullOrEmpty(x.Mobile) ? x.Mobile : String.Empty)}")
        );
        return noticeMsg;
    }

    private (bool isSuccess, string errorMsg) SaveSupplementNoSignReport(string reportContent, string reportFullPath)
    {
        try
        {
            File.WriteAllText(reportFullPath, reportContent);
            return (true, null);
        }
        catch (Exception ex)
        {
            _logger.LogError("儲存補件函(不包含簽名函)報表時發生錯誤 {@ex}", ex);
            return (false, ex.ToString());
        }
    }

    private async Task<bool> SaveDataToDB(
        List<Report_BatchReportDownload> reportBatchReportDownloads,
        List<UpdateChangeStatusDto> updateChangeStatusDtos
    )
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                using var conn = _dapperContext.CreateScoreSharpConnection();
                string insertReportBatchDownloadSql =
                    @"INSERT INTO [ScoreSharp].[dbo].[Report_BatchReportDownload]
                                            ([SeqNo]
                                            ,[ReportName]
                                            ,[ReportType]
                                            ,[ReportFullAddr]
                                            ,[AddTime]
                                            ,[LastDownloadUserId])

                                        VALUES
                                            (@SeqNo
                                            ,@ReportName
                                            ,@ReportType
                                            ,@ReportFullAddr
                                            ,@AddTime
                                            ,@LastDownloadUserId)";

                await conn.ExecuteAsync(insertReportBatchDownloadSql, reportBatchReportDownloads);

                string updateChangeStatusSql =
                    @"Update [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoHandle]
                        Set   [BatchSupplementStatus] = 'Y',
                            [BatchSupplementTime] = @BatchSupplementTime
                        Where [SeqNo] = @HandleSeqNo;";
                await conn.ExecuteAsync(updateChangeStatusSql, updateChangeStatusDtos);

                scope.Complete();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("資料庫更新資料失敗 {@ex}", ex);
                return false;
            }
        }
    }
}
