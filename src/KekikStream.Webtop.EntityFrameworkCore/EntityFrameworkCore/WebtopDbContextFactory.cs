using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace KekikStream.Webtop.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class WebtopDbContextFactory : IDesignTimeDbContextFactory<WebtopDbContext>
{
    public WebtopDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        WebtopEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<WebtopDbContext>()
            .UseSqlite(configuration.GetConnectionString("Default"));
        
        return new WebtopDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../KekikStream.Webtop.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
