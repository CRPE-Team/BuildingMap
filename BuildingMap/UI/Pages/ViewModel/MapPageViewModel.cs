using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using BuildingMap.UI.Components.View.Core.Utils;
using BuildingMap.UI.Logics;
using BuildingMap.UI.Utils;

namespace BuildingMap.UI.Pages.ViewModel
{
	public class MapPageViewModel : ObservableObject
	{
		private int _gridSize = 5;
		private Vector _offset;
		private Color _background;
		private double _zoom = 1;

		private readonly MapItemsFactory _mapItemsFactory;

		public MapPageViewModel(
			MapItemsFactory mapItemsFactory,
			MapEditModeViewModel editModeViewModel)
		{
			_mapItemsFactory = mapItemsFactory;

			MapEditModeViewModel = editModeViewModel;
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
			var newItem = _mapItemsFactory.CreateNew();
			args.CreatedObject = newItem;
			Items.Add(newItem);
		}

		public void CopyElement()
		{
			SelectedItem.CopyToClipboard();
		}

		public void InsertElement()
		{	
			if (!_mapItemsFactory.TryCreateCopyFromClipboard(out var copy)) return;

			copy.Position = (MousePosition - copy.Size.ToVector() / 2).Floor().ToPoint();

			Items.Add(copy);
		}

		public void DeleteElement()
		{
			if (SelectedItem == null) return;

			Items.Remove(SelectedItem);
			SelectedItem = null;
		}
	}
}