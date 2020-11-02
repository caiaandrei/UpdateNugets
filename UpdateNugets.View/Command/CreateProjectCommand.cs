using System;
using System.IO;
using System.Xml;
using UpdateNugets.UI.ViewModel;

namespace UpdateNugets.UI.Command
{
    public class CreateProjectCommand : ICreateProjectCommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            var viewModel = parameter as NewProjectViewModel;
            viewModel.ProjectFileHelper.GenerateProjectFile(viewModel.ProjectName, viewModel.ProjectFolderPath, viewModel.WorkspacePath);
            viewModel.OnProjectCreated();
        }
    }
}
