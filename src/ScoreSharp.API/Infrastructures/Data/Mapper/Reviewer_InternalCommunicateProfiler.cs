using ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetInternalCommunicateByApplyNo;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class Reviewer_InternalCommunicateProfiler : Profile
{
    public Reviewer_InternalCommunicateProfiler()
    {
        CreateMap<Reviewer_InternalCommunicate, GetInternalCommunicateByApplyNoResponse>();
    }
}
