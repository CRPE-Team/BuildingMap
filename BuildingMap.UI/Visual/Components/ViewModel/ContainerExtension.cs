﻿using Unity;
using Unity.Extension;

namespace BuildingMap.UI.Visual.Components.ViewModel
{
	public class ContainerExtension : UnityContainerExtension
	{
		protected override void Initialize()
		{
			Container.RegisterType<FloorSelectorViewModel>(TypeLifetime.Transient);
			Container.RegisterInstance(() => Container.Resolve<FloorSelectorViewModel>());
			Container.RegisterType<FloorSelectorItemViewModel>();
		}
	}
}
