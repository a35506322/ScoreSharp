using ScoreSharp.API.Modules.SysPersonnel.BatchSet.GetBatchSetById;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class SysParamManage_BatchSetProfiler : Profile
{
    public SysParamManage_BatchSetProfiler()
    {
        CreateMap<SysParamManage_BatchSet, GetBatchSetByIdResponse>();
    }
}
