using NuGet.Packaging;
using Prism.Commands;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using UpdateNugets.Core;
using UpdateNugets.UI.Events;

namespace UpdateNugets.UI.ViewModel
{
    public class SelectedNuGetDetailsViewModel : ViewModelBase
    {
        private ProjectNuGet _nuGet;
        private string _name;
        private bool _areVersionsLoading;
        private Version _selectedVersion;
        private IEventAggregator _eventAggregator;
        private ObservableCollection<Version> _versions = new ObservableCollection<Version>();
        private ObservableCollection<string> _dependencies = new ObservableCollection<string>();
        private ManageNugets _manageNugets;
        private bool _areVersionsVisible = true;
        private bool _areDependeciesVisible;
        private bool _areDependeciesLoading;

        public SelectedNuGetDetailsViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            UpdateNuGetCommand = new DelegateCommand(async () => await OnExecuteUpdateCommand(), OnCanExecuteUpdateCommand).ObservesProperty(() => SelectedVersion);
        }

        public ObservableCollection<Version> Versions
        {
            get { return _versions; }
            set
            {
                _versions = value;
                OnPropertyChanged(nameof(Versions));
            }
        }

        public ObservableCollection<string> Dependencies
        {
            get => _dependencies;
            set
            {
                _dependencies = value;
                OnPropertyChanged(nameof(Dependencies));
                OnPropertyChanged(nameof(HasDependencies));
            }
        }

        public bool HasDependencies => Dependencies.Count != 0;

        public Version SelectedVersion
        {
            get { return _selectedVersion; }
            set
            {
                _selectedVersion = value;
                _nuGet.CurrentSelectedVersion = _selectedVersion;
                OnPropertyChanged(nameof(SelectedVersion));
                _eventAggregator.GetEvent<SelectedVersionChanged>().Publish(SelectedVersion);
            }
        }

        public DelegateCommand UpdateNuGetCommand { get; }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public bool AreVersionsLoading
        {
            get { return _areVersionsLoading; }
            set
            {
                _areVersionsLoading = value;
                AreVersionsVisible = !_areVersionsLoading;
                OnPropertyChanged(nameof(AreVersionsLoading));
            }
        }

        public bool AreVersionsVisible
        {
            get { return _areVersionsVisible; }
            set
            {
                _areVersionsVisible = value;
                OnPropertyChanged(nameof(AreVersionsVisible));
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



        public async Task LoadAsync(ProjectNuGet nuGet, ManageNugets manageNugets)
        {
            _manageNugets = manageNugets;
            nuGet = await manageNugets.SearchNuGetVersions(nuGet);
            _nuGet = nuGet;
            Versions = new ObservableCollection<Version>(_nuGet.Versions);
            Name = nuGet.Name;
            SelectedVersion = nuGet.CurrentVersion;
        }

        public async Task LoadDependenciesAsync(ManageNugets manageNugets)
        {
            var dependencies = await manageNugets.GetDependecies(_nuGet, SelectedVersion.NuGetVersion);
            Dependencies = new ObservableCollection<string>(dependencies);
        }

        private bool OnCanExecuteUpdateCommand()
        {
            return _nuGet != null && _nuGet.CurrentVersion != _nuGet.CurrentSelectedVersion;
        }

        private async Task OnExecuteUpdateCommand()
        {
            _manageNugets.UpdateNuGets(_nuGet.Name, _nuGet.CurrentSelectedVersion.NuGetVersion, _nuGet.CurrentVersion.Files);

            _nuGet.CurrentSelectedVersion.Files.AddRange(_nuGet.CurrentVersion.Files);
            _nuGet.CurrentSelectedVersion.IsTheCurrentVersion = true;

            _nuGet.CurrentVersion.IsTheCurrentVersion = false;
            _nuGet.CurrentVersion.Files.Clear();

            _nuGet.CurrentVersion = _nuGet.CurrentSelectedVersion;

            Versions = new ObservableCollection<Version>(_nuGet.Versions);
            UpdateNuGetCommand.RaiseCanExecuteChanged();
            _eventAggregator.GetEvent<SelectedVersionChanged>().Publish(SelectedVersion);

            var dependencies = await _manageNugets.GetDependecies(_nuGet, SelectedVersion.NuGetVersion);
            Dependencies = new ObservableCollection<string>(dependencies);
        }
    }
}
