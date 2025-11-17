using ScoreSharp.API.Modules.SetUp.ApplicableInterestRateLimit.GetSingleApplicableInterestRateLimit;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class SetUp_ApplicableInterestRateLimitProfiler : Profile
{
    public SetUp_ApplicableInterestRateLimitProfiler()
    {
        CreateMap<SetUp_ApplicableInterestRateLimit, GetSingleApplicableInterestRateLimitResponse>();
    }
}
