using ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyCreditCardInfoByApplyNo;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class Reviewer_ApplyCreditCardInfoSupplementaryProfiler : Profile
{
    public Reviewer_ApplyCreditCardInfoSupplementaryProfiler()
    {
        CreateMap<Reviewer_ApplyCreditCardInfoSupplementary, Supplementary>();
    }
}
