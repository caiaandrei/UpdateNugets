using System.Windows;
using UpdateNugets.UI.ViewModel;

namespace UpdateNugets.UI.View
{
    public partial class MainWindow
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
            _mainViewModel.ChangePathCommand.Execute(_mainViewModel);
        }
    }
}
