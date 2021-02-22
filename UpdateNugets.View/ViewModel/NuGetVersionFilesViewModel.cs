using Prism.Commands;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using UpdateNugets.UI.Events;
using UpdateNugets.UI.Model;

namespace UpdateNugets.UI.ViewModel
{
    public class NuGetVersionFilesViewModel : ViewModelBase
    {
        private ObservableCollection<string> _visibleFiles = new ObservableCollection<string>();
        private ObservableCollection<string> _allFiles = new ObservableCollection<string>();
        private string _searchBoxText;
        private NugetModel _nugetModel;
        private IEventAggregator _eventAggregator;

        public NuGetVersionFilesViewModel(NugetModel nugetModel, IEventAggregator eventAggregator)
        {
            _nugetModel = nugetModel;
            _eventAggregator = eventAggregator;
            SearchCommand = new DelegateCommand(() => ExecuteSearchCommand());
            _eventAggregator.GetEvent<SelectedVersionChanged>().Subscribe(OnSelectedVersionChanged);
        }

        public ObservableCollection<string> VisibleFiles
        {
            get { return _visibleFiles; }
            set
            {
                _visibleFiles = value;
                OnPropertyChanged(nameof(VisibleFiles));
                OnPropertyChanged(nameof(NumberOfFilesInUsed));
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

        public ICommand SearchCommand { get; }

        public int NumberOfFilesInUsed => VisibleFiles.Count;

        private void ExecuteSearchCommand()
        {
            VisibleFiles = new ObservableCollection<string>(_allFiles.Where(item => item.ToLower().Contains(SearchBoxText.Trim().ToLower())));
        }

        private void OnSelectedVersionChanged(NugetModel nugetModel)
        {
            if (_nugetModel.Name != nugetModel.Name)
            {
                return;
            }

            _allFiles = new ObservableCollection<string>(nugetModel.CurrentSelectedVersion.Files);
            VisibleFiles = _allFiles;
        }
    }
}
