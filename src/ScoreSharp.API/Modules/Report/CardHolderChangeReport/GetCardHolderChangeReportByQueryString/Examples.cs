namespace ScoreSharp.API.Modules.Report.CardHolderChangeReport.GetCardHolderChangeReportByQueryString;

[ExampleAnnotation(Name = "[2000]取得正卡人附卡人資料變更報表(查詢)", ExampleType = ExampleType.Response)]
public class 取得正卡人附卡人資料變更報表_2000_ResEx : IExampleProvider<ResultResponse<List<GetCardHolderChangeReportByQueryStringResponse>>>
{
    public ResultResponse<List<GetCardHolderChangeReportByQueryStringResponse>> GetExample()
    {
        var response = new List<GetCardHolderChangeReportByQueryStringResponse>
        {
            new GetCardHolderChangeReportByQueryStringResponse
            {
                SeqNo = 1,
                ApplyNo = "20241104B0001",
                UserType = UserType.正卡人,
                SupplementaryID = null,
                ChangeDateTime = DateTime.Parse("2024-11-04T10:30:00"),
                ChangeUserId = "user123",
                ChangeUserName = "張三",
                ChangeSource = "API",
                ChangeAPIEndpoint = "UpdateApplicationInfoById",
                BeforeMobile = "0912345678",
                AfterMobile = "0987654321",
                BeforeEmail = "old@example.com",
                AfterEmail = "new@example.com",
                BeforeBillAddress = "台北市中正區重慶南路一段122號",
                AfterBillAddress = "台北市信義區市府路1號",
                BeforeSendCardAddress = null,
                AfterSendCardAddress = null,
            },
            new GetCardHolderChangeReportByQueryStringResponse
            {
                SeqNo = 2,
                ApplyNo = "20241104B0002",
                UserType = UserType.附卡人,
                SupplementaryID = "A123456789",
                ChangeDateTime = DateTime.Parse("2024-11-04T14:45:00"),
                ChangeUserId = "user456",
                ChangeUserName = "李四",
                ChangeSource = "API",
                ChangeAPIEndpoint = "UpdateSupplementaryInfoByApplyNo",
                BeforeMobile = null,
                AfterMobile = null,
                BeforeEmail = "supp@example.com",
                AfterEmail = "newsup@example.com",
                BeforeBillAddress = null,
                AfterBillAddress = null,
                BeforeSendCardAddress = "新北市板橋區縣民大道二段7號",
                AfterSendCardAddress = "新北市新店區北新路三段200號",
            },
        };

        return ApiResponseHelper.Success(response);
    }
}

[ExampleAnnotation(Name = "[2000]匯出正卡人附卡人資料變更報表Excel", ExampleType = ExampleType.Response)]
public class 匯出正卡人附卡人資料變更報表Excel_2000_ResEx : IExampleProvider<ResultResponse<ExportCardHolderChangeReportResponse>>
{
    public ResultResponse<ExportCardHolderChangeReportResponse> GetExample()
    {
        var response = new ExportCardHolderChangeReportResponse
        {
            FileName = "正卡人附卡人資料變更報表_202411041530.xlsx",
            FileContent = new byte[] { 0x50, 0x4B, 0x03, 0x04 },
        };

        return ApiResponseHelper.Success(response);
    }
}
