using KekikStream.Webtop.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace KekikStream.Webtop.Permissions;

public class WebtopPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(WebtopPermissions.GroupName);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(WebtopPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<WebtopResource>(name);
    }
}
