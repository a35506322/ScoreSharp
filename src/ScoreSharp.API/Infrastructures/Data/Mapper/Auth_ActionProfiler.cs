using ScoreSharp.API.Modules.Auth.Action.GetActionById;
using ScoreSharp.API.Modules.Auth.Action.GetActionsByQueryString;
using ScoreSharp.API.Modules.Auth.Action.InsertAction;
using ScoreSharp.API.Modules.Auth.Action.UpdateActionById;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class Auth_ActionProfiler : Profile
{
    public Auth_ActionProfiler()
    {
        CreateMap<Auth_Action, GetActionByIdResponse>();
        CreateMap<Auth_Action, GetActionsByQueryStringResponse>();
        CreateMap<UpdateActionByIdRequest, Auth_Action>();
        CreateMap<InsertActionRequest, Auth_Action>();
    }
}
