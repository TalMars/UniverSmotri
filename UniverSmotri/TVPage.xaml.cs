using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using UniverSmotri.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Media.Streaming.Adaptive;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UniverSmotri
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TVPage : Page
    {
        private TV_ViewModel vm;
        private string kfuTvUrl = "http://cdn.universmotri.ru/live/univer_kfu2/playlist.m3u8";
        private string univerSmotriUrl_Low = "http://cdn.universmotri.ru/live/lq.sdp/playlist.m3u8";
        private string univerSmotriUrl_Medium = "http://cdn.universmotri.ru/live/mq.sdp/playlist.m3u8";
        private string univerSmotriUrl_High = "http://cdn.universmotri.ru/live/hq.sdp/playlist.m3u8";
        private string epgKFU = "http://tv.kpfu.ru/epg_kpfu";
        private string epgUniverSmotri = "http://tv.kpfu.ru/epg_universmotri";
        private string tvUrl;
        private string epg;

        public TVPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += TVPage_BackRequested;
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait | DisplayOrientations.Landscape | DisplayOrientations.LandscapeFlipped;
            string header = "";
            bool internetAccess = NetworkInterface.GetIsNetworkAvailable();
            if (e.Parameter != null && internetAccess)
            {
                if ((bool)e.Parameter)
                {
                    epg = epgUniverSmotri;
                    string qualityStream = "";
                    header = "UNIVERSMOTRI";
                    if (Settings.Settings.QualityStreamVideo != null)
                    {
                        switch (Settings.Settings.QualityStreamVideo)
                        {
                            case "Низкое": qualityStream = univerSmotriUrl_Low; break;
                            case "Среднее": qualityStream = univerSmotriUrl_Medium; break;
                            case "Высокое": qualityStream = univerSmotriUrl_High; break;
                        }
                    }
                    else
                    {
                        qualityStream = univerSmotriUrl_Low;
                    }
                    tvUrl = qualityStream;
                }
                else
                {
                    header = "Телеканал КФУ";
                    epg = epgKFU;
                    tvUrl = kfuTvUrl;
                }
                DisplayInformation.GetForCurrentView().OrientationChanged += TVPage_OrientationChanged;
                vm = new TV_ViewModel(epg);
                vm.HeaderTV = header;
                var streamResponse = await AdaptiveMediaSource.CreateFromUriAsync(new Uri(tvUrl));
                if (streamResponse.Status == AdaptiveMediaSourceCreationStatus.Success)
                    mPlayer.MediaPlayer.SetMediaSource(streamResponse.MediaSource); //mPlayer.Source = new Uri(tvUrl);
                else
                {
                    var dialog = new MessageDialog("Не удалось загрузить видео.\nПроверьте соединение с интернетом.");
                    await dialog.ShowAsync();
                    Frame.GoBack();
                }
                DataContext = vm;
            }
            else
            {
                var dialog = new MessageDialog("Не удалось загрузить видео.\nПроверьте соединение с интернетом.");
                await dialog.ShowAsync();
                Frame.GoBack();
            }
        }

        private void TVPage_OrientationChanged(DisplayInformation sender, object args)
        {
            if (DisplayInformation.GetForCurrentView().CurrentOrientation == DisplayOrientations.Landscape || DisplayInformation.GetForCurrentView().CurrentOrientation == DisplayOrientations.LandscapeFlipped)
            {
                mPlayer.IsFullWindow = true;
            }
            if (DisplayInformation.GetForCurrentView().CurrentOrientation == DisplayOrientations.Portrait)
            {
                mPlayer.IsFullWindow = false;
            }
        }

        private void TVPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            mPlayer.MediaPlayer.Dispose();
        }

        private void mPlayer_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            mPlayer.IsFullWindow = !mPlayer.IsFullWindow;
        }

        private void mPlayer_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (mPlayer.MediaPlayer.PlaybackSession.PlaybackState == Windows.Media.Playback.MediaPlaybackState.None | mPlayer.MediaPlayer.PlaybackSession.PlaybackState == Windows.Media.Playback.MediaPlaybackState.Paused)
                mPlayer.MediaPlayer.Play();
            else
                if (mPlayer.MediaPlayer.PlaybackSession.PlaybackState != Windows.Media.Playback.MediaPlaybackState.Buffering)
                mPlayer.MediaPlayer.Pause();
        }
    }
}
