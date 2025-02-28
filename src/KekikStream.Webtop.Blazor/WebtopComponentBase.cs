using KekikStream.Webtop.Localization;
using Volo.Abp.AspNetCore.Components;

namespace KekikStream.Webtop.Blazor;

public abstract class WebtopComponentBase : AbpComponentBase
{
    protected WebtopComponentBase()
    {
        LocalizationResource = typeof(WebtopResource);
    }
}
