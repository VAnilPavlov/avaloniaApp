using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using avaloniaApp.Model;
using avaloniaApp.ViewModels;
using System;
using System.Collections.Generic;

namespace avaloniaApp.Views
{
    public partial class ChangeData : UserControl
    {
        public ChangeData(GridData gridData)
        {
            InitializeComponent();
            this.DataContext = new ChangeDataViewModel(gridData);
            ComboBoxStatus.ItemsSource = new List<StatusId> { StatusId.done, StatusId.sent, StatusId.closed};
            ComboBoxStatus.SelectedItem = gridData.Status;
        }

        public ChangeData()
        {
            InitializeComponent();
        }
    }
}
