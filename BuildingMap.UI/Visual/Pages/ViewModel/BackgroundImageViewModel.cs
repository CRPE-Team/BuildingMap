using System.Windows;
using Microsoft.Win32;

namespace BuildingMap.UI.Visual.Pages.ViewModel
{
	public class BackgroundImageViewModel : ObservableObject
	{
		private string _path;
		private Point _position;
		private double _scale = 0.4;
		private bool _show;

		public BackgroundImageViewModel()
		{
			SelectBackgroundImageCommand = new RelayCommand(SelectBackgroundImage);
			RemoveBackgroundImageCommand = new RelayCommand(RemoveBackgroundImage);
		}

		public RelayCommand SelectBackgroundImageCommand { get; }
		public RelayCommand RemoveBackgroundImageCommand { get; }

		public string Path
		{
			get => _path;
			set
			{
				_path = value;
				OnPropertyChanged();
			}
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

		public double Scale
		{
			get => _scale;
			set
			{
				_scale = value;
				OnPropertyChanged();
			}
		}

		public bool Show
		{
			get => _show;
			set
			{
				_show = value;
				OnPropertyChanged();
			}
		}

		private void SelectBackgroundImage()
		{
			var openFileDialog = new OpenFileDialog();
			var result = openFileDialog.ShowDialog();

			if (!result.Value) return;

			Path = openFileDialog.FileName;
		}

		private void RemoveBackgroundImage()
		{
			Path = null;
		}
	}
}
