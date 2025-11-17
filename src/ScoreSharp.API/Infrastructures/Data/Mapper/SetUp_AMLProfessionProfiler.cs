using ScoreSharp.API.Modules.SetUp.AMLProfession.GetAMLProfessionById;
using ScoreSharp.API.Modules.SetUp.AMLProfession.GetAMLProfessionsByQueryString;
using ScoreSharp.API.Modules.SetUp.AMLProfession.InsertAMLProfession;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class SetUp_AMLProfessionProfiler : Profile
{
    public SetUp_AMLProfessionProfiler()
    {
        CreateMap<InsertAMLProfessionRequest, SetUp_AMLProfession>();
        CreateMap<SetUp_AMLProfession, GetAMLProfessionsByQueryStringResponse>();
        CreateMap<SetUp_AMLProfession, GetAMLProfessionByIdResponse>();
    }
}
