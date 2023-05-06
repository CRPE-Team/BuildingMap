using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using BuildingMap.Core;
using BuildingMap.UI.Logic;
using BuildingMap.UI.Visual.Components.View.Core.Utils;
using BuildingMap.UI.Visual.Logics;
using BuildingMap.UI.Visual.Utils;

namespace BuildingMap.UI.Visual.Pages.ViewModel
{
	public class MapFloorViewModel : ObservableObject
	{
		private MapItemViewModel _drawingItem;

		private int _gridSize = 5;
		private Vector _offset;
		private Color _background;
		private double _zoom = 1;

		private Floor _floor;

		private readonly MapManager _mapManager;
		private readonly SettingsManager _settingsManager;
		private readonly MapItemsFactory _mapItemsFactory;

		public MapFloorViewModel(
			MapManager mapManager,
			SettingsManager settingsManager,
			MapItemsFactory mapItemsFactory,
			MapEditModeViewModel editModeViewModel)
		{
			_mapManager = mapManager;
			_settingsManager = settingsManager;
			_mapItemsFactory = mapItemsFactory;

			MapEditModeViewModel = editModeViewModel;

			Update();
		}

		public MapEditModeViewModel MapEditModeViewModel { get; }

		public ObservableCollection<MapItemViewModel> Items { get; } = new ObservableCollection<MapItemViewModel>();

		public MapItemViewModel SelectedItem { get; private set; }

		public int GridSize
		{
			get => _gridSize;
			set
			{
				_gridSize = value;
				OnPropertyChanged();
			}
		}

		public Vector Offset
		{
			get => _offset;
			set
			{
				_offset = value;
				OnPropertyChanged();
			}
		}

		public Color Background
		{
			get => _background;
			set
			{
				_background = value;
				OnPropertyChanged();
			}
		}

		public double Zoom
		{
			get => _zoom;
			set
			{
				_zoom = value;
				OnPropertyChanged();
			}
		}

		public Vector MousePosition { get; set; }

		public void SelectItem(MapItemViewModel item)
		{
			if (SelectedItem != null)
			{
				SelectedItem.IsSelected = false;
			}

			item.IsSelected = true;
			SelectedItem = item;
		}

		public void OnClickItem(object sender, MouseButtonEventArgs args)
		{
			if (!DataContextHelper.TryGetDataContext<MapItemViewModel>(sender, out var item)) return;
			if (DragManager.Moving) return;

			SelectItem(item);
		}

		public void OnStartDraw(object sender, StartDrawEventArgs args)
		{
			var viewModel = _mapItemsFactory.CreateNew();
			args.CreatedObject = viewModel;

			AddNewItem(viewModel);

			_drawingItem = viewModel;
		}

		public void OnStopDraw()
		{
			if (_drawingItem.Size.IsZeroSize())
			{
				RemoveItem(_drawingItem);
			}

			_drawingItem = null;
		}

		public void CopyElement()
		{
			SelectedItem?.CopyToClipboard();
		}

		public void InsertElement()
		{	
			if (!_mapItemsFactory.TryCreateCopyFromClipboard(out var copy)) return;

			copy.Position = (MousePosition - copy.Size.ToVector() / 2).Floor().ToPoint();

			AddNewItem(copy);
		}

		public void DeleteElement()
		{
			if (SelectedItem == null) return;

			RemoveItem(SelectedItem);
			SelectedItem = null;
		}

		public void Update()
		{
			Items.Clear();

			_floor = _mapManager.Map.Floors[_settingsManager.SelectedFloor];
			foreach (var mapItem in _floor.MapItems.Values)
			{
				var mapItemViewModel = _mapItemsFactory.Create(mapItem);
				Items.Add(mapItemViewModel);
			}
		}

		private void AddNewItem(MapItemViewModel viewModel)
		{
			Items.Add(viewModel);
			_floor.MapItems.Add(viewModel.MapItem.Id, viewModel.MapItem);
		}

		private void RemoveItem(MapItemViewModel viewModel)
		{
			Items.Remove(viewModel);
			_floor.MapItems.Remove(viewModel.MapItem.Id);
		}
	}
}