using ScoreSharp.API.Modules.OrgSetUp.UserOrgCaseSetUp.GetUserOrgCaseSetUpById;
using ScoreSharp.API.Modules.OrgSetUp.UserOrgCaseSetUp.GetUserOrgCasesSetUpByQueryString;
using ScoreSharp.API.Modules.OrgSetUp.UserOrgCaseSetUp.InsertUserOrgCaseSetUp;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class OrgSetUp_UserOrgCaseSetUpProfiler : Profile
{
    public OrgSetUp_UserOrgCaseSetUpProfiler()
    {
        CreateMap<InsertUserOrgCaseSetUpRequest, OrgSetUp_UserOrgCaseSetUp>();
        CreateMap<OrgSetUp_UserOrgCaseSetUp, GetUserOrgCaseSetUpByIdResponse>();
        CreateMap<OrgSetUp_UserOrgCaseSetUp, GetUserOrgCasesSetUpByQueryStringResponse>();
    }
}
