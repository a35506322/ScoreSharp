using ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyCreditCardInfoByApplyNo;
using ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateApplicationInfoById;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class Reviewer_ApplyCreditCardInfoMainProfiler : Profile
{
    public Reviewer_ApplyCreditCardInfoMainProfiler()
    {
        CreateMap<Reviewer_ApplyCreditCardInfoMain, MainInfo>();
        CreateMap<Reviewer_ApplyCreditCardInfoMain, Primary_BasicInfo>();
        CreateMap<Reviewer_ApplyCreditCardInfoMain, Primary_JobInfo>();
        CreateMap<Reviewer_ApplyCreditCardInfoMain, Primary_StudentInfo>();
        CreateMap<Reviewer_ApplyCreditCardInfoMain, Primary_WebCardInfo>();
        CreateMap<Reviewer_ApplyCreditCardInfoMain, Primary_ActivityInfo>();
        CreateMap<UpdateApplicationInfoByIdRequest, Reviewer_ApplyCreditCardInfoMain>();
    }
}
