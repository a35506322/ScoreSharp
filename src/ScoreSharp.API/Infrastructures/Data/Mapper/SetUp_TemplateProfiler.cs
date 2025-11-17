using ScoreSharp.API.Modules.SetUp.Template.GetTemplateById;
using ScoreSharp.API.Modules.SetUp.Template.GetTemplatesByQueryString;
using ScoreSharp.API.Modules.SetUp.Template.InsertTemplate;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class SetUp_TemplateProfiler : Profile
{
    public SetUp_TemplateProfiler()
    {
        CreateMap<InsertTemplateRequest, SetUp_Template>();
        CreateMap<SetUp_Template, GetTemplatesByQueryStringResponse>();
        CreateMap<SetUp_Template, GetTemplateByIdResponse>();
    }
}
