using ScoreSharp.API.Modules.SetUp.SupplementReason.GetSupplementReasonById;
using ScoreSharp.API.Modules.SetUp.SupplementReason.GetSupplementReasonByQueryString;
using ScoreSharp.API.Modules.SetUp.SupplementReason.InsertSupplementReason;
using ScoreSharp.API.Modules.SetUp.SupplementReason.UpdateSupplementReasonById;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class SetUp_SupplementReasonProfiler : Profile
{
    public SetUp_SupplementReasonProfiler()
    {
        CreateMap<InsertSupplementReasonRequest, SetUp_SupplementReason>();
        CreateMap<UpdateSupplementReasonByIdRequest, SetUp_SupplementReason>();
        CreateMap<SetUp_SupplementReason, GetSupplementReasonByIdResponse>();
        CreateMap<SetUp_SupplementReason, GetSupplementReasonByQueryStringResponse>();
    }
}
