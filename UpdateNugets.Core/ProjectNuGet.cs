using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UpdateNugets.Core
{
    public class ProjectNuGet
    {
        private Version _currentSelectedVersion;
        private Version _currentVersion;
        private readonly InterogateNuGetFeed _interogateNuGetFeed;

        public ProjectNuGet(string name, IList<Version> versions, InterogateNuGetFeed interogateNuGetFeed)
        {
            Name = name;
            Versions = versions;
            CurrentVersion = Versions.FirstOrDefault(version => version.IsTheCurrentVersion);
            InitialNuGetVersion = CurrentVersion.NuGetVersion;
            _interogateNuGetFeed = interogateNuGetFeed;
        }

        public event EventHandler CurrentSelectedVersionChanged;

        public string Name { get; }

        public IList<Version> Versions { get; }

        public Version CurrentVersion
        {
            get => _currentVersion;
            set => _currentVersion = value;
        }

        public Version CurrentSelectedVersion
        {
            get => _currentSelectedVersion;
            set
            {
                _currentSelectedVersion = value;
                CurrentSelectedVersionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public string InitialNuGetVersion { get; }

        public bool AreMultipleVersionUsed => Versions.Count(item => item.IsTheCurrentVersion) > 1;

        public override string ToString()
        {
            return Name;
        }

        public async Task<ProjectNuGet> SearchNuGetVersions(ProjectNuGet nuGet)
        {
            var foundPackages = await _interogateNuGetFeed.SearchAsync(nuGet.Name, 20);

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
                                NuGetVersion = packageVersion
                            });
                        }
                    }
                    break;
                }
            }

            OrderNuGetVersions(nuGet);

            return nuGet;
        }

        public async Task<IList<string>> GetDependecies(ProjectNuGet selectedNuGet, string nuGetVersion)
        {
            return await _interogateNuGetFeed.GetDependecies(selectedNuGet.Name, nuGetVersion);
        }

        public void UpdateNuGets(string nugetName, string newNuGetVersion, IList<string> files)
        {
            foreach (var file in files)
            {
                var project = new Csproj(file);
                project.UpdateANuget(nugetName, newNuGetVersion);
            }
        }

        public bool IsHigherVersionAvailable(ProjectNuGet nuGet)
        {
            if (nuGet.Versions.Count > 1)
            {
                var higherVersion = nuGet.Versions.FirstOrDefault(item => !item.IsTheCurrentVersion);
                foreach (var version in nuGet.Versions)
                {
                    if (higherVersion != null && version.IsTheCurrentVersion && higherVersion.IsGreaterThan(version))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsTheSameVersion(ProjectNuGet nuGet, string packageVersion)
        {
            return !nuGet.Versions.Any(version =>
            {
                return packageVersion.Equals(version.NuGetVersion)
                       || (packageVersion.Contains(version.NuGetVersion) && packageVersion.LastOrDefault().Equals('0'));
            });
        }

        private void OrderNuGetVersions(ProjectNuGet nuGet)
        {
            nuGet.Versions.Select(item =>
            {
                var splitedVersion = item.NuGetVersion.Split('.');

                if (splitedVersion.Length == 4 && splitedVersion[splitedVersion.Length - 1] == "0")
                {
                    item.NuGetVersion = item.NuGetVersion.Remove(item.NuGetVersion.Length - 2);
                }

                return item;
            }).ToList();

            nuGet.Versions.ToList().Sort(new OrderingVersions());
        }
    }
}
