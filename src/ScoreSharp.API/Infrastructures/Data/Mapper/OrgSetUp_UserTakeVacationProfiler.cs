using ScoreSharp.API.Modules.OrgSetUp.UserTakeVacation.GetUserTakeVacationByQueryString;
using ScoreSharp.API.Modules.OrgSetUp.UserTakeVacation.InsertUserTakeVacation;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class OrgSetUp_UserTakeVacationProfiler : Profile
{
    public OrgSetUp_UserTakeVacationProfiler()
    {
        CreateMap<InsertUserTakeVacationRequest, OrgSetUp_UserTakeVacation>();
        CreateMap<OrgSetUp_UserTakeVacation, GetUserTakeVacationByQueryStringResponse>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.StartTime.ToString("yyyy/MM/dd")));
    }
}
