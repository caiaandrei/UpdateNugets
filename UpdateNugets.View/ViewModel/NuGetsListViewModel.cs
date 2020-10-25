using Prism.Events;
using System.Collections.ObjectModel;
using System.Linq;
using UpdateNugets.Core;
using UpdateNugets.UI.Events;

namespace UpdateNugets.UI.ViewModel
{
    public class NuGetsListViewModel : ViewModelBase
    {
        private ProjectNuGet _selectedNuGet;
        private ObservableCollection<ProjectNuGet> _nuGets = new ObservableCollection<ProjectNuGet>();
        private IEventAggregator _eventAggregator;

        public NuGetsListViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
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

        public void Load(ManageNugets manageNuGets)
        {
            NuGets = manageNuGets.NuGets;
        }
    }
}
