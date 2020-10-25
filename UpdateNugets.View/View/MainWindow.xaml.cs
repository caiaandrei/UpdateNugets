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

        private void CtrlCCopyCmdCanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void RightClickCopyCmdCanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CtrlCCopyCmdExecuted(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            Clipboard.SetText(_mainViewModel.ProjectPath);
        }

        private void RightClickCopyCmdExecuted(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            Clipboard.SetText(_mainViewModel.ProjectPath);
        }
    }
}
