using ScoreSharp.API.Modules.SetUp.CardPromotion.GetCardPromotionById;
using ScoreSharp.API.Modules.SetUp.CardPromotion.GetCardPromotionsByQueryString;
using ScoreSharp.API.Modules.SetUp.CardPromotion.InsertCardPromotion;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class SetUp_CardPromotionProfiler : Profile
{
    public SetUp_CardPromotionProfiler()
    {
        CreateMap<InsertCardPromotionRequest, SetUp_CardPromotion>();
        CreateMap<SetUp_CardPromotion, GetCardPromotionByIdResponse>();
        CreateMap<SetUp_CardPromotion, GetCardPromotionsByQueryStringResponse>();
    }
}
