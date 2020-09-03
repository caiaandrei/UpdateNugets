using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Protocol.Core.Types;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace UpdateNugets.Core
{
    public class InterogateNuGetFeed
    {
        private string source = "https://api.nuget.org/v3/index.json";

        public InterogateNuGetFeed()
        {

        }

        public ObservableCollection<IPackageSearchMetadata> Packages { get; set; }

        public async Task SearchAsync(string text, int take)
        {
            Packages = new ObservableCollection<IPackageSearchMetadata>();
            var cancellationToken = new CancellationToken();

            var packageSource = new PackageSource(source);
            var _sourceRepository = new SourceRepository(packageSource, Repository.Provider.GetCoreV3());

            var rawPackageSearchResouce = await _sourceRepository.GetResourceAsync<PackageSearchResource>(cancellationToken);
            var searchFilter = new SearchFilter(true);

            var packages = await rawPackageSearchResouce.SearchAsync(text, searchFilter, 0, take, NullLogger.Instance, cancellationToken);

            Packages.AddRange(packages);
        }
    }
}
