using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using avaloniaApp.Model;
using System.Linq;

namespace avaloniaApp.Views
{
    public partial class MainGridData : UserControl
    {
        public MainGridData()
        {
            InitializeComponent();
        }

        private void NumericInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string text = textBox.Text;

                if (!int.TryParse(text, out _))
                {
                    textBox.Text = new string(text.Where(char.IsDigit).ToArray());
                    textBox.CaretIndex = textBox.Text.Length;
                }
            }
        }

        private void OpenPopup(object sender, Avalonia.Interactivity.RoutedEventArgs e) 
        {
            if ((GridData)DataGridv.SelectedItem is null)
                return;
            var popupControl = new ChangeData((GridData)DataGridv.SelectedItem);
            ChangeControlPopup.Child = popupControl;
            ChangeControlPopup.PlacementTarget = ButtonForPoput;
            ChangeControlPopup.IsOpen = !ChangeControlPopup.IsOpen;
        }
    }
}
