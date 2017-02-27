using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UniverSmotri.Parser;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Playback;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Graphics.Display;
using Windows.UI.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UniverSmotri
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string urlRadio;
        private Uri uriMute = new Uri("ms-appx:///Icons/appbar.sound.mute.png");
        private Uri uriSound = new Uri("ms-appx:///Icons/appbar.sound.3.png");

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;

            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                try
                {
                    if (BackgroundMediaPlayer.Current.PlaybackSession.PlaybackState == MediaPlaybackState.Paused | BackgroundMediaPlayer.Current.PlaybackSession.PlaybackState == MediaPlaybackState.None)
                        (Radio.Icon as BitmapIcon).UriSource = uriMute;
                    if (BackgroundMediaPlayer.Current.PlaybackSession.PlaybackState == MediaPlaybackState.Playing)
                        (Radio.Icon as BitmapIcon).UriSource = uriSound;

                } catch (Exception)
                {
                    (Radio.Icon as BitmapIcon).UriSource = uriMute;
                    BackgroundMediaPlayer.Shutdown();
                }
            });
            BackgroundMediaPlayer.Current.CurrentStateChanged += radioPlayer_CurrentStateChanged;
        }

        async void radioPlayer_CurrentStateChanged(MediaPlayer sender, object args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (BackgroundMediaPlayer.Current.PlaybackSession.PlaybackState == MediaPlaybackState.Playing)
                {
                    (Radio.Icon as BitmapIcon).UriSource = uriSound;
                }
                else if (BackgroundMediaPlayer.Current.PlaybackSession.PlaybackState == MediaPlaybackState.Paused)
                {
                    (Radio.Icon as BitmapIcon).UriSource = uriMute;
                }
            });

        }

        private void News_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewsSelect));
        }

        private void USTV_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TVPage), true);
        }

        private void KFUTV_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TVPage), false);
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }

        private async void RadioAppBtn_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton btn = sender as AppBarButton;
            bool error = false;
            if (btn != null)
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    try
                    {
                        if ((btn.Icon as BitmapIcon).UriSource == uriMute)
                        {
                            urlRadio = await RadioUrlParser.Parse();
                            if (BackgroundMediaPlayer.Current.PlaybackSession.PlaybackState != MediaPlaybackState.Playing)
                            {
                                var message = new ValueSet
                                {
                                  {
                                  "Play",
                                  urlRadio
                                  }
                                };
                                BackgroundMediaPlayer.SendMessageToBackground(message);
                        }
                        }
                        else
                        {
                            BackgroundMediaPlayer.Current.Pause();
                    }
                    }
                    catch (Exception)
                    {
                        (btn.Icon as BitmapIcon).UriSource = uriMute;
                        error = true;
                    }
                    if (error)
                    {
                        await (new MessageDialog("Ошибка в воспоизведении радио.")).ShowAsync();
                    }
                });


            }
        }
    }
}
