using AutoMapper;
using KekikStream.Webtop.Medias;

namespace KekikStream.Webtop.Blazor;

public class WebtopBlazorAutoMapperProfile : Profile
{
    public WebtopBlazorAutoMapperProfile()
    {
        //Define your AutoMapper configuration here for the Blazor project.
        CreateMap<MainPageResult, SearchResult>();
    }
}
