using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using BuildingMap.UI.Visual.Utils;

namespace BuildingMap.UI.Visual.Components.View
{
	public class BackgroundImage : BuildingGridItem
	{
		public static readonly DependencyProperty ShowProperty = DependencyPropertyEx.Register<bool, BackgroundImage>(OnShowChanged);
		public static readonly DependencyProperty ImageSourceProperty = DependencyPropertyEx.Register<ImageSource, BackgroundImage>(OnImageSourceChanged);
		public static readonly DependencyProperty ScaleProperty = DependencyPropertyEx.Register<double, BackgroundImage>(OnScaleChanged, ScaleCoerce, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault);
		
		private ImageBrush _image;

        private readonly ScaleTransform _scaleTransform;

		[Bindable(true)]
		public bool Show { get => (bool) GetValue(ShowProperty); set => SetValue(ShowProperty, value); }

		[Bindable(true)]
		public ImageSource ImageSource { get => (ImageSource) GetValue(ImageSourceProperty); set => SetValue(ImageSourceProperty, value); }

		[Bindable(true)]
		public double Scale { get => (double) GetValue(ScaleProperty); set => SetValue(ScaleProperty, value); }

        public bool CanDrag => Mouse.LeftButton == MouseButtonState.Pressed && Grid.AllowEdit;

		public BackgroundImage()
        {
            Background = _image = new ImageBrush();
            _image.Stretch = Stretch.Uniform;

            TransformGroup.Children.Add(_scaleTransform = new ScaleTransform());
			RenderTransformOrigin = new Point(0.5, 0.5);

			Show = false;
			Scale = 0.4;

			Width = Height = 1000;
			Margin = new Thickness(-1000);

			HorizontalAlignment = HorizontalAlignment.Center;
			VerticalAlignment = VerticalAlignment.Center;
		}

		public void Move(Vector offset)
		{
			Position += offset / Grid.RenderGridSize;
		}

		public override void Update()
		{
			base.Update();
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

		private static void OnImageSourceChanged(BackgroundImage d, DependencyPropertyChangedEventArgs e)
		{
			d.UpdateImage((ImageSource) e.NewValue);
		}

		private static object ScaleCoerce(BackgroundImage d, double scale)
		{
			return Math.Max(0.1, Math.Min(5, scale));
		}

		private static void OnScaleChanged(BackgroundImage d, DependencyPropertyChangedEventArgs e)
		{
			d._scaleTransform.ScaleX = d._scaleTransform.ScaleY = (double) e.NewValue;
			if (d.Grid != null) d.Update();
		}

		private void UpdateImage(ImageSource ImageSource)
        {
            _image.ImageSource = ImageSource;
        }
    }
}
