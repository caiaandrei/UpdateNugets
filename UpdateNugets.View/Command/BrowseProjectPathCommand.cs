using System;
using System.IO;
using UpdateNugets.UI.Helpers;
using UpdateNugets.UI.ViewModel;

namespace UpdateNugets.UI.Command
{
    public class BrowseProjectPathCommand : IBrowseProjectPathCommand
    {
        private BrowsePathHelper _browsePathHelper;

        public BrowseProjectPathCommand(BrowsePathHelper browsePathHelper)
        {
            _browsePathHelper = browsePathHelper;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            var viewModel = parameter as NewProjectViewModel;
            var selectedPath = _browsePathHelper.GetFolderPath();
            viewModel.ProjectFolderPath = Path.Combine(selectedPath, viewModel.ProjectName);
        }
    }
}
