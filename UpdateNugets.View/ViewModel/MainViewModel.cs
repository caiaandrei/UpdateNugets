using UpdateNugets.Core;

namespace UpdateNugets.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private string _projectPath;
        private NuGetsListViewModel _nuGetsListViewModel;

        public string ProjectPath
        {
            get { return _projectPath; }
            set
            {
                _projectPath = value;
                OnPropertyChanged(nameof(ProjectPath));
            }
        }

        public ManageNugets ManageNuGets
        {
            get;
            set;
        }

        public NuGetsListViewModel NuGetsListViewModel
        {
            get { return _nuGetsListViewModel; }
            set
            {
                _nuGetsListViewModel = value;
                OnPropertyChanged(nameof(NuGetsListViewModel));
            }
        }

    }
}
