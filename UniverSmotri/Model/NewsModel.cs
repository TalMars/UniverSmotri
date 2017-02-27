using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UniverSmotri.Model
{
    public class NewsModel : INotifyPropertyChanged
    {
        private string image;
        public string Image
        {
            get { return image; }
            set { image = value; OnPropertyChanged(); }
        }

        private string newsHeader;
        public string NewsHeader
        {
            get { return newsHeader; }
            set { newsHeader = value; OnPropertyChanged(); }
        }

        private string dateNews;

        public string DateNews
        {
            get { return dateNews; }
            set { dateNews = value; OnPropertyChanged(); }
        }

        private string href;

        public string Href
        {
            get { return href; }
            set { href = value; OnPropertyChanged(); }
        }


        public NewsModel()
        {

        }

        public void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
