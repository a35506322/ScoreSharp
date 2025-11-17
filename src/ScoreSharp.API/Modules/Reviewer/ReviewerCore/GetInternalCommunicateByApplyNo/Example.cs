namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetInternalCommunicateByApplyNo;

[ExampleAnnotation(Name = "[2000]取得內部溝通紀錄", ExampleType = ExampleType.Response)]
public class 取得內部溝通紀錄_2000_ResEx : IExampleProvider<ResultResponse<GetInternalCommunicateByApplyNoResponse>>
{
    public ResultResponse<GetInternalCommunicateByApplyNoResponse> GetExample()
    {
        var response = new GetInternalCommunicateByApplyNoResponse()
        {
            ApplyNo = "20250321X4220",
            CommunicationNotes = "註記",
            SupplementContactRecords_Summary = "摘要",
            SupplementContactRecords_UserId = "arthurlin",
            SupplementContactRecords_Result = SupplementContactRecordsResult.客戶願意補件,
            SupplementContactRecords_Type = SupplementContactRecordsType.人員通知,
        };
        return ApiResponseHelper.Success(response);
    }
}

[ExampleAnnotation(Name = "[4001]取得內部溝通紀錄-查無此ID", ExampleType = ExampleType.Response)]
public class 取得內部溝通紀錄查無此ID_4001_ResEx : IExampleProvider<ResultResponse<GetInternalCommunicateByApplyNoResponse>>
{
    public ResultResponse<GetInternalCommunicateByApplyNoResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetInternalCommunicateByApplyNoResponse>(null, "20241128M4480");
    }
}
