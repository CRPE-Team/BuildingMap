using System.Windows;
using System.Windows.Media;
using BuildingMap.Core;
using BuildingMap.UI.Visual.Utils;

namespace BuildingMap.UI.Visual.Pages.ViewModel
{
	public class MapItemViewModel : ObservableObject
	{
		private readonly ClipboardManager _clipboardManager;

		private bool _isSelected;

		public MapItemViewModel(ClipboardManager clipboardManager)
		{
			_clipboardManager = clipboardManager;
		}

		public MapItem MapItem { get; private set; }

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

		public Color Color
		{
			get => MapItem.Color.ToWindows();
			set
			{
				MapItem.Color = value.ToDrawing();
				OnPropertyChanged();
			}
		}

		public Color SelectedColor
		{
			get => MapItem.SelectedColor.ToWindows();
			set
			{
				MapItem.SelectedColor = value.ToDrawing();
				OnPropertyChanged();
			}
		}

		public bool IsSelected
		{
			get => _isSelected;
			set
			{
				_isSelected = value;
				OnPropertyChanged();
			}
		}

		public double Radius
		{
			get => MapItem.Radius;
			set
			{
				MapItem.Radius = value;
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

		public int FontSize
		{
			get => MapItem.FontSize;
			set
			{
				MapItem.FontSize = value;
				OnPropertyChanged();
			}
		}

		public Color ForegroundColor
		{
			get => MapItem.ForegroundColor.ToWindows();
			set
			{
				MapItem.ForegroundColor = value.ToDrawing();
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

		public string ImageSource
		{
			get => MapItem.ImageSource;
			set
			{
				MapItem.ImageSource = value;
				OnPropertyChanged();
			}
		}

		public double ImageScale
		{
			get => MapItem.ImageScale;
			set
			{
				MapItem.ImageScale = value;
				OnPropertyChanged();
			}
		}

		public void Initialize(MapItem mapItem)
		{
			MapItem = mapItem;
		}

		public void CopyToClipboard()
		{
			_clipboardManager.SetData(MapItem);
		}
	}
}
