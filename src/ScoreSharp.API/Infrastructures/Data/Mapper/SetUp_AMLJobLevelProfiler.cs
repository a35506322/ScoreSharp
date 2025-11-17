using ScoreSharp.API.Modules.SetUp.AMLJobLevel.GetAMLJobLevelById;
using ScoreSharp.API.Modules.SetUp.AMLJobLevel.GetAMLJobLevelsByQueryString;
using ScoreSharp.API.Modules.SetUp.AMLJobLevel.InsertAMLJobLevel;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class SetUp_AMLJobLevelProfiler : Profile
{
    public SetUp_AMLJobLevelProfiler()
    {
        CreateMap<InsertAMLJobLevelRequest, SetUp_AMLJobLevel>();
        CreateMap<SetUp_AMLJobLevel, GetAMLJobLevelsByQueryStringResponse>();
        CreateMap<SetUp_AMLJobLevel, GetAMLJobLevelByIdResponse>();
    }
}
