using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using Unity;
using Unity.Microsoft.DependencyInjection;

namespace BuildingMap
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost AppHost { get; private set; }

        public static IUnityContainer Container { get; private set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .UseUnityServiceProvider(SetupContainer())
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost?.StartAsync();

            AppHost.Services.GetRequiredService<UI.View.MainWindow>().Show();

            base.OnStartup(e);
        }

        private IUnityContainer SetupContainer()
        {
            Container = new UnityContainer();
            Container.RegisterInstance(this);

            Container.AddNewExtension<UI.ContainerExtension>();

            return Container;
        }
    }
}
