using Xunit;

namespace KekikStream.Webtop.EntityFrameworkCore;

[CollectionDefinition(WebtopTestConsts.CollectionDefinitionName)]
public class WebtopEntityFrameworkCoreCollection : ICollectionFixture<WebtopEntityFrameworkCoreFixture>
{

}
