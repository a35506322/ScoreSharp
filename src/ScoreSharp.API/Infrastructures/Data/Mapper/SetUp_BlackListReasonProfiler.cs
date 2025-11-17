using ScoreSharp.API.Modules.SetUp.BlackListReason.GetBlackListReasonById;
using ScoreSharp.API.Modules.SetUp.BlackListReason.GetBlackListReasonByQueryString;
using ScoreSharp.API.Modules.SetUp.BlackListReason.InsertBlackListReason;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class SetUp_BlackListReasonProfiler : Profile
{
    public SetUp_BlackListReasonProfiler()
    {
        CreateMap<InsertBlackListReasonRequest, SetUp_BlackListReason>();
        CreateMap<SetUp_BlackListReason, GetBlackListReasonByQueryStringResponse>();
        CreateMap<SetUp_BlackListReason, GetBlackListReasonByIdResponse>();
    }
}
