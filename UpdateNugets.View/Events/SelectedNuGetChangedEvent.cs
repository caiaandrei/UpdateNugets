using Prism.Events;
using UpdateNugets.UI.ViewModel;

namespace UpdateNugets.UI.Events
{
    public class SelectedNuGetChangedEvent : PubSubEvent<NuGetDetailsViewModel>
    {
    }
}
