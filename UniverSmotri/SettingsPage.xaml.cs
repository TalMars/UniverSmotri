using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            int index = 0;
            if (Settings.Settings.QualityYouTubeVideo != null)
            {
                switch (Settings.Settings.QualityYouTubeVideo)
                {
                    case "360p":
                        index = 0; break;
                    case "480p":
                        index = 1; break;
                    case "720p":
                        index = 2; break;
                    case "1080p":
                        index = 3; break;
                }
            }
            CB_QuailtyYT.SelectedIndex = index;
            if (Settings.Settings.QualityStreamVideo != null)
            {
                switch (Settings.Settings.QualityStreamVideo)
                {
                    case "Низкое": index = 0; break;
                    case "Среднее": index = 1; break;
                    case "Высокое": index = 2; break;
                }
            }
            CB_QualityStream.SelectedIndex = index;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void CB_QualityStream_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedItem == null)
                return;
            
            string quality = ((sender as ComboBox).SelectedItem as ComboBoxItem).Content.ToString();
            Settings.Settings.QualityStreamVideo = quality;
        }

        private void CB_QuailtyYT_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedItem == null)
                return;

            //string quality = ((sender as ComboBox).SelectedItem as TextBlock).Text;
            string quality = ((sender as ComboBox).SelectedItem as ComboBoxItem).Content.ToString();
            Settings.Settings.QualityYouTubeVideo = quality;
        }
    }
}
