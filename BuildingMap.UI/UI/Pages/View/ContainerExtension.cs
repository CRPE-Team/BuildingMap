using Unity;
using Unity.Extension;

namespace BuildingMap.UI.Pages.View
{
	public class ContainerExtension : UnityContainerExtension
	{
		protected override void Initialize()
		{
			Container.RegisterType<MapPageView>(TypeLifetime.Transient);
		}
	}
}
