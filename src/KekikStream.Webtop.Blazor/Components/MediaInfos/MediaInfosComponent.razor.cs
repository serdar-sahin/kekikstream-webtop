﻿using Microsoft.AspNetCore.Components;
using KekikStream.Webtop.Medias;
using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Notifications;
using System.Diagnostics;
using KekikStream.Webtop.Extensions;
using Blazorise.Video;
using Blazorise;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace KekikStream.Webtop.Blazor.Components.MediaInfos
{
    public partial class MediaInfosComponent : IAsyncDisposable
    {
        private readonly IUiNotificationService _notificationService;

        [Parameter]
        public PluginModel? plugin { get; set; }

        [Parameter]
        public MediaInfo? mediaInfo { get; set; }

        // blazorise video player
        private Video videoPlayer;
        private VideoSource videoSource;
        private List<Subtitle> subTitles;

        private VideoLink videoLink;

        private string subtitleSrcTr = "";
        private string subtitleSrcEn = "";

        private bool isBusy = false;
        private bool isVideoSource = false;
        private string videoUrl = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4";

        public MediaInfosComponent(IUiNotificationService notificationService) 
        {
            _notificationService = notificationService;
        }

        protected override async Task OnInitializedAsync()
        {
            videoSource = new VideoSource();

            //isBusy = true;
            //await Task.Delay(10000);
            //isBusy = false;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await GetVideoLinks(mediaInfo.Url);
            }
        }

        private void SetVideoSource(VideoSourceModel source)
        {
            isBusy = true;
            videoPlayer?.Stop();
            videoUrl = source.Url;
            subTitles = source.Subtitles;
            Debug.WriteLine(subTitles.ToJson());
            isBusy = false;

            StateHasChanged();
        }

        private async Task GetVideoLinks(string url)
        {
            isBusy = true;
            isVideoSource = false;

            if (plugin != null)
            {
                videoLink = await mediaService.GetVideoLinksAsync(plugin.Name, url);
                Debug.WriteLine(videoLink?.ToJson());

                if(videoLink != null && videoLink.VideoSources.Count > 0)
                {
                    isVideoSource = true;
                    videoUrl = videoLink.VideoSources[0].Url;
                    subTitles = videoLink.VideoSources[0].Subtitles;
                    //Debug.WriteLine(subTitles.ToJson());

                    videoSource = new VideoSource();

                    var videoMedias = new ValueEqualityList<VideoMedia>();
                    var videoTracks = new ValueEqualityList<VideoTrack>();

                    foreach (var source in videoLink.VideoSources)
                    {
                        var videoMedia = new VideoMedia(source.Url);
                        videoMedias.Add(videoMedia);

                        foreach (var sub in source.Subtitles)
                        {
                            var track = new VideoTrack(sub.Url) 
                            {  
                              Kind = "subtitles",
                              Label = sub.Name,
                              Language = sub.Name.ToLower(),
                              Source = sub.Url,
                              Type = Path.GetExtension(sub.Url)
                            };
                            videoTracks.Add(track);
                        }
                    }

                    videoSource.Medias = videoMedias;
                    videoSource.Tracks = videoTracks;

                    //videoPlayer.Source = videoSource;
                }
                else
                {
                    ShowInfo(false);
                }
            }
            else
            {
                ShowInfo(false);
            }

            isBusy = false;
        }

        private async Task GetVideoLinks_Old(string url)
        {
            isBusy = true;
            isVideoSource = false;

            if (plugin != null)
            {
                videoLink = await mediaService.GetVideoLinksAsync(plugin.Name, url);
                Debug.WriteLine(videoLink?.ToJson());

                if (videoLink != null && videoLink.VideoSources.Count > 0)
                {
                    isVideoSource = true;
                    videoUrl = videoLink.VideoSources[0].Url;

                    videoSource = new VideoSource()
                    {
                        Medias = new ValueEqualityList<VideoMedia>
                        {
                            new VideoMedia(videoUrl),
                            //new VideoMedia("https://cdn.plyr.io/static/demo/View_From_A_Blue_Moon_Trailer-720p.mp4", "video/mp4", 720),
                            //new VideoMedia("https://cdn.plyr.io/static/demo/View_From_A_Blue_Moon_Trailer-1080p.mp4", "video/mp4", 1080),
                        },
                        Tracks = new ValueEqualityList<VideoTrack> { new VideoTrack(videoUrl) }
                    };
                }
                else
                {
                    ShowInfo(false);
                }
            }
            else
            {
                ShowInfo(false);
            }

            isBusy = false;
        }

        private async void ShowInfo(bool isOk)
        {
            if (isOk)
            {
                await _notificationService.Success(L["SuccessMessage"]);
            }
            else
            {
                await _notificationService.Error(L["ErrorMessage"]);
            }
        }

        public async ValueTask DisposeAsync()
        {
            videoPlayer?.DisposeAsync();
            //await ValueTask.CompletedTask;
        }
    }
}
