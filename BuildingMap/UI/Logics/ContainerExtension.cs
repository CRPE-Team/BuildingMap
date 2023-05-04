﻿using BuildingMap.UI.Pages.ViewModel;
using Unity;
using Unity.Extension;

namespace BuildingMap.UI.Logics
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