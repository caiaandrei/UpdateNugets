using Autofac;
using NLog;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Credentials;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using UpdateNugets.UI.Helpers;
using UpdateNugets.UI.Startup;
using UpdateNugets.UI.View;

namespace UpdateNugets.UI
{
    public partial class App : Application
    {
        private static Logger _logger;
        private MainWindow _mainWindow;

        public void ApplicationStart(object sender, StartupEventArgs args)
        {
            HttpHandlerResourceV3.CredentialService =
                new Lazy<ICredentialService>(() => new CredentialService(
                                                      new AsyncLazy<IEnumerable<ICredentialProvider>>(() => GetProviders()),
                                                      nonInteractive: false,
                                                      handlesDefaultCredentials: true));

            LogManager.LoadConfiguration("NLog.config");
            _logger = LogManager.GetCurrentClassLogger();
            var bootstrapper = new Bootstrapper();
            var container = bootstrapper.Bootstrap();
            
            _mainWindow = container.Resolve<MainWindow>();
            _mainWindow.Show();
        }

        private static Task<IEnumerable<ICredentialProvider>> GetProviders()
        {
            return Task.FromResult<IEnumerable<ICredentialProvider>>(new ICredentialProvider[]
            {
                    new CredentialDialogProvider(new UIService())
            });
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            string errorMessage = CreateErrorMessage(e.Exception);

            MessageBox.Show(_mainWindow, errorMessage, "Error");
            _logger.Error(e.Exception, "Something bad happened");
            e.Handled = true;
        }

        private static string CreateErrorMessage(Exception ex)
        {
            var messageBuilder = new StringBuilder();

            AddInfoToExceptionMessage(ex, messageBuilder);

            var innerException = ex.InnerException;

            while (innerException is not null)
            {
                AddInfoToExceptionMessage(innerException, messageBuilder);

                innerException = innerException.InnerException;
            }

            return messageBuilder.ToString();
        }

        private static void AddInfoToExceptionMessage(Exception ex, StringBuilder messageBuilder)
        {
            messageBuilder.AppendLine(ex.Message);
            messageBuilder.AppendLine(ex.StackTrace);
            messageBuilder.AppendLine();
        }
    }
}
