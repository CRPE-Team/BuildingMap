using BuildingMap.UI.Visual.Pages.ViewModel;
using Unity;
using Unity.Extension;

namespace BuildingMap.UI.Visual.Logics
{
	public class ContainerExtension : UnityContainerExtension
	{
		protected override void Initialize()
		{
			Container.RegisterSingleton<MapItemsFactory>();
			Container.RegisterInstance(() => Container.Resolve<MapItemViewModel>());
		}
	}
}
