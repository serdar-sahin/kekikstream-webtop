using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace KekikStream.Webtop.Medias;

public interface IMediaAppService : IApplicationService
{
    Task<PluginModel>? GetPluginAsync();
    Task<List<PluginModel>?> GetPluginsAsync();
}
