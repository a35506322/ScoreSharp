using ScoreSharp.API.Modules.SysPersonnel.PaperApplyCardCheckJob.GetPaperApplyCardCheckJobsByQueryString;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class ReviewerPedding_PaperApplyCardCheckJobProfiler : Profile
{
    public ReviewerPedding_PaperApplyCardCheckJobProfiler()
    {
        CreateMap<ReviewerPedding_PaperApplyCardCheckJob, GetPaperApplyCardCheckJobsByQueryStringResponse>();
    }
}
