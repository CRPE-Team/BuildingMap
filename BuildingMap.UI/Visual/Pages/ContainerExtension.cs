using Unity;
using Unity.Extension;

namespace BuildingMap.UI.Visual.Pages
{
	public class ContainerExtension : UnityContainerExtension
	{
		protected override void Initialize()
		{
			Container.RegisterSingleton<PageNavigator>();

			Container.AddNewExtension<View.ContainerExtension>();
			Container.AddNewExtension<ViewModel.ContainerExtension>();
		}
	}
}
