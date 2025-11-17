using ScoreSharp.API.Modules.Auth.Router.GetRouterById;
using ScoreSharp.API.Modules.Auth.Router.GetRoutersByQueryString;
using ScoreSharp.API.Modules.Auth.Router.UpdateRouterById;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class Auth_RouterProfiler : Profile
{
    public Auth_RouterProfiler()
    {
        CreateMap<Auth_Router, GetRouterByIdResponse>();
        CreateMap<Auth_Router, GetRoutersByQueryStringResponse>();
        CreateMap<UpdateRouterByIdRequest, Auth_Router>();
    }
}
