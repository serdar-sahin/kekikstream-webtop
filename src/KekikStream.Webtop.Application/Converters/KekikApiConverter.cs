using KekikStream.Webtop.Medias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KekikStream.Webtop.Converters
{
    public class KekikApiConverter : IKekikApiConverter
    {
        
        public Task<PluginModel>? ConvertPluginModel(string json)
        {
            throw new NotImplementedException();
        }

        public Task<List<MainPageResult>?> ConvertMainPageResult(string json)
        {
            throw new NotImplementedException();
        }

        public Task<List<SearchResult>?> ConvertSearchResult(string json)
        {
            throw new NotImplementedException();
        }

        public Task<MediaInfo>? ConvertMediaInfo(string json)
        {
            throw new NotImplementedException();
        }

        public Task<List<VideoLink>>? ConvertVideoLinks(string json)
        {
            throw new NotImplementedException();
        }

        public Task<List<VideoSource>>? ConvertVideoSources(string json)
        {
            throw new NotImplementedException();
        }
    }
}
