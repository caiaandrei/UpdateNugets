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

            var xmlDocument = new XmlDocument();

            var root = xmlDocument.CreateElement("Root");

            var projectElement = xmlDocument.CreateElement("Project");
            projectElement.SetAttribute("Name", viewModel.ProjectName);
            projectElement.SetAttribute("WorkspacePath", viewModel.WorkspacePath);
            projectElement.SetAttribute("CreatedAt", DateTime.UtcNow.ToString());
            projectElement.SetAttribute("Status", "InProgress");

            root.AppendChild(projectElement);
            xmlDocument.AppendChild(root);

            Directory.CreateDirectory(viewModel.ProjectPath);
            xmlDocument.Save(Path.Combine(viewModel.ProjectPath, viewModel.ProjectName + ".xml"));
            viewModel.OnProjectCreated();
        }
    }
}
