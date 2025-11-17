namespace ScoreSharp.API.Modules.SetUp.BillDay.DeleteBillDayById;

[ExampleAnnotation(Name = "[4001]刪除單筆帳單日-查無此資料", ExampleType = ExampleType.Response)]
public class 刪除帳單日查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "刪除的ID");
    }
}

[ExampleAnnotation(Name = "[2000]刪除單筆帳單日", ExampleType = ExampleType.Response)]
public class 刪除帳單日_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("刪除的ID");
    }
}
