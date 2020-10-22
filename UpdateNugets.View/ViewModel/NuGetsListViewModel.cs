using Prism.Events;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using UpdateNugets.Core;
using UpdateNugets.UI.Command;
using UpdateNugets.UI.Events;

namespace UpdateNugets.UI.ViewModel
{
    public class NuGetsListViewModel : ViewModelBase
    {
        private ProjectNuGet _selectedNuGet;
        private ObservableCollection<ProjectNuGet> _nuGets;
        private string _searchBoxText = string.Empty;
        private bool _searchOnline;
        private IEventAggregator _eventAggregator;
        private ManageNugets _manageNuGets;

        public NuGetsListViewModel(ManageNugets manageNuGets, IEventAggregator eventAggregator)
        {
            _manageNuGets = manageNuGets;
            _eventAggregator = eventAggregator;
            SearchCommand = new SearchCommand();
            ClearCommand = new ClearCommand();
            NuGets = manageNuGets.NuGets;
        }

        public ObservableCollection<ProjectNuGet> NuGets
        {
            get => _nuGets;
            set
            {
                _nuGets = value;

                if (!_nuGets.Any())
                {
                    SelectedNuGet = null;
                }
                else
                {
                    SelectedNuGet = _nuGets[0];
                }

                OnPropertyChanged(nameof(NuGets));
                OnPropertyChanged(nameof(NumberOfNugetsInUsed));
            }
        }

        public int NumberOfNugetsInUsed => NuGets.Count;

        public ProjectNuGet SelectedNuGet
        {
            get { return _selectedNuGet; }
            set
            {
                _selectedNuGet = value;
                OnPropertyChanged(nameof(SelectedNuGet));
                if (_selectedNuGet != null)
                {
                    _eventAggregator.GetEvent<SelectedNuGetChangedEvent>().Publish(_selectedNuGet);
                }
            }
        }

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

        public bool SearchOnline
        {
            get { return _searchOnline; }
            set
            {
                _searchOnline = value;
                OnPropertyChanged(nameof(SearchOnline));
            }
        }

        public ICommand SearchCommand { get; }

        public ICommand ClearCommand { get; }

        public async Task SearchAsync()
        {
            NuGets = await _manageNuGets.SearchAsync(SearchBoxText.Trim(), SearchOnline);
        }
    }
}
