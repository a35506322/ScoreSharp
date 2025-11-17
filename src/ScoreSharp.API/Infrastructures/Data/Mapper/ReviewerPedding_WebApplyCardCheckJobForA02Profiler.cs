using ScoreSharp.API.Modules.SysPersonnel.WebApplyCardCheckJobForA02.GetWebApplyCardCheckJobForA02sByQueryString;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class ReviewerPedding_WebApplyCardCheckJobForA02Profiler : Profile
{
    public ReviewerPedding_WebApplyCardCheckJobForA02Profiler()
    {
        CreateMap<ReviewerPedding_WebApplyCardCheckJobForA02, GetWebApplyCardCheckJobForA02sByQueryStringResponse>();
    }
}
