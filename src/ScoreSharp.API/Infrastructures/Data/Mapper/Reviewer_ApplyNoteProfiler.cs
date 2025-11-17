using ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyNoteByApplyNo;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class Reviewer_ApplyNoteProfiler : Profile
{
    public Reviewer_ApplyNoteProfiler()
    {
        CreateMap<Reviewer_ApplyNote, GetApplyNoteByApplyNoResponse>()
            .ForMember(dest => dest.UserTypeName, opt => opt.MapFrom(src => src.UserType.ToString()));
    }
}
