using Prism.Commands;
using Prism.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private string _statusText;
        private bool _isStatusVisible;
        private List<ProjectNuGet> _allNuGets;
        private NuGetDetailsViewModel _selectedNuGetDetailsViewModel;
        private ManageNugets _manageNuGets;

        public MainViewModel(IEventAggregator eventAggregator,
                             NavigationViewModel nuGetsListViewModel,
                             SelectWorkspaceViewModel selectWorkspaceViewModel)
        {
            _eventAggregator = eventAggregator;

            NavigationViewModel = nuGetsListViewModel;
            SelectWorkspaceViewModel = selectWorkspaceViewModel;

            SelectedNuGets = new ObservableCollection<NuGetDetailsViewModel>();

            _eventAggregator.GetEvent<SelectedVersionChanged>().Subscribe(OnSelectedVersionChangedEvent);
            _eventAggregator.GetEvent<NuGetUpdated>().Subscribe(OnSelectedVersionChangedEvent);
            _eventAggregator.GetEvent<WorkspacePathSelectedEvent>().Subscribe(OnWorkspacePathChangedEvent);
            _eventAggregator.GetEvent<OpenDetailViewEvent>().Subscribe(OnOpenDetailViewEvent);
        }

        public ObservableCollection<NuGetDetailsViewModel> SelectedNuGets { get; }

        public NuGetDetailsViewModel SelectedNuGetDetailsViewModel
        {
            get { return _selectedNuGetDetailsViewModel; }
            set
            {
                _selectedNuGetDetailsViewModel = value;
                OnPropertyChanged(nameof(SelectedNuGetDetailsViewModel));
            }
        }

        public SelectedNuGetVersionFilesViewModel SelectedNuGetVersionFilesViewModel { get; }

        public NavigationViewModel NavigationViewModel { get; }

        public SelectWorkspaceViewModel SelectWorkspaceViewModel { get; }

        public string WorkspacePath
        {
            get { return _projectPath; }
            set
            {
                _projectPath = value;
                OnPropertyChanged(nameof(WorkspacePath));
                OnPropertyChanged(nameof(IsWorkspaceSet));
                ManageNuGets = new ManageNugets(_projectPath);
            }
        }

        public ManageNugets ManageNuGets
        {
            get => _manageNuGets;
            set
            {
                _manageNuGets = value;
                _allNuGets = _manageNuGets.NuGets.ToList();
                NavigationViewModel.Load(_allNuGets.Select(item => item.Name).ToList());
            }
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

        private async void OnOpenDetailViewEvent(OpenDetailViewEventArgs arg)
        {
            var nuGet = _allNuGets.First(item => item.Name == arg.Name);

            var existingItem = SelectedNuGets.FirstOrDefault(item => item.Name == arg.Name);

            if (existingItem is null)
            {
                var nugetDetails = new NuGetDetailsViewModel(nuGet);
                SelectedNuGets.Add(nugetDetails);
                SelectedNuGetDetailsViewModel = nugetDetails;
                await SelectedNuGetDetailsViewModel.LoadNuGetDetailsAsync();
            }
            else
            {
                SelectedNuGetDetailsViewModel = existingItem;
            }
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

        private void OnWorkspacePathChangedEvent(string path)
        {
            IsWorkspaceSet = true;
            WorkspacePath = path;
        }
    }
}
