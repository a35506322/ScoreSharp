using ScoreSharp.API.Modules.SetUp.BillDay.GetBillDayByQueryString;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class SetUp_BillDayProfiler : Profile
{
    public SetUp_BillDayProfiler()
    {
        CreateMap<SetUp_BillDay, GetBillDayByQueryStringResponse>();
    }
}
