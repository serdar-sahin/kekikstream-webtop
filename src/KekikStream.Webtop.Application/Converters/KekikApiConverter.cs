using KekikStream.Webtop.Medias;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace KekikStream.Webtop.Converters
{
    /// <summary>
    /// https://github.com/keyiflerolsun/KekikStreamAPI
    /// </summary>
    public class KekikApiConverter : IKekikApiConverter
    {

        public async Task<List<PluginModel>?> ConvertPluginsModel(string json)
        {
            try
            {
                //JObject? result = JObject.Parse(json);
                JObject? result = await JObject.LoadAsync(new JsonTextReader(new StringReader(json)));

                if (result != null)
                {
                    //Debug.WriteLine("With: " + result["with"]);

                    var pluginModels = new List<PluginModel>();

                    JArray resultArray = (JArray)result["result"];
                    
                    foreach (var item in resultArray)
                    {
                        var pluginModel = new PluginModel()
                        {
                            Name = item.ToString(),
                        };

                        pluginModels.Add(pluginModel);
                    }

                    return pluginModels;
                }
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }

            return null;

            /*
             url = http://localhost:3310/api/v1/get_plugin_names
             *{
              "with": "https://github.com/keyiflerolsun/KekikStream",
              "result": [
                "DiziBox",
                "DiziYou",
                "Dizilla",
                "FilmMakinesi",
                "FullHDFilmizlesene",
                "HDFilmCehennemi",
                "JetFilmizle",
                "RecTV",
                "SezonlukDizi",
                "Shorten",
                "SineWix",
                "UgurFilm"
              ]
            }
             */
        }

        public async Task<PluginModel?> ConvertPluginModel(string json)
        {
            try
            {
                //JObject? result = JObject.Parse(json);
                JObject? result = await JObject.LoadAsync(new JsonTextReader(new StringReader(json)));

                if(result != null)
                {
                    //Debug.WriteLine("With: " + result["with"]);

                    var pluginModel = new PluginModel();

                    pluginModel.Name = result["result"]["name"].ToString();
                    pluginModel.Language = result["result"]["language"].ToString();
                    pluginModel.Url = result["result"]["main_url"].ToString();
                    pluginModel.Description = result["result"]["description"].ToString();

                    pluginModel.MainPage = new List<GenericTitleUrlItem>();

                    JObject mainPage = (JObject)result["result"]["main_page"];
                    foreach (var page in mainPage)
                    {
                        var item = new GenericTitleUrlItem()
                        {
                            Url = WebUtility.UrlDecode(page.Key),
                            Title = WebUtility.UrlDecode(page.Value.ToString())
                        };

                        pluginModel.MainPage.Add(item);

                        //Debug.WriteLine("URL: " + page.Key + ", Title: " + page.Value);
                    }

                    return pluginModel;
                }
            }
            catch (Exception ex)
            {
                Log(ex.ToString()); 
            }

            return null;

            /* 
             url = http://localhost:3310/api/v1/get_plugin?plugin=Dizilla
            {
              "with": "https://github.com/keyiflerolsun/KekikStream",
              "result": {
                "name": "Dizilla",
                "language": "tr",
                "main_url": "https://dizilla.club",
                "favicon": "https://www.google.com/s2/favicons?domain=https://dizilla.club&sz=64",
                "description": "Dizilla tüm yabancı dizileri ücretsiz olarak Türkçe Dublaj ve altyazılı seçenekleri ile 1080P kalite izleyebileceğiniz yeni nesil yabancı dizi izleme siteniz.",
                "main_page": {
                  "https%3A%2F%2Fdizilla.club%2Ftum-bolumler": "Altyaz%C4%B1l%C4%B1+B%C3%B6l%C3%BCmler",
                  "https%3A%2F%2Fdizilla.club%2Fdublaj-bolumler": "Dublaj+B%C3%B6l%C3%BCmler",
                  "https%3A%2F%2Fdizilla.club%2Fdizi-turu%2Faile": "Aile",
                  "https%3A%2F%2Fdizilla.club%2Fdizi-turu%2Faksiyon": "Aksiyon",
                  "https%3A%2F%2Fdizilla.club%2Fdizi-turu%2Fbilim-kurgu": "Bilim+Kurgu",
                  "https%3A%2F%2Fdizilla.club%2Fdizi-turu%2Fromantik": "Romantik",
                  "https%3A%2F%2Fdizilla.club%2Fdizi-turu%2Fkomedi": "Komedi"
                }
              }
            }
            */
        }

        public async Task<List<MainPageResult>?> ConvertMainPageResult(string pluginName, string json)
        {
            try
            {
                Debug.WriteLine(json);
                //JObject? result = JObject.Parse(json);
                JObject? result = await JObject.LoadAsync(new JsonTextReader(new StringReader(json)));

                if (result != null)
                {
                    //Debug.WriteLine("With: " + result["with"]);

                    var mainPageResults = new List<MainPageResult>();

                    JArray resultArray = (JArray)result["result"];

                    foreach (var item in resultArray)
                    {
                        var page = new MainPageResult()
                        {
                            PluginName = pluginName,
                            Title = item["title"].ToString(),
                            Url = item["url"].ToString(),
                            Category = item["category"].ToString(),
                            Poster = item["poster"].ToString(),

                            //Title = WebUtility.HtmlDecode(item["title"].ToString()),
                            //Url = WebUtility.HtmlDecode(item["url"].ToString()),
                            //Category = WebUtility.HtmlDecode(item["category"].ToString()),
                            //Poster = WebUtility.HtmlDecode(item["poster"].ToString()),
                        };

                        mainPageResults.Add(page);
                    }

                    return mainPageResults;
                }
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }

            return null;

            /*
             url = http://localhost:3310/api/v1/get_main_page?plugin=Dizilla&page=1&encoded_url=https://dizilla.club/dizi-turu/aksiyon&encoded_category=Aksiyon
            {
              "with": "https://github.com/keyiflerolsun/KekikStream",
              "result": [
                {
                  "category": "Aksiyon",
                  "title": "Skeleton Crew",
                  "url": "https%3A%2F%2Fdizilla.club%2Fdizi%2Fskeleton-crew-c01",
                  "poster": "data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHZpZXdCb3g9IjAgMCAxNTMgMjMyIiB3aWR0aD0iMTUzIiBoZWlnaHQ9IjIzMiI+CiAgPHJlY3Qgd2lkdGg9IjE1MyIgaGVpZ2h0PSIyMzIiIGZpbGw9IiMyNjI2MjZGRiI+PC9yZWN0PgogIDx0ZXh0IHg9IjUwJSIgeT0iNTAlIiBkb21pbmFudC1iYXNlbGluZT0ibWlkZGxlIiB0ZXh0LWFuY2hvcj0ibWlkZGxlIiBmb250LWZhbWlseT0ibW9ub3NwYWNlIiBmb250LXNpemU9IjI2cHgiIGZpbGw9IiMzMzMzMzMiPiA8L3RleHQ+ICAgCjwvc3ZnPg=="
                },
                {
                  "category": "Aksiyon",
                  "title": "Baki Hanma VS Kengan Ashura",
                  "url": "https%3A%2F%2Fdizilla.club%2Fdizi%2Fbaki-hanma-vs-kengan-ashura",
                  "poster": "data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHZpZXdCb3g9IjAgMCAxNTMgMjMyIiB3aWR0aD0iMTUzIiBoZWlnaHQ9IjIzMiI+CiAgPHJlY3Qgd2lkdGg9IjE1MyIgaGVpZ2h0PSIyMzIiIGZpbGw9IiMyNjI2MjZGRiI+PC9yZWN0PgogIDx0ZXh0IHg9IjUwJSIgeT0iNTAlIiBkb21pbmFudC1iYXNlbGluZT0ibWlkZGxlIiB0ZXh0LWFuY2hvcj0ibWlkZGxlIiBmb250LWZhbWlseT0ibW9ub3NwYWNlIiBmb250LXNpemU9IjI2cHgiIGZpbGw9IiMzMzMzMzMiPiA8L3RleHQ+ICAgCjwvc3ZnPg=="
                }
            }
            */
        }

        public async Task<List<SearchResult>?> ConvertSearchResult(string pluginName, string json)
        {
            try
            {
                //JObject? result = JObject.Parse(json);
                JObject? result = await JObject.LoadAsync(new JsonTextReader(new StringReader(json)));

                if (result != null)
                {
                    //Debug.WriteLine("With: " + result["with"]);

                    var searchResults = new List<SearchResult>();

                    JArray resultArray = (JArray)result["result"];

                    foreach (var item in resultArray)
                    {
                        var page = new SearchResult()
                        {
                            PluginName = pluginName,
                            Title = item["title"].ToString(),
                            Url = item["url"].ToString(),
                            Poster = item["poster"].ToString(),
                        };

                        searchResults.Add(page);
                    }

                    return searchResults;
                }
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }

            return null;

            /*
            url = http://localhost:3310/api/v1/search?plugin=Dizilla&query=silo
           {
              "with": "https://github.com/keyiflerolsun/KekikStream",
              "result": [
                {
                  "title": "Silo",
                  "url": "https%3A%2F%2Fdizilla.club%2Fdizi%2Fsilo",
                  "poster": "https://file.macellan.online/images/f/f/100//silo--1685249685.jpg"
                }
              ]
            }
           */
        }

        public async Task<MediaInfo?> ConvertMediaInfo(string json)
        {
            try
            {
                //JObject? result = JObject.Parse(json);
                JObject? result = await JObject.LoadAsync(new JsonTextReader(new StringReader(json)));

                if (result != null)
                {
                    //Debug.WriteLine("With: " + result["with"]);

                    var mediaInfo = new MediaInfo();

                    mediaInfo.Title = result["result"]["title"].ToString();
                    mediaInfo.Description = result["result"]["description"].ToString();
                    mediaInfo.Url = result["result"]["main_url"].ToString();
                    mediaInfo.Poster = result["result"]["poster"].ToString();
                    mediaInfo.Tags = result["result"]["tags"].ToString();
                    mediaInfo.Rating = result["result"]["rating"].ToString();
                    mediaInfo.Year = result["result"]["year"].ToString();
                    mediaInfo.Actors = result["result"]["actors"].ToString();

                    mediaInfo.Episodes = new List<Episode>();

                    JArray resultArray = (JArray)result["result"]["episodes"];

                    foreach (var item in resultArray)
                    {
                        var episode = new Episode()
                        {
                            Season = (int)item["season"],
                            Title = item["title"].ToString(),
                            Url = item["url"].ToString(),
                            EpisodeNumber = (int)item["episode"],
                        };

                        mediaInfo.Episodes.Add(episode);
                    }

                    return mediaInfo;
                }
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }

            return null;

            /*
            url = http://localhost:3310/api/v1/load_item?plugin=Dizilla&encoded_url=https://dizilla.club/dizi/american-primeval
           {
              "with": "https://github.com/keyiflerolsun/KekikStream",
              "result": {
                "url": "https%3A%2F%2Fdizilla.club%2Fdizi%2Famerican-primeval",
                "poster": "https://images-macellan-online.cdn.ampproject.org/i/s/images.macellan.online/images/tv/backdrop/f/f/100/b47b8eep8vwo585a1162b07a5.jpg",
                "title": "American Primeval",
                "description": "Geçmişlerinden kaçan bir anne ve oğul, Amerikan Batı Yakası'nın özgürlük ve zalimlikle harmanlanmış sert coğrafyasında kendilerine bir aile kurarlar.",
                "tags": "",
                "rating": "1.5",
                "year": "2025",
                "actors": "Taylor Kitsch, Betty Gilpin, Dane DeHaan, Saura Lightfoot Leon, Derek Hinkey, Joe Tippett, Jai Courtney, Shawnee Pourier, Shea Whigham",
                "episodes": [
                  {
                    "season": 1,
                    "episode": 1,
                    "title": "",
                    "url": "https%3A%2F%2Fdizilla.club%2Famerican-primeval-1-sezon-1-bolum"
                  },
                  {
                    "season": 1,
                    "episode": 2,
                    "title": "",
                    "url": "https%3A%2F%2Fdizilla.club%2Famerican-primeval-1-sezon-2-bolum"
                  },
                  {
                    "season": 1,
                    "episode": 3,
                    "title": "",
                    "url": "https%3A%2F%2Fdizilla.club%2Famerican-primeval-1-sezon-3-bolum"
                  },
                  {
                    "season": 1,
                    "episode": 4,
                    "title": "",
                    "url": "https%3A%2F%2Fdizilla.club%2Famerican-primeval-1-sezon-4-bolum"
                  },
                  {
                    "season": 1,
                    "episode": 5,
                    "title": "",
                    "url": "https%3A%2F%2Fdizilla.club%2Famerican-primeval-1-sezon-5-bolum"
                  },
                  {
                    "season": 1,
                    "episode": 6,
                    "title": "",
                    "url": "https%3A%2F%2Fdizilla.club%2Famerican-primeval-1-sezon-6-bolum"
                  }
                ]
              }
            }
           */
        }

        public Task<List<VideoLink>?> ConvertVideoLinks(string json)
        {
            throw new NotImplementedException();
        }

        public Task<List<VideoSource>?> ConvertVideoSources(string json)
        {
            throw new NotImplementedException();
        }

        public void Log(string message)
        {
            Debug.WriteLine(message);
        }
    }
}
