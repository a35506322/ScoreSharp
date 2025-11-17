using ScoreSharp.API.Modules.SetUp.EUSanctionCountry.GetEUSanctionCountriesByQueryString;
using ScoreSharp.API.Modules.SetUp.EUSanctionCountry.GetEUSanctionCountryById;
using ScoreSharp.API.Modules.SetUp.EUSanctionCountry.InsertEUSanctionCountry;

namespace ScoreSharp.API.Infrastructures.Data.Mapper
{
    public class SetUp_EUSanctionCountryProfiler : Profile
    {
        public SetUp_EUSanctionCountryProfiler()
        {
            CreateMap<InsertEUSanctionCountryRequest, SetUp_EUSanctionCountry>();
            CreateMap<SetUp_EUSanctionCountry, GetEUSanctionCountryByIdResponse>();
            CreateMap<SetUp_EUSanctionCountry, GetEUSanctionCountriesByQueryStringResponse>();
        }
    }
}
