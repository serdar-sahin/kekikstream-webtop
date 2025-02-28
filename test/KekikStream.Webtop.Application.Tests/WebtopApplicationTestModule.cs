using Volo.Abp.Modularity;

namespace KekikStream.Webtop;

[DependsOn(
    typeof(WebtopApplicationModule),
    typeof(WebtopDomainTestModule)
)]
public class WebtopApplicationTestModule : AbpModule
{

}
