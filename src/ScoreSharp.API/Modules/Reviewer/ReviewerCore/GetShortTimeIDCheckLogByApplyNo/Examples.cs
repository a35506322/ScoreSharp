namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetShortTimeIDCheckLogByApplyNo;

[ExampleAnnotation(Name = "[2000]取得短時間ID檢核結果", ExampleType = ExampleType.Response)]
public class 取得短時間ID檢核結果_2000_ResEx : IExampleProvider<ResultResponse<GetShortTimeIDCheckLogByApplyNoResponse>>
{
    public ResultResponse<GetShortTimeIDCheckLogByApplyNoResponse> GetExample()
    {
        return ApiResponseHelper.Success(new GetShortTimeIDCheckLogByApplyNoResponse
        {
            ApplyNo = "20250124B0004",
            ShortTimeID_Flag = "Y",
            ShortTimeID_CheckRecord = "測試",
            ShortTimeID_UpdateUserId = "arthurlin",
            ShortTimeID_UpdateTime = DateTime.Parse("2025-03-25 16:05:00.000"),
            ShortTimeID_IsError = "N",
            CheckTraceDtos = new List<CheckTraceDto>
            {
                new CheckTraceDto
                {
                    ApplyNo = "20250101B9998",
                    ApplyDate = DateTime.Parse("2025-03-25 10:44:30.513"),
                    ApplyCardType = "JST59",
                },
                new CheckTraceDto
                {
                    ApplyNo = "20250101B9999",
                    ApplyDate = DateTime.Parse("2025-03-25 10:44:33.983"),
                    ApplyCardType = "JST59",
                },
                new CheckTraceDto
                {
                    ApplyNo = "20250206B0091",
                    ApplyDate = DateTime.Parse("2025-03-25 10:44:34.340"),
                    ApplyCardType = "MTB42",
                },
                new CheckTraceDto
                {
                    ApplyNo = "20250206B0092",
                    ApplyDate = DateTime.Parse("2025-03-25 10:44:34.743"),
                    ApplyCardType = "JST59",
                },
                new CheckTraceDto
                {
                    ApplyNo = "20250210B0098",
                    ApplyDate = DateTime.Parse("2025-03-25 10:44:36.147"),
                    ApplyCardType = "JST59",
                },
            },
        });
    }
}

[ExampleAnnotation(Name = "[4001]取得短時間ID檢核結果-查無此申請書編號", ExampleType = ExampleType.Response)]
public class 取得短時間ID檢核結果查無此申請書編號_4001_ResEx:IExampleProvider<ResultResponse<GetShortTimeIDCheckLogByApplyNoResponse>>
{
    public ResultResponse<GetShortTimeIDCheckLogByApplyNoResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetShortTimeIDCheckLogByApplyNoResponse>(null, "20250124B0004");
    }
}
