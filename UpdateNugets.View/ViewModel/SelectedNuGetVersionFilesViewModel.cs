using Prism.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using UpdateNugets.Core;

namespace UpdateNugets.UI.ViewModel
{
    public class SelectedNuGetVersionFilesViewModel : ViewModelBase
    {
        private ObservableCollection<string> _files = new ObservableCollection<string>();
        private ObservableCollection<string> _allFiles = new ObservableCollection<string>();
        private bool _areVersionsLoading;
        private bool _areVersionsVisible = true;
        private string _searchBoxText;

        public SelectedNuGetVersionFilesViewModel()
        {
            SearchCommand = new DelegateCommand(()=> ExecuteSearchCommand());
        }

        public ObservableCollection<string> Files
        {
            get { return _files; }
            set
            {
                _files = value;
                OnPropertyChanged(nameof(Files));
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

        public int NumberOfFilesInUsed => Files.Count;

        public bool AreVersionsLoading
        {
            get { return _areVersionsLoading; }
            set
            {
                _areVersionsLoading = value;
                AreVersionsVisible = !_areVersionsLoading;
                OnPropertyChanged(nameof(AreVersionsLoading));
            }
        }

        public bool AreVersionsVisible
        {
            get { return _areVersionsVisible; }
            set
            {
                _areVersionsVisible = value;
                OnPropertyChanged(nameof(AreVersionsVisible));
            }
        }

        public void Load(Version selectedVersion)
        {
            _allFiles = selectedVersion.Files;
            Files = _allFiles;
        }

        private void ExecuteSearchCommand()
        {
            Files = new ObservableCollection<string>(_allFiles.Where(item => item.ToLower().Contains(SearchBoxText.Trim().ToLower())));
        }
    }
}
