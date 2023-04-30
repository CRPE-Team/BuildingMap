using Unity;
using Unity.Extension;

namespace BuildingMap.UI.Pages.ViewModel
{
	public class ContainerExtension : UnityContainerExtension
	{
		protected override void Initialize()
		{
			Container.RegisterType<MapPageViewModel>();
		}
	}
}
