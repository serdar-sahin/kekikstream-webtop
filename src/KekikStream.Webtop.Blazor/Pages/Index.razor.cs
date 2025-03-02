using KekikStream.Webtop.Extensions;
using KekikStream.Webtop.Medias;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.JSInterop;
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

    private List<SearchResult>? searchResults;
    private string? query;

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
        searchResults = null;
        pageName = "";

        plugin = await mediaService.GetPluginAsync(name);

        if (plugin != null)
        {
            if (plugin.MainPage != null)
            {
                var firstPage = plugin.MainPage[0];
                pageName = firstPage.Title;
                pages = await mediaService.GetMainPageAsync(plugin.Name, 1, firstPage.Url, firstPage.Title);
                searchResults=  ObjectMapper.Map(pages, searchResults);
                //Debug.WriteLine("results: " + searchResults?.ToJson());
            }
            
            //Debug.WriteLine("Plugin: " + plugin.ToJson());
        }
        else
        {
            ShowInfo(false);
        }

        await js.InvokeVoidAsync("backToTop");
        isBusy = false;
    }

    private async Task GetPages(string url, string name)
    {
        isBusy = true;
        pages = null;
        searchResults = null;
        pageName = "";

        if (plugin != null)
        {
            pageName = name;
            pages = await mediaService.GetMainPageAsync(plugin.Name, 1, url, name);

            if (pages != null)
            {
                searchResults = ObjectMapper.Map(pages, searchResults);
                //Debug.WriteLine("results: " + searchResults?.ToJson());
            }
            else
            {
                ShowInfo(false);
                //Debug.WriteLine("Pages: " + pages.ToJson());
            }
        }

        await js.InvokeVoidAsync("backToTop");
        isBusy = false;
    }

    private async Task Search()
    {
        isBusy = true;
        pages = null;
        searchResults = null;
        pageName = L["SearchResults"];

        if (plugin != null && query != null)
        {
            pageName = L["SearchResults"] + ": " + query;
            searchResults = await mediaService.SearchAsync(plugin.Name, query);

            if (searchResults != null)
            {
                
            }
            else
            {
                ShowInfo(false);
                //Debug.WriteLine("Pages: " + pages.ToJson());
            }
        }

        query = null;
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
