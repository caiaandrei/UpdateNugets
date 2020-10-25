using System.Collections.ObjectModel;
using UpdateNugets.Core;

namespace UpdateNugets.UI.ViewModel
{
    public class SelectedNuGetVersionFilesViewModel : ViewModelBase
    {
        private ObservableCollection<string> _files = new ObservableCollection<string>();

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

        public void Load(Version selectedVersion)
        {
            Files = new ObservableCollection<string>(selectedVersion.Files);
        }
    }
}
