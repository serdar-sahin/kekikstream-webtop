using KekikStream.Webtop.Localization;
using Volo.Abp.Application.Services;

namespace KekikStream.Webtop;

/* Inherit your application services from this class.
 */
public abstract class WebtopAppService : ApplicationService
{
    protected WebtopAppService()
    {
        LocalizationResource = typeof(WebtopResource);
    }
}
