using System.Collections.Generic;

namespace UpdateNugets.Core
{
    public class NuGet
    {
        public NuGet(string name, IList<Version> versions)
        {
            Name = name;
            Versions = versions;
        }

        public string Name { get; }
        public IList<Version> Versions { get; }
    }
}
