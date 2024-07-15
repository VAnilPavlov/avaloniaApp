using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using avaloniaApp.Model;
using CommunityToolkit.Mvvm.Input;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace avaloniaApp.ViewModels
{
    public partial class ChangeDataViewModel : ViewModelBase
    {
        public GridData DataForChange { set; get; }
        public Popup PopupForChange { set; get; }
        public ICommand ChangeButton { get; }

        public ChangeDataViewModel()
        {
        }

        public ChangeDataViewModel(GridData gridData)
        {
            DataForChange = gridData;
            ChangeButton = new RelayCommand(ChangeData);
        }

        private void ChangeData()
        {
            var isOk = ApiInteraction.SaveData(DataForChange);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
