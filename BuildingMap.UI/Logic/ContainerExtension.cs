﻿using Unity;
using Unity.Extension;

namespace BuildingMap.UI.Logic
{
	public class ContainerExtension : UnityContainerExtension
	{
		protected override void Initialize()
		{
			Container.RegisterSingleton<SettingsManager>();
			Container.RegisterSingleton<MapManager>();
		}
	}
}
