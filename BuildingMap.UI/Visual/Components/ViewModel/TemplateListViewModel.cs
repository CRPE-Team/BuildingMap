using System;
using System.Collections.ObjectModel;
using BuildingMap.Core;
using BuildingMap.UI.Logic;

namespace BuildingMap.UI.Visual.Components.ViewModel
{
	public class TemplateListViewModel : ObservableObject
	{
		private readonly MapManager _mapManager;
		private readonly SettingsManager _settingsManager;
		private readonly Func<TemplateViewModel> _templateViewModelFactory;

		public TemplateListViewModel(
			MapManager mapManager,
			SettingsManager settingsManager,
			Func<TemplateViewModel> templateViewModelFactory)
		{
			_mapManager = mapManager;
			_settingsManager = settingsManager;
			_templateViewModelFactory = templateViewModelFactory;

			_mapManager.MapLoaded += (_, _) => Load();
			Load();
		}

		public ObservableCollection<TemplateViewModel> Templates { get; } = new ObservableCollection<TemplateViewModel>();

		private void Load()
		{
			Templates.Clear();

			foreach (var style in _settingsManager.DisplayStyles)
			{
				Templates.Add(CreateTemplateByStyle(style));
			}
		}

		private TemplateViewModel CreateTemplateByStyle(ItemStyle style)
		{
			var template = _templateViewModelFactory();
			template.Initialize(style);

			return template;
		}
	}
}
