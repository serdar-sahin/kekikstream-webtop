using Volo.Abp.Settings;

namespace KekikStream.Webtop.Settings;

public class WebtopSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(WebtopSettings.MySetting1));
    }
}
