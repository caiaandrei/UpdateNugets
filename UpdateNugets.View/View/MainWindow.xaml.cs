using System.IO;
using System.Windows;
using System.Windows.Controls;
using UpdateNugets.Core;
using UpdateNugets.UI.Command;
using UpdateNugets.UI.ViewModel;

namespace UpdateNugets.UI.View
{
    public partial class MainWindow : Window
    {
        private MainViewModel _mainViewModel;

        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();
            DataContext = mainViewModel;
            _mainViewModel = mainViewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _mainViewModel.ChangePathCommand.Execute(_mainViewModel);        }
    }
}
