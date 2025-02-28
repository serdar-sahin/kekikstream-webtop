using System.Threading.Tasks;

namespace KekikStream.Webtop.Data;

public interface IWebtopDbSchemaMigrator
{
    Task MigrateAsync();
}
