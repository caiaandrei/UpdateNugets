using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpdateNugets.Core;
using UpdateNugets.UI.ViewModel;

namespace UpdateNugets.UI.Model
{
    public class NugetModel : ViewModelBase
    {
        private ProjectNuGet _projectNuGet;
        private List<VersionModel> _versions;
        private List<string> _dependencies;
        private VersionModel _currentSelectedVersion;

        public NugetModel(ProjectNuGet projectNuGet)
        {
            _projectNuGet = projectNuGet;
            Name = _projectNuGet.Name;
        }

        public string Name { get; set; }

        public List<VersionModel> Versions
        {
            get { return _versions; }
            set
            {
                _versions = value;
                OnPropertyChanged(nameof(Versions));
            }
        }

        public List<string> Dependencies
        {
            get { return _dependencies; }
            set
            {
                _dependencies = value;
                OnPropertyChanged(nameof(Dependencies));
            }
        }

        public bool IsHigherVersionAvailable { get; set; }

        public bool AreMultipleVersionUsed { get; set; }

        public VersionModel CurrentSelectedVersion 
        {
            get => _currentSelectedVersion;
            set
            {
                if (value is null)
                {
                    return;
                }

                _currentSelectedVersion = value;
                _projectNuGet.CurrentSelectedVersion = _projectNuGet.Versions.First(item => item.NuGetVersion == _currentSelectedVersion.CurrentVersion);
            }
        }

        public async Task LoadNuGetVersions()
        {
            var versions = await _projectNuGet.SearchNuGetVersions();
            var versionModels = new List<VersionModel>();

            foreach (var item in versions)
            {
                versionModels.Add(new VersionModel
                {
                    CurrentVersion = item.NuGetVersion,
                    IsUsed = item.IsTheCurrentVersion,
                    Files = item.Files.ToList()
                });
            }

            Versions = new List<VersionModel>(versionModels);

            AreMultipleVersionUsed = _projectNuGet.AreMultipleVersionUsed;
            IsHigherVersionAvailable = _projectNuGet.IsHigherVersionAvailable();
        }

        public async Task LoadNuGetDependencies()
        {
            var dependencies = await _projectNuGet.GetDependecies();
            Dependencies = new List<string>(dependencies);
        }

        public void UpdateNuGets()
        {
            _projectNuGet.UpdateNuGets();
        }
    }
}
