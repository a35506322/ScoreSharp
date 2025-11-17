using ScoreSharp.API.Modules.Auth.Action.GetActionsByQueryString;
using ScoreSharp.API.Modules.OrgSetUp.User.GetUserById;
using ScoreSharp.API.Modules.OrgSetUp.User.GetUsersByQueryString;
using ScoreSharp.API.Modules.OrgSetUp.User.InsertUser;

namespace ScoreSharp.API.Infrastructures.Data.Mapper
{
    public class OrgSetUp_UserProfiler : Profile
    {
        public OrgSetUp_UserProfiler()
        {
            CreateMap<InsertUserRequest, OrgSetUp_User>();
            CreateMap<OrgSetUp_User, GetUserByIdResponse>();
            CreateMap<OrgSetUp_User, GetUsersByQueryStringResponse>();
        }
    }
}
