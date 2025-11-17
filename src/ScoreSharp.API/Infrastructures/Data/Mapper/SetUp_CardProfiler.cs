using ScoreSharp.API.Modules.SetUp.Card.GetCardById;
using ScoreSharp.API.Modules.SetUp.Card.GetCardsByQueryString;
using ScoreSharp.API.Modules.SetUp.Card.InsertCard;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class SetUp_CardProfiler : Profile
{
    public SetUp_CardProfiler()
    {
        CreateMap<InsertCardRequest, SetUp_Card>();
        CreateMap<SetUp_Card, GetCardByIdResponse>()
            .ForMember(x => x.CardCategoryName, opt => opt.MapFrom(src => src.CardCategory.ToString()))
            .ForMember(x => x.SaleLoanCategoryName, opt => opt.MapFrom(src => src.SaleLoanCategory.ToString()))
            .ForMember(x => x.SampleRejectionLetterName, opt => opt.MapFrom(src => src.SampleRejectionLetter.ToString()));
        CreateMap<SetUp_Card, GetCardsByQueryStringResponse>()
            .ForMember(x => x.CardCategoryName, opt => opt.MapFrom(src => src.CardCategory.ToString()))
            .ForMember(x => x.SaleLoanCategoryName, opt => opt.MapFrom(src => src.SaleLoanCategory.ToString()))
            .ForMember(x => x.SampleRejectionLetterName, opt => opt.MapFrom(src => src.SampleRejectionLetter.ToString()));
    }
}
