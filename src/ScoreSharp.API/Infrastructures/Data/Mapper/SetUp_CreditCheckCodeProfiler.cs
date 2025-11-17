using ScoreSharp.API.Modules.SetUp.CreditCheckCode.GetCreditCheckCodeById;
using ScoreSharp.API.Modules.SetUp.CreditCheckCode.GetCreditCheckCodesByQueryString;
using ScoreSharp.API.Modules.SetUp.CreditCheckCode.InsertCreditCheckCode;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class SetUp_CreditCheckCodeProfiler : Profile
{
    public SetUp_CreditCheckCodeProfiler()
    {
        CreateMap<InsertCreditCheckCodeRequest, SetUp_CreditCheckCode>();
        CreateMap<SetUp_CreditCheckCode, GetCreditCheckCodesByQueryStringResponse>();
        CreateMap<SetUp_CreditCheckCode, GetCreditCheckCodeByIdResponse>();
    }
}
