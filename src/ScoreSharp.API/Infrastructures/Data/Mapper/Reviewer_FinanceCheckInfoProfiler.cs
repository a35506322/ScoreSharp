using ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyCreditCardInfoByApplyNo;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class Reviewer_FinanceCheckInfoProfiler : Profile
{
    public Reviewer_FinanceCheckInfoProfiler()
    {
        CreateMap<Reviewer_FinanceCheckInfo, KYCInfo>();
    }
}
