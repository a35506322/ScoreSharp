namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetNameCheckLogByApplyNo;

[ExampleAnnotation(Name = "[2000]取得姓名檢核結果", ExampleType = ExampleType.Response)]
public class 取得姓名檢核紀錄_2000_ResEx : IExampleProvider<ResultResponse<List<GetNameCheckLogResponse>>>
{
    public ResultResponse<List<GetNameCheckLogResponse>> GetExample()
    {
        DateTime dateTime = DateTime.Now;

        return ApiResponseHelper.Success(
            new List<GetNameCheckLogResponse>
            {
                new GetNameCheckLogResponse
                {
                    SeqNo = 1,
                    ApplyNo = "20240803B0001",
                    ID = "A123456789",
                    Name = "王小明",
                    UserType = UserType.正卡人,
                    UserTypeName = UserType.正卡人.ToName(),
                    AMLId = "AML123456",
                    StartTime = dateTime,
                    EndTime = dateTime.AddSeconds(20),
                    ResponseResult = "N",
                    RcPoint = 0,
                },
            }
        );
    }
}

[ExampleAnnotation(Name = "[4001]取得姓名檢核結果_查無此申請書編號", ExampleType = ExampleType.Response)]
public class 取得姓名檢核紀錄查無此申請書編號_4001_ResEx : IExampleProvider<ResultResponse<List<GetNameCheckLogResponse>>>
{
    public ResultResponse<List<GetNameCheckLogResponse>> GetExample() =>
        ApiResponseHelper.NotFound<List<GetNameCheckLogResponse>>(null, "20240803B0001");
}
