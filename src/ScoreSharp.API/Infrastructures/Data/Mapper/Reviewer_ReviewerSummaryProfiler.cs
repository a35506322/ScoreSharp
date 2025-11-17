using ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetReviewerSummariesByApplyNo;
using ScoreSharp.API.Modules.Reviewer.ReviewerCore.InsertReviewerSummary;
using ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateReviewerSummaryBySeqNo;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class Reviewer_ReviewerSummaryProfiler : Profile
{
    public Reviewer_ReviewerSummaryProfiler()
    {
        CreateMap<InsertReviewerSummaryRequest, Reviewer_ReviewerSummary>();
        CreateMap<Reviewer_ReviewerSummary, GetReviewerSummariesByApplyNoResponse>();
    }
}
