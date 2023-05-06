using System.Windows;
using Unity;

namespace BuildingMap.UI
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
    {
        public static IUnityContainer Container { get; private set; }

        public App()
        {
			SetupContainer();
		}

        protected override void OnStartup(StartupEventArgs e)
        {
			Container.Resolve<Visual.MainWindow>().Show();
        }

        private IUnityContainer SetupContainer()
        {
            Container = new UnityContainer();
            Container.RegisterInstance(this);

            Container.AddNewExtension<Visual.ContainerExtension>();
			Container.AddNewExtension<Logic.ContainerExtension>();

            return Container;
        }
    }
}
