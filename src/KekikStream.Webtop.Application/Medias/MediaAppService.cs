using Microsoft.Extensions.Logging;
using static Volo.Abp.Http.MimeTypes;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Hosting;
using System.Web;
using System.Collections;
using KekikStream.Webtop.Converters;

namespace KekikStream.Webtop.Medias;

public class MediaAppService : WebtopAppService, IMediaAppService
{
    private readonly IHostEnvironment _hostEnvironment;
    private readonly IKekikApiConverter _kekikApiConverter;

    public MediaAppService(IHostEnvironment hostEnvironment, IKekikApiConverter kekikApiConverter)
    {
        _hostEnvironment = hostEnvironment;
        _kekikApiConverter = kekikApiConverter;
      
    }

    public async Task<string> HttpPost(string url, string query)
    {
        try
        {
            Uri uri = new Uri(url);

            var q = new Dictionary<string, string>();
            q.Add("query", query);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Referer", url);
                client.DefaultRequestHeaders.Add("x-requested-with", "XMLHttpRequest");
                client.DefaultRequestHeaders.Add("authority", uri.Authority);
                client.DefaultRequestHeaders.Add("origin", url);
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.81 Safari/537.36");

                using (HttpResponseMessage response = await client.PostAsync(url, new FormUrlEncodedContent(q)))
                {
                    using (HttpContent content = response.Content)
                    {

                        string json = await content.ReadAsStringAsync();
                        return json;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //logger.LogError(ex.ToString());
        }

        return string.Empty;
    }

    public async Task<string> HttpGet(string url)
    {
        try
        {
            Uri uri = new Uri(url);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Referer", url);
                client.DefaultRequestHeaders.Add("x-requested-with", "XMLHttpRequest");
                client.DefaultRequestHeaders.Add("authority", uri.Authority);
                client.DefaultRequestHeaders.Add("origin", url);
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.81 Safari/537.36");

                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    using (HttpContent content = response.Content)
                    {

                        string json = await content.ReadAsStringAsync();
                        return json;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //logger.LogError(ex.ToString());
        }

        return string.Empty;
    }


    public Task<PluginModel>? GetPluginAsync(string pluginName)
    {
        throw new NotImplementedException();
    }

    public Task<List<PluginModel>?> GetPluginsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<MainPageResult>?> GetMainPageAsync(string pluginName, string categoryUrl, string categoryName)
    {
        throw new NotImplementedException();
    }

    public Task<List<SearchResult>?> SearchAsync(string pluginName, string query)
    {
        throw new NotImplementedException();
    }

    public Task<MediaInfo>? GetMediaInfoAsync(string pluginName, string url)
    {
        throw new NotImplementedException();
    }

    public Task<List<VideoLink>>? GetVideoLinksAsync(string pluginName, string url)
    {
        throw new NotImplementedException();
    }

    public Task<List<VideoSource>>? GetVideoSourcesAsync(string url, string referrer)
    {
        throw new NotImplementedException();
    }
}
