using avaloniaApp.Model;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace avaloniaApp.ViewModels
{
    public partial class MainGridDataViewModel : ViewModelBase, INotifyPropertyChanged
    {

        private ObservableCollection<GridData> _gridDatas = new ObservableCollection<GridData>();
        public ObservableCollection<GridData> GridDatas { 
            set { _gridDatas = value; }
            get { return _gridDatas; } 
        }
        public ICommand GetDataButton { get; }

        public string Take { set; get; } = "25";
        public string Skip { set; get; } = "0";

        public MainGridDataViewModel()
        {
            GetDataButton = new RelayCommand(GetData);
        }

        private async void GetData()
        {
            var resonse = await ApiInteraction.GetData(int.Parse(Take), int.Parse(Skip));
            GridDatas.Clear();
            if (resonse == null)
                return;
            foreach (var i in resonse)
            {
                GridDatas.Add(i);
            }
            OnPropertyChanged(nameof(Text));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
