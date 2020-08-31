using System.Collections.ObjectModel;
using UpdateNugets.Core;

namespace UpdateNugets.UI.ViewModel
{
    public class NuGetsListViewModel : ViewModelBase
    {
        private NuGet _selectedNuGet;
        private ObservableCollection<NuGet> _nuGets;
        private string _searchBoxText;

        public NuGetsListViewModel(ManageNugets manageNuGets)
        {
            NuGets = manageNuGets.NuGets;
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
