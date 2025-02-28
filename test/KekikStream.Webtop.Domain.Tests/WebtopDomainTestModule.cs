using Volo.Abp.Modularity;

namespace KekikStream.Webtop;

[DependsOn(
    typeof(WebtopDomainModule),
    typeof(WebtopTestBaseModule)
)]
public class WebtopDomainTestModule : AbpModule
{

}
