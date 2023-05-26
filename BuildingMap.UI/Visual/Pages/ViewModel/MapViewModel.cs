using BuildingMap.UI.Logic;
using BuildingMap.UI.Visual.Components.ViewModel;

namespace BuildingMap.UI.Visual.Pages.ViewModel
{
	public class MapViewModel : ObservableObject
	{
		private readonly MapManager _mapManager;
		private readonly SettingsManager _settingsManager;

		private readonly MapFloorViewModel _currentFloor;
		private readonly FloorSelectorViewModel _floorSelector;
		private readonly ToolBarViewModel _toolBar;

		public MapViewModel(
			MapFloorViewModel currentFloor,
			MapManager mapManager, 
			SettingsManager settingsManager,
			FloorSelectorViewModel floorSelector,
			ToolBarViewModel toolBar)
		{
			_currentFloor = currentFloor;
			_mapManager = mapManager;
			_settingsManager = settingsManager;
			_floorSelector = floorSelector;
			_toolBar = toolBar;

			mapManager.MapLoaded += (_, _) => UpdateFloor();
			settingsManager.SelectedFloorChanged += (_, _) => UpdateFloor();
		}

		public MapFloorViewModel CurrentFloor => _currentFloor;

		public FloorSelectorViewModel FloorSelector => _floorSelector;

		public ToolBarViewModel ToolBar => _toolBar;

		private void UpdateFloor()
		{
			CurrentFloor.Update();
		}
	}
}
