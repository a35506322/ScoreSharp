using ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetCardRecordsByApplyNo;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class Reviewer_CardRecordProfiler : Profile
{
    public Reviewer_CardRecordProfiler()
    {
        CreateMap<Reviewer_CardRecord, GetCardRecordsByApplyNoResponse>();
    }
}
