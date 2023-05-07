using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using BuildingMap.Core;
using BuildingMap.UI.Logic;
using BuildingMap.UI.Visual.Utils;
using Microsoft.Win32;

namespace BuildingMap.UI.Visual.Pages.ViewModel
{
	public class MapItemViewModel : ObservableObject
	{
		private readonly MapManager _mapManager;
		private readonly ClipboardManager _clipboardManager;

		private bool _isSelected;

		public MapItemViewModel(MapManager mapManager, ClipboardManager clipboardManager)
		{
			_mapManager = mapManager;
			_clipboardManager = clipboardManager;

			AddImageCommand = new RelayCommand(AddImage);
			RemoveImageCommand = new RelayCommand(RemoveImage);
		}

		public RelayCommand AddImageCommand { get; set; }
		public RelayCommand RemoveImageCommand { get; set; }

		public MapItem MapItem { get; private set; }

		public bool HasImage => ImageData != null;

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

		public byte[] ImageData
		{
			get
			{
				return MapItem.ImageId == null ? null : _mapManager.Map.ImagesData.GetValueOrDefault(MapItem.ImageId);
			}
			set
			{
				if (value == null)
				{
					if (MapItem.ImageId != null)
					{
						_mapManager.Map.RemoveImage(MapItem.ImageId);
						MapItem.ImageId = null;
					}	
				}
				else
				{
					MapItem.ImageId = _mapManager.Map.AddNewImage(value);
				}
				
				OnPropertyChanged();
				OnPropertyChanged(nameof(HasImage));
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

		private void AddImage()
		{
			var openFileDialog = new OpenFileDialog();
			var result = openFileDialog.ShowDialog();

			if (!result.Value) return;

			ImageData = File.ReadAllBytes(openFileDialog.FileName);
		}

		private void RemoveImage()
		{
			ImageData = null;
		}
	}
}
