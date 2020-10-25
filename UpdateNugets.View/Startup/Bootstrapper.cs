using Autofac;
using Prism.Events;
using UpdateNugets.UI.Helpers;
using UpdateNugets.UI.View;
using UpdateNugets.UI.ViewModel;

namespace UpdateNugets.UI.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.RegisterType<UIService>().As<IUIServices>();

            builder.RegisterType<MainWindow>().AsSelf();

            builder.RegisterType<NuGetsListViewModel>().AsSelf();
            builder.RegisterType<SelectedNuGetDetailsViewModel>().AsSelf();
            builder.RegisterType<SelectedNuGetVersionFilesViewModel>().AsSelf();

            builder.RegisterType<MainViewModel>().AsSelf();

            return builder.Build();
        }
    }
}