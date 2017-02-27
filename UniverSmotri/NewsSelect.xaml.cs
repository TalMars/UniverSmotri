using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class NewsSelect : Page
    {
        public NewsSelect()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;
        }

        private async void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            TextBlock tb = sender as TextBlock;
            switch (tb.Text)
            {
                case "Все новости":
                    Frame.Navigate(typeof(News), new string[] { "http://tv.kpfu.ru/index.php/novosti.html1", "Все новости" });
                    break;
                case "Обзоры новостей":
                    Frame.Navigate(typeof(News), new string[] { "http://tv.kpfu.ru/index.php/novosti/obzory-novostey.html0", "Обзоры новостей" });
                    break;
                case "Новости ректората":
                    Frame.Navigate(typeof(News), new string[] { "http://tv.kpfu.ru/index.php/novosti/novosti-rektorata.html0", "Новости ректората" });
                    break;
                case "Общество и культура":
                    Frame.Navigate(typeof(News), new string[] { "http://tv.kpfu.ru/index.php/novosti/obschestvo-i-kultura.html0", "Общество и культура" });
                    break;
                case "Образование и наука":
                    Frame.Navigate(typeof(News), new string[] { "http://tv.kpfu.ru/index.php/novosti/obrazovanie-i-nauka.html0", "Образование и наука" });
                    break;
                case "Спорт":
                    Frame.Navigate(typeof(News), new string[] { "http://tv.kpfu.ru/index.php/novosti/sport.html0", "Спорт" });
                    break;
                default:
                    var dialog = new MessageDialog("error");
                    await dialog.ShowAsync();
                    break;
            }
        }
    }
}
