using System.Collections.ObjectModel;
using UpdateNugets.Core;

namespace UpdateNugets.UI.ViewModel
{
    public class SelectedNuGetVersionFilesViewModel : ViewModelBase
    {
        private Version _version;
        private ObservableCollection<string> _files;

        public SelectedNuGetVersionFilesViewModel(Version version)
        {
            _version = version;
            Files = new ObservableCollection<string>(_version.Files);
        }

        public ObservableCollection<string> Files
        {
            get { return _files; }
            set
            {
                _files = value;
                OnPropertyChanged(nameof(Files));
            }
        }

        public int NumberOfFilesInUsed => Files.Count;


    }
}
