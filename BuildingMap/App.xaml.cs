using System.Windows;
using Unity;

namespace BuildingMap
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
			Container.Resolve<UI.View.MainWindow>().Show();
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
