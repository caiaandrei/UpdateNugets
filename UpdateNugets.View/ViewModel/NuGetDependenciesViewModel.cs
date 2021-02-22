using Prism.Events;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using UpdateNugets.UI.Events;
using UpdateNugets.UI.Model;

namespace UpdateNugets.UI.ViewModel
{
    public class NuGetDependenciesViewModel : ViewModelBase
    {
        private const string _statusMessage = "Dependencies loading...";
        private IEventAggregator _eventAggregator;
        private NugetModel _nugetModel;
        private ObservableCollection<string> _dependencies;
        private bool _areDependeciesLoading;
        private bool _areDependeciesVisible;

        public NuGetDependenciesViewModel(NugetModel nugetModel, IEventAggregator eventAggregator)
        {
            _nugetModel = nugetModel;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<SelectedVersionChanged>().Subscribe(OnSelectedVersionChanged);
        }

        public ObservableCollection<string> Dependencies
        {
            get { return _dependencies; }
            set
            {
                _dependencies = value;
                OnPropertyChanged(nameof(Dependencies));
            }
        }

        public bool AreDependenciesLoading
        {
            get { return _areDependeciesLoading; }
            set
            {
                _areDependeciesLoading = value;
                AreDependenciesVisible = !_areDependeciesLoading;
                OnPropertyChanged(nameof(AreDependenciesLoading));
            }
        }

        public bool AreDependenciesVisible
        {
            get { return _areDependeciesVisible; }
            set
            {
                _areDependeciesVisible = value;
                OnPropertyChanged(nameof(AreDependenciesVisible));
            }
        }


        public async Task LoadDependenciesAsync()
        {
            _eventAggregator.GetEvent<PublishMessageEvent>().Publish(new PublishMessageEventArg
            {
                Message = _statusMessage,
                IsVisible = true
            });

            await _nugetModel.LoadNuGetDependencies();

            _eventAggregator.GetEvent<PublishMessageEvent>().Publish(new PublishMessageEventArg
            {
                Message = _statusMessage,
                IsVisible = false
            });
        }

        private async void OnSelectedVersionChanged(NugetModel nugetModel)
        {
            if (_nugetModel.Name != nugetModel.Name)
            {
                return;
            }

            AreDependenciesLoading = true;
            await LoadDependenciesAsync();
            Dependencies = new ObservableCollection<string>(_nugetModel.Dependencies);
            AreDependenciesLoading = false;
        }
    }
}
