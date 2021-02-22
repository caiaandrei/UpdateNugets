using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace UpdateNugets.Core
{
    public class WorkspaceNuGetsManager
    {
        private readonly string _projectPath;
        private readonly InterogateNuGetFeed _interogateNuGetFeed;

        public WorkspaceNuGetsManager(string projectPath, string packagesSource)
        {
            _projectPath = projectPath;
            _interogateNuGetFeed = new InterogateNuGetFeed(packagesSource);
            InitializeNuGetsList();
        }

        public ObservableCollection<ProjectNuGet> NuGets { get; } = new ObservableCollection<ProjectNuGet>();

        private void InitializeNuGetsList()
        {
            var allProjects = Directory.GetFiles(_projectPath, "*.csproj", SearchOption.AllDirectories);

            foreach (var projectPath in allProjects)
            {
                var project = new Csproj(projectPath);
                var projectNugets = project.GetAllNugets();
                foreach (var projectNuget in projectNugets)
                {
                    var nuget = NuGets.FirstOrDefault(item => item.Name.Equals(projectNuget.Key));
                    if (nuget == null)
                    {
                        var versions = new List<Version>
                        {
                            new Version
                            {
                                Files = new ObservableCollection<string> { projectPath },
                                NuGetVersion = projectNuget.Value.Trim(),
                                IsTheCurrentVersion = true
                            }
                        };

                        NuGets.Add(new ProjectNuGet(projectNuget.Key, versions, _interogateNuGetFeed));
                    }
                    else
                    {
                        var nugetVersion = nuget.Versions.FirstOrDefault(item => item.NuGetVersion.Equals(projectNuget.Value));

                        if (nugetVersion == null)
                        {
                            nuget.Versions.Add(new Version
                            {
                                Files = new ObservableCollection<string> { projectPath },
                                NuGetVersion = projectNuget.Value,
                                IsTheCurrentVersion = true
                            });
                        }
                        else
                        {
                            nugetVersion.Files.Add(projectPath);
                        }
                    }
                }
            }
        }
    }
}
