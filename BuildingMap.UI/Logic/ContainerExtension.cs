using Unity;
using Unity.Extension;

namespace BuildingMap.Logic
{
	public class ContainerExtension : UnityContainerExtension
	{
		protected override void Initialize()
		{
			Container.RegisterSingleton<SettingsManager>();
		}
	}
}
