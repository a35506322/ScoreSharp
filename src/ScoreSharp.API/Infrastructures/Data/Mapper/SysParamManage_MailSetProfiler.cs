using ScoreSharp.API.Modules.SysPersonnel.MailSet.GetMailSetById;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class SysParamManage_MailSetProfiler : Profile
{
    public SysParamManage_MailSetProfiler()
    {
        CreateMap<SysParamManage_MailSet, GetMailSetByIdResponse>();
    }
}
