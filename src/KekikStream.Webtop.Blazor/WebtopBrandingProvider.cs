using Microsoft.Extensions.Localization;
using KekikStream.Webtop.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace KekikStream.Webtop.Blazor;

[Dependency(ReplaceServices = true)]
public class WebtopBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<WebtopResource> _localizer;

    public WebtopBrandingProvider(IStringLocalizer<WebtopResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
