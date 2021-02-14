using Prism.Commands;
using Prism.Events;
using UpdateNugets.UI.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private string _searchBoxText;
        private string _statusText;
        private bool _isStatusVisible;
        private string _nuGetDetailsStatus = "Loading NuGet Details...";
        private string _nuGetDependenciesStatus = "Loading Dependecies...";

        public MainViewModel(IEventAggregator eventAggregator,
                             NuGetsListViewModel nuGetsListViewModel,
                             SelectedNuGetDetailsViewModel selectedNuGetDetailsViewModel,
                             SelectedNuGetVersionFilesViewModel selectedNuGetVersionFilesViewModel,
                             SelectWorkspaceViewModel selectWorkspaceViewModel)
        {
            _eventAggregator = eventAggregator;

            NuGetsListViewModel = nuGetsListViewModel;
            SelectedNuGetDetailsViewModel = selectedNuGetDetailsViewModel;
            SelectedNuGetVersionFilesViewModel = selectedNuGetVersionFilesViewModel;
            SelectWorkspaceViewModel = selectWorkspaceViewModel;

            SearchCommand = new DelegateCommand(async () => await ExecuteSearchAsyncCommand());

            SelectedNuGets = new ObservableCollection<Nuget>();

            _eventAggregator.GetEvent<SelectedNuGetChangedEvent>().Subscribe(OnSelectedNuGetChangedEvent);
            _eventAggregator.GetEvent<SelectedVersionChanged>().Subscribe(OnSelectedVersionChangedEvent);
            _eventAggregator.GetEvent<NuGetUpdated>().Subscribe(OnSelectedVersionChangedEvent);
            _eventAggregator.GetEvent<WorkspacePathSelectedEvent>().Subscribe(OnWorkspacePathChangedEvent);
        }

        public ObservableCollection<Nuget> SelectedNuGets { get; }

        public SelectedNuGetDetailsViewModel SelectedNuGetDetailsViewModel { get; }

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

        private void OnSelectedNuGetChangedEvent(ProjectNuGet nuGet)
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
            SelectedNuGets.Add(new Nuget
            {
                Name = nuGet.Name
            });
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

        private void OnWorkspacePathChangedEvent(string path)
        {
            IsWorkspaceSet = true;
            WorkspacePath = path;
        }
    }
}
