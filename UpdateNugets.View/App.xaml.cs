using NuGet.Common;
using NuGet.Configuration;
using NuGet.Credentials;
using NuGet.Protocol;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UpdateNugets.UI.Command;
using UpdateNugets.UI.Helpers;
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

            HttpHandlerResourceV3.CredentialService =
                new Lazy<ICredentialService>(() => new CredentialService(
                                                      new AsyncLazy<IEnumerable<ICredentialProvider>>(() => GetProviders()),
                                                      nonInteractive: false,
                                                      handlesDefaultCredentials: false));


            mainWindow.ShowDialog();
        }

        private static Task<IEnumerable<ICredentialProvider>> GetProviders()
        {
            return Task.FromResult<IEnumerable<ICredentialProvider>>(new ICredentialProvider[]
            {
                    new CredentialDialogProvider(new UIService())
            });
        }
    }
}
