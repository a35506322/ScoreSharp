using ScoreSharp.API.Modules.SysPersonnel.WebApplyCardCheckJobForNotA02.GetWebApplyCardCheckJobForNotA02sByQueryString;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class ReviewerPedding_WebApplyCardCheckJobForNotA02Profiler : Profile
{
    public ReviewerPedding_WebApplyCardCheckJobForNotA02Profiler()
    {
        CreateMap<ReviewerPedding_WebApplyCardCheckJobForNotA02, GetWebApplyCardCheckJobForNotA02sByQueryStringResponse>();
    }
}
