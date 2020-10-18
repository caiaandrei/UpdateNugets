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

        public async Task<ObservableCollection<ProjectNuGet>> SearchAsync(string name, bool searchOnline = false)
        {
            var intergogateNuGetFeed = new InterogateNuGetFeed();

            var result = new ObservableCollection<ProjectNuGet>();

            if (searchOnline)
            {
                await intergogateNuGetFeed.SearchAsync(name, 20);
                foreach (var item in intergogateNuGetFeed.Packages)
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

        public async Task<ProjectNuGet> SearchNuGetVersions(ProjectNuGet nuGet)
        {
            var intergogateNuGetFeed = new InterogateNuGetFeed();
            var foundPackages = await intergogateNuGetFeed.SearchAsync(nuGet.Name, 20);

            if (foundPackages.Count == 0)
            {
                return nuGet;
            }

            foreach (var package in foundPackages)
            {
                if (package.Title.Equals(nuGet.Name) || package.Identity.Id.Equals(nuGet.Name))
                {
                    var allVersions = await package.GetVersionsAsync();
                    List<Version> versions = new List<Version>();

                    foreach (var item in allVersions)
                    {
                        var packageVersion = item.Version.Version.ToString();
                        if (IsTheSameVersion(nuGet, packageVersion))
                        {
                            nuGet.Versions.Add(new Version
                            {
                                NuGetVersion = packageVersion,
                                Files = new List<string>()
                            });
                        }
                    }
                    break;
                }
            }

            //nuGet.Versions.Select(item =>
            //{
            //    if (item.NuGetVersion.LastIndexOf('0') == )
            //    {
            //        item.NuGetVersion = item.NuGetVersion.Remove(item.NuGetVersion.Length - 2);
            //    }
            //    return item;
            //}).ToList();

            nuGet.Versions.OrderByDescending(item => item.NuGetVersion);

            return nuGet;
        }

        private bool IsTheSameVersion(ProjectNuGet nuGet, string packageVersion)
        {
            return !nuGet.Versions.Any(version =>
            {
                return packageVersion.Equals(version.NuGetVersion)
                       || (packageVersion.Contains(version.NuGetVersion) && packageVersion.LastOrDefault().Equals('0'));
            });
        }

        public async Task<IList<string>> GetDependecies(ProjectNuGet selectedNuGet, string nuGetVersion)
        {
            var interogateNuGetFeed = new InterogateNuGetFeed();
            return await interogateNuGetFeed.GetDependecies(selectedNuGet.Name, nuGetVersion);
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
                                NuGetVersion = projectNuget.Value.Trim(),
                                IsTheCurrentVersion = true
                            }
                        }));
                    }
                    else
                    {
                        var nugetVersion = nuget.Versions.FirstOrDefault(item => item.NuGetVersion.Equals(projectNuget.Value));

                        if (nugetVersion == null)
                        {
                            nuget.Versions.Add(new Version
                            {
                                Files = new List<string> { projectPath },
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

        public void UpdateNuGets(string nugetName, string newNuGetVersion, IList<string> files)
        {
            foreach (var file in files)
            {
                var project = new Csproj(file);
                project.UpdateANuget(nugetName, newNuGetVersion);
            }
        }
    }
}
