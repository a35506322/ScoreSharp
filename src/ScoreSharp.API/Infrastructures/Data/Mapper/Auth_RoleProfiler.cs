using ScoreSharp.API.Modules.Auth.Role.GetRoleById;
using ScoreSharp.API.Modules.Auth.Role.GetRolesByQueryString;
using ScoreSharp.API.Modules.Auth.Role.InsertRole;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class Auth_RoleProfiler : Profile
{
    public Auth_RoleProfiler()
    {
        CreateMap<InsertRoleRequest, Auth_Role>();
        CreateMap<Auth_Role, GetRolesByQueryStringResponse>();
        CreateMap<Auth_Role, GetRoleByIdResponse>();
    }
}
