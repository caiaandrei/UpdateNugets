using Prism.Events;
using System.Collections.ObjectModel;
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

        public SelectedNuGetDetailsViewModel(ProjectNuGet nuGet, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _nuGet = nuGet;
            Versions = new ObservableCollection<Version>(_nuGet.Versions);
            Name = nuGet.Name;
            SelectedVersion = Versions[0];
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

        public Version SelectedVersion
        {
            get { return _selectedVersion; }
            set
            {
                _selectedVersion = value;
                OnPropertyChanged(nameof(SelectedVersion));
                _eventAggregator.GetEvent<SelectedVersionChanged>().Publish(SelectedVersion);
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private IEventAggregator _eventAggregator;
    }
}
