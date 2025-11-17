namespace ScoreSharp.API.Modules.Reviewer.ReviewerMonthlyIncome.UpdateMonthlyIncomeInfoByApplyNo;

[ExampleAnnotation(Name = "[2000]更新月收入簽核資料", ExampleType = ExampleType.Request)]
public class 更新月收入簽核資料_2000_ReqEx : IExampleProvider<UpdateMonthlyIncomeInfoByApplyNoRequest>
{
    public UpdateMonthlyIncomeInfoByApplyNoRequest GetExample()
    {
        return new UpdateMonthlyIncomeInfoByApplyNoRequest()
        {
            ApplyNo = "20240903X8997",
            CurrentMonthIncome = 100000,
            CreditCheckCode = "A02",
        };
    }
}

[ExampleAnnotation(Name = "[2000]更新月收入簽核資料", ExampleType = ExampleType.Response)]
public class 更新月收入簽核資料_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.UpdateByIdSuccess("20240903X8997", "20240903X8997");
}

[ExampleAnnotation(Name = "[4001]更新月收入簽核資料", ExampleType = ExampleType.Response)]
public class 更新月收入簽核資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.NotFound<string>(null, "20240903X8997");
}

[ExampleAnnotation(Name = "[4003]更新月收入簽核資料", ExampleType = ExampleType.Response)]
public class 更新月收入簽核資料_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.BusinessLogicFailed<string>(null, "申請書編號與Req比對錯誤");
}

[ExampleAnnotation(Name = "[4003]更新月收入簽核資料-查無定義值", ExampleType = ExampleType.Response)]
public class 更新月收入簽核資料查無定義值_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>(null, "徵信代碼不存在");
    }
}

[ExampleAnnotation(Name = "[4003]更新月收入簽核資料-卡片狀態錯誤", ExampleType = ExampleType.Response)]
public class 更新月收入簽核資料卡片狀態錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>(null, "卡片狀態錯誤，無待月收入預審狀態，無法更新月收入簽核資料");
    }
}
