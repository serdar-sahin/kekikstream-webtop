using KekikStream.Webtop.Extensions;
using KekikStream.Webtop.Medias;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace KekikStream.Webtop.Blazor.Pages;

public partial class Index
{
    private List<PluginModel>? plugins;
    private PluginModel? plugin;

    private List<MainPageResult>? pages;
    private string? pageName;

    private bool isBusy = false;
    private bool isWarning = false;

    protected override async Task OnInitializedAsync()
    {
       await GetPlugins();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            
        }
    }

    private async Task GetPlugins()
    {
        isBusy = true;
        plugins = await mediaService.GetPluginNamesAsync();

        if (plugins != null)
        {
            isWarning = false;
            //Debug.WriteLine(plugins.ToJson());
        }
        else
        {
            isWarning = true;
        }

        isBusy = false;
    }

    private async Task SetPlugin(string name)
    {
        isBusy = true;
        pages = null;

        plugin = await mediaService.GetPluginAsync(name);

        if (plugin is null)
        {
            ShowInfo(false);
            //Debug.WriteLine("Plugin: " + plugin.ToJson());
        }
        isBusy = false;
    }

    private async Task GetPages(string url, string name)
    {
        isBusy = true;
        pages = null;

        if(plugin != null)
        {
            pageName = name;
            pages = await mediaService.GetMainPageAsync(plugin.Name, 1, url, name);

            if (pages is null)
            {
                ShowInfo(false);
                //Debug.WriteLine("Pages: " + pages.ToJson());
            }
        }
        isBusy = false;
    }

    private async void ShowInfo(bool isOk)
    {
        if (isOk)
        {
            await Notify.Success(L["SuccessMessage"]);
        }
        else
        {
            await Notify.Error(L["ErrorMessage"]);
        }
    }
}
