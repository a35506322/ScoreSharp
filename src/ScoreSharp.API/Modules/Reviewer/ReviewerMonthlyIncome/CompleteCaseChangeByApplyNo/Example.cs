namespace ScoreSharp.API.Modules.Reviewer.ReviewerMonthlyIncome.CompleteCaseChangeByApplyNo;

[ExampleAnnotation(Name = "[2000]完成月收入確認案件異動", ExampleType = ExampleType.Response)]
public class 完成月收入確認案件異動_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.Success("20240903X8997", "完成月收入確認案件異動，申請書編號：20240903X8997");
}

[ExampleAnnotation(Name = "[4003]完成月收入確認案件異動_卡片狀態錯誤請先按儲存", ExampleType = ExampleType.Response)]
public class 完成月收入確認案件異動_卡片狀態錯誤請先按儲存_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() =>
        ApiResponseHelper.BusinessLogicFailed<string>(null, "卡片狀態錯誤，請先按儲存，再進行權限內本案徵審");
}

[ExampleAnnotation(Name = "[4003]完成月收入確認案件異動_所有狀態需相同請先按儲存", ExampleType = ExampleType.Response)]
public class 完成月收入確認案件異動_所有狀態需相同請先按儲存_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() =>
        ApiResponseHelper.BusinessLogicFailed<string>(null, "卡片狀態錯誤，所有狀態需相同，請先按儲存，再進行完成月收入確認");
}
