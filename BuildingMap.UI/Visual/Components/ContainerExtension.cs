using Unity;
using Unity.Extension;

namespace BuildingMap.UI.Visual.Components
{
	public class ContainerExtension : UnityContainerExtension
	{
		protected override void Initialize()
		{
			Container.AddNewExtension<ViewModel.ContainerExtension>();
		}
	}
}
