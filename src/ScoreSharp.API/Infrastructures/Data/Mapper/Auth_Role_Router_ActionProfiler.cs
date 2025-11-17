using ScoreSharp.API.Modules.Auth.Role.InsertRoleAuthById;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class Auth_Role_Router_ActionProfiler : Profile
{
    public Auth_Role_Router_ActionProfiler()
    {
        CreateMap<InsertRoleAuthByIdRequest, Auth_Role_Router_Action>();
    }
}
