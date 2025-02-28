using Volo.Abp.Modularity;

namespace KekikStream.Webtop;

/* Inherit from this class for your domain layer tests. */
public abstract class WebtopDomainTestBase<TStartupModule> : WebtopTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
