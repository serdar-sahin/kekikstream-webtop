using System.Diagnostics;
using System.Threading.Tasks;

namespace KekikStream.Webtop.Blazor.Pages;

public partial class Index
{
    protected override async Task OnInitializedAsync()
    {
        //await pythonService.CheckGlobalPython();
        //var result = await pythonService.CheckLocalPython();
        

        //string json = await pythonService.HttpGet("http://localhost:3310/api/v1/get_plugin_names");
        //Debug.WriteLine(json);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var result = pythonService.CheckLocalPython();

            if (result)
            {
                string json = await pythonService.HttpGet("http://localhost:3310/api/v1/get_plugin_names");
                Debug.WriteLine(json);
            }
        }
    }
}
