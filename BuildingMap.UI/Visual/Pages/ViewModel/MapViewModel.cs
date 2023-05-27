using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BuildingMap.Core;
using BuildingMap.UI.Logic;
using BuildingMap.UI.Visual.Components.View.Core.Utils;
using BuildingMap.UI.Visual.Components.ViewModel;
using BuildingMap.UI.Visual.Utils;

namespace BuildingMap.UI.Visual.Pages.ViewModel
{
	public class MapViewModel : ObservableObject
	{
		private readonly MapManager _mapManager;
		private readonly SettingsManager _settingsManager;


		public MapViewModel(
			MapManager mapManager, 
			SettingsManager settingsManager,
			MapEditModeViewModel editModeViewModel,
			MapFloorViewModel currentFloor,
			MapFloorViewModel backFloor,
			FloorSelectorViewModel floorSelector,
			ToolBarViewModel toolBar)
		{
			_mapManager = mapManager;
			_settingsManager = settingsManager;

			MapEditModeViewModel = editModeViewModel;
			CurrentFloor = currentFloor;
			BackFloor = backFloor;
			FloorSelector = floorSelector;
			ToolBar = toolBar;

			mapManager.MapLoaded += (_, _) => UpdateSelectedFloor();
			settingsManager.SelectedFloorChanged += (_, _) => UpdateSelectedFloor();
			settingsManager.BackFloorChanged += (_, _) => UpdateBackFloor();
			settingsManager.AllowEditChanged += (_, _) => UpdateBackFloor();
			_mapManager.MapMoved += (s, _) => BackFloor?.OnChanged();

			UpdateSelectedFloor();
		}

		public MapEditModeViewModel MapEditModeViewModel { get; }

		public MapFloorViewModel CurrentFloor { get; }

		public MapFloorViewModel BackFloor { get; }

		public FloorSelectorViewModel FloorSelector { get; }

		public ToolBarViewModel ToolBar { get; }

		public int GridSize
		{
			get => _mapManager.Map.GridSize;
			set
			{
				_mapManager.Map.GridSize = value;
				OnPropertyChanged();
				_mapManager.MapUpdated(this);
			}
		}

		public Vector Offset
		{
			get => _mapManager.Map.Offest.ToVector();
			set
			{
				_mapManager.Map.Offest = value.ToNumerics();
				OnPropertyChanged();
				_mapManager.MapUpdated(this);
			}
		}

		public double Zoom
		{
			get => _mapManager.Map.Zoom;
			set
			{
				_mapManager.Map.Zoom = value;
				OnPropertyChanged();
				_mapManager.MapUpdated(this);
			}
		}

		public Color Background
		{
			get => _mapManager.Map.BackgroundColor.ToWindows();
			set
			{
				_mapManager.Map.BackgroundColor = value.ToDrawing();
				OnPropertyChanged();
			}
		}
	
		public void OnMouseUp(object sender, MouseButtonEventArgs args)
		{
			if (DragManager.WasMoving) return;
			if (!DataContextHelper.TryGetDataContext(Mouse.DirectlyOver, out var dataContext) || dataContext != this) return;
			
			if (args.ChangedButton == MouseButton.Left) CurrentFloor.Unselect();
		}

		public void OnContextMenuOpening(object sender, ContextMenuEventArgs args)
		{
			args.Handled = !MapEditModeViewModel.AllowEdit;
		}

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
