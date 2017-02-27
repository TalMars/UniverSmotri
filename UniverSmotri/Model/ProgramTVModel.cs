using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UniverSmotri.Model
{
    public class ProgramTVModel : INotifyPropertyChanged
    {
        private string _time;

        public string Time
        {
            get { return _time; }
            set { _time = value; OnPropertyChanged(); }
        }
        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(); }
        }

        public ProgramTVModel()
        {

        }

        public ProgramTVModel(string time, string description)
        {
            _time = time;
            _description = description;
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
