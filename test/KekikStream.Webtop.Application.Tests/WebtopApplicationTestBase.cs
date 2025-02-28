using Volo.Abp.Modularity;

namespace KekikStream.Webtop;

public abstract class WebtopApplicationTestBase<TStartupModule> : WebtopTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
