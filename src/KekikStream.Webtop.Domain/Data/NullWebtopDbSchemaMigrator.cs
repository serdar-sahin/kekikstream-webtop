using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace KekikStream.Webtop.Data;

/* This is used if database provider does't define
 * IWebtopDbSchemaMigrator implementation.
 */
public class NullWebtopDbSchemaMigrator : IWebtopDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
