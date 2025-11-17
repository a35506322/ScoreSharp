using ScoreSharp.API.Modules.SetUp.RejectionReason.GetRejectionReasonById;
using ScoreSharp.API.Modules.SetUp.RejectionReason.GetRejectionReasonByQueryString;
using ScoreSharp.API.Modules.SetUp.RejectionReason.InsertRejectionReason;
using ScoreSharp.API.Modules.SetUp.RejectionReason.UpdateRejectionReasonById;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class SetUp_RejectionReasonProfiler : Profile
{
    public SetUp_RejectionReasonProfiler()
    {
        CreateMap<InsertRejectionReasonRequest, SetUp_RejectionReason>();
        CreateMap<SetUp_RejectionReason, GetRejectionReasonByIdResponse>();
        CreateMap<SetUp_RejectionReason, GetRejectionReasonByQueryStringResponse>();
        CreateMap<UpdateRejectionReasonByIdRequest, SetUp_RejectionReason>();
    }
}
