using ScoreSharp.API.Modules.SetUp.TemplateFixContent.GetTemplateFixContentsByQueryString;
using ScoreSharp.API.Modules.SetUp.TemplateFixContent.InsertTemplateFixContent;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class SetUp_TemplateFixConentProfiler : Profile
{
    public SetUp_TemplateFixConentProfiler()
    {
        CreateMap<InsertTemplateFixContentRequest, SetUp_TemplateFixContent>();
        CreateMap<SetUp_TemplateFixContent, GetTemplateFixContentsByQueryStringResponse>();
    }
}
