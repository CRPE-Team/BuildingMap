using Unity;
using Unity.Extension;

namespace BuildingMap.UI.Visual
{
	public class ContainerExtension : UnityContainerExtension
    {
        protected override void Initialize()
		{
			Container.RegisterType<MainWindow>(TypeLifetime.Transient);

			Container.AddNewExtension<Pages.ContainerExtension>();
			Container.AddNewExtension<Logics.ContainerExtension>();
			Container.AddNewExtension<Utils.ContainerExtension>();
		}
    }
}
