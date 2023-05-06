using Unity;
using Unity.Extension;

namespace BuildingMap.UI.Visual.Pages.ViewModel
{
	public class ContainerExtension : UnityContainerExtension
	{
		protected override void Initialize()
		{
			Container.RegisterType<MapViewModel>(TypeLifetime.Transient);
			Container.RegisterInstance(() => Container.Resolve<MapFloorViewModel>());
			Container.RegisterType<MapFloorViewModel>(TypeLifetime.Transient);
			Container.RegisterType<MapEditModeViewModel>(TypeLifetime.Transient);
			Container.RegisterType<BackgroundImageViewModel>(TypeLifetime.Transient);
		}
	}
}
