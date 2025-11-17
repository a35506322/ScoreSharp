using ScoreSharp.API.Modules.SetUp.PromotionUnit.GetPromotionUnitById;
using ScoreSharp.API.Modules.SetUp.PromotionUnit.GetPromotionUnitsByQueryString;
using ScoreSharp.API.Modules.SetUp.PromotionUnit.InsertPromotionUnit;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class SetUp_PromotionUnitProfiler : Profile
{
    public SetUp_PromotionUnitProfiler()
    {
        CreateMap<InsertPromotionUnitRequest, SetUp_PromotionUnit>();
        CreateMap<SetUp_PromotionUnit, GetPromotionUnitsByQueryStringResponse>();
        CreateMap<SetUp_PromotionUnit, GetPromotionUnitByIdResponse>();
    }
}
