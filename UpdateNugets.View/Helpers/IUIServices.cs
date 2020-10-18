using System.Net;

namespace UpdateNugets.UI.Helpers
{
    public interface IUIServices
    {
        bool OpenCredentialsDialog(string v, out NetworkCredential credential);
    }
}