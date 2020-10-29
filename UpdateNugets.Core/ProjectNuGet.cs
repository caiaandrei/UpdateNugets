using NuGet.Packaging;
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
            set => _currentSelectedVersion = value;
        }

        public string InitialNuGetVersion { get; }

        public bool AreMultipleVersionUsed => Versions.Count(item => item.IsTheCurrentVersion) > 1;

        public override string ToString()
        {
            return Name;
        }

        public async Task<IList<Version>> SearchNuGetVersions()
        {
            var foundPackages = await _interogateNuGetFeed.SearchAsync(Name, 20);

            if (foundPackages.Count == 0)
            {
                return Versions;
            }

            foreach (var package in foundPackages)
            {
                if (package.Title.Equals(Name) || package.Identity.Id.Equals(Name))
                {
                    var allVersions = await package.GetVersionsAsync();
                    List<Version> versions = new List<Version>();

                    foreach (var item in allVersions)
                    {
                        var packageVersion = item.Version.Version.ToString();
                        if (IsTheSameVersion(packageVersion))
                        {
                            Versions.Add(new Version
                            {
                                NuGetVersion = packageVersion
                            });
                        }
                    }
                    break;
                }
            }

            OrderNuGetVersions();

            return Versions;
        }

        public async Task<IList<string>> GetDependecies()
        {
            return await _interogateNuGetFeed.GetDependecies(Name, CurrentSelectedVersion.NuGetVersion);
        }

        public void UpdateNuGets()
        {
            foreach (var file in CurrentVersion.Files)
            {
                var project = new Csproj(file);
                project.UpdateANuget(Name, CurrentSelectedVersion.NuGetVersion);
            }

            CurrentSelectedVersion.Files.AddRange(CurrentVersion.Files);
            CurrentSelectedVersion.IsTheCurrentVersion = true;

            CurrentVersion.IsTheCurrentVersion = false;
            CurrentVersion.Files.Clear();

            CurrentVersion = CurrentSelectedVersion;

        }

        public bool IsHigherVersionAvailable()
        {
            if (Versions.Count > 1)
            {
                var higherVersion = Versions.FirstOrDefault(item => !item.IsTheCurrentVersion);
                foreach (var version in Versions)
                {
                    if (higherVersion != null && version.IsTheCurrentVersion && higherVersion.IsGreaterThan(version))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsTheSameVersion(string packageVersion)
        {
            return !Versions.Any(version =>
            {
                return packageVersion.Equals(version.NuGetVersion)
                       || (packageVersion.Contains(version.NuGetVersion) && packageVersion.LastOrDefault().Equals('0'));
            });
        }

        private void OrderNuGetVersions()
        {
            Versions.Select(item =>
            {
                var splitedVersion = item.NuGetVersion.Split('.');

                if (splitedVersion.Length == 4 && splitedVersion[splitedVersion.Length - 1] == "0")
                {
                    item.NuGetVersion = item.NuGetVersion.Remove(item.NuGetVersion.Length - 2);
                }

                return item;
            }).ToList();

            Versions.ToList().Sort(new OrderingVersions());
        }
    }
}
