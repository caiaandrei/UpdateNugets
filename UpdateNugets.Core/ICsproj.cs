using System.Collections.Generic;

namespace UpdateNugets.Core
{
    public interface ICsproj
    {
        IDictionary<string, string> GetAllNugets();
    }
}