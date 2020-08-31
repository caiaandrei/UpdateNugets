using Prism.Events;
using System.Collections.ObjectModel;
using UpdateNugets.Core;
using UpdateNugets.UI.Events;

namespace UpdateNugets.UI.ViewModel
{
    public class NuGetsListViewModel : ViewModelBase
    {
        private NuGet _selectedNuGet;
        private ObservableCollection<NuGet> _nuGets;
        private string _searchBoxText;
        private IEventAggregator _eventAggregator;

        public NuGetsListViewModel(ManageNugets manageNuGets, IEventAggregator eventAggregator)
        {
            NuGets = manageNuGets.NuGets;
            _eventAggregator = eventAggregator;
        }

        public ObservableCollection<NuGet> NuGets
        {
            get => _nuGets;
            set
            {
                _nuGets = value;
                OnPropertyChanged(nameof(NuGets));
            }
        }

        public NuGet SelectedNuGet
        {
            get { return _selectedNuGet; }
            set
            {
                _selectedNuGet = value;
                OnPropertyChanged(nameof(SelectedNuGet));
                _eventAggregator.GetEvent<SelectedNuGetChangedEvent>().Publish(_selectedNuGet);
            }
        }

        public string SearchBoxText
        {
            get { return _searchBoxText; }
            set
            {
                _searchBoxText = value;
                OnPropertyChanged(nameof(SearchBoxText));
            }
        }
    }
}
