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
        /// <summary>
        /// Convert KekikStreamAPI plugins model to <see cref="PluginModel"/> list 
        /// </summary>
        /// <returns>
        /// <see cref="PluginModel"/> 
        /// </returns>
        /// <param name="json"></param>
        /// <example>
        /// </example>
        Task<List<PluginModel>?> ConvertPluginsModel(string json);

        /// <summary>
        /// Convert KekikStreamAPI plugin model to <see cref="PluginModel"/> 
        /// </summary>
        /// <returns>
        /// <see cref="PluginModel"/> 
        /// </returns>
        /// <param name="json"></param>
        /// <example>
        /// </example>
        Task<PluginModel?> ConvertPluginModel(string json);

        /// <summary>
        /// Convert KekikStreamAPI main_page model to <see cref="MainPageResult"/> 
        /// </summary>
        /// <returns>
        /// <see cref="MainPageResult"/> 
        /// </returns>
        /// <param name="json"></param>
        Task<List<MainPageResult>?> ConvertMainPageResult(string pluginName, string json);

        /// <summary>
        /// Convert KekikStreamAPI search model to <see cref="SearchResult"/> 
        /// </summary>
        /// <returns>
        /// <see cref="SearchResult"/> 
        /// </returns>
        /// <param name="json"></param>
        Task<List<SearchResult>?> ConvertSearchResult(string pluginName, string json);

        /// <summary>
        /// Convert KekikStreamAPI movie_item model to <see cref="MediaInfo"/> 
        /// </summary>
        /// <returns>
        /// <see cref="MediaInfo"/> 
        /// </returns>
        /// <param name="json"></param>
        Task<MediaInfo?> ConvertMediaInfo(string json);

        /// <summary>
        /// Convert KekikStreamAPI link  model to <see cref="VideoLink"/> 
        /// </summary>
        /// <returns>
        /// <see cref="VideoLink"/> 
        /// </returns>
        /// <param name="json"></param>
        Task<VideoLink?> ConvertVideoLinks(string json);

        /// <summary>
        /// Convert KekikStreamAPI extract_url model to <see cref="VideoSource"/> 
        /// </summary>
        /// <returns>
        /// <see cref="VideoSource"/> 
        /// </returns>
        /// <param name="json"></param>
        Task<List<VideoSourceModel>?> ConvertVideoSources(string json);
    }
}
