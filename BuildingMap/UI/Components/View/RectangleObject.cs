using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using BuildingMap.UI.Utils;

namespace BuildingMap.UI.Components.View
{
	public partial class RectangleObject : BuildingGridItem
	{
		public static readonly DependencyProperty SizeProperty = DependencyPropertyEx.Register<Size, RectangleObject>(OnSizeChanged, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault);
		public static readonly DependencyProperty ColorProperty = DependencyPropertyEx.Register<Color, RectangleObject>(OnViewChanged);
		public static readonly DependencyProperty SelectedColorProperty = DependencyPropertyEx.Register<Color, RectangleObject>(OnViewChanged);
		public static readonly DependencyProperty SelectedProperty = DependencyPropertyEx.Register<bool, RectangleObject>(OnViewChanged);
		public static readonly DependencyProperty RadiusProperty = DependencyPropertyEx.Register<double, RectangleObject>(OnRadiusChanged);

		public const int AuraSize = 4;

        private readonly Rectangle _rectangle;

		[Bindable(true)]
		public Size Size { get => (Size) GetValue(SizeProperty); set => SetValue(SizeProperty, value); }

		[Bindable(true)]
		public Color Color { get => (Color) GetValue(ColorProperty); set => SetValue(ColorProperty, value); }

		[Bindable(true)]
		public Color SelectedColor { get => (Color) GetValue(SelectedColorProperty); set => SetValue(SelectedColorProperty, value); }

		[Bindable(true)]
		public bool Selected { get => (bool) GetValue(SelectedProperty); set => SetValue(SelectedProperty, value); }

		[Bindable(true)]
		public double Radius { get => (double) GetValue(RadiusProperty); set => SetValue(RadiusProperty, value); }

		public RectangleObject()
        {
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;

            Background = new SolidColorBrush(new Color());

            Children.Add(_rectangle = new Rectangle());
            _rectangle.Margin = new Thickness(AuraSize);

			Width = Height = 0;
        }

        public override void Update()
        {
            base.Update();
            UpdateSize(Size);
        }

        protected override void UpdateOffset(Vector position)
        {
            base.UpdateOffset(position - new Vector(AuraSize, AuraSize));
        }

		private static void OnSizeChanged(RectangleObject d, DependencyPropertyChangedEventArgs e)
		{
			d.UpdateSize((Size) e.NewValue);
		}

		private static void OnViewChanged(RectangleObject d, DependencyPropertyChangedEventArgs e)
		{
			d.SetSelect(d.Selected);
		}

		private static void OnRadiusChanged(RectangleObject d, DependencyPropertyChangedEventArgs e)
		{
			d._rectangle.RadiusX = d._rectangle.RadiusY = (double) e.NewValue;
		}

		private void SetSelect(bool selected)
		{
			if (selected)
			{
				_rectangle.Stroke = new SolidColorBrush(Color);
				_rectangle.Fill = new SolidColorBrush(SelectedColor);
				_rectangle.StrokeThickness = 3;
			}
			else
			{
				_rectangle.Stroke = null;
				_rectangle.StrokeThickness = 0;
				_rectangle.Fill = new SolidColorBrush(Color);
			}
		}

		private void UpdateSize(Size size)
        {
			if (Grid == null) return;

            var sizeVector = (size.ToVector() + new Vector(0.001, 0.001)).Round() * Grid.GridSize;

            Width = (_rectangle.Width = sizeVector.X) + AuraSize * 2;
            Height = (_rectangle.Height = sizeVector.Y) + AuraSize * 2;
        }
    }
}
