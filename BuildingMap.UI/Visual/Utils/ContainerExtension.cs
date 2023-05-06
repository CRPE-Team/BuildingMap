using Unity;
using Unity.Extension;

namespace BuildingMap.UI.Visual.Utils
{
	public class ContainerExtension : UnityContainerExtension
	{
		protected override void Initialize()
		{
			Container.RegisterSingleton<ClipboardManager>();
		}
	}
}
