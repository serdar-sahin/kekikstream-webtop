using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace KekikStream.Webtop.Medias;

public interface IMediaAppService : IApplicationService
{
    Task<PluginModel>? GetPluginAsync(string pluginName);
    Task<List<PluginModel>?> GetPluginsAsync();
    Task<List<MainPageResult>?> GetMainPageAsync(string pluginName, string categoryUrl, string categoryName);
    Task<List<SearchResult>?> SearchAsync(string pluginName, string query);
    Task<MediaInfo>? GetMediaInfoAsync(string pluginName, string url);
    Task<List<VideoLink>>? GetVideoLinksAsync(string pluginName, string url);
    Task<List<VideoSource>>? GetVideoSourcesAsync(string url, string referrer);
}
