using KekikStream.Webtop.Samples;
using Xunit;

namespace KekikStream.Webtop.EntityFrameworkCore.Applications;

[Collection(WebtopTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<WebtopEntityFrameworkCoreTestModule>
{

}
