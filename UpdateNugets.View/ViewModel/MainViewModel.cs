using Prism.Commands;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using UpdateNugets.Core;
using UpdateNugets.UI.Events;
using UpdateNugets.UI.Model;

namespace UpdateNugets.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private string _projectPath;
        private bool _hasSelectedNuGet;
        private readonly IEventAggregator _eventAggregator;
        private string _searchBoxText;
        private string _statusText;
        private bool _isStatusVisible;
        private string _nuGetDetailsStatus = "Loading NuGet Details...";
        private string _nuGetDependenciesStatus = "Loading Dependecies...";

        public MainViewModel(IEventAggregator eventAggregator,
                             NuGetsListViewModel nuGetsListViewModel,
                             SelectWorkspaceViewModel selectWorkspaceViewModel)
        {
            _eventAggregator = eventAggregator;

            NuGetsListViewModel = nuGetsListViewModel;
            SelectWorkspaceViewModel = selectWorkspaceViewModel;

            SearchCommand = new DelegateCommand(async () => await ExecuteSearchAsyncCommand());

            SelectedNuGets = new ObservableCollection<NuGetDetailsViewModel>();

            _eventAggregator.GetEvent<SelectedNuGetChangedEvent>().Subscribe(OnSelectedNuGetChangedEvent);
            _eventAggregator.GetEvent<SelectedVersionChanged>().Subscribe(OnSelectedVersionChangedEvent);
            _eventAggregator.GetEvent<NuGetUpdated>().Subscribe(OnSelectedVersionChangedEvent);
            _eventAggregator.GetEvent<WorkspacePathSelectedEvent>().Subscribe(OnWorkspacePathChangedEvent);
        }

        public ObservableCollection<NuGetDetailsViewModel> SelectedNuGets { get; }

        public NuGetDetailsViewModel NuGetDetailsViewModel { get; }

        public SelectedNuGetVersionFilesViewModel SelectedNuGetVersionFilesViewModel { get; }

        public NuGetsListViewModel NuGetsListViewModel { get; }

        public SelectWorkspaceViewModel SelectWorkspaceViewModel { get; }

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
                OnPropertyChanged(nameof(IsWorkspaceSet));
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

        public bool IsWorkspaceSet { get; private set; }

        private async void OnSelectedNuGetChangedEvent(NuGetDetailsViewModel nuGetDetailsViewModel)
        {
            //SelectedNuGetDetailsViewModel.AreVersionsLoading = true;
            //SelectedNuGetVersionFilesViewModel.AreVersionsLoading = true;
            //StatusText = _nuGetDetailsStatus;
            //await SelectedNuGetDetailsViewModel.LoadAsync(nuGet);
            //SelectedNuGetDetailsViewModel.AreVersionsLoading = false;
            //SelectedNuGetVersionFilesViewModel.AreVersionsLoading = false;

            //if (StatusText == _nuGetDetailsStatus)
            //{
            //    StatusText = string.Empty;
            //}

            //HasSelectedNuGet = true;
            SelectedNuGets.Add(nuGetDetailsViewModel);
            await nuGetDetailsViewModel.LoadNuGetDetailsAsync();
        }

        private async void OnSelectedVersionChangedEvent(Version selectedVersion)
        {
            //if (selectedVersion is null)
            //{
            //    return;
            //}

            //SelectedNuGetVersionFilesViewModel.Load(selectedVersion);
            //NuGetDetailsViewModel.AreDependenciesLoading = true;
            //StatusText = _nuGetDependenciesStatus;
            //await NuGetDetailsViewModel.LoadDependenciesAsync();
            //NuGetDetailsViewModel.AreDependenciesLoading = false;
            //StatusText = string.Empty;
        }

        private async Task ExecuteSearchAsyncCommand()
        {
            var allNuGets = await ManageNuGets.SearchAsync(SearchBoxText.Trim(), false, false);
            //NuGetsListViewModel.NuGetsDetail = allNuGets;
        }

        private void OnWorkspacePathChangedEvent(string path)
        {
            IsWorkspaceSet = true;
            WorkspacePath = path;
        }
    }
}
