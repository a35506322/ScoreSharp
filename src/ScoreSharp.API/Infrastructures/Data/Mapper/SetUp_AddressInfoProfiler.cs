using ScoreSharp.API.Modules.SetUp.AddressInfo.GetAddressInfoById;
using ScoreSharp.API.Modules.SetUp.AddressInfo.GetAddressInfosByQueryString;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class SetUp_AddressInfoProfiler : Profile
{
    public SetUp_AddressInfoProfiler()
    {
        CreateMap<SetUp_AddressInfo, GetAddressInfosByQueryStringResponse>();
        CreateMap<SetUp_AddressInfo, GetAddressInfoByIdResponse>();
    }
}
