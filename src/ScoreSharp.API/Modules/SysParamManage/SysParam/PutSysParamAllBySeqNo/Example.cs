namespace ScoreSharp.API.Modules.SysParamManage.SysParam.PutSysParamAllBySeqNo;

[ExampleAnnotation(Name = "[2000]修改全部系統參數設定", ExampleType = ExampleType.Request)]
public class 修改全部系統參數設定_2000_ReqEx : IExampleProvider<PutSysParamAllBySeqNoRequest>
{
    public PutSysParamAllBySeqNoRequest GetExample()
    {
        PutSysParamAllBySeqNoRequest request = new PutSysParamAllBySeqNoRequest()
        {
            SeqNo = 1,
            IPCompareHour = 24,
            IPMatchCount = 1,
            QueryHisDataDayRange = 3,
            WebCaseEmailCompareHour = 48,
            WebCaseEmailMatchCount = 2,
            WebCaseMobileCompareHour = 48,
            WebCaseMobileMatchCount = 2,
            ShortTimeIDCompareHour = 168,
            ShortTimeIDMatchCount = 5,
            AMLProfessionCode_Version = "20250102",
            KYCFixStartTime = DateTime.Now,
            KYCFixEndTime = DateTime.Now.AddDays(1),
            KYC_StrongReVersion = "20250101",
        };

        return request;
    }
}

[ExampleAnnotation(Name = "[2000]修改全部系統參數設定", ExampleType = ExampleType.Response)]
public class 修改全部系統參數設定_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess<string>("1", "1");
    }
}

[ExampleAnnotation(Name = "[4001]修改全部系統參數設定-查無資料", ExampleType = ExampleType.Request)]
public class 修改全部系統參數設定查無資料_4001_ReqEx : IExampleProvider<PutSysParamAllBySeqNoRequest>
{
    public PutSysParamAllBySeqNoRequest GetExample()
    {
        PutSysParamAllBySeqNoRequest request = new PutSysParamAllBySeqNoRequest()
        {
            SeqNo = 2,
            IPCompareHour = 36,
            IPMatchCount = 10,
            QueryHisDataDayRange = 3,
            WebCaseEmailCompareHour = 48,
            WebCaseEmailMatchCount = 2,
            WebCaseMobileCompareHour = 48,
            WebCaseMobileMatchCount = 2,
            ShortTimeIDCompareHour = 168,
            ShortTimeIDMatchCount = 5,
            AMLProfessionCode_Version = "20250102",
            KYCFixStartTime = DateTime.Now,
            KYCFixEndTime = DateTime.Now.AddDays(1),
            KYC_StrongReVersion = "20250101",
        };

        return request;
    }
}

[ExampleAnnotation(Name = "[4001]修改全部系統參數設定-查無資料", ExampleType = ExampleType.Response)]
public class 修改全部系統參數設定查無資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "2");
    }
}

[ExampleAnnotation(Name = "[4003]修改全部系統參數設定-呼叫有誤", ExampleType = ExampleType.Response)]
public class 修改全部系統參數設定呼叫有誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}

[ExampleAnnotation(Name = "[4000]修改全部系統參數設定-AML職業別版本為無效日期", ExampleType = ExampleType.Response)]
public class 修改全部系統參數設定AML職業別版本為無效日期_4000_ResEx : IExampleProvider<ResultResponse<Dictionary<string, IEnumerable<string>>>>
{
    public ResultResponse<Dictionary<string, IEnumerable<string>>> GetExample()
    {
        var error = new Dictionary<string, IEnumerable<string>>();
        error.Add("AMLProfessionCode_Version", new[] { "AML職業別版本 為無效日期，請輸入正確的日期（格式：yyyyMMdd）" });
        return ApiResponseHelper.BadRequest(error);
    }
}

[ExampleAnnotation(Name = "[4000]修改全部系統參數設定-KYC加強審核版本為無效日期", ExampleType = ExampleType.Response)]
public class 修改全部系統參數設定KYC加強審核版本為無效日期_4000_ResEx : IExampleProvider<ResultResponse<Dictionary<string, IEnumerable<string>>>>
{
    public ResultResponse<Dictionary<string, IEnumerable<string>>> GetExample()
    {
        var error = new Dictionary<string, IEnumerable<string>>();
        error.Add("KYC_StrongReVersion", new[] { "KYC加強審核版本 為無效日期，請輸入正確的日期（格式：yyyyMMdd）" });
        return ApiResponseHelper.BadRequest(error);
    }
}
