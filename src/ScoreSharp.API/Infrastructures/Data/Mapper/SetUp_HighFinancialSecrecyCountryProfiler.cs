using ScoreSharp.API.Modules.SetUp.HighFinancialSecrecyCountry.GetHighFinancialSecrecyCountriesByQueryString;
using ScoreSharp.API.Modules.SetUp.HighFinancialSecrecyCountry.GetHighFinancialSecrecyCountryById;
using ScoreSharp.API.Modules.SetUp.HighFinancialSecrecyCountry.InsertHighFinancialSecrecyCountry;

namespace ScoreSharp.API.Infrastructures.Data.Mapper
{
    public class SetUp_HighFinancialSecrecyCountryProfiler : Profile
    {
        public SetUp_HighFinancialSecrecyCountryProfiler()
        {
            CreateMap<InsertHighFinancialSecrecyCountryRequest, SetUp_HighFinancialSecrecyCountry>();
            CreateMap<SetUp_HighFinancialSecrecyCountry, GetHighFinancialSecrecyCountriesByQueryStringResponse>();
            CreateMap<SetUp_HighFinancialSecrecyCountry, GetHighFinancialSecrecyCountryByIdResponse>();
        }
    }
}
