using System;
using System.Threading.Tasks;
using UpdateNugets.Core;

namespace UpdateNugets.UI.ViewModel
{
    public class NuGetDetailsViewModel : ViewModelBase
    {
        public NuGetDetailsViewModel(Core.ProjectNuGet nuGet)
        {
            //NuGetVersionsViewModel = nuGetVersionsViewModel;
            Name = nuGet.Name;
            _nuGet = nuGet;
        }

        private string _name;
        private NuGetVersionsViewModel _nuGetVersionsViewModel;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private ProjectNuGet _nuGet;

        public NuGetVersionsViewModel NuGetVersionsViewModel 
        {
            get => _nuGetVersionsViewModel;
            set
            {
                _nuGetVersionsViewModel = value;
                OnPropertyChanged(nameof(NuGetVersionsViewModel));
            }
        }

        public async Task LoadNuGetDetailsAsync()
        {
            await NuGetVersionsViewModel.LoadVersionsAsync(_nuGet);
        }


        //public NuGetVersionsViewModel NuGetVersionsViewModel { get; }
    }
}
