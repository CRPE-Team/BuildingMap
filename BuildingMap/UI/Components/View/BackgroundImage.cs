using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BuildingMap.UI.Utils;

namespace BuildingMap.UI.Components.View
{
    public class BackgroundImage : BuildingGridItem
	{
		public static readonly DependencyProperty ShowProperty = DependencyPropertyEx.Register<bool, BackgroundImage>(OnShowChanged);
		public static readonly DependencyProperty ImageSourcePathProperty = DependencyPropertyEx.Register<string, BackgroundImage>(OnImageSourcePathChanged);
		public static readonly DependencyProperty ScaleProperty = DependencyPropertyEx.Register<double, BackgroundImage>(OnScaleChanged, ScaleCoerce, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault);

		private ImageBrush _image;

        private readonly ScaleTransform _scaleTransform;

		public bool Show { get => (bool) GetValue(ShowProperty); set => SetValue(ShowProperty, value); }

		public string ImageSourcePath { get => (string) GetValue(ImageSourcePathProperty); set => SetValue(ImageSourcePathProperty, value); }

        public double Scale { get => (double) GetValue(ScaleProperty); set => SetValue(ScaleProperty, value); }

        public bool CanDrag => Grid.AllowEdit;

		public BackgroundImage()
        {
            Background = _image = new ImageBrush();
            _image.Stretch = Stretch.Uniform;

            TransformGroup.Children.Add(_scaleTransform = new ScaleTransform());
			RenderTransformOrigin = new Point(0.5, 0.5);

			Show = false;
			Scale = 0.2;
		}

		public void Move(Vector offset)
		{
			Position += offset / Grid.RenderGridSize;
		}

		protected override void UpdateOffset(Vector _)
        {
            var position = (Position.ToVector() + (Grid.RenderOffset / 2).FloorInt() * 2 - Grid.RenderOffset + Grid.Offset) * Grid.GridSize;
            base.UpdateOffset(position / Scale);
        }

		private static void OnShowChanged(BackgroundImage d, DependencyPropertyChangedEventArgs e)
		{
			d.Visibility = (bool) e.NewValue ? Visibility.Visible : Visibility.Hidden;
		}

		private static void OnImageSourcePathChanged(BackgroundImage d, DependencyPropertyChangedEventArgs e)
		{
			d.UpdateImage((string) e.NewValue);
		}

		private static object ScaleCoerce(BackgroundImage d, double scale)
		{
			return Math.Max(0.01, Math.Min(3, scale));
		}

		private static void OnScaleChanged(BackgroundImage d, DependencyPropertyChangedEventArgs e)
		{
			d._scaleTransform.ScaleX = d._scaleTransform.ScaleY = (double) e.NewValue;
			if (d.Grid != null) d.Update();
		}

		private void UpdateImage(string path)
        {
			if (path == null) return;

            if (!File.Exists(path))
            {
				ImageSourcePath = null;
                _image.ImageSource = null;
                return;
            }

            _image.ImageSource = new BitmapImage(new Uri(ImageSourcePath, UriKind.RelativeOrAbsolute));
        }
    }
}
