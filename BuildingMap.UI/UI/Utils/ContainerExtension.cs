using Unity;
using Unity.Extension;

namespace BuildingMap.UI.Utils
{
	public class ContainerExtension : UnityContainerExtension
	{
		protected override void Initialize()
		{
			Container.RegisterSingleton<ClipboardManager>();
		}
	}
}
