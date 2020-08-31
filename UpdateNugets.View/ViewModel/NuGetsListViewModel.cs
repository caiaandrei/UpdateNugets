using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Input;
using UpdateNugets.Core;
using UpdateNugets.UI.Command;
using UpdateNugets.UI.Events;

namespace UpdateNugets.UI.ViewModel
{
    public class NuGetsListViewModel : ViewModelBase
    {
        private NuGet _selectedNuGet;
        private ObservableCollection<NuGet> _nuGets;
        private string _searchBoxText;
        private IEventAggregator _eventAggregator;
        private ManageNugets _manageNuGets;

        public NuGetsListViewModel(ManageNugets manageNuGets, IEventAggregator eventAggregator)
        {
            _manageNuGets = manageNuGets;
            NuGets = manageNuGets.NuGets;
            _eventAggregator = eventAggregator;
            SearchCommand = new SearchCommand();
            ClearCommand = new ClearCommand();
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
                OnPropertyChanged(nameof(SearchBoxText));
            }
        }

        public ICommand SearchCommand { get; }

        public ICommand ClearCommand { get; }

        public void Search()
        {
            NuGets = _manageNuGets.Search(SearchBoxText);
        }
    }
}
