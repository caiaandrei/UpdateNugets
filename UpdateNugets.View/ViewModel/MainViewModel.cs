using Prism.Commands;
using Prism.Events;
using System.Threading.Tasks;
using System.Windows.Input;
using UpdateNugets.Core;
using UpdateNugets.UI.Events;

namespace UpdateNugets.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private string _projectPath;
        private bool _hasSelectedNuGet;
        private readonly IEventAggregator _eventAggregator;
        private bool _searchOnline;
        private bool _includePrerelease;
        private string _searchBoxText;
        private string _statusText;
        private bool _isStatusVisible;
        private string _nuGetDetailsStatus = "Loading NuGet Details...";
        private string _nuGetDependenciesStatus = "Loading Dependecies...";

        public MainViewModel(IEventAggregator eventAggregator, NuGetsListViewModel nuGetsListViewModel, SelectedNuGetDetailsViewModel selectedNuGetDetailsViewModel, SelectedNuGetVersionFilesViewModel selectedNuGetVersionFilesViewModel)
        {
            _eventAggregator = eventAggregator;

            NuGetsListViewModel = nuGetsListViewModel;
            SelectedNuGetDetailsViewModel = selectedNuGetDetailsViewModel;
            SelectedNuGetVersionFilesViewModel = selectedNuGetVersionFilesViewModel;

            SearchCommand = new DelegateCommand(async () => await ExecuteSearchAsyncCommand());

            _eventAggregator.GetEvent<SelectedNuGetChangedEvent>().Subscribe(OnSelectedNuGetChangedEvent);
            _eventAggregator.GetEvent<SelectedVersionChanged>().Subscribe(OnSelectedVersionChangedEvent);
            _eventAggregator.GetEvent<NuGetUpdated>().Subscribe(OnSelectedVersionChangedEvent);
        }

        public SelectedNuGetDetailsViewModel SelectedNuGetDetailsViewModel { get; }

        public SelectedNuGetVersionFilesViewModel SelectedNuGetVersionFilesViewModel { get; }

        public NuGetsListViewModel NuGetsListViewModel { get; }

        public ICommand RefreshProjectCommand { get; }

        public ICommand NewProjectCommand { get; }

        public ICommand OpenProjectCommand { get; }

        public ICommand FinishProjectCommand { get; }

        public ICommand SettingsCommand { get; }

        public ICommand SearchCommand { get; }

        public string SearchBoxText
        {
            get { return _searchBoxText; }
            set
            {
                _searchBoxText = value;
                SearchCommand?.Execute(this);
                OnPropertyChanged(nameof(SearchBoxText));
            }
        }

        public string WorkspacePath
        {
            get { return _projectPath; }
            set
            {
                _projectPath = value;
                OnPropertyChanged(nameof(WorkspacePath));
                ManageNuGets = new ManageNugets(_projectPath);
                NuGetsListViewModel.Load(ManageNuGets);
            }
        }

        public ManageNugets ManageNuGets
        {
            get;
            set;
        }

        public bool HasSelectedNuGet
        {
            get { return _hasSelectedNuGet; }
            set
            {
                _hasSelectedNuGet = value;
                OnPropertyChanged(nameof(HasSelectedNuGet));
            }
        }

        public bool SearchOnline
        {
            get { return _searchOnline; }
            set
            {
                _searchOnline = value;
                OnPropertyChanged(nameof(SearchOnline));
            }
        }

        public bool IncludePrerelease
        {
            get { return _includePrerelease; }
            set
            {
                _includePrerelease = value;
                OnPropertyChanged(nameof(IncludePrerelease));
            }
        }

        public string StatusText
        {
            get { return _statusText; }
            set
            {
                _statusText = value;

                IsStatusVisible = !string.IsNullOrEmpty(_statusText);
                OnPropertyChanged(nameof(StatusText));
            }
        }

        public bool IsStatusVisible
        {
            get => _isStatusVisible;
            private set
            {
                _isStatusVisible = value;
                OnPropertyChanged(nameof(IsStatusVisible));
            }
        }

        private async void OnSelectedNuGetChangedEvent(ProjectNuGet nuGet)
        {
            SelectedNuGetDetailsViewModel.AreVersionsLoading = true;
            SelectedNuGetVersionFilesViewModel.AreVersionsLoading = true;
            StatusText = _nuGetDetailsStatus;
            await SelectedNuGetDetailsViewModel.LoadAsync(nuGet);
            SelectedNuGetDetailsViewModel.AreVersionsLoading = false;
            SelectedNuGetVersionFilesViewModel.AreVersionsLoading = false;

            if (StatusText == _nuGetDetailsStatus)
            {
                StatusText = string.Empty;
            }

            HasSelectedNuGet = true;
        }

        private async void OnSelectedVersionChangedEvent(Version selectedVersion)
        {
            if (selectedVersion is null)
            {
                return;
            }

            SelectedNuGetVersionFilesViewModel.Load(selectedVersion);
            SelectedNuGetDetailsViewModel.AreDependenciesLoading = true;
            StatusText = _nuGetDependenciesStatus;
            await SelectedNuGetDetailsViewModel.LoadDependenciesAsync();
            SelectedNuGetDetailsViewModel.AreDependenciesLoading = false;
            StatusText = string.Empty;
        }

        private async Task ExecuteSearchAsyncCommand()
        {
            var allNuGets = await ManageNuGets.SearchAsync(SearchBoxText.Trim(), false, false);
            NuGetsListViewModel.NuGets = allNuGets;
        }

    }
}
