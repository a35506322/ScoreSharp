using ScoreSharp.API.Modules.SetUp.UNSanctionCountry.GetUNSanctionCountriesByQueryString;
using ScoreSharp.API.Modules.SetUp.UNSanctionCountry.GetUNSanctionCountryById;
using ScoreSharp.API.Modules.SetUp.UNSanctionCountry.InsertUNSanctionCountry;

namespace ScoreSharp.API.Infrastructures.Data.Mapper
{
    public class SetUp_UNSanctionCountryProfiler : Profile
    {
        public SetUp_UNSanctionCountryProfiler()
        {
            CreateMap<InsertUNSanctionCountryRequest, SetUp_UNSanctionCountry>();
            CreateMap<SetUp_UNSanctionCountry, GetUNSanctionCountryByIdResponse>();
            CreateMap<SetUp_UNSanctionCountry, GetUNSanctionCountriesByQueryStringResponse>();
        }
    }
}
