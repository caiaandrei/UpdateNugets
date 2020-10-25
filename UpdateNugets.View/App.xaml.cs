using Autofac;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Credentials;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using UpdateNugets.UI.Helpers;
using UpdateNugets.UI.Startup;
using UpdateNugets.UI.View;

namespace UpdateNugets.UI
{
    public partial class App : Application
    {

        public void ApplicationStart(object sender, StartupEventArgs args)
        {
            HttpHandlerResourceV3.CredentialService =
                new Lazy<ICredentialService>(() => new CredentialService(
                                                      new AsyncLazy<IEnumerable<ICredentialProvider>>(() => GetProviders()),
                                                      nonInteractive: false,
                                                      handlesDefaultCredentials: false));

            var bootstrapper = new Bootstrapper();
            var container = bootstrapper.Bootstrap();

            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();
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
