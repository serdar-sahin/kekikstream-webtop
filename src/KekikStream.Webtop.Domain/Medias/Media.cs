using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KekikStream.Webtop.Medias
{
    /// <summary>
    /// main page results
    /// </summary>
    public class MainPageResult
    {
        public string PluginName { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string? Poster { get; set; } = null;
    }

    /// <summary>
    /// search results
    /// </summary>
    public class SearchResult
    {
        public string PluginName { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string? Poster { get; set; } = null;
    }


    /// <summary>
    /// movie,series media info
    /// </summary>
    public class MediaInfo
    {
        public string? Url { get; set; } = null;
        public string? Poster { get; set; } = null;
        public string? Title { get; set; } = null;
        public string? Description { get; set; } = null;
        public string? Tags { get; set; } = null;
        public string? Rating { get; set; } = null;
        public string? Year { get; set; } = null;
        public string? Duration { get; set; } = null;
        public string? Actors { get; set; } = null;
        public List<Episode>? Episodes { get; set; } = null;

        public void SetTags(object value)
        {
            Tags = ConvertLists(value);
        }

        public void SetActors(object value)
        {
            Actors = ConvertLists(value);
        }

        public void SetRating(object value)
        {
            //Rating = EnsureString(value);
        }

        public void SetYear(object value)
        {
            Year = EnsureString(value);
        }

        private string ConvertLists(object value)
        {
            if (value is List<string> list)
            {
                return string.Join(", ", list);
            }
            return value as string;
        }

        private string EnsureString(object value)
        {
            return value?.ToString();
        }
    }

    /// <summary>
    /// episode info
    /// </summary>
    public class Episode
    {
        public int? Season { get; set; } = null;
        public int? EpisodeNumber { get; set; } = null;
        public string Title { get; set; } = null;
        public string Url { get; set; } = null;

        public Episode CheckTitle()
        {
            if (string.IsNullOrEmpty(Title))
            {
                Title = "";
            }

            var keywords = new List<string> { "bölüm", "sezon", "episode" };
            if (keywords.Any(keyword => Title.ToLower().Contains(keyword)))
            {
                Title = "";
            }

            return this;
        }
    }

    public class PluginModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Language { get; set; }
        public List<GenericTitleUrlItem> MainPage { get; set; }

        //public List<Dictionary<string, string>> MainPage { get; set; }

        public string GetIcon()
        {
            // http://www.google.com/s2/favicons?domain=stackoverflow.com
            try
            {
                Uri myUri = new Uri(Url);
                string host = myUri.Host;
                host = host.Replace("www.", "");
                Icon = $"http://www.google.com/s2/favicons?domain={host}";
                return Icon;
            }
            catch 
            {
                return "./images/x_icon.png";
            }
            
        }
       
    }

    public class VideoLink
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class VideoSource
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Referer { get; set; }
        public List<GenericItem> Headers { get; set; }
        public List<Subtitle> Subtitles { get; set; }
    }

    public class Subtitle
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class GenericItem
    {
        public string Item { get; set; }
    }

    public class GenericTitleUrlItem
    {
        public string Title { get; set; }
        public string Url { get; set; }
    }
}
