using ScoreSharp.API.Modules.SetUp.WorkDay.GetWorkDayById;
using ScoreSharp.API.Modules.SetUp.WorkDay.GetWorkDaysByQueryString;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class SetUp_WorkDayProfiler : Profile
{
    public SetUp_WorkDayProfiler()
    {
        CreateMap<SetUp_WorkDay, GetWorkDaysByQueryStringResponse>();
        CreateMap<SetUp_WorkDay, GetWorkDayByIdResponse>();
    }
}
