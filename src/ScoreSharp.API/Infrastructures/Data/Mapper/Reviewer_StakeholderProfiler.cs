using ScoreSharp.API.Modules.Manage.Stakeholder.GetStakeholderById;
using ScoreSharp.API.Modules.Manage.Stakeholder.GetStakeholderByQueryString;
using ScoreSharp.API.Modules.Manage.Stakeholder.InsertStakeholder;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class Reviewer_StakeholderProfiler : Profile
{
    public Reviewer_StakeholderProfiler()
    {
        CreateMap<InsertStakeholderRequest, Reviewer_Stakeholder>();
        CreateMap<Reviewer_Stakeholder, GetStakeholderByQueryStringResponse>();
        CreateMap<Reviewer_Stakeholder, GetStakeholderByIdResponse>();
    }
}
