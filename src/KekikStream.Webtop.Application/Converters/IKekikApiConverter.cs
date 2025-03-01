using KekikStream.Webtop.Medias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Volo.Abp.DependencyInjection;

namespace KekikStream.Webtop.Converters
{
    public interface IKekikApiConverter: ISingletonDependency
    {
        Task<PluginModel>? ConvertPluginModel(string json);
        Task<List<MainPageResult>?> ConvertMainPageResult(string json);
        Task<List<SearchResult>?> ConvertSearchResult(string json);
        Task<MediaInfo>? ConvertMediaInfo(string json);
        Task<List<VideoLink>>? ConvertVideoLinks(string json);
        Task<List<VideoSource>>? ConvertVideoSources(string json);
    }
}
