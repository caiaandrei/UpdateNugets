using Autofac;
using Prism.Events;
using System.Windows.Input;
using UpdateNugets.UI.Command;
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

            builder.RegisterType<BrowsePathHelper>().AsSelf();
            builder.RegisterType<BrowseProjectPathCommand>().As<IBrowseProjectPathCommand>();
            builder.RegisterType<BrowseWorkspacePathCommand>().As<IBrowseWorkspacePathCommand>();
            builder.RegisterType<CreateProjectCommand>().As<ICreateProjectCommand>();

            builder.RegisterType<NuGetsListViewModel>().AsSelf();
            builder.RegisterType<SelectedNuGetDetailsViewModel>().AsSelf();
            builder.RegisterType<SelectedNuGetVersionFilesViewModel>().AsSelf();
            builder.RegisterType<NewProjectViewModel>().AsSelf();
            builder.RegisterType<OpenProjectViewModel>().AsSelf();
            builder.RegisterType<ProjectSettingsViewModel>().AsSelf();
            builder.RegisterType<FinishProjectViewModel>().AsSelf();

            builder.RegisterType<MainViewModel>().AsSelf();

            return builder.Build();
        }
    }
}