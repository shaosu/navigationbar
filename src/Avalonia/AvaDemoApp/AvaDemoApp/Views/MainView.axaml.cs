using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AvaDemoApp.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }
        private void MainWindow_Loaded(object? sender, RoutedEventArgs e)
        {
            bar.SelectedIndex = 0;
            bar2.SelectedIndex = 0;
        }
    }
}