using BuildingMap.Core;
using BuildingMap.UI.Logic;
using BuildingMap.UI.Visual.Components.ViewModel;

namespace BuildingMap.UI.Visual.Pages.ViewModel
{
	public class MapViewModel : ObservableObject
	{
		private readonly MapManager _mapManager;
		private readonly SettingsManager _settingsManager;

		private readonly MapFloorViewModel _currentFloor;
		private readonly MapFloorViewModel _backFloor;
		private readonly FloorSelectorViewModel _floorSelector;
		private readonly ToolBarViewModel _toolBar;

		public MapViewModel(
			MapFloorViewModel currentFloor,
			MapFloorViewModel backFloor,
			MapManager mapManager, 
			SettingsManager settingsManager,
			FloorSelectorViewModel floorSelector,
			ToolBarViewModel toolBar)
		{
			_currentFloor = currentFloor;
			_backFloor = backFloor;
			_mapManager = mapManager;
			_settingsManager = settingsManager;
			_floorSelector = floorSelector;
			_toolBar = toolBar;

			mapManager.MapLoaded += (_, _) => UpdateSelectedFloor();
			settingsManager.SelectedFloorChanged += (_, _) => UpdateSelectedFloor();
			settingsManager.BackFloorChanged += (_, _) => UpdateBackFloor();
			settingsManager.AllowEditChanged += (_, _) => UpdateBackFloor();
			_mapManager.MapMoved += (s, _) => _backFloor?.OnChanged();

			UpdateSelectedFloor();
		}

		public MapFloorViewModel CurrentFloor => _currentFloor;

		public MapFloorViewModel BackFloor => _backFloor;

		public FloorSelectorViewModel FloorSelector => _floorSelector;

		public ToolBarViewModel ToolBar => _toolBar;

		private void UpdateSelectedFloor()
		{
			CurrentFloor.Update(_mapManager.Map.GetFloorByNumber(_settingsManager.SelectedFloor));
			UpdateBackFloor();
		}

		private void UpdateBackFloor()
		{
			if (!_settingsManager.ShowBackFloor || _settingsManager.BackFloor == _settingsManager.SelectedFloor)
			{
				BackFloor.Update(new Floor());
				return;
			}

			BackFloor.Update(_mapManager.Map.GetFloorByNumber(_settingsManager.BackFloor));
		}
	}
}
