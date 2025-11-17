namespace ScoreSharp.API.Modules.SetUp.SupplementReason.GetSupplementReasonById;

[ExampleAnnotation(Name = "[4001]取得補件原因-查無此資料", ExampleType = ExampleType.Response)]
public class 取得補件原因查無此資料_4001_ResEx : IExampleProvider<ResultResponse<GetSupplementReasonByIdResponse>>
{
    public ResultResponse<GetSupplementReasonByIdResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetSupplementReasonByIdResponse>(null, "顯示找不到的ID");
    }
}

[ExampleAnnotation(Name = "[2000]取得補件原因", ExampleType = ExampleType.Response)]
public class 取得補件原因_2000_ResEx : IExampleProvider<ResultResponse<GetSupplementReasonByIdResponse>>
{
    public ResultResponse<GetSupplementReasonByIdResponse> GetExample()
    {
        GetSupplementReasonByIdResponse response = new GetSupplementReasonByIdResponse
        {
            SupplementReasonCode = "01",
            SupplementReasonName = "地址證明",
            IsActive = "Y",
            AddUserId = "ADMIN",
            AddTime = DateTime.Now,
            UpdateUserId = "ADMIN",
            UpdateTime = DateTime.Now,
        };

        return ApiResponseHelper.Success(response);
    }
}
