using ScoreSharp.API.Modules.SetUp.LongTermReason.GetLongTermReasonById;
using ScoreSharp.API.Modules.SetUp.LongTermReason.GetLongTermReasonsByQueryString;
using ScoreSharp.API.Modules.SetUp.LongTermReason.InsertLongTermReason;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class SetUp_LongTermReasonProfiler : Profile
{
    public SetUp_LongTermReasonProfiler()
    {
        CreateMap<InsertLongTermReasonRequest, SetUp_LongTermReason>();
        CreateMap<SetUp_LongTermReason, GetLongTermReasonsByQueryStringResponse>();
        CreateMap<SetUp_LongTermReason, GetLongTermReasonByIdResponse>();
    }
}
