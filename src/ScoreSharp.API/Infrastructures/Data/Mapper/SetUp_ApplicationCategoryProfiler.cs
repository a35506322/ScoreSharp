using ScoreSharp.API.Modules.SetUp.ApplicationCategory.InsertApplicationCategory;

namespace ScoreSharp.API.Infrastructures.Data.Mapper;

public class SetUp_ApplicationCategoryProfiler : Profile
{
    public SetUp_ApplicationCategoryProfiler()
    {
        CreateMap<InsertApplicationCategoryRequest, SetUp_ApplicationCategory>();
    }
}
