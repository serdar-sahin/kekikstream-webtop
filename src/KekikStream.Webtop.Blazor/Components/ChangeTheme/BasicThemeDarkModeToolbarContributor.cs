using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;


namespace KekikStream.Webtop.Blazor.Components.ChangeTheme
{
    public class BasicThemeDarkModeToolbarContributor : IToolbarContributor
    {
        public Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
        {
            if (context.Toolbar.Name == StandardToolbars.Main)
            {
                // ilk sıraya ekler
                //context.Toolbar.Items.Insert(0, new ToolbarItem(typeof(ChangeTheme)));

                // son sıraya ekler
                context.Toolbar.Items.Add(new ToolbarItem(typeof(ChangeTheme)));
            }

            return Task.CompletedTask;
        }
    }

}
