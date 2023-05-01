using System.Windows;

namespace BuildingMap.UI.Pages.ViewModel
{
	public class BackgroundImageViewModel : ObservableObject
	{
		private string _path = @"C:\Users\Lashov\source\repos\BuildingMap\BuildingMap\UI\Pages\Izuku-Midoriya-Изуку-Мидория-Моя-Геройская-Академия-арты-картинки-12.jpg";
		private Point _position;
		private double _scale = 0.2;
		private bool _show;

		public BackgroundImageViewModel()
		{

		}

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
	}
}
