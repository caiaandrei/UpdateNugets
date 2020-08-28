using System.Windows;
using UpdateNugets.UI.View;
using UpdateNugets.UI.ViewModel;

namespace UpdateNugets.UI
{
    public partial class App : Application
    {
        public void ApplicationStart(object sender, StartupEventArgs args)
        {
            var mainViewModel = new MainViewModel();

            var mainWindow = new MainWindow(mainViewModel);

            mainWindow.ShowDialog();
        }
    }
}
