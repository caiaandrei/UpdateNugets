using Prism.Events;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using UpdateNugets.Core;
using UpdateNugets.UI.Events;

namespace UpdateNugets.UI.ViewModel
{
    public class SelectedNuGetDetailsViewModel : ViewModelBase
    {
        private ProjectNuGet _nuGet;
        private ObservableCollection<Version> _version;
        private string _name;
        private Version _selectedVersion;
        private IEventAggregator _eventAggregator;
        private ObservableCollection<string> _dependencies = new ObservableCollection<string>();

        public SelectedNuGetDetailsViewModel(ProjectNuGet nuGet, IEventAggregator eventAggregator, ICommand updateNuGetCommand)
        {
            _eventAggregator = eventAggregator;
            _nuGet = nuGet;
            Versions = new ObservableCollection<Version>(_nuGet.Versions);
            Name = nuGet.Name;
            SelectedVersion = nuGet.CurrentVersion;
            UpdateNuGetCommand = updateNuGetCommand;
        }

        public ObservableCollection<Version> Versions
        {
            get { return _version; }
            set
            {
                _version = value;
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

        public ICommand UpdateNuGetCommand { get; }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }
}
