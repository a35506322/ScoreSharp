using ScoreSharp.API.Modules.SetUp.FixTime.GetFixTime;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class Setup_FixTimeProfiler : Profile
{
    public Setup_FixTimeProfiler()
    {
        CreateMap<SetUp_FixTime, GetFixTimeResponse>();
    }
}
