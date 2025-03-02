using Microsoft.AspNetCore.Components;
using KekikStream.Webtop.Medias;
using System;
using System.Threading.Tasks;

namespace KekikStream.Webtop.Blazor.Components
{
    public partial class MediaInfosComponent : IAsyncDisposable
    {
        [Parameter]
        public MediaInfo? mediaInfo { get; set; }

        public ValueTask DisposeAsync()
        {
            throw new NotImplementedException();
        }
    }
}
