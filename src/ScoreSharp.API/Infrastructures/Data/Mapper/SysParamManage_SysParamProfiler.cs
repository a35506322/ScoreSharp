using ScoreSharp.API.Modules.SysParamManage.SysParam.GetSysParamAll;

namespace ScoreSharp.API.Infrastructures.Data.Mapper
{
    public class SysParamManage_SysParamProfiler : Profile
    {
        public SysParamManage_SysParamProfiler()
        {
            CreateMap<SysParamManage_SysParam, GetSysParamAllResponse>();
        }
    }
}
