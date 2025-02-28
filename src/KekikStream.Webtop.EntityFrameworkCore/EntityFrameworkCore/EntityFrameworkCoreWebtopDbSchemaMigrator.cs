using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using KekikStream.Webtop.Data;
using Volo.Abp.DependencyInjection;

namespace KekikStream.Webtop.EntityFrameworkCore;

public class EntityFrameworkCoreWebtopDbSchemaMigrator
    : IWebtopDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreWebtopDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the WebtopDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<WebtopDbContext>()
            .Database
            .MigrateAsync();
    }
}
