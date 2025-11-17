using ScoreSharp.API.Modules.SetUp.HighRiskCountry.GetHighRiskCountriesByQueryString;
using ScoreSharp.API.Modules.SetUp.HighRiskCountry.GetHighRiskCountryById;
using ScoreSharp.API.Modules.SetUp.HighRiskCountry.InsertHighRiskCountry;

namespace ScoreSharp.API.Infrastructures.Data.Mapper
{
    public class SetUp_HighRiskCountryProfiler : Profile
    {
        public SetUp_HighRiskCountryProfiler()
        {
            CreateMap<SetUp_HighRiskCountry, GetHighRiskCountriesByQueryStringResponse>();
            CreateMap<InsertHighRiskCountryRequest, SetUp_HighRiskCountry>();
            CreateMap<SetUp_HighRiskCountry, GetHighRiskCountryByIdResponse>();
        }
    }
}
