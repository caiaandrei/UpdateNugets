using System.Collections.Generic;

namespace UpdateNugets.Core
{
    public class Version
    {
        public string NuGetVersion { get; set; }
        public IList<string> Files { get; set; }
    }
}