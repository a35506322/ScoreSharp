using ScoreSharp.API.Modules.SetUp.MainIncomeAndFund.GetMainIncomeAndFundById;
using ScoreSharp.API.Modules.SetUp.MainIncomeAndFund.GetMainIncomeAndFundsByQueryString;
using ScoreSharp.API.Modules.SetUp.MainIncomeAndFund.InsertMainIncomeAndFund;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class SetUp_MainIncomeAndFundProfiler : Profile
{
    public SetUp_MainIncomeAndFundProfiler()
    {
        CreateMap<InsertMainIncomeAndFundRequest, SetUp_MainIncomeAndFund>();
        CreateMap<SetUp_MainIncomeAndFund, GetMainIncomeAndFundsByQueryStringResponse>();
        CreateMap<SetUp_MainIncomeAndFund, GetMainIncomeAndFundByIdResponse>();
    }
}
