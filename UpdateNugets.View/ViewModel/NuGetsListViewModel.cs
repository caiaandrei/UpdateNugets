using Prism.Commands;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using UpdateNugets.Core;
using UpdateNugets.UI.Events;

namespace UpdateNugets.UI.ViewModel
{
    public class NuGetsListViewModel : ViewModelBase
    {
        private NuGetDetailsViewModel _selectedNuGet;
        private ObservableCollection<NuGetDetailsViewModel> _nuGets = new ObservableCollection<NuGetDetailsViewModel>();
        private IEventAggregator _eventAggregator;
        private ObservableCollection<NuGetDetailsViewModel> _allNuGetsDetail = new ObservableCollection<NuGetDetailsViewModel>();
        private string _searchBoxText;

        public NuGetsListViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            SearchCommand = new DelegateCommand(ExecuteSearchCommand);
        }

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

        public ObservableCollection<NuGetDetailsViewModel> NuGetsDetail
        {
            get => _nuGets;
            set
            {
                _nuGets = value;

                //if (!_nuGets.Any())
                //{
                //    SelectedNuGet = null;
                //}
                //else
                //{
                //    SelectedNuGet = _nuGets[0];
                //}

                OnPropertyChanged(nameof(NuGetsDetail));
                OnPropertyChanged(nameof(NumberOfNugetsInUsed));
            }
        }

        public int NumberOfNugetsInUsed => NuGetsDetail.Count;

        public NuGetDetailsViewModel SelectedNuGetDetails
        {
            get { return _selectedNuGet; }
            set
            {
                _selectedNuGet = value;
                OnPropertyChanged(nameof(SelectedNuGetDetails));
                if (_selectedNuGet != null)
                {
                    _eventAggregator.GetEvent<SelectedNuGetChangedEvent>().Publish(_selectedNuGet);
                }
            }
        }

        public void Load(ManageNugets manageNuGets)
        {
            foreach (var item in manageNuGets.NuGets)
            {
                _allNuGetsDetail.Add(new NuGetDetailsViewModel(item)
                {
                    NuGetVersionsViewModel = new NuGetVersionsViewModel(_eventAggregator)
                });
            }
            NuGetsDetail = new ObservableCollection<NuGetDetailsViewModel>(_allNuGetsDetail);
        }

        private void ExecuteSearchCommand()
        {
            if (string.IsNullOrEmpty(SearchBoxText))
            {
                NuGetsDetail = new ObservableCollection<NuGetDetailsViewModel>(_allNuGetsDetail);
            }
            else
            {
                NuGetsDetail = new ObservableCollection<NuGetDetailsViewModel>(_allNuGetsDetail.Where(item => item.Name.Contains(SearchBoxText)));
            }
        }
    }
}
