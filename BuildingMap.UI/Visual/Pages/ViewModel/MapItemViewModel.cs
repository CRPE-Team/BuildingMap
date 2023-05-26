using System.Collections.ObjectModel;
using System.Windows;
using BuildingMap.Core;
using BuildingMap.UI.Logic;
using BuildingMap.UI.Visual.Components.ViewModel;
using BuildingMap.UI.Visual.Utils;
using Xceed.Wpf.Toolkit;

namespace BuildingMap.UI.Visual.Pages.ViewModel
{
	public class MapItemViewModel : ObservableObject
	{
		private readonly MapManager _mapManager;
		private readonly ClipboardManager _clipboardManager;

		private bool _isSelected;

		public MapItemViewModel(TemplateViewModel templateViewModel, MapManager mapManager, ClipboardManager clipboardManager)
		{
			_mapManager = mapManager;
			_clipboardManager = clipboardManager;

			TemplateViewModel = templateViewModel;

			CopyCommand = new RelayCommand(CopyToClipboard);
		}

		public RelayCommand CopyCommand { get; set; }

		public TemplateViewModel TemplateViewModel { get; set; }

		public MapItem MapItem { get; private set; }

		public ObservableCollection<ColorItem> AvailableColors { get; } = new();

		public Point Position
		{
			get => MapItem.Position.ToPoint();
			set
			{
				MapItem.Position = value.ToNumerics();
				OnPropertyChanged();
			}
		}

		public Size Size
		{
			get => MapItem.Size.ToSize();
			set
			{
				if (value.Width == 0) value.Width = 1;
				if (value.Height == 0) value.Height = 1;

				MapItem.Size = value.ToNumerics();
				OnPropertyChanged();
			}
		}

		public bool IsSelected
		{
			get => _isSelected;
			set
			{
				_isSelected = value;
				if (value) TemplateViewModel.Select();

				OnPropertyChanged();
			}
		}

		public string Text
		{
			get => MapItem.Text;
			set
			{
				MapItem.Text = value;
				OnPropertyChanged();
			}
		}

		public RotationAngle RotationAngle
		{
			get => MapItem.RotationAngle;
			set
			{
				MapItem.RotationAngle = value;
				OnPropertyChanged();
			}
		}

		public void Initialize(MapItem mapItem)
		{
			MapItem = mapItem;
			TemplateViewModel.Initialize(_mapManager.Map.Styles[mapItem.StyleId]);
		}

		public void CopyToClipboard()
		{
			_clipboardManager.SetData(MapItem);
		}
	}
}
