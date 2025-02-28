using KekikStream.Webtop.Samples;
using Xunit;

namespace KekikStream.Webtop.EntityFrameworkCore.Domains;

[Collection(WebtopTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<WebtopEntityFrameworkCoreTestModule>
{

}
