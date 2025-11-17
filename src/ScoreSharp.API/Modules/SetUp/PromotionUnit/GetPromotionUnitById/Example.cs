namespace ScoreSharp.API.Modules.SetUp.PromotionUnit.GetPromotionUnitById;

[ExampleAnnotation(Name = "[2000]取得推廣單位", ExampleType = ExampleType.Response)]
public class 取得推廣單位_2000_ResEx : IExampleProvider<ResultResponse<GetPromotionUnitByIdResponse>>
{
    public ResultResponse<GetPromotionUnitByIdResponse> GetExample()
    {
        GetPromotionUnitByIdResponse response = new()
        {
            PromotionUnitCode = "200",
            PromotionUnitName = "財務主管或其他主管職",
            IsActive = "Y",
            AddUserId = "ADMIN",
            AddTime = DateTime.Now,
            UpdateTime = DateTime.Now,
            UpdateUserId = "ADMIN",
        };
        return ApiResponseHelper.Success(response);
    }
}

[ExampleAnnotation(Name = "[4001]取得推廣單位-查無此資料", ExampleType = ExampleType.Response)]
public class 取得推廣單位查無此資料_4001_ResEx : IExampleProvider<ResultResponse<GetPromotionUnitByIdResponse>>
{
    public ResultResponse<GetPromotionUnitByIdResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetPromotionUnitByIdResponse>(null, "150");
    }
}
