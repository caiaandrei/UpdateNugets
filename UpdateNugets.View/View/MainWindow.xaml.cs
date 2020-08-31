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
        private NuGetsListViewModel _nuGetsListViewModel;

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
            selectProjectPathView.Owner = this;

            selectProjectPathView.ShowDialog();

            if (!string.IsNullOrEmpty(selectProjectPathViewModel.ProjectPath))
            {
                _mainViewModel.ProjectPath = selectProjectPathViewModel.ProjectPath;
                _mainViewModel.ManageNuGets = new ManageNugets(selectProjectPathViewModel.ProjectPath);
                _mainViewModel.NuGetsListViewModel = new NuGetsListViewModel(_mainViewModel.ManageNuGets);
                

            }



        }
    }
}
