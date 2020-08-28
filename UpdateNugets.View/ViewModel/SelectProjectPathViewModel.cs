using System.Windows.Input;

namespace UpdateNugets.UI.ViewModel
{
    public class SelectProjectPathViewModel : ViewModelBase
    {
        private string _projectPath;

        public SelectProjectPathViewModel(ICommand browsePathCommand)
        {
            BrowsePathCommand = browsePathCommand;
        }

        public ICommand BrowsePathCommand { get; }

        public string ProjectPath
        {
            get { return _projectPath; }
            set
            {
                _projectPath = value;
                OnPropertyChanged(nameof(ProjectPath));
            }
        }
    }
}
