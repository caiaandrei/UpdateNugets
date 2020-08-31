using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace UpdateNugets.Core
{
    public class ManageNugets
    {
        private readonly string _projectPath;

        public ManageNugets(string projectPath)
        {
            _projectPath = projectPath;
            InitializeNuGetsList();
        }

        public ObservableCollection<NuGet> NuGets { get; } = new ObservableCollection<NuGet>();

        public ObservableCollection<NuGet> Search(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return NuGets;
            }

            var result = new ObservableCollection<NuGet>();
            foreach (var item in NuGets)
            {
                if (item.Name.ToLower().Contains(name))
                {
                    result.Add(item);
                }
            }

            return result;
        }

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
                        NuGets.Add(new NuGet(projectNuget.Key, new List<Version>
                        {
                            new Version
                            {
                                Files = new List<string> { projectPath },
                                NuGetVersion = projectNuget.Value
                            }
                        }));
                    }
                    else
                    {
                        var nugetVersion = nuget.Versions.FirstOrDefault(item => item.NuGetVersion == projectNuget.Value);

                        if (nugetVersion == null)
                        {
                            nuget.Versions.Add(new Version
                            {
                                Files = new List<string> { projectPath },
                                NuGetVersion = projectNuget.Value
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
