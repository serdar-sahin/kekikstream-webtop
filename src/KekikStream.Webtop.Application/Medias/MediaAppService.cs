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

namespace KekikStream.Webtop.Medias;

public class MediaAppService : WebtopAppService, IMediaAppService
{
    private readonly IHostEnvironment _hostEnvironment;
    
    public MediaAppService(IHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
      
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
}
