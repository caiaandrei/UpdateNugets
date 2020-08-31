using Prism.Events;
using System;
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
        private readonly IEventAggregator _eventAggregator;

        public MainViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<SelectedNuGetChangedEvent>().Subscribe(OnSelectedNuGetChangedEvent);
        }

        public string ProjectPath
        {
            get { return _projectPath; }
            set
            {
                _projectPath = value;
                OnPropertyChanged(nameof(ProjectPath));
                ManageNuGets = new ManageNugets(_projectPath);
                NuGetsListViewModel = new NuGetsListViewModel(ManageNuGets);
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

        private void OnSelectedNuGetChangedEvent(NuGet nuGet)
        {
            SelectedNuGetDetailsViewModel = new SelectedNuGetDetailsViewModel(nuGet);
            SelectedNuGetVersionFilesViewModel = new SelectedNuGetVersionFilesViewModel(nuGet);
        }

    }
}
