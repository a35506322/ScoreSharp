using ScoreSharp.API.Infrastructures.Data.Entities;
using ScoreSharp.API.Modules.Auth.RouterCategory.GetRouterCategoryById;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class Auth_RouterCategoryProfiler : Profile
{
    public Auth_RouterCategoryProfiler()
    {
        CreateMap<Auth_RouterCategory, GetRouterCategoryByIdResponse>();
    }
}
