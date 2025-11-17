using ScoreSharp.API.Modules.SetUp.ProjectCode.GetProjectCodeById;
using ScoreSharp.API.Modules.SetUp.ProjectCode.GetProjectCodesByQueryString;
using ScoreSharp.API.Modules.SetUp.ProjectCode.InsertProjectCode;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class SetUp_ProjectCodeProfiler : Profile
{
    public SetUp_ProjectCodeProfiler()
    {
        CreateMap<InsertProjectCodeRequest, SetUp_ProjectCode>();
        CreateMap<SetUp_ProjectCode, GetProjectCodesByQueryStringResponse>();
        CreateMap<SetUp_ProjectCode, GetProjectCodeByIdResponse>();
    }
}
