using Prism.Events;
using System.Windows;
using UpdateNugets.UI.View;
using UpdateNugets.UI.ViewModel;

namespace UpdateNugets.UI
{
    public partial class App : Application
    {
        public void ApplicationStart(object sender, StartupEventArgs args)
        {
            IEventAggregator eventAggregator = new EventAggregator();

            var mainViewModel = new MainViewModel(eventAggregator);

            var mainWindow = new MainWindow(mainViewModel);

            mainWindow.ShowDialog();
        }
    }
}
