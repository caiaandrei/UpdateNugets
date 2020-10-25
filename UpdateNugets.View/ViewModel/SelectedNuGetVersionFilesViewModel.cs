using System.Collections.ObjectModel;
using UpdateNugets.Core;

namespace UpdateNugets.UI.ViewModel
{
    public class SelectedNuGetVersionFilesViewModel : ViewModelBase
    {
        private ObservableCollection<string> _files = new ObservableCollection<string>();
        private bool _areVersionsLoading;
        private bool _areVersionsVisible = true;

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
            Files = new ObservableCollection<string>(selectedVersion.Files);
        }
    }
}
