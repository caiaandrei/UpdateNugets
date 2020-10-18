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
            var eventAggregator = new EventAggregator();
            var changePathCommand = new ChangePathCommand();
            var generateRaportCommand = new GenerateRaportCommand();

            var mainViewModel = new MainViewModel(eventAggregator, changePathCommand, generateRaportCommand);

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
