namespace ScoreSharp.PaperMiddleware.Modules.Reviewer.ReviewerCore.SyncApplyFile;

[ExampleAnnotation(Name = "[200][2000]圖檔傳送-紙本初始", ExampleType = ExampleType.Request)]
public class 圖檔傳送_紙本初始_200_2000_ReqEx : IExampleProvider<SyncApplyFileRequest>
{
    public SyncApplyFileRequest GetExample()
    {
        var request = new SyncApplyFileRequest()
        {
            ApplyNo = "20250603X0001",
            SyncStatus = SyncFileStatus.紙本初始,
            SyncUserId = "同步員編",
            ApplyFiles = new ApplyFileDto[]
            {
                new ApplyFileDto
                {
                    FileContent = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 }, // 模擬檔案內容
                    FileName = "file1.pdf",
                    FileId = 1, // 模擬檔案ID
                },
                new ApplyFileDto
                {
                    FileContent = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 }, // 模擬檔案內容
                    FileName = "file2.jpg",
                    FileId = 2, // 模擬檔案ID
                },
            },
        };

        return request;
    }
}

[ExampleAnnotation(Name = "[200][2000]圖檔傳送-紙本補件", ExampleType = ExampleType.Request)]
public class 圖檔傳送_紙本補件_200_2000_ReqEx : IExampleProvider<SyncApplyFileRequest>
{
    public SyncApplyFileRequest GetExample()
    {
        var request = new SyncApplyFileRequest()
        {
            ApplyNo = "20250603X0001",
            SyncStatus = SyncFileStatus.補件,
            SyncUserId = "同步員編",
            ApplyFiles = new ApplyFileDto[]
            {
                new ApplyFileDto
                {
                    FileContent = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 }, // 模擬檔案內容
                    FileName = "file1.pdf",
                    FileId = 1, // 模擬檔案ID
                },
                new ApplyFileDto
                {
                    FileContent = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 }, // 模擬檔案內容
                    FileName = "file2.jpg",
                    FileId = 2, // 模擬檔案ID
                },
            },
        };

        return request;
    }
}

[ExampleAnnotation(Name = "[200][2000]圖檔傳送-網路小白", ExampleType = ExampleType.Request)]
public class 圖檔傳送_網路小白_200_2000_ReqEx : IExampleProvider<SyncApplyFileRequest>
{
    public SyncApplyFileRequest GetExample()
    {
        var request = new SyncApplyFileRequest()
        {
            ApplyNo = "20250603X0001",
            SyncStatus = SyncFileStatus.網路小白件,
            SyncUserId = "同步員編",
            ApplyFiles = new ApplyFileDto[]
            {
                new ApplyFileDto
                {
                    FileContent = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 }, // 模擬檔案內容
                    FileName = "file1.pdf",
                    FileId = 1, // 模擬檔案ID
                },
                new ApplyFileDto
                {
                    FileContent = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 }, // 模擬檔案內容
                    FileName = "file2.jpg",
                    FileId = 2, // 模擬檔案ID
                },
            },
        };

        return request;
    }
}

[ExampleAnnotation(Name = "[200][2000]圖檔傳送-同步成功", ExampleType = ExampleType.Response)]
public class 圖檔傳送_同步成功_200_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string jsonString =
            "{\"returnCodeStatus\": 2000,\"returnMessage\": \"同步成功: 20250606X0002\",\"successData\": \"20250606X0002\",\"errorDetail\": null,\"validationErrors\": null,\"errorMessage\": \"\",\"traceId\": \"00-65fe9990c84ee002f70db3e7440a75a7-2fdf875f33404bbe-00\"}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<string>>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[400][4003]圖檔傳送-紙本初始_資料已存在", ExampleType = ExampleType.Response)]
public class 圖檔傳送_紙本初始_資料已存在_400_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string jsonString =
            "{\"returnCodeStatus\": 4003,\"returnMessage\": \"商業邏輯有誤\",\"successData\": null,\"errorDetail\": null,\"validationErrors\": null,\"errorMessage\": \"申請書編號 20250603X0001 已存在於主檔中。\",\"traceId\": \"00-f4b4ef60200babbb28ca75a0d136c312-76f9897fc2db5952-00\"}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<string>>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[404][4002]圖檔傳送-紙本補件_查無資料", ExampleType = ExampleType.Response)]
