using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using avaloniaApp.ViewModels;
using System;

namespace avaloniaApp.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private void TextBox_TextChanging(object? sender, Avalonia.Controls.TextChangingEventArgs e)
    {
        if ((DataContext is MainViewModel))
            (DataContext as MainViewModel).TextMessage = "";
    }
}
