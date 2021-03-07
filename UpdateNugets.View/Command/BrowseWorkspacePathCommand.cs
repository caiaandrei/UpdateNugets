using System;
using System.Windows.Input;
using UpdateNugets.UI.Helpers;
using UpdateNugets.UI.ViewModel;

namespace UpdateNugets.UI.Command
{
    public class BrowseWorkspacePathCommand : IBrowseWorkspacePathCommand
    {
        private BrowsePathHelper _browsePathHelper;

        public BrowseWorkspacePathCommand(BrowsePathHelper browsePathHelper)
        {
            _browsePathHelper = browsePathHelper;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            var viewModel = parameter as SelectWorkspaceViewModel;

            viewModel.WorkspacePath = _browsePathHelper.GetFolderPath();
        }
    }
}
