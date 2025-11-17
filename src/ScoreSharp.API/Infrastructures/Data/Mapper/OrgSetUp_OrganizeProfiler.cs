using ScoreSharp.API.Modules.OrgSetUp.Organize.GetOrganizeById;
using ScoreSharp.API.Modules.OrgSetUp.Organize.GetOrganizesByQueryString;
using ScoreSharp.API.Modules.OrgSetUp.Organize.InsertOrganize;
using ScoreSharp.API.Modules.OrgSetUp.Organize.UpdateOrganizeById;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class OrgSetUp_OrganizeProfiler : Profile
{
    public OrgSetUp_OrganizeProfiler()
    {
        CreateMap<OrgSetUp_Organize, GetOrganizeByIdResponse>();
        CreateMap<OrgSetUp_Organize, GetOrganizesByQueryStringResponse>();
        CreateMap<InsertOrganizeRequest, OrgSetUp_Organize>();
        CreateMap<UpdateOrganizeByIdRequest, OrgSetUp_Organize>();
    }
}
