using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KekikStream.Webtop.Settings
{
    public class PluginSettings
    {
        public const string Settings = "Settings";
        public string Version { get; set; }
        public string Author { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string SocketUrl { get; set; }
        public string LogoUrl { get; set; }
        public string SearchUrl { get; set; }
        public string MoviesUrl { get; set; }
        public string SeriesUrl { get; set; }
        public string PosterUrl { get; set; }
        public string CoverUrl { get; set; }
        public bool IsMovies { get; set; }
        public bool IsSeries { get; set; }
    }

    public class PluginInfo
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Logo { get; set; }
        public PluginType Type { get; set; }
    }

    public enum PluginType
    {
        None = 0,
        Video,
        Music,
        IpTv,
        IpRadio,
        Service
    }

    public enum SocketCommand
    {
        None = 0,
        Test,
        Message,
        GetSettings,
        SaveSettings,
        Search,
        GetCategories,
        GetMovies,
        GetMoviesWithCategories,
        GetMovieInfo,
        GetSeries,
        GetSeriesWithCategories,
        GetSeriesMainInfo,
        GetSeriesInfo,
        Dispose
    }
}
