using ScoreSharp.API.Modules.Test.Models;

namespace ScoreSharp.API.Modules.Test.Mapper;

public class Test_GetApplicationInfoReqDataProfiler : Profile
{
    public Test_GetApplicationInfoReqDataProfiler()
    {
        CreateMap<Reviewer_ApplyCreditCardInfoHandle, GetApplicationInfoReqDataResponse>();
        CreateMap<Reviewer_ApplyCreditCardInfoMain, GetApplicationInfoReqDataResponse>();
    }
}
