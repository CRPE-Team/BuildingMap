using BuildingMap.UI.View;
using Unity;
using Unity.Extension;

namespace BuildingMap.UI
{
    public class ContainerExtension : UnityContainerExtension
    {
        protected override void Initialize()
		{
			Container.RegisterType<MainWindow>();

			Container.AddNewExtension<Pages.ContainerExtension>();
		}
    }
}
