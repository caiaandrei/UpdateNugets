using System.Windows;
using UpdateNugets.UI.Command;
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
            var browseCommand = new BrowsePathCommand();
            var selectProjectPathViewModel = new SelectProjectPathViewModel(browseCommand);
            var selectProjectPathView = new SelectProjectPathView(selectProjectPathViewModel);
            selectProjectPathView.Owner = App.Current.MainWindow;

            selectProjectPathView.ShowDialog();

            if (!string.IsNullOrEmpty(selectProjectPathViewModel.ProjectPath))
            {
                _mainViewModel.WorkspacePath = selectProjectPathViewModel.ProjectPath;
            }
        }
    }
}
