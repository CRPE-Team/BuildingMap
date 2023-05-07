using BuildingMap.UI.Logic;
using BuildingMap.UI.Visual.Components.ViewModel;

namespace BuildingMap.UI.Visual.Pages.ViewModel
{
	public class MapViewModel : ObservableObject
	{
		private readonly MapFloorViewModel _currentFloor;
		private readonly MapManager _mapManager;
		private readonly SettingsManager _settingsManager;
		private readonly FloorSelectorViewModel _floorSelector;

		public MapViewModel(
			MapFloorViewModel currentFloor, 
			MapManager mapManager, 
			SettingsManager settingsManager,
			FloorSelectorViewModel floorSelector)
		{
			_currentFloor = currentFloor;
			_mapManager = mapManager;
			_settingsManager = settingsManager;
			_floorSelector = floorSelector;

			mapManager.MapLoaded += (_, _) => UpdateFloor();
			settingsManager.SelectedFloorChanged += (_, _) => UpdateFloor();
		}

		public MapFloorViewModel CurrentFloor => _currentFloor;

		public FloorSelectorViewModel FloorSelector => _floorSelector;

		private void UpdateFloor()
		{
			CurrentFloor.Update();
		}
	}
}
