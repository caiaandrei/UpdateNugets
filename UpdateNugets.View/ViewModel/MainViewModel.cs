using Prism.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UpdateNugets.Core;
using UpdateNugets.UI.Events;
using UpdateNugets.UI.Model;

namespace UpdateNugets.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private string _workspacePath;
        private bool _hasSelectedNuGet;
        private readonly IEventAggregator _eventAggregator;
        private string _statusText;
        private bool _isStatusVisible;
        private List<NugetModel> _allNuGets;
        private NuGetDetailsViewModel _selectedNuGetDetailsViewModel;
        private WorkspaceNuGetsManager _manageNuGets;
        private List<PublishMessageEventArg> _allStatusMessages;

        public MainViewModel(IEventAggregator eventAggregator,
                             NavigationViewModel nuGetsListViewModel,
                             SelectWorkspaceViewModel selectWorkspaceViewModel)
        {
            _eventAggregator = eventAggregator;

            NavigationViewModel = nuGetsListViewModel;
            SelectWorkspaceViewModel = selectWorkspaceViewModel;

            SelectedNuGets = new ObservableCollection<NuGetDetailsViewModel>();

            _eventAggregator.GetEvent<WorkspaceSelectedEvent>().Subscribe(OnWorkspaceChangedEvent);
            _eventAggregator.GetEvent<OpenDetailViewEvent>().Subscribe(OnOpenDetailViewEvent);
            _eventAggregator.GetEvent<PublishMessageEvent>().Subscribe(OnPublishMessageEvent);

            _allStatusMessages = new List<PublishMessageEventArg>();
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

        public NavigationViewModel NavigationViewModel { get; }

        public SelectWorkspaceViewModel SelectWorkspaceViewModel { get; }

        public string WorkspacePath
        {
            get { return _workspacePath; }
            set
            {
                _workspacePath = value;
                OnPropertyChanged(nameof(WorkspacePath));
                OnPropertyChanged(nameof(IsWorkspaceSet));
            }
        }

        public WorkspaceNuGetsManager ManageNuGets
        {
            get => _manageNuGets;
            set
            {
                _manageNuGets = value;
                _allNuGets = _manageNuGets.NuGets.Select(item => new NugetModel(item)).ToList();
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
                var nugetDetails = new NuGetDetailsViewModel(nuGet, _eventAggregator);
                SelectedNuGets.Add(nugetDetails);
                SelectedNuGetDetailsViewModel = nugetDetails;
                await SelectedNuGetDetailsViewModel.LoadNuGetDetailsAsync();
            }
            else
            {
                SelectedNuGetDetailsViewModel = existingItem;
            }
        }

        private void OnWorkspaceChangedEvent(WorkspaceSelectedEventArg arg)
        {
            IsWorkspaceSet = true;
            WorkspacePath = arg.WorkspacePath;
            ManageNuGets = new WorkspaceNuGetsManager(WorkspacePath, arg.PackagesSource);
        }

        private void OnPublishMessageEvent(PublishMessageEventArg arg)
        {
            if (arg.IsVisible)
            {
                _allStatusMessages.Add(arg);
                StatusText = arg.Message;
            }
            else
            {
                var existingMessage = _allStatusMessages.First(item => item.Message == arg.Message);

                if (existingMessage is null)
                {
                    //log this
                }

                _allStatusMessages.Remove(existingMessage);
            }

            if (_allStatusMessages.Count == 0)
            {
                StatusText = string.Empty;
            }
        }
    }
}
