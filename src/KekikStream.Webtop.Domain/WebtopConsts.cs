using Volo.Abp.Identity;

namespace KekikStream.Webtop;

public static class WebtopConsts
{
    public const string DbTablePrefix = "App";
    public const string? DbSchema = null;
    public const string AdminEmailDefaultValue = "admin@test.com"; //IdentityDataSeedContributor.AdminEmailDefaultValue;
    public const string AdminPasswordDefaultValue = "123654789"; //IdentityDataSeedContributor.AdminPasswordDefaultValue;
}
