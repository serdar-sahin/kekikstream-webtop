using KekikStream.Webtop.Extensions;
using KekikStream.Webtop.Medias;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.JSInterop;
using System;
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

    private MediaInfo? mediaInfo;

    private bool isPagination = false;
    private int currentPageNumber = 1;
    private string currentCategoryName = "";
    private string currentCategoryUrl = "";

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
        isPagination = false;
        pages = null;
        searchResults = null;
        pageName = "";
        currentCategoryName = "";
        currentCategoryUrl = "";
        currentPageNumber = 1;

        plugin = await mediaService.GetPluginAsync(name);

        if (plugin != null)
        {
            if (plugin.MainPage != null && plugin.MainPage.Count > 0)
            {
                var firstPage = plugin.MainPage[0];
                pageName = firstPage.Title;
                currentCategoryName = firstPage.Title;
                currentCategoryUrl = firstPage.Url;

                pages = await mediaService.GetMainPageAsync(plugin.Name, 1, firstPage.Url, firstPage.Title);

                if (pages != null && pages.Count > 0)
                {
                    searchResults = ObjectMapper.Map(pages, searchResults);
                    isPagination = true;
                }
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
        isPagination = false;
        pages = null;
        searchResults = null;
        pageName = "";
        currentCategoryName = "";
        currentCategoryUrl = "";
        currentPageNumber = 1;

        if (plugin != null)
        {
            pageName = name;
            pages = await mediaService.GetMainPageAsync(plugin.Name, 1, url, name);

            if (pages != null && pages.Count > 0)
            {
                currentCategoryName = name;
                currentCategoryUrl = url;
                searchResults = ObjectMapper.Map(pages, searchResults);
                //Debug.WriteLine("results: " + searchResults?.ToJson());

                isPagination = true;
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

    private async Task NextPages(string direction)
    {
        if (currentCategoryName == "" && currentCategoryUrl == "")
        {
            return;
        }

        if (direction == "Prev" && currentPageNumber == 1)
        {
            return;
        }

        int pageNumber = 1;

        if (direction == "Next")
        {
            pageNumber = currentPageNumber+1;
        }
        else
        {
            pageNumber =  currentPageNumber-1;
        }

        isBusy = true;
        pages = null;
        searchResults = null;
        pageName = "";

        if (plugin != null)
        {
            pageName = currentCategoryName;
            pages = await mediaService.GetMainPageAsync(plugin.Name, pageNumber, currentCategoryUrl, currentCategoryName);

            if (pages != null && pages.Count > 0)
            {
                searchResults = ObjectMapper.Map(pages, searchResults);

                if(direction == "Next")
                {
                    currentPageNumber++;
                }
                else
                {
                    currentPageNumber--;
                }

                if(currentPageNumber <= 1)
                {
                    currentPageNumber = 1;
                }
                //Debug.WriteLine("results: " + searchResults?.ToJson());
            }
            else
            {
                isPagination = false;
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
        isPagination = false;
        pages = null;
        searchResults = null;
        pageName = L["SearchResults"];
        currentCategoryName = "";
        currentCategoryUrl = "";

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

    private async Task GetMediaInfo(SearchResult searchResult)
    {
        isBusy = true;

        if (plugin != null)
        {
            mediaInfo = await mediaService.GetMediaInfoAsync(plugin.Name, searchResult.Url);

            if (mediaInfo != null)
            {
                ShowInfo(true);
                await ShowMediaInfosComponentModal(mediaInfo);
            }
            else
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
