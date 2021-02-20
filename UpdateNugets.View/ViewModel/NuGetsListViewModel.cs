using Prism.Events;
using System.Collections.ObjectModel;
using System.Linq;
using UpdateNugets.Core;
using UpdateNugets.UI.Events;

namespace UpdateNugets.UI.ViewModel
{
    public class NuGetsListViewModel : ViewModelBase
    {
        private NuGetDetailsViewModel _selectedNuGet;
        private ObservableCollection<NuGetDetailsViewModel> _nuGets = new ObservableCollection<NuGetDetailsViewModel>();
        private IEventAggregator _eventAggregator;

        public NuGetsListViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
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
                NuGetsDetail.Add(new NuGetDetailsViewModel(item)
                {
                    NuGetVersionsViewModel = new NuGetVersionsViewModel(_eventAggregator)
                });
            }
        }
    }
}
