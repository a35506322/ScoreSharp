using ScoreSharp.API.Modules.SetUp.IDCardRenewalLocation.GetIDCardRenewalLocationById;
using ScoreSharp.API.Modules.SetUp.IDCardRenewalLocation.GetIDCardRenewalLocationByQueryString;
using ScoreSharp.API.Modules.SetUp.IDCardRenewalLocation.InsertIDCardRenewalLocation;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class SetUp_IDCardRenewalLocationProfiler : Profile
{
    public SetUp_IDCardRenewalLocationProfiler()
    {
        CreateMap<InsertIDCardRenewalLocationRequest, SetUp_IDCardRenewalLocation>();
        CreateMap<SetUp_IDCardRenewalLocation, GetIDCardRenewalLocationByQueryStringResponse>();
        CreateMap<SetUp_IDCardRenewalLocation, GetIDCardRenewalLocationByIdResponse>();
    }
}
