using Prism.Commands;
using Prism.Events;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Windows.Input;
using UpdateNugets.UI.Command;
using UpdateNugets.UI.Events;

namespace UpdateNugets.UI.ViewModel
{
    public class SelectWorkspaceViewModel : ValidationViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly DelegateCommand _okCommand;
        private string _workspacePath;

        public SelectWorkspaceViewModel(IBrowseWorkspacePathCommand browseWorkspacePathCommand,
                                        IEventAggregator eventAggregator)
        {
            BrowseWorkspacePathCommand = browseWorkspacePathCommand;
            _eventAggregator = eventAggregator;

            _okCommand = new DelegateCommand(OnExecuteOKCommand, OnCanExecuteOKCommand);

            WorkspacePath = string.Empty;

            PackagesSource = "https://api.nuget.org/v3/index.json";
        }

        [CustomValidation(typeof(SelectWorkspaceViewModel), nameof(ValidateWorkspace))]
        public string WorkspacePath
        {
            get => _workspacePath;
            set
            {
                _workspacePath = value;
                OnPropertyChanged(nameof(WorkspacePath));
                ValidateProperty(nameof(WorkspacePath), value);
                _okCommand.RaiseCanExecuteChanged();
            }
        }

        private string _packagesSource;

        public string PackagesSource
        {
            get { return _packagesSource; }
            set
            {
                _packagesSource = value;
                OnPropertyChanged(nameof(PackagesSource));
            }
        }


        public ICommand BrowseWorkspacePathCommand { get; }

        public ICommand OkCommand => _okCommand;

        public static ValidationResult ValidateWorkspace(object obj, ValidationContext context)
        {
            var self = (SelectWorkspaceViewModel)context.ObjectInstance;
            return context.MemberName switch
            {
                nameof(WorkspacePath) => self.ValidateWorkspacePath((string)obj),
                _ => ValidationResult.Success,
            };
        }

        private bool OnCanExecuteOKCommand()
        {
            return !HasErrors;
        }

        private void OnExecuteOKCommand()
        {
            _eventAggregator.GetEvent<WorkspaceSelectedEvent>().Publish(new WorkspaceSelectedEventArg
            {
                WorkspacePath = WorkspacePath,
                PackagesSource = PackagesSource
            });
        }

        private ValidationResult ValidateWorkspacePath(string path)
        {
            try
            {
                var directory = new DirectoryInfo(path);

                if (!directory.Exists)
                {
                    return new ValidationResult("Please enter a valid directory path");
                }

                return ValidationResult.Success;
            }
            catch
            {
                return new ValidationResult("Please enter a valid directory path");
            }
        }
    }
}
