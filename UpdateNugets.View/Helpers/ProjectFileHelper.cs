using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace UpdateNugets.UI.Helpers
{
    public class ProjectFileHelper
    {
        public void GenerateProjectFile(string projectName, string projectFolderPath, string workspaceFolderPath)
        {
            var projectFilePath = Path.Combine(projectFolderPath, projectName);

            var xmlDocument = new XmlDocument();

            var root = xmlDocument.CreateElement("Root");

            var projectElement = xmlDocument.CreateElement("Project");
            projectElement.SetAttribute("Name", projectName);
            projectElement.SetAttribute("WorkspacePath", workspaceFolderPath);
            projectElement.SetAttribute("CreatedAt", DateTime.UtcNow.ToString());
            projectElement.SetAttribute("Status", "InProgress");

            root.AppendChild(projectElement);
            xmlDocument.AppendChild(root);

            Directory.CreateDirectory(projectFolderPath);
            xmlDocument.Save(projectFilePath + ".xml");
        }

        public void AddPackagesInProjectFile(string projectName, string projectFolderPath, IDictionary<string, IList<string>> packagesCollection)
        {
            var projectFilePath = Path.Combine(projectFolderPath, projectName + ".xml");
            var document = new XmlDocument();
            document.Load(projectFilePath);
            var root = document.DocumentElement;
            var packages = document.CreateElement("Packages");

            foreach (var item in packagesCollection)
            {
                foreach (var version in item.Value)
                {
                    var packageItem = document.CreateElement("PackageItem");
                    packageItem.SetAttribute("Name", item.Key);
                    packageItem.SetAttribute("InitialVersion", version);
                    packageItem.SetAttribute("CurrentVersion", version);
                    packages.AppendChild(packageItem);
                }
            }

            root.FirstChild.AppendChild(packages);

            document.Save(projectFilePath);
        }
    }
}
