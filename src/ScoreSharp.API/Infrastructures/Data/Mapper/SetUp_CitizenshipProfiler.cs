using ScoreSharp.API.Modules.SetUp.Citizenship.GetCitizenshipById;
using ScoreSharp.API.Modules.SetUp.Citizenship.GetCitizenshipsByQueryString;
using ScoreSharp.API.Modules.SetUp.Citizenship.InsertCitizenship;

namespace ScoreSharp.API.Infrastructures.Data.Mapper
{
    public class SetUp_CitizenshipProfiler : Profile
    {
        public SetUp_CitizenshipProfiler()
        {
            CreateMap<InsertCitizenshipRequest, SetUp_Citizenship>();
            CreateMap<SetUp_Citizenship, GetCitizenshipsByQueryStringResponse>();
            CreateMap<SetUp_Citizenship, GetCitizenshipByIdResponse>();
        }
    }
}
