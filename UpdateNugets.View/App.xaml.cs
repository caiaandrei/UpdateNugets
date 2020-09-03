using Prism.Events;
using System.Windows;
using System.Windows.Input;
using UpdateNugets.UI.Command;
using UpdateNugets.UI.View;
using UpdateNugets.UI.ViewModel;

namespace UpdateNugets.UI
{
    public partial class App : Application
    {
        public void ApplicationStart(object sender, StartupEventArgs args)
        {
            IEventAggregator eventAggregator = new EventAggregator();
            ICommand changePathCommand = new ChangePathCommand();

            var mainViewModel = new MainViewModel(eventAggregator, changePathCommand);

            var mainWindow = new MainWindow(mainViewModel);

            mainWindow.ShowDialog();
        }
    }
}
