using System.Net;

namespace UpdateNugets.UI.Helpers
{
    public interface IUIServices
    {
        bool OpenCredentialsDialog(string target, out NetworkCredential credential);
    }
}