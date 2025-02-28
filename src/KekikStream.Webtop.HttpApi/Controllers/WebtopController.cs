using KekikStream.Webtop.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace KekikStream.Webtop.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class WebtopController : AbpControllerBase
{
    protected WebtopController()
    {
        LocalizationResource = typeof(WebtopResource);
    }
}
