using Prism.Events;
using System.Threading.Tasks;
using UpdateNugets.Core;
using UpdateNugets.UI.Model;

namespace UpdateNugets.UI.ViewModel
{
    public class NuGetDetailsViewModel : ViewModelBase
    {
        private NugetModel _nugetModel;
        private readonly IEventAggregator _eventAggregator;
        private string _name;

        private NuGetVersionsViewModel _nuGetVersionsViewModel;

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

        public async Task LoadNuGetDetailsAsync()
        {
            NuGetVersionsViewModel = new NuGetVersionsViewModel(_nugetModel, _eventAggregator);
            await NuGetVersionsViewModel.LoadVersionsAsync();
        }
    }
}
