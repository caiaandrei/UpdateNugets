using NuGet.Configuration;
using NuGet.Credentials;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace UpdateNugets.UI.Helpers
{
    public class CredentialDialogProvider : ICredentialProvider
    {
        private readonly IUIServices _uiServices;

        public CredentialDialogProvider(IUIServices uiServices)
        {
            _uiServices = uiServices ?? throw new ArgumentNullException(nameof(uiServices));
        }

        public string Id => "UpdateNuGetsCredentialDialog";

        public async Task<CredentialResponse> GetAsync(Uri uri, IWebProxy proxy, CredentialRequestType type, string message, bool isRetry, bool nonInteractive, CancellationToken cancellationToken)
        {
            if (nonInteractive)
            {
                return new CredentialResponse(CredentialStatus.ProviderNotApplicable);
            }

            var success = false;
            NetworkCredential credential = null;

            Application.Current.Dispatcher.Invoke(() =>
            {
                success = _uiServices.OpenCredentialsDialog(uri.GetLeftPart(UriPartial.Authority), out credential);
            });

            cancellationToken.ThrowIfCancellationRequested();

            if (success)
            {
                return new CredentialResponse(credential);
            }

            return new CredentialResponse(CredentialStatus.UserCanceled);
        }
    }
}
