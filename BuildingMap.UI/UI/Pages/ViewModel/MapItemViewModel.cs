using System.Windows;
using System.Windows.Media;
using BuildingMap.UI.Utils;

namespace BuildingMap.UI.Pages.ViewModel
{
	public class MapItemViewModel : ObservableObject
	{
		private readonly ClipboardManager _clipboardManager;

		private Point _position;
		private Size _size;
		private Color _color;
		private Color _selectedColor;
		private bool _isSelected;
		private double _radius;

		private string _text;
		private int _fontSize;
		private Color _foregroundColor;
		private RotationAngle _rotationAngle;

		private string _imageSource;
		private double _imageScale;

		public MapItemViewModel(ClipboardManager clipboardManager)
		{
			_clipboardManager = clipboardManager;
		}

		public Point Position
		{
			get => _position;
			set
			{
				_position = value;
				OnPropertyChanged();
			}
		}

		public Size Size
		{
			get => _size;
			set
			{
				if (value.Width == 0) value.Width = 1;
				if (value.Height == 0) value.Height = 1;

				_size = value;
				OnPropertyChanged();
			}
		}

		public Color Color
		{
			get => _color;
			set
			{
				_color = value;
				OnPropertyChanged();
			}
		}

		public Color SelectedColor
		{
			get => _selectedColor;
			set
			{
				_selectedColor = value;
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
			get => _radius;
			set
			{
				_radius = value;
				OnPropertyChanged();
			}
		}

		public string Text
		{
			get => _text;
			set
			{
				_text = value;
				OnPropertyChanged();
			}
		}

		public int FontSize
		{
			get => _fontSize;
			set
			{
				_fontSize = value;
				OnPropertyChanged();
			}
		}

		public Color ForegroundColor
		{
			get => _foregroundColor;
			set
			{
				_foregroundColor = value;
				OnPropertyChanged();
			}
		}

		public RotationAngle RotationAngle
		{
			get => _rotationAngle;
			set
			{
				_rotationAngle = value;
				OnPropertyChanged();
			}
		}

		public string ImageSource
		{
			get => _imageSource;
			set
			{
				_imageSource = value;
				OnPropertyChanged();
			}
		}

		public double ImageScale
		{
			get => _imageScale;
			set
			{
				_imageScale = value;
				OnPropertyChanged();
			}
		}

		public void CopyToClipboard()
		{
			_clipboardManager.SetData(this);
		}
	}
}
