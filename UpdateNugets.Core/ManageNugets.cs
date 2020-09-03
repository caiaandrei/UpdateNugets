using NuGet.Packaging;
using NuGet.Protocol.Core.Types;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

        public ObservableCollection<ProjectNuGet> NuGets { get; } = new ObservableCollection<ProjectNuGet>();

        public async System.Threading.Tasks.Task<ObservableCollection<ProjectNuGet>> SearchAsync(string name, bool searchOnline = false)
        {
            var intergogateNuGetFeed = new InterogateNuGetFeed();

            var result = new ObservableCollection<ProjectNuGet>();

            if (searchOnline)
            {
                await intergogateNuGetFeed.SearchAsync(name);
                foreach (var item in intergogateNuGetFeed.Packages)
                {
                    result.Add(await ConvertPackageSearchMetadataToNuGetAsync(item));
                }

            }
            else
            {
                foreach (var item in NuGets)
                {
                    if (item.Name.ToLower().Contains(name))
                    {
                        result.Add(item);
                    }
                }
            }

            return result;
        }

        private async Task<ProjectNuGet> ConvertPackageSearchMetadataToNuGetAsync(IPackageSearchMetadata packageSearchMetadata)
        {
            var packageName = packageSearchMetadata.Title;
            var allVersions = await packageSearchMetadata.GetVersionsAsync();
            List<Version> versions = new List<Version>();

            foreach (var item in allVersions)
            {
                versions.Add(new Version{
                    NuGetVersion = item.Version.Version.ToString(),
                    Files = new List<string>()
                });
            }

            return new ProjectNuGet(packageName, versions);

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
                        NuGets.Add(new ProjectNuGet(projectNuget.Key, new List<Version>
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
