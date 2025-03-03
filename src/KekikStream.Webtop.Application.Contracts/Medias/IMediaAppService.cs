using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace KekikStream.Webtop.Medias;

public interface IMediaAppService : IApplicationService
{
    Task<List<PluginModel>?> GetPluginNamesAsync();
    Task<PluginModel?> GetPluginAsync(string pluginName);
    Task<List<PluginModel>?> GetPluginsAsync();
    Task<List<MainPageResult>?> GetMainPageAsync(string pluginName, int page, string categoryUrl, string categoryName);
    Task<List<SearchResult>?> SearchAsync(string pluginName, string query);
    Task<MediaInfo?> GetMediaInfoAsync(string pluginName, string url);
    Task<VideoLink?> GetVideoLinksAsync(string pluginName, string url);
    Task<List<VideoSourceModel>?> GetVideoSourcesAsync(string url, string referrer);
}
