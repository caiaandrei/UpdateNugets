using Prism.Events;
using System.ComponentModel.Design;
using System.Windows.Input;
using UpdateNugets.Core;
using UpdateNugets.UI.Events;

namespace UpdateNugets.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private string _projectPath;
        private NuGetsListViewModel _nuGetsListViewModel;
        private SelectedNuGetVersionFilesViewModel _selectedNuGetVersionFilesViewModel;
        private SelectedNuGetDetailsViewModel _selectedNuGetDetailsViewModel;
        private bool _hasSelectedNuGet;
        private readonly IEventAggregator _eventAggregator;

        public MainViewModel(IEventAggregator eventAggregator, ICommand changePathCommand)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<SelectedNuGetChangedEvent>().Subscribe(OnSelectedNuGetChangedEvent);
            _eventAggregator.GetEvent<SelectedVersionChanged>().Subscribe(OnSelectedVersionChangedEvent);

            ChangePathCommand = changePathCommand;
        }

        public ICommand ChangePathCommand { get; }

        public string ProjectPath
        {
            get { return _projectPath; }
            set
            {
                _projectPath = value;
                OnPropertyChanged(nameof(ProjectPath));
                ManageNuGets = new ManageNugets(_projectPath);
                NuGetsListViewModel = new NuGetsListViewModel(ManageNuGets, _eventAggregator);
            }
        }

        public ManageNugets ManageNuGets
        {
            get;
            set;
        }

        public NuGetsListViewModel NuGetsListViewModel
        {
            get { return _nuGetsListViewModel; }
            set
            {
                _nuGetsListViewModel = value;
                OnPropertyChanged(nameof(NuGetsListViewModel));
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

        public SelectedNuGetDetailsViewModel SelectedNuGetDetailsViewModel
        {
            get { return _selectedNuGetDetailsViewModel; }
            set
            {
                _selectedNuGetDetailsViewModel = value;
                OnPropertyChanged(nameof(SelectedNuGetDetailsViewModel));
            }
        }

        public SelectedNuGetVersionFilesViewModel SelectedNuGetVersionFilesViewModel
        {
            get { return _selectedNuGetVersionFilesViewModel; }
            set
            {
                _selectedNuGetVersionFilesViewModel = value;
                OnPropertyChanged(nameof(SelectedNuGetVersionFilesViewModel));
            }
        }

        private async void OnSelectedNuGetChangedEvent(ProjectNuGet nuGet)
        {
            nuGet = await ManageNuGets.SearchNuGetVersions(nuGet);
            SelectedNuGetDetailsViewModel = new SelectedNuGetDetailsViewModel(nuGet, _eventAggregator);
            HasSelectedNuGet = true;
        }

        private void OnSelectedVersionChangedEvent(Version version)
        {
            SelectedNuGetVersionFilesViewModel = new SelectedNuGetVersionFilesViewModel(version);
        }

    }
}
