using System;
using System.Windows.Input;
using UpdateNugets.UI.Command;

namespace UpdateNugets.UI.ViewModel
{
    public class NewProjectViewModel : ViewModelBase
    {
        private string _projectPath;
        private string _workspacePath;
        private string _projectName;

        public NewProjectViewModel(IBrowseProjectPathCommand browseProjectPathCommand,
                                   IBrowseWorkspacePathCommand browseWorkspacePathCommand,
                                   ICreateProjectCommand createProjectCommand)
        {
            BrowseProjectPathCommand = browseProjectPathCommand;
            BrowseWorkspacePathCommand = browseWorkspacePathCommand;
            CreateProjectCommand = createProjectCommand;
        }

        public event EventHandler ProjectCreated;

        public ICommand BrowseProjectPathCommand { get; }

        public ICommand BrowseWorkspacePathCommand { get; }

        public ICommand CreateProjectCommand { get; }

        public string ProjectName 
        {
            get => _projectName;
            set
            {
                _projectName = value;
                OnPropertyChanged(nameof(ProjectName));
            }
        }

        public string ProjectPath
        {
            get => _projectPath;
            set
            {
                _projectPath = value;
                OnPropertyChanged(nameof(ProjectPath));
            }
        }

        public string WorkspacePath 
        {
            get => _workspacePath;
            set
            {
                _workspacePath = value;
                OnPropertyChanged(nameof(WorkspacePath));
            }
        }

        public void OnProjectCreated()
        {
            ProjectCreated?.Invoke(this, EventArgs.Empty);
        }
    }
}
