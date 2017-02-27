using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UniverSmotri.Model;
using UniverSmotri.Parser;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace UniverSmotri.ViewModel
{
    public class NewsViewModel : IncrementalLoadingBase, INotifyPropertyChanged
    {

        private bool hasMoreItems;
        private int nextPage = 2;
        private bool isMain = true;
        private string mainUrl;
        private bool isJustNews;
        public NewsViewModel(string[] param)
        {
            hasMoreItems = true;
            isJustNews = param[0].Substring(param[0].Length - 1) == "1";
            this.mainUrl = param[0].Substring(0, param[0].Length - 1);
            this._headerPage = param[1];
        }

        private string _headerPage;

        public string HeaderPage
        {
            get { return _headerPage; }
            set { _headerPage = value; }
        }

        private Visibility _visible;

        public Visibility Visible
        {
            get { return _visible; }
            set { _visible = value; OnPropertyChanged(); }
        }

        protected async override Task<IList<object>> LoadMoreItemsOverrideAsync(System.Threading.CancellationToken c, uint count)
        {
            await CoreWindow.GetForCurrentThread().Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Visible = Visibility.Visible;
            });

            string nextPageUrl = "";
            if (isJustNews)
                nextPageUrl = mainUrl.Substring(0, mainUrl.Length - 5) + "/Page-" + nextPage.ToString() + "-10.html";
            else
                nextPageUrl = mainUrl.Substring(0, mainUrl.Length - 5) + "/Page-" + nextPage.ToString() + "-30.html";

            List<NewsModel> list = new List<NewsModel>();

            if (isMain)
                list = await HtmlParser.Parse(mainUrl);
            else
            {
                list = await HtmlParser.Parse(nextPageUrl);
                if (list.Count != 0)
                    nextPage++;
                else
                    hasMoreItems = false;
            }

            var values = from j in list
                         select (object)j;

            isMain = list.Count == 0;
            await CoreWindow.GetForCurrentThread().Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Visible = Visibility.Collapsed;
            });
            return values.ToArray();
        }

        protected override bool HasMoreItemsOverride()
        {
            return hasMoreItems;
        }

        public void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
