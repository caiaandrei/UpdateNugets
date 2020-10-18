using Ookii.Dialogs.Wpf;
using System;
using System.Net;

namespace UpdateNugets.UI.Helpers
{
    public class UIService : IUIServices
    {
        public bool OpenCredentialsDialog(string target, out NetworkCredential networkCredential)
        {
            using var dialog = new CredentialDialog
            {
                WindowTitle = "Enter Token",
                MainInstruction = "Credentials for " + target,
                Content = "Enter Personal Access Tokens in the username field.",
                Target = target,
                ShowSaveCheckBox = true, // Allow user to save the credentials to operating system's credential manager
                ShowUIForSavedCredentials = false // Do not show dialog when credentials can be grabbed from OS credential manager
            };

            try
            {
                // Show dialog or query credential manager
                if (dialog.ShowDialog())
                {
                    // When dialog was shown
                    if (!dialog.IsStoredCredential)
                    {
                        // Save the credentials when save checkbox was checked
                        dialog.ConfirmCredentials(true);
                    }
                    networkCredential = dialog.Credentials;
                    return true;
                }
            }
            catch (Exception e)
            {

            }

            networkCredential = null;
            return false;
        }
    }
}
