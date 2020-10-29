using Prism.Events;
using UpdateNugets.Core;

namespace UpdateNugets.UI.Events
{
    public class NuGetUpdated : PubSubEvent<Version>
    {
    }
}
