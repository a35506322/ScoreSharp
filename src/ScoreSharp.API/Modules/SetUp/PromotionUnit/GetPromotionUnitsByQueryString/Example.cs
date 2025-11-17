namespace ScoreSharp.API.Modules.SetUp.PromotionUnit.GetPromotionUnitsByQueryString;

[ExampleAnnotation(Name = "[2000]取得推廣單位", ExampleType = ExampleType.Response)]
public class 取得推廣單位_2000_ResEx : IExampleProvider<ResultResponse<List<GetPromotionUnitsByQueryStringResponse>>>
{
    public ResultResponse<List<GetPromotionUnitsByQueryStringResponse>> GetExample()
    {
        var data = new List<GetPromotionUnitsByQueryStringResponse>
        {
            new GetPromotionUnitsByQueryStringResponse
            {
                PromotionUnitCode = "100",
                PromotionUnitName = "聯邦文教推廣",
                IsActive = "Y",
                AddTime = DateTime.Now,
                AddUserId = "ADMIN",
                UpdateTime = DateTime.Now,
                UpdateUserId = "ADMIN",
            },
        };
        return ApiResponseHelper.Success(data);
    }
}
