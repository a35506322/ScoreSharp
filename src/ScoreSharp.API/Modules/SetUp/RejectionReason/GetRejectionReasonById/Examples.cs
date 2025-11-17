namespace ScoreSharp.API.Modules.SetUp.RejectionReason.GetRejectionReasonById;

[ExampleAnnotation(Name = "[2000]取得退件原因", ExampleType = ExampleType.Response)]
public class 取得退件原因_2000_ResEx : IExampleProvider<ResultResponse<GetRejectionReasonByIdResponse>>
{
    public ResultResponse<GetRejectionReasonByIdResponse> GetExample()
    {
        GetRejectionReasonByIdResponse response = new GetRejectionReasonByIdResponse
        {
            RejectionReasonCode = "01",
            RejectionReasonName = "信用評分不足",
            IsActive = "Y",
            AddTime = DateTime.Now,
            AddUserId = "ADMIN",
            UpdateTime = DateTime.Now,
            UpdateUserId = "ADMIN",
        };

        return ApiResponseHelper.Success(response);
    }
}

[ExampleAnnotation(Name = "[4001]取得退件原因-查無此資料", ExampleType = ExampleType.Response)]
public class 取得退件原因查無此資料_4001_ResEx : IExampleProvider<ResultResponse<GetRejectionReasonByIdResponse>>
{
    public ResultResponse<GetRejectionReasonByIdResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetRejectionReasonByIdResponse>(null, "顯示找不到的ID");
    }
}
