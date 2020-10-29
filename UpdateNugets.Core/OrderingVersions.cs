using System.Collections.Generic;
using System.Linq;

namespace UpdateNugets.Core
{
    public class OrderingVersions : IComparer<Version>
    {
        public int Compare(Version version1, Version version2)
        {
            var splitedVersion1 = version1.NuGetVersion.Split('.').Select(item => int.Parse(item)).ToArray();
            var splitedVersion2 = version2.NuGetVersion.Split('.').Select(item => int.Parse(item)).ToArray();

            if (splitedVersion1[0] == splitedVersion2[0])
            {
                if (splitedVersion1[1] == splitedVersion2[1])
                {
                    if (splitedVersion1[2] > splitedVersion2[2])
                    {
                        return 1;
                    }
                    else if (splitedVersion1[2] < splitedVersion2[2])
                    {
                        return -1;
                    }

                    return 0;
                }
                else if (splitedVersion1[1] > splitedVersion2[1])
                {
                    return 1;
                }

                return -1;
            }
            else if (splitedVersion1[0] > splitedVersion2[0])
            {
                return 1;
            }

            return -1;
        }
    }
}
