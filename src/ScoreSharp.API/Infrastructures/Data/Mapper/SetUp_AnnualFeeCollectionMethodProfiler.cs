using ScoreSharp.API.Modules.SetUp.AnnualFeeCollectionMethod.GetAnnualFeeCollectionMethodId;
using ScoreSharp.API.Modules.SetUp.AnnualFeeCollectionMethod.GetAnnualFeeCollectionMethodQueryString;
using ScoreSharp.API.Modules.SetUp.AnnualFeeCollectionMethod.InsertAnnualFeeCollectionMethod;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class SetUp_AnnualFeeCollectionMethodProfiler : Profile
{
    public SetUp_AnnualFeeCollectionMethodProfiler()
    {
        CreateMap<InsertAnnualFeeCollectionMethodRequest, SetUp_AnnualFeeCollectionMethod>();
        CreateMap<SetUp_AnnualFeeCollectionMethod, GetAnnualFeeCollectionMethodIdResponse>();
        CreateMap<SetUp_AnnualFeeCollectionMethod, GetAnnualFeeCollectionMethodQueryStringResponse>();
    }
}
