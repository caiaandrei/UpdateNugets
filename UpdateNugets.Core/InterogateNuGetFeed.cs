using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UpdateNugets.Core
{
    public class InterogateNuGetFeed
    {
        private string source = "https://pkgs.dev.azure.com/sdl/_packaging/SDLNuget/nuget/v3/index.json";

        public InterogateNuGetFeed()
        {

        }

        public ObservableCollection<IPackageSearchMetadata> Packages { get; set; }

        public async Task<IList<IPackageSearchMetadata>> SearchAsync(string text, int take)
        {
            Packages = new ObservableCollection<IPackageSearchMetadata>();
            var cancellationToken = new CancellationToken();

            var packageSource = new PackageSource(source);
            var sourceRepository = new SourceRepository(packageSource, Repository.Provider.GetCoreV3());

            var rawPackageSearchResouce = await sourceRepository.GetResourceAsync<PackageSearchResource>(cancellationToken);
            var searchFilter = new SearchFilter(false);

            var packages = await rawPackageSearchResouce.SearchAsync(text, searchFilter, 0, take, NullLogger.Instance, cancellationToken);

            Packages.AddRange(packages);
            return packages.ToList();
        }
    
        public async Task<IList<string>> GetDependecies(string packageId, string version)
        {
            ILogger logger = NullLogger.Instance;
            CancellationToken cancellationToken = CancellationToken.None;

            var cache = new SourceCacheContext();
            var packageSource = new PackageSource(source);
            var sourceRepository = new SourceRepository(packageSource, Repository.Provider.GetCoreV3());
            var resource = await sourceRepository.GetResourceAsync<FindPackageByIdResource>();

            var packageVersion = new NuGetVersion(version);
            using MemoryStream packageStream = new MemoryStream();

            await resource.CopyNupkgToStreamAsync(
                packageId,
                packageVersion,
                packageStream,
                cache,
                logger,
                cancellationToken);

            using PackageArchiveReader packageReader = new PackageArchiveReader(packageStream);
            NuspecReader nuspecReader = await packageReader.GetNuspecReaderAsync(cancellationToken);
            var packages = nuspecReader.GetDependencyGroups().SelectMany(item => item.Packages);
            return packages.Select(item => item.ToString()).ToList();
        }
    }
}
