using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using UpdateNugets.UI.Events;
using UpdateNugets.UI.Model;

namespace UpdateNugets.UI.ViewModel
{
    public class NuGetVersionsViewModel : ViewModelBase
    {
        private const string _statusMessage = "Versions loading...";
        private NugetModel _nugetModel;
        private bool _areVersionsLoading;
        private VersionModel _selectedVersion;
        private IEventAggregator _eventAggregator;
        private ObservableCollection<VersionModel> _versions = new ObservableCollection<VersionModel>();
        private bool _areVersionsVisible = true;
        private bool _isHigherVersionAvaiable;
        private bool _areMultipleVersions;

        public NuGetVersionsViewModel(NugetModel nugetModel, IEventAggregator eventAggregator)
        {
            _nugetModel = nugetModel;
            _eventAggregator = eventAggregator;
            UpdateNuGetCommand = new DelegateCommand(async () => await OnExecuteUpdateCommand());
        }

        public ObservableCollection<VersionModel> Versions
        {
            get { return _versions; }
            set
            {
                _versions = value;
                OnPropertyChanged(nameof(Versions));
                if (Versions.Count > 1)
                {
                    IsHigherVersionAvaiable = _nugetModel.IsHigherVersionAvailable;
                    AreMultipleVersions = _nugetModel.AreMultipleVersionUsed;
                }
            }
        }

        public VersionModel SelectedVersion
        {
            get { return _selectedVersion; }
            set
            {
                _selectedVersion = value;
                OnPropertyChanged(nameof(SelectedVersion));
                _nugetModel.CurrentSelectedVersion = _selectedVersion;
                _eventAggregator.GetEvent<SelectedVersionChanged>().Publish(_nugetModel);
            }
        }

        public DelegateCommand UpdateNuGetCommand { get; }

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

        public bool IsHigherVersionAvaiable
        {
            get { return _isHigherVersionAvaiable; }
            set
            {
                _isHigherVersionAvaiable = value;
                OnPropertyChanged(nameof(IsHigherVersionAvaiable));

            }
        }

        public bool AreMultipleVersions
        {
            get { return _areMultipleVersions; }
            set
            {
                _areMultipleVersions = value;
                OnPropertyChanged(nameof(AreMultipleVersions));
            }
        }

        public async Task LoadVersionsAsync()
        {
            _eventAggregator.GetEvent<PublishMessageEvent>().Publish(new PublishMessageEventArg
            {
                Message = _statusMessage,
                IsVisible = true
            });

            AreVersionsLoading = true;
            AreVersionsVisible = false;
            try
            {
                await _nugetModel.LoadNuGetVersions();
            }
            catch (Exception ex)
            {
                //log this
            }

            Versions = new ObservableCollection<VersionModel>(_nugetModel.Versions);
            SelectedVersion = Versions.First(item => item.IsUsed);
            AreVersionsLoading = false;
            AreVersionsVisible = true;

            _eventAggregator.GetEvent<PublishMessageEvent>().Publish(new PublishMessageEventArg
            {
                Message = _statusMessage,
                IsVisible = false
            });
        }

        private async Task OnExecuteUpdateCommand()
        {
            _nugetModel.UpdateNuGets();
            await LoadVersionsAsync();
        }
    }
}
