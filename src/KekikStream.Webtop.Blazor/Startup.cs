using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Modularity.PlugIns;
using KekikStream.Webtop.Hubs;

namespace KekikStream.Webtop.Blazor
{

    // cancelled: all settings in program.cs
    public class Startup
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public Startup(IWebHostEnvironment env)
        {
            _hostingEnvironment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // signalr üzerinden büyük boyutlu json veri gönderirken MaximumReceiveMessageSize aşıldı hatası veriyor.
            // bunun için aşağıdaki ayarlamalar yapıldı ve ilgili linkleri verildi.

            // *********
            // ileride, veriyi messagepack ile binary formatına çevirip performans testi yapılacak?
            // https://github.com/MessagePack-CSharp/MessagePack-CSharp
            // https://www.gencayyildiz.com/blog/net-coreda-messagepack-ile-binary-serialization/
            // https://scientificprogrammer.net/2022/09/28/advanced-signalr-configuration-fine-tuning-the-server-side-hub-and-all-supported-client-types/
            // *********

            // https://docs.telerik.com/blazor-ui/knowledge-base/common-increase-signalr-max-message-size
            services.AddServerSideBlazor().AddHubOptions(options => {
                options.MaximumReceiveMessageSize = null; // no limit or use a number (1024 * 1024; // 1MB )
            });

            // https://stackoverflow.com/questions/59248464/how-to-change-signalr-maximum-message-size
            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
                options.MaximumReceiveMessageSize = null;
            });

            // https://learn.microsoft.com/en-us/aspnet/core/blazor/tutorials/signalr-blazor?view=aspnetcore-5.0&tabs=visual-studio&pivots=server#add-services-and-an-endpoint-for-the-signalr-hub-1
            services.AddResponseCompression(options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });

            // https://docs.abp.io/en/abp/latest/PlugIn-Modules
            // https://github.com/abpframework/abp/issues/9252
            //services.AddApplication<WebtopBlazorModule>(options =>
            //{
            //    options.PlugInSources.AddFolder(Path.Combine(_hostingEnvironment.WebRootPath, "plugins"), SearchOption.AllDirectories);
            //});

            services.AddApplication<WebtopBlazorModule>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.InitializeApplication();

            app.UseResponseCompression();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<SocketHub>("/socket-hub", options =>
                {
                    options.Transports =
                        HttpTransportType.WebSockets |
                        HttpTransportType.LongPolling;
                });

            });
        }
    }
}
