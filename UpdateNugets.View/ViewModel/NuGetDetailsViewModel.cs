using Prism.Events;
using System.Threading.Tasks;
using UpdateNugets.UI.Model;

namespace UpdateNugets.UI.ViewModel
{
    public class NuGetDetailsViewModel : ViewModelBase
    {
        private NugetModel _nugetModel;
        private readonly IEventAggregator _eventAggregator;
        private string _name;

        private NuGetVersionsViewModel _nuGetVersionsViewModel;
        private NuGetDependenciesViewModel _nuGetDependenciesViewModel;
        private NuGetVersionFilesViewModel _selectedNuGetVersionFilesViewModel;

        public NuGetDetailsViewModel(NugetModel nugetModel, IEventAggregator eventAggregator)
        {
            Name = nugetModel.Name;
            _nugetModel = nugetModel;
            _eventAggregator = eventAggregator;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public NuGetVersionsViewModel NuGetVersionsViewModel
        {
            get => _nuGetVersionsViewModel;
            set
            {
                _nuGetVersionsViewModel = value;
                OnPropertyChanged(nameof(NuGetVersionsViewModel));
            }
        }

        public NuGetVersionFilesViewModel NuGetVersionFilesViewModel
        {
            get => _selectedNuGetVersionFilesViewModel;
            set
            {
                _selectedNuGetVersionFilesViewModel = value;
                OnPropertyChanged(nameof(NuGetVersionFilesViewModel));
            }
        }

        public NuGetDependenciesViewModel NuGetDependenciesViewModel
        {
            get => _nuGetDependenciesViewModel;
            set
            {
                _nuGetDependenciesViewModel = value;
                OnPropertyChanged(nameof(NuGetDependenciesViewModel));
            }
        }

        public async Task LoadNuGetDetailsAsync()
        {
            NuGetVersionsViewModel = new NuGetVersionsViewModel(_nugetModel, _eventAggregator);
            NuGetDependenciesViewModel = new NuGetDependenciesViewModel(_nugetModel, _eventAggregator);
            NuGetVersionFilesViewModel = new NuGetVersionFilesViewModel(_nugetModel, _eventAggregator);
            await NuGetVersionsViewModel.LoadVersionsAsync();
        }
    }
}
