using Prism.Commands;
using Prism.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using UpdateNugets.UI.Events;

namespace UpdateNugets.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase
    {
        private NuGetDetailsViewModel _selectedNuGet;
        private ObservableCollection<NavigationItemViewModel> _visibleNuGets = new ObservableCollection<NavigationItemViewModel>();
        private IEventAggregator _eventAggregator;
        private ObservableCollection<NavigationItemViewModel> _allNuGets = new ObservableCollection<NavigationItemViewModel>();
        private string _searchBoxText;

        public NavigationViewModel(IEventAggregator eventAggregator)
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

        public ObservableCollection<NavigationItemViewModel> VisibleNuGets
        {
            get => _visibleNuGets;
            set
            {
                _visibleNuGets = value;

                OnPropertyChanged(nameof(VisibleNuGets));
                OnPropertyChanged(nameof(NumberOfNugetsInUsed));
            }
        }

        public int NumberOfNugetsInUsed => VisibleNuGets.Count;

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

        public void Load(List<string> nugets)
        {
            foreach (var item in nugets)
            {
                _allNuGets.Add(new NavigationItemViewModel(item, _eventAggregator));
            }
            VisibleNuGets = new ObservableCollection<NavigationItemViewModel>(_allNuGets);
        }

        private void ExecuteSearchCommand()
        {
            if (string.IsNullOrEmpty(SearchBoxText))
            {
                VisibleNuGets = new ObservableCollection<NavigationItemViewModel>(_allNuGets);
            }
            else
            {
                VisibleNuGets = new ObservableCollection<NavigationItemViewModel>(_allNuGets.Where(item => item.Name.Contains(SearchBoxText, System.StringComparison.OrdinalIgnoreCase)));
            }
        }


    }
}
