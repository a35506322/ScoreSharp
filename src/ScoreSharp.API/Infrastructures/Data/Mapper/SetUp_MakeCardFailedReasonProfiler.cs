using ScoreSharp.API.Modules.SetUp.MakeCardFailedReason.GetMakeCardFailedReasonById;
using ScoreSharp.API.Modules.SetUp.MakeCardFailedReason.GetMakeCardFailedReasonByQueryString;
using ScoreSharp.API.Modules.SetUp.MakeCardFailedReason.InsertMakeCardFailedReason;
using ScoreSharp.API.Modules.SetUp.SupplementReason.GetSupplementReasonByQueryString;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class SetUp_MakeCardFailedReasonProfiler : Profile
{
    public SetUp_MakeCardFailedReasonProfiler()
    {
        CreateMap<InsertMakeCardFailedReasonRequest, SetUp_MakeCardFailedReason>();
        CreateMap<SetUp_MakeCardFailedReason, GetMakeCardFailedReasonByQueryStringResponse>();
        CreateMap<SetUp_MakeCardFailedReason, GetMakeCardFailedReasonByIdResponse>();
    }
}
