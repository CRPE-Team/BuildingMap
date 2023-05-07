using System;
using System.Collections.ObjectModel;
using BuildingMap.UI.Logic;

namespace BuildingMap.UI.Visual.Components.ViewModel
{
	public class FloorSelectorViewModel : ObservableObject
	{
		private readonly MapManager _mapManager;
		private readonly SettingsManager _settingsManager;
		private readonly Func<FloorSelectorItemViewModel> _floorSelectorItemFactory;

		public FloorSelectorViewModel(
			MapManager mapManager, 
			SettingsManager settingsManager,
			Func<FloorSelectorItemViewModel> floorSelectorItemFactory)
		{
			_mapManager = mapManager;
			_settingsManager = settingsManager;
			_floorSelectorItemFactory = floorSelectorItemFactory;

			_mapManager.FloorsChanged += (_, _) => Update();
			_settingsManager.SelectedFloorChanged += (_, _) => UpdateSelectedFloor();

			Update();
			UpdateSelectedFloor();
		}

		public ObservableCollection<FloorSelectorItemViewModel> Floors { get; } = new ObservableCollection<FloorSelectorItemViewModel>();

		private void Update()
		{
			Floors.Clear();

			foreach (var floor in _mapManager.Map.Floors)
			{
				var instance = _floorSelectorItemFactory();
				instance.Number = floor.Number;
				instance.IsSelected = floor.Number == _settingsManager.SelectedFloor;

				Floors.Add(instance);
			}
		}

		private void UpdateSelectedFloor()
		{
			foreach (var floor in Floors)
			{
				floor.IsSelected = floor.Number == _settingsManager.SelectedFloor;
			}
		}	
	}
}
