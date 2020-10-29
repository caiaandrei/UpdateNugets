using System.Collections.ObjectModel;

namespace UpdateNugets.Core
{
    public class Version
    {
        public string NuGetVersion { get; set; }

        public ObservableCollection<string> Files { get; set; } = new ObservableCollection<string>();

        public bool IsTheCurrentVersion { get; set; }

        public bool IsGreaterThan(Version version)
        {
            return new OrderingVersions().Compare(this, version) > 0;
        }
    }
}