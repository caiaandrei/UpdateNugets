namespace UpdateNugets.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private string _projectPath;

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
