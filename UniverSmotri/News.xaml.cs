using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using UniverSmotri.Model;
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
using Windows.UI.ViewManagement;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UniverSmotri
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class News : Page
    {
        private NewsViewModel newsVM;
        public News()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }


        /// <summary>
        /// Вызывается перед отображением этой страницы во фрейме.
        /// </summary>
        /// <param name="e">Данные события, описывающие, каким образом была достигнута эта страница.
        /// Этот параметр обычно используется для настройки страницы.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;

            if (e.NavigationMode == NavigationMode.Back)
                e = null;


            bool internetAccess = NetworkInterface.GetIsNetworkAvailable();
            if (internetAccess)
            {
                if (e != null && e.Parameter != null)
                {
                    string[] param = (string[])e.Parameter;

                    newsVM = new NewsViewModel(param);
                    DataContext = newsVM;
                    if (Settings.DeviceTypeDetectHelper.GetDeviceFormFactorType() == Settings.DeviceFormFactorType.Desktop)
                    {
                        ListView_News.Visibility = Visibility.Collapsed;
                        GridView_News.ItemsSource = newsVM;
                        e = null;
                    }
                    else
                    {
                        GridView_News.Visibility = Visibility.Collapsed;
                        ListView_News.ItemsSource = newsVM;
                        e = null;
                    }
                }
                else
                {
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



        private void ListView_News_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListView_News.SelectedItem == null)
                return;
            NewsModel nm = (NewsModel)ListView_News.SelectedItem;
            string[] arr = new string[3];
            arr[0] = nm.NewsHeader;
            arr[1] = nm.DateNews;
            arr[2] = nm.Href;

            ListView_News.SelectedItem = null;
            Frame.Navigate(typeof(NewsElement), arr);

        }

        private void GridView_News_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GridView_News.SelectedItem == null)
                return;

            NewsModel nm = (NewsModel)GridView_News.SelectedItem;
            string[] arr = new string[3];
            arr[0] = nm.NewsHeader;
            arr[1] = nm.DateNews;
            arr[2] = nm.Href;

            GridView_News.SelectedItem = null;
            Frame.Navigate(typeof(NewsElement), arr);
        }
    }
}
