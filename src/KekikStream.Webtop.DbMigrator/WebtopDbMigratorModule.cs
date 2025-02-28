using KekikStream.Webtop.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace KekikStream.Webtop.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(WebtopEntityFrameworkCoreModule),
    typeof(WebtopApplicationContractsModule)
)]
public class WebtopDbMigratorModule : AbpModule
{
}
