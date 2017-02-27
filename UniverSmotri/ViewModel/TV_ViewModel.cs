using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UniverSmotri.Model;
using UniverSmotri.Parser;

namespace UniverSmotri.ViewModel
{
    public class TV_ViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ProgramTVModel> Items
        {
            get;
            set;
        }
        public TV_ViewModel(string htmlUrl)
        {
            Items = new ObservableCollection<ProgramTVModel>();
            Load(htmlUrl);
        }

        private string _headerTV;

        public string HeaderTV
        {
            get { return _headerTV; }
            set { _headerTV = value; }
        }

        private async void Load(string hrmlUrl)
        {
            await Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                List<ProgramTVModel> list = await ProgramTVParser.Parse(hrmlUrl);

                foreach (var i in list)
                {
                    Items.Add(i);
                }
            });
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
