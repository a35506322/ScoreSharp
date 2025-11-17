using ScoreSharp.API.Modules.SetUp.EUCountry.GetEUCountriesByQueryString;
using ScoreSharp.API.Modules.SetUp.EUCountry.GetEUCountryById;
using ScoreSharp.API.Modules.SetUp.EUCountry.InsertEUCountry;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class SetUp_EUCountryProfiler : Profile
{
    public SetUp_EUCountryProfiler()
    {
        CreateMap<InsertEUCountryRequest, SetUp_EUCountry>();
        CreateMap<SetUp_EUCountry, GetEUCountryByIdResponse>();
        CreateMap<SetUp_EUCountry, GetEUCountriesByQueryStringResponse>();
    }
}