public class 圖檔傳送_紙本補件_查無資料_404_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string jsonString =
            "{\"returnCodeStatus\": 4002,\"returnMessage\": \"查無此資料\",\"successData\": null,\"errorDetail\": null,\"validationErrors\": null,\"errorMessage\": \"查無此資料: 申請書編號 20250603X0001 不存在於主檔中。\",\"traceId\": \"00-194284e70d29a45b81dcdc7c257c783e-5de1a35d971ccaf6-00\"}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<string>>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[404][4002]圖檔傳送-網路小白_查無資料", ExampleType = ExampleType.Response)]
public class 圖檔傳送_網路小白_查無資料_404_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string jsonString =
            "{\"returnCodeStatus\": 4002,\"returnMessage\": \"查無此資料\",\"successData\": null,\"errorDetail\": null,\"validationErrors\": null,\"errorMessage\": \"查無此資料: 申請書編號 20250603X0001 不存在於主檔或處理檔中。\",\"traceId\": \"00-194284e70d29a45b81dcdc7c257c783e-5de1a35d971ccaf6-00\"}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<string>>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[400][4000]圖檔傳送-格式驗證失敗", ExampleType = ExampleType.Request)]
public class 圖檔傳送_格式驗證失敗_400_4000_ReqEx : IExampleProvider<SyncApplyFileRequest>
{
    public SyncApplyFileRequest GetExample()
    {
        var request = new SyncApplyFileRequest()
        {
            ApplyNo = "20250603X0001",
            SyncStatus = SyncFileStatus.紙本初始,
            SyncUserId = "",
            ApplyFiles = new ApplyFileDto[]
            {
                new ApplyFileDto
                {
                    FileContent = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 }, // 模擬檔案內容
                    FileName = "file1.pdf",
                    FileId = 1, // 模擬檔案ID
                },
                new ApplyFileDto
                {
                    FileContent = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 }, // 模擬檔案內容
                    FileName = "file2.jpg",
                    FileId = 2, // 模擬檔案ID
                },
            },
        };

        return request;
    }
}

[ExampleAnnotation(Name = "[400][4000]圖檔傳送-格式驗證失敗", ExampleType = ExampleType.Response)]
public class 圖檔傳送_格式驗證失敗_400_4000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string jsonString =
            "{\"returnCodeStatus\": 4000,\"returnMessage\": \"格式驗證失敗\",\"successData\": null,\"errorDetail\": null,\"validationErrors\": {\"SyncUserId\": [\"同步員編 欄位為必填。\"]},\"errorMessage\": \"\",\"traceId\": \"00-493abcecb09362fb3c13aeca32dfc31a-5f526b78cedd32fc-00\"}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<string>>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[401][4004]圖檔傳送-表頭驗證失敗", ExampleType = ExampleType.Response)]
public class 圖檔傳送_表頭驗證失敗_401_4004_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string jsonString =
            "{\"returnCodeStatus\": 4004,\"returnMessage\": \"標頭驗證失敗\",\"successData\": null,\"errorDetail\": null,\"validationErrors\": null,\"errorMessage\": \"X-APPLYNO 標頭為必填欄位\",\"traceId\": \"00-493abcecb09362fb3c13aeca32dfc31a-5f526b78cedd32fc-00\"}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<string>>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[500][5000]圖檔傳送-內部程式失敗", ExampleType = ExampleType.Response)]
public class 圖檔傳送_內部程式失敗_500_5000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string jsonString =
            "{\"returnCodeStatus\": 5000,\"returnMessage\": \"內部程式失敗\",\"successData\": null,\"errorDetail\": null,\"validationErrors\": null,\"errorMessage\": \"\",\"traceId\": \"00-493abcecb09362fb3c13aeca32dfc31a-5f526b78cedd32fc-00\"}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<string>>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[502][5002]圖檔傳送-資料庫執行失敗", ExampleType = ExampleType.Response)]
public class 圖檔傳送_資料庫執行失敗_502_5002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string jsonString =
            "{\"returnCodeStatus\": 5002,\"returnMessage\": \"資料庫執行失敗\",\"successData\": null,\"errorDetail\": null,\"validationErrors\": null,\"errorMessage\": \"\",\"traceId\": \"00-493abcecb09362fb3c13aeca32dfc31a-5f526b78cedd32fc-00\"}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<string>>(jsonString);
        return data;
    }
}
