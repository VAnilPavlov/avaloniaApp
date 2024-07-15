using avaloniaApp.Model;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Net;
using System.Windows.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using avaloniaApp.Views;
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace avaloniaApp.ViewModels;

public partial class MainViewModel : ViewModelBase, INotifyPropertyChanged
{
    public string Login { get; set; }
    public string Password { set; get; }
    private string _textMessage = "";

    public string TextMessage 
    {
        set
        {
            _textMessage = value;
            OnPropertyChanged(nameof(TextMessage));
        }
        get
        {
            return _textMessage;
        }
    }

    public ICommand ClickLogin { get; }

    public MainViewModel()
    {
        ClickLogin = new RelayCommand(LoginLogic);
    }


    private async void LoginLogic()
    {
        var resonse = await ApiInteraction.Login(Login, Password);
        if (!resonse.Item1)
        {
            TextMessage = "Login failed";
            return;
        }

        var data = JObject.Parse(resonse.Item2);
        SaveAsync(data.ToString());
        (App.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime).MainWindow.Content = new MainGridData { DataContext = new MainGridDataViewModel() };
    }

    public async Task SaveAsync(string data)
    {
        if (!Directory.Exists("./Cache"))
        {
            Directory.CreateDirectory("./Cache");
        }

        File.WriteAllText("./Cache/tkSave", data);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
