using UpdateNugets.Core;

namespace UpdateNugets.UI.ViewModel
{
    public class SelectedNuGetDetailsViewModel
    {
        private NuGet _nuGet;

        public SelectedNuGetDetailsViewModel(NuGet nuGet)
        {
            _nuGet = nuGet;
        }
    }
}
