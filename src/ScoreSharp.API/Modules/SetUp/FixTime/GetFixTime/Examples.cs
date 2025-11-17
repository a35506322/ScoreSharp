namespace ScoreSharp.API.Modules.SetUp.FixTime.GetFixTime;

[ExampleAnnotation(Name = "[2000]取得維護時段設定成功", ExampleType = ExampleType.Response)]
public class 取得維護時段設定_2000_ResEx : IExampleProvider<ResultResponse<GetFixTimeResponse>>
{
    public ResultResponse<GetFixTimeResponse> GetExample()
    {
        return ApiResponseHelper.Success(
            new GetFixTimeResponse
            {
                SeqNo = 1,
                KYC_IsFix = "Y",
                KYC_StartTime = new DateTime(2025, 1, 1, 0, 0, 0),
                KYC_EndTime = new DateTime(2025, 1, 1, 6, 0, 0),
                UpdateUserId = "U123456",
                UpdateTime = new DateTime(2024, 6, 19, 12, 34, 56),
            }
        );
    }
}
