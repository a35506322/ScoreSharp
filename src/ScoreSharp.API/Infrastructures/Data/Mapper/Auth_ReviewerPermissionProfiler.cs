using ScoreSharp.API.Modules.Auth.ReviewerPermission.GetApplyPermissionById;
using ScoreSharp.API.Modules.Auth.ReviewerPermission.GetReviewerPermissionById;
using ScoreSharp.API.Modules.Auth.ReviewerPermission.GetReviewerPermissionsByQueryString;
using ScoreSharp.API.Modules.Auth.ReviewerPermission.InsertReviewerPermission;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class Auth_ReviewerPermissionProfiler : Profile
{
    public Auth_ReviewerPermissionProfiler()
    {
        CreateMap<InsertReviewerPermissionRequest, Auth_ReviewerPermission>();
        CreateMap<Auth_ReviewerPermission, GetReviewerPermissionByIdResponse>()
            .ForMember(x => x.CardStatusName, opt => opt.MapFrom(src => src.CardStatus.ToString()));
        CreateMap<Auth_ReviewerPermission, GetReviewerPermissionsByQueryStringResponse>()
            .ForMember(x => x.CardStatusName, opt => opt.MapFrom(src => src.CardStatus.ToString()));
        CreateMap<Auth_ReviewerPermission, GetApplyPermissionByIdResponse>();
    }
}
