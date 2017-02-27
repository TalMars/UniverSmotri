using MyToolkit.Multimedia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using UniverSmotri.Model;
using UniverSmotri.Parser;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UniverSmotri
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewsElement : Page
    {
        public NewsElement()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += NewsElement_BackRequested;
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait | DisplayOrientations.Landscape | DisplayOrientations.LandscapeFlipped;
            //ConnectionProfile InternetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();
            //NetworkConnectivityLevel connection = InternetConnectionProfile.GetNetworkConnectivityLevel();
            //bool internetAccess = !(connection == NetworkConnectivityLevel.None || connection == NetworkConnectivityLevel.LocalAccess);
            bool internetAccess = NetworkInterface.GetIsNetworkAvailable();
            if (internetAccess && e.Parameter != null)
            {
                DisplayInformation.GetForCurrentView().OrientationChanged += NewsElement_OrientationChanged;
                string[] param = (string[])e.Parameter;
                HeaderNews.Text = param[0];
                DateNews.Text = param[1];
                OneNewsModel oneNews = await NewsElementParser.Parse(param[2]);
                DescrNews.Text = oneNews.Description;
                YouTubeQuality quality = YouTubeQuality.Quality360P;
                if (Settings.Settings.QualityYouTubeVideo != null)
                {
                    switch (Settings.Settings.QualityYouTubeVideo)
                    {
                        case "360p":
                            quality = YouTubeQuality.Quality360P;
                            break;
                        case "480p":
                            quality = YouTubeQuality.Quality480P;
                            break;
                        case "720p":
                            quality = YouTubeQuality.Quality720P;
                            break;
                        case "1080p":
                            quality = YouTubeQuality.Quality1080P;
                            break;
                    }
                }
                YouTubeUri videoUri = null;
                bool error = false;
                try
                {
                    videoUri = await YouTube.GetVideoUriAsync(oneNews.YouTubeID, quality);
                }
                catch (YouTubeUriNotFoundException)
                {
                    error = true;
                }
                if (!error && videoUri != null)
                {
                    playerYouTube.MediaPlayer.SetUriSource(videoUri.Uri);
                }
                else
                {
                    var dialog = new MessageDialog("Не удалось загрузить видео.");
                    await dialog.ShowAsync();
                    Frame.GoBack();
                }
            }
            else
            {
                var dialog = new MessageDialog("Проверьте соединение с интернетом");
                await dialog.ShowAsync();
                Frame.GoBack();
            }
        }

        private void NewsElement_OrientationChanged(DisplayInformation sender, object args)
        {
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait | DisplayOrientations.Landscape | DisplayOrientations.LandscapeFlipped;
            if (DisplayInformation.GetForCurrentView().CurrentOrientation == DisplayOrientations.Landscape || DisplayInformation.GetForCurrentView().CurrentOrientation == DisplayOrientations.LandscapeFlipped)
            {
                try
                {
                    playerYouTube.IsFullWindow = true;
                } catch (Exception)
                {
                    DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;
                }
            }
            if (DisplayInformation.GetForCurrentView().CurrentOrientation == DisplayOrientations.Portrait)
            {
                playerYouTube.IsFullWindow = false;
            }
        }

        private void NewsElement_BackRequested(object sender, BackRequestedEventArgs e)
        {
            playerYouTube.MediaPlayer.Dispose();
        }

        private void playerYouTube_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            playerYouTube.IsFullWindow = !playerYouTube.IsFullWindow;
        }

        private void playerYouTube_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (playerYouTube.MediaPlayer.PlaybackSession.PlaybackState == Windows.Media.Playback.MediaPlaybackState.None | playerYouTube.MediaPlayer.PlaybackSession.PlaybackState == Windows.Media.Playback.MediaPlaybackState.Paused)
                playerYouTube.MediaPlayer.Play();
            else
                if (playerYouTube.MediaPlayer.PlaybackSession.PlaybackState != Windows.Media.Playback.MediaPlaybackState.Buffering)
                    playerYouTube.MediaPlayer.Pause();
        }
    }
}
