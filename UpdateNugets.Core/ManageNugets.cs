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
        private readonly InterogateNuGetFeed _interogateNuGetFeed;

        public ManageNugets(string projectPath)
        {
            _projectPath = projectPath;
            _interogateNuGetFeed = new InterogateNuGetFeed();
            InitializeNuGetsList();
        }

        public ObservableCollection<ProjectNuGet> NuGets { get; } = new ObservableCollection<ProjectNuGet>();

        public async Task<ObservableCollection<ProjectNuGet>> SearchAsync(string name, bool searchOnline = false, bool includePrerelease = false)
        {

            var result = new ObservableCollection<ProjectNuGet>();

            if (searchOnline)
            {
                var packages = await _interogateNuGetFeed.SearchAsync(name, 20, includePrerelease);
                foreach (var item in packages)
                {
                    result.Add(await ConvertPackageSearchMetadataToNuGetAsync(item));
                }

            }
            else
            {
                foreach (var item in NuGets)
                {
                    if (item.Name.ToLower().Contains(name.ToLower()))
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
                versions.Add(new Version
                {
                    NuGetVersion = item.Version.Version.ToString()
                });
            }

            return new ProjectNuGet(packageName, versions, _interogateNuGetFeed);

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
